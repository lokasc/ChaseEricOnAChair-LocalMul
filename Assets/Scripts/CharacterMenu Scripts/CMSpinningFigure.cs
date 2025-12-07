using UnityEngine;
using System.Collections;

public class CMSpinningFigure : MonoBehaviour
{
    CharacterManager manager;
    Vector3 startPosition;
    public float rotationSpeed = 20f;
    public float bobAmplitude = 0.1f;
    public float bobFrequency = 1f;

    float currentRotationSpeed;
    Coroutine speedRoutine;

    void Awake()
    {
        manager = FindObjectOfType<CharacterManager>();
        startPosition = transform.position;
        currentRotationSpeed = rotationSpeed;
    }

    void Update()
    {
        if (manager == null || manager.currentIndex == -1) return;

        transform.Rotate(0f, currentRotationSpeed * Time.deltaTime, 0f, Space.Self);

        float bob = Mathf.Sin(Time.time * bobFrequency) * bobAmplitude;
        Vector3 pos = startPosition;
        pos.y += bob;
        transform.position = pos;

        foreach (var input in manager.characterSelectors)
        {
            if (input.SelectPressed() && speedRoutine == null)
            {
                speedRoutine = StartCoroutine(SpinBoost());
            }
        }
    }
//spin when select!
    IEnumerator SpinBoost()
    {
        float duration = 0.5f;
        float elapsed = 0f;
        float startSpeed = rotationSpeed;
        float targetSpeed = rotationSpeed * 100f;


        while (elapsed < duration)
        {
            currentRotationSpeed = Mathf.Lerp(startSpeed, targetSpeed, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        currentRotationSpeed = targetSpeed;


        elapsed = 0f;
        float decelDuration = duration * 2f;
        while (elapsed < decelDuration)
        {
            currentRotationSpeed = Mathf.Lerp(targetSpeed, rotationSpeed, elapsed / decelDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        currentRotationSpeed = rotationSpeed;

        speedRoutine = null;
    }
}
