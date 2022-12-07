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
    private InputAction _activate;

    private void Start()
    {
        _activate = actionAsset.FindActionMap("XRI LeftHand Interaction").FindAction("Activate");
        _activate.Enable();
        _activate.performed += OnTeleportActivate;
    }

    private void OnDestroy()
    {
        //Think this handles null propagation without subscribing twice
        _activate.performed += OnTeleportActivate;
        _activate.performed -= OnTeleportActivate;
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

    public void TeleportToStartPos()
    {
        transform.parent.position = startPoint.position;
        transform.parent.rotation = startPoint.rotation;
    }
}
