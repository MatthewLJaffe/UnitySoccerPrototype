
using System;
using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class ScoreTracker : MonoBehaviour
{
    private bool _timerOn;
    private float decisionTime;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI decisionScoreText;
    [SerializeField] private TextMeshProUGUI timePenaltyText;
    [SerializeField] private GameObject endMenu;
    [SerializeField] private GameObject soccerPlayers;
    public UnityEvent onTimeStart;




    public static Action gameStopped = delegate {  };
    [SerializeField] private float maxScore;
    
    
    public void StartTimer()
    {
        _timerOn = true;
        onTimeStart.Invoke();
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
        if (decisionTime <= 0)
            decisionTime = .01f;
        var timePenalty = -Mathf.RoundToInt(Mathf.Clamp(decisionTime * 10 - 20, 0, 100));
        if (baseScore == 0)
            timePenalty = 0;
        scoreText.text = "Score: " + Mathf.RoundToInt(baseScore + timePenalty) + "/100";
        timePenaltyText.text = "Time Penalty: " + timePenalty; 
        decisionScoreText.text = "Decision Score: " + baseScore;
        timeText.text = "Time: " + Math.Round(decisionTime, 2) + " sec";
        StartCoroutine(WaitToShowMenu());
    }

    private IEnumerator WaitToShowMenu()
    {
        yield return new WaitForSeconds(1);
        soccerPlayers.SetActive(false);
        endMenu.SetActive(true);
    }
}
