using Photon.Pun;
using UnityEngine;
using System.Collections.Generic;

public class CustomPrefabPool : MonoBehaviour, IPunPrefabPool
{
    // A dictionary to store different prefab types.
    public GameObject playerPrefab;
    public GameObject pBoss_mapa1Prefab;
    public GameObject pM_W_1Prefab;
    public GameObject PR_F_1Prefab;

    private Dictionary<string, GameObject> prefabDictionary;


    public GameObject Instantiate(string prefabId, Vector3 position, Quaternion rotation)
    {
        // Check if the prefabId exists in the dictionary.
        // if (prefabId == "Player")
        // {
        //     // Debug.Log("⏹️ player???" + prefabId );
            

        //     // Ensure the prefab is assigned in the inspector.
        //     if (playerPrefab == null)
        //     {
        //         Debug.LogError($"Prefab for {prefabId} is not assigned!");
        //         return null;
        //     }

        //     // **Call PhotonNetwork.Instantiate directly** instead of recursively calling the same method
        //     // return PhotonNetwork.Instantiate(playerPrefab.name, position, rotation);
        //     return Instantiate(playerPrefab, position,rotation);
        // }
        
        // Debug.LogError($"Prefab not found: {prefabId}");
        // return null;

        switch (prefabId)
        {
            case "Player": return Instantiate(playerPrefab, position, rotation);                
            case "pM_W_1": return PhotonNetwork.Instantiate(pM_W_1Prefab.name, position, rotation);                

            default: Debug.Log("deu merda");
            break;
        }

        Debug.LogError($"Prefab not found: {prefabId}");
        return null;
    }

    public void Destroy(GameObject gameObject)
    {
        // Clean up the prefab when it's no longer needed.
        PhotonNetwork.Destroy(gameObject);
    }
}
