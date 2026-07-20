using Microsoft.AspNetCore.Mvc;
using OnlineAuctionApp.WebAPI.Helpers;

namespace OnlineAuctionApp.WebAPI.Controllers;

public abstract class BaseApiController : ControllerBase
{
    protected IActionResult ApiSuccess<T>(
        T data,
        int statusCode = StatusCodes.Status200OK)
    {
        return StatusCode(
            statusCode,
            ResponseModelHelper.CreateSuccessResponse(data, statusCode));
    }

    protected IActionResult ApiCreated<T>(
        string actionName,
        object routeValues,
        T data)
    {
        return CreatedAtAction(
            actionName,
            routeValues,
            ResponseModelHelper.CreateSuccessResponse(data, StatusCodes.Status201Created));
    }

    protected IActionResult ApiBadRequest(string error)
    {
        return BadRequest(
            ResponseModelHelper.CreateBadRequestResponse<string>(error));
    }

    protected IActionResult ApiUnauthorized(string error)
    {
        return Unauthorized(
            ResponseModelHelper.CreateUnauthorizedResponse<string>(error));
    }

    protected IActionResult ApiForbidden(string error)
    {
        return StatusCode(
            StatusCodes.Status403Forbidden,
            ResponseModelHelper.CreateForbiddenResponse<string>(error));
    }

    protected IActionResult ApiNotFound(string error)
    {
        return NotFound(
            ResponseModelHelper.CreateNotFoundResponse<string>(error));
    }

    protected IActionResult ApiConflict(string error)
    {
        return Conflict(
            ResponseModelHelper.CreateConflictResponse<string>(error));
    }

    protected IActionResult ApiDeleted(string message = "Deleted successfully.")
    {
        return Ok(
            ResponseModelHelper.CreateSuccessResponse(message));
    }
}