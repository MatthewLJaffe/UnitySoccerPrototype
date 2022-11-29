using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootTracker : MonoBehaviour
{
    public Vector3 lastPos;
    public Vector3 velocity;

    private void Awake()
    {
        lastPos = transform.position;
    }

    private void FixedUpdate()
    {
        velocity = transform.position - lastPos;
        lastPos = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("SoccerBall"))
            Debug.Log("Velocity: " + velocity * (1 / Time.fixedDeltaTime));
    }
}
