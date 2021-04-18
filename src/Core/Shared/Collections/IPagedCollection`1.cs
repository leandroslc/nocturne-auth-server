using System.Collections.Generic;

namespace Nocturne.Auth.Core.Shared.Collections
{
    public interface IPagedCollection<TValue> : IPagedCollection, IEnumerable<TValue>
    {
    }
}
