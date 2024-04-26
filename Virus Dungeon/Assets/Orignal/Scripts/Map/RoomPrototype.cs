using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu]
public class RoomPrototype : ScriptableObject
{
    public string sceneName;

    public List<EnemyPrototype> normalEnemies;
    public List<EnemyPrototype> specialEnemies;
    public List<EnemyPrototype> verySpecialEnemies;

    public List<float> spawnWaves;
}