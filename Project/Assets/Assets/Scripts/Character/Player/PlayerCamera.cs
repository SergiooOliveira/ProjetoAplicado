using Photon.Pun;
using UnityEngine;

public class PlayerCamera : MonoBehaviourPunCallbacks
{
    [SerializeField] private Camera cameraPrefab; // Camera prefab to instantiate
    [SerializeField] private Transform cameraHolder; // Holder for the camera's position and rotation

    // This method will run once this object is spawned
    private void Start()
    {
        // We need to only instantiate the camera for the local player (owner)
        if (photonView.IsMine)
        {
            // Instantiate the camera for the local player and set it as a child of the camera holder
            Instantiate(cameraPrefab, cameraHolder.position, cameraHolder.rotation, cameraHolder);
        }
    }
}