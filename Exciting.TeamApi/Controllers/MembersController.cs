using Exciting.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using TeamMemberDto = Exciting.TeamModel.TeamMember;

namespace Exciting.TeamApi;

[Route("members")]
[ApiController]
public class MembersController(ExcitingDbContext dbContext) : ControllerBase
{
    // GET: /members
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TeamMemberDto>>> GetMembers()
    {
        var entities = await dbContext.Members.ToArrayAsync();

        var dtos = entities
            .Select(e => new TeamMemberDto(e.Id, e.FirstName, e.LastName))
            .ToArray();

        return dtos;
    }

    // GET: /members/1
    [HttpGet("{id}")]
    public async Task<ActionResult<TeamMemberDto>> GetMembers(int id)
    {
        var entity = await dbContext.Members.FindAsync(id);

        if (entity is null)
            return NotFound();

        var dto = new TeamMemberDto(entity.Id, entity.FirstName, entity.LastName);

        return dto;
    }

    // // PUT: api/SupportTickets/5
    // // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    // [HttpPut("{id}")]
    // public async Task<IActionResult> PutSupportTicket(int id, SupportTicket supportTicket)
    // {
    //     if (id != supportTicket.Id)
    //     {
    //         return BadRequest();
    //     }

    //     _context.Entry(supportTicket).State = EntityState.Modified;

    //     try
    //     {
    //         await _context.SaveChangesAsync();
    //     }
    //     catch (DbUpdateConcurrencyException)
    //     {
    //         if (!SupportTicketExists(id))
    //         {
    //             return NotFound();
    //         }
    //         else
    //         {
    //             throw;
    //         }
    //     }

    //     return NoContent();
    // }

    // // POST: api/SupportTickets
    // // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    // [HttpPost]
    // public async Task<ActionResult<SupportTicket>> PostSupportTicket(SupportTicket supportTicket)
    // {
    //     _context.Tickets.Add(supportTicket);
    //     await _context.SaveChangesAsync();

    //     return CreatedAtAction("GetSupportTicket", new { id = supportTicket.Id }, supportTicket);
    // }

    // // DELETE: api/SupportTickets/5
    // [HttpDelete("{id}")]
    // public async Task<IActionResult> DeleteSupportTicket(int id)
    // {
    //     var supportTicket = await _context.Tickets.FindAsync(id);
    //     if (supportTicket == null)
    //     {
    //         return NotFound();
    //     }

    //     _context.Tickets.Remove(supportTicket);
    //     await _context.SaveChangesAsync();

    //     return NoContent();
    // }

    // private bool SupportTicketExists(int id)
    // {
    //     return _context.Tickets.Any(e => e.Id == id);
    // }
}