using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

/// <summary>
/// Handles various inputs for each player, such as control inputs and player color
/// </summary>
public class PlayerInputHandler : MonoBehaviour
{
    private PlayerConfiguation playerConfig;
    private PlayerMovement playerMovement;

    [SerializeField] private MeshRenderer playerMesh = default;

    private Controls controls;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        controls = new Controls();
    }

    /*
     * Overloads the method with the PlayerConfiguration class
     * Sets the player's material
     * Listens to player inputs - using C# events instead of Unity events
     */
    public void InitializePlayer(PlayerConfiguation pc)
    {
        playerConfig = pc;
        playerMesh.material = pc.PlayerMaterial;
        playerConfig.Input.onActionTriggered += Input_onActionTriggered;
    }

    /*
     * Handler method
     * Checks to see if The object action name matches the control input name
     * If it does, trigger the onMove method
     */
    private void Input_onActionTriggered(CallbackContext obj)
    {
        if (obj.action.name == controls.Gameplay.MoveInput.name)
        {
            OnMove(obj);
        }
    }

    // Passes the input values triggered by the player
    public void OnMove(CallbackContext context)
    {
        playerMovement.SetInputVector(context.ReadValue<Vector2>());
    }
}
