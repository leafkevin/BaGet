using System.Collections.Generic;
using BaGet.Core.Metadata;
using BaGet.Protocol.Models;

namespace BaGet.Core.Search;

public interface ISearchResponseBuilder
{
    SearchResponse BuildSearch(IReadOnlyList<PackageRegistration> results);
    AutocompleteResponse BuildAutocomplete(IReadOnlyList<string> data);
    DependentsResponse BuildDependents(IReadOnlyList<PackageDependent> results);
}
