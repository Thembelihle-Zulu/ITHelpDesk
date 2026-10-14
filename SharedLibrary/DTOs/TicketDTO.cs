using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
namespace SharedLibrary.DTOs
{
    public class TicketDTO
    {
        [Key]
        public int TicketId { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; } = null!;

        [JsonPropertyName("description")]
        public string Description { get; set; } = null!;

        [JsonPropertyName("category")]
        public string Category { get; set; } = null!;

        [JsonPropertyName("createdDate")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [JsonPropertyName("dateClosed")]
        public DateTime? DateClosed { get; set; }

        [JsonPropertyName("userId")]
        public string UserId { get; set; } =string.Empty!;

        [JsonPropertyName("agentId")]
        public string? AgentId { get; set; }

        [JsonPropertyName("priority")]
        public string Priority { get; set; } = null!;

        [JsonPropertyName("status")]
        public string Status { get; set; } = null!;

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
