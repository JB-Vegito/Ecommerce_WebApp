using System;
using System.Collections.Generic;
using System.Linq;
using ecommerce_web.Repository.IRepository;
using ecommerce_web.Models;
using ecommerce_web.Data;

namespace ecommerce_web.Repository{
    public class CategoryRepository:Repository<Category>, ICategoryRepository{
        private ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context):base(context){
            _context = context;
        }

        public void Update(Category category)
        {
            _context.Categories.Update(category);
        }
    }
}