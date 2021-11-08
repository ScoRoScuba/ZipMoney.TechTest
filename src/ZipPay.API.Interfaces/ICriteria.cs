using System.Collections.Generic;
using System.Linq;

namespace ZipPay.API.Interfaces
{
    public interface ICriteria<TEntity>
    {
        IList<TEntity> MatchQueryFrom(IQueryable<TEntity> dataEntities);
    }
}