using System;
using System.Collections.Generic;
using System.Linq;
using ecommerce_web.Repository.IRepository;
using ecommerce_web.Models;
using ecommerce_web.Data;

namespace ecommerce_web.Repository{
    public class OrdersRepository:Repository<Orders>, IOrdersRepository{
        private ApplicationDbContext _context;
        public OrdersRepository(ApplicationDbContext context):base(context){
            _context = context;
        }

        public void Update(Orders order)
        {
            _context.Orders.Update(order);
        }
    }
}