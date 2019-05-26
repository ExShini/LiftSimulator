using System.Collections.Generic;
using UnityEngine;

class ElevatorInnerControlsView : MonoBehaviour, IElevatorInnerControlsView
{
    public GameObject PauseRequestBtn;
    public GameObject ContentField;

    List<IButtonWithLabel> m_controls;

    public void ConnectRequestBtnController(List<IElevatorBtnCtr> btnCrts)
    {
        if(m_controls == null)
        {
            Debug.LogError("We didn't initialize view collections!");
            return;
        }

        if(btnCrts.Count != m_controls.Count)
        {
            Debug.LogError("We can't match controls!");
            return;
        }

        for(int i = 0; i < btnCrts.Count; i++)
        {
            IButtonWithLabel view = m_controls[i];
            IElevatorBtnCtr ctr = btnCrts[i];

            view.ButtonPushEvent += ctr.OnBtnPush;
        }
    }

    public void CreateBtnViews(int numOfComponents)
    {
        m_controls = new List<IButtonWithLabel>(numOfComponents);
        UiObjectFactoryCtr uiObjFactory = UiObjectFactoryCtr.Instance;

        for (int ind = 0; ind < numOfComponents; ind++)
        {
            GameObject btnObj = uiObjFactory.CreateDestinationBtn();
            IButtonWithLabel viewCtr = btnObj.GetComponent<IButtonWithLabel>();

            viewCtr.SetLabel("Floor " + (ind + 1).ToString());
            btnObj.transform.SetParent(ContentField.transform);

            m_controls.Add(viewCtr);
        }
    }

    public void ConnectPauseBtnController(IElevatorBtnCtr pauseBtnCtr)
    {
        if(PauseRequestBtn == null)
        {
            Debug.LogError("We didn't set pause btn!");
            return;
        }

        IButtonView view = PauseRequestBtn.GetComponent<IButtonView>();

        if (view == null)
        {
            Debug.LogError("We didn't set correct veiw controller for pause btn!");
            return;
        }

        view.ButtonPushEvent += pauseBtnCtr.OnBtnPush;
    }
}
