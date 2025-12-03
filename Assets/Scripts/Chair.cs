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
    [SerializeField] private GameObject chair; // This is where all the visuals are.
    [SerializeField] public ChairUI playerUI;
    
    private float currentCoolDown;
    private bool canMash;
    public bool canDrive = false;
    
    private void Start()
    {
        rb.gameObject.transform.name += " " + transform.name;
        currentCoolDown = legCoolDown;
        canMash = true;
    }

    private void Update()
    {
        if (!canDrive)
        {
            // rb.linearVelocity = Vector3.zero;
            rb.transform.localPosition = Vector3.zero;
            print("hello!");
            return;
        }
        
        // Cooldown Timer
        if (!canMash)
        {
            CooldownLogic();
        }
        
        if (playerInput.AccelPressed() && canMash)
        {
            canMash = false;
            
            chair.transform.Rotate(new Vector3(0f, playerInput.GetSteerValue() * maxSteerAngle, 0f));
            rb.AddForce(legPower * chair.transform.forward, ForceMode.Impulse);
        }
        
        // Set the transform
        chair.transform.position = rb.position;
        
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

    public void OnCountDownComplete()
    {
        canDrive = true;
    }
}
