using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    //--------------------
    //  GAME CONTROLLER
    //--------------------
    //This script controlls the game loop and reset

    //  REGIONS
    //  Audio - Audio static vars
    //  MonoBehavior - Basic Monobehavior functions, Start updates UI, Update checks reset
    //  Game Loop - The game loop, starts the game, countdown and reset the time

    #region Audio
    public static float MinListenerDistance = 1;
    public static float MaxListenerDistance = 25;
    #endregion
    #region MonoBehavior
    List<TimeEntity> timeEntities = new List<TimeEntity>();
    public static GameController main;
    private void Awake()
    {
        Application.targetFrameRate = 60;
        main = this;
        timeEntities.AddRange(FindObjectsOfType<TimeEntity>());
        PlayerController.player = GameObject.FindObjectOfType<PlayerController>();
        PlayerController.player.gameObject.SetActive(false);
        Camera.main.GetComponent<CameraController>().TrackTarget(PlayerController.player.gameObject);
        Cursor.visible = false;
    }
    private void Start()
    {
        UIProgressTracker.main.UpdateProgressBar(gameRound);
        GameOverUI.main.gameObject.SetActive(false);
        StartNewGame();
    }
    public void Update()
    {
        if (Input.GetButtonUp("Reset"))
        {
            HandleReset();
        }
    }
    #endregion
    #region Game Loop
    int gameRound = 1;
    float newGameTime = 0;
    public int roundCount = 3;
    public float roundTime = 30;
    Coroutine runningTime;
    public void StartNewGame()
    {
        PlayerController.player.gameObject.SetActive(true);
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
        if (gameRound<=roundCount)
        {
            EndGameRound();
        }
        if (IsGameOver() )
        {
            GameOverUI.main.gameObject.SetActive(true);
        }
        StartNewGame();
    }
    public void EndGameRound()
    {
        Debug.Log("TimeController - Reset time at "+Time.time);
        ClonePlayer(PlayerController.player);
        PlayerController.player.AudioSourcePlayer.PlayOneShot(PlayerController.player.AudioClipRewind);
        PlayerController.player.RewriteHistory();
        gameRound++;
        UIProgressTracker.main.UpdateProgressBar(gameRound);
    }
    public bool IsGameOver()
    {
        return gameRound > roundCount;
    }
    public void TimeReset()
    {
        Debug.Log("TimeController - Reset " + timeEntities.Count + " entities...");
        newGameTime = Time.time;
        foreach (TimeEntity entity in timeEntities)
        {
            entity.TimeReset();
        }
        UIProgressTracker.main.ResetProgress();
    }
    public float GetRemainingRoundTime()
    {
        // return Time.time - newGameTime + PlayerController.player.GetDelay();
        return Time.time - newGameTime;
    }
    public void HandleReset()
    {
        if (IsGameOver() || GetRemainingRoundTime() < .33f)
        {
            Scene currentscene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentscene.name);
        }
        else
        {
            StartNewGame();
        }
    }
    #endregion
    #region Player Clones
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
    #endregion
}
