using Declarations;
using System.Collections.Generic;
using UnityEngine;

class UIManager : MonoBehaviour, IPlayerPositionHandler, IPlayerStateHandler, IElevatorPositionHandler, IDoorStateHandler, IButtonPresenter
{
    public GameObject PlayerStatePanel;
    public GameObject ElevatorAndDoorStatePanel;
    public GameObject ElevatorRequestPanel;
    public GameObject ElevatorControlPanel;

    IElevatorAndDoorStateView m_elevatorAndDoorStateView;
    IPlayerStateView m_playerStateView;
    IRequestPanelView m_requestPanelView;
    IElevatorInnerControlsView m_elevatorInnerControlsView;

    int m_lowFloorNumber;
    int m_topFloorNumber;

    public static UIManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void Initialize()
    {
        m_elevatorAndDoorStateView = ElevatorAndDoorStatePanel.GetComponent<IElevatorAndDoorStateView>();
        m_playerStateView = PlayerStatePanel.GetComponent<IPlayerStateView>();
        m_requestPanelView = ElevatorRequestPanel.GetComponent<IRequestPanelView>();
        m_elevatorInnerControlsView = ElevatorControlPanel.GetComponent<IElevatorInnerControlsView>();

        if (m_elevatorAndDoorStateView == null ||
            m_playerStateView == null ||
            m_requestPanelView == null ||
            m_elevatorInnerControlsView == null)
        {
            Debug.LogError("We didn't set proper components");
            return;
        }

        GameSettingStorageCtr gss = GameSettingStorageCtr.Instance;
        m_topFloorNumber = gss.NumOfFloors;
        m_lowFloorNumber = 1;

        m_elevatorInnerControlsView.CreateBtnViews(gss.NumOfFloors);
    }

    public void ConnectRequestBtnView(DIRECTION direction, IElevatorBtnCtr ctr)
    {
        m_requestPanelView.ConnectRequestBtnController(direction, ctr);
    }

    public void ConnectElevatorControlBtnView(List<IElevatorBtnCtr> btnControls)
    {
        m_elevatorInnerControlsView.ConnectRequestBtnController(btnControls);
    }

    public void ConnectElevatorPausebtn(IElevatorBtnCtr ctr)
    {
        m_elevatorInnerControlsView.ConnectPauseBtnController(ctr);
    }

    public void OnElevatorPositionChanged(int position)
    {
        m_elevatorAndDoorStateView.SetFloor(position);
    }

    public void OnPlayerPositionChanged(int position)
    {
        m_playerStateView.SetFloor(position);

        if (position <= m_lowFloorNumber)
        {
            m_requestPanelView.SetRequestBtnVisualState(DIRECTION.DOWN, false);
        }
        else if (position >= m_topFloorNumber)
        {
            m_requestPanelView.SetRequestBtnVisualState(DIRECTION.UP, false);
        }
        else
        {
            m_requestPanelView.SetRequestBtnVisualState(DIRECTION.UP, true);
            m_requestPanelView.SetRequestBtnVisualState(DIRECTION.DOWN, true);
        }
    }

    public void OnPlayerStateChanged(PLAYER_STATE state)
    {
        switch(state)
        {
            case PLAYER_STATE.IN_ELEVATOR:
                ElevatorControlPanel.SetActive(true);
                ElevatorRequestPanel.SetActive(false);
                break;
            case PLAYER_STATE.OUT_ELEVATOR:
                ElevatorControlPanel.SetActive(false);
                ElevatorRequestPanel.SetActive(true);
                break;
            default:
                Debug.LogError("We tryed to use wrong state: " + state);
                return;
        }

        m_playerStateView.SerPlayerState(state);
    }

    public void OnDoorStateChanged(bool isOpen)
    {
        m_elevatorAndDoorStateView.SerDoorState(isOpen);
    }
}
