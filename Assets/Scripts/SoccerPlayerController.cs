using UnityEngine;
public class SoccerPlayerController : MonoBehaviour
{
    public Animator anim;
    [SerializeField] private MovementAction[] movementActions;
    [SerializeField] private float topSpeed;
    [SerializeField] private float slowThreshold;
    public Vector3 velocity = Vector3.zero;
    int _currAnimActionIdx;
    private float _currStillTime;
    private bool gameGoing = true;

    [System.Serializable]
    private struct MovementAction
    {
        public Transform targetPos;
        public float acc;
        public float maxSpeed;
        public float distanceThreshold;
        public float maxStillTime;
        public EasingStratagey stratagey;
    }

    [System.Serializable]
    public enum EasingStratagey
    {
        Slow,
        Curve
    }

    private void Awake()
    {
        SoccerBallController.gameComplete += StopMovement;
    }

    private void OnDestroy()
    {
        SoccerBallController.gameComplete -= StopMovement;
    }

    private void StopMovement()
    {
        gameGoing = false;
        anim.SetFloat("Speed", 0);
    }

    private void FixedUpdate()
    {
        if (_currAnimActionIdx < movementActions.Length && gameGoing)
            PerformMovementAction();
    }

    private void PerformMovementAction()
    {
        var movementAction = movementActions[_currAnimActionIdx];
        if (movementAction.targetPos != null)
        {
            var dir = (movementAction.targetPos.position - transform.position).normalized;
            if (movementAction.stratagey == EasingStratagey.Curve) {
                velocity += movementAction.acc * Time.fixedDeltaTime * dir;
            }
            else if (movementAction.stratagey == EasingStratagey.Slow)
            {
                //velocity slow or in right direction speed up
                if (Vector3.Dot(velocity.normalized, dir) > .8 || velocity.magnitude < slowThreshold)
                {
                    velocity += movementAction.acc * Time.fixedDeltaTime * dir;
                }
                //velocity vector is big and in wrong direction slow down
                else
                {
                    velocity -= velocity.normalized * Time.fixedDeltaTime * movementAction.acc;
                }
            }
            if (velocity.magnitude > movementAction.maxSpeed)
                velocity = velocity.normalized * movementAction.maxSpeed;
            anim.SetFloat("Speed", velocity.magnitude / topSpeed);
            transform.forward = velocity;
            transform.position += velocity * Time.fixedDeltaTime;
            if (Vector3.Distance(transform.position, movementAction.targetPos.position) < movementAction.distanceThreshold)
            {
                _currAnimActionIdx++;
            }
        }
        //slow down for certain time
        else
        {
            if (velocity.magnitude > 0)
                velocity -= velocity.normalized * Time.fixedDeltaTime* movementAction.acc;
            if (velocity.magnitude <= Time.fixedDeltaTime * movementAction.acc)
            {
                velocity = Vector3.zero;
            }
            transform.position += velocity * Time.fixedDeltaTime;
            anim.SetFloat("Speed", velocity.magnitude / topSpeed);
            _currStillTime += Time.fixedDeltaTime;
            if (_currStillTime >= movementAction.maxStillTime)
            {
                _currStillTime = 0;
                _currAnimActionIdx++;
            }
        }
    }
}
