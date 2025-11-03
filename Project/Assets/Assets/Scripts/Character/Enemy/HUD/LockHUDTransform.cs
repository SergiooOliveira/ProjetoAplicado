using UnityEngine;

public class LockHUDTransform : MonoBehaviour
{
    private Vector3 initialWorldScale;
    private Quaternion initialWorldRotation;
    private Transform parent;

    void Start()
    {
        initialWorldScale = transform.lossyScale;
        initialWorldRotation = transform.rotation;
        parent = transform.parent;
    }

    void LateUpdate()
    {
        if (parent == null) return;

        // Lock rotation
        transform.rotation = initialWorldRotation;

        // Calculate local scale to keep world scale fixed
        Vector3 parentLossy = parent.lossyScale;
        Vector3 newLocalScale = transform.localScale;

        newLocalScale.x = (Mathf.Abs(parentLossy.x) > 0.0001f) ? initialWorldScale.x / parentLossy.x : newLocalScale.x;
        newLocalScale.y = (Mathf.Abs(parentLossy.y) > 0.0001f) ? initialWorldScale.y / parentLossy.y : newLocalScale.y;
        newLocalScale.z = (Mathf.Abs(parentLossy.z) > 0.0001f) ? initialWorldScale.z / parentLossy.z : newLocalScale.z;

        transform.localScale = newLocalScale;
    }
}