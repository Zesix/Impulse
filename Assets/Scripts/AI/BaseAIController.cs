using UnityEngine;

public class BaseAIController : ExtendedMonoBehaviour
{

    // AI states are defined in the AIStates namespace

    private Transform _proxyTarget;
    private Vector3 _relativeTarget;
    private float _targetAngle;
    private RaycastHit _hit;
    private Vector3 _tempDirVec;

    // The rate the AI controller updates.
    public float UpdateAiRate = 0.1f;

    // In some cases, we don't want this script to manipulate an object but instead
    // provide input for another script to use for movement. This script's movement inputs
    // are saved in the horz and vert variables.
    public float Horz;
    public float Vert;

    private int _obstacleHitType;

    // editor changeable / visible

    // If true, the object will not rotate or provide input values;
    // it will move from waypoint to waypoint purely through Transform.Translate().
    public bool IsStationary;

    public AIState CurrentAiState;

    public float PatrolSpeed = 5f;
    public float PatrolTurnSpeed = 10f;
    public float WallAvoidDistance = 40f;

    public Transform FollowTarget;

    public float ModelRotateSpeed = 15f;
    public int FollowTargetMaxTurnAngle = 120;

    public float MinChaseDistance = 2f;
    public float MaxChaseDistance = 10f;
    public float VisionHeightOffset = 1f;

    [System.NonSerialized]
    public Vector3 MoveDirection;

    // waypoint following related variables
    public WaypointPathManager MyWayControl;

    public int CurrentWaypointNum;

    [System.NonSerialized]
    public Transform CurrentWaypointTransform;

    private int _totalWaypoints;

    private Vector3 _nodePosition = Vector3.zero;
    private Vector3 _myPosition = Vector3.zero;
    private Vector3 _diff;
    private float _currentWayDist;

    [System.NonSerialized]
    public bool ReachedLastWaypoint;
    private Vector3 _moveVec;
    private Vector3 _targetMoveVec;
    private float _distanceToChaseTarget;

    public float WaypointDistance = 5f;
    public float MoveSpeed = 30f;
    public float PathSmoothing = 2f;

    // If true, the AI object will go through the waypoints in reverse order.
    public bool ShouldReversePathFollowing;

    // If true, the AI object will loop through the waypoints.
    public bool LoopPath;

    // If true, the AI object will destroy itself once it reaches the last waypoint.
    public bool DestroyAtEndOfWaypoints;

    public bool FaceWaypoints;
    public bool StartAtFirstWaypoint;

    [System.NonSerialized]
    public bool IsRespawning;

    private int _obstacleFinderResult;
    public Transform RotateTransform;

    [System.NonSerialized]
    public Vector3 RelativeWaypointPosition;

    public bool AiControlled;

    private void Start()
    {
        Init();

        // Begin custom AI update loop
        InvokeRepeating(nameof(AiUpdateLoop), 0.0f, UpdateAiRate);
    }

    /// <summary>
    /// Cache references to required components.
    /// </summary>
    public virtual void Init()
    {
        MyGameObject = gameObject;
        MyTransform = transform;

        // rotateTransform may be set if the object to rotate is different than the main transform.
        if (RotateTransform == null)
            RotateTransform = MyTransform;

        DidInit = true;
    }

    /// <summary>
    /// Sets whether or not this script should take control of the object.
    /// </summary>
    /// <param name="state"></param>
    public void SetAiControl(bool state)
    {
        AiControlled = state;
    }

    // set up AI parameters --------------------

    /// <summary>
    /// Sets how quickly the AI object should move when patrolling.
    /// </summary>
    /// <param name="aNum"></param>
    public void SetPatrolSpeed(float aNum)
    {
        PatrolSpeed = aNum;
    }

    /// <summary>
    /// Sets how quickly the AI object should turn when patrolling.
    /// </summary>
    /// <param name="aNum"></param>
    public void SetPatrolTurnSpeed(float aNum)
    {
        PatrolTurnSpeed = aNum;
    }

    /// <summary>
    /// Sets how far the AI object should look ahead in attempts to avoid running into walls.
    /// Units are used in 3D and not 2D.
    /// </summary>
    /// <param name="aNum"></param>
    public void SetWallAvoidDistance(float aNum)
    {
        WallAvoidDistance = aNum;
    }

    /// <summary>
    /// Sets how close the AI object will be allowed to get to a waypoint before it advances to the next waypoint.
    /// </summary>
    /// <param name="aNum"></param>
    public void SetWaypointDistance(float aNum)
    {
        WaypointDistance = aNum;
    }

