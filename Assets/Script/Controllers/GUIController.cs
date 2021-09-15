using UnityEngine;
using UnityEngine.UI;

public class GUIController : MonoBehaviour
{
    public static GUIController main;
    Text TimerLabel;
    Transform ProgressBar;
    void Awake()
    {
        main = this;
        TimerLabel = transform.Find("Timer").GetComponent<Text>();
        ProgressBar = transform.Find("ProgressBar");
    }
    public void UpdateTime()
    {
        TimerLabel.text = Mathf.Ceil(GameController.main.GetRemainingRoundTime())+"";
    }
    public void UpdateProgressBar(int progress)
    {
        for (int I =0; I< ProgressBar.childCount; I++)
        {
            ProgressBar.GetChild(I).gameObject.SetActive(I < progress);
        }
    }
}
