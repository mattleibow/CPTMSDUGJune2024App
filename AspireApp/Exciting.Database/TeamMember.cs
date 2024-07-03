namespace Exciting.Database;

public class TeamMember
{
    public int Id { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public string? Nickname { get; set; }

    public string? Bio { get; set; }

    public byte[]? ProfilePicture { get; set; }

    public List<TaskItem> Tasks { get; } = [];
}
