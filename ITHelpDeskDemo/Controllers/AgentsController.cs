//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using SharedLibrary.Data;
//using SharedLibrary.Model;
//using SharedLibrary.DTOs;
//using SharedLibrary.Mappers;
//using Microsoft.AspNetCore.Authorization;

//namespace ITHelpDeskDemo.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class AgentsController : ControllerBase
//    {
//        private readonly AppDbContext _context;

//        public AgentsController(AppDbContext context)
//        {
//            _context = context;
//        }

//        // GET: api/Agents
//        [HttpGet]
//        [Authorize(AuthenticationSchemes = "Bearer")]
//        public async Task<ActionResult<IEnumerable<AgentDTO>>> GetAgent()
//        {
//            //Agent agent=AgentMapper.DtoToEntity
//            var agent= _context.Agent.Include(t=> t.Tickets).ToList();
//            var agentdto =  agent.Select(a => AgentMapper.EntityToDTO(a)).ToList();
//            return agentdto;
//        }

//        // GET: api/Agents/5
//        [Authorize]
//        [HttpGet("{id}")]
//        public async Task<ActionResult<AgentDTO>> GetAgent(int id)
//        {
            
//            var agent = await _context.Agent.Include(t => t.Tickets).FirstOrDefaultAsync(a=> a.AgentId==id);
//            var agentDto = AgentMapper.EntityToDTO(agent);
//            if (agentDto == null)
//            {
//                return NotFound();
//            }

//            return Ok(agentDto);
//        }

//        // PUT: api/Agents/5
//        // For the following method, the mapping has to be done manually not with a Mapper like the other endpoint
//        // The reason for that is that simply mapping the attributes of the dto to an actual entity won't 
//        // update the relationships between the current table and other tables that are related to it
//        // if you have an updated entity, it only has the new details without the previous details
//        // EF takes this as a new entity, the previous relationships might be deleted leading to alot of
//        // inconsistencies

//        //              **HOW TO PROPERLY UPDATE AN AGENT ENTITY**
//        // => Fetch the existing entity with a matching id to the one that is being updated.
//        // => if it is not found that return NotFound
//        // => Update the scalar properties
//        // => Remove the old relationships(we remove them so the list

//        [Authorize(AuthenticationSchemes = "Bearer")]
//        [HttpPut("{id}")]
//        public async Task<IActionResult> PutAgent(int id, AgentDTO agentDto)
//        {
//            if (id != agentDto.AgentId)
//            {
//                return BadRequest();
//            }

//            var existingAgent = await _context.Agent.Include(t => t.Tickets).FirstOrDefaultAsync(a=>a.AgentId==id);
//            existingAgent.FullName = agentDto.FullName;
//            existingAgent.EmailAddress = agentDto.EmailAddress;

//            try
//            {
//                await _context.SaveChangesAsync();
//            }
//            catch (DbUpdateConcurrencyException)
//            {  
//                if (!AgentExists(id))
//                {
//                    return NotFound();
//                }
//                else
//                {
//                    throw;
//                }
//            }

//            return NoContent();
//        }

//        // POST: api/Agents
//        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

//        [Authorize(AuthenticationSchemes = "Bearer")]
//        [HttpPost]
//        public async Task<ActionResult<AgentDTO>> PostAgent(AgentDTO agentDto)
//        {
//            var agent = AgentMapper.DtoToEntity(agentDto);
//            _context.Agent.Add(agent);
//            await _context.SaveChangesAsync();

//            return CreatedAtAction("GetAgent", new { id = AgentMapper.EntityToDTO(agent).AgentId }, AgentMapper.EntityToDTO(agent));
//        }

//        // DELETE: api/Agents/5
//        [Authorize(AuthenticationSchemes = "Bearer")]
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteAgent(int id)
//        {
//            var agent = await _context.Agent.FindAsync(id);
//            if (agent == null)
//            {
//                return NotFound();
//            }

//            _context.Agent.Remove(agent);
//            await _context.SaveChangesAsync();

//            return NoContent();
//        }

//        private bool AgentExists(int id)
//        {
//            return _context.Agent.Any(e => e.AgentId == id);
//        }
//    }
//}
