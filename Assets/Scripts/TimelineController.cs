using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class TimelineController : MonoBehaviour
{
    [SerializeField] private PlayableDirector timeline;
    [SerializeField] private TimedEvent[] timedEvents;
    private Coroutine timelineRoutine;
    private float currTimeStep = 1f;
    private int timeDir;
    private bool _inReview;
    private bool _paused;

    [System.Serializable]
    public class TimedEvent
    {
        public float time;
        public UnityEvent uEvent;
        public bool playForwards;
        public bool playBackwards;
        public bool playInReview;
        [HideInInspector] public bool _playedForward;
        [HideInInspector] public bool _playedBackwards;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlayForward();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlayReversed();
        }
    }

    public void StopTimeline()
    {
        if (timelineRoutine != null)
            StopCoroutine(timelineRoutine);
        _paused = true;
    }

    public void PlayTimeline()
    {
        timelineRoutine = StartCoroutine(UpdateTimeline());
        _paused = false;
    }


    public void PlayReversed()
    {
        if (timeDir == -1) return;
        timeDir = -1;
        if (!_paused)
        {
            if (timelineRoutine != null)
                StopCoroutine(timelineRoutine);
            timelineRoutine = StartCoroutine(UpdateTimeline());
        }
    }

    public void PlayForward()
    {
        if (timeDir == 1) return;
        timeDir = 1;
        if (!_paused)
        {
            if (timelineRoutine != null)
                StopCoroutine(timelineRoutine);
            timelineRoutine = StartCoroutine(UpdateTimeline());
        }
    }

    public void PlayFromBeginning()
    {
        timeDir = 1;
        timeline.time = 0;
        if (timelineRoutine != null)
            StopCoroutine(timelineRoutine);
        timelineRoutine = StartCoroutine(UpdateTimeline());
    }

    private IEnumerator UpdateTimeline()
    {
        timeline.timeUpdateMode = DirectorUpdateMode.Manual;
        while (timeDir == -1 && timeline.time > 0 || timeDir == 1 && timeline.time < timeline.duration)
        {
            timeline.time += Time.deltaTime * currTimeStep * timeDir;
            //update timeline
            timeline.DeferredEvaluate();
            //events
            if (timeDir == 1)
            {
                foreach(var te in timedEvents)
                {
                    if (te.playForwards && !te._playedForward && timeline.time >= te.time && (!_inReview || te.playInReview))
                    {
                        te.uEvent.Invoke();
                        te._playedForward = true;
                        te._playedBackwards = false;
                    }
                }
            }
            else if (timeDir == -1)
            {
                foreach (var te in timedEvents)
                {
                    if (te.playBackwards && !te._playedBackwards && timeline.time <= te.time && (!_inReview || te.playInReview))
                    {
                        te.uEvent.Invoke();
                        te._playedBackwards = true;
                        te._playedForward = false;
                    }
                }
            }
            yield return null;

        }
        timeline.time = Mathf.Clamp((float)timeline.time, 0, (float)timeline.duration);
        timeline.DeferredEvaluate();
        timeline.Pause();
    }

    private bool ApproxEquals(double num, double other)
    {
        return Mathf.Approximately((float)num, (float)other);
    }

    public void SetReview(bool b)
    {
        _inReview = b;
    }
}
