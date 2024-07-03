namespace Exciting.Database;

public class TaskItem
{
    public int Id { get; set; }

    public required string Title { get; set; }

    public string? Notes { get; set; }

    public bool IsComplete { get; set; }

    public int TeamMemberId { get; set; }

    public TeamMember TeamMember { get; set; } = null!;
}
