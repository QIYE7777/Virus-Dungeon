using UnityEngine;

public class PoisonCloud : MonoBehaviour
{
    bool isPlayerInRange;
    public float poisonAttackRate = 0.2f;
    public int damage;
    private float poisonAttackTimestamp;

    public PlayerBlink playerBlink;

    private void Awake()
    {
        isPlayerInRange = false;
    }

    private void Start()
    {
        poisonAttackTimestamp = com.GameTime.time;
    }

    private void Update()
    {
        if (com.GameTime.time > poisonAttackTimestamp)
        {
            poisonAttackTimestamp += poisonAttackRate;
            if (isPlayerInRange)
                PlayerMovement.instance.transform.GetComponent<PlayerHealth>().TakeDamage(damage);
        }
    }

    Collider player;

    public void CheckIsPlayerInRange()
    {

    }

    /*private void OnCollisionStay(Collision  collision)
    {
        if (collision.transform.GetComponent<BoltBehaviour>())
            return;
        if (collision.transform.GetComponent<EnemyMovement>())
            return;

        if (collision.transform == PlayerMovement.instance.transform)
        {
            isPlayerInRange = true;
        }
        else
        {
            isPlayerInRange = false;
        }
    }*/


    private void OnTriggerEnter(Collider other)
    {
        
        if (other.transform.GetComponent<BoltBehaviour>())
            return;
        if (other.transform.GetComponent<EnemyMovement>())
            return;

        if (other.transform == PlayerMovement.instance.transform)
        {
            player = other;
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.GetComponent<BoltBehaviour>())
            return;
        if (other.transform.GetComponent<EnemyMovement>())
            return;

        if (other.transform == PlayerMovement.instance.transform)
        {
            isPlayerInRange = false;
        }
    }
}
