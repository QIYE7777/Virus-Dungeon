using UnityEngine;
using System.Collections.Generic;
using RoguelikeCombat;

public class BuildAmmoUi : MonoBehaviour
{
    public static PlayerAmmunitionBehaviour PAB;

    private void Start()
    {
        PAB.ammos = new List<AmmoUiBehaviour>();
        for (var i = 0; i < PAB.maxAmmo; i++)
        {
            var newAmmo = Instantiate(PAB.ammoPrefab, PAB.ammoParent);
            newAmmo.SetToFilled();
            newAmmo.reloadAnimTime = PAB.reloadTime;
            PAB.ammos.Add(newAmmo);
        }


    }
}
