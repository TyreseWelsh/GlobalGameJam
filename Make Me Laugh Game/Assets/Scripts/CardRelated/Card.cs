using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CardState
{
    InDeck,
    InHand,
    OnField,
    InChoiceList
}

// Conatins the data that defines this card
// Instances of this class will be constructed with the data from a players decklist
[System.Serializable]
public class Card
{
    public CardState currentCardState = CardState.InDeck;
    public int owner;
    public int cardid;
    public HorizontalLayoutGroup owningHand;
    public HorizontalLayoutGroup owningField;

    public string cardname;
    public string cardimage;
    public int health;
    public int attack;
    public string type;
    public int weighting;
    public string effectname;
    public bool positive;
    public bool targeting;
    public int effectvalue;
    public string namedcard;
    
    public void Init(Card _card)
    {
        currentCardState = _card.currentCardState;
        owner = _card.owner;
        cardid = _card.cardid;
        cardname = _card.cardname;
        cardimage = _card.cardimage;
        health = _card.health;
        attack = _card.attack;
        type = _card.type;
        weighting = _card.weighting;
        effectname = _card.effectname;
        positive = _card.positive;
        targeting = _card.targeting;
        effectvalue = _card.effectvalue;
        namedcard = _card.namedcard;
    }

    // Dont think i actually need this constructor but will keep it regardless
    //public Card(int _cardid, string _cardname, string _cardimage, int _health, int _attack, string _type, int _weighting, string _activationCost, List<string> _effects)
    //{
    //    cardid = _cardid;
    //    cardname = _cardname;
    //    cardimage = _cardimage;
    //    health = _health;
    //    attack = _attack;
    //    type = _type;
    //    weighting = _weighting;
    //    activationCost = _activationCost;
    //    effects = _effects;
    //}
}