using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Data;
using SharedLibrary.Mappers;
using SharedLibrary.Model;
namespace ITHelpDeskDemo.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="SuperAdmin, Admin, Agent")]
    public class AppUserController : ControllerBase
    {
        private AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager; 
        public AppUserController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager= userManager;
        }

        [HttpGet("Users")]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetUsers()
        {
            var users= await _context.AppUser.Include(t=>t.CreatedTickets).ToListAsync();
            var userDto=users.Select(t=>UserMapper.EntityToDto(t)).ToList();
            return Ok(userDto);
        }

        [HttpGet("Agents")]

        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetAgents()
        {
            var agents =await _userManager.GetUsersInRoleAsync("Agent");
            var agentDtos = agents.Select(t => AgentMapper.EntityToDTO(t)).ToList();
            return Ok(agentDtos);
        }

        [HttpGet("agent/{id}")]
        public async Task<ActionResult<ApplicationUser>> GetAgent(string id)
        {
            var agents = await _userManager.GetUsersInRoleAsync("Agent");
            var agent = agents.FirstOrDefault(t=>t.Id==id);
            if (agent == null)
            {
                return NotFound(agent);
            }

            return Ok(agent);
        }


        [HttpPut("AllocateRole/{id}")]
        public async Task<ActionResult<ApplicationUser>> AlllocateRole(string id)
        {
            var user = await _context.AppUser.FirstOrDefaultAsync(u=> u.Id==id);
            if (user == null)
            {
                return NotFound($"Agent with id:  {user.Id} was not Found");
            }
            //var roles =await  _userManager.GetRolesAsync(user);
            if (await _userManager.IsInRoleAsync(user, "Agent"))
            {
                return Ok(new { message = "Agent already assigned a role", user});
            }
            
            var result= await _userManager.AddToRoleAsync(user, "Agent");

            if (!result.Succeeded)
            {
                return BadRequest("Failed to add agent to role");
            }
            
            return Ok(new { message = "Agent Added Successfully", user });
        }

       

    }
}
