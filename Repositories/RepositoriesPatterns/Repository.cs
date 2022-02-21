using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PlatformService.Data;

namespace PlatformService.Repositories.RepositoriesPatterns
{
        public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new() {

                protected readonly AppDbContext _context;
                public Repository(AppDbContext context) {
                        _context = context;
                }

                public async Task<TEntity> Get(int id)
                {
                        return await _context.Set<TEntity>().FindAsync(id);
                }

                public IQueryable<TEntity> GetAll()
                {
                        try {
                                return _context.Set<TEntity>();
                        }
                        catch (Exception ex) {
                                throw new Exception($"Couldn't retrieve entities: {ex.Message}");
                        }
                }

                public async Task<TEntity> AddAsync(TEntity entity)
                {
                        if (entity == null) {
                                throw new ArgumentNullException($"{nameof(AddAsync)} entity must not be null");
                        }

                        try {
                                await _context.AddAsync(entity);
                                await _context.SaveChangesAsync();

                                return entity;
                        }
                        catch (Exception ex)
                        {
                                throw new Exception($"{nameof(entity)} could not be saved: {ex.Message}");
                        }
                }

                public TEntity Update(TEntity entity)
                {
                        if (entity == null) {
                                throw new ArgumentNullException($"{nameof(Update)} entity must not be null");
                        }

                        try{
                                _context.Update(entity);
                                _context.SaveChanges();
                                  return entity;
                        }
                        catch (Exception ex) {
                                throw new Exception($"{nameof(entity)} could not be updated: {ex.Message}");
                        }
                }
                public  TEntity Delete( TEntity entity ){
                        if ( entity == null ) {
                                throw new ArgumentNullException($"{nameof(Delete)} entity must not be null");
                        }
                        try{
                                _context.Remove(entity);
                                _context.SaveChanges();
                                 return entity;
                        }
                        catch ( Exception e ) {
                                throw new Exception($"{nameof(entity)} could not be deleted: {e.Message}");
                        }
                }
                public IQueryable<TEntity> GetMany(Expression<Func<TEntity, bool>> where = null, Expression<Func<TEntity, bool>> orderBy = null ){

                        IQueryable<TEntity> Query = null ;
                        if ( where != null ){
                                try{
                                        Query =  _context.Set<TEntity>().Where(where);
                                }
                                catch (Exception ex) {
                                        throw new Exception($"Couldn't retrieve entities: {ex.Message}");
                                }
                        }
                        if ( orderBy != null ){
                                try{
                                        Query = _context.Set<TEntity>().OrderBy(orderBy);
                                }
                                catch ( Exception ex ){
                                        throw new Exception("Couldn't retrieve entities: {ex.Message}");
                                }
                        }
                        return Query;
                }
                public void DeleteMany( Expression<Func<TEntity, bool>> where ){
                        IQueryable<TEntity> Query = null ;
                        try{
                                Query =  _context.Set<TEntity>().Where(where);
                                foreach ( var entity in Query ){
                                        _context.Remove(entity);
                                }
                        }
                        catch (Exception ex) {
                                throw new Exception($"Couldn't retrieve entities: {ex.Message}");
                        }
                }
        }
}