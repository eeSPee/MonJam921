using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    /* TODO
	 * BG
	 * mushrooms
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
        GUIController.main.UpdateProgressBar(gameRound);
        GameOverUI.main.gameObject.SetActive(false);
        StartNewGame();
    }
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
        GUIController.main.UpdateProgressBar(gameRound);
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
        // return Time.time - newGameTime + PlayerController.player.GetDelay();
        return Time.time - newGameTime;
    }
    public void Update()
    {
        if (Input.GetButtonUp("Reset"))
        {
            HandleReset();
        }
    }
    public void HandleReset()
    {
        Debug.Log("PLAYER RESET");
        if (IsGameOver() || GetRemainingRoundTime()<.33f)
        {
            Scene currentscene = SceneManager.GetActiveScene(); 
            SceneManager.LoadScene(currentscene.name);
        }
        else
        {
            StartNewGame();
        }
    }
}
