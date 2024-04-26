using UnityEngine;

public class SpawnEnemyBehaviour : MonoBehaviour
{
    public enum SpawnState
    {
        None = 0,
        VFX = 1,
        Enemy = 2,
    }

    public SpawnState state { get; private set; }

    private float _spawnTime;
    private EnemyPrototype _enemyToSpawn;

    private void Start()
    {
        var r = GetComponent<MeshRenderer>();
        r.enabled = false;

        var c = GetComponent<Collider>();
        if (c != null)
            c.enabled = false;

        state = SpawnState.None;
    }

    public void Spawn(EnemyPrototype e, float delay)
    {
        if (e == null)
            return;

        _spawnTime = delay + com.GameTime.time;
        _enemyToSpawn = e;
        state = SpawnState.VFX;
    }

    private void Update()
    {
        if (state == SpawnState.None)
            return;


        if (com.GameTime.time > _spawnTime)
        {
            if (state == SpawnState.VFX)
            {
                var vfx = Instantiate(CombatManager.instance.spawnEnemyVfx, transform.position, Quaternion.identity);
                Destroy(vfx, 5);

                _spawnTime = CombatManager.instance.spawnEnemyTimeAfterVfx + com.GameTime.time;
                state = SpawnState.Enemy;
            }
            else if (state == SpawnState.Enemy)
            {
                var enemyId = Instantiate(_enemyToSpawn.prefab, transform.position, transform.rotation);
                enemyId.InitializeEnemyPrototype(_enemyToSpawn);
                state = SpawnState.None;
            }
        }
    }
}