using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// Creates a singleton that carries configurations of players from scene to scene
/// </summary>
public class PlayerConfigurationManager : MonoBehaviour
{
    private List<PlayerConfiguation> playerConfigs;

    public static PlayerConfigurationManager Instance { get; private set; }

    /*
     * Checks to see if the variable "instance" is already being used
     * If it is not, assigns this game object to the variable "instance"
     * Makes this game object persist between scenes
     * initializes the playerConfigs variable
     */
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("SINGLETON - Trying to create another instance of Singleton!");
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
            playerConfigs = new List<PlayerConfiguation>();
        }
    }

    public List<PlayerConfiguation> GetPlayerConfigs()
    {
        return playerConfigs;
    }

    // Assigns the selected color to the player who matches the player input index
    public void SetPlayerColor(int index, Material Color)
    {
        playerConfigs[index].PlayerMaterial = Color;
    }

    /*
     * sets the player who matches the player input index to "ready"
     * If there are as many players ready as the max player count, load the level 1 scene
     * CHANGE - make it so it will load the level 1 scene if all active players are ready, instead of max player count
     */
    public void ReadyPlayer(int index)
    {
        playerConfigs[index].IsReady = true;
        if (playerConfigs.All(p => p.IsReady == true))
        {
            SceneManager.LoadScene("Level 1");
        }
    }

    /*
     * Sets the player input transform to match this game object's transform
     * Checks to see if you haven't already added the player. If not, it adds the player to the player configuraiton list
     */
    public void HandlePlayerJoin(PlayerInput pi)
    {
        if (!playerConfigs.Any(p => p.PlayerIndex == pi.playerIndex))
        {
            pi.transform.SetParent(transform);
            playerConfigs.Add(new PlayerConfiguation(pi));
        }
    }
}

public class PlayerConfiguation
{
    /*
     * A constructor
     * Sets the PlayerIndex value to match the player input index
     * Sets the variable "input" the player input
     */
    public PlayerConfiguation(PlayerInput pi)
    {
        PlayerIndex = pi.playerIndex;
        Input = pi;
    }
    public PlayerInput Input { get; set; }
    public int PlayerIndex { get; set; }
    public bool IsReady { get; set; }
    public Material PlayerMaterial { get; set; }
}
