using UnityEngine;

public class BoltBehaviour : MonoBehaviour
{
    PlayerHealth playerHealth;

    float _currentSpeed;
    public float acc;
    public float maxSpeed;
    public int damage = 10;
    Vector3 _dir;
    public GameObject trail;
    public GameObject explode;
    bool _released;

    public void Release()
    {
        trail.SetActive(true);
        _released = true;
        var player = PlayerMovement.instance.transform;
        var targetPosition = player.position + Vector3.up * 0.5f;
        _dir = (targetPosition - transform.position).normalized;

        playerHealth = player.GetComponent<PlayerHealth>();
        _currentSpeed = 0;
    }

    private void Update()
    {
        Accelerate();
        MoveToPlayer();
    }

    void Accelerate()
    {
        _currentSpeed += com.GameTime.deltaTime * acc;
        if (_currentSpeed > maxSpeed)
        {
            _currentSpeed = maxSpeed;
        }
    }

    void MoveToPlayer()
    {
        transform.position += _dir * _currentSpeed * com.GameTime.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_released)
            return;
        //Debug.Log(other.gameObject);
        if (other.transform.GetComponent<BoltBehaviour>())
            return;
        if (other.transform.GetComponent<EnemyMovement>())
            return;

        if (other.transform == PlayerMovement.instance.transform)
        {
            //hit player!
            playerHealth.TakeDamage(damage);
        }

        var exp = Instantiate(explode, transform.position, Quaternion.identity, transform.parent);
        Destroy(exp, 2);
        Destroy(gameObject);
    }
}
