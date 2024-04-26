using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Image))]
public class SellCardAreaBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static SellCardAreaBehaviour instance;

    public GameObject soldPsPrefab;

    CardBehaviour _tpCard;

    public CanvasGroup glowCg;

    private void Awake()
    {
        instance = this;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("OnPointerEnter");
        Debug.Log(eventData.pointerDrag);
        Debug.Log(eventData.dragging);
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
        //Debug.Log("ValidDrop");
        if (c == _tpCard)
        {
            CardSold(c);
        }


        _tpCard = null;
    }

    public void CardSold(CardBehaviour c)
    {
        Debug.Log("CardSold");
        HandCardAreaBehaviour.instance.RemoveCard(c);
        var ps = Instantiate(soldPsPrefab, c.icon.transform.position, Quaternion.identity, soldPsPrefab.transform.parent);//should auto destory
        ps.gameObject.SetActive(true);
        Destroy(ps.gameObject, 2.5f);

        InventorySystem.instance.ModifyMoney((int)(c.cfg.price * 0.5f));
        Destroy(c.gameObject);
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
