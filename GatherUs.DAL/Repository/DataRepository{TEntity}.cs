using GatherUs.DAL.Context;
using GatherUs.DAL.Models;

namespace GatherUs.DAL.Repository;

public class DataRepository<TEntity> : DataRepository<TEntity, int>, IDataRepository<TEntity>
    where TEntity : EntityBase
{
    public DataRepository(IDataContext context)
        : base(context)
    {
    }
}