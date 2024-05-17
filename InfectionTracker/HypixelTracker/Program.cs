// well known library for converting things like timespans to a human-readable string
using Humanizer;
using Hypixel;
using HypixelTracker;
// to navigate through json as .net doesn't have a built-in way to do that rn
using JsonEasyNavigation;
// alternative to built-in .net's json serializer
using Newtonsoft.Json;
// tiny but powerful library I created to style console output (closed source currently, but is planned to be public)
using Quad;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text.Json;

// btw sorry for any1 actually reading this code... ik its a garbage but it works lol

Console.Title = "Infection Tracker";

if (!File.Exists(HypixelAPI.API_FILE))
{
    Console.WriteLine("No API key set. Since Hypixel requires all API requests to be authorized, you'll need to set an API key.");
    Console.WriteLine();
    Console.WriteLine("1. Register a permanent key or just a development key on https://developer.hypixel.net/dashboard.");
    Console.WriteLine("2. Put your key in this app. Never share it with anyone else.");

    Console.WriteLine();

    while (true)
    {

        Console.Write("Your API key: ".Bold());
        var apikey = Console.ReadLine();

        if (Guid.TryParse(apikey, out _))
        {
            File.WriteAllText(HypixelAPI.API_FILE, apikey);
            break;
        }

        Console.WriteLine("This key doesn't seem to be a Hypixel API key.".Foreground(ConsoleColor.Red));
    }

    Console.WriteLine();
    Console.WriteLine($"""
        This program is {"still in a development".Foreground(ConsoleColor.Magenta)} and may not work as expected!
        It may also get your Hypixel account banned, so be careful.
        """.Foreground(ConsoleColor.White));
    Console.WriteLine();
    Console.WriteLine("Restart the program or press any key to continue . . .");
    Console.ReadKey(true);

    Process.Start(Environment.ProcessPath!);
    return;
}

Helper.ShowGeneralToast("Before you track anyone ...", "My program is violating Hypixel API terms of service, and I don't take any responsibility for your account, so use it at your own risk.");

#region Magic things
Console.Clear();
Console.WriteLine("Infection Tracker by ymqn".Foreground(ConsoleColor.White).Bold());
Console.WriteLine("Works by comparing stats and finding a differences");
Console.WriteLine();

NamesWindow wnd = null!;
Thread thread = new(() =>
{
    wnd = new();
    wnd.ShowDialog();
});

thread.SetApartmentState(ApartmentState.STA);
thread.Start();
thread.Join();

if (wnd is null ||
    !wnd.IsRequestingTracking)
{
    return;
}

// Convert names to UUIDs
Dictionary<string, Guid> uuids = [];
var names = wnd.Names;

Console.WriteLine($"{names.Length} players will be processed");
Console.WriteLine("Converting all names to UUIDs ...");
Console.WriteLine();

int complete = 0;
int errors = 0;

var lck = new object();
void WriteError(string name)
{
    lock (lck)
    {
        errors++;
        Console.WriteLine($"{Environment.NewLine}Couldn't fetch {name.Foreground(ConsoleColor.Red)}".Foreground(ConsoleColor.DarkRed));
    }
}

var tasks = names.ToArray().Select(async name =>
{
    lock (lck)
    {
        Console.Write($"\r{$"{++complete}/{wnd.Names.Length,-10}".Foreground(ConsoleColor.DarkGray)} {name.Bold().Foreground(ConsoleColor.White)}");
        QuadPixel.Clear.ToEndOfLine();
    }

    // had some ratelimit issues with mojang api, so I used playerdb.co instead, which seems to work as expected
    var url = $"https://playerdb.co/api/player/minecraft/{name}";
    bool success = false;

    for (int i = 0; i < 2; i++)
    {
        using HttpResponseMessage response = await Helper.SharedHttp.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            Thread.Sleep(5000);
            continue;
        }

        var json = await response.Content.ReadAsStringAsync();
        var parsed = JsonDocument.Parse(json).RootElement.ToNavigation();
        var dataNode = parsed["data"][0];

        if (!dataNode.Exist)
        {
            Thread.Sleep(1000);
            continue;
        }

        lock (lck)
        {
            var id = dataNode["id"].GetStringOrEmpty();
            var username = dataNode["username"].GetStringOrEmpty();

            uuids.Add(
                username,
                new(id)
                );
        }

        success = true;
        break;
    }

    if (!success)
    {
        WriteError(name);
    }
});

foreach (var task in tasks.Chunk(Environment.ProcessorCount))
{
    await Task.WhenAll(task);
}

Console.Write($"\r{$"{complete}/{wnd.Names.Length,-10}".Foreground(ConsoleColor.DarkGray)} {"Job finished".Bold().Foreground(ConsoleColor.White)}");
QuadPixel.Clear.ToEndOfLine();

Console.WriteLine();
Console.WriteLine();

Console.WriteLine($"Converted to UUIDs with {$"{errors} error(s)".Foreground(ConsoleColor.Red)}");
Console.WriteLine($"Filtering {"MVP++".Bold().Foreground(ConsoleColor.Yellow)} players, that are able to nick ...");
Console.WriteLine();

int completeH = 0;
int errorsH = 0;

