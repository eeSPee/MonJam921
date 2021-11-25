using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIProgressTracker : MonoBehaviour
{
    //--------------------
    //  UI PROGRESS TRACKER
    //--------------------
    //This script controlls the UI topleft that tracks player progress

    //  VARS
    //  ProgressBar, GoalTracker, PickTracker - attached components
    //  pickups, tpickups, goals, tgoals - Progress and total pickups/objectives
    //  FUNCTIONS
    //  Awake - declare components
    //  InitialCountDown - Count all the current pickups
    //  UpdateScoreText - change the text on the components
    //  IncrementProgress - increases the counter and updates text
    //  GetProgress - get the current pickup/goal progress
    //  UpdateProgressBar - update the n of icons in the progress bar
    //  ResetProgress - resets the progress to 0

    public static UIProgressTracker main;
    Transform ProgressBar;
    Text GoalTracker;
    Text PickTracker;

    int pickups = 0;
    int tpickups = 0;
    int goals = 0;
    int tgoals = 0;
    void Awake()
    {
        main = this;
        ProgressBar = transform.Find("ProgressBar");
        GoalTracker = transform.Find("Goal Tracker").GetComponent<Text>();
        PickTracker = transform.Find("Pickup Tracker").GetComponent<Text>();
        InitialCountDown();
    }
    public void InitialCountDown()
    {
        foreach (TimePickup Pickup in FindObjectsOfType<TimePickup>())
        {
            if (Pickup.Important)
            {
                tgoals++;
            }
            else
            {
                tpickups++;
            }
        }
    }
    public void ResetProgress()
    {
        pickups = 0;
        goals = 0;
        UpdateScoreText();
    }
    public void IncrementProgress(bool goal)
    {
        if (!goal)
            pickups++;
        else
            goals++;
        UpdateScoreText();
    }
    public void UpdateScoreText()
    {
        GoalTracker.text = goals + " / " + tgoals;
        PickTracker.text = pickups + " / " + tpickups;
    }
    public int[] GetProgress()
    {
        return new int[] { pickups, tpickups, goals, tgoals };
    }
    public void UpdateProgressBar(int progress)
    {
        for (int I = 0; I < ProgressBar.childCount; I++)
        {
            ProgressBar.GetChild(I).gameObject.SetActive(I < progress);
        }
    }
}
