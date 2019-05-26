using Declarations;

class RequstElevatorBtnCtr: IElevatorBtnCtr, IPlayerPositionHandler
{
    public event RequesPosted RequestEvent;
    readonly DIRECTION m_reqDirection;
    int m_currentPosition;

    public RequstElevatorBtnCtr(DIRECTION direction)
    {
        m_reqDirection = direction;
    }

    public void OnBtnPush()
    {
        Request req = new Request();
        req.Type = REQUEST_TYPE.ELEVATOR_CALL;
        req.Floor = m_currentPosition;
        req.ReqDirection = m_reqDirection;

        if (RequestEvent != null)
        {
            RequestEvent(req);
        }
    }

    public void OnPlayerPositionChanged(int position)
    {
        m_currentPosition = position;
    }
}

