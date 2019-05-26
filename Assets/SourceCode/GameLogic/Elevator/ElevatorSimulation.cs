using System.Collections.Generic;
using UnityEngine;
using Declarations;


class ElevatorSimulation : IElevatorSimulation, IExecuteble
{
    enum ELEVATOR_STATE
    {
        MOVING,
        WAITING,
        BOARDING,
        PAUSED_MOVING,
        PAUSED_BOARDING
    }

    public event PositionChanged ElevatorPosChanged;
    public event Action ElematorLeaveFloor;
    public event PositionChanged ElevatorStopMoving;

    int m_elevatorPosition;
    ELEVATOR_STATE m_state;
    DIRECTION m_movingDirection;
    DIRECTION m_goalDirection;

    float m_boardingTimer = 4.0f;
    float m_timeForPassingOneFloor = 1.0f;
    float m_timer = 0.0f;
    bool m_changeDirectionIsPlanned;

    LinkedList<Request> m_postponedTasks = new LinkedList<Request>();
    HashSet<int> m_currentMovingPlan = new HashSet<int>();

    public int Position
    {
        get
        {
            return m_elevatorPosition;
        }

        private set
        {
            m_elevatorPosition = value;
            if (ElevatorPosChanged != null)
            {
                ElevatorPosChanged(m_elevatorPosition);
            }
        }
    }

    public void ResetSimulationWithRandomValues()
    {
        int LowFloorLimit = 1;
        int UpFloorLimit = GameSettingStorageCtr.Instance.NumOfFloors;

        Position = Random.Range(LowFloorLimit, UpFloorLimit);
        m_state = ELEVATOR_STATE.WAITING;
        m_movingDirection = DIRECTION.UP;
        m_goalDirection = DIRECTION.UP;
        m_changeDirectionIsPlanned = true;

        if (ElevatorStopMoving != null)
        {
            ElevatorStopMoving(Position);
        }
    }

    public void ApplyRequest(Request req)
    {
        Debug.Log("New requst: " + req.Type + " from Floor " + req.Floor);

        if (m_state == ELEVATOR_STATE.WAITING)
        {
            if (req.Type != REQUEST_TYPE.PAUSE_ELEVATOR)
            {
                m_postponedTasks.AddLast(req);
            }
            return;
        }

        bool AddedToExecution = TryToAddRequestForExecuting(req);
        if (!AddedToExecution)
        {
            m_postponedTasks.AddLast(req);
        }
    }

    protected bool TryToAddRequestForExecuting(Request req)
    {
        bool result = false;

        switch (req.Type)
        {
            case REQUEST_TYPE.DESTINATION_SET:
                result = TryToAddDestinationReq(req);
                break;
            case REQUEST_TYPE.ELEVATOR_CALL:
                result = TryToAddElevatorCallReq(req);
                break;
            case REQUEST_TYPE.PAUSE_ELEVATOR:
                result = AddPauseReq();
                break;
        }

        return result;
    }


    protected bool TryToAddDestinationReq(Request req)
    {
        int requestedFloor = req.Floor;

        if (m_goalDirection == DIRECTION.UP && requestedFloor > Position)
        {
            m_currentMovingPlan.Add(requestedFloor);
            return true;
        }
        else if (m_goalDirection == DIRECTION.DOWN && requestedFloor < Position)
        {
            m_currentMovingPlan.Add(requestedFloor);
            return true;
        }
        else if (requestedFloor == Position && m_state == ELEVATOR_STATE.BOARDING)
        {
            StartBoarding(true);
            return true;
        }

        return false;
    }

    protected bool TryToAddElevatorCallReq(Request req)
    {
        if (m_changeDirectionIsPlanned)
        {
            return false;
        }

        int requestedFloor = req.Floor;
        DIRECTION reqDirection = req.ReqDirection;

        if (m_movingDirection == DIRECTION.UP && requestedFloor > Position && reqDirection == m_goalDirection)
        {
            m_currentMovingPlan.Add(requestedFloor);
            return true;
        }
        else if (m_movingDirection == DIRECTION.DOWN && requestedFloor < Position && reqDirection == m_goalDirection)
        {
            m_currentMovingPlan.Add(requestedFloor);
            return true;
        }
        else if (requestedFloor == Position && m_state == ELEVATOR_STATE.BOARDING && reqDirection == m_goalDirection)
        {
            StartBoarding();
            return true;
        }

        return false;
    }

