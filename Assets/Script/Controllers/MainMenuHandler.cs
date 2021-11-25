using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour
{
    //--------------------
    //  MAIN MENU HANDLER
    //--------------------
    //This script controlls scene transition in the menu

    //  VARS
    //  PlayScene - TextScene to transition to
    //  PlayBtn - Button in charge
    //  FUNCTIONS
    //  Awake - assigns a listener that changes the scene to our desired scene

    public string PlayScene = "MechanicsPlayground";
    Button PlayBtn;
    void Awake()
    {
        PlayBtn = GetComponent<Button>();
        PlayBtn.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(PlayScene);
        });
    }
}
