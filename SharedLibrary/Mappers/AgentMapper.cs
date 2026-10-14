using SharedLibrary.DTOs;
using SharedLibrary.Model;
namespace SharedLibrary.Mappers
{
    public static class AgentMapper
    {
        public static AgentDTO EntityToDTO(ApplicationUser agent)
        {
            return new AgentDTO
            {
                AgentId = agent.Id,
                FullName = agent.FullName,
                EmailAddress = agent.Email,
                Tickets = agent.AssignedTickets.Select(t=> new TicketDTO
                {
                    TicketId = t.TicketId
                }).ToList()
            };
        }

        public static ApplicationUser DtoToEntity(AgentDTO agentDto)
        {
            return new ApplicationUser
            {
                Id = agentDto.AgentId,
                FullName = agentDto.FullName,
                Email = agentDto.EmailAddress,
                AssignedTickets = agentDto.Tickets.Select(t => new Ticket
                {
                    TicketId = t.TicketId
                }).ToList()
            };
        }
    }
}
