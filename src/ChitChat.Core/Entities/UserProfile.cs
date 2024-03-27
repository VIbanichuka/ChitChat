using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChitChat.Core.Entities
{
    public class UserProfile
    {
        [Key]
        public Guid UserProfileId { get; set; }

        public Guid UserId { get; set; }

        public string? ProfilePicture { get; set; } = string.Empty;

        [Display(Name = "Bio")]
        [MaxLength(300)]
        public string? Bio { get; set; }

        [Display(Name = "First Name")]
        [MaxLength(50)]
        public string? FirstName { get; set; }

        [Display(Name = "Last Name")]
        [MaxLength(50)]
        public string? LastName { get; set; }

        public virtual User User { get; set; }
    }
}
