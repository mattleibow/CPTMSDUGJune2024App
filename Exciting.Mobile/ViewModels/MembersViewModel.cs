using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Exciting.TeamClient;

namespace Exciting.Mobile;

public partial class MembersViewModel(TeamApiClient api) : INotifyPropertyChanged
{
    private ICommand? refreshCommand;
    private ICommand? changeLayoutCommand;
    private bool isRefreshing;
    private string errorMessage = "";
    private bool isTilesLayout;

    public ICommand RefreshCommand =>
        refreshCommand ??= new Command(async () => await Refresh());

    public ICommand ChangeLayoutCommand =>
        changeLayoutCommand ??= new Command(() => ChangeLayout());

    public ObservableCollection<TeamMemberViewModel> Members { get; } = [];

    public bool IsRefreshing
    {
        get => isRefreshing;
        set
        {
            isRefreshing = value;
            OnPropertyChanged(nameof(IsRefreshing));
        }
    }

    public string ErrorMessage
    {
        get => errorMessage;
        private set
        {
            errorMessage = value;
            OnPropertyChanged(nameof(ErrorMessage));
        }
    }

    public bool IsTilesLayout
    {
        get => isTilesLayout;
        private set
        {
            isTilesLayout = value;
            OnPropertyChanged(nameof(IsTilesLayout));
        }
    }

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

    private void ChangeLayout()
    {
        IsTilesLayout = !IsTilesLayout;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
