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
        print("Updating Card Visuals...");
        cardName.text = _cardData.cardname;
        cardHealth.text = _cardData.health.ToString();
        cardAttack.text = _cardData.attack.ToString();
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
