using OnlineAuctionApp.Application.Common.Responses;

namespace OnlineAuctionApp.WebAPI.Helpers;

public static class ResponseModelHelper
{
    public static ResponseModel<T> CreateSuccessResponse<T>(
        T data,
        int statusCode = StatusCodes.Status200OK)
    {
        return new ResponseModel<T>
        {
            IsSuccess = true,
            StatusCode = statusCode,
            Errors = null,
            Data = data
        };
    }

    public static ResponseModel<T> CreateBadRequestResponse<T>(string error)
    {
        return new ResponseModel<T>
        {
            IsSuccess = false,
            StatusCode = StatusCodes.Status400BadRequest,
            Errors = new List<string> { error },
            Data = default
        };
    }

    public static ResponseModel<T> CreateBadRequestResponse<T>(List<string> errors)
    {
        return new ResponseModel<T>
        {
            IsSuccess = false,
            StatusCode = StatusCodes.Status400BadRequest,
            Errors = errors,
            Data = default
        };
    }

    public static ResponseModel<T> CreateUnauthorizedResponse<T>(string error)
    {
        return new ResponseModel<T>
        {
            IsSuccess = false,
            StatusCode = StatusCodes.Status401Unauthorized,
            Errors = new List<string> { error },
            Data = default
        };
    }

    public static ResponseModel<T> CreateForbiddenResponse<T>(string error)
    {
        return new ResponseModel<T>
        {
            IsSuccess = false,
            StatusCode = StatusCodes.Status403Forbidden,
            Errors = new List<string> { error },
            Data = default
        };
    }

    public static ResponseModel<T> CreateNotFoundResponse<T>(string error)
    {
        return new ResponseModel<T>
        {
            IsSuccess = false,
            StatusCode = StatusCodes.Status404NotFound,
            Errors = new List<string> { error },
            Data = default
        };
    }

    public static ResponseModel<T> CreateConflictResponse<T>(string error)
    {
        return new ResponseModel<T>
        {
            IsSuccess = false,
            StatusCode = StatusCodes.Status409Conflict,
            Errors = new List<string> { error },
            Data = default
        };
    }

    public static ResponseModel<T> CreateErrorResponse<T>(List<string> errors)
    {
        return new ResponseModel<T>
        {
            IsSuccess = false,
            StatusCode = StatusCodes.Status500InternalServerError,
            Errors = errors,
            Data = default
        };
    }
}