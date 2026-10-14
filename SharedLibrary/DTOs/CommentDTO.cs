using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.DTOs
{
    public class CommentDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CommentId { get; set; }

        public DateTime CommentTimestamp { get; set; } = DateTime.UtcNow;

        [Required]
        public string? CommentText { get; set; }

        public int TicketId { get; set; }

        public string AgentId { get; set; } = string.Empty!;
    }
}
