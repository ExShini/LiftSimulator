using UnityEngine;
using UnityEngine.UI;

public class SettingsPanelView : MonoBehaviour
{
    public Button LeftArrowBtn;
    public Button RightArrowBtn;
    public Button StartSimulationBtn;
    public Text NumOfFloorText;

    protected int m_lowLimit;
    protected int m_UpLimit;
    protected int m_currentFloorsNumber;

    void Start()
    {
        GameSettingStorageCtr gss = GameSettingStorageCtr.Instance;
        m_lowLimit = gss.LowFloorLimit;
        m_UpLimit = gss.UpFloorLimit;
        m_currentFloorsNumber = gss.DefaultFloorNumbers;

        SetHandlerForButton(LeftArrowBtn, DecreaseNumOfFloors);
        SetHandlerForButton(RightArrowBtn, IncreaseNumOfFloors);
        SetHandlerForButton(StartSimulationBtn, StartSimulation);

        UpdateCounter();
    }


    void SetHandlerForButton(Button btn, ButtonPush handler)
    {
        if (btn == null)
        {
            Debug.LogError("We didn't set button!");
            return;
        }

        IButtonView view = btn.GetComponent<IButtonView>();
        if (view == null)
        {
            Debug.LogError("Button have not ButtonView controller!");
            return;
        }

        view.ButtonPushEvent += handler;
    }


    void IncreaseNumOfFloors()
    {
        m_currentFloorsNumber++;
        if (m_currentFloorsNumber > m_UpLimit)
        {
            m_currentFloorsNumber = m_UpLimit;
        }

        UpdateCounter();
    }

    void DecreaseNumOfFloors()
    {
        m_currentFloorsNumber--;
        if (m_currentFloorsNumber < m_lowLimit)
        {
            m_currentFloorsNumber = m_lowLimit;
        }

        UpdateCounter();
    }

    void UpdateCounter()
    {
        if (NumOfFloorText == null)
        {
            Debug.LogError("We didn't set NumOfFloorText!");
            return;
        }

        NumOfFloorText.text = m_currentFloorsNumber.ToString();
    }

    void StartSimulation()
    {
        GameSettingStorageCtr gss = GameSettingStorageCtr.Instance;
        gss.NumOfFloors = m_currentFloorsNumber;
        SimulationInitializer.Instance.LoadSimulationScene();
    }
}
