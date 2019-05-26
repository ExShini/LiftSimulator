using Declarations;

interface IRequestPanelView
{
    void ConnectRequestBtnController(DIRECTION btnReqDirection, IElevatorBtnCtr ctr);
    void SetRequestBtnVisualState(DIRECTION btnReqDirection, bool isAvailable);
}
