using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.API.Errors;

public class ExceptionProblemDetailsFactory
{
    public static ProblemDetails Create(HttpContext context, Exception exception, IHostEnvironment environment)
    {
        var problems = new ProblemDetails()
        {
            Detail = environment.IsDevelopment() ? exception.Message : "Try Again!",
            Instance = context.Request.Path,
            Title = "Unhandled Exception errored",
            Status = StatusCodes.Status500InternalServerError
        };
        problems.Extensions["RequestId"] = context.TraceIdentifier;
        return problems;
    }
}