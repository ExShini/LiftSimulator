using UnityEngine;
using Declarations;

class PlayerController: IElevatorPositionHandler
{
    public event PositionChanged PlayerPositionChanged;
    public event PlayerStateChange PlayerStateChanged;

    PLAYER_STATE m_state = PLAYER_STATE.PRE_INIT;
    int m_floorNumberPosition;
    int m_targetFloor;

    int PlayerFloorPosition
    {
        get
        {
            return m_floorNumberPosition;
        }
        set
        {
            if (m_floorNumberPosition != value)
            {
                m_floorNumberPosition = value;
                if (PlayerPositionChanged != null)
                {
                    PlayerPositionChanged(m_floorNumberPosition);
                }
            }
        }
    }

    PLAYER_STATE State
    {
        get
        {
            return m_state;
        }
        set
        {
            if(m_state != value)
            {
                m_state = value;
                if(PlayerStateChanged != null)
                {
                    PlayerStateChanged(m_state);
                }
            }
        }
    }

    public void ResetSimulationWithRandomValues()
    {
        int LowFloorLimit = 1;
        int UpFloorLimit = GameSettingStorageCtr.Instance.NumOfFloors;

        PlayerFloorPosition = Random.Range(LowFloorLimit, UpFloorLimit + 1);
    }


    public void OnElevatorPositionChanged(int elevatorPosition)
    {
        switch(State)
        {
            case PLAYER_STATE.PRE_INIT:
                ChooseInitialPlayerState(elevatorPosition);
                break;
            case PLAYER_STATE.IN_ELEVATOR:
                PlayerFloorPosition = elevatorPosition;
                break;
        }
    }

    public void OnElevatorAchiveFloor(int elevatorPosition)
    {
        switch (State)
        {
            case PLAYER_STATE.IN_ELEVATOR:
                TryToLeave(elevatorPosition);
                break;
            case PLAYER_STATE.OUT_ELEVATOR:
                TryToEnter(elevatorPosition);
                break;
        }
    }

    protected void TryToLeave(int elevatorPosition)
    {
        if(m_targetFloor == elevatorPosition)
        {
            State = PLAYER_STATE.OUT_ELEVATOR;
        }
    }

    protected void TryToEnter(int elevatorPosition)
    {
        if(elevatorPosition == PlayerFloorPosition)
        {
            State = PLAYER_STATE.IN_ELEVATOR;
        }
    }

    protected void ChooseInitialPlayerState(int elevatorPosition)
    {
        if (elevatorPosition == PlayerFloorPosition)
        {
            State = PLAYER_STATE.IN_ELEVATOR;
        }
        else
        {
            State = PLAYER_STATE.OUT_ELEVATOR;
        }
    }

    public void OnPlayerMakeDecision(Request req)
    {
        if(req.Type == REQUEST_TYPE.DESTINATION_SET)
        {
            m_targetFloor = req.Floor;
        }
    }
}
