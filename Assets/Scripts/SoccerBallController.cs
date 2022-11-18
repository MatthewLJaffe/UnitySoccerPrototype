using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SoccerBallController : MonoBehaviour
{
    public UnityEvent<float> onGameComplete;
    [SerializeField] private AudioSource kickSound;
    [SerializeField] private ScoreTracker scoreTracker;
    [SerializeField] private ChoiceTarget[] choiceTargets;
    [SerializeField] private AnimationCurve translateCurve;
    [SerializeField] private AnimationCurve heightCurve;
    [SerializeField] private AnimationCurve rotateCurve;
    private Coroutine _moveRoutine;

    
    [System.Serializable]
    private struct ChoiceTarget
    {
        public Transform target;
        public KeyCode choice;
        public float travelTime;
        public float score;
        public AudioClip feedbackNoise;
    }
    

    public void CompleteGame(float baseScore)
    {
        onGameComplete.Invoke(baseScore);
    }

    private void Update()
    {
        foreach (var ct in choiceTargets)
        {
            if (Input.GetKeyDown(ct.choice) && _moveRoutine == null)
            {
                _moveRoutine = StartCoroutine(MoveBall(ct));
            }
        }
    }

    private IEnumerator MoveBall(ChoiceTarget ct)
    {
        kickSound.Play();
        scoreTracker.StopTimer();
        Vector3 initialRot = transform.rotation.eulerAngles;
        Vector3 initialPos = transform.position;
        Vector3 targetPos = ct.target.position;
        Quaternion targetRot = ct.target.rotation;
        var height = Vector3.Distance(ct.target.position, initialPos) / 15f;
        for (var t = 0f; t < ct.travelTime; t += Time.deltaTime)
        {
            var newPos =  Vector3.Lerp(initialPos, targetPos, translateCurve.Evaluate(t/ct.travelTime));
            newPos.y += Mathf.Lerp(0, height, heightCurve.Evaluate(t/ct.travelTime));
            transform.position = newPos;
            transform.rotation = 
                Quaternion.Euler(Vector3.Lerp(initialRot, targetRot.eulerAngles, rotateCurve.Evaluate(t/ct.travelTime)));
            Debug.Log(transform.rotation.eulerAngles);
            yield return null;
        }

        kickSound.clip = ct.feedbackNoise;
        kickSound.Play();
        onGameComplete.Invoke(ct.score);
    }
}
