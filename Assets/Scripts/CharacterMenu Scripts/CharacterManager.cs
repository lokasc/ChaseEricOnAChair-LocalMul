using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using System;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CharacterManager : MonoBehaviour
{
    [Header("Events")] public UnityEvent OnNewPlayerJoin;
    public UnityEvent<Character> OnPlayer1Selected;
    public UnityEvent<Character> OnPlayer2Selected;
    public UnityEvent OnBothPlayersReady;
    
    [Header("Co-op References")]
    public List<CharacterSelectControls> characterSelectors;
    public GameObject characterSelectPrefab;
       
    [Header("Rest")]
    public Camera mainCamera;
    public Camera overviewCamera;

    public string[] characterNames;    
    List<Character> availableCharacters = new List<Character>();

    //actually these two might not be needed
    bool isAnimating; //if camera is moving
    float transitionTime = 0.5f;//how fast we want the camera movements to be
    public int currentIndex = -1;//-1 is overview
    
    //overview camera movement
    float overviewOscillateSpeed = 0.5f;
    float overviewMinZ = 52f;
    float overviewMaxZ = 68f;

    float overviewOscillateTime = 0f;
    
    void Start()
    {
        // subscribe to Onjoin Event.
        GameManager.instance.playerInputManager.playerJoinedEvent.AddListener(OnNewPlayerJoined);
        
        for (int i = 0; i < characterNames.Length; i++)
        {
            GameObject b = GameObject.Find(characterNames[i]);
            if (b != null)
            {
                Character bc = b.GetComponent<Character>();
                if (bc != null)
                    availableCharacters.Add(bc);
            }
        }

        MoveToOverview();
    }

    // A new player joined, create a character select prefab and give them the correct controls.
    void OnNewPlayerJoined(PlayerInput newPlayerInput)
    {
        GameObject newPlayer = Instantiate(characterSelectPrefab);
        CharacterSelectControls control = newPlayer.GetComponent<CharacterSelectControls>();
        characterSelectors.Add(control);
        control.playerInput = newPlayerInput;
        
        CharacterSelector infoHolder = newPlayer.GetComponent<CharacterSelector>();
        infoHolder.myManager = newPlayerInput.GetComponent<PlayerManager>();
        OnNewPlayerJoin.Invoke();
    }
    
    void Update()
    {
        if (isAnimating) return;
        Vector2 nav = new Vector2();

        //overview camera movement
        if (currentIndex == -1)
        {
            OscillateOverviewCamera();
        }
        
        // Dont check for input if we dont have anything. 
        if (characterSelectors.Count == 0)
        {
            return; 
        }
        
        // Let both players navigate, its gonna be funny (maybe)
        foreach (CharacterSelectControls input in characterSelectors)
        {
            if (input.GetNavigate().x == 0)
            {
                continue; 
            }
            nav = input.GetNavigate();
        }
        // nav = characterSelectors[1].GetNavigate(); // Idk how to make them fight each other lol.
        
        if (currentIndex != -1)
        {
            if (nav.x > 0.5f)
            {
                int next = (currentIndex + 1) % availableCharacters.Count;
                MoveToCharacter(next);
                return;
            }
            if (nav.x < -0.5f)
            {
                int prev = (currentIndex - 1 + availableCharacters.Count) % availableCharacters.Count;
                MoveToCharacter(prev);
                return;
            }
        }
        
        // Selecting characters
        foreach (CharacterSelectControls input in characterSelectors)
        {
            if (input.SelectPressed() && !IsPlayersReady())//select button
            {
                // This goes from Main Menu to Character Select. (overview to first character)
                // Can only continue if theres a player connected.
                if ((currentIndex == -1 && availableCharacters.Count > 0) && GameManager.instance.currentScene == GameManager.Scene.MainMenu && GameManager.instance.playerInputManager.playerCount >= 1)
                {
                    MoveToCharacter(0);
                    GameManager.instance.currentScene = GameManager.Scene.CharacterSelect;
                }
                else if (currentIndex != -1)//selecting a character
                {
                    Character selected = availableCharacters[currentIndex];
                    CharacterSelector infoHolder = input.GetComponent<CharacterSelector>();

                    infoHolder.characterName = selected.name;
                    infoHolder.legLength = selected.legPower;
                    infoHolder.legCooldown = selected.cooldown;
                    infoHolder.handling = selected.steerAngle;
                    infoHolder.model = selected.model;
                    infoHolder.CopyToManager();
                    infoHolder.isSelected = true;
                    // print("Player " + (characterSelectors.IndexOf(input) + 1).ToString() + " selected " + infoHolder.characterName);
                    
                    // Selected Events!
                    if (characterSelectors.IndexOf(input) == 0 ) { OnPlayer1Selected.Invoke(selected); print("Player " + (characterSelectors.IndexOf(input) + 1).ToString() + " selected " + infoHolder.characterName); }
                    if (characterSelectors.IndexOf(input) == 1) { OnPlayer2Selected.Invoke(selected); print("Player " + (characterSelectors.IndexOf(input) + 1).ToString() + " selected " + infoHolder.characterName); }
                    
                    //Check if both players are selected, if so fire an event once! 
                    if (IsPlayersReady())
                    {
                        OnBothPlayersReady.Invoke();
                        print("Both Players Ready!");
                    }
                }

                return; // this line is here so that u cant press select and go into the next level as the first player
            }
        }
        
        // When both players have selected characters and ready to go.
        if (characterSelectors[0].SelectPressed() && IsPlayersReady() && PlayerInputManager.instance.playerCount > 1 )
        {
            // ask gm to call next.
            GameManager.instance.TransferToTrack();
        }
        

        // If anyone pressses go back, we remove selected choices and we go to main menu
        foreach (CharacterSelectControls input in characterSelectors)
        {
            if (input.CancelPressed())//back out back to main screen overview
            {
                if (currentIndex != -1)
                {
                    foreach (CharacterSelectControls y in characterSelectors)
                    {
                        y.GetComponent<CharacterSelector>().ResetChoice();
                    }
                    MoveToOverview();
                }
            }
        }
    }

    void MoveToOverview()
    {
        if (overviewCamera == null) return;
        StartCoroutine(MoveCamera(mainCamera.transform, overviewCamera.transform, overviewCamera.fieldOfView));
        currentIndex = -1;
        
        overviewOscillateTime = 0f;
        
        GameManager.instance.currentScene = GameManager.Scene.MainMenu;
    }

    void MoveToCharacter(int index)
    {
        if (index < 0 || index >= availableCharacters.Count) return;

        Camera targetCam = availableCharacters[index].DefaultCamera;
        StartCoroutine(MoveCamera(mainCamera.transform, targetCam.transform, targetCam.fieldOfView));
        currentIndex = index;
    }

    //helper coroutine for moving camera stuff
    IEnumerator MoveCamera(Transform cam, Transform target, float targetFOV)
    {
        isAnimating = true;

        Vector3 startPos = cam.position;
        Quaternion startRot = cam.rotation;
        float startFOV = mainCamera.fieldOfView;

        float elapsed = 0f;

        while (elapsed < transitionTime)
        {
            float t = elapsed / transitionTime;
            t = t * t * (3f - 2f * t);

            cam.position = Vector3.Lerp(startPos, target.position, t);
            cam.rotation = Quaternion.Slerp(startRot, target.rotation, t);
            mainCamera.fieldOfView = Mathf.Lerp(startFOV, targetFOV, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        cam.position = target.position;
        cam.rotation = target.rotation;
        mainCamera.fieldOfView = targetFOV;

        isAnimating = false;
    }

    //for moving camera back and forth during title screen
    void OscillateOverviewCamera()
    {
        if (overviewCamera == null) return;

        overviewOscillateTime += Time.deltaTime * overviewOscillateSpeed;

        float z = Mathf.Lerp(
            overviewMinZ,
            overviewMaxZ,
            (Mathf.Sin(overviewOscillateTime) + 1f) * 0.5f
        );

        Vector3 pos = mainCamera.transform.position;
        pos.z = z;
        mainCamera.transform.position = pos;
    }

    bool IsPlayersReady()
    {
        foreach (CharacterSelectControls charControl in characterSelectors)
        {
            if (!charControl.GetComponent<CharacterSelector>().isSelected)
            {
                return false;
            }
        }

        return true;
    }
}
