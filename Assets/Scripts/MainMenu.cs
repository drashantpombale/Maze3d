using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public Button start;
    public Button exit;
    // Start is called before the first frame update
    void Start()
    {
        start.onClick.AddListener(StartGame);
        exit.onClick.AddListener(ExitGame);
    }

    private void ExitGame()
    {
        Application.Quit();
    }

    private void StartGame()
    {
        SceneController.Instance.LoadGameScene(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
