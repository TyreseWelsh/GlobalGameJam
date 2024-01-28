using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardVisualHandler : MonoBehaviour
{
    Card cardData;

    [SerializeField] TextMeshProUGUI cardName;
    [SerializeField] TextMeshProUGUI cardHealth;
    [SerializeField] TextMeshProUGUI cardAttack;
    [SerializeField] TextMeshProUGUI cardWeight;
    [SerializeField] TextMeshProUGUI cardEffect;
    int cardId;
    int cardOwner;

    public void Init(Card _cardData)
    {
        print("Initialising card data...");
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
