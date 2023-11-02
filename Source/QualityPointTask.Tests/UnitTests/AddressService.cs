using AutoMapper;
using Dadata;
using Dadata.Model;
using Microsoft.Extensions.Logging;
using Moq;
using QualityPointTask.Infrastructure.Models;
using QualityPointTask.Services;

namespace QualityPointTask.Tests.UnitTests;

public class AddressServiceTest
{
    [Fact]
    public void GetAddressResultFromAsync_correct()
    {
        string[] addressParts = { "мск", "сухонска", "11", "-89" };
        string fullAddress = "мск сухонска 11/-89";
        
        var cleanClientAsync = new Mock<ICleanClientAsync>();

        cleanClientAsync
            .Setup(client => client.Clean<Address>( fullAddress , It.IsAny<CancellationToken>() ))
            .ReturnsAsync
            (
                new Address()
                {
                    country = "Россия",
                    region = "Москва",
                }
            );

        var logger = new Mock<ILogger<AddressService>>();

        var mapper = new MapperConfiguration( cfg =>
            {
                cfg.SourceMemberNamingConvention = LowerUnderscoreNamingConvention.Instance;
                cfg.DestinationMemberNamingConvention = PascalCaseNamingConvention.Instance;
                cfg.CreateMap<Address, AddressResult>();
            }
        );

        mapper.AssertConfigurationIsValid();

        var addressService = new AddressService(logger.Object, cleanClientAsync.Object, mapper.CreateMapper());
        
        var addressResult = addressService.GetAddressResultFromAsync(addressParts).Result;

        Assert.Equal("Россия", addressResult.Country);
        Assert.Equal("Москва", addressResult.Region);
    }
}