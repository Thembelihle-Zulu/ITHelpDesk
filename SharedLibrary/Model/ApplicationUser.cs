using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace SharedLibrary.Model
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FullName { get; set; } = null!;

        public string Department { get; set; } = null!;

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public ICollection<Ticket> AssignedTickets { get; set; } = new List<Ticket>();

        public ICollection<Ticket> CreatedTickets { get; set; } = new List<Ticket>();
    }
}
