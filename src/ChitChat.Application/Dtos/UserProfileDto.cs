using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ChitChat.Application.Dtos
{
    public class UserProfileDto
    {
        public Guid UserProfileId { get; set; }

        public Guid UserId { get; set; }

        public string? ProfilePicture { get; set; }

        public string? Bio { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public IFormFile ImageFile { get; set; }
    }
}
