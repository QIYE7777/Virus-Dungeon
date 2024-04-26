using UnityEngine;
using RoguelikeCombat;

public class Hemophagia : MonoBehaviour
{
    PlayerHealth playerHealth;
    public GameObject gunShoot;
    PlayerShooting playerShooting;

    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
        playerShooting = gunShoot.GetComponent<PlayerShooting>();
    }

    public void LifeSteal()
    {
        float healPerShoot = 0;

        if (RoguelikeRewardSystem.instance.HasPerk(RoguelikeUpgradeId.Leech_80))
        {
            healPerShoot =  playerShooting.damagePerShot/10 ;//aww10%
        }

        else if (RoguelikeRewardSystem.instance.HasPerk(RoguelikeUpgradeId.Leech_10))
        {
            healPerShoot = playerShooting.damagePerShot / 20;//5%
        }
        else if (RoguelikeRewardSystem.instance.HasPerk(RoguelikeUpgradeId.Leech_5))
        {
            healPerShoot = playerShooting.damagePerShot / 50;//2%
        }


        if (healPerShoot > 0)
        {
            playerHealth.Heal(healPerShoot);
            Debug.Log(healPerShoot);
        }
    }
}