using Declarations;
using UnityEngine;

class RequestPanelViewCtr : MonoBehaviour, IRequestPanelView
{
    public GameObject UpRequestBtn;
    public GameObject DownRequestBtn;
    
    public void ConnectRequestBtnController(DIRECTION btnReqDirection, IElevatorBtnCtr ctr)
    {
        GameObject btnObj = null;
        switch (btnReqDirection)
        {
            case DIRECTION.DOWN:
                btnObj = DownRequestBtn;
                break;
            case DIRECTION.UP:
                btnObj = UpRequestBtn;
                break;
        }

        if(btnObj == null)
        {
            Debug.LogError("We didn't set request button! Button direction: " + btnReqDirection);
            return;
        }

        IButtonView view = btnObj.GetComponent<IButtonView>();
        if (view == null)
        {
            Debug.LogError("We didn't set proper view control for button! Button direction: " + btnReqDirection);
            return;
        }

        view.ButtonPushEvent += ctr.OnBtnPush;
    }

    public void SetRequestBtnVisualState(DIRECTION btnReqDirection, bool isAvailable)
    {
        switch(btnReqDirection)
        {
            case DIRECTION.DOWN:
                if(DownRequestBtn != null)
                {
                    DownRequestBtn.SetActive(isAvailable);
                }
                break;
            case DIRECTION.UP:
                if (UpRequestBtn != null)
                {
                    UpRequestBtn.SetActive(isAvailable);
                }
                break;
        }
    }
}
