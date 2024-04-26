using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    public bool isUnlock;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
           // if (!CombatManager.instance.HasEnemyLeft(true) && RoomBehaviour.instance.IsSpawnDone())
           //     SceneSwitcher.instance.SwitchToNextRoom();
        }
    }
}