using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

[RequireComponent(typeof(Image))]
public class HandCardAreaBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static HandCardAreaBehaviour instance;

    CardBehaviour _tpCard;

    public RectTransform cardParent;

    public int maxSlots = 7;

    List<CardBehaviour> _handCards = new List<CardBehaviour>();

    public CanvasGroup glowCg;

    private void Awake()
    {
        instance = this;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("OnPointerEnter");
        //Debug.Log(eventData.pointerDrag);
        //Debug.Log(eventData.dragging);
        if (eventData.dragging && eventData.pointerDrag != null)
        {
            var c = eventData.pointerDrag.GetComponent<CardBehaviour>();
            if (c != null)
                _tpCard = c;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("OnPointerExit");
        _tpCard = null;
    }

    public void ValidDrop(CardBehaviour c)
    {
        if (c == _tpCard)
        {
            if (InventorySystem.instance.TryBuy(_tpCard))
            {
                Debug.Log("buy suc");
            }
            else
            {
                Debug.Log("buy fail");
            }
        }


        _tpCard = null;
    }

    public void AddCard(CardBehaviour c)
    {
        c.transform.SetParent(cardParent);
        _handCards.Add(c);
    }

    public void RemoveCard(CardBehaviour c)
    {
        _handCards.Remove(c);
    }

    public int CurrentCardNum { get { return _handCards.Count; } }

    public bool HasTriple(CardBehaviour c)
    {
        return HasTriple(c.cfg.id);
    }

    public bool HasTriple(string id)
    {
        int exist = 0;
        foreach (var c in _handCards)
        {
            if (c.cfg.id == id)
            {
                exist++;
                if (exist == 2)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void ToggleCanDrop(bool b)
    {
        glowCg.DOKill();
        if (b)
        {
            glowCg.DOFade(1, 0.35f);
        }
        else
        {
            glowCg.DOFade(0, 0.35f);
        }
    }
}
