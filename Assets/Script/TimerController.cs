using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerController : MonoBehaviour
{
    public static TimerController main;
    int gameRound = 1;
    float newGameTime = 0;
    public int roundCount = 3;
    public float roundTime = 30;
    List<TimeEntity> timeEntities = new List<TimeEntity>();
    private void Awake()
    {
        main = this;
        timeEntities.AddRange(FindObjectsOfType<TimeEntity>());

        PlayerController.player = GameObject.FindObjectOfType<PlayerController>();
    }
    private void Start()
    {
        UIManager.main.UpdateProgressBar(gameRound);
        StartNewGame();
    }
    Coroutine runningTime;
    public void StartNewGame()
    {
        if (runningTime!=null)
            StopCoroutine(runningTime);
        runningTime = StartCoroutine(HandleGameTime());
    }
    public IEnumerator HandleGameTime()
    {
        TimeReset();
        while (GetRemainingRoundTime() < roundTime)
            {
            UIManager.main.UpdateTime();
                yield return new WaitForEndOfFrame();
        }
        if (gameRound<=roundCount)
        {
            EndGameRound();
            StartNewGame();
        }
    }
    public void EndGameRound()
    {
        Debug.Log("TimeController - Reset time at "+Time.time);
        ClonePlayer(PlayerController.player);
        PlayerController.player.RewriteHistory();
        gameRound++;
        UIManager.main.UpdateProgressBar(gameRound);
    }
    public void GameReset()
    {
        if (runningTime != null)
            StopCoroutine(runningTime);
    }
    public void TimeReset()
    {
        Debug.Log("TimeController - Reset " + timeEntities.Count + " entities...");
        newGameTime = Time.time;
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
    public float GetRemainingRoundTime()
    {
        return Time.time - newGameTime + PlayerController.player.GetDelay();
    }

}
