namespace Declarations
{
    delegate void PositionChanged(int position);
    delegate void PlayerStateChange(PLAYER_STATE newState);
    delegate void DoorStateChanged(bool isOpen);
    delegate void RequesPosted(Request req);
    delegate void Action();

    public enum PLAYER_STATE
    {
        IN_ELEVATOR,
        OUT_ELEVATOR,
        NUM_OF_STATE,
        PRE_INIT
    }

    public enum DIRECTION
    {
        UP,
        DOWN
    }

    public enum REQUEST_TYPE
    {
        ELEVATOR_CALL,
        DESTINATION_SET,
        PAUSE_ELEVATOR
    }
}

