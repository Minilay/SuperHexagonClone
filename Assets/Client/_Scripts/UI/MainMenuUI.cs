using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public void PlayButton()
    {
        Debug.Log("Play");

        SceneManager.LoadScene(1); 
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
