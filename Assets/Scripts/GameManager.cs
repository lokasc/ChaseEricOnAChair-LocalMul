using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public bool isTest = false;
    public static GameManager instance;
    
    public List<PlayerManager> playerManagers;

    public bool isInMM = true;
    public bool isInCharacterSelect = false;
    public bool isInTrack = false;
    public PlayerInputManager playerInputManager;
    public Scene currentScene;

    public GameObject defaultModelIfNoNull; //shldnt do this here and move to kartmanager but its the easiest way to modify it cuz its always existing and i really do not car enad give no shits nay mroe. (lies, i do care but i dont care eh, u know waht i mean) 
    
    public enum Scene
    {
        MainMenu,
        CharacterSelect,
        Track
    }
    
    private void Awake()
    {
        if (instance != null) { Destroy(this.gameObject);}
        instance = this;
        currentScene = Scene.MainMenu;
    }

    void Start()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
        // Im gonna instantitate here:
        if (isTest)
        {
            return;
        }
        SceneManager.LoadScene("CharacterMenu", LoadSceneMode.Additive);
    }
    
    public void OnAddPlayer(PlayerInput playerInput)
    {
        playerManagers.Add(playerInput.GetComponent<PlayerManager>());
        // Add to the scene im in.
        SceneManager.MoveGameObjectToScene(playerInput.gameObject, this.gameObject.scene);
    }

    public void TransferToTrack()
    {
        playerInputManager.DisableJoining();
        print("Going to next level!");
        SceneManager.UnloadSceneAsync("CharacterMenu", UnloadSceneOptions.None);
        SceneManager.LoadScene("Map1", LoadSceneMode.Additive);
    }
}
