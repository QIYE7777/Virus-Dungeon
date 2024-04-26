using System.Collections.Generic;
using RoguelikeCombat;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealth : PlayerComponent
{
    public float hpMax = 600;
    public float currentHealth;

    public AudioClip deathClip;

    Animator anim;
    AudioSource playerAudio;
    PlayerMovement playerMovement;
    PlayerShooting playerShooting;
    bool isDead;
    int preventDeathCount = 1;

    bool hpMax_4 = false;
    bool hpMax_3 = false;
    bool hpMax_2 = false;
    bool hpMax_1 = false;
    public bool hpMaxOver = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        playerMovement = GetComponent<PlayerMovement>();
        playerShooting = GetComponentInChildren<PlayerShooting>();
        currentHealth = hpMax;
    }

    public void Start()
    {
        hpMax = CombatManager.instance.hpMaxInGame;

        currentHealth = hpMax - CombatManager.instance.lostHealth;
        if (currentHealth <= 0)
            currentHealth = 1;

        RefreshHpBar();
        preventDeathCount = 1;
    }

    void SetHpMax()
    {
        if (RoguelikeRewardSystem.instance.HasPerk(RoguelikeUpgradeId.HpMax_4) )
        {
            if (hpMax_4 == false)
            {
                hpMax = 210;
                hpMax_4 = true;
                currentHealth = hpMax;
                hpMaxOver = true;
                CombatManager.instance.hpMaxInGame = hpMax;
            }
        }

        else if (RoguelikeRewardSystem.instance.HasPerk(RoguelikeUpgradeId.HpMax_3))
        {
            if (hpMax_3 == false)
            {
                hpMax = 170;
                hpMax_3 = true;
                currentHealth = hpMax;
                CombatManager.instance.hpMaxInGame = hpMax;
            }
        }

        else if (RoguelikeRewardSystem.instance.HasPerk(RoguelikeUpgradeId.HpMax_2))
        {
            if (hpMax_2 == false)
            {
                hpMax = 150;
                hpMax_2 = true;
                currentHealth = hpMax;
                CombatManager.instance.hpMaxInGame = hpMax;
            }
        }

        else if (RoguelikeRewardSystem.instance.HasPerk(RoguelikeUpgradeId.HpMax_1))
        {
            if (hpMax_1 == false)
            {
                hpMax = 120;
                hpMax_1 = true;
                currentHealth = hpMax;
                CombatManager.instance.hpMaxInGame = hpMax;
            }
        }

        Debug.Log(hpMax);
    }

    public void MaxHealthUp()
    {
        SetHpMax();

        SaveHp();
        RefreshHpBar();
    }

    public void Heal(float amount)
    {
        if (isDead)
            return;

        currentHealth += amount;
        if (currentHealth > hpMax)
            currentHealth = hpMax;

        RefreshHpBar();
    }

    public void TakeDamage(int amount)
    {
        if (isDead)
            return;

        HudBehaviour.instance.OnDamaged();
        currentHealth -= amount;
        if (currentHealth <= 0 && !isDead)
        {
            if (preventDeathCount > 0 && currentHealth > -20)
            {
                currentHealth = 1;
                preventDeathCount--;
            }
        }

        if (currentHealth < 0)
            currentHealth = 0;

        RefreshHpBar();
        playerAudio.Play();

        if (currentHealth <= 0 && !isDead)
            Death();
    }

    void RefreshHpBar()
    {
        float currentHpRatio = (float)currentHealth / (float)hpMax;
        HudBehaviour.instance.SetHpBar(Mathf.Pow(currentHpRatio, 1.5f));//让最后的血条显得更耐用的一点，增加玩家残血过关的概率
    }

    void Death()
    {
        isDead = true;
        playerShooting.DisableEffects();
        anim.SetTrigger("Die");
        playerAudio.clip = deathClip;
        playerAudio.Play();

        playerMovement.enabled = false;
        playerShooting.enabled = false;


        StartCoroutine(Restart());
    }

    IEnumerator Restart()
    {
        yield return new WaitForSeconds(3);
        Destroy(DontDestroyOnLoadBh.uniqueInstance.gameObject);
        EnemyIdentifier.ClearEnemyList();
        SceneManager.LoadScene(0);
    }

    public void RestartLevel()
    {
        SceneSwitcher.instance.RestartCurrentLevel();
    }

    public void SaveHp()
    {
        CombatManager.instance.lostHealth = hpMax - currentHealth;
    }
}
