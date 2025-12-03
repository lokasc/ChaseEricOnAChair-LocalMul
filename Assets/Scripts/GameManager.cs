using System;
using UnityEngine;
using UnityEngine.Splines;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine.InputSystem;

// Need to rename this to KartManager.
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public SplineContainer mapSpline;
    public List<Chair> players;
    public bool isGameStarted = false;
    public bool isCountDown;
    public GameObject boss;
    public Transform[] spawnLocation;
    

    [SerializeField] private Camera tempCamera;
    public class ChairData
    {
        public Chair chair;
        public int currentLaps;
        public float t; // this is the ratio on the track.
        
        // Constructor
        public ChairData(Chair chair, int currentLaps = 1, float t = 0f)
        {
            this.chair = chair;
            this.currentLaps = currentLaps;
            this.t = t;
        }
    }
    
    
    
    public List<ChairData> playerRanking = new List<ChairData>();


    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (!isGameStarted && Input.GetKeyDown(KeyCode.Space))
        {
            InitializeGame();
            StartGame();
            StartCountDown();
        }

        if (!isGameStarted)
        {
            boss.GetComponent<MoveBasedOnSpline>().combinedSpeed = 0;
            return;
        }
        CalculatePositions();
    }
    
    // Grabs references..
    public void InitializeGame()
    {
        // Initialize the players
        foreach (Chair player in players)
        {
            // ChairData newChair = new ChairData();
            // newChair.chair = player;
            // newChair.currentLaps = 1;
            // newChair.t = 0;
            // playerRanking.Add(newChair);
        }
        
    }
    
    // Starts the game.
    public void StartGame()
    {
        PlayerInputManager.instance.DisableJoining();
        tempCamera.targetDisplay = 2;
        isGameStarted = true;
        boss.GetComponent<MoveBasedOnSpline>().combinedSpeed = boss.GetComponent<MoveBasedOnSpline>().speed;
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
            SplineUtility.GetNearestPoint(mapSpline.Spline, playerRank.chair.transform.position, out Unity.Mathematics.float3 nearestPoint,
                out float t);
            
            // Abs to wrap.
            float diff = Mathf.Abs(t - bossOffset);
            playerRank.t = Mathf.Min(diff, 1f - diff);
            // print("playerRank t: " + t + " bossOffset: " + bossOffset);
            // print(Mathf.Abs(playerRank.t));
        }

        // Sort based on rank and lap!
        playerRanking.Sort((a, b) =>
        {
            // Compares laps, if b is smaller than a, we compare t
            // lapCompare is positive if b is larger than a, same is 0
            int lapCompare = b.currentLaps.CompareTo(a.currentLaps);

            // sort by t if rank is same
            if (lapCompare != 0) return lapCompare;
            
            return a.t.CompareTo(b.t);
        });
        string combinedText = "";

        foreach (ChairData x in playerRanking)
        {
            
            combinedText += x.chair.transform.name + " ";
        }
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
    
    // Will remove this later, just for this play-test:
    public void OnPlayerJoined(PlayerInput playerInput)
    {
        players.Add(playerInput.GetComponent<Chair>());
        playerRanking.Add(new ChairData(playerInput.GetComponent<Chair>(), 1, 0));
        InitializeGame();
        
        playerInput.gameObject.transform.position = spawnLocation[PlayerInputManager.instance.playerCount - 1].position;
        playerInput.gameObject.transform.rotation = Quaternion.Euler(0, -180, 0);
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
