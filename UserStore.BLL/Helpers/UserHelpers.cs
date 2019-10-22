using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStore.BLL.DTO;
using UserStore.DAL.Entities;

namespace UserStore.BLL.Helpers
{
    public static class UserHelpers
    {
        public static AppUser UserToAppUser(this UserDTO userDTO)
        {
            AppUser appUser = new AppUser();
            appUser.Id = userDTO.Id;
            appUser.Email = userDTO.Email;
            appUser.UserName = userDTO.UserName;
            appUser.ClientProfile.Name = userDTO.Name;
            appUser.ClientProfile.Adress = userDTO.Address;
      
            return appUser;
        }

        public static UserDTO AppUserToUser(this AppUser appUser)
        {
            UserDTO userDTO = new UserDTO();
            userDTO.Id = appUser.Id;
            userDTO.Email = appUser.Email;
            userDTO.UserName = appUser.UserName;
            if (appUser.ClientProfile != null)
            {
                userDTO.Name = appUser.ClientProfile.Name ?? "";
                userDTO.Address = appUser.ClientProfile.Adress ?? "";
            }
            return userDTO;
        }

        public static IEnumerable<UserDTO> UserDTOList(this IEnumerable<AppUser> appUserList)
        {
            List<UserDTO> userDtoList = new List<UserDTO>();

            foreach (AppUser appUser in appUserList)
            {
                userDtoList.Add(appUser.AppUserToUser());
            }
            return userDtoList;
        }

        public static IEnumerable<AppUser> AppUserList(this IEnumerable<UserDTO> userDtoList)
        {
            List<AppUser> appUserList = new List<AppUser>();

            foreach (UserDTO user in userDtoList.ToList())
            {
                appUserList.Add(user.UserToAppUser());
            }
            return appUserList;
        }
    }
}
