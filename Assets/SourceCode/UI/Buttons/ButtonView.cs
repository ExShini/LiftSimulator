using UnityEngine;

class ButtonView : MonoBehaviour, IButtonView
{
    public event ButtonPush ButtonPushEvent;

    public void PushButton()
    {
        if (ButtonPushEvent != null)
        {
            ButtonPushEvent();
        }
    }
}
