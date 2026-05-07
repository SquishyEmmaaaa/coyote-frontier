using System.Numerics;
using Content.Shared.Salvage;
using Content.Shared.Salvage.Expeditions;
using Robust.Shared.Audio;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.List;

namespace Content.Server.Salvage.Expeditions;

/// <summary>
/// Designates this entity as holding a salvage expedition.
/// </summary>
[RegisterComponent, AutoGenerateComponentPause]
public sealed partial class SalvageExpeditionComponent : SharedSalvageExpeditionComponent
{
    public SalvageMissionParams MissionParams = default!;

    /// <summary>
    /// Consoles sharing this expedition's offer pool economy.
    /// </summary>
    public string EconomyId = "Frontier";

    /// <summary>
    /// Where the dungeon is located for initial announcement.
    /// </summary>
    [DataField("dungeonLocation")]
    public Vector2 DungeonLocation = Vector2.Zero;

    /// <summary>
    /// Bounding box of the generated expedition objective area.
    /// </summary>
    public Box2 DungeonBounds;

    /// <summary>
    /// Reserved shuttle landing zones in expedition-map coordinates.
    /// </summary>
    public List<Box2> ReservedLandingZones = new();

    /// <summary>
    /// Stations currently participating in this expedition.
    /// </summary>
    public HashSet<EntityUid> ParticipantStations = new();

    /// <summary>
    /// For shared expeditions, stores which shuttle grid each player entity arrived on.
    /// Used to return bodies to the correct ship when that ship's timer naturally expires.
    /// </summary>
    public Dictionary<EntityUid, EntityUid> SharedArrivalShuttles = new();

    /// <summary>
    /// Per-shuttle expedition end times.
    /// </summary>
    public Dictionary<EntityUid, TimeSpan> ShuttleEndTimes = new();

    /// <summary>
    /// When the expeditions ends.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("endTime", customTypeSerializer: typeof(TimeOffsetSerializer))]
    [AutoPausedField]
    public TimeSpan EndTime;

    /// <summary>
    /// Station whose mission this is.
    /// </summary>
    [DataField("station")]
    public EntityUid Station;

    [ViewVariables] public bool Completed = false;

    // _CS: moved to Client
    /// <summary>
    /// Countdown audio stream.
    /// </summary>
    // [DataField, AutoNetworkedField]
    // public EntityUid? Stream = null;
    // _CS End: moved to Client

    /// <summary>
    /// Sound that plays when the mission end is imminent.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField]
    public SoundSpecifier Sound = new SoundCollectionSpecifier("ExpeditionEnd")
    {
        Params = AudioParams.Default.WithVolume(-5),
    };

    // _CS: moved to Shared
    /// <summary>
    /// Song selected on MapInit so we can predict the audio countdown properly.
    /// </summary>
    // [DataField]
    // public ResolvedSoundSpecifier SelectedSong;
    // _CS End: moved to Shared

    /// <summary>
    /// next time to check for autoabort
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField]
    public TimeSpan NextAutoAbortCheck = TimeSpan.Zero;

    /// <summary>
    /// The goobers on this exped who were SSD on arrival
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField]
    public HashSet<EntityUid> InitialSsdGoobers = new();

    /// <summary>
    /// Is it aborted?
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField]
    public bool Aborted = false;

    /// <summary>
    /// Shuttles whose departure timer was force-shortened (e.g. early finish / abort) instead of natural timer expiry.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField]
    public HashSet<EntityUid> ForcedDepartureShuttles = new();

    // _CS Start: weather system fields
    // Weather system fields — not serialized; reconstructed when the first shuttle arrives.

    /// <summary>
    /// Biome ID for this expedition, cached for weather rolling.
    /// </summary>
    public string BiomeId = string.Empty;

    /// <summary>
    /// When the second weather roll fires. TimeSpan.MaxValue means no roll is pending.
    /// </summary>
    public TimeSpan WeatherNextRoll = TimeSpan.MaxValue;

    /// <summary>
    /// Active staged weather phase prototype IDs. Null when no staged weather is in progress.
    /// </summary>
    public List<string>? WeatherPhaseSequence;

    /// <summary>
    /// Current index into WeatherPhaseSequence.
    /// </summary>
    public int WeatherPhaseIndex;

    /// <summary>
    /// When the current weather phase ends and the system should advance to the next phase.
    /// </summary>
    public TimeSpan WeatherPhaseEnd;

    /// <summary>
    /// Duration of each phase in the currently running staged sequence.
    /// </summary>
    public TimeSpan WeatherPhaseDuration;

    // _CS End: weather system fields
}
