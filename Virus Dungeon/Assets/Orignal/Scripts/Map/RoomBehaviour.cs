using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using DG.Tweening;

public class RoomBehaviour : MonoBehaviour
{
    public List<SpawnEnemyBehaviour> normalSpawns;
    public List<SpawnEnemyBehaviour> specialSpawns;
    public List<SpawnEnemyBehaviour> verySpecialSpawns;
    public Transform[] levelEndPosList;

    public DoorBehaviour exit;
    public Transform entrance;

    float _nextWaveWaitTime;
    int _waveIndex;
    public static RoomBehaviour instance;
    bool _hasWaveToSpawn;


    private void Awake()
    {
        instance = this;
    }

    void HideExitEndEntranceCube()
    {
        if (exit != null)
        {
            var r = exit.transform.GetComponent<MeshRenderer>();
            r.enabled = false;
        }

        var r1 = entrance.GetComponent<MeshRenderer>();
        r1.enabled = false;

        var c1 = entrance.GetComponent<Collider>();
        if (c1 != null)
            c1.enabled = false;

        foreach (var levelEndPos in levelEndPosList)
        {
            levelEndPos.gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        HideExitEndEntranceCube();
        var door = Instantiate(CombatManager.instance.levelStartDoor);
        var spawnPosition = entrance.position;
        spawnPosition.y = 8;
        door.transform.position = spawnPosition;
        door.transform.DOMoveY(0, 1.0f).SetEase(Ease.InCubic).OnComplete(
            () =>
            {
                CameraFollow.instance.enabled = true;
                CameraFollow.instance.Init(PlayerBehaviour.instance.transform);
            });

        var player = Instantiate(SceneSwitcher.instance.playerPrefab, door.transform.position, entrance.rotation, door.transform);
        player.transform.localPosition = new Vector3(-0.034f, 0.407f, 0.694f);
        player.move.cc.enabled = false;
        // CameraFollow.instance.Init(player.transform);
        CameraFollow.instance.SyncPos(entrance.position);
        CameraFollow.instance.enabled = false;
        StartCoroutine(StartLevelPlay(door.GetComponent<StartRoomDoor>()));

        player.move.disableMove = true;
        player.shooting.enabled = false;
        player.shootSuper.enabled = false;
        player.blink.enabled = false;
        _hasWaveToSpawn = false;
        _waveIndex = 0;
        _nextWaveWaitTime = 0;
    }

    IEnumerator StartLevelPlay(StartRoomDoor door)
    {
        var player = PlayerBehaviour.instance;
        yield return new WaitForSeconds(1.3f);
        door.OpenDoor();
        player.transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        player.move.cc.enabled = true;
        player.move.simulateMoveForward = true;
        yield return new WaitForSeconds(1.0f);
        player.transform.SetParent(entrance.transform.parent);
        player.move.disableMove = false;
        player.move.simulateMoveForward = false;
        player.shooting.enabled = true;
        player.shootSuper.enabled = true;
        player.blink.enabled = true;
        yield return new WaitForSeconds(0.2f);
        //close door
        door.CloseDoor();
        yield return new WaitForSeconds(1.0f);
        //rise door
        door.transform.DOMoveY(15, 1.2f).SetEase(Ease.InCubic).OnComplete(
          () => { Destroy(door.gameObject); });
    }

    private void Update()
    {
        if (CombatManager.instance.HasEnemyLeft(true))
            return;//有敌人时，什么都不做

        _nextWaveWaitTime -= com.GameTime.deltaTime;
        if (_nextWaveWaitTime < 0)
        {
            if (_hasWaveToSpawn)

                TrySpawn();
            else
                PrepareWave();
        }
    }

    public void PrepareWave()
    {
        //Debug.Log("PrepareWave");
        if (IsSpawnDone())
            return;
        //Debug.Log("准备生成第" + (_waveIndex + 1) + "波");
        _nextWaveWaitTime = SceneSwitcher.instance.roomPrototype.spawnWaves[_waveIndex];
        _hasWaveToSpawn = true;
    }

    void TrySpawn()
    {
        //Debug.Log("生成第" + (_waveIndex + 1) + "波");
        List<EnemyPrototype> normalEnemies = SceneSwitcher.instance.roomPrototype.normalEnemies;
        List<EnemyPrototype> specialEnemies = SceneSwitcher.instance.roomPrototype.specialEnemies;
        List<EnemyPrototype> verySpecialEnemies = SceneSwitcher.instance.roomPrototype.verySpecialEnemies;
        SpawnOneKindOfEnemy(normalEnemies, normalSpawns);
        SpawnOneKindOfEnemy(specialEnemies, specialSpawns);
        SpawnOneKindOfEnemy(verySpecialEnemies, verySpecialSpawns);
        _waveIndex += 1;
        _hasWaveToSpawn = false;

        if (IsSpawnDone())
        {
            Debug.Log("生成完了");
        }
    }

    void SpawnOneKindOfEnemy(List<EnemyPrototype> enemies, List<SpawnEnemyBehaviour> spots)
    {
        for (int i = 0; i < spots.Count; i++)
        {
            if (spots == null)
                continue;
            if (i >= enemies.Count)
                continue;
            spots[i].Spawn(enemies[i], Random.Range(0f, 2.5f));
        }
    }

    public bool IsSpawnDone()
    {
        //Debug.Log(this.GetHashCode() + " IsSpawnDone " + _waveIndex + " >= " + SceneSwitcher.instance.roomPrototype.spawnWaves.Count);
        return _waveIndex >= SceneSwitcher.instance.roomPrototype.spawnWaves.Count;
    }

    public void SpawnLevelEndReward()
    {
        var nearestLevelEndPos = levelEndPosList[0];
        var playerPos = PlayerBehaviour.instance.transform.position;
        var maxDistance = float.MaxValue;
        foreach (var levelEndPos in levelEndPosList)
        {
            var dist = (levelEndPos.position - playerPos).magnitude;
            if (dist< maxDistance)
            {
                maxDistance = dist;
                nearestLevelEndPos = levelEndPos;
            }
        }

        var roomReward = Instantiate(CombatManager.instance.roomRewardPrefab, nearestLevelEndPos.position, Quaternion.identity, null);
        var vfx = Instantiate(CombatManager.instance.roomRewardVfx, roomReward.transform.position, Quaternion.identity, null);
        Destroy(vfx, 5);
    }
}