using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    public static GameOverUI main;
    Text UpdateText;
    List<TimePickup> timePickups = new List<TimePickup>();
    private void Awake()
    {
        UpdateText = transform.GetChild(0).GetComponent<Text>();
        main = this;
        timePickups.AddRange(FindObjectsOfType<TimePickup>());
    }
    private void OnEnable()
    {
        int pickups = 0;
        int tpickups = 0;
        int goals = 0;
        int tgoals = 0;

        foreach (TimePickup Pickup in timePickups)
        {
            if (Pickup.Important)
            {
                tgoals++;
                if (!Pickup.state)
                {
                    goals++;
                }
            }
            else
            {
                tpickups++;
                if (!Pickup.state)
                {
                    pickups++;
                }
            }
        }
        UpdateText.text = "Clocks: " + goals + "/" + tgoals + "\nFlies " + pickups + "/" + tpickups;
    }
}
