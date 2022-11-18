using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookControls : MonoBehaviour
{
    private Vector2 _mouseDelta;
    [SerializeField] private Vector2 sensitivity;
    [SerializeField] private Vector2 cameraYRotBounds;
    [SerializeField] private Vector2 cameraXRotBounds;

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        _mouseDelta = Vector2.zero;
    }

    void Update()
    {
        _mouseDelta.x = Input.GetAxis("Mouse X");
        _mouseDelta.y = Input.GetAxis("Mouse Y");
        transform.Rotate(Vector2.up * (sensitivity.y * _mouseDelta.x));
        transform.Rotate(Vector2.right * (sensitivity.x * -_mouseDelta.y));
        
        var boundX = transform.eulerAngles.x;
        var boundY = transform.eulerAngles.y;
        if (boundX > 270)
            boundX -= 360;
        if (boundY > 270)
            boundY -= 360;
        
        if (boundY > cameraYRotBounds.y ||
            boundY < cameraYRotBounds.x)
        {
            boundY = Mathf.Clamp(boundY, cameraYRotBounds.x, cameraYRotBounds.y);
        }
        if (boundX > cameraXRotBounds.y || boundX < cameraXRotBounds.x)
        {
            boundX = Mathf.Clamp(boundX, cameraXRotBounds.x, cameraXRotBounds.y);
        }
        transform.rotation = Quaternion.Euler(boundX, boundY, 0);
        
    }        
}
