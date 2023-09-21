﻿using System.ComponentModel.DataAnnotations;

namespace RaxRot.Blog.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [MinLength(6,ErrorMessage ="Password has to be at least 6 characters")]
        public string Password { get; set; }
        [Required]
        public string? ReturnUrl { get; set; }
        
    }
}
