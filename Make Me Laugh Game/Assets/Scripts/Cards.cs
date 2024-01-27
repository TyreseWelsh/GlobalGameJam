using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class is to be used as an object to store data taken from a .json file
// When I read from a json file, the contents are assigned to a string, and using the JsonUtility.FromJson<T> function, the variables in the .json file are assigned to variables of the same name here
[System.Serializable]
public class Cards
{
    public string deckname;
    public Card[] cards;
}
