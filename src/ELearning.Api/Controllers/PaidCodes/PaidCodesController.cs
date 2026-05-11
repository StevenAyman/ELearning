using ELearning.Api.DTOs.PaidCodes;
using ELearning.Api.DTOs.Shared;
using ELearning.Application.PaidCodes.DTOs;
using ELearning.Application.PaidCodes.ExpirePaidCode;
using ELearning.Application.PaidCodes.GeneratePaidCodes;
using ELearning.Application.PaidCodes.GetAllPaidCodes;
using ELearning.Application.PaidCodes.GetPaidCode;
using ELearning.Application.PaidCodes.RedeemPaidCode;
using ELearning.Domain.Purchases;
using ELearning.Domain.Shared;
using ELearning.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace ELearning.Api.Controllers.PaidCodes;
[Route("api/[controller]")]
[ApiController]
public class PaidCodesController(
    ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

    /// <summary>
    /// Creates any amount of codes with specific balance
    /// </summary>
    /// <param name="dto">Object contains codes count and balance</param>
    /// <returns>All created codes</returns>
    [ProducesResponseType<AllDataDto<PaidCodeResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<ActionResult<AllDataDto<PaidCodeResponse>>> Create(PaidCodeCreateRequest dto)
    {
        var command = new GeneratePaidCodesCommand(dto.Count, dto.Balance);

        var result = await _sender.Send(command);

        if (result.IsFailure)
        {
            return Problem(
                detail: result.Error?.Message,
                title: result.Error?.Code,
                statusCode: StatusCodes.Status400BadRequest);
        }

        var response = new AllDataDto<PaidCodeResponse>
        {
            Data = result.Value
        };
        return Ok(response);
    }

    /// <summary>
    /// Gets all codes
    /// </summary>
    /// <param name="paidCodesQuery">Query params required for filteration </param>
    /// <returns>All available codes with their details</returns>
    [ProducesResponseType<AllDataDto<FullPaidCodeResponse>>(StatusCodes.Status200OK)]
    [HttpGet]
    public async Task<ActionResult<AllDataDto<FullPaidCodeResponse>>> GetAll([FromQuery]PaidCodesQuery paidCodesQuery)
    {
        var query = new GetAllPaidCodesQuery(paidCodesQuery.Status, paidCodesQuery.StartDate, paidCodesQuery.EndDate);

        var result = await _sender.Send(query);

        var response = new AllDataDto<FullPaidCodeResponse>()
        {
            Data = result.Value
        };

        return Ok(response);
    }

    /// <summary>
    /// Get specific code using its Identifier
    /// </summary>
    /// <param name="id">Code unique identifier</param>
    /// <returns>Requested code details</returns>
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType<FullPaidCodeResponse>(StatusCodes.Status200OK)]
    [HttpGet("{id}")]
    public async Task<ActionResult<FullPaidCodeResponse>> Get(string id)
    {
        var query = new GetPaidCodeQuery(id);

        var result = await _sender.Send(query);

        if (result.IsFailure)
        {
            return Problem(
                detail: result.Error?.Message,
                title: result.Error?.Code,
                statusCode: StatusCodes.Status404NotFound);
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Changes code status to be expired
    /// </summary>
    /// <param name="expirePaidCodeRequest">Object contains code</param>
    /// <returns></returns>
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpPost("expire")]
    public async Task<IActionResult> ExpireCode(ExpirePaidCodeRequest expirePaidCodeRequest)
    {
        var command = new ExpirePaidCodeCommand(expirePaidCodeRequest.Code);

        var result = await _sender.Send(command);

        if (result.IsFailure)
        {
            var problem = Problem(
                detail: result.Error?.Message,
                title: result.Error?.Code,
                statusCode: result.Error == PaidCodeErrors.NotFound ? StatusCodes.Status404NotFound : StatusCodes.Status400BadRequest);

            return problem;
        }

        return NoContent();
    }

    /// <summary>
    /// Used to redeem a valid code
    /// </summary>
    /// <param name="expirePaidCodeRequest">Object contains code</param>
    /// <param name="studentId">Student identifier that want to redeem that code</param>
    /// <returns></returns>
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [EnableRateLimiting("redeem-code")]
    [HttpPost("redeem/{studentId}")]
    public async Task<IActionResult> RedeemCode(ExpirePaidCodeRequest expirePaidCodeRequest, string studentId)
    {
        var command = new RedeemPaidCodeCommand(expirePaidCodeRequest.Code, studentId);

        var result = await _sender.Send(command);

        if (result.IsFailure)
        {
            var problem = Problem(
                detail: result.Error?.Message,
                title: result.Error?.Code);

            if (result.Error == UserErrors.UserNotExist || result.Error == PaidCodeErrors.NotFound)
            {
                problem.StatusCode = StatusCodes.Status404NotFound;
                return problem;
            }

            problem.StatusCode = StatusCodes.Status400BadRequest;
            return problem;
        }

        return NoContent();
    }
}


