using UnityEngine;

public class CharacterInfo : MonoBehaviour
{
    public static CharacterInfo Instance;

    [Header("Player 1 Selection")]
    public string player1Character;
    public float legPower1;
    public float cooldown1;
    public float steerAngle1;

    [Header("Player 2 Selection")]
    public string player2Character;
    public float legPower2;
    public float cooldown2;
    public float steerAngle2;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); 
        }
    }
}
