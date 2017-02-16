using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

public class CommonEffectComponent : BaseMonoBehaviour
{
    private Animation animationComponent;
    protected AssetData asset;
    protected string baseName;
    protected string effectName;
    [SerializeField]
    protected float endtime = 5f;
    protected bool isDestroy = true;
    private bool isEndless;
    protected bool isPause;
    protected bool isSkip;
    protected bool isStart;
    [SerializeField]
    protected bool loop;
    [SerializeField]
    protected float losttime = 5f;
    [SerializeField]
    protected ParticleSystem[] particlelist;
    protected string playAnimation;
    protected string requestAnimation;
    protected Status status;
    protected float totaltime;

    protected void Awake()
    {
        this.animationComponent = base.GetComponent<Animation>();
    }

    public void ForceLoop()
    {
        if (this.status != Status.DESTORY)
        {
            if (this.status != Status.INIT)
            {
                if (this.animationComponent != null)
                {
                    AnimationState state = this.animationComponent[this.baseName + "_loop"];
                    if (state != null)
                    {
                        state.wrapMode = WrapMode.Loop;
                    }
                    this.animationComponent.Stop();
                }
                this.loop = true;
                this.totaltime = 0f;
                this.playAnimation = null;
                this.NextPlayAnimation(Status.LOOP);
            }
            else
            {
                this.Init(false, false);
                this.NextPlayAnimation(Status.LOOP);
            }
        }
    }

    public void ForceStart()
    {
        if (this.status != Status.DESTORY)
        {
            if (this.status != Status.INIT)
            {
                if (this.animationComponent != null)
                {
                    AnimationState state = this.animationComponent[this.baseName + "_loop"];
                    if (state != null)
                    {
                        state.wrapMode = WrapMode.Loop;
                    }
                    this.animationComponent.Stop();
                }
                this.loop = true;
                this.totaltime = 0f;
                this.playAnimation = null;
                this.NextPlayAnimation(Status.START);
            }
            else
            {
                this.Init(false, false);
            }
        }
    }

    public void Init(bool isSkip = false, bool isPause = false)
    {
        if (this.status == Status.INIT)
        {
            if (this.effectName == null)
            {
                this.effectName = base.gameObject.name;
                if (this.effectName.EndsWith("(Clone)"))
                {
                    this.effectName = this.effectName.Substring(0, this.effectName.Length - 7);
                }
            }
            if (this.baseName == null)
            {
                this.baseName = this.effectName;
                int num = this.baseName.LastIndexOf('/');
                if (num >= 0)
                {
                    this.baseName = this.baseName.Substring(num + 1);
                }
            }
            base.gameObject.name = "Effect(" + this.effectName + ")";
            if (base.transform.parent != null)
            {
                this.SetChildInit(base.transform, base.transform.parent.gameObject.layer);
            }
            this.isStart = true;
            this.isSkip = isSkip;
            this.isPause = isPause;
            if (this.isPause)
            {
                this.status = Status.PAUSE;
            }
            else if (this.isSkip)
            {
                if (this.loop)
                {
                    this.NextPlayAnimation(Status.LOOP);
                }
                else
                {
                    UnityEngine.Object.Destroy(base.gameObject);
                }
            }
            else
            {
                this.NextPlayAnimation(Status.START);
            }
        }
    }

    public void Init(AssetData data, bool isSkip = false, bool isPause = false)
    {
        if (this.status == Status.INIT)
        {
            this.asset = data;
            this.effectName = data.Name;
            this.Init(isSkip, isPause);
        }
    }

    public void Init(string effectName, bool isSkip = false, bool isPause = false)
    {
        if (this.status == Status.INIT)
        {
            this.effectName = effectName;
            this.Init(isSkip, isPause);
        }
    }

