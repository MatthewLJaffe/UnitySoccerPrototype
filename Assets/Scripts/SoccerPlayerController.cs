using UnityEngine;
public class SoccerPlayerController : MonoBehaviour
{
    public Animator anim;
    [SerializeField] private MovementAction[] movementActions;
    [SerializeField] private float topSpeed;
    [SerializeField] private float slowThreshold;
    private Vector3 _velocity = Vector3.zero;
    int _currAnimActionIdx;
    private float _currStillTime;
    private bool gameStarted;

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


    private void FixedUpdate()
    {
        if (_currAnimActionIdx < movementActions.Length)
            PerformMovementAction();
    }

    private void PerformMovementAction()
    {
        var movementAction = movementActions[_currAnimActionIdx];
        if (movementAction.targetPos != null)
        {
            var dir = (movementAction.targetPos.position - transform.position).normalized;
            if (movementAction.stratagey == EasingStratagey.Curve) {
                _velocity += movementAction.acc * Time.fixedDeltaTime * dir;
            }
            else if (movementAction.stratagey == EasingStratagey.Slow)
            {
                //velocity slow or in right direction speed up
                if (Vector3.Dot(_velocity.normalized, dir) > .8 || _velocity.magnitude < slowThreshold)
                {
                    _velocity += movementAction.acc * Time.fixedDeltaTime * dir;
                }
                //velocity vector is big and in wrong direction slow down
                else
                {
                    _velocity -= _velocity.normalized * Time.fixedDeltaTime * movementAction.acc;
                }
            }
            if (_velocity.magnitude > movementAction.maxSpeed)
                _velocity = _velocity.normalized * movementAction.maxSpeed;
            anim.SetFloat("Speed", _velocity.magnitude / topSpeed);
            transform.forward = _velocity;
            transform.position += _velocity * Time.fixedDeltaTime;
            if (Vector3.Distance(transform.position, movementAction.targetPos.position) < movementAction.distanceThreshold)
            {
                _currAnimActionIdx++;
            }
        }
        //slow down for certain time
        else
        {
            if (_velocity.magnitude > 0)
                _velocity -= _velocity.normalized * Time.fixedDeltaTime* movementAction.acc;
            if (_velocity.magnitude <= Time.fixedDeltaTime * movementAction.acc)
            {
                _velocity = Vector3.zero;
            }
            transform.position += _velocity * Time.fixedDeltaTime;
            anim.SetFloat("Speed", _velocity.magnitude / topSpeed);
            _currStillTime += Time.fixedDeltaTime;
            if (_currStillTime >= movementAction.maxStillTime)
            {
                _currStillTime = 0;
                _currAnimActionIdx++;
            }
        }
    }




}
