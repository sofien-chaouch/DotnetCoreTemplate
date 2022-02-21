using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PlatformService.Repositories
{
    public interface IRepository<TEntity> where TEntity : class, new()  {
        IQueryable<TEntity> GetAll();
        Task<TEntity> Get(int id);

        Task<TEntity> AddAsync(TEntity entity);

        TEntity Delete(TEntity entity);
        TEntity Update( TEntity entity );
        IQueryable<TEntity> GetMany( Expression<Func<TEntity, bool>> where = null, Expression<Func<TEntity, bool>> orderBy = null );
        void DeleteMany( Expression<Func<TEntity, bool>> where );
    }
}