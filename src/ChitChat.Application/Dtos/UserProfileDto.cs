using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ChitChat.Application.Dtos
{
    public class UserProfileDto
    {
        [Key]
        public Guid UserProfileId { get; set; } = Guid.NewGuid();

        public Guid UserId { get; set; }

        public string? ProfilePicture { get; set; }

        public string? Bio { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public IFormFile ImageFile { get; set; }
    }
}
