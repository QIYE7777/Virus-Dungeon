using UnityEngine;

public class ShopSystem : MonoBehaviour
{
    public static CardTier GetCardTierByShopTier(ShopTier shopTier)
    {
        float randomValue = Random.value;
        switch (shopTier)
        {
            case ShopTier.ShopLevel1:
                return CardTier.CardLevel1;

            case ShopTier.ShopLevel2:
                if (randomValue > 0.5f)
                {
                    return CardTier.CardLevel2;
                }
                else
                {
                    return CardTier.CardLevel1;
                }

            case ShopTier.ShopLevel3:
                if (randomValue > 0.7f)
                {
                    return CardTier.CardLevel3;
                }
                else if (randomValue > 0.4f)
                {
                    return CardTier.CardLevel2;
                }
                else
                {
                    return CardTier.CardLevel1;
                }
        }

        return CardTier.CardLevel1;
    }

}
