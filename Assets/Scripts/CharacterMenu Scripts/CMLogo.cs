using UnityEngine;

public class CMLogo : MonoBehaviour
{
    public CharacterManager manager;
    public Camera mainCamera;

    public float startScale = 22f;
    public float endScale = 0.48f;
    public float scaleSpeed = 1.5f;
    public float pulseAmount = 0.05f;
    public float pulseSpeed = 2f;

    SpriteRenderer sr;
    float t;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.enabled = false;
        t = 0f;
    }

    void Update()
    {
        if (manager == null || mainCamera == null) return;

        Vector3 p = transform.position;
        p.z = mainCamera.transform.position.z;
        transform.position = p;

        if (manager.currentIndex == -1)
        {
            if (!sr.enabled)
            {
                sr.enabled = true;
                t = 0f;
            }

            t += Time.deltaTime * scaleSpeed;
            float baseScale = Mathf.Lerp(startScale, endScale, SmoothStep01(t));

            if (t >= 1f)
                baseScale = endScale + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;

            transform.localScale = new Vector3(baseScale, baseScale, baseScale);
        }
        else
        {
            if (sr.enabled)
                sr.enabled = false;
        }
    }

    float SmoothStep01(float x)
    {
        x = Mathf.Clamp01(x);
        return x * x * (3f - 2f * x);
    }
}
