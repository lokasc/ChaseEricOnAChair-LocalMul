using System;
using Unity.VisualScripting;
using UnityEngine;

public class Chair : MonoBehaviour
{
    [Header("Player Profile")] 
    public float legPower = 30f;
    public float legCoolDown = 0.5f;
    public float maxSteerAngle = 45f;
    
    
    [Header("References")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Controls playerInput;
    
    private float currentCoolDown;
    private bool canMash;
    

    private void Start()
    {
        rb.gameObject.transform.name += " " + transform.name;
        rb.gameObject.transform.parent = null;
        currentCoolDown = legCoolDown;
        canMash = true;
    }

    private void Update()
    {
        print(playerInput.GetSteerValue());
        // Cooldown Timer
        if (!canMash)
        {
            CooldownLogic();
        }
        
        if (playerInput.AccelPressed() && canMash)
        {
            canMash = false;
            
            transform.Rotate(new Vector3(0f, playerInput.GetSteerValue() * maxSteerAngle, 0f));
            rb.AddForce(legPower * transform.forward, ForceMode.Impulse);
        }
        
        // Set the transform
        transform.position = rb.position;
        
        // Steering
        //transform.Rotate(new Vector3(0, playerInput.GetSteerValue() * steerPower, 0) * Time.deltaTime);
    }
    


    private void CooldownLogic()
    {
        currentCoolDown -= Time.deltaTime;
        if (currentCoolDown <= 0)
        {
            currentCoolDown = legCoolDown;
            canMash = true;
        }
    }
}
