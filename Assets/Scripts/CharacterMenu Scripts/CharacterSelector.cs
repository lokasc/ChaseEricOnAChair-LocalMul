using UnityEngine;


// This stores the information needed and transfers it to the PlayerManager
public class CharacterSelector : MonoBehaviour
{
    public PlayerManager myManager;
    public bool isSelected = false;
    
    [Header("Stats to collect")] public string characterName;
    public float legLength;
    public float legCooldown;
    public float handling;
    public GameObject model;

    
    public void CopyToManager()
    {
        myManager.cooldown = legCooldown;
        myManager.handling = handling;
        myManager.legLength = legLength;
        myManager.characterModel = model;
        myManager.characterName = characterName;
    }

    public void ResetChoice()
    {
        name = "";
        legLength = 0;
        legCooldown = 0;
        handling = 0;
        isSelected = false;
    }
}
