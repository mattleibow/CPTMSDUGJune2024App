using Exciting.Database;
using Microsoft.AspNetCore.Mvc;

using TeamMemberDto = Exciting.TeamModel.TeamMember;
using TaskItemDto = Exciting.TeamModel.TaskItem;
using Microsoft.EntityFrameworkCore;

namespace Exciting.TeamApi;

[Route("members")]
[ApiController]
public class MembersController(ExcitingDbContext dbContext) : ControllerBase
{
    // GET: /members
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TeamMemberDto>>> GetMembers()
    {
        var dtos = new List<TeamMemberDto>();

        var query = dbContext
            .Members
            .Include(m => m.Tasks)
            .AsAsyncEnumerable();

        await foreach (var entity in query)
        {
            var dto = GetDto(entity);
            dtos.Add(dto);
        }

        return dtos;
    }

    // GET: /members/1
    [HttpGet("{id}")]
    public async Task<ActionResult<TeamMemberDto>> GetMembers(int id)
    {
        var query = dbContext
            .Members
            .Include(m => m.Tasks)
            .FirstOrDefaultAsync(m => m.Id == id);

        var entity = await query;

        if (entity is null)
            return NotFound();

        var dto = GetDto(entity);

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

    private static TeamMemberDto GetDto(TeamMember entity) =>
        new()
        {
            Id = entity.Id,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Nickname = entity.Nickname,
            Bio = entity.Bio,
            ProfilePicture = entity.ProfilePicture,
            Tasks = GetTaskDtos(entity)
        };

    private static List<TaskItemDto> GetTaskDtos(TeamMember entity)
    {
        var tasks = new List<TaskItemDto>();

        foreach (var task in entity.Tasks)
        {
            var taskDto = GetTaskDto(task);
            tasks.Add(taskDto);
        }

        return tasks;
    }

    private static TaskItemDto GetTaskDto(TaskItem task) => 
        new()
        {
            Id = task.Id,
            Title = task.Title,
            Notes = task.Notes,
            IsComplete = task.IsComplete,
            TeamMemberId = task.TeamMemberId
        };
}