using System.Collections.Generic;
using Declarations;
using UnityEngine;

class ButtonManager
{
    protected IElevatorBtnCtr m_upReqBtn;
    protected IElevatorBtnCtr m_downReqBtn;
    protected IElevatorBtnCtr m_pauseBtn;
    List<IElevatorBtnCtr> m_destReqBtns;

    public void InitializeControllers()
    {
        int numOfFloors = GameSettingStorageCtr.Instance.NumOfFloors;

        m_upReqBtn = new RequstElevatorBtnCtr(DIRECTION.UP);
        m_downReqBtn = new RequstElevatorBtnCtr(DIRECTION.DOWN);
        m_pauseBtn = new PauseBtnCtr();
        m_destReqBtns = new List<IElevatorBtnCtr>(numOfFloors);
        int firstFloorNumber = 1;

        for (int i = 0; i < numOfFloors; i++)
        {
            m_destReqBtns.Add(new DestinationBtnCtr(i + firstFloorNumber));
        }
    }

    public void ConnectWithPresenter(IButtonPresenter presenter)
    {
        presenter.ConnectElevatorControlBtnView(m_destReqBtns);
        presenter.ConnectElevatorPausebtn(m_pauseBtn);
        presenter.ConnectRequestBtnView(DIRECTION.UP, m_upReqBtn);
        presenter.ConnectRequestBtnView(DIRECTION.DOWN, m_downReqBtn);
    }

    public void ConnectWithElevator(IElevatorSimulation elevator)
    {
        m_upReqBtn.RequestEvent += elevator.ApplyRequest;
        m_downReqBtn.RequestEvent += elevator.ApplyRequest;
        m_pauseBtn.RequestEvent += elevator.ApplyRequest;

        for(int i = 0; i < m_destReqBtns.Count; i++)
        {
            IElevatorBtnCtr btnCtr = m_destReqBtns[i];
            btnCtr.RequestEvent += elevator.ApplyRequest;
        }
    }

    public void ConnectWithPlayer(PlayerController plCtr)
    {
        IPlayerPositionHandler requestBtn = m_upReqBtn as IPlayerPositionHandler;
        plCtr.PlayerPositionChanged += requestBtn.OnPlayerPositionChanged;

        requestBtn = m_downReqBtn as IPlayerPositionHandler;
        plCtr.PlayerPositionChanged += requestBtn.OnPlayerPositionChanged;

        for(int i = 0; i < m_destReqBtns.Count; i++)
        {
            IElevatorBtnCtr btnCtr = m_destReqBtns[i];
            btnCtr.RequestEvent += plCtr.OnPlayerMakeDecision;
        }

    }
}
