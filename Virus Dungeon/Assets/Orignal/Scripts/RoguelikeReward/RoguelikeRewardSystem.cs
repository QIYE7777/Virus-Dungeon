using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using System.Collections;
using System.Xml.Linq;

namespace RoguelikeCombat
{
    public class RoguelikeRewardSystem : MonoBehaviour
    {
        public static RoguelikeRewardSystem instance;
        public RoguelikeRewardConfig config;

        public List<RoguelikeUpgradeId> perks;

        public PlayerAmmunitionBehaviour PAB;

        public AudioSource pong;

        PlayerHealth playerHealth;

        Coroutine crtCoroutine;
        private void Awake()
        {
            instance = this;
            perks = new List<RoguelikeUpgradeId>();
        }

        /*void Start()
        {
            
        }*/

        public void StartNewEvent(bool nextLevel = true)
        {
            var data = new RoguelikeRewardEventData();
            data.title = "Choose a Upgrade!";
            data.nextLevel = nextLevel;
            //var availableRewardPool = config.roguelikeRewards;
            var availableRewardPool = GetRewardPool();
            data.rewards.Add(availableRewardPool[0]);
            data.rewards.Add(availableRewardPool[1]);
            data.rewards.Add(availableRewardPool[2]);
            RoguelikeRewardWindowBehaviour.instance.Setup(data);
            RoguelikeRewardWindowBehaviour.instance.Show();
            com.GameTime.timeScale = 0;
            PlayerBehaviour.instance.move.anim.SetBool("IsWalking", false);
        }

        public List<RoguelikeRewardPrototype> GetRewardPool()
        {
            var total = config.roguelikeRewards;
            List<RoguelikeRewardPrototype> res = new List<RoguelikeRewardPrototype>();
            List<RoguelikeRewardPrototype> candidates = new List<RoguelikeRewardPrototype>();

            foreach (var p in total)
            {
                //exist
                if (perks.Contains(p.id))
                    continue;
                //dependency not meet
                if (p.dependency != RoguelikeUpgradeId.None && !perks.Contains(p.dependency))
                    continue;

                candidates.Add(p);
            }

            int perksOnce = 3;
            if (candidates.Count < perksOnce)
                return null;


            for (int i = 0; i < perksOnce; i++)
            {
                var index = Random.Range(0, candidates.Count);
                res.Add(candidates[index]);
                candidates.RemoveAt(index);
            }

            return res;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                StartNewEvent(false);
            }
        }
        public void OnEventFinished(RoguelikeRewardEventData data)
        {
            if (crtCoroutine == null)
                crtCoroutine = StartCoroutine(EventFishCoroutine(data));
            PAB.CheckAmmunitionState();

            var player = PlayerBehaviour.instance;
            playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth.hpMaxOver == false)
            {
                playerHealth.MaxHealthUp();
            }

        }

        IEnumerator EventFishCoroutine(RoguelikeRewardEventData data)
        {
            com.GameTime.timeScale = 1;

            if (data.nextLevel)
            {
                var player = PlayerBehaviour.instance;
                var playerTrans = player.move.transform;


                player.move.disableMove = true;
                player.shooting.enabled = false;
                player.shootSuper.enabled = false;
                player.blink.enabled = false;
                yield return new WaitForSeconds(0.1f);
                //Debug.Log("TODO door show and take you up");
                var door = Instantiate(CombatManager.instance.levelStartDoor);
                var spawnPosition = playerTrans.position;
                spawnPosition.y = 8;
                spawnPosition.z += 2f;
                door.transform.position = spawnPosition;
                door.transform.DOMoveY(0, 1.0f).SetEase(Ease.InCubic);

                pong.Play();

                yield return new WaitForSeconds(1.0f);
                var doorBehaviour = door.GetComponent<StartRoomDoor>();


                yield return new WaitForSeconds(0.4f);
                doorBehaviour.OpenDoor();
                player.move.cc.enabled = true;
                player.move.simulateMoveForward = true;

                yield return new WaitForSeconds(0.8f);

                player.transform.SetParent(doorBehaviour.transform);
                player.move.disableMove = false;
                player.move.simulateMoveForward = false;
                player.move.cc.enabled = false;
                doorBehaviour.CloseDoor();


                yield return new WaitForSeconds(0.8f);
                CameraFollow.instance.SyncPos(player.transform.position);
                CameraFollow.instance.enabled = false;
                door.transform.DOMoveY(9, 1.2f).SetEase(Ease.InCubic);


                yield return new WaitForSeconds(0.3f);
                player.transform.GetChild(0).gameObject.SetActive(false);

                yield return new WaitForSeconds(1.3f);
                crtCoroutine = null;
                SceneSwitcher.instance.SwitchToNextRoom();
            }
            else
            {

            }
        }

        public void AddPerk(RoguelikeUpgradeId id)
        {
            perks.Add(id);
            RewardSlots.instance.Add(GetPrototype(id));
        }

        public bool HasPerk(RoguelikeUpgradeId id)
        {
            return perks.IndexOf(id) >= 0;
        }

        public RoguelikeRewardPrototype GetPrototype(RoguelikeUpgradeId id)
        {
            foreach (var p in config.roguelikeRewards)
            {
                if (p.id == id)
                    return p;
            }

            return null;
        }
    }
}
