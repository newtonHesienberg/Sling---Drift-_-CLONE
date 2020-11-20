using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool start = false;

    public GameObject startUI;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(start)
        {
            Time.timeScale = 1f;
            startUI.SetActive(false);
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        start = true;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
