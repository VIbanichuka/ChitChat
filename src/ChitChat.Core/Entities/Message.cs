using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChitChat.Core.Entities
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }

        [Required]
        [MaxLength(1000)]
        [Display(Name = "Content")]
        public string? Content { get; set; }

        public DateTime Timestamp { get; set; }

        public int SenderId { get; set; }

        public int ReceiverId { get; set; }

        public int ChannelId { get; set; }
        public virtual Channel? Channel { get; set; }

        [ForeignKey("SenderId")]
        public virtual User? Sender { get; set; }

        [ForeignKey("ReceiverId")]
        public virtual User? Receiver { get; set; }
    }
}
