using ELearning.Api.DTOs.Discounts;
using ELearning.Api.DTOs.Shared;
using ELearning.Application.Discounts.CreateDiscountCode;
using ELearning.Application.Discounts.DeleteDiscountCode;
using ELearning.Application.Discounts.DTOs;
using ELearning.Application.Discounts.ExpireDiscountCode;
using ELearning.Application.Discounts.GetAllDiscountCodes;
using ELearning.Application.Discounts.GetDiscountCode;
using ELearning.Application.Discounts.UpdateDiscountCode;
using ELearning.Domain.Discounts;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ELearning.Api.Controllers.DiscountCodes;
[Route("api/[controller]")]
[ApiController]
public class DiscountCodesController(
    ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;
    /// <summary>
    /// Creates new discount code
    /// </summary>
    /// <param name="request">Object contains or discount code details to be created</param>
    /// <returns>Object of created code</returns>
    [ProducesResponseType<DiscountCodeResponseWithTargets>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<ActionResult<DiscountCodeResponseWithTargets>> Create(CreateDiscountCodeRequest request)
    {
        var command = new CreateDiscountCodeCommand(
            request.Code,
            request.DiscountType,
            request.ExpireType,
            request.Amount,
            request.CountLimit,
            request.ExpirePeriodStart,
            request.ExpirePeriodEnd,
            request.DiscountApplicableAreaId,
            request.TargetIds);

        var result = await _sender.Send(command);

        if (result.IsFailure)
        {
            return Problem(
                detail: result.Error?.Message,
                title: result.Error?.Code,
                statusCode: StatusCodes.Status400BadRequest);
        }

        return CreatedAtAction(nameof(Get), new { id = result.Value.Id}, result.Value);
    }

    /// <summary>
    /// Gets a discount code with its unique identifier
    /// </summary>
    /// <param name="id">Discount code unique identifier</param>
    /// <returns>Object represents discount code details</returns>
    [ProducesResponseType<DiscountCodeResponseWithTargets>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id}")]
    public async Task<ActionResult<DiscountCodeResponseWithTargets>> Get(string id)
    {
        var query = new GetDiscountCodeQuery(id);

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
    /// Gets all discount codes based on (query params)
    /// </summary>
    /// <param name="request">All acceptable query params</param>
    /// <returns>List of objects each one contains code details</returns>
    [ProducesResponseType<AllDataDto<DiscountCodeResponse>>(StatusCodes.Status200OK)]
    [HttpGet]
    public async Task<ActionResult<AllDataDto<DiscountCodeResponse>>> GetAll([FromQuery] DiscountCodeQuery request)
    {
        var query = new GetAllDiscountCodesQuery(request.Status, request.DiscountType, request.ExpireType, request.Code);

        var result = await _sender.Send(query);

        var response = new AllDataDto<DiscountCodeResponse>()
        {
            Data = result.Value
        };

        return Ok(response);
    }

    /// <summary>
    /// Removes a discount code using its unique identifier
    /// </summary>
    /// <param name="id">Discount code unique identifier</param>
    /// <returns>Status code 204 if successful</returns>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCode(string id)
    {
        var command = new DeleteDiscountCodeCommand(id);

        var result = await _sender.Send(command);

        if (result.IsFailure)
        {
            return Problem(
                detail: result.Error?.Message,
                title: result.Error?.Code,
                statusCode: StatusCodes.Status404NotFound);
        }

        return NoContent();

    }

    /// <summary>
    /// Expires an active discount code using its identifier
    /// </summary>
    /// <param name="id">Discount code identifier</param>
    /// <returns>Status code 204 if successful</returns>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost("{id}/expire")]
    public async Task<IActionResult> ExpireCode(string id)
    {
        var command = new ExpireDiscountCodeCommand(id);

        var result = await _sender.Send(command);

        if (result.IsFailure)
        {
            return Problem(
                detail: result.Error?.Message,
                title: result.Error?.Code,
                statusCode: result.Error == DiscountErrors.NotFound ? StatusCodes.Status404NotFound : StatusCodes.Status400BadRequest);
        }

        return NoContent();
    }

    /// <summary>
    /// Updates an discount code
    /// </summary>
    /// <param name="id">The unique identifier of discount code required to be updated</param>
    /// <param name="request">Object contains all discount code details (including updated values and unchanged ones)</param>
    /// <returns>Status code 204 if successful</returns>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, CreateDiscountCodeRequest request)
    {
        var command = new UpdateDiscountCodeCommand(
            id,
            request.Code,
            request.DiscountType,
            request.ExpireType,
            (decimal)request.Amount,
            request.ExpirePeriodStart,
            request.ExpirePeriodEnd,
            request.CountLimit,
            request.DiscountApplicableAreaId,
            request.TargetIds);

        var result = await _sender.Send(command);

        if (result.IsFailure)
        {
            var problemDetails = Problem(
                detail: result.Error?.Message,
                title: result.Error?.Code);

            if (result.Error == DiscountErrors.Overlap)
            {
                problemDetails.StatusCode = StatusCodes.Status409Conflict;
            }
            else if (result.Error == DiscountErrors.NotFound || result.Error == DiscountErrors.AreaNotFound)
            {
                problemDetails.StatusCode = StatusCodes.Status404NotFound;
            }
            else
            {
                problemDetails.StatusCode = StatusCodes.Status400BadRequest;
            }

            return problemDetails;
        }

        return NoContent();
    }
}


