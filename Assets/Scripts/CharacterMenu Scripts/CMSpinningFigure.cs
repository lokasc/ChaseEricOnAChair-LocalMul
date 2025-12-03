using UnityEngine;

public class CMSpinningFigure : MonoBehaviour
{
    CharacterManager manager;
    Vector3 startPosition;
    public float rotationSpeed = 20f;
    public float bobAmplitude = 0.1f;
    public float bobFrequency = 1f;

    void Awake()
    {
        manager = FindObjectOfType<CharacterManager>();
        startPosition = transform.position;
    }

    void Update()
    {
        if (manager == null || manager.currentIndex == -1) return;

        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f, Space.Self);

        float bob = Mathf.Sin(Time.time * bobFrequency) * bobAmplitude;
        Vector3 pos = startPosition;
        pos.y += bob;
        transform.position = pos;
    }
}
