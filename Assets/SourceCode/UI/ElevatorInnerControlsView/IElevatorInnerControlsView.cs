using System.Collections.Generic;

interface IElevatorInnerControlsView
{
    void CreateBtnViews(int numOfComponents);
    void ConnectRequestBtnController(List<IElevatorBtnCtr> btnCrts);
    void ConnectPauseBtnController(IElevatorBtnCtr pauseBtnCtr);
}

