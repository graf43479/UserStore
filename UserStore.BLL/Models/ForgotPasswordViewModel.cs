using System.ComponentModel.DataAnnotations;

namespace UserStore.BLL.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]        
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
