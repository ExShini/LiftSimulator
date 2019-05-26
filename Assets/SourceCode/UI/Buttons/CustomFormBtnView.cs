using System.Collections;
using UnityEngine;
using UnityEngine.UI;

class CustomFormBtnView : ButtonView
{
    void Start()
    {
        Image buttonImg = GetComponent<Image>();
        buttonImg.alphaHitTestMinimumThreshold = 0.5f;
    }
}
