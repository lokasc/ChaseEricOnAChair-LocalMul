using System;
using UnityEngine;
using UnityEngine.Splines;
using System.Collections.Generic;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public SplineContainer mapSpline;
    public Chair[] players;
    public bool isGameStarted;
    
    public class ChairData
    {
        public Chair chair;
        public int currentLaps;
        public float t;
    }
    
    public List<ChairData> playerRanking = new List<ChairData>();


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartGame();
    }

    private void Update()
    {
        if (!isGameStarted) return;
        
        CalculatePositions();
    }


    // Call this when players press play, for now it will be called at the very start of the game!
    public void StartGame()
    {
        isGameStarted = true;
        
        // Initialize the players
        foreach (Chair player in players)
        {
            ChairData newChair = new ChairData();
            newChair.chair = player;
            newChair.currentLaps = 0;
            newChair.t = 0;
            playerRanking.Add(newChair);
        }
    }


    void CalculatePositions()
    {
        // Get percentage based on my position.
        foreach (ChairData playerRank in playerRanking)
        {
            SplineUtility.GetNearestPoint(mapSpline.Spline, playerRank.chair.transform.position, out Unity.Mathematics.float3 nearestPoint,
                out float t);
            
            playerRank.t = t;
        }

        // Sort based on rank and lap!
        playerRanking.Sort((a, b) =>
        {
            // Compares laps, if b is smaller than a, we compare t
            // lapCompare is positive if b is larger than a, same is 0
            int lapCompare = b.currentLaps.CompareTo(a.currentLaps);

            // sort by t if rank is same
            if (lapCompare != 0) return lapCompare;
            
            return b.t.CompareTo(a.t);
        });
        string combinedText = "";

        foreach (ChairData x in playerRanking)
        {
            
            combinedText += x.chair.transform.name + " ";
        }
        // print(combinedText);
    }
    
    // Returns the position of the car (1 -> num of players)
    public int GetCurrentPosition(Chair chair)
    {
        foreach(ChairData x in playerRanking)
        {
            if (x.chair == chair)
            {
                return playerRanking.IndexOf(x) + 1;
            }
        }
        return -1;
    }
}
