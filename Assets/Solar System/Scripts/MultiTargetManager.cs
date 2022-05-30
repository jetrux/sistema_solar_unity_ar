using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class MultiTargetManager : MonoBehaviour
{
    [SerializeField]
    private ARTrackedImageManager aRTrackedImageManager;
    [SerializeField]
    private GameObject[] aRModelToPlace;

    private Dictionary<string, GameObject> aRModels = new Dictionary<string, GameObject>();
    private Dictionary<string, bool> modelState = new Dictionary<string, bool>();

    // Start is called before the first frame update
    void Start()
    {
        foreach(var aRModel in aRModelToPlace)
        {
            GameObject newARModel = Instantiate(aRModel, Vector3.zero, Quaternion.identity);
            newARModel.name = aRModel.name;
            aRModels.Add(newARModel.name, newARModel);
            newARModel.SetActive(false);
            modelState.Add(newARModel.name, false);
        }
    }

    private void OnEnabled()
    {
        aRTrackedImageManager.trackedImagesChanged += ImageFound;
    }

    private void ImageFound(ARTrackedImagesChangedEventArgs eventData)
    {
        foreach(var trackedImage in eventData.added)
        {
            ShowARModel(trackedImage);
        }
        foreach(var trackedImage in eventData.updated)
        {
            if(trackedImage.trackingState == TrackingState.Tracking)
            {
                ShowARModel(trackedImage);
            }
            else if(trackedImage.trackingState==TrackingState.Limited)
            {
                HideARModel(trackedImage);
            }
        }
    }

    private void ShowARModel(ARTrackedImage trackedImage)
    {
        bool isModelActivated = modelState[trackedImage.referenceImage.name];
        if(isModelActivated)
        {
            GameObject aRModel=aRModels[trackedImage.referenceImage.name];
            aRModel.transform.position = trackedImage.transform.position;
            aRModel.SetActive(true);
            modelState[trackedImage.referenceImage.name] = trackedImage;
        }
        else
        {
            GameObject aRModel = aRModels[trackedImage.referenceImage.name];
            aRModel.transform.position=trackedImage.transform.position;
        }
    }

    private void HideARModel(ARTrackedImage trackedImage)
    {
        bool isModelActivated = modelState[trackedImage.referenceImage.name];
        if (isModelActivated)
        {
            GameObject aRModel = aRModels[trackedImage.referenceImage.name];
            aRModel.SetActive(false);
            modelState[trackedImage.referenceImage.name] = trackedImage;
        }
    }
}