    /// <summary>
    /// Sets the speed the AI object will move when advancing along a path of waypoints.
    /// </summary>
    /// <param name="aNum"></param>
    public void SetMoveSpeed(float aNum)
    {
        MoveSpeed = aNum;
    }

    /// <summary>
    /// Sets how close the AI object should get to its target before it stops moving toward it.
    /// Used to prevent the AI object from getting too close and colliding with the target.
    /// </summary>
    /// <param name="aNum"></param>
    public void SetMinChaseDistance(float aNum)
    {
        MinChaseDistance = aNum;
    }

    /// <summary>
    /// Sets how far away from the target the AI object is allowed to get before it gives up chasing and returns to patrolling.
    /// </summary>
    /// <param name="aNum"></param>
    public void SetMaxChaseDistance(float aNum)
    {
        MaxChaseDistance = aNum;
    }

    /// <summary>
    /// Sets how much to interpolate the damping of rotation when following a waypoint path.
    /// Prevents the AI object from jumping to a point directly at each point.
    /// </summary>
    /// <param name="aNum"></param>
    public void SetPathSmoothing(float aNum)
    {
        PathSmoothing = aNum;
    }

    // -----------------------------------------

    /// <summary>
    /// Used to force the state of the AI object from another script.
    /// </summary>
    /// <param name="newState">The AI state the AI object should begin with.</param>
    public virtual void SetAIState(AIState newState)
    {
        // update AI state
        CurrentAiState = newState;
    }

    /// <summary>
    /// Sets the target for the AI object to chase.
    /// </summary>
    /// <param name="theTransform"></param>
    public virtual void SetChaseTarget(Transform theTransform)
    {
        // set a target for this AI to chase, if required
        FollowTarget = theTransform;
    }

    /// <summary>
    /// The custom update loop for the AI.
    /// </summary>
    public virtual void AiUpdateLoop()
    {
        // make sure we have initialized before doing anything
        if (!DidInit)
            Init();

        // check to see if we're supposed to be controlling the player
        if (!AiControlled)
            return;

        // do AI updates
        UpdateAi();
    }

