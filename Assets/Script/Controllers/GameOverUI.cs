using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    public static GameOverUI main;
    Text UpdateText;
    private void Awake()
    {
        UpdateText = transform.GetChild(0).GetComponent<Text>();
        main = this;
    }
    private void OnEnable()
    {
        int[] progressData = UIProgressTracker.main.GetProgress();
        UpdateText.text = "Clocks: " + progressData[2] + "/" + progressData[3] + "\nFlies " + progressData[0] + "/" + progressData[1];
    }
}
