using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace UserStore.DAL.Entities
{
    public class ClientProfile
    {
        [Key]
        [ForeignKey("AppUser")]
        public string ClientProfileID { get; set; }
        public string Name { get; set; }
        public string Adress { get; set; }
        public virtual AppUser AppUser { get; set; }
    }
}
