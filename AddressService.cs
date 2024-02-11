//-----------------------------------------------------------------------
// <copyright file="AddressService.cs" company="Procare Software, LLC">
//     Copyright © 2021-2024 Procare Software, LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Procare.Address.IntegrationTests;

using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

public class AddressService
{
    private static readonly HttpClient HttpClient = new();

    public AddressService(string baseUrl)
        : this(new Uri(baseUrl))
    {
    }

    public AddressService(Uri baseUrl)
    {
        this.BaseUrl = baseUrl;
    }

    public Uri BaseUrl { get; }

    public async Task<GetAddressesResponse> GetAddressesAsync(AddressFilter request, CancellationToken token = default)
    {
        using var httpRequest = request.ToHttpRequest(this.BaseUrl);
        using var response = await HttpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead, token).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(token).ConfigureAwait(false);

        return JsonSerializer.Deserialize<GetAddressesResponse>(content)!;
    }
}
