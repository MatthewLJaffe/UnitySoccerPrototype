using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;


public class TransitionController : MonoBehaviour
{
    public UnityEvent onCountDownStart;
    public UnityEvent onGameStart;
    public static Action gameStart = delegate { };
    private bool _gameBegin;
    [SerializeField] private TextMeshProUGUI text;
    

    private void Update()
    {
        //check for pressing space key
        if (!_gameBegin && Input.GetButtonDown("Jump"))
        {
            onCountDownStart.Invoke();
            _gameBegin = true;
            StartCoroutine(CountDown());
        }
    }

    private IEnumerator CountDown()
    {
        text.text = "3";
        yield return new WaitForSeconds(1);
        text.text = "2";
        yield return new WaitForSeconds(1);
        text.text = "1";
        yield return new WaitForSeconds(1);
        onGameStart.Invoke();
        gameStart.Invoke();
    }
}
