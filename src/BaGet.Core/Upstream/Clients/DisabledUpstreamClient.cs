using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using BaGet.Core.Entities;
using NuGet.Versioning;

namespace BaGet.Core.Upstream.Clients;

/// <summary>
/// The client used when there are no upstream package sources.
/// </summary>
public class DisabledUpstreamClient : IUpstreamClient
{
    private readonly IReadOnlyList<NuGetVersion> _emptyVersionList = new List<NuGetVersion>();
    private readonly IReadOnlyList<Package> _emptyPackageList = new List<Package>();

    public Task<IReadOnlyList<NuGetVersion>> ListPackageVersionsAsync(string id, CancellationToken cancellationToken)
        => Task.FromResult(_emptyVersionList);

    public Task<IReadOnlyList<Package>> ListPackagesAsync(string id, CancellationToken cancellationToken)
        => Task.FromResult(_emptyPackageList);
    public Task<Stream> DownloadPackageOrNullAsync(string id, NuGetVersion version, CancellationToken cancellationToken)
        => Task.FromResult<Stream>(null);
}
