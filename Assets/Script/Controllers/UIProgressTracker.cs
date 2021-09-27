using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIProgressTracker : MonoBehaviour
{
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
