using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using authentication.Common.Extensions;
using System.ComponentModel.DataAnnotations.Schema;

namespace authentication.Domain.Entities.Identity
{
    public class ApplicationRole : IdentityRole<int>, IEntity
    {
        public override int Id { get; set; }

        public ICollection<ApplicationUserRole> UserRoles { get; set; }

        public ApplicationRole()
        {
            UserRoles = UserRoles.Empty();
        }
    }
}
