using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    /* TODO
     * game over UI
     * press to play
     * trumpet enemy
     * */


    public static GameController main;
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
        GUIController.main.UpdateProgressBar(gameRound);
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
            GUIController.main.UpdateTime();
                yield return new WaitForEndOfFrame();
        }
        if (gameRound<roundCount)
        {
            EndGameRound();
            StartNewGame();
        }
        else
        {
            if (CheckGameState())
            {
                Debug.Log("Game Won");
            }else
            {

                Debug.Log("Game Lost");
            }
        }
    }
    public void EndGameRound()
    {
        Debug.Log("TimeController - Reset time at "+Time.time);
        ClonePlayer(PlayerController.player);
        PlayerController.player.RewriteHistory();
        gameRound++;
        GUIController.main.UpdateProgressBar(gameRound);
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
    public void FixedUpdate()
    {
        Camera.main.transform.position = new Vector3(PlayerController.player.transform.position.x, PlayerController.player.transform.position.y,-10);
    }
    public bool CheckGameState()
    {
        foreach (TimePickup Pickup in FindObjectsOfType<TimePickup>())
        {
            if (Pickup.Important && Pickup.state)
            {
                return false;
            }
        }
        return true;
    }
}
