using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UserStore.WEB.Models
{
    public class LoginModel
    {
        [Required]        
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        //public bool RememberMe { get; set; }


        [Display(Name = "Я не робот")]
        public string Captcha { get; set; }
    }

    public class RegisterModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]        
        [Display(Name = "Повторите пароль")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        
        public string Address { get; set; }

        [Required]
        [Display(Name = "Имя")]
        public string Name { get; set; }
    }


        
}