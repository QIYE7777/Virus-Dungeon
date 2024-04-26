using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public static SceneSwitcher instance;

    public LevelPrototype currentLevel;
    public RoomPrototype roomPrototype;

    public RoomBehaviour roomBehavior;

    int currentIndex;

    public PlayerBehaviour playerPrefab;

    public bool hasDead = false;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        currentIndex = 0;
        SwitchToRoomOfLevel();
    }

    void SwitchToRoomOfLevel()
    {
        if (currentIndex >= currentLevel.rooms.Count)
        {
            Debug.LogError("no enough room! currentIndex is " + currentIndex);
            // Win();
            return;
        }

        var room = currentLevel.rooms[currentIndex];
        //var room = currentLevel.rooms[0];

        roomPrototype = room;
        SceneManager.LoadScene(room.sceneName, LoadSceneMode.Single);
        Debug.Log("SwitchToRoomOfLevel" + room);
    }

    public void SwitchToNextRoom()
    {
        currentIndex += 1;
        SwitchToRoomOfLevel();
    }

    public void RestartCurrentLevel()
    {
        //SceneManager.LoadScene(1);
        currentIndex = 0;
        SwitchToRoomOfLevel();
        hasDead = true;
    }
}