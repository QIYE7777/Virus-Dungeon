using UnityEngine;

[CreateAssetMenu]
public class ShopConfig : ScriptableObject
{
    public int price_addSlot;
    public int price_rest;
    public int price_refresh;

    public ShopTier tier;
}
