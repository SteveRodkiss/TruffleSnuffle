using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void OnPlayButtonClicked()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OnRestartButtonClicked()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void OnQuitButtonClicked()
    {
        Application.Quit();
    }

}
