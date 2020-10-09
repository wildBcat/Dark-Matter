using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

/// <summary>
/// Creates the individual player setup menus and assigns the player input and index to them
/// </summary>
public class SpawnPlayerSetupMenu : MonoBehaviour
{
    private const string Name = "MainLayout";
    public GameObject playerSetupMenuPrefab;
    public PlayerInput input;

    /*
     * Locates the "MainLayout" game object
     * If it isn't null, instantiate the playersetupmenu into this root menu
     * Sets the UI input module to our player input component
     * Passes the player input index to the PlayerSetupMenuController's SetPlayerIndex method
     */
    private void Awake()
    {
        GameObject rootMenu = GameObject.Find(Name);
        if (rootMenu != null)
        {
            var menu = Instantiate(playerSetupMenuPrefab, rootMenu.transform);
            input.uiInputModule = menu.GetComponentInChildren<InputSystemUIInputModule>();
            menu.GetComponent<PlayerSetupMenuController>().SetPlayerIndex(input.playerIndex);
        }
    }
}
