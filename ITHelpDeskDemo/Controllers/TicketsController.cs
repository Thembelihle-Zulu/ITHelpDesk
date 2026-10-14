using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Data;
using SharedLibrary.Model;
using SharedLibrary.DTOs;
using SharedLibrary.Mappers;
using Microsoft.AspNetCore.Authorization;
using SharedLibrary.Services;
using Microsoft.AspNetCore.Identity;

namespace ITHelpDeskDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ICustomEmailSender _emailSender;
        private readonly UserManager<ApplicationUser> _userManager;

        public TicketsController(AppDbContext context, ICustomEmailSender emailService, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _emailSender = emailService;
            _userManager = userManager;
        }

        // GET: api/Tickets
        [Authorize(Roles = "Agent")]
        [HttpGet("AllTickets")]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTicket()
        {
            var tickets= await _context.Ticket.ToListAsync();
           
            var dtos= tickets.Select(t => TicketMapper.MapNewTicketEntityToDTO(t)).ToList();
            return Ok(dtos);
        }

        // GET: api/Tickets/5
        [Authorize(Roles ="Agent")]
        [HttpGet("{id}")]
        public async Task<ActionResult<TicketDTO>> GetTicket(int id)
        {
            var ticket = await _context.Ticket.FindAsync(id);

            if (ticket == null)
            {
                return NotFound();
            }
            var ticketDto = TicketMapper.MapNewTicketEntityToDTO(ticket);
            return ticketDto;
        }

        // POST: api/Tickets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [AllowAnonymous]
        [HttpPost("createTicket")]
        public async Task<ActionResult<TicketDTO>> CreateTicket(TicketDTO ticketDto)
        {
            
            var ticket = TicketMapper.MapInitialTicketDtoToTicket(ticketDto);
            var agents = await _userManager.GetUsersInRoleAsync("Agent");
            if (ticket.UserId == null)
                return BadRequest(new { message = "Invalid UserId" });
            var ticketSender = await _context.AppUser.FirstOrDefaultAsync(t=>t.Id==ticketDto.UserId);
            try
            {
                await _context.Ticket.AddAsync(ticket);
                await _context.SaveChangesAsync();
                var body = $@"
                                Hello,

                                A new support ticket has been logged on the system.

                                Ticket Details:
                                 Title:{ticketDto.Title}
                                    Description: {ticketDto.Description}
                                    Priority: {ticketDto.Priority}
                                    Category:{ticketDto.Category}
                                    Submitted By: {ticketSender.FullName}
                                    Date Submitted: {ticketDto.CreatedDate}
                                Please log in to the IT Help Desk system to review and respond to the ticket.


                                Best regards,
                                IT Help Desk System
                           ";
                foreach (var agent in agents)
                {
                    await _emailSender.SendEmailAsync(agent.Email, "A new Ticket has been loged", body);
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
            return CreatedAtAction("GetTicket", new { id = ticket.TicketId }, TicketMapper.MapNewTicketEntityToDTO(ticket));
        
        }

        // PUT: api/Tickets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "Admin")]
        [HttpPut("assignTicket/{id}")]
        public async Task<IActionResult> AssignTicket(int id, TicketDTO ticketDto)
        {
            var ticketToBeUpdated = await  _context.Ticket.FirstOrDefaultAsync(i => i.TicketId == id);
            if (ticketToBeUpdated == null)
            {
                return NotFound();
            }
            TicketMapper.MapGeneralUpdateOnDtoToTicketEntity(ticketToBeUpdated, ticketDto);
            _context.SaveChanges();
            return Ok();
        }

        [Authorize]
        [HttpPut("updateStatus/{id}")]
        public async Task<IActionResult> UpdateStatus(int id, TicketDTO ticketDto)
        {
            var ticketToBeUpdated = await _context.Ticket.FirstOrDefaultAsync(i=> i.TicketId==id);
            if(ticketToBeUpdated == null)
            {
                return NotFound();
            }
            TicketMapper.MapFinalUpdatesOnDTOtoEntity(ticketToBeUpdated, ticketDto);
            _context.SaveChanges();
            return Ok();
        }

        // DELETE: api/Tickets/5
        [Authorize]
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            var ticket = await _context.Ticket.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            _context.Ticket.Remove(ticket);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TicketExists(int id)
        {
            return _context.Ticket.Any(e => e.TicketId == id);
        }
    }
}
