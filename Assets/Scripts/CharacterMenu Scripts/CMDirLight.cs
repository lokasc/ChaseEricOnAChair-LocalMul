using UnityEngine;

public class CharacterMenuLight  : MonoBehaviour
{
    public CharacterManager manager;
    public float rotateSpeed = 20f; 
    public float lerpSpeed = 5f; 
    public Light lightSource; 
public float fadeSpeed = 2f; 
public float onIntensity = 1f; 

    void Update()
{
    if (manager == null || lightSource == null) return;

    bool inOverview = manager.currentIndex == -1;

    if (inOverview)
    {
        Vector3 e = transform.eulerAngles;
        e.x += rotateSpeed * Time.deltaTime;
        transform.eulerAngles = e;
    }
    else
    {
        Quaternion targetRot = Quaternion.Euler(270f, transform.eulerAngles.y, transform.eulerAngles.z);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, lerpSpeed * Time.deltaTime);
    }

    float targetIntensity = inOverview ? onIntensity : 0f;

    lightSource.intensity = Mathf.Lerp(
        lightSource.intensity,
        targetIntensity,
        fadeSpeed * Time.deltaTime
    );
}

    void Awake()
{
    if (lightSource == null)
        lightSource = GetComponent<Light>();
}
}