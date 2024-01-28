using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, IPointerExitHandler
{
    private Image image;
    private Transform optionsPanel;

    int owner;
    public int id;
    string cardName;
    string cardImage;
    public int health;
    public int attack;
    string cardType;
    int weighting;
    string activationCost;
    string effect;
    int effectvalue;
    string namedcard;
    public CardState currentCardState;

    CardVisualHandler cardVisuals;


    private void Awake()
    {
        optionsPanel = this.gameObject.transform.GetChild(0);
        image = GetComponent<Image>();
        cardVisuals = GetComponent<CardVisualHandler>();
    }

    public void Init(Card _card)
    {
        owner = _card.owner;
        id = _card.cardid;
        cardName = _card.cardname;
        cardImage = _card.cardimage;
        health = _card.health;
        attack = _card.attack;
        cardType = _card.type;
        weighting = _card.weighting;
        activationCost = _card.activationCost;
        effect = _card.effectname;
        effectvalue = _card.effectvalue;
        namedcard = _card.namedcard;
        currentCardState = _card.currentCardState;

        cardVisuals.UpdateVisuals(_card);
    }












    public void OnPointerClick(PointerEventData eventData)
    {
        optionsPanel.gameObject.SetActive(true);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetParent(transform.root);
        optionsPanel.gameObject.SetActive(false);
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(CardManager.LastEnteredDropZone);
        image.raycastTarget = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        optionsPanel.gameObject.SetActive(false);
    }
}
