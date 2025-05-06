using Microsoft.AspNetCore.Diagnostics;
using Models.DTOs;
using Serilog;

namespace MouqimApi.Utils;

public class AppExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext,
        Exception exception, CancellationToken cancellationToken)
    {
        Log.Error("{StackTree}- {Instance}- {Message}", exception.StackTrace,
            exception.InnerException, exception.Message);

        var response = new BaseResponse
        {
            Success = false,
            Message = "Internal Server Error, pls try again later"
        };

        httpContext.Response.StatusCode = 500;
        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

        return true;
    }
}