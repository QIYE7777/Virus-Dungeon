using com;
using RoguelikeCombat;
using UnityEngine;
using UnityEngine.UI;


public class RewardSlotBehaviour : MonoBehaviour
{
    public Image img;
    public HoverBehaviour hoverBehaviour;

    public void Init(RoguelikeRewardPrototype proto)
    {
        img.sprite = proto.sp;
        hoverBehaviour.description = proto.desc;
        hoverBehaviour.title = proto.title;
    }
}