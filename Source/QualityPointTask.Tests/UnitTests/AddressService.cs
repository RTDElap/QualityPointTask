using System.Runtime.InteropServices;
using AutoMapper;
using Dadata;
using Dadata.Model;
using Microsoft.Extensions.Logging;
using Moq;
using QualityPointTask.Core.Exceptions;
using QualityPointTask.Infrastructure.Models;
using QualityPointTask.Services;

namespace QualityPointTask.Tests.UnitTests;

public class AddressServiceTest
{
    [Fact]
    public void GetAddressResultFromAsync_correct()
    {
        string[] addressParts = { "мск", "сухонска", "11", "-89" };
        string fullAddress = "мск сухонска 11 -89";
        
        var cleanClientAsync = new Mock<ICleanClientAsync>();

        cleanClientAsync
            .Setup(client => client.Clean<Address>( fullAddress , It.IsAny<CancellationToken>() ))
            .ReturnsAsync
            (
                new Address()
                {
                    country = "Россия",
                    region = "Москва",
                    qc = "0",
                    qc_complete = "0"
                }
            );

        var logger = new Mock<ILogger<AddressService>>();

        var mapper = new MapperConfiguration( cfg =>
            {
                cfg.SourceMemberNamingConvention = LowerUnderscoreNamingConvention.Instance;
                cfg.DestinationMemberNamingConvention = PascalCaseNamingConvention.Instance;
                cfg
                    .CreateMap<Address, AddressResult>()
                    .ForMember(x => x.MailingQuality, opt => opt.Ignore());
            }
        );

        var addressService = new AddressService(logger.Object, cleanClientAsync.Object, mapper.CreateMapper());
        
        var addressResult = addressService.GetAddressResultFromAsync(addressParts).Result;

        Assert.Equal("Россия", addressResult.Country);
        Assert.Equal("Москва", addressResult.Region);
    }

    [Fact]
    public void GetAddressResultFromAsync_qc_1_exception()
    {
        string[] addressParts = { "мск", "сухонска", "11", "-89" };
        string fullAddress = "мск сухонска 11 -89";
        
        var cleanClientAsync = new Mock<ICleanClientAsync>();

        cleanClientAsync
            .Setup(client => client.Clean<Address>( fullAddress , It.IsAny<CancellationToken>() ))
            .ReturnsAsync
            (
                new Address()
                {
                    country = "Россия",
                    region = "Москва",
                    qc = "1",
                    qc_complete = "0"
                }
            );

        var logger = new Mock<ILogger<AddressService>>();

        var mapper = new MapperConfiguration( cfg =>
            {
                cfg.SourceMemberNamingConvention = LowerUnderscoreNamingConvention.Instance;
                cfg.DestinationMemberNamingConvention = PascalCaseNamingConvention.Instance;
                cfg
                    .CreateMap<Address, AddressResult>()
                    .ForMember(x => x.MailingQuality, opt => opt.Ignore());
            }
        );

        var addressService = new AddressService(logger.Object, cleanClientAsync.Object, mapper.CreateMapper());

        Assert.ThrowsAsync<NotEnoughDataException>( () => addressService.GetAddressResultFromAsync(addressParts) );
    }

    [Fact]
    public void GetAddressResultFromAsync_qc_3_exception()
    {
        string[] addressParts = { "мск", "сухонска", "11", "-89" };
        string fullAddress = "мск сухонска 11 -89";
        
        var cleanClientAsync = new Mock<ICleanClientAsync>();

        cleanClientAsync
            .Setup(client => client.Clean<Address>( fullAddress , It.IsAny<CancellationToken>() ))
            .ReturnsAsync
            (
                new Address()
                {
                    country = "Россия",
                    region = "Москва",
                    qc = "3",
                    qc_complete = "0"
                }
            );

        var logger = new Mock<ILogger<AddressService>>();

        var mapper = new MapperConfiguration( cfg =>
            {
                cfg.SourceMemberNamingConvention = LowerUnderscoreNamingConvention.Instance;
                cfg.DestinationMemberNamingConvention = PascalCaseNamingConvention.Instance;
                cfg
                    .CreateMap<Address, AddressResult>()
                    .ForMember(x => x.MailingQuality, opt => opt.Ignore());
            }
        );

        var addressService = new AddressService(logger.Object, cleanClientAsync.Object, mapper.CreateMapper());
        
        Assert.ThrowsAsync<UndefinedAddressException>( () => addressService.GetAddressResultFromAsync(addressParts) );
    }
}