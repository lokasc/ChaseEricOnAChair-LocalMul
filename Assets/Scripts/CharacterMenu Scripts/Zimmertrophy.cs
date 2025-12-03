using UnityEngine;

public class Zimmertrophy : MonoBehaviour
{
    public CharacterManager manager;
    public Camera mainCamera;

    public float rotationSpeed = 10f;


    void Awake()
    {

    }

    void Update()
    {
        if (manager == null || mainCamera == null) return;

        

        if (manager.currentIndex == -1)
        {
            transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f, Space.Self);
            Vector3 p = transform.position;
        p.z = mainCamera.transform.position.z;
        transform.position = p;
        }

    float SmoothStep01(float x)
    {
        x = Mathf.Clamp01(x);
        return x * x * (3f - 2f * x);
    }
    }
}
