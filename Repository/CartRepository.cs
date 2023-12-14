using System;
using System.Collections.Generic;
using System.Linq;
using ecommerce_web.Repository.IRepository;
using ecommerce_web.Models;
using ecommerce_web.Data;

namespace ecommerce_web.Repository{
    public class CartRepository:Repository<Cart>, ICartRepository{
        private ApplicationDbContext _context;
        public CartRepository(ApplicationDbContext context):base(context){
            _context = context;
        }

        public void Update(Cart cart)
        {
            _context.Carts.Update(cart);
        }
    }
}