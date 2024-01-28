using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public enum Turn
{
    player,
    opponent
}

public enum TurnPhase
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
    [SerializeField] HorizontalLayoutGroup playerField;
    [SerializeField] HorizontalLayoutGroup opponentHand;
    [SerializeField] HorizontalLayoutGroup opponentField;

    List<Player> m_players = new List<Player>();                                            // m_player[0] will also be the user

    public Turn currentTurn = Turn.player;
    bool playerDrawn = true;
    [SerializeField] TextMeshProUGUI turnText;
    public TurnPhase currentTurnPhase = TurnPhase.Open;
    int currentTurnNum = 0;
    public Player user;
    public Player currentOpponent;
    [SerializeField] TextMeshProUGUI playerWeightText;
    [SerializeField] TextMeshProUGUI opponentWeightText;

    public GameObject sourceCard;
    public string cardEffectToApply;
    public int cardEffectToApplyValue;
    public GameObject currentlyTargetedCard;

    // Start is called before the first frame update
    void Start()
    {
        print("GameManager is live");
        ParsePlayerNames();

        user = m_players[0];
        currentOpponent = m_players[1];

        user.Draw(5);
        currentOpponent.Draw(5);
    }

    // Update is called once per frame
    void Update()
    {
        playerWeightText.text = user.currentWeighting.ToString();
        opponentWeightText.text = currentOpponent.currentWeighting.ToString();
    }

    // Function to read from a the PlayerList.json file which contains an array of the players that will be included in the game
    void ParsePlayerNames()
    {
        string playerListJson = File.ReadAllText(Application.dataPath + "/Resources/PlayerList.json");

        if (playerListJson != "")
        {
            Players allPlayers = JsonUtility.FromJson<Players>(playerListJson);
            int i = 0;
            foreach (string playername in allPlayers.playernames)
            {
                if(i == 0)
                {
                    InstantiatePlayer(playername, cardBase, playerHand, playerField);
                }
                else if(i > 0)
                {
                    InstantiatePlayer(playername, cardBase, opponentHand, opponentField);
                }

                i++;
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
    void InstantiatePlayer(string _playerName, GameObject _cardBase, HorizontalLayoutGroup _playerHand, HorizontalLayoutGroup _playerField)
    {
        Player playerToAdd = new Player(_playerName, _cardBase, _playerHand, _playerField);
        m_players.Add(playerToAdd);
    }

    public void ApplyCardEffect()
    {
        switch(currentTurn)
        {
            case (Turn.player):
                switch(cardEffectToApply)
                {
                    case ("BasicAttack"):
                        user.BasicAttack(currentOpponent, sourceCard, currentlyTargetedCard);
                        break;
                    case ("BuffHealth"):
                        user.BoostHealth(currentlyTargetedCard, cardEffectToApplyValue);
                        break;
                    case ("BoostAllAlliesHealth"):
                        user.BoostAllAlliesHealth(cardEffectToApplyValue);
                        break;
                    case ("BuffAttack"):
                        user.BoostAttack(currentlyTargetedCard, cardEffectToApplyValue);
                        break;
                    case ("BoostAllAlliesAttack"):
                        user.BoostAllAlliesAttack(cardEffectToApplyValue);
                        break;
                    case ("DebuffHealth"):
                        user.DebuffHealth(currentlyTargetedCard, cardEffectToApplyValue);
                        break;
                    case ("DebuffAttack"):
                        user.DebuffAttack(currentlyTargetedCard, cardEffectToApplyValue);
                        break;
                }
            break;
            case (Turn.opponent):
                switch (cardEffectToApply)
                {
                    case ("BasicAttack"):
                        currentOpponent.BasicAttack(user, sourceCard, currentlyTargetedCard);
                        break;
                    case ("BuffHealth"):
                        currentOpponent.BoostHealth(currentlyTargetedCard, cardEffectToApplyValue);
                        break;
                    case ("BuffAttack"):
                        currentOpponent.BoostAttack(currentlyTargetedCard, cardEffectToApplyValue);
                        break;
                    case ("DebuffHealth"):
                        currentOpponent.DebuffHealth(currentlyTargetedCard, cardEffectToApplyValue);
                        break;
                    case ("DebuffAttack"):
                        currentOpponent.DebuffAttack(currentlyTargetedCard, cardEffectToApplyValue);
                        break;
                }
                break;
        }
    }

    public void PlayerTurnDraw()
    {
        if (user.DeckData.Count >= 1 && !playerDrawn)
        {
            { 
                GameObject newCard = GameObject.Instantiate(cardBase, playerHand.transform);
                CardController currentCardController = newCard.GetComponent<CardController>();
                currentCardController.Init(user.DeckData[0], user);
                currentCardController.currentCardState = CardState.InHand;
                user.HandCards.Add(newCard);

                user.DeckData.RemoveAt(0);

                playerDrawn = true;
            }
        }
    }

    public void EndTurn()
    {
        user.EndTurn();
        currentOpponent.EndTurn();

        foreach(GameObject cardObj in user.FieldCards)
        {
            cardObj.GetComponent<CardController>().EndTurn();
        }
        foreach (GameObject cardObj in currentOpponent.FieldCards)
        {
            cardObj.GetComponent<CardController>().EndTurn();
        }

        switch (currentTurn)
        {
            case (Turn.player):
                currentTurn = Turn.opponent;
                turnText.text = "OPPONENTS TURN";
                AITakeTurn();
                break;
            case(Turn.opponent):
                currentTurn = Turn.player;
                turnText.text = "YOUR TURN";
                playerDrawn = false;
                break;
        }
    }

    public void AITakeTurn()
    {
        print("AI Taking turn...");
        StartCoroutine(WaitForDraw());
        
    }
    IEnumerator WaitForDraw()
    {
        yield return new WaitForSeconds(1.5f);
        print("Should draw...");
        currentOpponent.Draw(1);

        StartCoroutine(WaitForUseACardFromHand());
    }

    IEnumerator WaitForUseACardFromHand()
    {
        yield return new WaitForSeconds(2.0f);
        print("Should use card in hand...");
        GameObject cardToPlay = new GameObject();

        foreach (GameObject cardObj in currentOpponent.HandCards)
        {
            CardController cardScript = cardObj.GetComponent<CardController>();
            if(cardScript.weighting <= currentOpponent.currentWeighting)
            {
                cardToPlay = cardObj;
            }


        }
        if(cardToPlay.GetComponent<CardController>() != null)
        {
            cardToPlay.GetComponent<CardController>().Use();
        }

        StartCoroutine(WaitForAttack());

    }

    IEnumerator WaitForAttack()
    {
        yield return new WaitForSeconds(1.5f);
        print("Should attack...");

        if (user.FieldCards.Count > 0 && currentOpponent.FieldCards.Count > 0)
        {
            for (int i = 0; i <  currentOpponent.FieldCards.Count; i++)
            {
                if (user.FieldCards.Count > 0 && currentOpponent.FieldCards.Count > 0)
                {
                    CardController opCardScript = currentOpponent.FieldCards[i].GetComponent<CardController>();
                    CardController playerCardScript = user.FieldCards[0].GetComponent<CardController>();

                    opCardScript.health -= playerCardScript.attack;
                    playerCardScript.health -= opCardScript.attack;

                    if(playerCardScript.health <= 0)
                    {
                        user.FieldCards.Remove(user.FieldCards[0]);
                        Destroy(user.FieldCards[0]);
                    }
                    if(opCardScript.health <= 0)
                    {
                        currentOpponent.FieldCards.Remove(currentOpponent.FieldCards[i]);
                        Destroy(currentOpponent.FieldCards[i]);
                    }
                }
            }
        }

        StartCoroutine(WaitForEndTurn());
    }

    IEnumerator WaitForEndTurn()
    {
        yield return new WaitForSeconds(1.5f);
        print("Should end turn...");

        EndTurn();
    }
}
