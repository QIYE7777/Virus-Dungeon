using UnityEngine;
using System.Collections;

public class RoomRewardBehaviour : MonoBehaviour
{
    private GameObject vfx;
    bool _triggered;
    public static RoomRewardBehaviour instance;

    private void Awake()
    {
        instance = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_triggered)
            return;

        if (other.gameObject.tag == "Player")
        {
            _triggered = true;
            StartCoroutine(ShowRoguelike());
            Destroy(gameObject, 0.3f);
        }
    }

    IEnumerator ShowRoguelike()
    {
        yield return new WaitForSeconds(0.2f);
        RoguelikeCombat.RoguelikeRewardSystem.instance.StartNewEvent();
        PlayerBehaviour.instance.health.SaveHp();
    }
}
