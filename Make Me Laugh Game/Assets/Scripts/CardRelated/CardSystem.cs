using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Holds the lists a player will need to store cards
// (Might move this to the Player class at some point to keep everything together)
public class CardSystem
{   
    // Lists containing the appropriate cards for this player
    private List<Card> boardCards = new List<Card>();
    public List<Card> BoardCards { get { return boardCards; } }                                     // "Properties" - Essentially a safe way for other files/classes to get the value of these variables (the same as a getter function just formatted differently)
                                                                                                    // Video explaining - https://www.youtube.com/watch?v=HzIqrlSbjjU
    private List<Card> deckCards = new List<Card>();
    public List<Card> DeckCards { get { return deckCards; } }

    private List<Card> handCards = new List<Card>();
    public List<Card> HandCards { get { return handCards; } }

    private List<Card> graveyardCards = new List<Card>();
    public List<Card> GraveyardCards { get { return graveyardCards; } }
}
