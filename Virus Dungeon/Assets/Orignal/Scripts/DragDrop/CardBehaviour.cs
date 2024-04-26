using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum CardState
{
    None = 0,
    InHand = 1,
    InSell = 2,
    Discarded = 3,
}

[RequireComponent(typeof(Image))]
public class CardBehaviour : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject icon { get; private set; }
    private RectTransform m_DraggingPlane;

    public CardConfig cfg;
    CardViewBehaviour _view;
    public CardState cardState;
    public bool isGolden { get; private set; }

    private void Awake()
    {
        _view = GetComponent<CardViewBehaviour>();
    }

    public void Init()
    {
        _view.Setup(cfg);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (cardState == CardState.None)
            return;
        if (cardState == CardState.Discarded)
            return;

        var prefab = CardSellSystem.instance.IconPrefab;
        icon = Instantiate(prefab, prefab.transform.parent);
        icon.SetActive(true);
        //m_DraggingIcon.transform.SetAsLastSibling();

        var iconImg = icon.GetComponent<Image>();
        iconImg.raycastTarget = false;
        iconImg.sprite = cfg.sp;
        iconImg.SetNativeSize();

        _view.SetTransparent();

        m_DraggingPlane = transform as RectTransform;
        SetDraggedPosition(eventData);

        _view.OnStartDrag();
        if (cardState == CardState.InSell)
            HandCardAreaBehaviour.instance.ToggleCanDrop(true);

        if (cardState == CardState.InHand)
            SellCardAreaBehaviour.instance.ToggleCanDrop(true);
    }

    public void OnDrag(PointerEventData data)
    {
        if (icon != null)
            SetDraggedPosition(data);
    }

    private void SetDraggedPosition(PointerEventData data)
    {
        if (data.pointerEnter != null && data.pointerEnter.transform as RectTransform != null)
            m_DraggingPlane = data.pointerEnter.transform as RectTransform;

        var rt = icon.GetComponent<RectTransform>();
        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(m_DraggingPlane, data.position, data.pressEventCamera, out globalMousePos))
        {
            rt.position = globalMousePos;
            rt.rotation = m_DraggingPlane.rotation;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("OnEndDrag");
        if (icon != null)
            Destroy(icon);

        _view.SetNonTransparent();
        if (cardState == CardState.InSell)
            HandCardAreaBehaviour.instance.ValidDrop(this);

        if (cardState == CardState.InHand)
            SellCardAreaBehaviour.instance.ValidDrop(this);

        SellCardAreaBehaviour.instance.ToggleCanDrop(false);
        HandCardAreaBehaviour.instance.ToggleCanDrop(false);
        _view.OnStopDrag();
    }
}