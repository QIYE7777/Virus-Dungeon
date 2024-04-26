using UnityEngine;
using System.Collections.Generic;
using RoguelikeCombat;

public class PlayerAmmunitionBehaviour : MonoBehaviour
{
    public static PlayerAmmunitionBehaviour instance;
    public int defaultMaxAmmo = 3;

    [HideInInspector]
    public int maxAmmo;

    public AmmoUiBehaviour ammoPrefab;
    public RectTransform ammoParent;

    public float reloadTime = 2;

    float _reloadDoneTimestamp;//这个数值小于等于0 表示不在换弹，否则表示换弹结束的时间点

    [HideInInspector]
    public int currentAmmoCount;
    [HideInInspector]
    public List<AmmoUiBehaviour> ammos = new List<AmmoUiBehaviour>();

    public AudioSource reload;

    private void Awake()
    {
        instance = this;
        _reloadDoneTimestamp = 0;
        currentAmmoCount = maxAmmo;
    }

    private void Start()
    {
        CheckAmmunitionState();
    }

    void BuildAmmoUi()
    {
        ammos = new List<AmmoUiBehaviour>();
        for (var i = 0; i < maxAmmo; i++)
        {
            var newAmmo = Instantiate(ammoPrefab, ammoParent);
            newAmmo.SetToFilled();
            newAmmo.reloadAnimTime = reloadTime;
            ammos.Add(newAmmo);
        }
    }

    void ClearAmmoUi()
    {
        foreach (var i in ammos)
        {
            Destroy(i.gameObject);
        }
        ammos = new List<AmmoUiBehaviour>();
    }

    void SetMaxAmmo()
    {
        maxAmmo = defaultMaxAmmo;
        if (RoguelikeRewardSystem.instance.HasPerk(RoguelikeUpgradeId.Bullet_00))
            maxAmmo = 1;
        else if (RoguelikeRewardSystem.instance.HasPerk(RoguelikeUpgradeId.Bullet_10))
            maxAmmo = 7;
        else if (RoguelikeRewardSystem.instance.HasPerk(RoguelikeUpgradeId.Bullet_5))
            maxAmmo = 5;
    }

    public void CheckAmmunitionState()
    {
        SetMaxAmmo();
        ClearAmmoUi();
        BuildAmmoUi();
        currentAmmoCount = maxAmmo;
    }

    public bool IsReloading()
    {
        if (_reloadDoneTimestamp <= 0)
        {
            return false;
        }

        if (Time.time > _reloadDoneTimestamp)
        {
            return false;
        }

        return true;
    }

    public void OnFired()
    {

        currentAmmoCount -= 1;
        for (var i = 0; i < maxAmmo; i++)
        {
            if (i + 1 <= currentAmmoCount)
            {
                ammos[i].SetToFilled();
            }
            else
            {
                ammos[i].SetToEmpty();
            }
        }

        if (currentAmmoCount <= 0)
        {
            Reload();
        }
    }

    public void Reload()
    {
        var t = reloadTime;
        if (RoguelikeRewardSystem.instance.HasPerk(RoguelikeUpgradeId.Bullet_00))
            t = 0;


        _reloadDoneTimestamp = Time.time + t;
        currentAmmoCount = maxAmmo;
        if (!RoguelikeRewardSystem.instance.HasPerk(RoguelikeUpgradeId.Bullet_00))
            reload.Play();

        for (var i = 0; i < maxAmmo; i++)
            ammos[i].ReloadAnim();
    }
}