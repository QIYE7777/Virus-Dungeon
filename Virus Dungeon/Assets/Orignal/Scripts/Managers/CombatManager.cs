using UnityEngine;
using System.Collections;

public class CombatManager : MonoBehaviour
{
    public static CombatManager instance;
    public GameObject spawnEnemyVfx;
    public float spawnEnemyTimeAfterVfx = 2.7f;
    public GameObject levelStartDoor;
    public RoomRewardBehaviour roomRewardPrefab;
    public GameObject roomRewardVfx;
    public float lostHealth;
    public float hpMaxInGame = 100;

    private void Awake()
    {
        instance = this;
    }

    public bool HasEnemyLeft(bool includeSpawningEnemies)
    {
        bool hasEnemy = !EnemyIdentifier.NoEnemyExist();
        if (!includeSpawningEnemies)
        {
            return hasEnemy;
        }

        if (hasEnemy)
            return true;

        var room = RoomBehaviour.instance;
        if (room != null)
        {
            foreach (var s in room.normalSpawns)
            {
                if (s != null && s.state == SpawnEnemyBehaviour.SpawnState.Enemy || s.state == SpawnEnemyBehaviour.SpawnState.VFX)
                    return true;
            }

            foreach (var s in room.specialSpawns)
            {
                if (s != null && s.state == SpawnEnemyBehaviour.SpawnState.Enemy || s.state == SpawnEnemyBehaviour.SpawnState.VFX)
                    return true;
            }

            foreach (var s in room.verySpecialSpawns)
            {
                if (s != null && s.state == SpawnEnemyBehaviour.SpawnState.Enemy || s.state == SpawnEnemyBehaviour.SpawnState.VFX)
                    return true;
            }
        }

        return false;
    }
}
