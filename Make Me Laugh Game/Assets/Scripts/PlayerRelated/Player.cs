using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Callbacks;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

// This class will be used for all "players" in the game including the enemies that you will be dueling
// When an instance of this class is created, a deck is automatically created for it if the decklist exists
public class Player
{
    private List<Card>deckData;
    public List<Card> DeckData{ get { return deckData; } }

    public List<GameObject> handCards;
    public List<GameObject> fieldCards;

    public string playerDeckName;
    public int weightLimit = 20;
    public int maxWeighting = 1;
    public int currentWeighting = 1;

    GameObject cardBase;
    HorizontalLayoutGroup playerHand;

    // This is a custom constructor, denoted by a function with the exact same name as the owning class.
    // This will be called automatically when a Player class instance is created with the parameters matching this one (a single string parameter)
    // Other constructors can be made with different parameters and allows you to do different things with the player data, depending on the number/order/type of parameters given
    // For example: public Player(int playerId) could be used if we wanted to create a player using their id instead of a name and you could provide different functionality in this new custom constructor
    public Player(string _playerName, GameObject _cardBase, HorizontalLayoutGroup _playerHand)
    {
        cardBase = _cardBase;
        playerHand = _playerHand;

        CreateDeck(_playerName);
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

            //Debug.Log(playerDeckName);

            int i = 0;
            foreach (Card card in cardsFromDeck.cards)
            {
                //Debug.Log(card.cardname);
                card.cardid = i;
                deckData.Add(card);
                i++;
            }

            //Debug.Log(playerName + " is live");
        }
        else
        {
            Debug.Log("ERROR: COULD NOT FIND " + playerName + " FILE");
        }
    }

    public void EndTurn()
    {
        maxWeighting++;
        currentWeighting = maxWeighting;
    }



    // Player card effects

    public void Draw(int amountToDraw)
    {
        if (deckData.Count >= amountToDraw)
        {
            for (int i = 0; i < amountToDraw; i++)
            {
                GameObject newCard = GameObject.Instantiate(cardBase, playerHand.transform);
                CardController currentCardController = newCard.GetComponent<CardController>();
                currentCardController.Init(deckData[0]);
                currentCardController.currentCardState = CardState.InHand;
                handCards.Add(newCard);

                deckData.RemoveAt(0);
            }
        }
    }

    public void RemoveFromField(Player opponent, GameObject targetCard)
    {
        opponent.fieldCards.Remove(targetCard);
        GameObject.Destroy(targetCard);
    }

    public void AddToHand(string cardToAdd)
    {
        foreach(Card card in deckData)
        {
            if(card.cardname == cardToAdd)
            {
                GameObject newCard = GameObject.Instantiate(cardBase, playerHand.transform);
                CardController currentCardController = newCard.GetComponent<CardController>();
                currentCardController.Init(card);
                currentCardController.currentCardState = CardState.InHand;

                deckData.Remove(card);
            }
        }
    }

    public void BoostAttack(GameObject targetCard, int buffValue)
    {
        CardController cardController = targetCard.GetComponent<CardController>();
        cardController.attack += buffValue;
        CardVisualHandler cardVisuals = targetCard.GetComponent<CardVisualHandler>();
        cardVisuals.cardAttack.text = cardController.attack.ToString();
    }

    public void BoostAllAlliesAttack(int buffValue)
    {
        foreach(GameObject cardObj in fieldCards)
        {
            BoostAttack(cardObj, buffValue);
        }
    }

    public void BoostHealth(GameObject targetCard, int buffValue)
    {
        CardController cardController = targetCard.GetComponent<CardController>();
        cardController.health += buffValue;
        CardVisualHandler cardVisuals = targetCard.GetComponent<CardVisualHandler>();
        cardVisuals.cardHealth.text = cardController.health.ToString();
    }

    public void BoostAllAlliesHealth(int buffValue)
    {
        foreach (GameObject cardObj in fieldCards)
        {
            BoostHealth(cardObj, buffValue);
        }
    }

    public void DebuffAttack(GameObject targetCard, int debuffValue)
    {
        CardController cardController = targetCard.GetComponent<CardController>();
        cardController.attack -= debuffValue;
        if (cardController.attack < 0)
        {
            cardController.attack = 0;
        }
        CardVisualHandler cardVisuals = targetCard.GetComponent<CardVisualHandler>();
        cardVisuals.cardAttack.text = cardController.attack.ToString();
    }

    public void DebuffAllEnemiesAttack(Player opponent, int buffValue)
    {
        foreach (GameObject cardObj in opponent.fieldCards)
        {
            DebuffAttack(cardObj, buffValue);
        }
    }

    public void DebuffHealth(GameObject targetCard, int debuffValue)
    {
        CardController cardController = targetCard.GetComponent<CardController>();
        cardController.health -= debuffValue;
        CardVisualHandler cardVisuals = targetCard.GetComponent<CardVisualHandler>();
        cardVisuals.cardAttack.text = cardController.attack.ToString();
        if (cardController.health <= 0)
        {
            GameObject.Destroy(targetCard);
        }
    }

    public void DebuffAllEnemiesHealth(Player opponent, int buffValue)
    {
        foreach (GameObject cardObj in opponent.fieldCards)
        {
            DebuffHealth(cardObj, buffValue);
        }
    }
}