using UnityEngine;

public class DeathExplosion : MonoBehaviour
{
    public int damage;

    bool _hasDamaged = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!_hasDamaged && other.transform == PlayerMovement.instance.transform)
        {
            _hasDamaged = true;
            PlayerMovement.instance.transform.GetComponent<PlayerHealth>().TakeDamage(damage);
        }
    }
}
