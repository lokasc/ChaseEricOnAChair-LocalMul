using UnityEngine;
using UnityEngine.InputSystem;


// THIS IS SPECIFICALLY FOR INPUTING AND GOING BACK TO MAIN MENU CUZ LMAO!
public class VictoryInputStuff : MonoBehaviour
{
    private GameManager gm;
    public bool CanContinueBacktoZimmerLand = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CanContinueBacktoZimmerLand = false;
        gm = FindObjectOfType<GameManager>(); // wow! this is so much better than game manager instance, just super better for sum reason. 
        
        // Change the input settings
        foreach (PlayerManager player in gm.playerManagers)
        {
            player.playerInput.SwitchCurrentActionMap("CharacterSelect");
        }

        PlayerInputManager.instance.splitScreen = false;
    }
    
    void Update()
    {
        // Only Winning Player can press the button to continue LOL
        if (CanContinueBacktoZimmerLand)
        {
            if (gm.winningPlayer.playerInput.actions["Select"].triggered)
            {
                gm.ReloadTheGame(); // WE GO BACK TO DA STARTTTTTTTTTTTTTTTTTTTTT
            }
        }
        
    }

    public void AllowZimmering()
    {
        CanContinueBacktoZimmerLand = true;
    }
}
