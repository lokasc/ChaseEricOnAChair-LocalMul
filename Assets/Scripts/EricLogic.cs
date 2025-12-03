using System;
using UnityEditor.UI;
using UnityEngine;

public class EricLogic : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.boss = this.gameObject;
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.tag == "Player")
        {
            GameManager.Instance.AddLap(other.transform.parent.GetComponent<Chair>());
            
            
        }
    }
}
