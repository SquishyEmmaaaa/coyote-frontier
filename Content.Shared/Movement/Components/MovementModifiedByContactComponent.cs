using System;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.Movement.Components;

/// <summary>
/// Exists just to listen to a single event. What a life.
/// </summary>
[NetworkedComponent, RegisterComponent] // must be networked to properly predict adding & removal
public sealed partial class SpeedModifiedByContactComponent : Component
{
    // Keeps contact slowdowns stable for a short window to avoid rapid edge toggling.
    public TimeSpan LastContactUpdate;

    public float LastWalkSpeedModifier = 1.0f;
    public float LastSprintSpeedModifier = 1.0f;
}

[NetworkedComponent, RegisterComponent] // ditto but for friction
public sealed partial class FrictionModifiedByContactComponent : Component
{
}
