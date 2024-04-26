using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using RoguelikeCombat;

public class PlayerShootSuper : PlayerComponent
{
    public int damagePerShot = 20;
    public float timeBetweenBullets = 0.15f;
    public float range = 100f;

    float timer;
    public int shootableMask;
    ParticleSystem gunParticles;
    AudioSource gunAudio;
    Light gunLight;
    public Light faceLight;

    float effectsDisplayTime = 0.2f;
    public GameObject heatWavePlane;
    public float maxDistortion = 512;
    public float minDistortion = 0;
    Material _distortionMaterial;

    private void Awake()
    {
        gunParticles = GetComponent<ParticleSystem>();
        gunAudio = GetComponent<AudioSource>();
        gunLight = GetComponent<Light>();
        heatWavePlane.SetActive(false);
        _distortionMaterial = heatWavePlane.GetComponentInChildren<MeshRenderer>().material;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (com.GameTime.timeScale == 0)
            return;

        timer += com.GameTime.deltaTime;

        if (RoguelikeRewardSystem.instance.HasPerk(RoguelikeUpgradeId.ShockWave ))
            { 
            if (Input.GetButton("Fire2") && timer >= timeBetweenBullets && Time.timeScale != 0)
            Shoot();
            if (timer >= timeBetweenBullets * effectsDisplayTime)
            DisableEffects();
            }
    }

    public void DisableEffects()
    {
        faceLight.enabled = false;
        gunLight.enabled = false;
    }

    void PlayHeatWave()
    {
        heatWavePlane.transform.localScale = Vector3.one * 0.5f;
        var camPos = Camera.main.transform.position;
        var heatWavePos = heatWavePlane.transform.position;
        heatWavePlane.transform.localPosition = Vector3.zero;
        heatWavePlane.transform.position += (camPos - heatWavePos).normalized * 1f;
        _distortionMaterial.SetFloat("_BumpAmt", maxDistortion);
        //_distortionMaterial.DOKill();
        _distortionMaterial.DOFloat(minDistortion, "_BumpAmt", 0.45f).SetEase(Ease.OutCubic);

        heatWavePlane.SetActive(true);
        heatWavePlane.transform.DOKill();
        heatWavePlane.transform.DOScale(1.2f, 0.45f).OnComplete(
            () => { heatWavePlane.SetActive(false); });
    }

    void Shoot()
    {
        timer = 0f;
        gunAudio.Play();
        gunLight.enabled = true;
        faceLight.enabled = true;

        gunParticles.Stop();
        gunParticles.Play();

        PlayHeatWave();
        var allTargets = new List<EnemyIdentifier>();
        var shootResult = Physics.RaycastAll(transform.position, transform.forward, range, 1 << shootableMask);
        foreach (var res in shootResult)
        {
            EnemyIdentifier e = res.collider.GetComponent<EnemyIdentifier>();
            if (e != null)
                allTargets.Add(e);
        }

        foreach (var e in EnemyIdentifier.enemies)
        {
            if (e == null)
                continue;

            var dist = (transform.position - e.transform.position).magnitude;
            if (dist < 2.0f)
            {
                bool duplicated = false;
                foreach (var res in shootResult)
                {
                    EnemyIdentifier shootResEnemy = res.collider.GetComponent<EnemyIdentifier>();
                    if (shootResEnemy == e)
                    {
                        duplicated = true;
                    }
                }

                if (!duplicated)
                {
                    allTargets.Add(e);
                }
            }
        }

        foreach (var e in allTargets)
        {
            e.health.TakeDamage(damagePerShot);
            e.move.Knockback(75, transform.position);
        }
    }
}
