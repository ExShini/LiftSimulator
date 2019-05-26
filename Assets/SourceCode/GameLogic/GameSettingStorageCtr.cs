using UnityEngine;

public class GameSettingStorageCtr : MonoBehaviour
{
    public static GameSettingStorageCtr Instance { get; private set; }
    public int NumOfFloors { get; set; }

    public int LowFloorLimit;
    public int UpFloorLimit;
    public int DefaultFloorNumbers;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }


}
