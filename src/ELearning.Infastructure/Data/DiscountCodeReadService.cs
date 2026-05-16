using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using ELearning.Application.Abstractions.Data;
using ELearning.Application.Discounts.DTOs;
using ELearning.Domain.Discounts;

namespace ELearning.Infastructure.Data;
public sealed class DiscountCodeReadService(
    IDbConnectionFactory dbConnectionFactory) : IDiscountCodeReadService
{
    private readonly IDbConnectionFactory _dbConnectionFactory = dbConnectionFactory;

    public async Task<DiscountCodeResponseWithTargets?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var sql = """
            Select
            dc.id AS Id,
            dc.code AS Code,
            dc.discount_type AS DiscountType,
            dc.expire_type AS ExpireType,
            dc.discount_amount AS DiscountAmount,
            dc.count_limit AS CountLimit,
            dc.current_count AS UsageCount,
            dc.status AS Status,
            dc.expire_start_date AS ExpirePeriodStart,
            dc.expire_end_date As ExpirePeriodEnd,
            dc.expired_at_utc AS ExpiredAtUtc,
            dc.last_used_at_utc AS LastUsedAtUtc,
            caa.type AS Area,
            caa.id AS Id,
            case caa.type
                When 'instructor' Then i.first_name + ' ' + i.last_name
                When 'subject' Then sb.name
                When 'session' Then ss.title
            End As Name,
            ca.target_id As Id
            From discount_codes dc Inner Join code_areas ca
            On dc.id = ca.code_id
            Inner Join code_applicable_areas caa
            On caa.id = ca.appplicable_area_id
            Left Join users i
            On i.id = ca.target_id and caa.type = 'instructor'
            Left Join subjects sb
            On sb.id = ca.target_id and caa.type = 'subject'
            Left Join sessions ss
            On ss.id = ca.target_id and caa.type = 'session'
            Where dc.id = @Id
            """;

        using var connection = _dbConnectionFactory.CreateConnection();

        var codeDictionary = new Dictionary<string, DiscountCodeResponseWithTargets>();
        
        await connection.QueryAsync<DiscountCodeResponseWithTargets, ApplicableAreaDto, AreaTargetDto, DiscountCodeResponseWithTargets>(
            new CommandDefinition(
            sql,
            new { Id = id },
            cancellationToken: cancellationToken), (discountCode, area, target) =>
            {

                if (!codeDictionary.TryGetValue(discountCode.Id, out var existingCode))
                {
                    existingCode = discountCode;
                    codeDictionary.Add(discountCode.Id, existingCode);
                }

                existingCode.ApplicableArea = area;

                if (target is not null)
                {
                    existingCode.AreaTargets.Add(target);
                }
                

                return discountCode;
            }, splitOn: "Area,Name");

        codeDictionary.TryGetValue(id, out var code);
        return code;
    }

    public async Task<IEnumerable<DiscountCodeResponse>> GetAllAsync(
        string? code,
        DiscountAmountType? discountType,
        DiscountExpirationType? expireType,
        DiscountStatus? status,
        CancellationToken cancellationToken = default)
    {
        var sql = """
            Select
            id AS Id,
            code AS Code,
            discount_type AS DiscountType,
            expire_type AS ExpireType,
            discount_amount AS DiscountAmount,
            count_limit AS CountLimit,
            current_count AS UsageCount,
            status AS Status,
            expire_start_date AS ExpirePeriodStart,
            expire_end_date As ExpirePeriodEnd,
            expired_at_utc AS ExpiredAtUtc,
            last_used_at_utc AS LastUsedAtUtc
            From discount_codes
            Where (@Status is NULL OR status = @Status) and
                  (@DiscountType is NULL OR discount_type = @DiscountType) and
                  (@ExpireType is NULL OR expire_type = @ExpireType) and
                  (@Code is NULL OR code = @Code)
            """;

        using var connection = _dbConnectionFactory.CreateConnection();

        var result = await connection.QueryAsync<DiscountCodeResponse>(new CommandDefinition(
            sql,
            new { Code = code, DiscountType = discountType, ExpireType = expireType, Status = status },
            cancellationToken: cancellationToken));

        return result;
    }
}
