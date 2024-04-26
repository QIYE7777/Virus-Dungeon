using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem instance;
    //public Canvas canvas;
    public int gold { get; private set; }

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        gold = 0;
        ModifyMoney(100);
    }

    public bool Affordable(int price)
    {
        return gold >= price;
    }

    public void ModifyMoney(int amount)
    {
        gold += amount;
        InventoryViewBehaviour.instance.Sync();
    }

    public bool TryBuy(CardBehaviour card, bool buy = true)
    {
        var price = card.cfg.price;
        if (Affordable(price))
        {
            if (HasEmptySlot(card))
            {
                if (buy)
                {
                    ModifyMoney(-price);
                    CommitBuyCard(card);
                }
                return true;
            }
            else
            {
                Debug.Log("no slot");
            }
        }
        else
        {
            Debug.Log("no gold");
        }

        return false;
    }


    public void CommitBuyCard(CardBehaviour card)
    {
        HandCardAreaBehaviour.instance.AddCard(card);
     
        CardSellSystem.instance.RemoveSellCard(card);
        card.cardState = CardState.InHand;

        //TODO triple
    }

    public bool HasEmptySlot(CardBehaviour toAddCard = null)
    {
        var handCard = HandCardAreaBehaviour.instance;
        var rest = handCard.maxSlots - handCard.CurrentCardNum;
        if (toAddCard != null && HandCardAreaBehaviour.instance.HasTriple(toAddCard))
        {
            rest += 1;
        }
        return rest > 0;
    }
}
