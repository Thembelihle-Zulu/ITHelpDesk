using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SharedLibrary.Model;

public  class Comment
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CommentId { get; set; }

    public DateTime CommentTimestamp { get; set; } = DateTime.UtcNow;

    [Required]
    public string? CommentText { get; set; }

    public int TicketId { get; set; }

    public string AgentId { get; set; } = string.Empty!;

    //Navigation properties
    public ApplicationUser Agent { get; set; } = null!;

    public Ticket Ticket { get; set; } = null!;
}
