using Microsoft.Toolkit.Uwp.Notifications;
using System.IO;
using System.Net.Http;

namespace HypixelTracker;
internal static class Helper
{
    // http client instance shared across the entire app. it's thread safe
    public static HttpClient SharedHttp { get; }
    static Helper()
    {
        SharedHttp = new();
        SharedHttp.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; AcmeInc/1.0)");
    }

    public static bool IsMvpPlusPlus(dynamic api)
    {
        var isMvpPlusPlus = (string)api.player.monthlyPackageRank == "SUPERSTAR";
        return isMvpPlusPlus;
    }

    // tracker desktop notification
    public static async Task ShowTrackedNotification(string name)
    {
        var apiLink = $@"https://visage.surgeplay.com/bust/{name}?no=shadow";
        var response = await SharedHttp.GetAsync(apiLink);

        if (!response.IsSuccessStatusCode)
        {
            MessageBox.Show($"Failed fetching image, not showing toast notification.\n\n{name}'s stats changed. They are or have been active a while ago.");
            return;
        }

        var path = Path.Join(Path.GetTempPath(), $"{name.ToLower()}.png");
        using var image = await response.Content.ReadAsStreamAsync();
        using var file = File.Create(path);

        await image.CopyToAsync(file);

        new ToastContentBuilder()
            .AddInlineImage(new Uri(path))
            .AddText("Tracker noticed something!")
            .AddText($"Detected stats change for {name} — this player is likely active now. In the next hour, logs from this player will not cause a pop-up to open.")
            .SetToastDuration(ToastDuration.Short)
        .Show();
    }

    public static void ShowGeneralToast(string title, string message)
    {
        new ToastContentBuilder()
            .AddText(title)
            .AddText(message)
            .SetToastDuration(ToastDuration.Short)
            .Show();
    }
}
