using System;
using System.Collections.Generic;
using System.Linq;
using ecommerce_web.Repository.IRepository;
using ecommerce_web.Models;
using ecommerce_web.Data;

namespace ecommerce_web.Repository{
    public class OrderDetailsRepository:Repository<OrderDetails>, IOrderDetailsRepository{
        private ApplicationDbContext _context;
        public OrderDetailsRepository(ApplicationDbContext context):base(context){
            _context = context;
        }

        public void Update(OrderDetails details)
        {
            _context.Shipping.Update(details);
        }
    }
}