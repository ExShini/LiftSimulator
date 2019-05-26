using Declarations;

class DoorManager: IPlayerPositionHandler, IElevatorMovingHandler
{
    public event DoorStateChanged DoorIsOpen;

    int m_currentPlayerPosition;
    PLAYER_STATE m_currentPlayerState;

    public void OnPlayerPositionChanged(int position)
    {
        m_currentPlayerPosition = position;
    }

    public void OnPlayerStateChanged(PLAYER_STATE state)
    {
        m_currentPlayerState = state;
    }

    public void OnElevatorStartMoving()
    {
        if (DoorIsOpen != null)
        {
            DoorIsOpen(false);
        }
    }

    public void OnElevatorStopMoving(int elevatorPosition)
    {
        if (m_currentPlayerState == PLAYER_STATE.OUT_ELEVATOR)
        {
            if (elevatorPosition == m_currentPlayerPosition)
            {
                SetDoorState(true);
            }
            else
            {
                SetDoorState(false);
            }
        }
        else if(m_currentPlayerState == PLAYER_STATE.IN_ELEVATOR)
        {
            SetDoorState(true);
        }
    }

    private void SetDoorState(bool isOpen)
    {
        if (DoorIsOpen != null)
        {
            DoorIsOpen(isOpen);
        }
    }
}

