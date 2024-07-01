using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Exciting.TeamClient;

namespace Exciting.Mobile;

public partial class MembersViewModel(TeamApiClient api) : ObservableObject
{
    [ObservableProperty]
    private bool isRefreshing;

    [ObservableProperty]
    private string errorMessage = "";

    [ObservableProperty]
    private bool isTilesLayout;

    public ObservableCollection<TeamMemberViewModel> Members { get; } = [];

    [RelayCommand]
    private async Task Refresh()
    {
        try
        {
            ErrorMessage = "";

            var members = await api.GetMembersAsync();

            var viewModels = members
                .Select(m => new TeamMemberViewModel(m))
                .OrderBy(m => m.FullName)
                .ToList();

            Members.Clear();
            foreach (var vm in viewModels)
            {
                Members.Add(vm);
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = "Failed to load members, try again.";
            Console.WriteLine($"Error loading members: {ex}");
        }
        finally
        {
            IsRefreshing = false;
        }
    }

    [RelayCommand]
    private void ChangeLayout()
    {
        IsTilesLayout = !IsTilesLayout;
    }
}
