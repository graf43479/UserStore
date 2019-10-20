using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStore.BLL.Models
{
    //TODO: перевести се viewmodel в presentation layer, создать невалидируемые в BLL, сввязать чере automaapper
    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name ="Пароль")]
        [DataType(DataType.Password)]

        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]

        //TODO: проставить везде кириллический Display и требования 
        public string ConfirmPassword { get; set; }

        [Required]
        public string Code { get; set; }

    }
}
