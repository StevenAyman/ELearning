using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using ELearning.Application.Abstractions.Data;
using ELearning.Application.Users.GetUserWithIdentity;

namespace ELearning.Infastructure.Data;
public sealed class UserReadService(IDbConnectionFactory dbConnectionFactory) : IUserReadService
{
    private readonly IDbConnectionFactory _dbConnectionFactory = dbConnectionFactory;
    public async Task<UserDto?> GetUserWithIdentityIdAsync(string identityId, CancellationToken cancellationToken = default)
    {
        using var connection = _dbConnectionFactory.CreateConnection();
        var sql = """
            SELECT
            id As Id,
            first_name As FirstName,
            last_name As LastName,
            email As Email,
            date_of_birth As BirthDate,
            city As City,
            identity_id As IdentityId
            From users
            Where identity_id = @IdentityId
            """;
        var user = await connection.QueryFirstOrDefaultAsync<UserDto>(
            new CommandDefinition(sql, 
            new { IdentityId = identityId }, 
            cancellationToken: cancellationToken));

        return user;
    }
}
