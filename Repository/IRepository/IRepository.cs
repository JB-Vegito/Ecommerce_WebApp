using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ecommerce_web.Repository.IRepository{
    public interface IRepository<T> where T:class{
        T GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null);
        IEnumerable<T> GetAll(string? includeProperties = null);
        IEnumerable<T> GetSome(Expression<Func<T,bool>> filter, string? includeProperties=null);        
        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);
        // void ClearAll(Expression<Func<T,bool>> filter);
    }
}