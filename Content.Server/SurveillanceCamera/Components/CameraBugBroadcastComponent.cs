using Robust.Shared.Prototypes;

namespace Content.Server.SurveillanceCamera;

/// <summary>
///     Enables a toggleable wireless broadcast mode on the camera bug item.
///     When active, a covert entertainment-subnet camera is spawned and parented to the bug,
///     so remote monitors (TVs) can see and hear the area around it.
/// </summary>
[RegisterComponent]
public sealed partial class CameraBugBroadcastComponent : Component
{
    /// <summary> Whether the broadcast mode is currently active. </summary>
    [ViewVariables]
    public bool Broadcasting { get; set; } = false;

    /// <summary> The spawned covert camera entity, if broadcast is active. </summary>
    [ViewVariables]
    public EntityUid? SpawnedCamera { get; set; }

    /// <summary> Prototype to spawn for the broadcast camera. </summary>
    [DataField]
    public EntProtoId CameraPrototype { get; set; } = "CameraBugCamera";
}
