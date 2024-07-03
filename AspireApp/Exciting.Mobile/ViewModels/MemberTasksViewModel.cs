using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Exciting.TeamClient;

namespace Exciting.Mobile;

[QueryProperty(nameof(Member), "member")]
public partial class MemberTasksViewModel(TeamApiClient api) : ObservableObject
{
    [ObservableProperty]
    TeamMemberViewModel? member;

    [RelayCommand]
    private async Task SaveChanges()
    {
        await Task.Delay(1000); // Simulate saving changes
    }

    [RelayCommand]
    private void RevertChanges()
    {
        Member?.RevertChanges();
    }
}
