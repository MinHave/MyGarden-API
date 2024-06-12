using System.Threading.Tasks;

namespace MyGarden_API.Services.Interfaces
{
    public interface IBaseService<T>
    {
        public Task<bool> Create(T entity);
        public Task<bool> Delete(T entity);
        public Task<bool> ToggleDisabled(T entity);
        public Task<bool> Update(T entity);
    }
}