    public virtual void UpdateAi()
    {
        // reset our inputs
        Horz = 0;
        Vert = 0;

        var obstacleFinderResult = IsObstacleAhead();

        switch (CurrentAiState)
        {
            // -----------------------------
            case AIState.MovingLookingForTarget:
                // look for chase target
                if (FollowTarget != null)
                    LookAroundFor(FollowTarget);

                // the AvoidWalls function looks to see if there's anything in-front. If there is,
                // it will automatically change the value of moveDirection before we do the actual move
                if (obstacleFinderResult == 1)
                { // GO LEFT
                    SetAIState(AIState.StoppedTurningLeft);
                }
                if (obstacleFinderResult == 2)
                { // GO RIGHT
                    SetAIState(AIState.StoppedTurningRight);
                }

                if (obstacleFinderResult == 3)
                { // BACK UP
                    SetAIState(AIState.BackingUpLookingForTarget);
                }

                // all clear! head forward
                MoveForward();
                break;
            case AIState.ChasingTarget:
                // chasing
                // in case mode, we point toward the target and go right at it!

                // quick check to make sure that we have a target (if not, we drop back to patrol mode)
                if (FollowTarget == null)
                    SetAIState(AIState.MovingLookingForTarget);

                // the TurnTowardTarget function does just that, so to chase we just throw it the current target
                TurnTowardTarget(FollowTarget);

                // find the distance between us and the chase target to see if it is within range
                _distanceToChaseTarget = Vector3.Distance(MyTransform.position, FollowTarget.position);

                // check the range
                if (_distanceToChaseTarget > MinChaseDistance)
                {
                    // keep charging forward
                    MoveForward();
                }

                // here we do a quick check to test the distance between AI and target. If it's higher than
                // our maxChaseDistance variable, we drop out of chase mode and go back to patrolling.
                if (_distanceToChaseTarget > MaxChaseDistance || CanSee(FollowTarget) == false)
                {
                    // set our state to 1 - moving_looking_for_target
                    SetAIState(AIState.MovingLookingForTarget);
                }

                break;
            // -----------------------------

            case AIState.BackingUpLookingForTarget:

                // look for chase target
                if (FollowTarget != null)
                    LookAroundFor(FollowTarget);

                // backing up
                MoveBack();

                if (obstacleFinderResult == 0)
                {
                    // now we've backed up, lets randomize whether to go left or right
                    if (Random.Range(0, 100) > 50)
                    {
                        SetAIState(AIState.StoppedTurningLeft);
                    }
                    else
                    {
                        SetAIState(AIState.StoppedTurningRight);
                    }
                }

                break;
            case AIState.StoppedTurningLeft:
                // look for chase target
                if (FollowTarget != null)
                    LookAroundFor(FollowTarget);

                // stopped, turning left
                TurnLeft();

                if (obstacleFinderResult == 0)
                {
                    SetAIState(AIState.MovingLookingForTarget);
                }
                break;

            case AIState.StoppedTurningRight:
                // look for chase target
                if (FollowTarget != null)
                    LookAroundFor(FollowTarget);

                // stopped, turning right
                TurnRight();

                // check results from looking, to see if path ahead is clear
                if (obstacleFinderResult == 0)
                {
                    SetAIState(AIState.MovingLookingForTarget);
                }
                break;
            case AIState.PausedLookingForTarget:
                // standing still, with looking for chase target
                // look for chase target
                if (FollowTarget != null)
                    LookAroundFor(FollowTarget);
                break;

            case AIState.TranslateAlongWaypointPath:
                // following waypoints (moving toward them, not pointing at them) at the speed of
                // moveSpeed

                // make sure we have been initialized before trying to access waypoints
                if (!DidInit && !ReachedLastWaypoint)
                    return;

                UpdateWaypoints();

                // move the ship
                if (!IsStationary)
                {
                    _targetMoveVec = Vector3.Normalize(CurrentWaypointTransform.position - MyTransform.position);
                    _moveVec = Vector3.Lerp(_moveVec, _targetMoveVec, Time.deltaTime * PathSmoothing);
                    MyTransform.Translate(_moveVec * MoveSpeed * Time.deltaTime);
                    MoveForward();

                    if (FaceWaypoints)
                    {
                        TurnTowardTarget(CurrentWaypointTransform);
                    }
                }
                break;

            case AIState.SteerToWaypoint:

                // make sure we have been initialized before trying to access waypoints
                if (!DidInit && !ReachedLastWaypoint)
                    return;

                UpdateWaypoints();

                if (CurrentWaypointTransform == null)
                {
                    // it may be possible that this function gets called before waypoints have been set up, so we catch any nulls here
                    return;
                }

                // now we just find the relative position of the waypoint from the car transform,
                // that way we can determine how far to the left and right the waypoint is.
                RelativeWaypointPosition = MyTransform.InverseTransformPoint(CurrentWaypointTransform.position);

                // by dividing the horz position by the magnitude, we get a decimal percentage of the turn angle that we can use to drive the wheels
                Horz = (RelativeWaypointPosition.x / RelativeWaypointPosition.magnitude);

                // now we do the same for torque, but make sure that it doesn't apply any engine torque when going around a sharp turn...
                if (Mathf.Abs(Horz) < 0.5f)
                {
                    Vert = RelativeWaypointPosition.z / RelativeWaypointPosition.magnitude - Mathf.Abs(Horz);
                }
                else
                {
                    NoMove();
                }
                break;

            case AIState.SteerToTarget:

                // make sure we have been initialized before trying to access waypoints
                if (!DidInit)
                    return;

                if (FollowTarget == null)
                {
                    // it may be possible that this function gets called before a targer has been set up, so we catch any nulls here
                    return;
                }

                // now we just find the relative position of the waypoint from the car transform,
                // that way we can determine how far to the left and right the waypoint is.
                RelativeWaypointPosition = transform.InverseTransformPoint(FollowTarget.position);

                // by dividing the horz position by the magnitude, we get a decimal percentage of the turn angle that we can use to drive the wheels
                Horz = (RelativeWaypointPosition.x / RelativeWaypointPosition.magnitude);

                // if we're outside of the minimum chase distance, drive!
                if (Vector3.Distance(FollowTarget.position, MyTransform.position) > MinChaseDistance)
                {
                    MoveForward();
                }
                else
                {
                    NoMove();
                }

                if (FollowTarget != null)
                    LookAroundFor(FollowTarget);

                // the AvoidWalls function looks to see if there's anything in-front. If there is,
                // it will automatically change the value of moveDirection before we do the actual move
                if (obstacleFinderResult == 1)
                { // GO LEFT
                    TurnLeft();
                }

                if (obstacleFinderResult == 2)
                { // GO RIGHT
                    TurnRight();
                }

                if (obstacleFinderResult == 3)
                { // BACK UP
                    MoveBack();
                }

                break;

            case AIState.PausedNoTarget:
                // paused_no_target
                break;
        }
    }

    public virtual void TurnLeft()
    {
        Horz = -1;
    }

    public virtual void TurnRight()
    {
        Horz = 1;
    }

    public virtual void MoveForward()
    {
        Vert = 1;
    }

    public virtual void MoveBack()
    {
        Vert = -1;
    }

