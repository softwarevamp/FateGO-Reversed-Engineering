namespace WellFired
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [USequencerEventHideDuration, USequencerFriendlyName("FGO Set Chr Color Disp"), USequencerEvent("FGO/Character/Set Chr Disp")]
    public class USFGOChrDispChangeEvent : USEventBase
    {
        private List<GameObject> dispObjects;
        private List<bool> dispObjectsDefault;
        public bool IsDisp;
        public ChangeTarget target;

        public override void EndEvent()
        {
        }

        public override void FireEvent()
        {
            if (base.AffectedObject == null)
            {
                Debug.Log("Can not found FGOSequenceManager in USFGOFadeEvent.FireEvent");
                return;
            }
            this.dispObjects = new List<GameObject>();
            this.dispObjectsDefault = new List<bool>();
            if (!SingletonMonoBehaviour<FGOSequenceManager>.Instance.isEditorMode)
            {
                BattleActorControl component = SingletonMonoBehaviour<BattleSequenceManager>.Instance.actor.GetComponent<BattleActorControl>();
                if ((component != null) && component.IsEnemy)
                {
                    switch (this.target)
                    {
                        case ChangeTarget.PlayerSide:
                            this.target = ChangeTarget.EnemySide;
                            break;

                        case ChangeTarget.EnemySide:
                            this.target = ChangeTarget.PlayerSide;
                            break;
                    }
                }
            }
            switch (this.target)
            {
                case ChangeTarget.Actor:
                {
                    if (!SingletonMonoBehaviour<FGOSequenceManager>.Instance.isEditorMode)
                    {
                        this.dispObjects.Add(SingletonMonoBehaviour<BattleSequenceManager>.Instance.actor);
                        break;
                    }
                    GameObject obj2 = GameObject.Find("/Actor");
                    this.dispObjects.Add(obj2.transform.Find("chr").gameObject);
                    break;
                }
                case ChangeTarget.PlayerSide:
                    if (!SingletonMonoBehaviour<FGOSequenceManager>.Instance.isEditorMode)
                    {
                        foreach (GameObject obj5 in SingletonMonoBehaviour<BattleSequenceManager>.Instance.Performance.PlayerActorList)
                        {
                            if ((obj5 != SingletonMonoBehaviour<BattleSequenceManager>.Instance.actor) && (obj5 != null))
                            {
                                this.dispObjectsDefault.Add(obj5.activeSelf);
                                this.dispObjects.Add(obj5);
                            }
                        }
                    }
                    else
                    {
                        GameObject self = GameObject.Find("/BattleBaseField/PlayerSide");
                        if (self != null)
                        {
                            for (int i = 0; i < self.transform.childCount; i++)
                            {
                                GameObject gameObject = self.GetChild(i).gameObject;
                                this.dispObjectsDefault.Add(gameObject.activeSelf);
                                this.dispObjects.Add(gameObject);
                            }
                        }
                    }
                    goto Label_04F3;

                case ChangeTarget.EnemySide:
                    if (!SingletonMonoBehaviour<FGOSequenceManager>.Instance.isEditorMode)
                    {
                        foreach (GameObject obj8 in SingletonMonoBehaviour<BattleSequenceManager>.Instance.Performance.EnemyActorList)
                        {
                            if ((obj8 != SingletonMonoBehaviour<BattleSequenceManager>.Instance.actor) && (obj8 != null))
                            {
                                this.dispObjectsDefault.Add(obj8.activeSelf);
                                this.dispObjects.Add(obj8);
                            }
                        }
                    }
                    else
                    {
                        GameObject obj6 = GameObject.Find("/BattleBaseField/EnemySide");
                        for (int j = 0; j < obj6.transform.childCount; j++)
                        {
                            GameObject item = obj6.GetChild(j).gameObject;
                            this.dispObjectsDefault.Add(item.activeSelf);
                            this.dispObjects.Add(item);
                        }
                    }
                    goto Label_04F3;

                case ChangeTarget.All:
                    if (!SingletonMonoBehaviour<FGOSequenceManager>.Instance.isEditorMode)
                    {
                        foreach (GameObject obj13 in SingletonMonoBehaviour<BattleSequenceManager>.Instance.Performance.PlayerActorList)
                        {
                            if (obj13 != null)
                            {
                                this.dispObjectsDefault.Add(obj13.activeSelf);
                                this.dispObjects.Add(obj13);
                            }
                        }
                        foreach (GameObject obj14 in SingletonMonoBehaviour<BattleSequenceManager>.Instance.Performance.EnemyActorList)
                        {
                            if (obj14 != null)
                            {
                                this.dispObjectsDefault.Add(obj14.activeSelf);
                                this.dispObjects.Add(obj14);
                            }
                        }
                    }
                    else
                    {
                        GameObject obj9 = GameObject.Find("/Actor");
                        this.dispObjects.Add(obj9.transform.Find("chr").gameObject);
                        this.dispObjectsDefault.Add(obj9.transform.Find("chr").gameObject.activeSelf);
                        GameObject obj10 = GameObject.Find("/BattleBaseField/PlayerSide");
                        for (int k = 0; k < obj10.transform.childCount; k++)
                        {
                            GameObject obj11 = obj10.GetChild(k).gameObject;
                            this.dispObjectsDefault.Add(obj11.activeSelf);
                            this.dispObjects.Add(obj11);
                        }
                        obj10 = GameObject.Find("/BattleBaseField/EnemySide");
                        for (int m = 0; m < obj10.transform.childCount; m++)
                        {
                            GameObject obj12 = obj10.GetChild(m).gameObject;
                            this.dispObjectsDefault.Add(obj12.activeSelf);
                            this.dispObjects.Add(obj12);
                        }
                    }
                    goto Label_04F3;

                default:
                    goto Label_04F3;
            }
            this.dispObjectsDefault.Add(this.dispObjects[this.dispObjects.Count - 1].activeSelf);
        Label_04F3:
            base.Duration = 0.5f;
        }

        private void OnEnable()
        {
        }

        public override void ProcessEvent(float deltaTime)
        {
            foreach (GameObject obj2 in this.dispObjects)
            {
                if ((this.IsDisp && (this.target != ChangeTarget.Actor)) && (!SingletonMonoBehaviour<FGOSequenceManager>.Instance.isEditorMode && (SingletonMonoBehaviour<BattleSequenceManager>.Instance.SingleTarget != null)))
                {
                    if (obj2 == SingletonMonoBehaviour<BattleSequenceManager>.Instance.SingleTarget.gameObject)
                    {
                        obj2.gameObject.SetActive(this.IsDisp);
                    }
                }
                else
                {
                    obj2.gameObject.SetActive(this.IsDisp);
                }
            }
        }

        public override void StopEvent()
        {
            this.UndoEvent();
        }

        public override void UndoEvent()
        {
            if (this.dispObjects != null)
            {
                for (int i = 0; i < this.dispObjects.Count; i++)
                {
                    this.dispObjects[i].SetActive(this.dispObjectsDefault[i]);
                }
            }
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

