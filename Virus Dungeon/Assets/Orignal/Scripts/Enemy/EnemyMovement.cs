using UnityEngine.AI;
using UnityEngine;
using com;

public class EnemyMovement : MonoBehaviour
{
    NavMeshAgent nav;
    EnemyIdentifier id;
    public float dec = 100;
    public float _knockSpeed;
    Vector3 _knockDir;
    CharacterController cc;

    float _resumeSlowDownTimestamp;
    bool _isSlowed;

    private void Awake()
    {
        id = GetComponent<EnemyIdentifier>();
        nav = GetComponent<NavMeshAgent>();
        cc = GetComponent<CharacterController>();
    }

    public void SetSpeed(float s)
    {
        nav.speed = s;
    }

    public void SlowDown(float slowDownRatio, float duration)
    {
        nav.speed = id.proto.speed * (1 - slowDownRatio);
        _isSlowed = true;
        _resumeSlowDownTimestamp = GameTime.time + duration;
    }

    private void Start()
    {
        id.anim.SetBool("walk", true);
    }

    void Update()
    {
        if (com.GameTime.timeScale == 0)
        {
            nav.enabled = false;
            return;
        }

        if (!nav.enabled)
            nav.enabled = true;

        if (_knockSpeed > 0)
            CheckKnockback();
        else
            Walk();

        if (_isSlowed && com.GameTime.time > _resumeSlowDownTimestamp)
        {
            _isSlowed = false;
            nav.speed = id.proto.speed;
        }
    }

    void CheckKnockback()
    {
        _knockSpeed -= com.GameTime.deltaTime * dec;
        if (_knockSpeed <= 0)
        {
            //switch to walk
            nav.enabled = true;
            return;
        }
        if (cc.enabled)
            cc.SimpleMove(_knockDir * _knockSpeed);
    }

    void Walk()
    {
        var player = PlayerBehaviour.instance;
        var playerHealth = player.health;
        if (nav.enabled && id.health.hp > 0 && playerHealth.currentHealth > 0)
            nav.SetDestination(player.transform.position);
        else
            nav.enabled = false;
    }

    public void Knockback(float speed, Vector3 origin)
    {
        var dir = transform.position - origin;
        dir.y = 0;
        _knockDir = dir.normalized;
        _knockSpeed = speed;
        nav.enabled = false;
    }
}