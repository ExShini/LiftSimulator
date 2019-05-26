using Declarations;

class PauseBtnCtr : IElevatorBtnCtr
{
    public event RequesPosted RequestEvent;

    public void OnBtnPush()
    {
        Request req = new Request();
        req.Type = REQUEST_TYPE.PAUSE_ELEVATOR;
        req.Floor = 0;

        if (RequestEvent != null)
        {
            RequestEvent(req);
        }
    }
}
