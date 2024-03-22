using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ItemFaceTracking : MonoBehaviour
{
    public static ItemFaceTracking Instance;
    public ARFaceManager faceManager;
    public Camera arCamera;
    public GameObject cube;
    public TextMeshProUGUI text;

    Dictionary<TrackableId, ARFace> trackedFaces = new Dictionary<TrackableId, ARFace>();

    [HideInInspector] public bool trackingIsDisable;

    private void Awake()
    {
        Instance = this;
        trackingIsDisable = true;
    }

    void Start()
    {
        faceManager.facesChanged += OnFacesChanged;
    }

    void OnFacesChanged(ARFacesChangedEventArgs eventArgs)
    {
        foreach (var addedFace in eventArgs.added)
        {
            trackedFaces.Add(addedFace.trackableId, addedFace);
        }

        foreach (var updatedFace in eventArgs.updated)
        {
            trackedFaces[updatedFace.trackableId] = updatedFace;
        }

        foreach (var removedFace in eventArgs.removed)
        {
            trackedFaces.Remove(removedFace.trackableId);
        }
    }

    void Update()
    {
        if (arCamera == null)
            return;

        if(trackingIsDisable) return;
        if (trackedFaces.Count > 0)
        {
            ARFace face = trackedFaces.Values.First();
            Vector3 faceWorldPosition = face.transform.position;
            Vector3 screenPosition = arCamera.WorldToScreenPoint(faceWorldPosition);
            
            text.text = screenPosition.ToString();

            if(screenPosition.x < 100 || screenPosition.x > 1000) return;
            cube.transform.position = new Vector3(faceWorldPosition.x * 10,-0.5f,-9.5f);
        }
    }
}
