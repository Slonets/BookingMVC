using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using DataAccess.Data;
using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        internal BookingDbContext _context;
        internal DbSet<TEntity> dbSet;

        public Repository(BookingDbContext context)
        {
            _context = context;
            dbSet = context.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> GetAsync(
        Expression<Func<TEntity, bool>> filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        params string[] includeProperties)
        {
            IQueryable<TEntity> query = dbSet;
            await Task.Run
                (
                    () =>
                    {
                        if (filter != null)
                        {
                            query = query.Where(filter);
                        }

                        foreach (var includeProperty in includeProperties)
                        {
                            query = query.Include(includeProperty);
                        }
                    });
            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }
        public async Task<TEntity> GetByIDAsync(object id)
        {
            return await dbSet.FindAsync(id);
        }
        public async Task InsertAsync(TEntity entity)
        {
            await dbSet.AddAsync(entity);
        }
        public async Task DeleteAsync(object id)
        {
            TEntity entityToDelete = await dbSet.FindAsync(id);
            await DeleteAsync(entityToDelete);
        }
        public async Task DeleteAsync(TEntity entityToDelete)
        {
            await Task.Run
                (
                    () =>
                    {
                        if (_context.Entry(entityToDelete).State == EntityState.Detached)
                        {
                            dbSet.Attach(entityToDelete);
                        }
                        dbSet.Remove(entityToDelete);
                    });
        }
        public async Task UpdateAsync(TEntity entityToUpdate)
        {
            await Task.Run
                (
                () =>
                {
                    dbSet.Attach(entityToUpdate);
                    _context.Entry(entityToUpdate).State = EntityState.Modified;
                });
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        // working with specifications

        //Отримати все за відповідними правилами специфікації, які будуть передані
        public async Task<IEnumerable<TEntity>> GetListBySpec(ISpecification<TEntity> specification)
        {
            return await ApplySpecification(specification).ToListAsync();
        }

        //Отримати елемент
        public async Task<TEntity?> GetItemBySpec(ISpecification<TEntity> specification)
        {
            return await ApplySpecification(specification).FirstOrDefaultAsync();
        }

        //у приватний метод винесений інтерфейс специфікації

        //IQueryable це колекція сутності. Різниця від IEnumerable, що повністю будується запит вибірки
        //IEnumerable повертає повністю колекцію обєктів, а IQueryable повертає уже обєкти з вибіркою
        // select... from... join... join... where.. order by
        private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> specification)
        {
            var evaluator = new SpecificationEvaluator(); //
            return evaluator.GetQuery(dbSet, specification);
        }
    }
}
