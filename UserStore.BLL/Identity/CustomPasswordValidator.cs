using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStore.BLL.Identity
{
   public class CustomPasswordValidator : PasswordValidator
    {
        public CustomPasswordValidator()
        {
            RequiredLength = 6;
            RequireNonLetterOrDigit = false;
            RequireDigit = false;
            RequireLowercase = false;
            RequireUppercase = false;           
        }

        public override async Task<IdentityResult> ValidateAsync(string pass)
        {
            IdentityResult result = await base.ValidateAsync(pass);

            if (pass.Contains("QWERTY"))
            {
                var errors = result.Errors.ToList();
                errors.Add("Запрещенная последовательность символов пароля");
                result = new IdentityResult(errors);
            }

            return result;
        }
    }
}

//manager.PasswordValidator = new CustomPasswordValidator() //new PasswordValidator()
//{
//    RequiredLength = 6,
//                RequireNonLetterOrDigit = false,
//                RequireDigit = false,
//                RequireLowercase = true,
//                RequireUppercase = true
//            };


