using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    public Transform player;

    void Update()
    {
        transform.LookAt(player);
        //transform.forward = player.position - transform.position;
    }
}