    public virtual void NoMove()
    {
        Vert = 0;
    }

    public virtual void LookAroundFor(Transform aTransform)
    {
        // here we do a quick check to test the distance between AI and target. If it's higher than
        // our maxChaseDistance variable, we drop out of chase mode and go back to patrolling.
        if (Vector3.Distance(MyTransform.position, aTransform.position) < MaxChaseDistance)
        {
            // check to see if the target is visible before going into chase mode
            if (CanSee(FollowTarget))
            {
                // set our state to chase the target
                SetAIState(AIState.ChasingTarget);
            }
        }
    }

    private int _obstacleFinding;

    /// <summary>
    /// Checks if an obstacle using raycasting.
    /// Return information:
    /// 0 = No obstacles were found.
    /// 1 = An obstacle was detected in front, left side only.
    /// 2 = An obstacle was detected in front, right side only.
    /// 3 = Obstacles were detected in front, on both sides.
    /// </summary>
    /// <returns>An integer corresponding to the raycast result.</returns>
    public virtual int IsObstacleAhead()
    {
        _obstacleHitType = 0;

        // quick check to make sure that myTransform has been set
        if (MyTransform == null)
        {
            return 0;
        }

        // draw this raycast so we can see what it is doing
        Debug.DrawRay(MyTransform.position, ((MyTransform.forward + (MyTransform.right * 0.5f)) * WallAvoidDistance));
        Debug.DrawRay(MyTransform.position, ((MyTransform.forward + (MyTransform.right * -0.5f)) * WallAvoidDistance));

        // cast a ray out forward from our AI and put the 'result' into the variable named hit
        if (Physics.Raycast(MyTransform.position, MyTransform.forward + (MyTransform.right * 0.5f), out _hit, WallAvoidDistance))
        {
            // obstacle
            // it's a left hit, so it's a type 1 right now (though it could change when we check on the other side)
            _obstacleHitType = 1;
        }

        if (Physics.Raycast(MyTransform.position, MyTransform.forward + (MyTransform.right * -0.5f), out _hit, WallAvoidDistance))
        {
            // obstacle
            if (_obstacleHitType == 0)
            {
                // if we haven't hit anything yet, this is a type 2
                _obstacleHitType = 2;
            }
            else
            {
                // if we have hits on both left and right raycasts, it's a type 3
                _obstacleHitType = 3;
            }
        }

        return _obstacleHitType;
    }

    public void TurnTowardTarget(Transform aTarget)
    {
        if (aTarget == null)
            return;

        // Calculate the target position relative to the 
        // target this transforms coordinate system. 
        // eg. a positive x value means the target is to  
        // to the right of the car, a positive z means 
        // the target is in front of the car 
        _relativeTarget = RotateTransform.InverseTransformPoint(aTarget.position); // note we use rotateTransform as a rotation object rather than myTransform!

        // Calculate the target angle  
        _targetAngle = Mathf.Atan2(_relativeTarget.x, _relativeTarget.z);

        // Atan returns the angle in radians, convert to degrees 
        _targetAngle *= Mathf.Rad2Deg;

        // The wheels should have a maximum rotation angle 
        _targetAngle = Mathf.Clamp(_targetAngle, -FollowTargetMaxTurnAngle - _targetAngle, FollowTargetMaxTurnAngle);

        // turn towards the target at the rate of modelRotateSpeed
        RotateTransform.Rotate(0, _targetAngle * ModelRotateSpeed * Time.deltaTime, 0);
    }

    /// <summary>
    /// Returns true if the AI object can see the target transform (i.e. no obstacles blocking vision).
    /// </summary>
    /// <param name="aTarget"></param>
    /// <returns></returns>
    public bool CanSee(Transform aTarget)
    {
        // first, let's get a vector to use for raycasting by subtracting the target position from our AI position
        _tempDirVec = Vector3.Normalize(aTarget.position - MyTransform.position);

        // lets have a debug line to check the distance between the two manually, in case you run into trouble!
        Debug.DrawLine(MyTransform.position, aTarget.position);

        // cast a ray from our AI, out toward the target passed in (use the tempDirVec magnitude as the distance to cast)
        if (Physics.Raycast(MyTransform.position + (VisionHeightOffset * MyTransform.up), _tempDirVec, out _hit, MaxChaseDistance))
        {
            // check to see if we hit the target
            if (_hit.transform.gameObject == aTarget.gameObject)
            {
                return true;
            }
        }

        // nothing found, so return false
        return false;
    }

