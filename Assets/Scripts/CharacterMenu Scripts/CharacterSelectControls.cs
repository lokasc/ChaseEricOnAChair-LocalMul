using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterSelectControls : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;

    public Vector2 GetNavigate() =>
        playerInput.actions["Navigate"].ReadValue<Vector2>();

    public bool SelectPressed() =>
        playerInput.actions["Select"].triggered;

    public bool CancelPressed() =>
        playerInput.actions["Cancel"].triggered;
}
