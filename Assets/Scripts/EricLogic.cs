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

    public bool winnningPlayerCollided;
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
        // print(closestT);

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
    
    // Send Players to ZimmerHell or Let them go to ZimmerhEaven
    private void OnTriggerEnter(Collider other)
    {
        // Check if player is infront
        if (other.gameObject.tag != "Player") { return; }

        // Direction from me to the player
        Vector3 direction = (other.transform.position - transform.position).normalized;

        // Compare direction to my forward direction // i got lazy, its 3am i dont wanna think about maths.
        float dot = Vector3.Dot(transform.forward, direction);
        
        // kill the player if they collide from the front. 
        if (dot > 0)
        {
            //KartManager.Instance.RespawnPlayer(other.transform.parent.GetComponent<Chair>());
        }
        else // player wins if collide at back
        {
            if (!winnningPlayerCollided)
            {
                // when eric is collided by someone, they win right?
                Chair winningPlayer = other.transform.parent.GetComponent<Chair>();
                KartManager.Instance.OnPlayerFinish(winningPlayer);
                winnningPlayerCollided = true;
            }
        }
        

        // // How the fuck would I do this?
        // if (other.gameObject.tag == "Player" && !winnningPlayerCollided)
        // {
        //     // when eric is collided by someone, they win right?
        //     Chair winningPlayer = other.transform.parent.GetComponent<Chair>();
        //     KartManager.Instance.OnPlayerFinish(winningPlayer);
        //     winnningPlayerCollided = true;
        //     // 1. get who won and get the loser.
        //     // 2. KART MANAGER SAYS GAME FINISHED!!!!
        //     // 3. Send who won to gamemanager.
        //     // 5. GOD WHY THE FUCK DO I HAVE SO MANY MANAGERS and I HATE SINGLETONS.
        //     // 4. TRANSITION TO THE Victory Screen
        // }
    }
}
