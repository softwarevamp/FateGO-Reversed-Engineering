namespace WellFired
{
    using System;
    using UnityEngine;

    [USequencerFriendlyName("FGO Reset Character Position(s)"), USequencerEvent("FGO/Character/Reset Character Position(s)")]
    public class USFGOResetCharacterPositionEvent : USEventBase
    {
        private const string posEnemyPrefix = "Enemy";
        public CharacterPosition positionInfo;
        private const string posPlayerPrefix = "Player";

        public override void FireEvent()
        {
            if (!SingletonMonoBehaviour<FGOSequenceManager>.Instance.isEditorMode)
            {
                bool flag = this.positionInfo == CharacterPosition.ResetEnemies;
                bool flag2 = this.positionInfo == CharacterPosition.ResetPlayers;
                if (this.positionInfo == CharacterPosition.ResetAll)
                {
                    flag = flag2 = true;
                }
                bool cameraFlip = BattlePerformance.CameraFlip;
                BattlePerformance performance = SingletonMonoBehaviour<BattleSequenceManager>.Instance.Performance;
                if (flag2)
                {
                    if ((!SingletonMonoBehaviour<FGOSequenceManager>.Instance.isEditorMode && !cameraFlip) && ((SingletonMonoBehaviour<BattleSequenceManager>.Instance.SingleTarget != null) && !SingletonMonoBehaviour<BattleSequenceManager>.Instance.SingleTarget.GetComponent<BattleActorControl>().IsEnemy))
                    {
                        foreach (GameObject obj2 in performance.PlayerActorList)
                        {
                            if (obj2 != null)
                            {
                                if (obj2 == SingletonMonoBehaviour<BattleSequenceManager>.Instance.SingleTarget)
                                {
                                    obj2.transform.position = SingletonMonoBehaviour<FGOSequenceManager>.Instance.getCharacterPosition((!cameraFlip ? "Player" : "Enemy") + "2").position;
                                }
                                else
                                {
                                    obj2.SetActive(false);
                                }
                            }
                        }
                    }
                    else
                    {
                        int num2 = 1;
                        foreach (GameObject obj3 in performance.PlayerActorList)
                        {
                            if (obj3 != null)
                            {
                                obj3.transform.position = SingletonMonoBehaviour<FGOSequenceManager>.Instance.getCharacterPosition((!cameraFlip ? "Player" : "Enemy") + num2).position;
                            }
                            num2++;
                        }
                    }
                }
                if (flag)
                {
                    if ((!SingletonMonoBehaviour<FGOSequenceManager>.Instance.isEditorMode && cameraFlip) && ((SingletonMonoBehaviour<BattleSequenceManager>.Instance.SingleTarget != null) && SingletonMonoBehaviour<BattleSequenceManager>.Instance.SingleTarget.GetComponent<BattleActorControl>().IsEnemy))
                    {
                        foreach (GameObject obj4 in performance.EnemyActorList)
                        {
                            if (obj4 != null)
                            {
                                if (obj4 == SingletonMonoBehaviour<BattleSequenceManager>.Instance.SingleTarget)
                                {
                                    obj4.transform.position = SingletonMonoBehaviour<FGOSequenceManager>.Instance.getCharacterPosition((!cameraFlip ? "Enemy" : "Player") + "2").position;
                                }
                                else
                                {
                                    obj4.SetActive(false);
                                }
                            }
                        }
                    }
                    else
                    {
                        int num5 = 1;
                        foreach (GameObject obj5 in performance.EnemyActorList)
                        {
                            if (obj5 != null)
                            {
                                obj5.transform.position = SingletonMonoBehaviour<FGOSequenceManager>.Instance.getCharacterPosition((!cameraFlip ? "Enemy" : "Player") + num5).position;
                            }
                            num5++;
                        }
                    }
                }
            }
            else
            {
                Transform transform = SingletonMonoBehaviour<FGOSequenceManager>.Instance.getCharacterPosition("Player1");
                if (transform != null)
                {
                    GameObject obj6 = GameObject.Find("/Actor");
                    if (obj6 != null)
                    {
                        obj6.transform.position = transform.position;
                    }
                }
            }
        }

        public override void ProcessEvent(float deltaTime)
        {
        }

        public override void StopEvent()
        {
            this.UndoEvent();
        }

        public override void UndoEvent()
        {
            if (SingletonMonoBehaviour<FGOSequenceManager>.Instance.isEditorMode)
            {
                GameObject obj2 = GameObject.Find("/Actor");
                if (obj2 != null)
                {
                    obj2.transform.position = new Vector3(12f, -0.12f, -45.50462f);
                }
            }
        }

        public void Update()
        {
        }

        public enum CharacterPosition
        {
            ResetAll,
            ResetPlayers,
            ResetEnemies
        }
    }
}

