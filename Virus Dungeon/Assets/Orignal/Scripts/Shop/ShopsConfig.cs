using UnityEngine;

[CreateAssetMenu]
public class ShopsConfig : ScriptableObject
{
    public float globalPriceModifier = 1f;

    public float[] addSlotPriceModifier;

    public float restRecoverHpPercent = 0.2f;
}
