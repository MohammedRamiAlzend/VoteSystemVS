using Microsoft.Extensions.DependencyInjection;
using MediatR;
using Application.Features.Auth;
using Application.Features.Auth.Common;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IOtpService, OtpService>();

        // Register MediatR and scan this assembly for handlers
        services.AddMediatR(typeof(DependencyInjection).Assembly);

        services.AddAutoMapper(typeof(DependencyInjection).Assembly);
        return services;
    }
}
