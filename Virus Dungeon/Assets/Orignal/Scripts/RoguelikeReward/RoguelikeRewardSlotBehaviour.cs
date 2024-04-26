using UnityEngine;
using UnityEngine.UI;

namespace RoguelikeCombat
{
    public class RoguelikeRewardSlotBehaviour : MonoBehaviour
    {
        public Image icon;
        public RoguelikeRewardPrototype proto;
        public GameObject checker;

        private void Awake()
        {
            ToggleChecker(false);
        }

        public void Show(RoguelikeRewardPrototype r)
        {
            proto = r;
            icon.sprite = r.sp;
            gameObject.SetActive(true);
            ToggleChecker(false);
        }

        public void ToggleChecker(bool b)
        {
            checker.SetActive(b);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void OnClick()
        {
            RoguelikeRewardWindowBehaviour.instance.ShowDetail(proto);
        }
    }
}