using System;
using UnityEngine;

[Serializable]
public struct CameraCheckpoint
{
    public Camera camera;
    public Checkpoint checkpoint;
}

public class AutomaticCameraSystem : MonoBehaviour
{
    public CheckpointManager kartToFollow;
    public CameraCheckpoint[] cameraCheckpoints;

    private bool manualOverride = false;
    private int currentManualIndex = -1;

   
    private void Start()
    {
        setCameraActive(0);
        kartToFollow.reachedCheckpoint += OnReachedCheckpoint;
    }

    private void OnReachedCheckpoint(Checkpoint checkpoint)
    {
        if (manualOverride) return;

        foreach (CameraCheckpoint cameraCheckpoint in cameraCheckpoints)
        {
            if (cameraCheckpoint.checkpoint == checkpoint)
            {
                DeactivateAllCameras();
                cameraCheckpoint.camera.gameObject.SetActive(true);
            }
        }
    }

    public void setCameraActive(int index)
    {
        DeactivateAllCameras();
        cameraCheckpoints[index].camera.gameObject.SetActive(true);
    }

    private void DeactivateAllCameras()
    {
        foreach (CameraCheckpoint cameraCheckpoint in cameraCheckpoints)
        {
            cameraCheckpoint.camera.gameObject.SetActive(false);
        }
    }


    public void ToggleManualCamera(int index)
    {
        if (!manualOverride)
        {
            manualOverride = true;
            currentManualIndex = index;
            DeactivateAllCameras();
            cameraCheckpoints[index].camera.gameObject.SetActive(true);
        }
        else
        {
            manualOverride = false;
            currentManualIndex = -1;

            DeactivateAllCameras();
        }
    }


}
