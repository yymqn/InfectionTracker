namespace HypixelTracker;
// stores player name, uuid and the stats in a single class.
// while struct would be more efficient, i need to use a reference type, so class
internal class PlayerInfo
{
    public required string Name { get; init; }
    public required Guid UUID { get; init; }
    public StatsVerifier Stats { get; set; }
}
