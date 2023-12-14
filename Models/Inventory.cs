using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ecommerce_web.Models{
    public class Inventory{
        [Key]
        public int Id {get; set;}
        [Required]
        public string? Name {get; set;}
        [Required]
        [Display(Name="Full Name")]
        public string? FullName {get; set;}
        [Required]
        public string? Description {get; set;}
        [Required]
        public int? Price {get; set;}
        [ValidateNever]
        public string? Image {get; set;}
        [Required]
        public int Quantity {get; set;}
        public string? CategoryId {get; set;}
        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category Category {get; set;}
    }
}