    protected void NextPlayAnimation(Status next)
    {
        if (this.animationComponent != null)
        {
            AnimationState state = null;
            Debug.Log("Animation count " + this.animationComponent.GetClipCount());
            IEnumerator enumerator = this.animationComponent.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    AnimationState current = (AnimationState) enumerator.Current;
                    Debug.Log("AnimationState [" + current.name + "]");
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable == null)
                {
                }
                disposable.Dispose();
            }
            if (next == Status.START)
            {
                state = this.animationComponent[this.baseName + "_start"];
                if (state == null)
                {
                    state = this.animationComponent[this.baseName];
                }
                if (state == null)
                {
                    next = Status.LOOP;
                }
                else
                {
                    this.endtime = 0f;
                }
            }
            if (next == Status.LOOP)
            {
                if (this.loop)
                {
                    state = this.animationComponent[this.baseName + "_loop"];
                    if (state == null)
                    {
                        next = Status.END;
                    }
                }
                else
                {
                    next = Status.END;
                }
            }
            if (next == Status.END)
            {
                state = this.animationComponent[this.baseName + "_end"];
            }
            this.requestAnimation = (state == null) ? string.Empty : state.name;
        }
        else
        {
            if ((next == Status.LOOP) && !this.loop)
            {
                next = Status.END;
            }
            this.requestAnimation = string.Empty;
        }
        this.status = next;
    }

    protected void onDesyory()
    {
        if (this.asset != null)
        {
            AssetManager.releaseAsset(this.asset);
            this.asset = null;
        }
        this.status = Status.DESTORY;
    }

    protected void PlaySe(string name)
    {
        char[] separator = new char[] { ':' };
        string[] strArray = name.Split(separator);
        if (strArray.Length >= 2)
        {
            SoundManager.playSe(strArray[0], strArray[1]);
        }
        else
        {
            SoundManager.playSe(strArray[0]);
        }
    }

    public void Resume(bool isSkip)
    {
        this.isSkip = isSkip;
        if (this.isStart)
        {
            if (this.isPause)
            {
                this.isPause = false;
                this.ForceStart();
            }
        }
        else
        {
            this.isPause = false;
        }
    }

    protected void SetChildInit(Transform tf, int layer)
    {
        if (tf.gameObject.layer != layer)
        {
            tf.gameObject.layer = layer;
            IEnumerator enumerator = tf.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Transform current = (Transform) enumerator.Current;
                    this.SetChildInit(current, layer);
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable == null)
                {
                }
                disposable.Dispose();
            }
        }
    }

    public void SetEndlessEnable(bool is_enable)
    {
        this.isEndless = is_enable;
    }

    public virtual void SetParam(object param)
    {
    }

    protected void Start()
    {
        this.Init(false, false);
    }

    public void Stop(bool isDestroy = true)
    {
        if (this.status != Status.DESTORY)
        {
            this.loop = false;
            this.isDestroy = isDestroy;
            if (this.animationComponent != null)
            {
                AnimationState state = this.animationComponent[this.baseName + "_loop"];
                if (state != null)
                {
                    state.wrapMode = WrapMode.Once;
                }
            }
        }
    }

    protected void Update()
    {
        if (((this.status != Status.INIT) && (this.status != Status.PAUSE)) && (this.status != Status.DESTORY))
        {
            this.totaltime += RealTime.deltaTime;
            if (this.requestAnimation == null)
            {
                if ((this.animationComponent != null) && (this.playAnimation != null))
                {
                    if (this.animationComponent.isPlaying)
                    {
                        return;
                    }
                    if (this.status == Status.END)
                    {
                        this.playAnimation = null;
                    }
                }
                switch (this.status)
                {
                    case Status.START:
                        if (this.endtime <= this.totaltime)
                        {
                            this.NextPlayAnimation(Status.LOOP);
                            break;
                        }
                        return;

                    case Status.LOOP:
                        this.NextPlayAnimation(Status.LOOP);
                        break;

                    case Status.END:
                        if (!this.isEndless)
                        {
                            if (((this.endtime + this.losttime) < this.totaltime) && this.isDestroy)
                            {
                                UnityEngine.Object.Destroy(base.gameObject);
                            }
                            return;
                        }
                        break;
                }
            }
            if ((this.requestAnimation != null) && ((this.animationComponent == null) || !this.animationComponent.isPlaying))
            {
                if (this.requestAnimation != string.Empty)
                {
                    this.animationComponent.Play(this.requestAnimation);
                    this.playAnimation = this.requestAnimation;
                }
                else
                {
                    this.playAnimation = null;
                }
                if (this.status == Status.END)
                {
                    if (this.particlelist != null)
                    {
                        foreach (ParticleSystem system in this.particlelist)
                        {
                            if (system != null)
                            {
                                system.Stop();
                            }
                        }
                    }
                    if (this.totaltime > this.endtime)
                    {
                        this.endtime = this.totaltime;
                    }
                }
                this.requestAnimation = null;
            }
        }
    }

    public string EffectName =>
        this.effectName;

    public bool IsStart =>
        this.isStart;

    protected enum Status
    {
        INIT,
        PAUSE,
        DESTORY,
        START,
        LOOP,
        END
    }
}

