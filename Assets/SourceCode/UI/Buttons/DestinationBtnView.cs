using UnityEngine.UI;

class DestinationBtnView : ButtonView, IButtonWithLabel
{
    public Text label;

    public void SetLabel(string labelText)
    {
        label.text = labelText;
    }
}
