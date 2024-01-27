using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Game Manager class I think will be used to manage the players and the current game state
public class GameManager : MonoBehaviour
{
    List<Player> m_players = new List<Player>();

    // Start is called before the first frame update
    void Start()
    {
        print("GameManager is live");
        ParsePlayerNames();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Function to read from a the PlayerList.json file which contains an array of the players that will be included in the game
    void ParsePlayerNames()
    {
        string playerListJson = File.ReadAllText(Application.dataPath + "/Resources/PlayerList.json");

        if (playerListJson != "")
        {
            Players allPlayers = JsonUtility.FromJson<Players>(playerListJson);

            foreach (string playername in allPlayers.playernames)
            {
                InstantiatePlayer(playername);
            }
        }
        else
        {
            print("ERROR: COULD NOT FIND PLAYER LIST FILE");
        }
    }

    // Used to create a new Player instance, constructed using the given playername
    // 1 Input Parameter
    // playername = The name of the player given by the designer and taken from the PlayerList.json file. Used to find appropriate deck for player.
    void InstantiatePlayer(string playerName)
    {
        Player playerToAdd = new Player(playerName);
        m_players.Add(playerToAdd);
    }
}
