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

        if ( builder.Environment.IsDevelopment() )
        {
            builder.Services
            .AddSwaggerGen();
        }

        builder.Services
        .AddCors
        (
            setup => setup.AddDefaultPolicy
            (
                policy => policy.WithOrigins( "http://cleaner.dadata.ru/" )
            )
        );
        
    }

    static void ConfigureMiddleware(WebApplication app)
    {
        if ( app.Environment.IsDevelopment() )
        {
            app.UseSwagger();
            app.UseSwaggerUI
            (
                opt =>
                {
                    opt.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                }
            );
        }
        else
        {
            app.UseHsts();
        }

        app
        .UseHttpsRedirection()
        .UseRouting()
        .UseCors()
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