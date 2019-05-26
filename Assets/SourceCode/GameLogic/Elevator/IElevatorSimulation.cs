using Declarations;

interface IElevatorSimulation
{
    event PositionChanged ElevatorPosChanged;
    event Action ElematorLeaveFloor;
    event PositionChanged ElevatorStopMoving;

    int Position { get; }

    void ApplyRequest(Request req);
    void ResetSimulationWithRandomValues();
}
