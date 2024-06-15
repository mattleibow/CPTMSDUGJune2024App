namespace Exciting.TeamModel;

public record TaskItem(
    int Id = default,
    int TeamMemberId = default,
    string? Title = default,
    bool IsComplete = default,
    string? Notes = default);
