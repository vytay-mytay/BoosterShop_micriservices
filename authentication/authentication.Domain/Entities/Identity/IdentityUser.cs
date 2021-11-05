using authentication.Common.Extensions;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace authentication.Domain.Entities.Identity
{
    public partial class ApplicationUser : IdentityUser<int>, IEntity
    {
        #region Properties

        [Key]
        public override int Id { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; }

        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        [DataType("DateTime")]
        public DateTime RegistratedAt { get; set; }

        [DataType("DateTime")]
        public DateTime? LastVisitAt { get; set; }

        #endregion

        #region Navigation Properties

        [InverseProperty("User")]
        public virtual ICollection<UserToken> Tokens { get; set; }

        public ICollection<ApplicationUserRole> UserRoles { get; set; }

        #endregion

        #region Ctors

        public ApplicationUser()
        {
            Tokens = Tokens.Empty();
            UserRoles = UserRoles.Empty();
        }

        #endregion
    }
}