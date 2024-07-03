namespace Exciting.TeamModel;

public record TeamMember(
    int Id = default,
    string? FirstName = default,
    string? LastName = default,
    string? Nickname = default,
    string? Bio = default,
    byte[]? ProfilePicture = default,
    IList<TaskItem>? Tasks = default);
