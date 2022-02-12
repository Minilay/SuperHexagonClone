using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timerUI;

    private float _currentTime;
    private string _timerText;
    private void Update()
    {
        _currentTime += Time.deltaTime;

        _timerText = TimeSpan.FromSeconds(_currentTime).ToString(@"mm\:ss\:fff");
        _timerUI.text = _timerText;
    }

    public string GetCurrentTime() => _timerText;

    public float GetCurrentScore() => _currentTime;
}
