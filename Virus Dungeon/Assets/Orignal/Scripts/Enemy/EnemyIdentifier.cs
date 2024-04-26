using UnityEngine;
using System.Collections.Generic;

public class EnemyIdentifier : MonoBehaviour
{
    public static List<EnemyIdentifier> enemies = new List<EnemyIdentifier>();

    public Animator anim;
    [HideInInspector]
    public EnemyHealth health;
    [HideInInspector]
    public EnemyAttack attack;
    [HideInInspector]
    public EnemyMovement move;
    public EnemyPrototype proto;

    private void Awake()
    {
        health = GetComponent<EnemyHealth>();
        move = GetComponent<EnemyMovement>();
        attack = GetComponent<EnemyAttack>();
    }

    private void OnEnable()
    {
        enemies.Add(this);
    }

    private void OnDestroy()
    {
        enemies.Remove(this);
    }

    public static void ClearEnemyList()
    {
        enemies = new List<EnemyIdentifier>();
    }
    public static bool NoEnemyExist()
    {
        //Debug.Log(enemies.Count);
        return enemies.Count == 0;
    }

    public void InitializeEnemyPrototype(EnemyPrototype p)
    {
        proto = p;
        health.ResetHp(p.hp);
        attack.attackDamage = proto.attack;
        move.SetSpeed(p.speed);
    }
}
