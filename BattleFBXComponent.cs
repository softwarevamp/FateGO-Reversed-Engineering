using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class BattleFBXComponent : MonoBehaviour
{
    [CompilerGenerated]
    private static Func<Transform, bool> <>f__am$cache1C;
    [CompilerGenerated]
    private static Func<Transform, bool> <>f__am$cache1D;
    private Animation animationComponent;
    public string animename;
    public static readonly float animFps = 30f;
    private BillBoard billBoard;
    private static readonly string chrnodeMarker = "chrnode_";
    private Dictionary<string, string> CommonAnimNames;
    public onEventDelegate Complete;
    private string currentAnimName;
    private float currentAnimTime;
    private string currentCommonAnimName;
    private int currentEventIndex;
    public static bool EnableEvent = true;
    private static readonly string enItemMarker = "en_item_";
    private static readonly string enWeaponMarker = "en_weapon_";
    private AnimEvent[][] eventlist;
    public bool isAutoPlay;
    private bool isDirty;
    private bool isStop;
    public static readonly string levelMarker = "_level";
    private static readonly string npPathHead = "Assets/AssetStorages/NoblePhantasm/";
    public onEventDelegate OnEvent;
    private static readonly string prefabMarker = "prefabn_";
    protected List<GameObject> PrefabNodes;
    [SerializeField]
    private Dictionary<string, string> RealAnimNames;
    private Transform rootTransform;
    private static readonly string sideflipMarker = "joint_sideflip_";
    private static readonly string svtPathHead = "Assets/AssetStorages/Servants/";
    public float timescale = 1f;

    [DebuggerHidden]
    private IEnumerator AnimationCheck() => 
        new <AnimationCheck>c__Iterator17 { <>f__this = this };

    public void AnimUpdate(float deltaTime)
    {
        float num = deltaTime * this.timescale;
        if (((num != 0f) && !this.isStop) && (((this.rootTransform != null) && this.animationComponent.isPlaying) && ((this.currentAnimName != null) && (this.animationComponent[this.currentAnimName] != null))))
        {
            int num2 = (int) (this.currentAnimTime * 30f);
            if (EnableEvent && (this.eventlist != null))
            {
                int index = AnimationList.getIndex(this.currentCommonAnimName);
                if (index >= 0)
                {
                    AnimEvent[] eventArray = this.eventlist[index];
                    if (eventArray != null)
                    {
                        for (int i = this.currentEventIndex; i < eventArray.Length; i++)
                        {
                            AnimEvent ev = eventArray[i];
                            if ((ev.time >= this.currentAnimTime) && (ev.time < (this.currentAnimTime + num)))
                            {
                                this.animationComponent[this.currentAnimName].time = ev.time;
                                this.animationComponent.Sample();
                                bool isDirty = this.isDirty;
                                this.isDirty = false;
                                if (this.billBoard != null)
                                {
                                    this.billBoard.UpdateBillboard();
                                }
                                this.OnAnimEvent(ev);
                                bool flag2 = this.isDirty;
                                this.isDirty |= isDirty;
                                if (this.isDirty)
                                {
                                    if (!flag2)
                                    {
                                        this.currentAnimTime = ev.time;
                                    }
                                    this.isDirty = false;
                                    return;
                                }
                            }
                            else if (ev.time >= (this.currentAnimTime + num))
                            {
                                this.currentEventIndex = i;
                                break;
                            }
                        }
                    }
                }
            }
            this.animationComponent[this.currentAnimName].time = ((float) num2) / 30f;
            this.currentAnimTime += num;
            if (this.currentAnimTime >= this.animationComponent[this.currentAnimName].length)
            {
                if (this.animationComponent[this.currentAnimName].wrapMode == WrapMode.Loop)
                {
                    while (this.currentAnimTime >= this.animationComponent[this.currentAnimName].length)
                    {
                        this.currentAnimTime -= this.animationComponent[this.currentAnimName].length;
                    }
                    this.currentEventIndex = 0;
                }
                else
                {
                    this.animationComponent.Stop();
                }
            }
        }
    }

    public void AttachCl()
    {
    }

    private void Awake()
    {
        this.InitAnimNameDict();
        if (this.isAutoPlay)
        {
            this.playAnimation(this.animename);
        }
    }

    public void ChangeActorLimitCount()
    {
        BattleActorControl component = base.gameObject.GetComponent<BattleActorControl>();
        if (component != null)
        {
            int svtId = component.getServantId();
            int limitcount = component.getLimitCount();
            this.SetEvolutionLevel(svtId, limitcount);
        }
    }

    public bool checkAnimation(string filename)
    {
        if (this.RealAnimNames.ContainsKey(filename))
        {
            filename = this.RealAnimNames[filename];
        }
        return (this.animationComponent[filename] != null);
    }

    protected GameObject CreatePrefabNode(Transform node, int svtId = -1, int limitcount = -1)
    {
        char[] separator = new char[] { '_' };
        string name = node.name.Split(separator)[1];
        GameObject original = ServantAssetLoadManager.loadActorEffectFromActor(svtId, limitcount, 0, name);
        if (original != null)
        {
            GameObject item = UnityEngine.Object.Instantiate<GameObject>(original);
            if (item != null)
            {
                ObjectScaleEnabler enabler = item.AddComponent<ObjectScaleEnabler>();
                item.SetActive(false);
                enabler.visibleCheckTarget = node;
                item.transform.parent = node.parent.transform;
                item.transform.localPosition = Vector3.zero;
                item.transform.localEulerAngles = Vector3.zero;
                item.transform.localScale = Vector3.one;
                enabler.OnUpdate();
                item.SetActive(true);
                this.PrefabNodes.Add(item);
                return item;
            }
        }
        return null;
    }

    private void DebugPrint(string str)
    {
    }

    protected void DestroyPrefabNode()
    {
        if (this.PrefabNodes != null)
        {
            foreach (GameObject obj2 in this.PrefabNodes)
            {
                UnityEngine.Object.Destroy(obj2);
            }
            this.PrefabNodes.Clear();
        }
    }

    public float getLength()
    {
        if (((this.rootTransform != null) && (this.currentAnimName != string.Empty)) && ((this.animationComponent != null) && (this.animationComponent[this.currentAnimName] != null)))
        {
            return this.animationComponent[this.currentAnimName].length;
        }
        return 0f;
    }

    public GameObject GetPrefabNode(string name)
    {
        foreach (GameObject obj2 in this.PrefabNodes)
        {
            string str = obj2.transform.name;
            string str2 = "(Clone)";
            if (str.EndsWith(str2))
            {
                str = str.Substring(0, str.Length - str2.Length);
            }
            if (str.Equals(name))
            {
                return obj2;
            }
        }
        return null;
    }

    public float getTagTime(string animname, string tag)
    {
        int index = AnimationList.getIndex(animname);
        AnimEvent[] eventArray = this.eventlist[index];
        foreach (AnimEvent event2 in eventArray)
        {
            if (event2.tag.Equals(tag))
            {
                return event2.time;
            }
        }
        return 0f;
    }

    protected void InitAnimNameDict()
    {
        this.RealAnimNames = new Dictionary<string, string>();
        this.CommonAnimNames = new Dictionary<string, string>();
    }

    public void inSetEvolutionLevel(GameObject gameObject, int svtId, int limitcount)
    {
        BattleActorControl control = null;
        this.DestroyPrefabNode();
        this.PrefabNodes = new List<GameObject>();
        Transform[] componentsInChildren = gameObject.GetComponentsInChildren<Transform>(true);
        if (<>f__am$cache1C == null)
        {
            <>f__am$cache1C = p => p.gameObject.name.Contains(levelMarker) || p.gameObject.name.Contains(sideflipMarker);
        }
        IEnumerable<Transform> enumerable = componentsInChildren.Where<Transform>(<>f__am$cache1C);
        int num = ServantAssetLoadManager.GetLimitImageIndex(svtId, limitcount) + 1;
        Dictionary<string, SkinnedMeshRenderer> dictionary = new Dictionary<string, SkinnedMeshRenderer>();
        bool flag = true;
        IEnumerator<Transform> enumerator = enumerable.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                Transform current = enumerator.Current;
                bool flag2 = false;
                if (current.name.StartsWith(sideflipMarker))
                {
                    control = base.gameObject.GetComponent<BattleActorControl>();
                    if (control != null)
                    {
                        current.transform.localScale = new Vector3(!control.IsEnemy ? 1f : -1f, 1f, 1f);
                    }
                    continue;
                }
                string str = current.name.Substring(0, current.name.IndexOf(levelMarker));
                int startIndex = current.name.IndexOf(levelMarker) + 7;
                string str2 = current.name.Substring(startIndex);
                if (str2.IndexOf(" ") >= 0)
                {
                    str2 = str2.Substring(0, str2.IndexOf(" "));
                }
                char[] separator = new char[] { '_' };
                foreach (string str3 in str2.Split(separator))
                {
                    int result = 0x63;
                    if (int.TryParse(str3, out result) && (int.Parse(str3) == num))
                    {
                        flag2 = true;
                        break;
                    }
                }
                if (flag2 && current.name.StartsWith(prefabMarker))
                {
                    this.CreatePrefabNode(current, svtId, limitcount);
                }
                else if (flag2 && current.name.StartsWith(chrnodeMarker))
                {
                    this.CreatePrefabNode(current, svtId, limitcount);
                }
                SkinnedMeshRenderer component = current.GetComponent<SkinnedMeshRenderer>();
                if (component != null)
                {
                    current.gameObject.layer = LayerMask.NameToLayer("Battle2D");
                    dictionary[str] = component;
                    component.enabled = flag2;
                    if (flag2)
                    {
                        flag = false;
                    }
                }
            }
        }
        finally
        {
            if (enumerator == null)
            {
            }
            enumerator.Dispose();
        }
        if (flag)
        {
            foreach (SkinnedMeshRenderer renderer2 in dictionary.Values)
            {
                renderer2.enabled = true;
            }
        }
        if (<>f__am$cache1D == null)
        {
            <>f__am$cache1D = p => p.gameObject.name.Contains(enItemMarker);
        }
        IEnumerator<Transform> enumerator3 = componentsInChildren.Where<Transform>(<>f__am$cache1D).GetEnumerator();
        try
        {
            while (enumerator3.MoveNext())
            {
                Transform transform2 = enumerator3.Current;
                string str4 = transform2.name.Substring(enItemMarker.Length);
                Transform transform4 = transform2.parent.FindChild(enWeaponMarker + str4);
                if (transform4 != null)
                {
                    LookAtTarget target = transform2.gameObject.AddComponent<LookAtTarget>();
                    target.target = transform4;
                    target.speed = 100f;
                }
            }
        }
        finally
        {
            if (enumerator3 == null)
            {
            }
            enumerator3.Dispose();
        }
    }

    public void loadAnimationEvents(TextAsset data, int svtId, int level)
    {
        this.InitAnimNameDict();
        this.eventlist = new AnimEvent[0x18][];
        int num = ServantAssetLoadManager.GetLimitImageIndex(svtId, level) + 1;
        ServantMaster master = (ServantMaster) SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT);
        if (master.getEntityFromId<ServantEntity>(svtId).type == 4)
        {
            num = 1;
        }
        char[] separator = new char[] { '\r', '\n' };
        string[] strArray = data.text.Split(separator);
        for (int i = 0; i < strArray.Length; i++)
        {
            char[] chArray2 = new char[] { ","[0] };
            string[] strArray2 = strArray[i].Split(chArray2);
            if (((strArray2.Length <= 0) || !strArray2[0].StartsWith("#")) && (strArray2.Length > 5))
            {
                int num3 = int.Parse(strArray2[0]);
                string str = strArray2[1];
                if (num3 == num)
                {
                    string inname = str;
                    bool flag = str.IndexOf(levelMarker) >= 0;
                    if (flag)
                    {
                        inname = str.Substring(0, str.IndexOf(levelMarker));
                    }
                    int index = AnimationList.getIndex(inname);
                    if (flag)
                    {
                        this.RealAnimNames[inname] = str;
                        this.CommonAnimNames[str] = inname;
                    }
                    List<AnimEvent> list = new List<AnimEvent>();
                    if (this.eventlist[index] == null)
                    {
                        for (int j = 2; j < strArray2.Length; j += 3)
                        {
                            if (strArray2[j].Length == 0)
                            {
                                break;
                            }
                            float num6 = float.Parse(strArray2[j]) / animFps;
                            string str3 = strArray2[j + 1];
                            string str4 = strArray2[j + 2];
                            AnimEvent item = new AnimEvent {
                                time = num6,
                                tag = str3,
                                param = str4
                            };
                            list.Add(item);
                        }
                    }
                    this.eventlist[index] = list.ToArray();
                }
            }
        }
    }

    private void OnAnimEvent(AnimEvent ev)
    {
        if (EnableEvent && (this.OnEvent != null))
        {
            this.OnEvent(ev.tag);
        }
    }

    private void OnAnimEvent(AnimationEvent ev)
    {
        if (EnableEvent)
        {
            char[] separator = new char[] { ':' };
            string[] strArray = ev.stringParameter.Split(separator);
            this.OnEvent(strArray[0]);
        }
    }

    private void OnEnable()
    {
        if ((this.animename != null) && this.animename.StartsWith("wait"))
        {
            this.playAnimation("wait");
        }
    }

    public void playAnimation(string filename)
    {
        this.DebugPrint(filename);
        this.playAnimationTimeline(filename, string.Empty, string.Empty);
        base.StopCoroutine("AnimationCheck");
        base.StartCoroutine("AnimationCheck");
    }

    public void playAnimationFromTag(string animname, string tag)
    {
        this.playAnimation(animname);
        animname = this.CurrentAnimName;
        this.isDirty = true;
        float num = this.getTagTime(this.currentCommonAnimName, tag);
        this.animationComponent[animname].time = num;
        this.animationComponent[animname].speed = 0f;
        this.currentAnimName = animname;
        this.currentAnimTime = num;
        this.currentEventIndex = 0;
        this.isStop = false;
    }

    public void playAnimationTimeline(string filename, string startEvent = "", string stopEvent = "")
    {
        this.currentCommonAnimName = filename;
        if ((this.RealAnimNames != null) && this.RealAnimNames.ContainsKey(filename))
        {
            filename = this.RealAnimNames[filename];
        }
        this.setupAnimation();
        this.isDirty = true;
        this.currentAnimName = filename;
        this.currentAnimTime = 0f;
        this.currentEventIndex = 0;
        this.isStop = false;
        this.animationComponent.Play(filename);
        this.animationComponent[filename].speed = 0f;
        this.animename = filename;
        if ((startEvent != null) && (startEvent.Length > 0))
        {
            this.currentAnimTime = this.getTagTime(filename, startEvent);
            this.AnimUpdate(0f);
        }
    }

    public void SetConnectPrefabNode(string name, bool isConnect = true)
    {
        GameObject prefabNode = this.GetPrefabNode(name);
        if (prefabNode != null)
        {
            this.SetConnectPrefabNode(prefabNode, isConnect);
        }
    }

    public void SetConnectPrefabNode(GameObject obj, bool isConnect = true)
    {
        if (isConnect)
        {
            ObjectScaleEnabler component = obj.GetComponent<ObjectScaleEnabler>();
            if (component != null)
            {
                Transform visibleCheckTarget = component.visibleCheckTarget;
                obj.transform.parent = visibleCheckTarget.parent.transform;
                obj.transform.localScale = Vector3.one;
                BattleActorControl control = visibleCheckTarget.GetComponent<BattleActorControl>();
                if (control != null)
                {
                    control.AddChildNodesRenderer(obj);
                }
            }
        }
        else
        {
            obj.transform.parent = base.transform.parent;
            BattleActorControl control2 = base.gameObject.GetComponent<BattleActorControl>();
            if (control2 != null)
            {
                control2.RemoveChildNodesRenderer(obj);
            }
        }
    }

    public void setCurrentAnimTime(float time)
    {
        this.currentAnimTime = time;
    }

    public void SetEvolutionLevel(int svtId, int limitcount)
    {
        this.inSetEvolutionLevel(this.rootTransform.gameObject, svtId, limitcount);
    }

    public void setTimeScale(float timescale)
    {
        this.timescale = timescale;
    }

    private void setupAnimation()
    {
        if (this.rootTransform == null)
        {
            this.RootTransform = base.transform.FindChild("chr");
            EnableEvent = false;
        }
        if (this.rootTransform == null)
        {
            this.RootTransform = base.transform;
        }
    }

    public void SetWrapMode(string animName, WrapMode wrapMode)
    {
        this.setupAnimation();
        AnimationState state = this.animationComponent[animName];
        if (state != null)
        {
            state.wrapMode = wrapMode;
        }
    }

    private void Start()
    {
        this.billBoard = base.gameObject.GetComponent<BillBoard>();
    }

    public void stopAnimation()
    {
        this.isStop = true;
    }

    public void stopAnimationCheck()
    {
        base.StopCoroutine("AnimationCheck");
    }

    public void stopParticle()
    {
        foreach (ParticleSystem system in base.gameObject.GetComponentsInChildren<ParticleSystem>())
        {
            system.Stop();
        }
    }

    private void Update()
    {
        this.AnimUpdate(Time.deltaTime);
    }

    public string CurrentAnimName =>
        this.currentAnimName;

    public float CurrentAnimTime =>
        this.currentAnimTime;

    public Transform RootTransform
    {
        get => 
            this.rootTransform;
        set
        {
            this.rootTransform = value;
            this.animationComponent = this.rootTransform.gameObject.GetComponent<Animation>();
        }
    }

    [CompilerGenerated]
    private sealed class <AnimationCheck>c__Iterator17 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal BattleFBXComponent <>f__this;
        internal string <compname>__1;
        internal string <prevname>__0;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                    this.<prevname>__0 = this.<>f__this.animename;
                    break;

                case 1:
                    break;

                case 2:
                    if (this.<>f__this.Complete != null)
                    {
                        this.<compname>__1 = this.<>f__this.animename;
                        if (this.<>f__this.CommonAnimNames.ContainsKey(this.<compname>__1))
                        {
                            this.<compname>__1 = this.<>f__this.CommonAnimNames[this.<compname>__1];
                        }
                        this.<>f__this.Complete(this.<compname>__1);
                    }
                    this.$PC = -1;
                    goto Label_00F0;

                default:
                    goto Label_00F0;
            }
            if (this.<>f__this.animationComponent.isPlaying)
            {
                this.$current = null;
                this.$PC = 1;
            }
            else
            {
                this.$current = 0;
                this.$PC = 2;
            }
            return true;
        Label_00F0:
            return false;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current =>
            this.$current;

        object IEnumerator.Current =>
            this.$current;
    }

    private class AnimEvent
    {
        public string param;
        public string tag;
        public float time;
    }

    public delegate void onEventDelegate(string n);
}

