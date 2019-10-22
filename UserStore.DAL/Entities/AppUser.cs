using Microsoft.AspNet.Identity.EntityFramework;

namespace UserStore.DAL.Entities
{
    public class AppUser : IdentityUser
    {
        public virtual ClientProfile ClientProfile { get; set; }
    }
}
