using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Highscore : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _highscoreUI;
    [SerializeField] private TimerUI _timerUI;
    
    private float _currentHighscore;
    private void Awake()
    {
        _currentHighscore = PlayerPrefs.GetFloat("Highscore");
        _highscoreUI.text = "HS: " + TimeSpan.FromSeconds(_currentHighscore).ToString(@"mm\:ss\:fff");

        StartCoroutine(SynchronizeHighscore());
    }

    
    private IEnumerator SynchronizeHighscore()
    {
        yield return new WaitForSeconds(_currentHighscore);
        while (true)
        {
            _highscoreUI.text = "HS: " + _timerUI.GetCurrentTime();
            
            PlayerPrefs.SetFloat("Highscore", _timerUI.GetCurrentScore());
            yield return null;
        }
    }
}
