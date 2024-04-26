using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public HitSpecialEffectData hitSpecialEffect;
    public float timeBetweenAttacks = 0.5f;
    public int attackDamage = 10;
    public float attackPrepareTime = 0.25f;
    public float attackPosFrontDistance = 0.6f;
    public float attackRange =0.9f;

    bool playerInRange
    {
        get
        {
            var attackPos = transform.position + transform.forward * attackPosFrontDistance;
            var dist = PlayerBehaviour.instance.transform.position - attackPos;
            dist.y = 0;
            return dist.magnitude < attackRange;
        }
    }

    float timer;
    bool _canValidateAttack;
    public SlowSpeed slowspeed;

    EnemyIdentifier id;

    public bool canPlayAttackAnim = true;

    private void Awake()
    {
        id = GetComponent<EnemyIdentifier>();
    }

    // Update is called once per frame
    void Update()
    {
        if (com.GameTime.timeScale == 0)
            return;

        timer += com.GameTime.deltaTime;

        if (timer >= timeBetweenAttacks && playerInRange && id.health.hp > 0)
        {
            Attack();
            if (canPlayAttackAnim)
                id.anim.SetTrigger("attack");
        }

        var player = PlayerBehaviour.instance;
        if (player.health.currentHealth <= 0)
            id.anim.SetBool("walk", false);
    }

    void Attack()
    {
        timer = 0f;
        _canValidateAttack = true;
        Invoke("OnAttacked", attackPrepareTime);
    }

    void OnAttacked()
    {
        if (!_canValidateAttack)
            return;
        if (!playerInRange)
            return;
        _canValidateAttack = false;

        var player = PlayerBehaviour.instance;
        player.health.TakeDamage(attackDamage);
        if (slowspeed != null)
            slowspeed.Slow(player);

        if (hitSpecialEffect != null && hitSpecialEffect.effectType != HitSpecialEffectData.HitSpecialEffectType.None)
            player.specialState.ApplyHitSpecialEffect(hitSpecialEffect);
    }
}