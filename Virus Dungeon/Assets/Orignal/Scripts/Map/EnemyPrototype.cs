using UnityEngine;

[CreateAssetMenu]
public class EnemyPrototype : ScriptableObject
{
    public EnemyIdentifier prefab;
    public int hp;
    public int attack;
    public float speed;
    //public int gold;
}