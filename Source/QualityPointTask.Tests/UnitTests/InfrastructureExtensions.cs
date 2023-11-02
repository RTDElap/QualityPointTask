using System.Runtime.InteropServices;
using AutoMapper;
using Dadata;
using Dadata.Model;
using Microsoft.Extensions.Logging;
using Moq;
using QualityPointTask.Core.Exceptions;
using QualityPointTask.Infrastructure.Extensions;
using QualityPointTask.Infrastructure.Models;
using QualityPointTask.Services;
using QualityPointTask.Infrastructure.Enums;

namespace QualityPointTask.Tests.UnitTests;

public class InfrastructureExtensionsTest
{
    [Fact]
    public void ParseMailingQuality_yes()
    {
        string cqComplete = "0";

        Assert.Equal( MailingQuality.Yes, cqComplete.ParseMailingQuality() );
    }

    [Theory]
    [InlineData("10")]
    [InlineData("5")]
    [InlineData("8")]
    [InlineData("9")]
    public void ParseMailingQuality_maybe(string cqComplete)
    {
        Assert.Equal( MailingQuality.Maybe, cqComplete.ParseMailingQuality() );
    }

    [Theory]
    [InlineData("1")]
    [InlineData("2")]
    [InlineData("3")]
    [InlineData("4")]
    [InlineData("6")]
    [InlineData("7")]
    public void ParseMailingQuality_no(string cqComplete)
    {
        Assert.Equal( MailingQuality.No, cqComplete.ParseMailingQuality() );
    }

    [Fact]
    public void ParseMailingQuality_throw_argument_out_of_range()
    {
        Assert.Throws<ArgumentOutOfRangeException>( () => "52".ParseMailingQuality() );
    }
}