using System.Linq.Expressions;
using System.Collections.Generic;

namespace MyGarden_API.Repositories
{
    public interface IRepositoryDesignPattern<T>
    {
        Task<bool> Create(T entity);
        Task<ICollection<TResult>> GetAll<TResult>(Expression<Func<T, TResult>> selector, Expression<Func<T, bool>> disabledCondition, bool useDisabledCondition);
        Task<TResult> GetByCondition<TResult>(Expression<Func<T, bool>> condition, Expression<Func<T, TResult>> selector, Expression<Func<T, bool>> disabledCondition, bool useDisabledCondition);
        Task<ICollection<TResult>> GetListByCondition<TResult>(Expression<Func<T, bool>> condition, Expression<Func<T, TResult>> selector, Expression<Func<T, bool>> disabledCondition, bool useDisabledCondition);
        Task<bool> Update(T entity);
        Task<bool> Delete(T entity);
    }
}
