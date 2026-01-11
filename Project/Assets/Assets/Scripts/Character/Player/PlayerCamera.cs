using FishNet.Object;
using UnityEngine;

public class PlayerCamera : NetworkBehaviour
{
    [SerializeField] private Transform cameraHolder;
    private Camera mainCamera;

    public override void OnStartClient()
    {
        if (!IsOwner) return;

        // Look for the global camera in the Persistent scene
        mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("Não foi encontrada a câmera global na PersistentScene!");
            return;
        }

        // Unlink from the original scene
        mainCamera.transform.SetParent(null);

        // Move into CameraHolder
        mainCamera.transform.SetParent(cameraHolder);

        // Reset localPosition/Rotation if you want it to be at the same offset
        mainCamera.transform.localPosition = Vector3.zero;
        mainCamera.transform.localRotation = Quaternion.identity;

        // AudioListener already exists, just ensure it is active
        AudioListener listener = mainCamera.GetComponent<AudioListener>();
        if (listener != null) listener.enabled = true;
    }

    public override void OnStopClient()
    {
        if (!IsOwner) return;

        // Unlink camera if player exits
        if (mainCamera != null)
            mainCamera.transform.SetParent(null);
    }
}
