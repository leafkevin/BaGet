// ReSharper disable EntityFramework.ModelValidation.UnlimitedStringLength
namespace BaGet.Core.Entities;

public class TargetFramework
{
    public int Key { get; set; }
    public string Moniker { get; set; }
    public Package Package { get; set; }
}
