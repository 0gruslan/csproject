using System.Security.Claims;
using WebApplication1.Repositories.Interfaces;

namespace WebApplication1.Middleware;

public class ApiKeyAuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ApiKeyAuthenticationMiddleware> _logger;

    public ApiKeyAuthenticationMiddleware(RequestDelegate next, ILogger<ApiKeyAuthenticationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, IApiKeyRepository apiKeyRepository)
    {
        if (context.Request.Headers.TryGetValue("X-API-KEY", out var apiKeyHeader))
        {
            var apiKeyValue = apiKeyHeader.ToString();
            var apiKey = await apiKeyRepository.GetByKeyValueAsync(apiKeyValue);

            if (apiKey != null && apiKey.IsActive && 
                (apiKey.ExpiresAt == null || apiKey.ExpiresAt > DateTime.UtcNow))
            {
                // Обновляем время последнего использования
                apiKey.LastUsedAt = DateTime.UtcNow;

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, "api-key"),
                    new Claim("ApiKeyId", apiKey.Id.ToString())
                };

                var identity = new ClaimsIdentity(claims, "ApiKey");
                context.User = new ClaimsPrincipal(identity);

                _logger.LogInformation("API Key authenticated: {ApiKeyName}", apiKey.Name);
            }
            else
            {
                _logger.LogWarning("Invalid or expired API Key attempted");
            }
        }

        await _next(context);
    }
}