ConcurrentBag<string> scheduledRemovalBag = [];
List<PlayerInfo> trackablePlayers = [];


Parallel.ForEach([.. uuids], item =>
{
    lock (lck)
    {
        Console.Write($"\r{$"{++completeH}/{uuids.Count,-10}".Foreground(ConsoleColor.DarkGray)} {"Filtering players out".Bold().Foreground(ConsoleColor.White)}");
        QuadPixel.Clear.ToEndOfLine();
    }

    try
    {
        dynamic api = GetApiFromPlayerUuid(item.Value);

        if (!Helper.IsMvpPlusPlus(api))
        {
            scheduledRemovalBag.Add(item.Key);
        }
        else
        {
            trackablePlayers.Add(new()
            {
                Name = item.Key,
                UUID = item.Value,
                Stats = new(api)
            });
        }
    }
    catch
    {
        errorsH++;
    }
});

Console.Write($"\r{$"{completeH}/{uuids.Count,-10}".Foreground(ConsoleColor.DarkGray)} {$"Removed {scheduledRemovalBag.Count} player(s)".Bold().Foreground(ConsoleColor.White)}");
QuadPixel.Clear.ToEndOfLine();

// remove non mvp++s
foreach (var removal in scheduledRemovalBag)
{
    uuids.Remove(removal);
}

Console.WriteLine();
Console.WriteLine();
Console.WriteLine($"Filtered all the players with {$"{errors} error(s)".Foreground(ConsoleColor.Red)}");

if (uuids.Count == 0)
{
    Console.WriteLine("All players have been filtered. There is nobody left to be tracked.".Foreground(ConsoleColor.Red));
    return;
}

// player queue - take one, process (only 1 request), wait 5 seconds, repeat for next players

Console.WriteLine($"{$"{uuids.Count} player(s)".Foreground(ConsoleColor.Magenta)} are being tracked now! Any updates will be written right here."
    .Foreground(ConsoleColor.White).Bold());
Console.WriteLine();

// get hypixel data from the player as a single json (serialized to dynamic) 
static dynamic GetApiFromPlayerUuid(Guid item)
{
    return JsonConvert.DeserializeObject(
                HypixelAPI.GetPlayerByUuid(item.ToString())) ?? throw new Exception();
}

// following two methods were added just to inform the user about what is currently happening
void ChangeTrackingStatusLine(string status)
{
    Console.Write($"\r{status}".Foreground(ConsoleColor.DarkGray).Italic());
    QuadPixel.Clear.ToEndOfLine();
}

async Task ChangeTrackingStatusLineWithTimer(string status, TimeSpan delay)
{
    var start = DateTime.Now;
    var end = start + delay;

    var periodicTimer = new PeriodicTimer(TimeSpan.FromSeconds(1));
    while (await periodicTimer.WaitForNextTickAsync())
    {
        var diff = end - DateTime.Now;

        if (diff < TimeSpan.FromSeconds(1))
        {
            break;
        }

        ChangeTrackingStatusLine($"{status} - {diff.Humanize(2, minUnit: TimeUnit.Second)} remaining");
    }

    ChangeTrackingStatusLine($"{status} - running");
}

Dictionary<string, DateTime?> lastToast = [];
async Task ShowPlayerToastTimeLimited(string name)
{
    async Task Show() => await Helper.ShowTrackedNotification(name);

    if (lastToast.TryGetValue(name, out var time))
    {
        if (time + TimeSpan.FromHours(1) > DateTime.Now)
        {
            return;
        }
    }

    await Show();
    lastToast[name] = DateTime.Now;
}
#endregion

// wait 5 minutes because of the rate-limit. the snapshot has already been taken, so the tracking isn't "paused"
Helper.ShowGeneralToast("Tracking will start soon", "You will receive all important messages like this one!");
await ChangeTrackingStatusLineWithTimer("First comparison will be ran soon", TimeSpan.FromMinutes(5));

while (true)
{
    ChangeTrackingStatusLine("Comparing stats of tracked players");

    try
    {
        // process every player that is mvp++ (from the filtered list)
        foreach (var player in trackablePlayers)
        {
            await ProcessPlayerTracking(player);
            Thread.Sleep(5000);
        }
    }
    catch
    {
        await ChangeTrackingStatusLineWithTimer("Exception thrown, temporarily paused", TimeSpan.FromMinutes(5));
        continue;
    }

    await ChangeTrackingStatusLineWithTimer("Comparison finished, idling", TimeSpan.FromMinutes(3));
}

// a method that actually does all the tracking things
async Task ProcessPlayerTracking(PlayerInfo player)
{
    // track one player. compare stats and write anything if tracking has detected change.
    var newStats = GetApiFromPlayerUuid(player.UUID);

    if (player.Stats.IsSameAs(newStats))
    {
        return;
    }

    player.Stats = new(newStats);

    Console.WriteLine($"{Environment.NewLine}{DateTime.Now
        .ToString().Dimmed().Foreground(ConsoleColor.Gray)} Detected different {player.Name.Bold().Foreground(ConsoleColor.Magenta)}'s stats".Foreground(Color.White));
    QuadPixel.Clear.ToEndOfLine();

    await ShowPlayerToastTimeLimited(player.Name);
}