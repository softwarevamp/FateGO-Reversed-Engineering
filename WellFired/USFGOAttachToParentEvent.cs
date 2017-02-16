namespace WellFired
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [USequencerFriendlyName("FGO Attach Object To Parent"), USequencerEvent("FGO/Effect/Attach To Parent"), USequencerEventHideDuration]
    public class USFGOAttachToParentEvent : USEventBase
    {
        private GameObject dummyNode;
        public bool flipEffect;
        public string nodeName = string.Empty;
        private Transform originalParent;
        public Transform parentObject;
        public FGOTarget target;
        public Vector3 tposition = Vector3.zero;
        public Vector3 trotation = Vector3.zero;
        public Vector3 tscale = Vector3.one;

        public override void FireEvent()
        {
            if (this.parentObject == null)
            {
                Debug.Log("USFGOAttachToParentEvent has been asked to attach an object, but it hasn't been given a parent from USFGOAttachToParentEvent::FireEvent");
            }
            else
            {
                Transform parentObject = this.parentObject.transform.getNodeFromLvName(this.nodeName, -1);
                if (parentObject == null)
                {
                    parentObject = this.parentObject;
                }
                this.originalParent = base.AffectedObject.transform.parent;
                if (this.flipEffect)
                {
                    if (this.dummyNode != null)
                    {
                        UnityEngine.Object.Destroy(this.dummyNode);
                    }
                    this.dummyNode = new GameObject(base.AffectedObject.name + "(Parent)");
                    this.dummyNode.AddComponent<FlipEffectUpdater>().ConnectTarget = parentObject;
                    this.dummyNode.transform.parent = parentObject;
                    this.dummyNode.transform.localPosition = Vector3.zero;
                    this.dummyNode.transform.localEulerAngles = Vector3.zero;
                    this.dummyNode.transform.localScale = Vector3.one;
                    base.AffectedObject.transform.parent = this.dummyNode.transform;
                    base.AffectedObject.transform.localPosition = this.tposition;
                    base.AffectedObject.transform.localEulerAngles = this.trotation;
                    base.AffectedObject.transform.localScale = this.tscale;
                    this.dummyNode.transform.parent = this.parentObject;
                }
                else
                {
                    base.AffectedObject.transform.parent = parentObject;
                    base.AffectedObject.transform.localPosition = this.tposition;
                    base.AffectedObject.transform.localEulerAngles = this.trotation;
                    base.AffectedObject.transform.localScale = this.tscale;
                }
            }
        }

        public override void ProcessEvent(float deltaTime)
        {
        }

        public void SetupTarget(BattlePerformance performance, GameObject actor)
        {
            if (!SingletonMonoBehaviour<FGOSequenceManager>.Instance.isEditorMode)
            {
                BattleActorControl component = SingletonMonoBehaviour<BattleSequenceManager>.Instance.actor.GetComponent<BattleActorControl>();
                if ((component != null) && component.IsEnemy)
                {
                    Dictionary<FGOTarget, FGOTarget> dictionary = new Dictionary<FGOTarget, FGOTarget> {
                        { 
                            FGOTarget.ActorObject,
                            FGOTarget.ActorObject
                        },
                        { 
                            FGOTarget.EnemyObject,
                            FGOTarget.EnemyObject
                        },
                        { 
                            FGOTarget.Zero,
                            FGOTarget.Zero
                        },
                        { 
                            FGOTarget.Camera,
                            FGOTarget.Camera
                        },
                        { 
                            FGOTarget.Actor1,
                            FGOTarget.Actor4
                        },
                        { 
                            FGOTarget.Actor2,
                            FGOTarget.Actor5
                        },
                        { 
                            FGOTarget.Actor3,
                            FGOTarget.Actor6
                        },
                        { 
                            FGOTarget.Actor4,
                            FGOTarget.Actor1
                        },
                        { 
                            FGOTarget.Actor5,
                            FGOTarget.Actor2
                        },
                        { 
                            FGOTarget.Actor6,
                            FGOTarget.Actor3
                        }
                    };
                    this.target = dictionary[this.target];
                }
            }
            switch (this.target)
            {
                case FGOTarget.EnemyObject:
                {
                    GameObject singleTarget = SingletonMonoBehaviour<BattleSequenceManager>.Instance.SingleTarget;
                    if (singleTarget != null)
                    {
                        this.parentObject = singleTarget.transform;
                    }
                    break;
                }
                case FGOTarget.Zero:
                    this.parentObject = null;
                    break;

                case FGOTarget.Camera:
                    this.parentObject = performance.actorcamera.transform;
                    break;

                case FGOTarget.Actor1:
                    this.parentObject = performance.PlayerActorList[0].transform;
                    break;

                case FGOTarget.Actor2:
                    this.parentObject = performance.PlayerActorList[1].transform;
                    break;

                case FGOTarget.Actor3:
                    this.parentObject = performance.PlayerActorList[2].transform;
                    break;

                case FGOTarget.Actor4:
                    this.parentObject = performance.EnemyActorList[0].transform;
                    break;

                case FGOTarget.Actor5:
                    this.parentObject = performance.EnemyActorList[1].transform;
                    break;

                case FGOTarget.Actor6:
                    this.parentObject = performance.EnemyActorList[2].transform;
                    break;

                default:
                    this.parentObject = actor.transform;
                    break;
            }
        }

        public override void StopEvent()
        {
            this.UndoEvent();
        }

        public override void UndoEvent()
        {
            if (base.AffectedObject != null)
            {
                base.AffectedObject.transform.parent = this.originalParent;
                if (this.dummyNode != null)
                {
                    UnityEngine.Object.DestroyImmediate(this.dummyNode);
                }
            }
        }
    }
}

