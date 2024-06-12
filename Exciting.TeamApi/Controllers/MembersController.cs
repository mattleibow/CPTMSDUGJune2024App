using Exciting.TeamModel;
using Microsoft.AspNetCore.Mvc;

namespace Exciting.TeamApi;

[Route("members")]
[ApiController]
public class MembersController : ControllerBase
{
    static readonly string[] summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    // GET: /members
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TeamMember>>> GetMembers()
    {
        // simulate a real thing hitting the DB
        await Task.Delay(500);

        var forecast =  Enumerable
            .Range(1, 7)
            .Select(index =>
                new TeamMember
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();

        return forecast;
    }


    // private readonly TicketContext _context;

    // public SupportTicketsController(TicketContext context)
    // {
    //     _context = context;
    // }

    // // GET: api/SupportTickets
    // [HttpGet]
    // public async Task<ActionResult<IEnumerable<SupportTicket>>> GetTickets()
    // {
    //     return await _context.Tickets.ToListAsync();
    // }

    // // GET: api/SupportTickets/5
    // [HttpGet("{id}")]
    // public async Task<ActionResult<SupportTicket>> GetSupportTicket(int id)
    // {
    //     var supportTicket = await _context.Tickets.FindAsync(id);

    //     if (supportTicket == null)
    //     {
    //         return NotFound();
    //     }

    //     return supportTicket;
    // }

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