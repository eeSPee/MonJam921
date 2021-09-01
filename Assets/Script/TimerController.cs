using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerController : MonoBehaviour
{
    float gameTime = 0;
    List<TimeEntity> timeEntities = new List<TimeEntity>();
    private void Start()
    {
        timeEntities.AddRange(FindObjectsOfType<TimeEntity>());

        PlayerController.player = GameObject.FindObjectOfType<PlayerController>();

        TimeReset();

        float T = 5;
        Invoke("NewGame", T*1);
        Invoke("NewGame", T*2);
        Invoke("NewGame", T*3);
    }
    public void NewGame()
    {
        Debug.Log("TimeController - Reset time at "+Time.time);
        ClonePlayer(PlayerController.player);
        PlayerController.player.RewriteHistory();
        TimeReset();
    }
    public void TimeReset()
    {
        Debug.Log("TimeController - Reset " + timeEntities.Count + " entities...");
        foreach (TimeEntity entity in timeEntities)
        {
            entity.TimeReset();
        }
    }
    public CloneController ClonePlayer(PlayerController Spieler)
    {
        Debug.Log("TimeController - Clone " + Spieler.name);
        GameObject Clone = Instantiate(Resources.Load<GameObject>("Prefabs/Player/Clone"));

        CloneController Controller = Clone.GetComponent<CloneController>();
        Controller.MimicPlayer(Spieler);
        Controller.enabled = true;
        timeEntities.Add(Controller);

        return Controller;
    }
}
