using CommunityToolkit.Mvvm.ComponentModel;
using Exciting.TeamModel;

namespace Exciting.Mobile;

public partial class TaskItemViewModel(TaskItem task) : ObservableObject
{
    [ObservableProperty]
    string title = task.Title ?? "";

    [ObservableProperty]
    bool isComplete = task.IsComplete;

    [ObservableProperty]
    string? notes = task.Notes;
}
