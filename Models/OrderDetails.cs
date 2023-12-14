using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ecommerce_web.Models{
    public class OrderDetails{
        [Key]
        public int Id {get; set;}
        [Required]
        // [ValidateNever]
        public string OrderId {get;set;}
        // [ForeignKey("OrderId")]
        // public Order order{get; set;}
        public DateTime OrderDate {get; set;}
        [Required]
        public string? Address{get; set;}
        [Required]
        public string? State{get; set;}
        [Required]
        public string? City{get; set;}     
        [Required]
        // [RegularExpression(@"^[1-9][0-9]{5}$",ErrorMessage ="Not a valid pincode")]        
        public string? PinCode{get; set;}        
        [Required]
        public int FinalAmount{get; set;}
        public string? Shipping{get; set;}
    }
}