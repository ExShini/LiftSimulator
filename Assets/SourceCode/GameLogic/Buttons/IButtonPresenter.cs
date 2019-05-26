using System.Collections.Generic;
using Declarations;

interface IButtonPresenter
{
    void ConnectRequestBtnView(DIRECTION direction, IElevatorBtnCtr ctr);
    void ConnectElevatorControlBtnView(List<IElevatorBtnCtr> btnControls);
    void ConnectElevatorPausebtn(IElevatorBtnCtr ctr);
}
