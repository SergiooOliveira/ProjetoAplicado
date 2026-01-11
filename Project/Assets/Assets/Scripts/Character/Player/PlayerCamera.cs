using FishNet.Object;
using UnityEngine;

public class PlayerCamera : NetworkBehaviour
{
    [Header("Camera Setup")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform cameraHolder;

    private Camera spawnedCamera;

    public override void OnStartClient()
    {
        if (!IsOwner)
            return;

        // Disable GlobalCamera when player enters
        var globalCam = FindAnyObjectByType<GlobalCameraBootstrap>();
        if (globalCam != null)
            globalCam.gameObject.SetActive(false);

        // Instantiate the MainCamera
        spawnedCamera = Instantiate(mainCamera);

        // Parent in the player's holder
        spawnedCamera.transform.SetParent(cameraHolder);
        spawnedCamera.transform.localPosition = Vector3.zero;
        spawnedCamera.transform.localRotation = Quaternion.identity;

        // Ensure Active AudioListener
        var listener = spawnedCamera.GetComponent<AudioListener>();
        if (listener != null)
            listener.enabled = true;
    }

    public override void OnStopClient()
    {
        if (!IsOwner)
            return;

        if (spawnedCamera != null)
            Destroy(spawnedCamera.gameObject);
    }
}
