using System.Collections;
using UnityEngine;

public class RewardSlots : MonoBehaviour
{
    public static RewardSlots instance;
    public RewardSlotBehaviour prefab;
    public RectTransform slotParent;

    private void Awake()
    {
        instance = this;
    }

    public void Add(RoguelikeRewardPrototype proto)
    {
        var slot = Instantiate(prefab, slotParent);
        slot.gameObject.SetActive(true);
        slot.Init(proto);
    }
}