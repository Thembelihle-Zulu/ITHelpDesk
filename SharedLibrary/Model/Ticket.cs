using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.Model
{
    public  class Ticket
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TicketId { get; set; }

        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string Category { get; set; } = null!;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime? DateClosed { get; set; }

        public string UserId { get; set; } = string.Empty!;

        public string? AgentId { get; set; } 
        public string? Priority { get; set; }

        public string? Status { get; set; }

        public ApplicationUser Agent { get; set; } = null!;

        public Comment? Comment { get; set; }

        public ApplicationUser User { get; set; } = null!;
    }

    public enum Status
    {
        Open,
        InProgress,
        Closed
    }

    public enum Priority
    {
        Low,
        Medium,
        High
    }

    public enum Category
    {
        Software,
        Hardware
    }
}
