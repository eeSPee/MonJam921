using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour
{
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
