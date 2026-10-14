using SharedLibrary.Model;
using SharedLibrary.DTOs;
namespace SharedLibrary.Mappers
{
    public static class TicketMapper
    {

        public static TicketDTO MapNewTicketEntityToDTO(Ticket ticket)
        {
            return new TicketDTO
            {
                TicketId = ticket.TicketId,
                Title = ticket.Title,
                Description = ticket.Description,
                CreatedDate = ticket.CreatedDate,
                Priority = ticket.Priority ?? "Low",
                Status = ticket.Status ?? "Open",
                UserId = ticket.UserId,
                //AgentId = ticket.AgentId ?? ,
                Category=ticket.Category
                
            };
        }


        public static Ticket MapInitialTicketDtoToTicket(TicketDTO ticketDto)
        {
            return new Ticket{
                TicketId=ticketDto.TicketId,
                Title = ticketDto.Title,
                Description = ticketDto.Description,
                CreatedDate = ticketDto.CreatedDate,
                Priority = ticketDto.Priority ?? "Low",
                Status = ticketDto.Status ?? "Open",
                UserId = ticketDto.UserId,
                Category=ticketDto.Category,
                AgentId = ticketDto.AgentId
            };
        }

        public static void MapGeneralUpdateOnDtoToTicketEntity(Ticket ticket,TicketDTO ticketDto)
        {
            ticket.Priority = ticketDto.Priority;
            ticket.AgentId = ticketDto.AgentId;
            ticket.Category = ticketDto.Category;
        }



        public static void MapFinalUpdatesOnDTOtoEntity(Ticket ticket, TicketDTO ticketDTO)
        {
            ticket.Status = ticketDTO.Status;
            ticket.DateClosed=ticketDTO.DateClosed;
        }
    }
}
