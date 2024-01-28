using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

enum Turn
{
    player,
    opponent
}

enum TurnPhase
{
    Draw,
    Open,
    Targeting,
    Battle,
    End
}

// Game Manager class I think will be used to manage the players and the current game state
public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject cardBase;
    [SerializeField] HorizontalLayoutGroup playerHand;
    [SerializeField] GameObject opponentHand;

    List<Player> m_players = new List<Player>();                                            // m_player[0] will also be the user

    Turn currentTurn = Turn.player;
    int currentTurnNum = 0;
    Player user;
    Player currentOpponent;

    // Start is called before the first frame update
    void Start()
    {
        print("GameManager is live");
        ParsePlayerNames();

        user = m_players[0];
        //currentOpponent = m_players[1];


        GameObject newCard = Instantiate(cardBase, playerHand.transform);
        CardVisualHandler currentCardVisuals = newCard.GetComponent<CardVisualHandler>();
        currentCardVisuals.Init(user.CardSystem.DeckCards[0]);

        //CardEffects.Draw(user, 5);
        //CardEffects.Draw(currentOpponent, 5);
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
                InstantiatePlayer(playername, cardBase, playerHand);
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
    void InstantiatePlayer(string _playerName, GameObject _cardBase, HorizontalLayoutGroup _playerHand)
    {
        Player playerToAdd = new Player(_playerName, _cardBase, _playerHand);
        m_players.Add(playerToAdd);
    }
}
