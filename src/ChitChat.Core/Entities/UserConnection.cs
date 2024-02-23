using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ChitChat.Core.Entities
{
    public class UserConnection
    {
        [Key]
        public Guid UserConnectionId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public string? ConnectionId { get; set; }

        [Required]
        public DateTime ConnectedAt { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User? User { get; set; }
    }
}
