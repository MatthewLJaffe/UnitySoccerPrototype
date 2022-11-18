
using System;
using TMPro;
using UnityEngine;

public class ScoreTracker : MonoBehaviour
{
    private bool _timerOn;
    private float decisionTime;
    [SerializeField] private TextMeshProUGUI scoreText;
    public static Action gameStopped = delegate {  };
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private float maxScore;
    [SerializeField] private float startDelay;
    
    
    public void StartTimer()
    {
        _timerOn = true;
    }
    
    public void StopTimer()
    {
        _timerOn = false;
        gameStopped.Invoke();
    }

    private void Update()
    {
        if (_timerOn)
        {
            decisionTime += Time.deltaTime;
        }
    }

    public void ComputeScore(float baseScore)
    {
        decisionTime -= startDelay;
        if (decisionTime <= 0)
            decisionTime = .01f;
        scoreText.text = "Score: " + Mathf.RoundToInt(Mathf.Clamp(baseScore / (.25f * decisionTime), 0, maxScore)) + "/100";
        timeText.text = "Time: " + Math.Round(decisionTime, 2) + " sec";
    }
}
