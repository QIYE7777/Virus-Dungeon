using RoguelikeCombat;
using UnityEngine;

public class PlayerBlink : PlayerComponent
{
    public float timeBetweenBlinks = 7f;
    float timer;

    public float blinkDistance = 3f;

    Ray blinkRay = new Ray();

    public Transform gunPos;
    public int blinkableMask;

    PlayerMovement _movement;

    public bool passThoughEnemies;
    public int damageToPassThoughEnemy = 0;
    public float checkDistanceToPassThoughEnemy = 1f;

    public GameObject trait;
    public float trailDuration;
    private float _blinkTrailDisappearTimestamp;

    void Start()
    {
        _movement = GetComponent<PlayerMovement>();
        timer = 0;
    }

    void Update()
    {
        timer += com.GameTime.deltaTime;
        if (Input.GetButtonDown("Jump") && timer >= timeBetweenBlinks && com.GameTime.timeScale != 0)
            Blink();

        if (trait.activeSelf && com.GameTime.time > _blinkTrailDisappearTimestamp + 0.05)
            trait.SetActive(false);
    }

    void Blink()
    {
        if (RoguelikeRewardSystem.instance.HasPerk(RoguelikeUpgradeId.Blink))
        {
            trait.SetActive(true);
            _blinkTrailDisappearTimestamp = com.GameTime.time + trailDuration;

            timer = 0;
            var pos = GetBlinkTargetPlace();
            pos.y = 0;
            //Debug.Log(transform.position);
            //Debug.Log(pos);
            host.move.cc.enabled = false;
            transform.position = pos;
            host.move.cc.enabled = true;

            SetCD();

            if (RoguelikeRewardSystem.instance.HasPerk(RoguelikeUpgradeId.Blink_damage_1))
            {
                damageToPassThoughEnemy = 30;
            }

            if (RoguelikeRewardSystem.instance.HasPerk(RoguelikeUpgradeId.Blink_damage_2))
            {
                damageToPassThoughEnemy = 110;
            }
        }
    }

    void SetCD()
    {
        if (RoguelikeRewardSystem.instance.HasPerk(RoguelikeUpgradeId.Blink_CD_2))
        {
            timeBetweenBlinks = 1f;
        }

        else if (RoguelikeRewardSystem.instance.HasPerk(RoguelikeUpgradeId.Blink_CD_1))
        {
            timeBetweenBlinks = 3f;
        }
    }

    Vector3 GetBlinkDirection()
    {
        //return _movement.transform.forward; 
        var rotateDir = _movement.movement;
        rotateDir.y = 0;
        if (rotateDir.magnitude == 0)
            return transform.forward;

        return rotateDir.normalized;
    }

    public Vector3 targetpoint;

    Vector3 GetBlinkTargetPlace()
    {
        blinkRay.origin = gunPos.position;
        //blinkRay.direction = _movement.playerToMouse.normalized;
        blinkRay.direction = GetBlinkDirection();

        //public static RaycastHit[] RaycastAll(Ray ray, float maxDistance, int layerMask);
        RaycastHit[] blinkHits = Physics.RaycastAll(blinkRay, blinkDistance, 1 << blinkableMask);
        bool isRayBlockedByObstacle = false;

        var targetPlace = blinkRay.origin + blinkRay.direction * blinkDistance;

        targetpoint = targetPlace;

        foreach (var blinkHit in blinkHits)
        {
            EnemyHealth eh = blinkHit.collider.gameObject.GetComponent<EnemyHealth>();
            if (eh != null)
            {
                //ray hits an enemy
                if (!isRayBlockedByObstacle)
                {
                    //old way to test enemies pass though
                    //not accuracy
                }
            }
            else
            {
                //ray hits an obstacle
                if (!isRayBlockedByObstacle)
                {
                    isRayBlockedByObstacle = true;
                    targetPlace = blinkHit.point;
                }
            }
        }

        if (damageToPassThoughEnemy > 0)
        {
            foreach (var e in EnemyIdentifier.enemies)
            {
                if (e == null)
                    continue;

                if (MathGame.NearestDistanceFromLine(blinkRay.origin, targetPlace, e.transform.position) < checkDistanceToPassThoughEnemy)
                {
                    var eh = e.GetComponent<EnemyHealth>();
                    eh.TakeDamage(damageToPassThoughEnemy);
                }
            }
        }

        return targetPlace;
    }
}