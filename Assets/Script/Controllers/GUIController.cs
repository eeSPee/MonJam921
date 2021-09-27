using UnityEngine;
using UnityEngine.UI;

public class GUIController : MonoBehaviour
{
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
