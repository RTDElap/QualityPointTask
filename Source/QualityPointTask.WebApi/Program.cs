using System;
using AutoMapper;
using Dadata;
using Dadata.Model;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using QualityPointTask.Core.Services;
using QualityPointTask.Infrastructure.Models;
using QualityPointTask.Services;
using QualityPointTask.WebApi.Configs;
using QualityPointTask.WebApi.Extensions;

namespace QualityPointTask.WebApi;

internal class Program
{
    static void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services
        .Configure<DadataConfig>
        ( 
            builder.Configuration.GetSection( nameof(DadataConfig) ) 
        );

        builder.Services
        .AddAutoMapper
        ( 
            cfg => 
            {
                cfg.SourceMemberNamingConvention = LowerUnderscoreNamingConvention.Instance;
                cfg.DestinationMemberNamingConvention = PascalCaseNamingConvention.Instance;
                cfg.CreateMap<Address, AddressResult>();
            }
        );

        builder.Services
        .AddHttpClient( nameof( ICleanClientAsync ) );

        builder.Services
        .AddCleanClient()
        .AddAddressService()
        .AddControllers();
    }

    static void ConfigureMiddleware(WebApplication app)
    {
        app
        .UseRouting()
        .UseEndpoints
        (
            endpoints => endpoints.MapControllers()
        );
    }

    static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        ConfigureServices(builder);

        var app = builder.Build();

        ConfigureMiddleware(app);

        app.Run();
    }
}