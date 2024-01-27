using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public readonly struct ActivationCosts
{

}

public class Card : MonoBehaviour
{
    int health = 0;
    int attack = 0;
    string type = "";
    int weighting = 0;
    string activationCost = "";
    List<string> effects = new List<string>();

    public Card(int _health, int _attack, string _type, int _weighting, string _activationCost, List<string> _effects)
    {
        health = _health;
        attack = _attack;
        type = _type;
        weighting = _weighting;
        activationCost = _activationCost;
        effects = _effects;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
