using UnityEngine;
using System.Collections.Generic;

public class CardSellSystem : MonoBehaviour
{
    public static CardSellSystem instance;

    public RectTransform rectSell;
    public CardBehaviour cardPrefab;

    public List<CardBehaviour> cardsToSell { get; private set; }


    public CardConfig testCard1;
    public CardConfig testCard2;
    public GameObject IconPrefab;

    private void Awake()
    {
        instance = this;
        cardsToSell = new List<CardBehaviour>();
        Clear();
    }

    private void Start()
    {
        StartSell();
    }

    public void StartSell()
    {
        RestockAll();
    }

    public void RestockAll()
    {
        Clear();

        var count = GetStockCount();
        for (int i = 0; i < count; i++)
            AddSellCard();
    }

    public int GetStockCount()
    {
        return 6;
    }

    public CardConfig GetRandomCardConfig()
    {
        float r = Random.value;
        if (r > 0.5f)
        {
            return testCard1;
        }

        return testCard2;
    }

    void AddSellCard()
    {
        AddSellCard(GetRandomCardConfig());
    }

    void AddSellCard(CardConfig cfg)
    {
        var c = Instantiate(cardPrefab, cardPrefab.transform.parent);
        c.gameObject.SetActive(true);
        c.cfg = cfg;
        c.Init();
        c.cardState = CardState.InSell;
        cardsToSell.Add(c);
    }

    public void RemoveSellCard(CardBehaviour c)
    {
        cardsToSell.Remove(c);
    }

    public void Clear()
    {
        foreach (var c in cardsToSell)
        {
            Destroy(c.gameObject);
        }

        cardsToSell = new List<CardBehaviour>();
    }

    public void OnClickRefresh()
    {
        var price = GetRefreshPrice();
        if (InventorySystem.instance.Affordable(price))
        {
            InventorySystem.instance.ModifyMoney(-price);
            RestockAll();
        }
    }

    public int GetRefreshPrice()
    {
        return 1;
    }
}
