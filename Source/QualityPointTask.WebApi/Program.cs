using System;
using AutoMapper;
using Dadata;
using Dadata.Model;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using QualityPointTask.Core.Services;
using QualityPointTask.Infrastructure.Models;
using QualityPointTask.Services;

internal class Program
{
    static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        Console.WriteLine( builder.Configuration.GetValue<string>("DADATA_API_KEY") );

        builder.Services
            .AddAutoMapper( cfg => 
            {
                cfg.SourceMemberNamingConvention = LowerUnderscoreNamingConvention.Instance;
                cfg.DestinationMemberNamingConvention = PascalCaseNamingConvention.Instance;
                cfg.CreateMap<Address, AddressResult>();
            })
            .AddSingleton<ICleanClientAsync>( x =>
            {
                return new CleanClientAsync(token: builder.Configuration.GetValue<string>("DADATA_API_KEY"), secret: builder.Configuration.GetValue<string>("DADATA_SECRET") );

            } )
            .AddScoped<IAddressService, AddressService>()
            .AddControllers();

        var app = builder.Build();

        app
            .UseRouting()
            .UseEndpoints( x => x.MapControllers() );

        app.Run();
    }
}