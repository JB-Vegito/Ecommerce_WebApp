using System;
using System.Linq;
using System.Collections.Generic;
using ecommerce_web.Data;
using ecommerce_web.Repository.IRepository;

namespace ecommerce_web.Repository{
    public class UnitOfWork : IUnitOfWork{
        private ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context){
            _context = context;
            Category = new CategoryRepository(_context);
            Inventory = new InventoryRepository(_context);
            Cart = new CartRepository(_context);
            Order = new OrdersRepository(_context);
            Shipping = new OrderDetailsRepository(_context);
        }
        public ICategoryRepository Category{get; private set;}
        public IInventoryRepository Inventory{get; private set;}
        public ICartRepository Cart{get; private set;}
        public IOrdersRepository Order{get; private set;}
        public IOrderDetailsRepository Shipping{get; private set;}

        public void Save(){
            _context.SaveChanges();
        }
    }
}