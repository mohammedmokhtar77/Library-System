using LibrarySystem.API.Errors;

namespace LibrarySystem.API.Middlewares;

public class ExceptionHandlingMiddleware(
    RequestDelegate next 
    , ILogger<ExceptionHandlingMiddleware> logger
    , IHostEnvironment environment)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            if (context.Response.HasStarted)
            {
                logger.LogError(ex,
                    "Request {TraceId} failed after the response started; the response cannot be replaced",
                    context.TraceIdentifier);
                throw;
            }
            
            logger.LogError(ex,
                "Unhandled exception while the processing requet {TraceId}",
                context.TraceIdentifier);
            
            context.Response.Clear();
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.Headers["RequestId"] = context.TraceIdentifier;

            var problem = ExceptionProblemDetailsFactory.Create(context,ex, environment);

            await context.Response.WriteAsJsonAsync(
                problem,
                options: null,
                contentType: "application/problem+json",
                cancellationToken: context.RequestAborted
            );
        }
    }
}