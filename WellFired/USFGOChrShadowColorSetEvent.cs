namespace WellFired
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [USequencerEvent("FGO/Character/Set Chr Shadow Color"), USequencerEventHideDuration, USequencerFriendlyName("FGO Chr Shadow Color Set")]
    public class USFGOChrShadowColorSetEvent : USEventBase
    {
        public bool resetColor;
        public Color shadowColour = Color.black;
        public ChangeTarget target;

        public override void EndEvent()
        {
        }

        public override void FireEvent()
        {
            if (base.AffectedObject == null)
            {
                Debug.Log("Can not found FGOSequenceManager in USFGOFadeEvent.FireEvent");
            }
            else
            {
                List<GameObject> list = new List<GameObject>();
                switch (this.target)
                {
                    case ChangeTarget.Actor:
                        if (!SingletonMonoBehaviour<FGOSequenceManager>.Instance.isEditorMode)
                        {
                            list.Add(SingletonMonoBehaviour<BattleSequenceManager>.Instance.actor);
                            break;
                        }
                        list.Add(GameObject.Find("/Actor"));
                        break;

                    case ChangeTarget.PlayerSide:
                    {
                        if (!SingletonMonoBehaviour<FGOSequenceManager>.Instance.isEditorMode)
                        {
                            foreach (GameObject obj3 in SingletonMonoBehaviour<BattleSequenceManager>.Instance.Performance.PlayerActorList)
                            {
                                list.Add(obj3);
                            }
                            break;
                        }
                        GameObject item = GameObject.Find("/BattleBaseField/PlayerSide");
                        if (item != null)
                        {
                            list.Add(item);
                        }
                        break;
                    }
                    case ChangeTarget.EnemySide:
                    {
                        if (!SingletonMonoBehaviour<FGOSequenceManager>.Instance.isEditorMode)
                        {
                            foreach (GameObject obj5 in SingletonMonoBehaviour<BattleSequenceManager>.Instance.Performance.EnemyActorList)
                            {
                                list.Add(obj5);
                            }
                            break;
                        }
                        GameObject obj4 = GameObject.Find("/BattleBaseField/EnemySide");
                        if (obj4 != null)
                        {
                            list.Add(obj4);
                        }
                        break;
                    }
                    case ChangeTarget.All:
                    {
                        if (!SingletonMonoBehaviour<FGOSequenceManager>.Instance.isEditorMode)
                        {
                            foreach (GameObject obj7 in SingletonMonoBehaviour<BattleSequenceManager>.Instance.Performance.PlayerActorList)
                            {
                                list.Add(obj7);
                            }
                            foreach (GameObject obj8 in SingletonMonoBehaviour<BattleSequenceManager>.Instance.Performance.EnemyActorList)
                            {
                                list.Add(obj8);
                            }
                            break;
                        }
                        list.Add(GameObject.Find("/Actor"));
                        GameObject obj6 = GameObject.Find("/BattleBaseField/PlayerSide");
                        if (obj6 != null)
                        {
                            list.Add(obj6);
                        }
                        obj6 = GameObject.Find("/BattleBaseField/EnemySide");
                        if (obj6 != null)
                        {
                            list.Add(obj6);
                        }
                        break;
                    }
                }
                foreach (GameObject obj9 in list)
                {
                    if (obj9 != null)
                    {
                        BattleActorControl component = obj9.GetComponent<BattleActorControl>();
                        if (component != null)
                        {
                            if (this.resetColor)
                            {
                                component.ChangeShadowColor(Color.clear, 0.3f);
                            }
                            else
                            {
                                component.ChangeShadowColor(this.shadowColour, 0.3f);
                            }
                        }
                    }
                }
                base.Duration = 0.5f;
            }
        }

        private void OnEnable()
        {
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
        }

        private void Update()
        {
        }

        public enum ChangeTarget
        {
            Actor,
            PlayerSide,
            EnemySide,
            All
        }
    }
}

