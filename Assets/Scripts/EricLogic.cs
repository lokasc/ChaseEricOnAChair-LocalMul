using System;
using System.ComponentModel.Design;
//using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;


public class EricLogic : MonoBehaviour
{
    [Range(0f, 0.49f)]
    public float maxDistanceToStop = 0.48f; // Eric will stop if the player is too far, so we dont lap again!
    public SplineContainer mapSpline; // This is the same one in the kart manager.

    public MoveBasedOnSpline movement;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        KartManager.Instance.boss = this.gameObject;
    }

    void Update()
    {
        // Get closest player to eric 
        float closestT = 10000;
        foreach (KartManager.ChairData chairData in KartManager.Instance.playerRanking)
        {
            if (Mathf.Sign(chairData.t) == 1f)
            {
                continue; // Ignore players in the front half of eric cuz thats our technical limitation or my capability to think .
            }
            if (Mathf.Abs(chairData.t) < closestT)
            {
                closestT = Mathf.Abs(chairData.t);
            }
        }
        print(closestT);

        // If eric is too far! we stop!
        if (closestT >= maxDistanceToStop)
        {
            movement.currentSpeed = 0;
        }
        else
        {
            movement.currentSpeed = movement.speed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.tag == "Player")
        {
        }
    }
}
