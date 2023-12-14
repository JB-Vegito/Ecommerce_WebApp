using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ecommerce_web.Models{
    public class Cart{
        [Key]
        public int Cart_Id {get; set;}
        [Required]
        public string? Username{get; set;}
        [ForeignKey("Username")]
        public Users Users{get; set;}
        [Required]
        public int Product_Id {get; set;}
        [ForeignKey("Product_Id")]
        public Inventory Inventory {get; set;}
        [Required]
        public int Item_Quantity {get; set;}
    }
}