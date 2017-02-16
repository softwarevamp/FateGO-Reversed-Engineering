namespace WellFired
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [USequencerEventHideDuration, USequencerFriendlyName("FGO Chr Color Change"), USequencerEvent("FGO/Character/Change Chr Color")]
    public class USFGOChrColorChangeEvent : USEventBase
    {
        private float currentCurveSampleTime;
        public Color fadeColour;
        public AnimationCurve fadeCurve;
        private List<SkinnedMeshRenderer> faderRenderers;
        private List<Color> originalColors;
        public ChangeTarget target;

        public USFGOChrColorChangeEvent()
        {
            Keyframe[] keys = new Keyframe[] { new Keyframe(0f, 0f), new Keyframe(1f, 1f), new Keyframe(3f, 1f), new Keyframe(4f, 0f) };
            this.fadeCurve = new AnimationCurve(keys);
            this.fadeColour = Color.black;
        }

        public override void EndEvent()
        {
            float b = this.fadeCurve.Evaluate(this.fadeCurve.keys[this.fadeCurve.length - 1].time);
            b = Mathf.Min(Mathf.Max(0f, b), 1f);
            this.SetMaterialColors(b);
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
                this.faderRenderers = new List<SkinnedMeshRenderer>();
                this.originalColors = new List<Color>();
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
                        foreach (SkinnedMeshRenderer renderer in obj9.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>())
                        {
                            if (renderer.enabled)
                            {
                                this.faderRenderers.Add(renderer);
                                if (SingletonMonoBehaviour<FGOSequenceManager>.Instance.isEditorMode)
                                {
                                    this.originalColors.Add(Color.white);
                                }
                                else
                                {
                                    this.originalColors.Add(renderer.material.GetColor("_Color"));
                                }
                            }
                        }
                    }
                }
            }
        }

        private void OnEnable()
        {
            this.SetMaterialColors(0f);
        }

        public override void ProcessEvent(float deltaTime)
        {
            this.currentCurveSampleTime = deltaTime;
            float b = this.fadeCurve.Evaluate(this.currentCurveSampleTime);
            b = Mathf.Min(Mathf.Max(0f, b), 1f);
            this.SetMaterialColors(b);
            float time = 0f;
            foreach (Keyframe keyframe in this.fadeCurve.keys)
            {
                if (keyframe.time > time)
                {
                    time = keyframe.time;
                }
            }
            base.Duration = time;
        }

        private void SetMaterialColors(float alpha)
        {
            if (this.faderRenderers != null)
            {
                float num = 1f - alpha;
                int num2 = 0;
                foreach (SkinnedMeshRenderer renderer in this.faderRenderers)
                {
                    Color color = this.originalColors[num2];
                    color = new Color(color.r * num, color.g * num, color.b * num, 1f);
                    Color color2 = new Color(this.fadeColour.r * alpha, this.fadeColour.g * alpha, this.fadeColour.b * alpha, 0f);
                    if (SingletonMonoBehaviour<FGOSequenceManager>.Instance.isEditorMode)
                    {
                        renderer.sharedMaterial.SetColor("_Color", color);
                        renderer.sharedMaterial.SetColor("_AddColor", color2);
                    }
                    else
                    {
                        foreach (Material material in renderer.materials)
                        {
                            material.SetColor("_Color", color);
                            material.SetColor("_AddColor", color2);
                        }
                    }
                    num2++;
                }
            }
        }

        public override void StopEvent()
        {
            this.UndoEvent();
        }

        public override void UndoEvent()
        {
            this.currentCurveSampleTime = 0f;
            this.SetMaterialColors(0f);
        }

        private void Update()
        {
            float time = 0f;
            foreach (Keyframe keyframe in this.fadeCurve.keys)
            {
                if (keyframe.time > time)
                {
                    time = keyframe.time;
                }
            }
            base.Duration = time;
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

