
using System;
using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.Playables;


public class ScoreTracker : MonoBehaviour
{
    private bool _timerOn;
    private float decisionTime;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI decisionScoreText;
    [SerializeField] private TextMeshProUGUI timePenaltyText;
    [SerializeField] private GameObject endMenu;
    [SerializeField] private GameObject soccerPlayers;
    [SerializeField] private SoccerBallController soccerBallController;
    [SerializeField] private GameSettings gameSettings;
    public UnityEvent onTimeStart;
    private bool _scoreComputed;




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
        if (!_timerOn)
        {
            if (decisionTime <= 0)
                decisionTime = .01f;
            var timePenalty = -Mathf.RoundToInt(Mathf.Clamp(decisionTime * 10 - 20, 0, 100));
            if (baseScore == 0)
                timePenalty = 0;
            titleText.text = "Scenario Complete";
            scoreText.text = "Score: " + Mathf.RoundToInt(baseScore + timePenalty) + "/100";
            timePenaltyText.text = "Time Penalty: " + timePenalty;
            decisionScoreText.text = "Decision Score: " + baseScore;
            timeText.text = "Time: " + Math.Round(decisionTime / Mathf.Lerp(.75f, 1.25f, gameSettings.speed / 10), 2) + " sec";
        }
        else
        {
            titleText.text = "Ran out of time!";
            scoreText.text = "Score: 0";
            timePenaltyText.text = "Time: " + Math.Round(decisionTime / Mathf.Lerp(.75f, 1.25f, gameSettings.speed / 10), 2) + " sec";
            decisionScoreText.text = "";
            timeText.text = "";
            soccerBallController.endTime = (float)soccerBallController.timeline.duration;
        }
        StartCoroutine(WaitToShowMenu());
    }

    private IEnumerator WaitToShowMenu()
    {
        yield return new WaitForSeconds(1);
        soccerPlayers.SetActive(false);
        endMenu.SetActive(true);
    }
}
