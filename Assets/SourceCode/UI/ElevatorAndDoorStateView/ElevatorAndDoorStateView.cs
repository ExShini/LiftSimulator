using UnityEngine;
using UnityEngine.UI;

public class ElevatorAndDoorStateView : MonoBehaviour, IElevatorAndDoorStateView
{
    public Text ElevatorPositionText;
    public Text DoorStateText;

    public void SerDoorState(bool isOpen)
    {
        if(DoorStateText == null)
        {
            Debug.LogError("DoorStateText is null");
            return;
        }

        if(isOpen)
        {
            DoorStateText.text = "The door is Open";
        }
        else
        {
            DoorStateText.text = "The door is Closed";
        }
    }

    public void SetFloor(int floor)
    {
        if (ElevatorPositionText == null)
        {
            Debug.LogError("ElevatorPositionText is null");
            return;
        }

        ElevatorPositionText.text = "Elevator on floor " + floor;
    }
}
