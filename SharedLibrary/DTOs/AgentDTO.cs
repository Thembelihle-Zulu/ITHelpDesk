using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace SharedLibrary.DTOs
{
    public  class AgentDTO
    {
        [Key]
        public string AgentId { get; set; } = string.Empty!;

        [Required]
        public string FullName { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; } = null!;

        public ICollection<TicketDTO> Tickets { get; set; } = new List<TicketDTO>();
    }
}
