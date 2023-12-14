using System;
using System.Collections.Generic;
using System.Linq;
using ecommerce_web.Repository.IRepository;

namespace ecommerce_web.Repository{
    public interface IUnitOfWork{
        ICategoryRepository Category{get;}
        IInventoryRepository Inventory{get;}
        ICartRepository Cart{get;}
        IOrdersRepository Order{get;}
        IOrderDetailsRepository Shipping{get; }
        void Save();
    }
}
