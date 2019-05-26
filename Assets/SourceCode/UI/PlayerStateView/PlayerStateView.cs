using UnityEngine;
using UnityEngine.UI;
using Declarations;

public class PlayerStateView : MonoBehaviour, IPlayerStateView
{
    public Text PlayerPositionText;
    public Text PlayerStateText;

    public void SerPlayerState(PLAYER_STATE state)
    {
        if(PlayerStateText != null)
        {
            switch(state)
            {
                case PLAYER_STATE.IN_ELEVATOR:
                    PlayerStateText.text = "Player in elevator";
                    break;
                case PLAYER_STATE.OUT_ELEVATOR:
                    PlayerStateText.text = "Player on floor";
                    break;
                default:
                    Debug.LogError("Wrong state!");
                    break;
            }
        }
        else
        {
            Debug.LogError("PlayerStateText is null!");
        }
    }

    public void SetFloor(int floor)
    {
        if(PlayerPositionText != null)
        {
            PlayerPositionText.text = "Player on floor " + floor;
        }
        else
        {
            Debug.LogError("PlayerPositionText is null!");
        }
    }
}
