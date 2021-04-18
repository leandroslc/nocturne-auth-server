using System.Threading;
using System.Threading.Tasks;

namespace Nocturne.Auth.Core.Services.OpenIddict.Managers
{
    public interface ICustomOpenIddictApplicationManager
    {
        ValueTask<string> GetUnprotectedClientSecretAsync(
            object application,
            CancellationToken cancellationToken = default);
    }
}
