using Microsoft.AspNetCore.Diagnostics;

namespace FullTractor.Api.Exceptions;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<GlobalExceptionHandler> _logger;
    public GlobalExceptionHandler(IWebHostEnvironment env, ILogger<GlobalExceptionHandler> logger)
    {
        _env = env;
        _logger = logger;
    }
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        IProblemDetailsService? problemDetailService = httpContext.RequestServices.GetService<IProblemDetailsService>();
        if (problemDetailService != null)
        {
            _logger.LogError(exception, "Unhandled expcetion ocurred.");
            await problemDetailService.WriteAsync(new ProblemDetailsContext
            {
                HttpContext = httpContext,
                ProblemDetails =
                {
                    Status = 500,
                    Title = "Ocurrio un error que no se encuentra controlado",
                    Detail = _env.IsDevelopment() ? exception.Message : "Existe un error que necesita ser corregido",
                }
            });
            return true;
        }
        return false;
    }
}