using System.Collections.Generic;

namespace Nocturne.Auth.Core.Collections
{
    public interface IPagedCollection<TValue> : IPagedCollection, IEnumerable<TValue>
    {
    }
}
