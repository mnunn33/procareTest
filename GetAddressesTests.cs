//-----------------------------------------------------------------------
// <copyright file="GetAddressesTests.cs" company="Procare Software, LLC">
//     Copyright © 2021-2024 Procare Software, LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Procare.Address.IntegrationTests;

using System;
using System.ComponentModel.Design;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Xml.Xsl;
using Xunit;

public class GetAddressesTests
{
    private readonly AddressService service = new("https://address.dev-procarepay.com");

    [Fact]
    public async Task GetAddresses_With_Owm_ShouldResultIn_OneMatchingAddress()
    {
        // var result = await this.service.GetAddressesAsync(new AddressFilter { Line1 = "1125 17TH ST", Line2 = "STE 1800", City = "Denver", StateCode = "CO" }).ConfigureAwait(true);
        var result = await this.service.GetAddressesAsync(new AddressFilter { Line1 = "1 W Main St", City = "Medford", StateCode = "OR" }).ConfigureAwait(true);

        Assert.NotNull(result);
        Assert.Equal(1, result.Count);
        Assert.NotNull(result.Addresses);
        Assert.Equal(result.Count, result.Addresses!.Count);
    }

    [Fact]
    public async Task GetAddresses_With_AmbiguousAddress_ShouldResultIn_MultipleMatchingAddresses()
    {
        var result = await this.service.GetAddressesAsync(new AddressFilter { Line1 = "123 main St", City = "Ontario", StateCode = "CA" }).ConfigureAwait(true);

        Assert.NotNull(result);
        Assert.True(result.Count > 1);
        Assert.NotNull(result.Addresses);
    }

    [Fact]
    public async Task GetAddresses_with_InvalidCombination_Empty_String_State_ShouldResultIn_Exception()
    {
        var result = await this.service.GetAddressesAsync(new AddressFilter { Line1 = "4343 S Syracuse St", City = string.Empty, StateCode = string.Empty, ZipCodeLeading5 = string.Empty}).ConfigureAwait(true);
        Assert.ThrowsAny<HttpRequestException>(() => result);
    }

    [Fact]
    public async Task GetAddresses_With_ZipCode_Instead_Of_City_And_State_Should_Return_Multiple_Addresses()
    {
        var result = await this.service.GetAddressesAsync(new AddressFilter { Line1 = "4343 S Syracuse St", ZipCodeLeading5 = "80237" }).ConfigureAwait(true);
        Assert.NotNull(result);
        Assert.Equal(1,result.Count);
        Assert.NotNull(result.Addresses);
    }
}