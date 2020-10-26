using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

// Adds ARRaycastManager class to any object with this script
[RequireComponent(typeof(ARRaycastManager))]
public class SpawnOnPlane : MonoBehaviour
{
    private ARRaycastManager raycastManager;
    private GameObject enemyObject;
    public Transform arPosition;

    [SerializeField]
    private GameObject placeablePrefab;

    static List<ARRaycastHit> raycastHits = new List<ARRaycastHit>();

    public Vector2 touchPosition;
    public Vector2 cameraViewPosition;


    private void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
    }

    // Gets where the user presses on the screen
    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = default;
        return false;
    }

    private void Update()
    {
        if(TryGetTouchPosition(out Vector2 touchPosition))
        {
            if (raycastManager.Raycast(touchPosition, raycastHits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
            {
                var hitPose = raycastHits[0].pose;
                enemyObject = Instantiate(placeablePrefab, hitPose.position, hitPose.rotation);
            }
        }
    }


}
