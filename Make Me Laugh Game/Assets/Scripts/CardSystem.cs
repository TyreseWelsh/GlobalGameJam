using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardSystem : MonoBehaviour
{
    System.Random rand;

    Card card;
    
    // Lists containing the appropriate cards for this player
    private List<Card> boardCards = new List<Card>();
    public List<Card> BoardCards { get { return boardCards; } }                                     // "Properties" - Essentially a way safe way for other files/classes to get the value of these variables (the same as a getter function just formatted differently)
                                                                                                    // Video explaining - https://www.youtube.com/watch?v=HzIqrlSbjjU
    private List<Card> deckCards = new List<Card>();
    public List<Card> DeckCards { get { return deckCards; } }

    private List<Card> handCards = new List<Card>();
    public List<Card> HandCards { get { return handCards; } }

    private List<Card> graveyardCards = new List<Card>();
    public List<Card> GraveyardCards { get { return graveyardCards; } }


    // Start is called before the first frame update
    void Start()
    {
        rand = new System.Random();


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Function to move a singular card from one list to another at the desired position. For example "adding" a card from the hand to the graveyard in the form of discarding or adding from deck to hand.
    // 4 Input parameters:
    // CardIndex = The position of the card to take from the list,
    // ListToTakeFrom = The list to move a card FROM
    // ListToMoveTo = The list to move the card TO
    // PositionToMoveTo = Either "start" to move the card to the front of the list or "end" to move the card to the end of the list
    public void MoveACardFromList(int CardIndex, List<Card> ListToTakeFrom, List<Card> ListToMoveTo, string PositionToMoveTo)
    {
        PositionToMoveTo = PositionToMoveTo.ToUpper();

        if (CardIndex >= 0 && CardIndex < ListToTakeFrom.Count())
        {
            switch (PositionToMoveTo)
            {
                case "START":
                    ListToMoveTo.Prepend(ListToTakeFrom[CardIndex]);
                    ListToTakeFrom.RemoveAt(CardIndex);
                    break;
                case "END":
                    ListToMoveTo.Add(ListToTakeFrom[CardIndex]);
                    ListToTakeFrom.RemoveAt(CardIndex);
                    break;
            }
        }
        else
        {
            Debug.Log("ERROR: CARD INDEX OUT OF BOUNDS - CANNOT ADD SINGULAR CARD TO LIST");
        }
    }

    // Function to move a given number of cards from one list to another at the desired position. Calls "MoveACardFromList" function as many times as given.
    // 4 Input Parameters:
    // NumCardsToMove = The number of cards to move from the first list to the second
    // ListToTakeFrom = The list to move the cards FROM
    // ListToMoveTo = The list to move the cards TO
    // PositionToMoveTo = Either "start" to move the cards to the front of the list or "end" to move the cards to the end of the list
    public void MoveCardsFromList(int NumCardsToMove, List<Card> ListToTakeFrom, List<Card> ListToMoveTo, string PositionToMoveTo)
    {
        if(NumCardsToMove <= ListToTakeFrom.Count())
        {
            PositionToMoveTo = PositionToMoveTo.ToUpper();

            switch (PositionToMoveTo)
            {
                case "START":
                    for (int i = 0; i < NumCardsToMove; i++)
                    {
                        MoveACardFromList(0, ListToTakeFrom, ListToMoveTo, PositionToMoveTo);
                    }
                    break;
                case "END":
                    for (int i = 0; i < NumCardsToMove; i++)
                    {
                        MoveACardFromList(ListToMoveTo.Count() - 1, ListToTakeFrom, ListToMoveTo, PositionToMoveTo);
                    }
                    break;
            }
        }
        else
        {
            Debug.Log("ERROR: LIST DOES NOT CONTAIN ENOUGH CARDS TO MOVE");
        }
    }

    // Function to shuffle a given list in place using the Fisher-Yates shuffle which you can read about here - https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle
    // How it works: Get a random number from 1 to the number of items (n), then swap the position of the nth and last element, then repeat but continually minus 1 from the range the random number is taken from
    // so if their were 10 items the first time going through (1 to 10), for this next time do 1 to 9 then 1 to 8 and so on
    // 1 Input Parameter:
    // ListToShuffle = The given lift to shuffle in place
    public void ShuffleList(List<Card> ListToShuffle)
    {
       for (int i = ListToShuffle.Count() - 1; i > 0; i--)
        {
            int randNum = rand.Next(i + 1);
            Card value = ListToShuffle[randNum];
            ListToShuffle[randNum] = ListToShuffle[i];
            ListToShuffle[i] = value;
        }
    }
}
