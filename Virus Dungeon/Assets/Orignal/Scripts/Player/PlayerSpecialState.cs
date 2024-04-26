using UnityEngine;
using System.Collections;

public class PlayerSpecialState : Ticker
{
    public int restTicks;
    public float slowRatio;
    public int  poisonDot;

    public void ApplyHitSpecialEffect(HitSpecialEffectData hitSpecialEffect)
    {
        switch (hitSpecialEffect.effectType)
        {
            case HitSpecialEffectData.HitSpecialEffectType.Poison:
                break;

            case HitSpecialEffectData.HitSpecialEffectType.Slow:
                break;
        }
    }

    protected override void Tick()
    {
    }
}