    public void SetWaypointManager(WaypointPathManager aControl)
    {
        MyWayControl = aControl;

        // grab total waypoints
        _totalWaypoints = MyWayControl.GetTotal();

        // make sure that if you use SetReversePath to set shouldReversePathFollowing that you
        // call SetReversePath for the first time BEFORE SetWayController, otherwise it won't set the first waypoint correctly

        if (ShouldReversePathFollowing)
        {
            CurrentWaypointNum = _totalWaypoints - 1;
        }
        else
        {
            CurrentWaypointNum = 0;
        }

        Init();

        // get the first waypoint from the waypoint controller
        CurrentWaypointTransform = MyWayControl.GetWaypoint(CurrentWaypointNum);

        if (StartAtFirstWaypoint)
        {
            // position at the currentWaypointTransform position
            MyTransform.position = CurrentWaypointTransform.position;
        }
    }

    public void SetReversePath(bool shouldRev)
    {
        ShouldReversePathFollowing = shouldRev;
    }

    public void SetSpeed(float aSpeed)
    {
        MoveSpeed = aSpeed;
    }

    public void SetPathSmoothingRate(float aRate)
    {
        PathSmoothing = aRate;
    }

    public void SetRotateSpeed(float aRate)
    {
        ModelRotateSpeed = aRate;
    }

    private void UpdateWaypoints()
    {
        // If we don't have a waypoint controller, we safely drop out
        if (MyWayControl == null)
            return;

        if (ReachedLastWaypoint && DestroyAtEndOfWaypoints)
        {
            // destroy myself(!)
            Destroy(gameObject);
            return;
        }
        if (ReachedLastWaypoint)
        {
            CurrentWaypointNum = 0;
            ReachedLastWaypoint = false;
        }

        // because of the order that scripts run and are initialised, it is possible for this function
        // to be called before we have actually finished running the waypoints initialization, which
        // means we need to drop out to avoid doing anything silly or before it breaks the game.
        if (_totalWaypoints == 0)
        {
            // grab total waypoints
            _totalWaypoints = MyWayControl.GetTotal();
            return;
        }

        if (CurrentWaypointTransform == null)
        {
            // grab our transform reference from the waypoint controller
            CurrentWaypointTransform = MyWayControl.GetWaypoint(CurrentWaypointNum);
        }

        // now we check to see if we are close enough to the current waypoint
        // to advance on to the next one

        // LEAVE THIS COMMENTED OUT UNLESS YOUR GAME IS NOT IN 3D.
        // myPosition = myTransform.position;
        // myPosition.y = 0;

        // get waypoint position and 'flatten' it
        // LEAVE THIS COMMENTED OUT UNLESS YOUR GAME IS NOT IN 3D.
        // nodePosition = currentWaypointTransform.position;
        // nodePosition.y = 0;

        // check distance from this to the waypoint

        _currentWayDist = Vector3.Distance(_nodePosition, _myPosition);

        if (_currentWayDist < WaypointDistance)
        {
            // we are close to the current node, so let's move on to the next one!

            if (ShouldReversePathFollowing)
            {
                CurrentWaypointNum--;
                // now check to see if we have been all the way around
                if (CurrentWaypointNum < 0)
                {
                    // just incase it gets referenced before we are destroyed, let's keep it to a safe index number
                    CurrentWaypointNum = 0;
                    // completed the route!
                    ReachedLastWaypoint = true;
                    // if we are set to loop, reset the currentWaypointNum to 0
                    if (LoopPath)
                    {
                        CurrentWaypointNum = _totalWaypoints;

                        // the route keeps going in a loop, so we don't want reachedLastWaypoint to ever become true
                        ReachedLastWaypoint = false;
                    }
                    // drop out of this function before we grab another waypoint into currentWaypointTransform, as
                    // we don't need one and the index may be invalid
                    return;
                }
            }
            else
            {
                CurrentWaypointNum++;
                // now check to see if we have been all the way around
                if (CurrentWaypointNum >= _totalWaypoints)
                {
                    // completed the route!
                    ReachedLastWaypoint = true;
                    // if we are set to loop, reset the currentWaypointNum to 0
                    if (LoopPath)
                    {
                        CurrentWaypointNum = 0;

                        // the route keeps going in a loop, so we don't want reachedLastWaypoint to ever become true
                        ReachedLastWaypoint = false;
                    }
                    // drop out of this function before we grab another waypoint into currentWaypointTransform, as
                    // we don't need one and the index may be invalid
                    return;
                }
            }

            // grab our transform reference from the waypoint controller
            CurrentWaypointTransform = MyWayControl.GetWaypoint(CurrentWaypointNum);

        }
    }

    public float GetHorizontal()
    {
        return Horz;
    }

    public float GetVertical()
    {
        return Vert;
    }

}
