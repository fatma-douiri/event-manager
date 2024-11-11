using EventManager.Application.Auth.Validators;
using FluentValidation;

namespace EventManager.API.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();



        // Add validators
        services.AddValidatorsFromAssemblyContaining<LoginRequestValidator>();
        return services;
    }
}