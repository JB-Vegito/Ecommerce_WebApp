using System;
using System.Collections.Generic;
using System.Linq;
using ecommerce_web.Repository.IRepository;
using ecommerce_web.Models;
using ecommerce_web.Data;

namespace ecommerce_web.Repository{
    public class InventoryRepository:Repository<Inventory>, IInventoryRepository{
        private ApplicationDbContext _context;
        public InventoryRepository(ApplicationDbContext context):base(context){
            _context = context;
        }

        public void Update(Inventory inventory)
        {
            Console.WriteLine("Reached product update");
            Console.WriteLine(inventory.Id);
            var updateProduct = _context.Inventories.FirstOrDefault(u => u.Id == inventory.Id);
            if(updateProduct != null){
                updateProduct.Name = inventory.Name;
                updateProduct.FullName = inventory.FullName;
                updateProduct.Description = inventory.Description;
                updateProduct.Price = inventory.Price;
                updateProduct.Quantity = inventory.Quantity;
                updateProduct.CategoryId = inventory.CategoryId;

                if(updateProduct.Image != null){
                    updateProduct.Image = inventory.Image;
                }
            }

        }
    }
}