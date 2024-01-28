using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardVisualHandler : MonoBehaviour
{
    Card cardData;

    public TextMeshProUGUI cardName;
    public TextMeshProUGUI cardHealth;
    public TextMeshProUGUI cardAttack;
    public TextMeshProUGUI cardWeight;
    public TextMeshProUGUI cardEffect;
    int cardId;
    int cardOwner;

    public void UpdateVisuals (Card _cardData)
    {
        cardName.text = _cardData.cardname;
        if (_cardData.health < 0)
        {
            cardHealth.text = " ";
        }
        else
        {
            cardHealth.text = _cardData.health.ToString();
        }
        if (_cardData.attack < 0)
        {
            cardAttack.text = " ";
        }
        else
        {
            cardAttack.text = _cardData.attack.ToString();
        }
        cardWeight.text = _cardData.weighting.ToString();
        cardEffect.text = "PlaceHolder Effect";

        cardId = _cardData.cardid;
        cardOwner = _cardData.owner;
    }

    private void Awake()
    {

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
