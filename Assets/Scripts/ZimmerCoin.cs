using UnityEngine;
using UnityEngine.Events;

public class ZimmerCoin : MonoBehaviour
{
    public float rotationSpeed = 150f;
    public UnityEvent OnCoinCollected;
    
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
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        
    }
}
