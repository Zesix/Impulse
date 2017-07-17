/// <summary>
/// The AI States used by BaseAIController.
/// </summary>
public enum AIState
{
    MovingLookingForTarget,
    ChasingTarget,
    BackingUpLookingForTarget,
    StoppedTurningLeft,
    StoppedTurningRight,
    PausedLookingForTarget,
    TranslateAlongWaypointPath,
    PausedNoTarget,
    SteerToWaypoint,
    SteerToTarget,
}

