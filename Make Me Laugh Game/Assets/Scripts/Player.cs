using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Callbacks;
using UnityEditor.SceneManagement;
using UnityEngine;

// This class will be used for all "players" in the game including the enemies that you will be dueling
// When an instance of this class is created, a deck is automatically created for it if the decklist exists
public class Player
{
    CardSystem cardSystem;
    public string playerDeckName;

    // This is a custom constructor, denoted by a function with the exact same name as the owning class.
    // This will be called automatically when a Player class instance is created with the parameters matching this one (a single string parameter)
    // Other constructors can be made with different parameters and allows you to do different things with the player data, depending on the number/order/type of parameters given
    // For example: public Player(int playerId) could be used if we wanted to create a player using their id instead of a name and you could provide different functionality in this new custom constructor
    public Player(string playerName)
    {
        cardSystem = new CardSystem();

        CreateDeck(playerName);
    }

    // Function to create a players starting deck from the cards in their .json decklist
    // 1 Input Parameter
    // playerName = A string name used to get the appropriate decklist for this player
    void CreateDeck(string playerName)
    {
        string jsonFile = File.ReadAllText(Application.dataPath + "/Resources/" + playerName + ".json");
        
        if(jsonFile != "")
        {
            Cards cardsFromDeck = JsonUtility.FromJson<Cards>(jsonFile);

            playerDeckName = cardsFromDeck.deckname;

            Debug.Log(playerDeckName);

            foreach (Card card in cardsFromDeck.cards)
            {
                Debug.Log(card.cardname);
                cardSystem.DeckCards.Add(card);
            }

            Debug.Log(playerName + " is live");
        }
        else
        {
            Debug.Log("ERROR: COULD NOT FIND " + playerName + " FILE");
        }
    }
}