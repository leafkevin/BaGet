using BaGet.Protocol.Catalog;
using BaGet.Protocol.Models;
using BaGet.Protocol.PackageContent;
using BaGet.Protocol.PackageMetadata;
using BaGet.Protocol.Search;

namespace BaGet.Protocol.ClientFactories
{
    public class NuGetClients
    {
        public ServiceIndexResponse ServiceIndex { get; set; }
        public IPackageContentClient PackageContentClient { get; set; }
        public IPackageMetadataClient PackageMetadataClient { get; set; }
        public ISearchClient SearchClient { get; set; }
        public IAutocompleteClient AutocompleteClient { get; set; }
        public ICatalogClient CatalogClient { get; set; }
    }
}
