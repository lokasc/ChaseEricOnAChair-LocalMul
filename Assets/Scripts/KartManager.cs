using System;
using UnityEngine;
using UnityEngine.Splines;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine.InputSystem;

// Need to rename this to KartManager.
public class KartManager : MonoBehaviour
{
    public static KartManager Instance { get; private set; }
    public SplineContainer mapSpline;
    public List<Chair> players;
    public bool isGameStarted = false;
    public bool isCountDown;
    public GameObject boss;
    public Transform[] spawnLocation;
    
    public GameObject chairBasePrefab;

    // [SerializeField] private Camera tempCamera;
    public class ChairData
    {
        public Chair chair;
        public int currentLaps;
        public float t; // this is the ratio on the track.
        public float trueT = 0f;
        
        // Constructor
        public ChairData(Chair chair, int currentLaps = 1, float t = 0f)
        {
            this.chair = chair;
            this.currentLaps = currentLaps;
            this.t = t;
            this.trueT = 0;
        }
    }
    
    public List<ChairData> playerRanking = new List<ChairData>();


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // Get a copy of the list cuz unity buggy af.
        List<PlayerManager> copy = GameManager.instance.playerManagers;

        int index = 0;
        //Initialize & Set stats
        foreach (PlayerManager player in copy)
        {
            var newPlayer = Instantiate(chairBasePrefab);
            Chair newChair = newPlayer.GetComponent<Chair>();

            // // Get the component for playerInput; // i know naming is werid but chain of refs.
            newChair.playerInput.playerInput = player.playerInput;
            player.playerInput.camera = newChair.GetComponent<CameraFollow>().myCamera;

            // Set stats
            newChair.legPower = player.legLength;
            newChair.legCoolDown = player.cooldown;
            newChair.maxSteerAngle = player.handling;
            
            // Add zimmerkart specific model, parent it in da model container >-<
            newChair.modelContainer.transform.GetChild(0).gameObject.SetActive(false); // we removin all da default chairs.

            if (player.characterModel == null)
            {
                // Cuz im lazy to make the individual models to test the different characters so im adding a default one, mars.
                Debug.LogWarning("character model null, please add a character model or i will lose my mind.");
                Instantiate(GameManager.instance.defaultModelIfNoNull, newChair.modelContainer.transform);
            }
            else
            {
                Instantiate(player.characterModel, newChair.modelContainer.transform);
            }
            
            newChair.InitializeChair();

            // Add to arrays to remember references.
            players.Add(newChair);
            playerRanking.Add(new ChairData(newChair, 1, 0));

            // Place players into the correct positions.
            newPlayer.transform.position = spawnLocation[index].position;
            // newPlayer.transform.rotation = Quaternion.Euler(0, -180, 0);
            newPlayer.transform.rotation = Quaternion.Euler(spawnLocation[index].rotation.eulerAngles);
            
            //Swithc the fuckign input action map cuz unity hella bent and needs a lot of help 
            
            player.playerInput.SwitchCurrentActionMap("Driving");
            index++;
        }

        // Activate SplitScreen
        PlayerInputManager.instance.splitScreen = true;
        StartGame();
    }

    private void Update()
    {

        if (!isGameStarted)
        {
            boss.GetComponent<MoveBasedOnSpline>().currentSpeed = 0;
            return;
        }
        CalculatePositions();
    }
    
    // Starts the game.
    public void StartGame()
    {
        PlayerInputManager.instance.DisableJoining();
        // tempCamera.targetDisplay = 2;
        isGameStarted = true;
        boss.GetComponent<MoveBasedOnSpline>().currentSpeed = boss.GetComponent<MoveBasedOnSpline>().speed;
        StartCountDown();
    }

    void StartCountDown()
    {
        isGameStarted = true;
        foreach (Chair player in players)
        {
            player.playerUI.StartCountDown();
        }
    }

    // Calculate the player's current ranking based on Eric's Position.
    void CalculatePositions()
    {
        // Calculate Eric's position first.
        SplineUtility.GetNearestPoint(mapSpline.Spline, boss.transform.position, out float3 _ ,out float bossOffset);
        
        
        // Get percentage based on my position.
        foreach (ChairData playerRank in playerRanking)
        {
            SplineUtility.GetNearestPoint(mapSpline.Spline, playerRank.chair.rb.transform.position, out Unity.Mathematics.float3 nearestPoint,
                out float t);

            // print(playerRank.chair.rb.transform.position);
            playerRank.trueT = t;
            
            float offset = (t - bossOffset + 1f) % 1f;

            // Convert offset into signed -0.5 → +0.5
            float signedOffset = offset <= 0.5f ? offset : offset - 1f;
            
            
            // -0.5 → boss far ahead (playerbehind boss)
            // +0.5 → player far ahead (player ahead of boss).
            playerRank.t = signedOffset;


            // // Abs to wrap.
            // float diff = Mathf.Abs(t - bossOffset);
            // playerRank.t = Mathf.Min(diff, 1f - diff);
            // print("Player's offsetted: " + playerRank.t);
            //print("playerRank t: " + t + " bossOffset: " + bossOffset);
            // print("Player " + playerRank.chair.name + ": " + playerRank.trueT);
        }

        // // Sort based on rank and lap!
        // playerRanking.Sort((a, b) =>
        // {
        //     // Compares laps, if b is smaller than a, we compare t
        //     // lapCompare is positive if b is larger than a, same is 0
        //     int lapCompare = b.currentLaps.CompareTo(a.currentLaps);
        //
        //     // sort by t if rank is same
        //     if (lapCompare != 0) return lapCompare;
        //     
        //     return a.t.CompareTo(b.t);
        // });
        // string combinedText = "";
        //
        // foreach (ChairData x in playerRanking)
        // {
        //     
        //     combinedText += x.chair.transform.name + " ";
        // }
        // print(combinedText);
    }
    
    // Returns the position of the car (1 -> num of players)
    public int GetCurrentPosition(Chair player)
    {
        foreach(ChairData x in playerRanking)
        {
            if (x.chair == player)
            {
                return playerRanking.IndexOf(x) + 1;
            }
        }
        
        // Edge case, theres only one player
        if (playerRanking.Count == 1)
        {
            return 1;
        }
        
        return -1;
    }

    public int GetCurrentLaps(Chair player)
    {
        foreach (ChairData x in playerRanking)
        {
            if (x.chair == player)
            {
                return x.currentLaps;
            }
        }
        return -1;
    }
    
    public void AddLap(Chair player)
    {
        foreach (ChairData x in playerRanking)
        {
            Debug.Log("Adding!");
            if (x.chair == player)
            {
                x.currentLaps += 1;
            }
        }
    }
}
