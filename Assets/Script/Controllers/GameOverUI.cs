using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    //--------------------
    //  GAME OVER UI
    //--------------------
    //This script controlls the popup game over text

    //  VARS
    //  UpdateText - Text component that displays progress
    //  FUNCTIONS
    //  Awake - declare components
    //  OnEnable - updates the progress on the text component
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
