using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Exciting.TeamModel;

namespace Exciting.Mobile;

public partial class TeamMemberViewModel(TeamMember member) : ObservableObject
{
    const int MaxPlaceholderProfiles = 3;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(FullName))]
    [NotifyPropertyChangedFor(nameof(Initial))]
    string firstName = member.FirstName ?? "";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(FullName))]
    [NotifyPropertyChangedFor(nameof(Initial))]
    string lastName = member.LastName ?? "";

    [ObservableProperty]
    string? nickname = member.Nickname;

    [ObservableProperty]
    string? bio = member.Bio;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ProfilePicture))]
    byte[]? profilePictureBytes = member.ProfilePicture;

    public ObservableCollection<TaskItemViewModel> Tasks { get; private set; } = GetTasks(member.Tasks);

    public string FullName => $"{FirstName} {LastName}";

    public string Initial => FirstName.Length > 0 ? FirstName[..1] : "";

    public ImageSource? ProfilePicture => DecodeImage(ProfilePictureBytes, LastName);

    public void RevertChanges()
    {
        FirstName = member.FirstName ?? "";
        LastName = member.LastName ?? "";
        Nickname = member.Nickname;
        Bio = member.Bio;
        ProfilePictureBytes = member.ProfilePicture;
        Tasks = GetTasks(member.Tasks);
        OnPropertyChanged(nameof(Tasks));
    }

    private static ImageSource? DecodeImage(byte[]? bytes, string initial)
    {
        if (bytes is null || bytes.Length == 0)
        {
            var idx = Math.Abs(initial.GetHashCode()) % MaxPlaceholderProfiles;
            var placeholder = ImageSource.FromFile($"placeholder_profile_{idx}.png");
            return placeholder;
        }

        var source = ImageSource.FromStream(() => new MemoryStream(bytes));

        return source;
    }

    private static ObservableCollection<TaskItemViewModel> GetTasks(IList<TaskItem>? tasks)
    {
        if (tasks is null || tasks.Count == 0)
            return [];

        var vms = new ObservableCollection<TaskItemViewModel>();

        foreach (var task in tasks)
        {
            vms.Add(new TaskItemViewModel(task));
        }

        return vms;
    }
}
