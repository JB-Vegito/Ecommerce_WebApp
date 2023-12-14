using System;
using System.Collections.Generic;
using System.Linq;
using ecommerce_web.Models;

namespace ecommerce_web.Repository.IRepository{
    public interface IOrderDetailsRepository : IRepository<OrderDetails>{
        void Update(OrderDetails details);
    }
}