using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Exciting.TeamClient;

namespace Exciting.Mobile;

[QueryProperty(nameof(TaskItem), "task")]
public partial class EditTaskViewModel(TeamApiClient api) : ObservableObject
{
    [ObservableProperty]
    TaskItemViewModel? taskItem;

    [RelayCommand]
    private async Task SaveChanges()
    {
        await Task.Delay(1000); // Simulate saving changes
    }

    [RelayCommand]
    private void RevertChanges()
    {
        // TaskItem?.RevertChanges();
    }

    public void UpdateTaskNotes(string? notes)
    {
        if (TaskItem is null)
            return;

        if (string.IsNullOrWhiteSpace(notes))
            TaskItem.Notes = null;
        else
            TaskItem.Notes = notes.Trim();
    }

    public string GetTaskNotes()
    {
        if (TaskItem is null)
            return "";

        return TaskItem.Notes ?? "";
    }
}
