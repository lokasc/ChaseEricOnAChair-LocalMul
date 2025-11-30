using UnityEngine;
using TMPro;

public class ChairUI : MonoBehaviour
{
    public TextMeshProUGUI positionText;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        positionText.text = GameManager.Instance.GetCurrentPosition(transform.parent.GetComponent<Chair>()).ToString();

    }
}
