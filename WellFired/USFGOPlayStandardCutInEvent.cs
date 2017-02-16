namespace WellFired
{
    using System;
    using UnityEngine;

    [USequencerEventHideDuration, USequencerFriendlyName("FGO Play Standard CutIn"), USequencerEvent("FGO/CutIn/Play Standard CutIn(Servant)")]
    public class USFGOPlayStandardCutInEvent : USEventBase
    {
        private Animation[] animations;
        private bool AnimStarted;
        private changeVColor[] changeVColors;
        public CutInType cutInType;
        public Face.Type faceType;
        public int limitCount;
        private GameObject myCutIn;
        private UIStandFigureM myStandFigure;
        private bool originalActive;
        public bool parentCamera = true;
        private ParticleSystem[] particles;
        public int svtId;
        private bool useAssetBundle;
        private UVScroll[] uvScrolls;

        public override void EndEvent()
        {
        }

        public override void FireEvent()
        {
            if (base.AffectedObject != null)
            {
                if (this.myCutIn != null)
                {
                    if (SingletonMonoBehaviour<FGOSequenceManager>.Instance.isEditorMode)
                    {
                        UnityEngine.Object.DestroyImmediate(this.myCutIn);
                    }
                    else
                    {
                        UnityEngine.Object.Destroy(this.myCutIn);
                    }
                    this.myCutIn = null;
                }
                int limitCount = this.limitCount;
                bool flag = !SingletonMonoBehaviour<FGOSequenceManager>.Instance.isEditorMode;
                this.AnimStarted = !SingletonMonoBehaviour<FGOSequenceManager>.Instance.isEditorMode;
                GameObject obj2 = UnityEngine.Object.Instantiate<GameObject>(SingletonMonoBehaviour<FGOSequenceManager>.Instance.standardCutIn);
                this.myCutIn = obj2;
                this.originalActive = this.myCutIn.activeSelf;
                this.myCutIn.SetActive(true);
                this.animations = this.myCutIn.GetComponentsInChildren<Animation>(true);
                if (!Application.isPlaying)
                {
                    this.particles = this.myCutIn.GetComponentsInChildren<ParticleSystem>(true);
                    this.uvScrolls = this.myCutIn.GetComponentsInChildren<UVScroll>(true);
                    this.changeVColors = this.myCutIn.GetComponentsInChildren<changeVColor>(true);
                }
                if (this.animations != null)
                {
                    foreach (Animation animation in this.animations)
                    {
                        if (animation != null)
                        {
                            if (flag)
                            {
                                animation.Stop();
                            }
                            else
                            {
                                string name = "CutIn" + (((int) this.cutInType) + 1);
                                if (animation[name] == null)
                                {
                                    name = animation.clip.name;
                                }
                                animation.Play(name);
                            }
                        }
                    }
                }
                if (SingletonMonoBehaviour<FGOSequenceManager>.Instance.isEditorMode)
                {
                    this.useAssetBundle = false;
                }
                else
                {
                    this.useAssetBundle = true;
                    BattleActorControl component = SingletonMonoBehaviour<BattleSequenceManager>.Instance.actor.GetComponent<BattleActorControl>();
                    if ((component != null) && (this.svtId == component.getServantId()))
                    {
                        limitCount = component.LimitImageIndex;
                    }
                    MeshRenderer componentInChildren = obj2.GetComponentInChildren<MeshRenderer>();
                    if (componentInChildren != null)
                    {
                        componentInChildren.material.mainTexture = null;
                        componentInChildren.material.SetTexture("_SubTex", null);
                        componentInChildren.enabled = false;
                        GameObject gameObject = componentInChildren.gameObject;
                        UIStandFigureM em = SingletonMonoBehaviour<BattleSequenceManager>.Instance.FetchMeshPrefab(this.svtId, limitCount, this.faceType);
                        if (em != null)
                        {
                            this.myStandFigure = em;
                            this.myStandFigure.gameObject.transform.parent = gameObject.transform;
                            this.myStandFigure.SetActive(true);
                            Transform transform = em.transform.getNodeFromName("Body", false);
                            if (transform != null)
                            {
                                FGOStandFigureMColor color = obj2.transform.getNodeFromName("cutin", true).GetComponent<FGOStandFigureMColor>();
                                color.renderers = new MeshRenderer[] { transform.GetComponent<MeshRenderer>() };
                                color.OnUpdate();
                            }
                            this.onAssetLoad();
                        }
                        else
                        {
                            base.Sequence.Pause();
                            this.myStandFigure = StandFigureManager.CreateMeshPrefab(gameObject, this.svtId, limitCount, this.faceType, 0, new System.Action(this.onAssetLoad));
                        }
                        this.myStandFigure.transform.localPosition = new Vector3(0f, 0f, -5f);
                        this.myStandFigure.transform.localEulerAngles = new Vector3(90f, 180f, 0f);
                        this.myStandFigure.transform.localScale = new Vector3(0.01f, 0.01f, 1f);
                    }
                }
                if (this.parentCamera)
                {
                    obj2.transform.parent = SingletonMonoBehaviour<FGOSequenceManager>.Instance.mainCamera.transform;
                    obj2.transform.localPosition = Vector3.zero;
                    obj2.transform.localEulerAngles = Vector3.zero;
                    obj2.transform.localScale = Vector3.one;
                }
                if (Application.isPlaying && (this.particles != null))
                {
                    foreach (ParticleSystem system in this.particles)
                    {
                        if (system != null)
                        {
                            if (flag)
                            {
                                system.Stop();
                            }
                            else
                            {
                                system.Play();
                            }
                        }
                    }
                }
            }
        }

        private void onAssetLoad()
        {
            if (!base.Sequence.IsPlaying)
            {
                base.Sequence.Play();
            }
            this.AnimStarted = true;
            if (this.animations != null)
            {
                foreach (Animation animation in this.animations)
                {
                    string name = "CutIn" + (((int) this.cutInType) + 1);
                    if (animation[name] == null)
                    {
                        name = base.GetComponent<Animation>().clip.name;
                    }
                    if (animation != null)
                    {
                        animation.Play(name);
                    }
                }
            }
            if (this.particles != null)
            {
                foreach (ParticleSystem system in this.particles)
                {
                    if (system != null)
                    {
                        system.Play();
                    }
                }
            }
        }

        protected void OnDestroy()
        {
            if (this.myStandFigure != null)
            {
                this.myStandFigure.Destroy();
                UnityEngine.Object.Destroy(this.myStandFigure);
                this.myStandFigure = null;
            }
            if (this.myCutIn != null)
            {
                UnityEngine.Object.Destroy(this.myCutIn);
                this.myCutIn = null;
            }
            if (this.useAssetBundle)
            {
                StandFigureManager.ReleaseAsset(this.svtId, this.limitCount);
            }
        }

        public override void ProcessEvent(float deltaTime)
        {
            if (SingletonMonoBehaviour<FGOSequenceManager>.Instance.isEditorMode)
            {
                if ((this.animations != null) && this.AnimStarted)
                {
                    foreach (Animation animation in this.animations)
                    {
                        string name = "CutIn" + (((int) this.cutInType) + 1);
                        if (animation[name] == null)
                        {
                            name = animation.clip.name;
                        }
                        AnimationState state = animation[name];
                        if (!animation.IsPlaying(name))
                        {
                            animation.Play(name);
                        }
                        state.time = deltaTime;
                        state.enabled = true;
                        animation.Sample();
                        state.enabled = false;
                    }
                }
                if (!Application.isPlaying)
                {
                    if (this.particles != null)
                    {
                        foreach (ParticleSystem system in this.particles)
                        {
                            system.Simulate(deltaTime);
                        }
                    }
                    if (this.uvScrolls != null)
                    {
                        foreach (UVScroll scroll in this.uvScrolls)
                        {
                            scroll.UpdateUV();
                        }
                    }
                    if (this.changeVColors != null)
                    {
                        foreach (changeVColor color in this.changeVColors)
                        {
                            color.UpdateVColor();
                        }
                    }
                }
            }
        }

        public override void StopEvent()
        {
            if (this.animations != null)
            {
                foreach (Animation animation in this.animations)
                {
                    if (animation != null)
                    {
                        animation.Stop();
                    }
                }
            }
            this.UndoEvent();
        }

        public override void UndoEvent()
        {
            if (this.myCutIn != null)
            {
                this.myCutIn.SetActive(this.originalActive);
                if (this.myCutIn != null)
                {
                    if (SingletonMonoBehaviour<FGOSequenceManager>.Instance.isEditorMode)
                    {
                        UnityEngine.Object.DestroyImmediate(this.myCutIn);
                    }
                    else
                    {
                        UnityEngine.Object.Destroy(this.myCutIn);
                    }
                    this.myCutIn = null;
                }
            }
        }

        public void Update()
        {
            if (base.AffectedObject != null)
            {
                float length = 0f;
                if (this.animations != null)
                {
                    foreach (Animation animation in this.animations)
                    {
                        if ((animation != null) && (length < animation.clip.length))
                        {
                            length = animation.clip.length;
                        }
                    }
                }
                if (this.particles != null)
                {
                    foreach (ParticleSystem system in this.particles)
                    {
                        if (system != null)
                        {
                            float num4 = system.duration + system.startLifetime;
                            if (length < num4)
                            {
                                length = num4;
                            }
                        }
                    }
                }
                base.Duration = length;
            }
        }

        public enum CutInType
        {
            CutIn1,
            CutIn2,
            CutIn3,
            CutIn4,
            CutIn5,
            CutIn6,
            CutIn7,
            CutIn8,
            CutIn9,
            CutIn10,
            CutIn11,
            CutIn12,
            CutIn13,
            CutIn14,
            CutIn15
        }
    }
}

