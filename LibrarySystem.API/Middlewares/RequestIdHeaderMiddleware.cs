namespace LibrarySystem.API.Middlewares;

public class RequestIdHeaderMiddleware(ILogger<RequestIdHeaderMiddleware> logger) : IMiddleware
{
    public Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var requestId = context.TraceIdentifier;
        context.Request.Headers["Request-Id"] = requestId;
        
        logger.LogDebug(
            "assigned request ID {RequestId} to {Method} {Path}",
            requestId,
            context.Request.Method,
            context.Request.Path
        );
        
        return next(context);
    }
}