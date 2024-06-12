using MyGarden_API.Data;
using System.Linq.Expressions;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace MyGarden_API.Repositories
{
    public class RepositoryDesignPattern<T> : IRepositoryDesignPattern<T> where T : class
    {
        private readonly ApiDbContext _context;

        public RepositoryDesignPattern(ApiDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Create(T entity)
        {
            _context.Set<T>().Add(entity);
            return await _context.SaveChangesAsync() != 0;
        }

        public async Task<bool> Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            return await _context.SaveChangesAsync() != 0;
        }

        public async Task<ICollection<TResult>> GetAll<TResult>(Expression<Func<T, TResult>> selector, Expression<Func<T, bool>> disabledCondition, bool useDisabledCondition)
        {
            if (useDisabledCondition)
            {
                var result = await _context.Set<T>().Where(disabledCondition).Select(selector).ToListAsync();
                return result;
            }
            else
            {
                var result = await _context.Set<T>().Select(selector).ToListAsync();
                return result;
            }
        }

        public async Task<TResult> GetByCondition<TResult>(Expression<Func<T, bool>> condition, Expression<Func<T, TResult>> selector, Expression<Func<T, bool>> disabledCondition, bool useDisabledCondition)
        {
            if (useDisabledCondition)
            {
                var result = await _context.Set<T>().Where(condition).Where(disabledCondition).Select(selector).SingleOrDefaultAsync();
                return result;
            }
            else
            {
                var result = await _context.Set<T>().Where(condition).Select(selector).SingleOrDefaultAsync();
                return result;
            }
        }

        public async Task<ICollection<TResult>> GetListByCondition<TResult>(Expression<Func<T, bool>> condition, Expression<Func<T, TResult>> selector, Expression<Func<T, bool>> disabledCondition, bool useDisabledCondition)
        {

            if (useDisabledCondition)
            {
                var result = await _context.Set<T>().Where(condition).Where(disabledCondition).Select(selector).ToListAsync();
                return result;
            }
            else
            {
                var result = await _context.Set<T>().Where(condition).Select(selector).ToListAsync();
                return result;
            }
        }

        public async Task<bool> Update(T entity)
        {
            _context.Set<T>().Update(entity);
            return await _context.SaveChangesAsync() != 0;
        }
    }
}
