using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportationManager : MonoBehaviour
{
    [SerializeField] private InputActionAsset actionAsset;
    [SerializeField] private XRRayInteractor rayInteractor;
    [SerializeField] private TeleportationProvider provider;
    [SerializeField] private Transform startPoint;
    private InputAction _thumbstick;
    private InputAction _activate;
    private bool _isActive;

    private void Start()
    {
        _activate = actionAsset.FindActionMap("XRI LeftHand Interaction").FindAction("Activate");
        _activate.Enable();
        _activate.performed += OnTeleportActivate;


        //var cancel = actionAsset.FindActionMap("XRI LeftHand Locomotion").FindAction("Teleport Mode Cancel");
        //cancel.Enable();

        //_thumbstick = actionAsset.FindActionMap("XRI LeftHand Locomotion").FindAction("Move");
        //_thumbstick.Enable();
        //cancel.performed += OnTeleportCancel;

    }

    private void OnDestroy()
    {
        _activate.performed -= OnTeleportActivate;
    }

    private void Update()
    {
        /*
        if (!_isActive || _thumbstick.triggered)
            return;
        if(!rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            rayInteractor.enabled = false;
            _isActive = false;
            return;
        }
        TeleportRequest request = new TeleportRequest()
        {
            destinationPosition = hit.point
        };

        provider.QueueTeleportRequest(request);
        rayInteractor.enabled = false;
        _isActive = false;
        */
    }

    private void OnTeleportActivate(InputAction.CallbackContext callbackContext)
    {
        if(!rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit)) {
            return;
        }
        TeleportRequest request = new TeleportRequest()
        {
            destinationPosition = hit.point
        };
        provider.QueueTeleportRequest(request);
    }

    private void OnTeleportCancel(InputAction.CallbackContext callbackContext)
    {
        rayInteractor.enabled = true;
        _isActive = false;
        Debug.Log("Teleport cancel");
    }

    public void TeleportToStartPos()
    {
        transform.position = startPoint.position;
        transform.rotation = startPoint.rotation;
    }
}
