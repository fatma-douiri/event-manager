using Microsoft.Extensions.DependencyInjection;
using EventManager.Application.Auth.Commands.Login;
using EventManager.Application.Auth.Commands.Register;

namespace EventManager.Application.Extensions;

/// <summary>
/// A class to inject MediatR services into the IServiceCollection.
/// </summary>
public static class MediatorExtensions
{
    /// <summary>
    /// Adds MediatR services to the specified IServiceCollection.
    /// </summary>
    /// <param name="services">The IServiceCollection to add the services to.</param>
    /// <returns>The IServiceCollection with MediatR services added.</returns>
    public static IServiceCollection AddApplicationMediatR(this IServiceCollection services)
    {
       
        var assemblies = new[]
        {
            
            typeof(LoginCommand).Assembly,
           

        };

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(assemblies);

          
        });

        return services;
    }
}