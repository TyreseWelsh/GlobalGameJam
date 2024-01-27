using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardState
{
    InDeck,
    InHand,
    OnBoard,
    InGrave,
    InChoiceList
}

// Conatins the data that defines this card
// Instances of this class will be constructed with the data from a players decklist
[System.Serializable]
public class Card
{
    public CardState currentCardState = CardState.InDeck;

    public int cardid;
    public string cardname;
    public string cardimage;
    public int health;
    public int attack;
    public string type;
    public int weighting;
    public string activationCost;
    public List<string> effects;

    // Dont think i actually need this constructor but will keep it regardless
    public Card(int _cardid, string _cardname, string _cardimage, int _health, int _attack, string _type, int _weighting, string _activationCost, List<string> _effects)
    {
        cardid = _cardid;
        cardname = _cardname;
        cardimage = _cardimage;
        health = _health;
        attack = _attack;
        type = _type;
        weighting = _weighting;
        activationCost = _activationCost;
        effects = _effects;
    }
}