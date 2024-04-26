using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections.Generic;
using com;

namespace RoguelikeCombat
{
    public class RoguelikeRewardWindowBehaviour : MonoBehaviour
    {
        public static RoguelikeRewardWindowBehaviour instance;
        public CanvasGroup cg;

        RoguelikeRewardEventData _data;
        public TMPro.TextMeshProUGUI detailTitle;
        public TMPro.TextMeshProUGUI detailDesc;
        public GameObject button;
        public GameObject detailPanel;
        public List<RoguelikeRewardSlotBehaviour> slots;

        RoguelikeUpgradeId _currentSelectedRoguelikeUpgradeId;

        private void Awake()
        {
            instance = this;
            _currentSelectedRoguelikeUpgradeId = RoguelikeUpgradeId.None;
        }

        private void Start()
        {
            Hide();
        }

        public void Setup(RoguelikeRewardEventData data)
        {
            _currentSelectedRoguelikeUpgradeId = RoguelikeUpgradeId.None;
            _data = data;

            button.SetActive(false);
            detailPanel.SetActive(false);

            int lenSlot = slots.Count;
            int lenReward = data.rewards.Count;
            for (int i = 0; i < lenSlot; i++)
            {
                //Debug.Log(i);
                var slot = slots[i];
                if (i >= lenReward)
                {
                    slot.Hide();
                    continue;
                }

                var reward = data.rewards[i];
                //Debug.Log(reward.id);
                if (reward == null)
                {
                    slot.Hide();
                }
                else
                {
                    slot.Show(reward);
                }
            }
        }

        public void ShowDetail(RoguelikeRewardPrototype proto)
        {
            _currentSelectedRoguelikeUpgradeId = proto.id;
            button.SetActive(true);

            detailTitle.text = proto.title;
            detailDesc.text = proto.desc;

            detailPanel.SetActive(true);
            foreach (var slot in slots)
            {
                slot.ToggleChecker(slot.proto == proto);
            }
        }

        public void OnClickConfirm()
        {
            RoguelikeRewardSystem.instance.AddPerk(_currentSelectedRoguelikeUpgradeId);
            Hide();
            RoguelikeRewardSystem.instance.OnEventFinished(_data);
        }

        public void Show()
        {
            cg.alpha = 0;
            cg.DOKill();
            cg.DOFade(1, 0.35f);

            cg.interactable = true;
            cg.blocksRaycasts = true;
            GameTime.timeScale = 0;
        }

        public void Hide()
        {
            cg.DOKill();
            cg.alpha = 0;

            cg.interactable = false;
            cg.blocksRaycasts = false;

            GameTime.timeScale = 1;
        }
    }
}