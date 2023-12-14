using System;
using System.ComponentModel.DataAnnotations;

namespace ecommerce_web.Models{
    public class Users{
        [Key]
        [Required(ErrorMessage ="Username is required.")]  
        public string? username {get; set;}
        [Required(ErrorMessage ="Password is required.")]  
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{6,}$",ErrorMessage ="Password length must be minimum 6 characters and must include atleast 1 uppercase, lowercase, special character and a number.")]
        public string? password {get; set;}
    }
}