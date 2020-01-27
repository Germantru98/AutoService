using Microsoft.AspNet.Identity.EntityFramework;

namespace AutoService.DAL.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ClientProfile ClientProfile { get; set; }
    }
}