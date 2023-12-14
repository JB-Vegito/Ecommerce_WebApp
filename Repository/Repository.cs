using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ecommerce_web.Repository.IRepository;
using ecommerce_web.Data;

namespace ecommerce_web.Repository{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        internal DbSet<T> dbSet;

        public Repository(ApplicationDbContext context){
            _context=context;
            // _context.Products.Include(u => u.Category);
            this.dbSet = _context.Set<T>();
        }
        public void Add(T entity)
        {
            Console.WriteLine("Reached add method");
            dbSet.Add(entity);
        }

        public IEnumerable<T> GetAll(string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if(includeProperties != null){
                foreach(var obj in includeProperties.Split(new char[]{','}, StringSplitOptions.RemoveEmptyEntries)){
                    query = query.Include(obj);
                }
            }
            return query.ToList();
        }

        public IEnumerable<T> GetSome(Expression<Func<T,bool>> filter, string? includeProperties=null){
            IQueryable<T> query = dbSet;
            query = query.Where(filter);
            if(includeProperties != null){
                foreach(var obj in includeProperties.Split(new char[]{','}, StringSplitOptions.RemoveEmptyEntries)){
                    query = query.Include(obj);
                }
            }  
            return query.ToList();          
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            query = query.Where(filter);
            if(includeProperties != null){
                foreach(var obj in includeProperties.Split(new char[]{','}, StringSplitOptions.RemoveEmptyEntries)){
                    query = query.Include(obj);
                }
            }            
            return query.FirstOrDefault();
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity);
        }
    }
}