using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStore.DAL.Entities;
using System.Text.RegularExpressions;

namespace UserStore.BLL.Identity
{
    public class CustomUserValidator : IIdentityValidator<AppUser> //для подмены английских сообщений 
    {
        
        public async Task<IdentityResult> ValidateAsync(AppUser item)
        {
            List<string> errors = new List<string>();

            if (String.IsNullOrEmpty(item.UserName.Trim()))
                errors.Add("Вы указали пустое имя.");

          /*  string userNamePattern = @"^[a-zA-Z0-9а-яА-Я]+$";

            if (!Regex.IsMatch(item.UserName, userNamePattern))
                errors.Add("В имени разрешается указывать буквы английского или русского языков, и цифры");*/

            if (errors.Count > 0)
                return IdentityResult.Failed(errors.ToArray());

            return IdentityResult.Success;
        }
    }

    //manager.UserValidator = new CustomUserValidator(manager) //new UserValidator<AppUser>(manager)
    //{
    //    AllowOnlyAlphanumericUserNames = true,
    //    RequireUniqueEmail = true
    //};


    /* public class CustomUserValidator : UserValidator<AppUser>
     {
         public CustomUserValidator(AppUserManager manager) : base(manager)
         {
         }

         public override async Task<IdentityResult> ValidateAsync(AppUser user)
         {
             IdentityResult result = await base.ValidateAsync(user);

             if (user.Email.ToLower().EndsWith("@mail.com"))
             {
                 var errors = result.Errors.ToList();
                 errors.Add("Любой домен почты, отличный от @mail.com запрещен");
                 result = new IdentityResult(errors);
             }
             return result;
         }
     }
     */

}
