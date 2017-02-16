namespace WellFired
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [USequencerEvent("FGO/Effect/Create Effect"), USequencerFriendlyName("FGO Create Effect")]
    public class USFGOCreateEffectEvent : USEventBase
    {
        [CompilerGenerated]
        private static Predicate<GameObject> <>f__am$cacheE;
        [CompilerGenerated]
        private static Predicate<GameObject> <>f__am$cacheF;
        private List<Animation> animations;
        public EffectCategory category;
        private List<changeVColor> changeVColors;
        public string effectName;
        private List<GameObject> effectObjs;
        public int functionIndex = -1;
        public string groupId;
        public bool isParent;
        public Vector3 offsetPosition = Vector3.zero;
        public Vector3 offsetRotation = Vector3.zero;
        private List<ParticleSystem> particles;
        public bool sideflip;
        public EffectTarget target;
        private List<UVScroll> uvScrolls;

        private void AddAnimationElements(GameObject obj)
        {
            Animation[] componentsInChildren = obj.GetComponentsInChildren<Animation>();
            ParticleSystem[] collection = obj.GetComponentsInChildren<ParticleSystem>();
            UVScroll[] scrollArray = obj.GetComponentsInChildren<UVScroll>();
            changeVColor[] colorArray = obj.GetComponentsInChildren<changeVColor>();
            this.animations.AddRange(componentsInChildren);
            this.particles.AddRange(collection);
            this.uvScrolls.AddRange(scrollArray);
            this.changeVColors.AddRange(colorArray);
        }

        private void destroyEffectObjs()
        {
            if (this.effectObjs != null)
            {
                if (SingletonMonoBehaviour<FGOSequenceManager>.Instance.isEditorMode)
                {
                    foreach (GameObject obj2 in this.effectObjs)
                    {
                        UnityEngine.Object.DestroyImmediate(obj2);
                    }
                }
                else
                {
                    foreach (GameObject obj3 in this.effectObjs)
                    {
                        UnityEngine.Object.Destroy(obj3);
                    }
                }
                this.effectObjs = null;
            }
        }

        public override void EndEvent()
        {
            this.UndoEvent();
        }

        public override void FireEvent()
        {
            Debug.Log("Fired! : USFGOCreateEffectEvent");
            string effectAssetPath = this.GetEffectAssetPath();
            string effectAssetFileName = this.GetEffectAssetFileName();
            this.destroyEffectObjs();
            this.effectObjs = new List<GameObject>();
            this.animations = new List<Animation>();
            this.particles = new List<ParticleSystem>();
            this.uvScrolls = new List<UVScroll>();
            this.changeVColors = new List<changeVColor>();
            GameObject original = null;
            string str3 = string.Empty;
            if (!SingletonMonoBehaviour<FGOSequenceManager>.Instance.isEditorMode)
            {
                Debug.Log("Fired! : proc1");
                string str4 = string.Empty;
                if (this.IsResource())
                {
                    Debug.Log("Fired! : res");
                    str4 = effectAssetPath;
                    string[] textArray1 = new string[] { "Assets/", str4, "/", effectAssetFileName, ".prefab" };
                    str3 = string.Concat(textArray1);
                    Debug.Log("Fired! : fullpath: " + str3);
                }
                else
                {
                    str4 = "AssetStorages/" + effectAssetPath;
                    AssetData data = AssetManager.getAssetStorage(effectAssetPath);
                    if (data == null)
                    {
                        Debug.LogError("Can not found assetbundle : " + effectAssetPath);
                        return;
                    }
                    char[] separator = new char[] { '/' };
                    string[] strArray = effectAssetFileName.Split(separator);
                    original = data.GetObject<GameObject>(strArray[strArray.Length - 1]);
                    str3 = effectAssetPath + ":" + effectAssetFileName;
                }
            }
            else
            {
                string str5 = string.Empty;
                if (this.IsResource())
                {
                    str5 = effectAssetPath;
                }
                else
                {
                    str5 = "AssetStorages/" + effectAssetPath;
                }
                string[] textArray2 = new string[] { "Assets/", str5, "/", effectAssetFileName, ".prefab" };
                str3 = string.Concat(textArray2);
            }
            if (original == null)
            {
                Debug.LogError("USFGOCreateEffectEvent:FireEvent:File Not Found:" + str3);
            }
            else
            {
                List<GameObject> list = getTargets(this.target, -1);
                Transform transform = null;
                if (SingletonMonoBehaviour<FGOSequenceManager>.Instance.isEditorMode)
                {
                    transform = GameObject.Find("/AllEffects").transform;
                }
                else
                {
                    transform = SingletonMonoBehaviour<BattleSequenceManager>.Instance.Performance.root_field;
                }
                foreach (GameObject obj3 in list)
                {
                    GameObject item = UnityEngine.Object.Instantiate<GameObject>(original);
                    item.SetActive(true);
                    item.transform.parent = obj3.transform;
                    item.transform.localPosition = this.offsetPosition;
                    item.transform.localEulerAngles = this.offsetRotation;
                    item.transform.localScale = Vector3.one;
                    if (this.sideflip && this.IsEnemy(obj3))
                    {
                        item.transform.localPosition = new Vector3(-this.offsetPosition.x, this.offsetPosition.y, this.offsetPosition.z);
                        item.transform.Rotate((float) 0f, 180f, (float) 0f);
                    }
                    if (!this.isParent)
                    {
                        item.transform.parent = transform;
                    }
                    this.effectObjs.Add(item);
                    this.AddAnimationElements(item);
                }
                this.PlayAllEffects();
            }
        }

        private string GetEffectAssetFileName()
        {
            string str = string.Empty;
            switch (this.category)
            {
            }
            return (str + this.effectName);
        }

        private string GetEffectAssetPath()
        {
            string str = string.Empty;
            switch (this.category)
            {
                case EffectCategory.ServantNoblePhantasm:
                    return ("NoblePhantasm/" + this.groupId);

                case EffectCategory.Servant:
                    return ("Servants/" + this.groupId);

                case EffectCategory.Weapon:
                    return ("Effect/weapon/" + this.groupId);

                case EffectCategory.Common:
                    return "Resources/effect";
            }
            return str;
        }

        public static List<GameObject> getTargets(EffectTarget target, int functionIndex = -1)
        {
            List<GameObject> list = new List<GameObject>();
            if (!SingletonMonoBehaviour<FGOSequenceManager>.Instance.isEditorMode)
            {
                BattlePerformance performance = SingletonMonoBehaviour<BattleSequenceManager>.Instance.Performance;
                BattleActorControl component = SingletonMonoBehaviour<BattleSequenceManager>.Instance.actor.GetComponent<BattleActorControl>();
                switch (target)
                {
                    case EffectTarget.Actor:
                        list.Add(SingletonMonoBehaviour<BattleSequenceManager>.Instance.actor);
                        break;

                    case EffectTarget.Target:
                    {
                        int uniqueId = component.ActionData.GetTarget(functionIndex);
                        list.Add(performance.getServantGameObject(uniqueId));
                        break;
                    }
                    case EffectTarget.DamageTagets:
                        foreach (int num2 in component.ActionData.GetDamageTargets(functionIndex))
                        {
                            list.Add(performance.getServantGameObject(num2));
                        }
                        break;

                    case EffectTarget.BuffTargets:
                        foreach (int num4 in component.ActionData.GetBuffTargets(functionIndex))
                        {
                            list.Add(performance.getServantGameObject(num4));
                        }
                        break;

                    case EffectTarget.DebuffTargets:
                        foreach (int num6 in component.ActionData.GetDebuffTargets(functionIndex))
                        {
                            list.Add(performance.getServantGameObject(num6));
                        }
                        break;

                    case EffectTarget.PlayerParty:
                    case EffectTarget.EnemyParty:
                    case EffectTarget.All:
                        if (target != EffectTarget.EnemyParty)
                        {
                            foreach (GameObject obj2 in performance.PlayerActorList)
                            {
                                list.Add(obj2);
                            }
                        }
                        if (target != EffectTarget.PlayerParty)
                        {
                            foreach (GameObject obj3 in performance.EnemyActorList)
                            {
                                list.Add(obj3);
                            }
                        }
                        break;
                }
            }
            else
            {
                switch (target)
                {
                    case EffectTarget.Actor:
                        list.Add(GameObject.Find("/Actor"));
                        break;

                    case EffectTarget.Target:
                        list.Add(GameObject.Find("/BattleBaseField/EnemySide/chrRoot1"));
                        list.Add(GameObject.Find("/BattleBaseField/EnemySide/chrRoot2"));
                        list.Add(GameObject.Find("/BattleBaseField/EnemySide/chrRoot3"));
                        break;

                    case EffectTarget.DamageTagets:
                        list.Add(GameObject.Find("/BattleBaseField/EnemySide/chrRoot1"));
                        break;

                    case EffectTarget.BuffTargets:
                        list.Add(GameObject.Find("/BattleBaseField/PlayerSide/chrRoot2"));
                        break;

                    case EffectTarget.DebuffTargets:
                        list.Add(GameObject.Find("/BattleBaseField/EnemySide/chrRoot1"));
                        break;

                    case EffectTarget.PlayerParty:
                        list.Add(GameObject.Find("/BattleBaseField/PlayerSide/chrRoot1"));
                        list.Add(GameObject.Find("/BattleBaseField/PlayerSide/chrRoot2"));
                        list.Add(GameObject.Find("/BattleBaseField/PlayerSide/chrRoot3"));
                        list.Add(GameObject.Find("/Actor"));
                        break;

                    case EffectTarget.EnemyParty:
                        list.Add(GameObject.Find("/BattleBaseField/EnemySide/chrRoot1"));
                        list.Add(GameObject.Find("/BattleBaseField/EnemySide/chrRoot2"));
                        list.Add(GameObject.Find("/BattleBaseField/EnemySide/chrRoot3"));
                        break;

                    case EffectTarget.All:
                        list.Add(GameObject.Find("/Actor"));
                        list.Add(GameObject.Find("/BattleBaseField/PlayerSide/chrRoot1"));
                        list.Add(GameObject.Find("/BattleBaseField/PlayerSide/chrRoot2"));
                        list.Add(GameObject.Find("/BattleBaseField/PlayerSide/chrRoot3"));
                        list.Add(GameObject.Find("/BattleBaseField/EnemySide/chrRoot1"));
                        list.Add(GameObject.Find("/BattleBaseField/EnemySide/chrRoot2"));
                        list.Add(GameObject.Find("/BattleBaseField/EnemySide/chrRoot3"));
                        break;
                }
                if (<>f__am$cacheE == null)
                {
                    <>f__am$cacheE = x => x == null;
                }
                list.RemoveAll(<>f__am$cacheE);
                return list;
            }
            if (<>f__am$cacheF == null)
            {
                <>f__am$cacheF = x => x == null;
            }
            list.RemoveAll(<>f__am$cacheF);
            return list;
        }

        private bool IsEnemy(GameObject tgt)
        {
            if (!SingletonMonoBehaviour<FGOSequenceManager>.Instance.isEditorMode)
            {
                return false;
            }
            return (((tgt != GameObject.Find("/Actor")) && (tgt != GameObject.Find("/BattleBaseField/PlayerSide/chrRoot1"))) && ((tgt != GameObject.Find("/BattleBaseField/PlayerSide/chrRoot2")) && (tgt != GameObject.Find("/BattleBaseField/PlayerSide/chrRoot3"))));
        }

        private bool IsResource() => 
            (this.category == EffectCategory.Common);

        private void PlayAllEffects()
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
                        float num2 = system.duration + system.startLifetime;
                        if (length < num2)
                        {
                            length = num2;
                        }
                    }
                }
            }
            base.Duration = length;
            if (this.animations != null)
            {
                foreach (Animation animation2 in this.animations)
                {
                    if (animation2 != null)
                    {
                        animation2.Play(animation2.clip.name);
                    }
                }
            }
            if (Application.isPlaying && (this.particles != null))
            {
                foreach (ParticleSystem system2 in this.particles)
                {
                    if (system2 != null)
                    {
                        system2.Play();
                    }
                }
            }
        }

        public override void ProcessEvent(float deltaTime)
        {
            this.UpdateAllEffects(deltaTime);
        }

        public override void StopEvent()
        {
            this.UndoEvent();
        }

        public override void UndoEvent()
        {
            this.destroyEffectObjs();
        }

        public void Update()
        {
        }

        private void UpdateAllEffects(float deltaTime)
        {
            if (this.animations != null)
            {
                foreach (Animation animation in this.animations)
                {
                    AnimationState state = animation[animation.clip.name];
                    if (!animation.IsPlaying(animation.clip.name))
                    {
                        animation.Play(animation.clip.name);
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

        public enum EffectCategory
        {
            ServantNoblePhantasm,
            Servant,
            Weapon,
            Common
        }

        public enum EffectTarget
        {
            Actor,
            Target,
            DamageTagets,
            BuffTargets,
            DebuffTargets,
            PlayerParty,
            EnemyParty,
            All
        }
    }
}

