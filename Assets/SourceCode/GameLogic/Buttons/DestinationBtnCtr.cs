using Declarations;

class DestinationBtnCtr : IElevatorBtnCtr
{
    public event RequesPosted RequestEvent;
    readonly int m_targetFloor;

    public DestinationBtnCtr(int targetFloor)
    {
        m_targetFloor = targetFloor;
    }

    public void OnBtnPush()
    {
        Request req = new Request();
        req.Type = REQUEST_TYPE.DESTINATION_SET;
        req.Floor = m_targetFloor;

        if(RequestEvent != null)
        {
            RequestEvent(req);
        }
    }
}

