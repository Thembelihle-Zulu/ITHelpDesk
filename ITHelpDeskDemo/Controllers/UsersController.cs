using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Data;
using SharedLibrary.DTOs;
using SharedLibrary.Model;
using SharedLibrary.Mappers;
using Microsoft.AspNetCore.Authorization;

namespace ITHelpDeskDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [Authorize(Roles = "Agent")]
        [HttpGet("All")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUser()
        {
            var user = _context.AppUser.Include(t => t.AssignedTickets).ToList();
            var userdto = user.Select(a => UserMapper.EntityToDto(a)).ToList();
            return userdto;
        }

        // GET: api/Users/5
        [Authorize(Roles = "Agent")]
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(string id)
        {
            var user = await _context.AppUser.Include(t => t.AssignedTickets).FirstOrDefaultAsync(a => a.Id == id);
            var userDto = UserMapper.EntityToDto(user);
            if (userDto == null)
            {
                return NotFound();
            }

            return Ok(userDto);
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "Agent")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(string id, ApplicationUser user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "Agent")]
        [HttpPost]
        public async Task<ActionResult<UserDTO>> PostUser(UserDTO userDto)
        {
            var user = UserMapper.DtoToEntity(userDto);
            _context.AppUser.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id =user.Id}, UserMapper.EntityToDto(user));
        }

        // DELETE: api/Users/5
        [Authorize(Roles = "Agent")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.AppUser.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.AppUser.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(string id)
        {
            return _context.AppUser.Any(e => e.Id == id);
        }
    }
}
