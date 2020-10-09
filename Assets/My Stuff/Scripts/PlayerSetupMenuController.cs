using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Sets the UI and button function of the player selection screen
/// </summary>
public class PlayerSetupMenuController : MonoBehaviour
{
    // UI elements
    [SerializeField] private TextMeshProUGUI titleText = default;
    [SerializeField] GameObject readyPanel = default;
    [SerializeField] GameObject menuPanel = default;
    [SerializeField] Button readyButton = default;

    private int playerIndex;
    private float ignoreInputTime = 1.5f;
    private bool inputEnabled;

    /*
     * Assigns pi the value of the player index
     * Sets the title text to read the player designation
     * Sets the ignoreInputTime value to be now plus the original ignoreInputTime value
     */
    public void SetPlayerIndex(int pi)
    {
        playerIndex = pi;
        titleText.SetText("Player " + (pi + 1).ToString());
        ignoreInputTime = Time.time + ignoreInputTime;
    }

    /*
     * Checks to see if the current time is past the ignoreInputTime value
     * If it is, it will enable selection
     */
    void Update()
    {
        if (Time.time > ignoreInputTime)
        {
            inputEnabled = true;
        }
    }

    /*
     * Action for color button
     * If input is enabled, pass the button color, and the player index, to the player configuration manager game object's method "SetPlayerColor"
     * Activate the ready panel
     * Focus the ready button
     * Deactivate the menu panel
     */
    public void SetColor(Material color)
    {
        if (!inputEnabled) { return; }
        PlayerConfigurationManager.Instance.SetPlayerColor(playerIndex, color);
        readyPanel.SetActive(true);
        readyButton.Select();
        menuPanel.SetActive(false);
    }

    /*
     * Action for ready button
     * If input is enabled, pass the player index to the player configuration manager game object's method "ReadyPlayer"
     * Deactivate the ready button gameobject
     */
    public void ReadyPlayer()
    {
        if (!inputEnabled) { return; }
        PlayerConfigurationManager.Instance.ReadyPlayer(playerIndex);
        readyButton.gameObject.SetActive(false);
    }
}
