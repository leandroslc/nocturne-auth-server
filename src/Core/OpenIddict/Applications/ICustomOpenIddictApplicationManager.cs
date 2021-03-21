using System.Threading;
using System.Threading.Tasks;

namespace Nocturne.Auth.Core.OpenIddict.Applications
{
    public interface ICustomOpenIddictApplicationManager
    {
        ValueTask<string> GetUnprotectedClientSecretAsync(
            object application,
            CancellationToken cancellationToken = default);
    }
}
