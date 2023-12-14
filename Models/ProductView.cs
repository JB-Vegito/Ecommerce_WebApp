using System;

namespace ecommerce_web.Models{
    public class ProductView{
        public IEnumerable<Category> Categories{get; set;}
        public IEnumerable<Inventory>? Smartphones{get; set;}
        public IEnumerable<Inventory>? Cameras{get; set;}
        public IEnumerable<Inventory>? Laptops{get; set;}
        public IEnumerable<Inventory>? Headphones{get; set;}
    }
}

