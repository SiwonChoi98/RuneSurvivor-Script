using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StartButton : MonoBehaviour
{
    public AudioSource buttonClickSound;
    public void GameStart()
    {
        buttonClickSound.Play();
        LoadingSceneController.LoadScene("GameScene");
    }
    public void GameQuit()
    {
        buttonClickSound.Play();
        Application.Quit();
    }
}
