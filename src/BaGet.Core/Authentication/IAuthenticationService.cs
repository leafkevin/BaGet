using System.Threading;
using System.Threading.Tasks;

namespace BaGet.Core.Authentication;

public interface IAuthenticationService
{
    Task<bool> AuthenticateAsync(string apiKey, CancellationToken cancellationToken);
}
