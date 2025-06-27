using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdstractHelpers.Storage.Abstraction.Interfases
{
    public interface IDbStorage<TItem, TKey>
        where TItem : class, IId<TKey>
        where TKey : struct
    {
        public Task<TItem?> GetItemById(TKey id);

        public Task<TItem?> AddItem<TCreate>(TCreate model);

        public Task<List<TItem>> AddItems<TCreate>(List<TCreate> model);

        public Task<TItem?> UpdateItem<TUpdate>(TKey id, TUpdate target);

        public Task<TItem?> DeleteElement(TKey id);

        public Task SaveChangesAsync();
    }
}
