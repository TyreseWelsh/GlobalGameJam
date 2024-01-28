using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, IPointerExitHandler
{
    private Image image;
    private Transform handOptionsPanel;
    private Transform fieldOptionsPanel;

    int owner;
    public int id;
    string cardName;
    string cardImage;
    public int health;
    public int attack;
    string cardType;
    public int weighting;
    string effect;
    bool positive;
    bool targeting;
    int effectvalue;
    string namedcard;
    public CardState currentCardState;

    Player owningPlayer;
    HorizontalLayoutGroup owningHand;
    HorizontalLayoutGroup owningField;

    GameObject opponentHand;
    GameObject opponentField;

    bool attacked = false;
    bool usedEffect = false;

    GameManager gameManager;
    CardVisualHandler cardVisuals;


    private void Awake()
    {
        handOptionsPanel = this.gameObject.transform.Find("HandOptionsPanel");
        fieldOptionsPanel = this.gameObject.transform.Find("FieldOptionsPanel");
        image = GetComponent<Image>();

        gameManager = GameObject.Find("Managers").GetComponent<GameManager>();
        cardVisuals = GetComponent<CardVisualHandler>();
    }

    public void Init(Card _card, Player _owningPlayer)
    {
        owner = _card.owner;
        id = _card.cardid;
        cardName = _card.cardname;
        cardImage = _card.cardimage;
        health = _card.health;
        attack = _card.attack;
        cardType = _card.type;
        weighting = _card.weighting;
        effect = _card.effectname;
        targeting = _card.targeting;
        effectvalue = _card.effectvalue;
        namedcard = _card.namedcard;
        currentCardState = _card.currentCardState;

        owningPlayer = _owningPlayer;
        owningHand = _card.owningHand;
        owningField = _card.owningField;

        SetOpponentAreas();

        cardVisuals.UpdateVisuals(_card);
    }

    private void SetOpponentAreas()
    {
        List<GameObject> hands = new List<GameObject>();
        hands.Add(GameObject.Find("PlayerHand"));
        hands.Add(GameObject.Find("EnemyHand"));
        hands.Remove(owningHand.gameObject);
        opponentHand = hands[0];

        List<GameObject> fields = new List<GameObject>();
        fields.Add(GameObject.Find("PlayerPlayArea"));
        fields.Add(GameObject.Find("EnemyPlayArea"));
        fields.Remove(owningHand.gameObject);
        opponentField = fields[0];
    }

    public void EndTurn()
    {
        attacked = false;
        usedEffect = false;

        var attackButton = fieldOptionsPanel.Find("AttackButton");
        attackButton.gameObject.SetActive(true);
    }








    public void OnPointerClick(PointerEventData eventData)
    {
        switch (gameManager.currentTurnPhase)
        {
            case (TurnPhase.Open):
                switch (currentCardState)
                {
                    case (CardState.InHand):
                        handOptionsPanel.gameObject.SetActive(true);
                        break;
                    case (CardState.OnField):
                        fieldOptionsPanel.gameObject.SetActive(true);
                        break;
                }
                break;
            case (TurnPhase.Targeting):
                gameManager.currentlyTargetedCard = gameObject;
                gameManager.ApplyCardEffect();
                gameManager.currentTurnPhase = TurnPhase.Open;
                break;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(currentCardState == CardState.InHand)
        {
            transform.SetParent(transform.root);
            handOptionsPanel.gameObject.SetActive(false);
            fieldOptionsPanel.gameObject.SetActive(false);
            image.raycastTarget = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(currentCardState == CardState.InHand && weighting <= gameManager.user.currentWeighting)
        {
            transform.position = Input.mousePosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(weighting <= gameManager.user.currentWeighting)
        {
            if (currentCardState != CardState.OnField)
            {
                transform.SetParent(CardManager.LastEnteredDropZone);
                image.raycastTarget = true;
            }

            if (CardManager.LastEnteredDropZone == GameObject.Find("PlayerPlayArea").transform)
            {
                currentCardState = CardState.OnField;
                owningPlayer.FieldCards.Add(gameObject);
                owningPlayer.HandCards.Remove(gameObject);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        handOptionsPanel.gameObject.SetActive(false);
        fieldOptionsPanel.gameObject.SetActive(false);
    }




    public void Use()
    {
        if(gameManager.currentTurn == Turn.player && weighting <= gameManager.user.currentWeighting
            || gameManager.currentTurn == Turn.opponent && weighting <= gameManager.currentOpponent.currentWeighting)
        {
            cardType = cardType.ToUpper();
            switch (cardType)
            {
                case ("RAT"):
                    transform.SetParent(owningField.transform);
                    currentCardState = CardState.OnField;
                    owningPlayer.FieldCards.Add(gameObject);
                    owningPlayer.HandCards.Remove(gameObject.gameObject);
                    break;
                case ("ACTION"):
                    if (gameManager.currentTurn == Turn.opponent && positive && gameManager.currentOpponent.FieldCards.Count > 0)
                    {
                        UseEffect();
                        Destroy(gameObject);
                    }
                    else if (gameManager.currentTurn == Turn.opponent && !positive && gameManager.user.FieldCards.Count > 0)
                    {
                        UseEffect();
                        Destroy(gameObject);
                    }
                    else if (gameManager.currentTurn == Turn.player && positive && gameManager.user.FieldCards.Count > 0)
                    {
                        UseEffect();
                        Destroy(gameObject);
                    }
                    else if (gameManager.currentTurn == Turn.player && !positive && gameManager.currentOpponent.FieldCards.Count > 0)
                    {
                        UseEffect();
                        Destroy(gameObject);
                    }
                    break;
            }
        }
    }

    public void AddWeight()
    {
        owningPlayer.currentWeighting += weighting;
        Destroy(gameObject);
    }

    public void Attack()
    {
        if(!attacked)
        {
            attacked = true;
            var attackButton = fieldOptionsPanel.Find("AttackButton");
            attackButton.gameObject.SetActive(false);
            if (gameManager.currentOpponent.FieldCards.Count <= 0)
            {
                gameManager.currentOpponent.playerHealth -= attack;
                if(gameManager.currentOpponent.playerHealth <= 0)
                {
                     print(owningPlayer.playerDeckName + " wins! YAY");
                }
            }
            else
            {
                gameManager.currentTurnPhase = TurnPhase.Targeting;
                // DISPLAY TARGET PROMPT TEXT
                gameManager.sourceCard = gameObject;
                gameManager.cardEffectToApply = "BasicAttack";
                gameManager.cardEffectToApplyValue = attack;
            }
        }
    }

    public void UseEffect()
    {
        if(targeting && gameManager.currentTurn == Turn.player)
        {
            gameManager.currentTurnPhase = TurnPhase.Targeting;
        }
        else if (targeting && gameManager.currentTurn == Turn.opponent)
        {
            gameManager.currentlyTargetedCard = gameManager.user.FieldCards[0];
        }
        gameManager.sourceCard = gameObject;
        gameManager.cardEffectToApply = effect;
        gameManager.cardEffectToApplyValue = effectvalue;
    }
}