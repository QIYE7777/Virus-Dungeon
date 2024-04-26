using RoguelikeCombat;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : PlayerComponent
{
    public float damagePerShot = 20;
    public float timeBetweenBullets = 0.05f;
    public float range = 100f;

    public Hemophagia hemophagia { get; private set; }
    public PlayerFreeze freeze;

    float timer;
    RaycastHit shootHit;
    public int shootableMask;
    ParticleSystem gunParticles;
    public LineRenderer gunLinePrefab;
    AudioSource gunAudio;
    Light gunLight;
    public Light faceLight;

    float effectsDisplayTime = 0.2f;

    public AudioSource cannotShoot;

    private void Start()
    {
        gunParticles = GetComponent<ParticleSystem>();
        gunAudio = GetComponent<AudioSource>();
        gunLight = GetComponent<Light>();
        hemophagia = host.GetComponent<Hemophagia>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (com.GameTime.timeScale == 0)
            return;

        timer += com.GameTime.deltaTime;
        var ammunitionBehaviour = PlayerAmmunitionBehaviour.instance;

        if (Input.GetButton("R") && timer >= timeBetweenBullets && com.GameTime.timeScale != 0)
        {
            ammunitionBehaviour.Reload();
        }
        

            if (Input.GetButton("Fire1") && timer >= timeBetweenBullets && com.GameTime.timeScale != 0)
        {
            if (!ammunitionBehaviour.IsReloading())
            {
                Shoot();
                ammunitionBehaviour.OnFired();
            }
            else
            {
                //cannotShoot.Play();
                //点击射击但是正在换弹夹，什么都不做
                //播放一个不能设计的音效
            }
        }

        if (timer >= timeBetweenBullets * effectsDisplayTime)
            DisableEffects();
    }

    public void DisableEffects()
    {
        faceLight.enabled = false;
        gunLight.enabled = false;
    }

    void Shoot()
    {
        if (RoguelikeRewardSystem.instance.HasPerk(RoguelikeUpgradeId.Damage_1))
        {
            damagePerShot = 45;
        }

        if (RoguelikeRewardSystem.instance.HasPerk(RoguelikeUpgradeId.Damage_2))
        {
            damagePerShot = 60;
        }

        if (RoguelikeRewardSystem.instance.HasPerk(RoguelikeUpgradeId.Damage_3))
        {
            damagePerShot = 100;
        }

        timer = 0f;
        gunAudio.Play();
        gunLight.enabled = true;
        faceLight.enabled = true;

        gunParticles.Stop();
        gunParticles.Play();

        FireShoot(1.8f, damagePerShot, 0);
        if (RoguelikeRewardSystem.instance.HasPerk(RoguelikeUpgradeId.MultiShoot_add2shoot_30degree))
        {
            //add 2 shoot
            FireShoot(0.5f, Mathf.FloorToInt(damagePerShot * 0.4f), 12);
            FireShoot(0.5f, Mathf.FloorToInt(damagePerShot * 0.4f), -12);
        }
    }


    void FireShoot(float widthMultiplier, float damage, float angleOffset = 0)
    {
        damage = damagePerShot;
        var gunLine = Instantiate(gunLinePrefab, gunLinePrefab.transform.parent);
        gunLine.gameObject.SetActive(true);
        gunLine.transform.localPosition = Vector3.zero;
        gunLine.SetPosition(0, Vector3.zero);
        gunLine.widthMultiplier = widthMultiplier;
        Destroy(gunLine.gameObject, timeBetweenBullets - 0.07f);

        Ray shootRay = new Ray();
        shootRay.origin = transform.position;
        Vector3 relativeDirection = Vector3.forward;

        if (angleOffset == 0)
        {
            shootRay.direction = transform.forward;
        }
        else if (angleOffset > 0)
        {
            shootRay.direction = Vector3.RotateTowards(transform.forward, transform.right, angleOffset * Mathf.Deg2Rad, float.MaxValue);
            relativeDirection = Vector3.RotateTowards(Vector3.forward, Vector3.right, angleOffset * Mathf.Deg2Rad, float.MaxValue);
        }
        else if (angleOffset < 0)
        {
            shootRay.direction = Vector3.RotateTowards(transform.forward, -transform.right, -angleOffset * Mathf.Deg2Rad, float.MaxValue);
            relativeDirection = Vector3.RotateTowards(Vector3.forward, -Vector3.right, -angleOffset * Mathf.Deg2Rad, float.MaxValue);
        }

        if (Physics.Raycast(shootRay, out shootHit, range, 1 << shootableMask))
        {
            EnemyHealth enemyHealth = shootHit.collider.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
                hemophagia.LifeSteal();
                if (RoguelikeRewardSystem.instance.HasPerk(RoguelikeUpgradeId.SlowDown))
                {
                    var move = shootHit.collider.GetComponent<EnemyMovement>();
                    move.SlowDown(freeze.slowDown, freeze.duration);
                }
            }

            gunLine.SetPosition(1, relativeDirection * (shootHit.point - shootRay.origin).magnitude);
        }
        else
        {
            gunLine.SetPosition(1, relativeDirection * range);
        }
    }
}
