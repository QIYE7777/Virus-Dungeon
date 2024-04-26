
[System.Serializable]
public class HitSpecialEffectData
{
    public enum HitSpecialEffectType
    {
        None = 0,
        Poison = 1,
        Slow = 2,
    }

    public HitSpecialEffectType effectType;
    public float duration;
}
