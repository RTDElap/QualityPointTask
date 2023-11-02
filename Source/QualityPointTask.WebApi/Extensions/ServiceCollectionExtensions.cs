

using Dadata;
using Microsoft.Extensions.Options;
using QualityPointTask.Core.Services;
using QualityPointTask.Services;
using QualityPointTask.WebApi.Configs;

namespace QualityPointTask.WebApi.Extensions;

public static partial class ServiceCollectionExtensions
{
    /// <summary>
    /// Регистрирует ICleanClientAsync
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddCleanClient(this IServiceCollection services)
    {
        services.AddSingleton<ICleanClientAsync>( serviceProvider =>
            {
                IOptions<DadataConfig>? config = serviceProvider.GetService<IOptions<DadataConfig>>();
                IHttpClientFactory? httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();

                if ( config is null ) 
                    throw new ArgumentNullException("DadataConfig не установлен");

                if ( httpClientFactory is null ) 
                    throw new ArgumentNullException("HttpClientFactory не установлен");

                return new CleanClientAsync
                ( 
                    token: config.Value.Token, 
                    secret: config.Value.Secret, 
                    client: httpClientFactory.CreateClient(nameof(ICleanClientAsync)) 
                );
            }
        );

        return services;
    }

    /// <summary>
    /// Регистрирует IAddressService
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddAddressService(this IServiceCollection services)
    {
        services.AddScoped<IAddressService, AddressService>();

        return services;
    }
}