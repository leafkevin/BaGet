using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using BaGet.Protocol.Extensions;
using BaGet.Protocol.Models;

namespace BaGet.Protocol.PackageMetadata;

/// <summary>
/// The client to interact with an upstream source's Package Metadata resource.
/// </summary>
public class RawPackageMetadataClient : IPackageMetadataClient
{
    private readonly HttpClient _httpClient;
    private readonly string _packageMetadataUrl;

    /// <summary>
    /// Create a new Package Metadata client.
    /// </summary>
    /// <param name="httpClient">The HTTP client used to send requests.</param>
    /// <param name="registrationBaseUrl">The NuGet server's registration resource URL.</param>
    public RawPackageMetadataClient(HttpClient httpClient, string registrationBaseUrl)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _packageMetadataUrl = registrationBaseUrl ?? throw new ArgumentNullException(nameof(registrationBaseUrl));
    }
    public async Task<RegistrationIndexResponse> GetRegistrationIndexOrNullAsync(string packageId, CancellationToken cancellationToken = default)
    {
        var url = $"{_packageMetadataUrl}/{packageId.ToLowerInvariant()}/index.json";
        return await _httpClient.GetFromJsonOrDefaultAsync<RegistrationIndexResponse>(url, cancellationToken);
    }
    public async Task<RegistrationPageResponse> GetRegistrationPageAsync(string pageUrl, CancellationToken cancellationToken = default)
        => await _httpClient.GetFromJsonAsync<RegistrationPageResponse>(pageUrl, cancellationToken);
    public async Task<RegistrationLeafResponse> GetRegistrationLeafAsync(string leafUrl, CancellationToken cancellationToken = default)
        => await _httpClient.GetFromJsonAsync<RegistrationLeafResponse>(leafUrl, cancellationToken);
}
