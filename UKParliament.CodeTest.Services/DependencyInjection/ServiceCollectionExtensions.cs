using Microsoft.Extensions.DependencyInjection;
using UKParliament.CodeTest.Services.Interface;
using UKParliament.CodeTest.Services.Service;

namespace UKParliament.CodeTest.Services.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IValidationService, ValidationService>();

            return services;
        }
    }
}