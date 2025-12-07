using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterSelectControls : MonoBehaviour
{
    [SerializeField] public PlayerInput playerInput;

    public Vector2 GetNavigate()
    {
        return playerInput.actions["Navigate"].ReadValue<Vector2>();
    }

    public bool SelectPressed() =>
        playerInput.actions["Select"].triggered;

    public bool CancelPressed() =>
        playerInput.actions["Cancel"].triggered;
}
