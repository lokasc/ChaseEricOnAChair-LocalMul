using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controls : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    
    public float GetSteerValue() => playerInput.actions["Steer"].ReadValue<float>(); 
    public bool AccelPressed() => playerInput.actions["Accel"].triggered;
}