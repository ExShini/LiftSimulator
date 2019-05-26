using UnityEngine;

public class UiObjectFactoryCtr : MonoBehaviour
{
    public static UiObjectFactoryCtr Instance { get; private set; }

    public GameObject DestinationBtnPrefab;

    private void Awake()
    {
        Instance = this;
    }

    public GameObject CreateDestinationBtn()
    {
        if(DestinationBtnPrefab == null)
        {
            Debug.LogError("We didn't set button prefab!");
            return null;
        }

        GameObject btnInstance = (GameObject)GameObject.Instantiate(DestinationBtnPrefab); 
        return btnInstance;
    }

}
