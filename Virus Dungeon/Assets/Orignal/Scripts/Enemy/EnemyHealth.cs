using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    float _hpMax;
    public float hp;
    public float sinkSpeed = 2.5f;
    public int scoreValue = 10;
    public AudioClip deathClip;

    AudioSource enemyAudio;
    public ParticleSystem hitParticles;
    public ParticleSystem dieParticles;
    bool isDead;
    bool isSinking;
    public SpawnBoltOnDeath spawnBoltOnDeath;
    public SpawnPoisonCloudOnDeath spawnPoisonCloudOnDeath;
    public ExplodeOnDeath explodeOnDeath;
    EnemyIdentifier id;
    public bool canPlayWoundAnim = true;

    public EnemyDeath eD;

    private void Awake()
    {
        id = GetComponent<EnemyIdentifier>();
        enemyAudio = GetComponent<AudioSource>();
    }

    public void ResetHp(float hp)
    {
        _hpMax = hp;
        this.hp = _hpMax;
    }
    // Update is called once per frame
    void Update()
    {
        if (isSinking)
            transform.Translate(-Vector3.up * sinkSpeed * com.GameTime.deltaTime);
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;
        hitParticles.Play();

        enemyAudio.Play();
        float damageRatio = (float)amount / (float)_hpMax;
        hp -= amount;
        if (hp <= 0)
        {
            Death();
        }
        else
        {
            if (damageRatio > 0.25f ||
                (damageRatio > 0.1f && Random.value > 0.6f) ||
                 (damageRatio > 0.01f && Random.value > 0.85f))
            {
                if (canPlayWoundAnim)
                    id.anim.SetTrigger("wound");
            }
        }
    }

    void Death()
    {
        if (isDead)
            return;
        isDead = true;

        if (spawnBoltOnDeath != null)
            spawnBoltOnDeath.Spawn();

        if (spawnPoisonCloudOnDeath != null)
            spawnPoisonCloudOnDeath.Spawn();

        if (explodeOnDeath != null)
            explodeOnDeath.Spawn();

        var cc = GetComponent<CharacterController>();
        cc.enabled = false;
        var cols = GetComponentsInChildren<Collider>();
        foreach (var col in cols)
        {
            col.enabled = false;
        }
        id.anim.SetTrigger("die");
        dieParticles.Play();
        enemyAudio.clip = deathClip;
        enemyAudio.Play();

        Invoke("RemoveDeathBody", 0.35f);


        eD.BreakDown();
    }

    public void StartSinking()
    {

    }

    public void RemoveDeathBody()
    {
        if (isSinking)
            return;

        GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
        isSinking = true;

        ScoreManager.score += scoreValue;
        EnemyIdentifier.enemies.Remove(this.id);

        StartCoroutine(OnBodyRemoved());
    }

    IEnumerator OnBodyRemoved()
    {
        yield return new WaitForSeconds(2);
        CheckLevelEnd();
        Destroy(gameObject);
    }

    void CheckLevelEnd()
    {
        //Debug.Log(!CombatManager.instance.HasEnemyLeft(true));
        // Debug.Log(RoomBehaviour.instance.IsSpawnDone());
        if (!CombatManager.instance.HasEnemyLeft(true) && RoomBehaviour.instance.IsSpawnDone())
        {
            if (RoomRewardBehaviour.instance != null && RoomRewardBehaviour.instance.gameObject != null)
            {
                Debug.LogWarning("spawn room reward but there is one already!!!");
                return;
            }

            var bodyPos = transform.position;
            bodyPos.y = 0.5f;
            //var playerPos = PlayerBehaviour.instance.transform.position;
            //var pos = bodyPos + (playerPos - bodyPos).normalized * 2;
            RoomBehaviour.instance.SpawnLevelEndReward();
        }
    }
}
