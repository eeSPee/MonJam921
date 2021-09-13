using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager main;
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
        TimerLabel.text = Mathf.Ceil(TimerController.main.GetRemainingRoundTime())+"";
    }
    public void UpdateProgressBar(int progress)
    {
        for (int I =0; I< ProgressBar.childCount; I++)
        {
            ProgressBar.GetChild(I).gameObject.SetActive(I < progress);
        }
    }
}
