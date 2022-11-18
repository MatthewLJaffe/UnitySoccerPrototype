using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private Camera _mainCam;
    [SerializeField] private float maxCameraX = 1420;
    [SerializeField] private float minCameraX = 50;
    private Vector3 _initialPos;

    private void Awake()
    {
        _mainCam = Camera.main;
        _initialPos = transform.position;
    }


    void Update()
    {
        var screenSpacePos = _mainCam.WorldToScreenPoint(_initialPos);
        screenSpacePos.x = Mathf.Clamp(screenSpacePos.x, minCameraX, maxCameraX);
        Debug.Log(screenSpacePos);
        transform.position = _mainCam.ScreenToWorldPoint(screenSpacePos);
    }
}
