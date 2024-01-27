using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    private Image image;
    private Transform optionsPanel;

    private void Awake()
    {
        optionsPanel = this.gameObject.transform.GetChild(0);
        image = GetComponent<Image>();
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


}
