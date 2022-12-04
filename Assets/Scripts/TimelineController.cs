using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineController : MonoBehaviour
{
    [SerializeField] private PlayableDirector timeline;
    [SerializeField] private float[] timeSteps;
    private bool _reversed;
    private Coroutine timelineRoutine;
    private float currTimeStep = .5f;
    private int timeDir;


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


    public void PlayReversed()
    {
        if (_reversed) return;
        _reversed = true;
        timeDir = -1;
        if (timelineRoutine != null)
            StopCoroutine(timelineRoutine);
        timelineRoutine = StartCoroutine(UpdateTimeline());
    }

    public void PlayForward()
    {
        if (!_reversed) return;
        _reversed = false;
        timeDir = 1;
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
            timeline.DeferredEvaluate();
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
}
