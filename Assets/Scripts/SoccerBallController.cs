using System.Collections;
using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.Playables;

public class SoccerBallController : MonoBehaviour
{
    public UnityEvent<float> onGameComplete;
    public static Action gameComplete = delegate { };
    [SerializeField] private AudioSource kickSound;
    [SerializeField] private ScoreTracker scoreTracker;
    [SerializeField] private ChoiceTarget[] choiceTargets;
    [SerializeField] private AnimationCurve translateCurve;
    [SerializeField] private AnimationCurve heightCurve;
    [SerializeField] private AnimationCurve rotateCurve;
    [SerializeField] private Transform footballStartPos;
    [SerializeField] private EndMenu endMenu;
    [SerializeField] private PlayableDirector timeline;
    private Vector3 _initialPos;
    private Vector3 _initialRot;
    private Vector3 _targetPos;
    private Quaternion _targetRot;
    private bool _gameGoing;
    private Coroutine _moveRoutine;
    private float _startTime;
    private float _endTime;

    
    [System.Serializable]
    private struct ChoiceTarget
    {
        public Transform target;
        public String choice;
        public KeyCode KeyCode;
        public float travelTime;
        public float score;
        public AudioClip feedbackNoise;
        public int analysisScreenIdx;
    }

    private void Update()
    {

        if (!_gameGoing) return;
        foreach (var ct in choiceTargets)
        {
            if ((Input.GetButton(ct.choice) || Input.GetKeyDown(ct.KeyCode)) && _moveRoutine == null)
            {
                _moveRoutine = StartCoroutine(MoveBall(ct));
            }
        }
    }

    public void SetGameGoing(bool b)
    {
        _gameGoing = b;
    }

    private IEnumerator MoveBall(ChoiceTarget ct)
    {
        kickSound.Play();
        scoreTracker.StopTimer();
        _initialRot = transform.rotation.eulerAngles;
        _initialPos = transform.position;
        _targetPos = ct.target.position;
        _targetRot = ct.target.rotation;
        _startTime = (float)timeline.time;
        _endTime = _startTime + ct.travelTime;
        var height = Vector3.Distance(ct.target.position, _initialPos) / 15f;
        for (var t = 0f; t < ct.travelTime; t += Time.deltaTime)
        {
            var newPos =  Vector3.Lerp(_initialPos, _targetPos, translateCurve.Evaluate(t/ct.travelTime));
            newPos.y = 1;
            newPos.y += Mathf.Lerp(0, height, heightCurve.Evaluate(t/ct.travelTime));
            transform.position = newPos;
            transform.rotation = 
                Quaternion.Euler(Vector3.Lerp(_initialRot, _targetRot.eulerAngles, rotateCurve.Evaluate(t/ct.travelTime)));
            yield return null;
        }

        kickSound.clip = ct.feedbackNoise;
        kickSound.Play();
        onGameComplete.Invoke(ct.score);
        gameComplete.Invoke();
        endMenu.anaysisScreenToShow = ct.analysisScreenIdx;
    }

    public void ReplayBall(float time)
    {
        var height = Vector3.Distance(_targetPos, _initialPos) / 15f;
        if (time < _startTime || time > _endTime) return;
        var currT = (time - _startTime) / (_endTime - _startTime);
        var newPos = Vector3.Lerp(_initialPos, _targetPos, translateCurve.Evaluate(currT));
        newPos.y = 1;
        newPos.y += Mathf.Lerp(0, height, heightCurve.Evaluate(currT));
        transform.position = newPos;
        transform.rotation =
            Quaternion.Euler(Vector3.Lerp(_initialRot, _targetRot.eulerAngles, rotateCurve.Evaluate(currT)));
    }

    public void SetFootballStartPos()
    {
        transform.position = footballStartPos.position;
    }

    public void ResetBall()
    {
        transform.position = Vector3.zero;
    }
}

