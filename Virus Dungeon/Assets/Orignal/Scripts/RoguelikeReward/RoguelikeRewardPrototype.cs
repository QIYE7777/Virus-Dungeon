using UnityEngine;
using System;

[CreateAssetMenu]
[Serializable]
public class RoguelikeRewardPrototype : ScriptableObject
{
    public RoguelikeUpgradeId id;
    public string title;
    [Multiline]
    public string desc;
    public Sprite sp;

    public RoguelikeUpgradeId dependency;

}

//枚举类型
public enum RoguelikeUpgradeId
{
    None = 0,
    Leech_5 = 1,
    Leech_10 = 2,
  
    MultiShoot_add2shoot_30degree = 3,

    Leech_80 = 4,

    SlowDown = 10,

    Blink = 20,
    Blink_damage_1 = 21,
    Blink_damage_2 = 22,
    Blink_CD_1 = 23,
    Blink_CD_2 = 24,

    Bullet_5 =30,
    Bullet_10 = 31,
    Bullet_00 = 32,

    ShockWave = 40,


    HpMax_1 = 50,
    HpMax_2 = 51,   
    HpMax_3 = 52,
    HpMax_4 = 53,

    Damage_1 = 60,
    Damage_2 = 61,
    Damage_3 = 62,

}