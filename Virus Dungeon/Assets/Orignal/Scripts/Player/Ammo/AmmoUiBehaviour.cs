using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class AmmoUiBehaviour : MonoBehaviour
{
    public Image filledAmmo;
    [HideInInspector]
    public float reloadAnimTime;

    public void SetToEmpty()
    {
        filledAmmo.gameObject.SetActive(false);
    }

    public void SetToFilled()
    {
        filledAmmo.gameObject.SetActive(true);
    }

    public void ReloadAnim()
    {
        filledAmmo.fillAmount = 0;
        filledAmmo.gameObject.SetActive(true);

        filledAmmo.DOKill();
        filledAmmo.DOFillAmount(1, reloadAnimTime);
    }
}
