using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class holds functions that can be called to cause different effects during a duel
public static class CardEffects
{
    // A function to move a number of cards from the top of the players deck to the hand
    public static void Draw(Player targetPlayer, int amountToDraw)
    {
        if(targetPlayer.CardSystem.DeckCards.Count >= amountToDraw)
        {
            for (int i = 0; i < amountToDraw; i++)
            {
                targetPlayer.CardSystem.DeckCards[i].currentCardState = CardState.InHand;
                ListManipulationFunctions.MoveACardFromList(targetPlayer.CardSystem.DeckCards[i], targetPlayer.CardSystem.DeckCards, targetPlayer.CardSystem.HandCards, "end");
            }
        }
    }

    // Removes an opponents card from the field, moving it to the graveyard
    public static void RemoveFromField(Player targetPlayer, int cardIdToRemove)
    {
        foreach(Card card in targetPlayer.CardSystem.BoardCards)
        {
            if(card.cardid == cardIdToRemove)
            {
                card.currentCardState = CardState.InGrave;
                ListManipulationFunctions.MoveACardFromList(card, targetPlayer.CardSystem.BoardCards, targetPlayer.CardSystem.GraveyardCards, "end");
            }
        }
    }

    // Adds a card of a specific name from the players deck to their hand
    public static void AddToHand(Player targetPlayer, string cardNameToAdd)
    {
        foreach (Card card in targetPlayer.CardSystem.DeckCards)
        {
            if(card.cardname == cardNameToAdd)
            {
                card.currentCardState = CardState.InHand;
                ListManipulationFunctions.MoveACardFromList(card, targetPlayer.CardSystem.DeckCards, targetPlayer.CardSystem.HandCards, "end");
            }
        }
    }

    // Increases the attack of the targeted card
    public static void BoostAttack(Player targetPlayer, int cardIdToTarget, int boostValue)
    {
        foreach(Card card in targetPlayer.CardSystem.BoardCards)
        {
            if (card.cardid == cardIdToTarget)
            {
                card.attack += boostValue;
            }
        }
    }

    // Increases the health of the target card
    public static void BoostHealth(Player targetPlayer, int cardIdToTarget, int boostValue)
    {
        foreach (Card card in targetPlayer.CardSystem.BoardCards)
        {
            if (card.cardid == cardIdToTarget)
            {
                card.health += boostValue;
            }
        }
    }

    // Decreases the attack of the targeted card by the given value
    // If the cards new attack is less than 0, change it to 0
    public static void DebuffAttack(Player targetPlayer, int cardIdToTarget, int debuffValue)
    {
        foreach (Card card in targetPlayer.CardSystem.BoardCards)
        {
            if (card.cardid == cardIdToTarget && card.attack > 0)
            {
                card.attack -= debuffValue;

                if (card.attack < 0)
                {
                    card.attack = 0;
                }
            }
        }
    }

    // Decreases the health of target card by the given value (same as taking damage)
    // If the cards new health is 0 or less, remove the card from the field
    public static void DebuffHealth(Player targetPlayer, int cardIdToTarget, int debuffValue)
    {
        foreach (Card card in targetPlayer.CardSystem.BoardCards)
        {
            if (card.cardid == cardIdToTarget)
            {
                card.health -= debuffValue;

                if(card.health <= 0)
                {
                    RemoveFromField(targetPlayer, cardIdToTarget);
                }
            }
        }
    }
}