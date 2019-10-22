using System.ComponentModel.DataAnnotations;


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
        [Display(Name = "Повторите пароль")]        
        public string ConfirmPassword { get; set; }

        [Required]        
        public string Code { get; set; }

    }
}
