
using System;
using UnityEngine;
using UnityEngine.Events;

public class MoveInTime : MonoBehaviour
{
    [SerializeField] private float time;
    [SerializeField] private Transform target;
    public UnityEvent onReachDestination;
    private Vector3 _initialPos;
    private float _currTime;
    private bool _gameGoing;
    private bool _atDestination;

    private void Awake()
    {
        _initialPos = transform.position;
        TransitionController.gameStart += delegate { _gameGoing = true; };
        ScoreTracker.gameStopped += delegate { _gameGoing = false; };
    }

    private void Update()
    {
        //if (!_gameGoing) return;
        if (_currTime < time)
        {        
            transform.position = Vector3.Lerp(_initialPos, target.position, _currTime/time);
            _currTime += Time.deltaTime;
        }
        else if (!_atDestination)
        {
            _atDestination = true;
            onReachDestination.Invoke();
        }
    }
}
