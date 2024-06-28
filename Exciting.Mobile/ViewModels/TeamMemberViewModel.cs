using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Exciting.TeamModel;
using SkiaSharp;
using SkiaSharp.Views.Maui;

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
    [NotifyPropertyChangedFor(nameof(ProfileColor))]
    byte[]? profilePictureBytes = member.ProfilePicture;

    public ObservableCollection<TaskItemViewModel> Tasks { get; private set; } = GetTasks(member.Tasks);

    public string FullName => $"{FirstName} {LastName}";

    public string Initial => FirstName.Length > 0 ? FirstName[..1] : "";

    public ImageSource? ProfilePicture => DecodeImage(ProfilePictureBytes, LastName);

    public Color ProfileColor => DecodeColor(ProfilePictureBytes);

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

    private static ImageSource? DecodeImage(byte[]? bytes, string fallback)
    {
        if (bytes is null || bytes.Length == 0)
        {
            var idx = Math.Abs(fallback.GetHashCode()) % MaxPlaceholderProfiles;
            var placeholder = ImageSource.FromFile($"placeholder_profile_{idx}.png");
            return placeholder;
        }

        var source = ImageSource.FromStream(() => new MemoryStream(bytes));

        return source;
    }

    private static Color DecodeColor(byte[]? profilePictureBytes)
    {
        if (profilePictureBytes is null || profilePictureBytes.Length == 0)
        {
            return Colors.Black;
        }

        var best = GetBestColor(profilePictureBytes);

        return best.ToMauiColor();
    }

    private static SKColor GetBestColor(byte[] bytes)
    {
        var colors = GetColors(bytes).ToArray();

        var r = colors.Sum(c => c.Red);
        var g = colors.Sum(c => c.Green);
        var b = colors.Sum(c => c.Blue);

        var count = colors.Length;

        var best = new SKColor(
            (byte)(r / count),
            (byte)(g / count),
            (byte)(b / count));

        return best;
    }

    private static IEnumerable<SKColor> GetColors(byte[] bytes)
    {
        using var image = SKImage.FromEncodedData(bytes);
        using var loaded = image.ToRasterImage(true);
        using var pixmap = loaded.PeekPixels();

        for (var x = 0; x < image.Width; x++)
        {
            for (var y = 0; y < image.Height; y++)
            {
                var color = pixmap.GetPixelColor(x, y);
                yield return color;
            }
        }
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
