using System;
using System.ComponentModel.DataAnnotations;

namespace ecommerce_web.Models{
    public class Category{
        [Key]
        [Display(Name = "Category Id")]
        public string? CategoryId {get; set;}
        [Required]
        [Display(Name = "Category Name")]
        public string? CategoryName {get; set;}
    }
}