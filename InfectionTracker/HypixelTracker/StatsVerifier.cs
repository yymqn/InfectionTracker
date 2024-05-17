namespace HypixelTracker;

// todo: in the future, if I will add support for more minigames, this struct will be changed completely, but now it's ok

// this thing is storing player stats in a more efficient way than the api's json data
internal readonly struct StatsVerifier(dynamic json)
{
    // store these stats
    public long Deaths { get; } = (long)json.player.stats.MurderMystery.deaths_MURDER_INFECTION;
    public long Kills { get; } = (long)json.player.stats.MurderMystery.kills_MURDER_INFECTION + (long)json.player.stats.MurderMystery.kills_as_infected_MURDER_INFECTION;
    public long Wins { get; } = (long)json.player.stats.MurderMystery.wins_MURDER_INFECTION;
    public long Games { get; } = (long)json.player.stats.MurderMystery.games_MURDER_INFECTION;
    public long GoldsPickedUp { get; } = (long)json.player.stats.MurderMystery.coins_pickedup_MURDER_INFECTION;
    public long SurvivedTime { get; } = (long)json.player.stats.MurderMystery.total_time_survived_seconds_MURDER_INFECTION;

    // returns true if the second (new) api's data are same as the old one (current)
    public bool IsSameAs(dynamic secondJson)
    {
        StatsVerifier verif = new(secondJson);

        return
            Deaths == verif.Deaths &&
            Kills == verif.Kills &&
            Wins == verif.Wins &&
            Games == verif.Games &&
            GoldsPickedUp == verif.GoldsPickedUp &&
            SurvivedTime == verif.SurvivedTime;
    }
}
