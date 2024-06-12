using MyGarden_API.Data;
using System.Linq.Expressions;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MyGarden_API.Repositories
{
    public class RepositoryDesignPattern<T> : IRepositoryDesignPattern<T> where T : class
    {
        private readonly ApiDbContext _context;
        private readonly IMapper _mapper;

        public RepositoryDesignPattern(ApiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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

        public async Task<ICollection<TResult>> GetAll<TResult>(Expression<Func<T, bool>> disabledCondition, bool useDisabledCondition)
        {
            if (useDisabledCondition)
            {
                var result = await _context.Set<T>().Where(disabledCondition).ToListAsync();
                return _mapper.Map<ICollection<TResult>> (result);
            }
            else
            {
                var result = await _context.Set<T>().ToListAsync();
                return _mapper.Map<ICollection<TResult>>(result);
            }
        }

        public async Task<TResult> GetByCondition<TResult>(Expression<Func<T, bool>> condition, Expression<Func<T, bool>> disabledCondition, bool useDisabledCondition)
        {
            if (useDisabledCondition)
            {
                var result = await _context.Set<T>().Where(condition).Where(disabledCondition).SingleOrDefaultAsync();
                return _mapper.Map<TResult>(result);
            }
            else
            {
                var result = await _context.Set<T>().Where(condition).SingleOrDefaultAsync();
                return _mapper.Map<TResult>(result);
            }
        }

        public async Task<ICollection<TResult>> GetListByCondition<TResult>(Expression<Func<T, bool>> condition, Expression<Func<T, bool>> disabledCondition, bool useDisabledCondition)
        {

            if (useDisabledCondition)
            {
                var result = await _context.Set<T>().Where(condition).Where(disabledCondition).ToListAsync();
                return _mapper.Map<ICollection<TResult>>(result);
            }
            else
            {
                var result = await _context.Set<T>().Where(condition).ToListAsync();
                return _mapper.Map<ICollection<TResult>>(result);
            }
        }

        public async Task<bool> Update(T entity)
        {
            _context.Set<T>().Update(entity);
            return await _context.SaveChangesAsync() != 0;
        }
    }
}
