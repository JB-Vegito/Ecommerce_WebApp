using System;
using System.Collections.Generic;
using System.Linq;
using ecommerce_web.Models;

namespace ecommerce_web.Repository.IRepository{
    public interface IInventoryRepository : IRepository<Inventory>{
        void Update(Inventory inventories);
    }
}