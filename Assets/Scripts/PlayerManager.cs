using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    // This contains the stats and the prefab for character models.
    [Header("Stats")] 
    public float legLength;
    public float cooldown;
    public float handling;

    public GameObject characterModel;
    public string characterName;
    
    public PlayerInput playerInput;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }
}
