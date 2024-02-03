using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChitChat.Core.Entities
{
    public class Channel
    {
        public Channel()
        {
            Users = new HashSet<User>();
            Messages = new HashSet<Message>();
        }

        [Key]
        public int ChannelId { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Channel")]
        public string? ChannelName { get; set; }

        public virtual ICollection<User> Users { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
    }
}
