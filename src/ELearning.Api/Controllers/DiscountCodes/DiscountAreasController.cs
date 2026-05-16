using ELearning.Api.DTOs.Discounts;
using ELearning.Api.DTOs.Shared;
using ELearning.Application.Discounts.CreateDiscountArea;
using ELearning.Application.Discounts.DeleteDiscountArea;
using ELearning.Application.Discounts.DTOs;
using ELearning.Application.Discounts.GetAllDiscountAreas;
using ELearning.Application.Discounts.GetDiscountArea;
using ELearning.Application.Discounts.UpdateDiscountArea;
using ELearning.Domain.Discounts;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ELearning.Api.Controllers.DiscountCodes;
[Route("api/[controller]")]
[ApiController]
public sealed class DiscountAreasController(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

    /// <summary>
    /// Gets all available discount areas
    /// </summary>
    /// <returns>List of objects each contains area details</returns>
    [ProducesResponseType<AllDataDto<DiscountAreaResponse>>(StatusCodes.Status200OK)]
    [HttpGet]
    public async Task<ActionResult<AllDataDto<DiscountAreaResponse>>> GetAll()
    {
        var query = new GetAllDiscountAreasQuery();

        var result = await _sender.Send(query);

        var response = new AllDataDto<DiscountAreaResponse>()
        {
            Data = result.Value
        };

        return Ok(response);
    }

    /// <summary>
    /// Gets discount area by its identifier
    /// </summary>
    /// <param name="id">Discount area unique identifier</param>
    /// <returns>Object contains request aread details</returns>
    [ProducesResponseType<DiscountAreaResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id}")]
    public async Task<ActionResult<DiscountAreaResponse>> Get(int id)
    {
        var query = new GetDiscountAreaQuery(id);

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
    /// Creates new discount area
    /// </summary>
    /// <param name="request">Object contains new area name</param>
    /// <returns>Object contains created area details</returns>
    [ProducesResponseType<DiscountAreaResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [HttpPost]
    public async Task<ActionResult<DiscountAreaResponse>> Create(CreateDiscountAreaRequest request)
    {
        var command = new CreateDiscountAreaCommand(request.Area);

        var result = await _sender.Send(command);

        if (result.IsFailure)
        {
            return Problem(
                detail: result.Error?.Message,
                title: result.Error?.Code,
                statusCode: StatusCodes.Status409Conflict);
        }

        var response = new DiscountAreaResponse()
        {
            Id = result.Value,
            Area = request.Area,
        };

        return CreatedAtAction(nameof(Get), new { id = result.Value }, response);
    }

    /// <summary>
    /// Updates discount area name using its identifier
    /// </summary>
    /// <param name="id">Discount area unique identifier</param>
    /// <param name="request">Object contains new area name</param>
    /// <returns></returns>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, CreateDiscountAreaRequest request)
    {
        var command = new UpdateDiscountAreaCommand(id, request.Area);

        var result = await _sender.Send(command);

        if (result.IsFailure)
        {
            return Problem(
                detail: result.Error?.Message,
                title: result.Error?.Code,
                statusCode: result.Error == DiscountErrors.AreaExists? StatusCodes.Status409Conflict : StatusCodes.Status404NotFound);
        }

        return NoContent();
    }

    /// <summary>
    /// Deletes discount area using its identifier
    /// </summary>
    /// <param name="id">Discount area unique identifier</param>
    /// <returns></returns>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new DeleteDiscountAreaCommand(id);

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

}
