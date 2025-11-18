//using UnityEngine;
//using FishNet;
//using FishNet.Managing.Scened;

//public class TesteSceneManager : MonoBehaviour
//{
//    // Start is called once before the first execution of Update after the MonoBehaviour is created
//    private void Awake()
//    {
//        DontDestroyOnLoad(this);
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        //if (!InstanceFinder.IsServer)
//        //    return;

//        if (Input.GetKeyDown(KeyCode.KeypadPlus))
//        {
//            Debug.Log("Premiste a tecla + do teclado principal!");
//            LoadScene("Map1_Part1");
//            UnloadScene("Map2_cloud");
//        }

//        if (Input.GetKeyDown(KeyCode.KeypadMinus))
//        {
//            Debug.Log("Premiste a tecla - do teclado principal!");
//            LoadScene("Map2_cloud");
//            UnloadScene("Map1_Part1");
//        }
//    }

//    void LoadScene(string sceneName)
//    {
//        //if (!InstanceFinder.IsServer)
//        //    return;

//        Debug.Log($"Função LoadScene: {sceneName}");

//        SceneLoadData sld = new SceneLoadData(sceneName);
//        InstanceFinder.SceneManager.LoadGlobalScenes(sld);

//    }

//    void UnloadScene(string sceneName)
//    {
//        //if (!InstanceFinder.IsServer)
//        //    return;

//        Debug.Log($"Função UnloadScene: {sceneName}");

//        SceneUnloadData sld = new SceneUnloadData(sceneName);
//        InstanceFinder.SceneManager.UnloadGlobalScenes(sld);

//    }
//}


using FishNet;
using FishNet.Managing.Scened;
using UnityEngine;

public class TesteSceneManager : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            LoadMap("Map1_Part1", "Map2_cloud");
        }

        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            LoadMap("Map2_cloud", "Map1_Part1");
        }
    }

    private void LoadMap(string sceneToLoad, string sceneToUnload)
    {
        // Descarrega a cena antiga
        if (!string.IsNullOrEmpty(sceneToUnload))
            InstanceFinder.SceneManager.UnloadGlobalScenes(new SceneUnloadData(sceneToUnload));

        // Carrega a nova cena
        SceneLoadData sld = new SceneLoadData(sceneToLoad);
        InstanceFinder.SceneManager.LoadGlobalScenes(sld).Completed += (op) =>
        {
            Debug.Log($"Cena {sceneToLoad} carregada, agora spawna o jogador");

            // Chama spawn manual do jogador
            if (InstanceFinder.IsServer)
            {
                foreach (var conn in InstanceFinder.ServerManager.Clients)
                {
                    InstanceFinder.ServerManager.Spawn(conn.FirstObject);
                }
            }
        };
    }
}