    protected bool AddPauseReq()
    {
        switch (m_state)
        {
            case ELEVATOR_STATE.MOVING:
                m_state = ELEVATOR_STATE.PAUSED_MOVING;
                break;
            case ELEVATOR_STATE.BOARDING:
                m_state = ELEVATOR_STATE.PAUSED_BOARDING;
                break;
            case ELEVATOR_STATE.PAUSED_BOARDING:
                m_state = ELEVATOR_STATE.BOARDING;
                break;
            case ELEVATOR_STATE.PAUSED_MOVING:
                m_state = ELEVATOR_STATE.MOVING;
                break;
        }

        return true;
    }



    public void Execute()
    {
        switch (m_state)
        {
            case ELEVATOR_STATE.PAUSED_BOARDING:
            case ELEVATOR_STATE.PAUSED_MOVING:
                return;
            case ELEVATOR_STATE.MOVING:
                Moving();
                break;
            case ELEVATOR_STATE.BOARDING:
                Boarding();
                break;
            case ELEVATOR_STATE.WAITING:
                Waiting();
                break;
            default:
                Debug.LogError("Unexpected elevator state: " + m_state);
                return;
        }
    }

    protected void Moving()
    {
        m_timer -= Time.deltaTime;
        if (m_timer <= 0.0f)
        {
            if (m_movingDirection == DIRECTION.UP)
            {
                Position++;
            }
            else
            {
                Position--;
            }

            if (m_currentMovingPlan.Contains(Position))
            {
                StartBoarding();
                m_currentMovingPlan.Remove(Position);
            }
            else
            {
                m_timer += m_timeForPassingOneFloor;
            }
        }
    }

    protected void Boarding()
    {
        m_timer -= Time.deltaTime;
        if (m_timer <= 0.0f)
        {
            if (m_currentMovingPlan.Count > 0)
            {
                StartMoving();
            }
            else
            {
                StartWaiting();
            }
        }
    }

    protected void Waiting()
    {
        if (m_postponedTasks.Count > 0)
        {
            Request nextReq = m_postponedTasks.First.Value;
            m_postponedTasks.RemoveFirst();

            if (nextReq.Floor == Position)
            {
                StartBoarding();
                return;
            }

            DIRECTION targetDrection;
            if (nextReq.Floor > Position)
            {
                targetDrection = DIRECTION.UP;
            }
            else
            {
                targetDrection = DIRECTION.DOWN;
            }

            if (nextReq.Type == REQUEST_TYPE.DESTINATION_SET)
            {
                m_goalDirection = targetDrection;
                m_movingDirection = targetDrection;
                m_currentMovingPlan.Add(nextReq.Floor);
            }
            else if (nextReq.Type == REQUEST_TYPE.ELEVATOR_CALL)
            {
                m_movingDirection = targetDrection;
                m_goalDirection = nextReq.ReqDirection;

                if (m_movingDirection != m_goalDirection)
                {
                    m_changeDirectionIsPlanned = true;
                }

                m_currentMovingPlan.Add(nextReq.Floor);
            }

            StartMoving();
        }
    }

    protected void StartWaiting()
    {
        m_state = ELEVATOR_STATE.WAITING;
        if (ElematorLeaveFloor != null)
        {
            ElematorLeaveFloor();
        }
    }

    protected void StartBoarding(bool forsed = false)
    {
        m_state = ELEVATOR_STATE.BOARDING;
        m_timer = m_boardingTimer;

        if (m_changeDirectionIsPlanned && !forsed)
        {
            m_changeDirectionIsPlanned = false;
            m_movingDirection = m_goalDirection;
        }

        if (ElevatorStopMoving != null)
        {
            ElevatorStopMoving(Position);
        }
    }

    protected void StartMoving()
    {
        m_state = ELEVATOR_STATE.MOVING;
        m_timer = m_timeForPassingOneFloor;

        if (ElematorLeaveFloor != null)
        {
            ElematorLeaveFloor();
        }
    }
}
