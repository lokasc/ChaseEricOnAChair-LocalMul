using UnityEngine;

public class CMHideSelf : MonoBehaviour
{
    SpriteRenderer sr;
    Light[] spotlights;
    CharacterManager manager;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        spotlights = GetComponentsInChildren<Light>();
        manager = FindObjectOfType<CharacterManager>();
        UpdateVisibility();
    }

    void Update()
    {
        if (manager != null)
        {
            UpdateVisibility();
        }
    }

    void UpdateVisibility()
    {
        bool visible = manager.currentIndex != -1;

        if (sr != null)
        {
            sr.enabled = visible;
        }

        if (spotlights != null && spotlights.Length > 0)
        {
            foreach (var light in spotlights)
            {
                if (light != null && light.type == LightType.Spot)
                    light.enabled = visible;
            }
        }
    }
}
