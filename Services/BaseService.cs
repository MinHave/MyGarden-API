using MyGarden_API.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace MyGarden_API.Services.Interfaces
{
    public class BaseService<T> : IBaseService<T> where T : class
    {
        private readonly IRepositoryDesignPattern<T> _designPattern;

        public BaseService(IRepositoryDesignPattern<T> designPattern)
        {
            _designPattern = designPattern;
        }

        public async Task<bool> Create(T entity)
        {
            var result = await _designPattern.Create(entity);
            return result;
        }

        public async Task<bool> Delete(T entity)
        {
            var result = await _designPattern.Delete(entity);
            return result;
        }

        public async Task<bool> ToggleDisabled(T entity)
        {
            var propertyInfo = entity.GetType().GetProperty("IsDisabled");
            if (propertyInfo == null)
            {
                throw new InvalidOperationException("Entity does not have an 'IsDisabled' property");
            }

            bool currentValue = (bool)propertyInfo.GetValue(entity);
            propertyInfo.SetValue(entity, !currentValue);

            var result = await _designPattern.Update(entity);
            return result;
        }

        public async Task<bool> Update(T entity)
        {
            var result = await _designPattern.Update(entity);
            return result;
        }
    }
}
