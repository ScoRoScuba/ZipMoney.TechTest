using System.Collections.Generic;

namespace ZipPay.API.Interfaces
{
    public interface IRepository<TEntity>
    {
        long Add(TEntity item);
        TEntity Get(long id);
        List<TEntity> Get();

        IList<TEntity> Match(ICriteria<TEntity> criteria);
    }
}
