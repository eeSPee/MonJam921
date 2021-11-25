using UnityEngine;
using UnityEngine.UI;

public class GUIController : MonoBehaviour
{
    //--------------------
    //  GUI Controller
    //--------------------
    //This script controlls the timer on top

    //  VARS
    //  TimerLabel - Text component of timer
    //  FUNCTIONS
    //  Awake - declare components
    //  UpdateTime - updates the remaining time on the UI

    public static GUIController main;
    Text TimerLabel;
    void Awake()
    {
        main = this;
        TimerLabel = transform.Find("Timer").GetComponent<Text>();
    }
    public void UpdateTime()
    {
        TimerLabel.text = Mathf.Ceil(GameController.main.roundTime - GameController.main.GetRemainingRoundTime())+"";
    }
}
