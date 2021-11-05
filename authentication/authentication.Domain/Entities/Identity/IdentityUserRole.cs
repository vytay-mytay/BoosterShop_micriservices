using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace authentication.Domain.Entities.Identity
{
    public class ApplicationUserRole : IdentityUserRole<int>
    {
        public virtual ApplicationUser User { get; set; }

        public virtual ApplicationRole Role { get; set; }
    }
}
