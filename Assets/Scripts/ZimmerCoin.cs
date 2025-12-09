using UnityEngine;
using UnityEngine.Events;

public class ZimmerCoin : MonoBehaviour
{
    public float rotationSpeed = 150f;
    public UnityEvent OnCoinCollected;
    // public float respawnTime;
    public Collider sphereCollider;
    
    void Update()
    {
        // Always rotate:
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f, Space.Self);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            // other is the physics sphere
            Chair zimmerChair= other.transform.parent.GetComponent<Chair>();
            zimmerChair.AddCoin(1);
            OnCoinCollected.Invoke();
            sphereCollider.enabled = false;
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    public void OnTimerFinish()
    {
        sphereCollider.enabled = true;
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);
    }
}
