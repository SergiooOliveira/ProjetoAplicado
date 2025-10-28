//using UnityEngine;

//public class ParallaxController : MonoBehaviour
//{
//    [Header("Referência da câmera (pode ser definida dinamicamente)")]
//    public Transform cam;

//    Vector3 camStartPos;
//    float distance;

//    GameObject[] backgrounds;
//    Material[] mat;
//    float[] backSpeed;

//    float farthestBack;

//    [Range(0.01f, 0.05f)]
//    public float parallaxSpeed;

//    void Start()
//    {
//        cam = Camera.main.transform;
//        camStartPos = cam.position;

//        int backCount = transform.childCount;
//        mat = new Material[backCount];
//        backSpeed = new float[backCount];
//        backgrounds = new GameObject[backCount];

//        for (int i = 0; i < backCount; i++)
//        {
//            backgrounds[i] = transform.GetChild(i).gameObject;
//            mat[i] = backgrounds[i].GetComponent<Renderer>().material;
//        }

//        BackSpeedCalculate(backCount);
//    }

//    void BackSpeedCalculate(int backCount)
//    {
//        for (int i = 0; i < backCount; i++)
//        {
//            if ((backgrounds[i].transform.position.z - cam.position.z) > farthestBack)
//            {
//                farthestBack = backgrounds[i].transform.position.z - cam.position.z;
//            }
//        }

//        for (int i = 0; i < backCount; i++)
//        {
//            backSpeed[i] = 1 - (backgrounds[i].transform.position.z - cam.position.z) / farthestBack;
//        }

//    }

//    private void LateUpdate()
//    {
//        distance = cam.position.x - camStartPos.x;
//        transform.position = new Vector3(cam.position.x, transform.position.y, 0);

//        for (int i = 0; i < backgrounds.Length; i++)
//        {
//            float speed = backSpeed[i] * parallaxSpeed;
//            mat[i].SetTextureOffset("_MainTex", new Vector2(distance, 0) * speed);
//        }

//    }

//    public void InitializeCamera(Transform playerCam)
//    {
//        cam = playerCam;
//        Start();
//    }

//}

using UnityEngine;
using System.Collections;

public class ParallaxController : MonoBehaviour
{
    [Header("Câmera usada para o Parallax (pode ser atribuída depois)")]
    public Transform cam;

    private Vector3 camStartPos;
    private GameObject[] backgrounds;
    private Material[] mat;
    private float[] backSpeed;
    private float farthestBack;

    [Range(0.001f, 0.1f)]
    public float parallaxSpeed = 0.02f;

    private bool initialized = false;

    private void Start()
    {
        StartCoroutine(WaitForCamera());
    }

    private IEnumerator WaitForCamera()
    {
        while (cam == null)
        {
            if (Camera.main != null)
                cam = Camera.main.transform;
            else
            {
                GameObject possibleCam = GameObject.Find("MainCamera");
                if (possibleCam != null)
                    cam = possibleCam.transform;
            }

            yield return null;
        }

        InitializeParallax();
    }

    private void InitializeParallax()
    {
        camStartPos = cam.position;

        int backCount = transform.childCount;
        mat = new Material[backCount];
        backSpeed = new float[backCount];
        backgrounds = new GameObject[backCount];

        for (int i = 0; i < backCount; i++)
        {
            backgrounds[i] = transform.GetChild(i).gameObject;

            Renderer rend = backgrounds[i].GetComponent<Renderer>();
            mat[i] = new Material(rend.material);
            rend.material = mat[i];
        }

        CalculateBackSpeeds(backCount);
        initialized = true;
    }

    private void CalculateBackSpeeds(int backCount)
    {
        farthestBack = float.MinValue;

        for (int i = 0; i < backCount; i++)
        {
            float depth = backgrounds[i].transform.position.z - cam.position.z;
            if (depth > farthestBack)
                farthestBack = depth;
        }

        for (int i = 0; i < backCount; i++)
        {
            float depth = backgrounds[i].transform.position.z - cam.position.z;
            backSpeed[i] = 1 - (depth / farthestBack);
        }
    }

    private void LateUpdate()
    {
        if (!initialized || cam == null)
            return;

        float distance = cam.position.x - camStartPos.x;
        transform.position = new Vector3(cam.position.x, transform.position.y, 0);

        for (int i = 0; i < backgrounds.Length; i++)
        {
            float speed = backSpeed[i] * parallaxSpeed;
            mat[i].SetTextureOffset("_MainTex", new Vector2(distance, 0) * speed);
        }
    }

    public void InitializeCamera(Transform playerCam)
    {
        cam = playerCam;
        InitializeParallax();
    }
}
