using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode, AddComponentMenu("NGUI/UI/Sprite Animation"), RequireComponent(typeof(UISprite))]
public class UISpriteAnimation : MonoBehaviour
{
    protected bool mActive = true;
    protected float mDelta;
    [HideInInspector, SerializeField]
    protected int mFPS = 30;
    protected int mIndex;
    [HideInInspector, SerializeField]
    protected bool mLoop = true;
    [HideInInspector, SerializeField]
    protected string mPrefix = string.Empty;
    [HideInInspector, SerializeField]
    protected bool mSnap = true;
    protected UISprite mSprite;
    protected List<string> mSpriteNames = new List<string>();

    public void Pause()
    {
        this.mActive = false;
    }

    public void Play()
    {
        this.mActive = true;
    }

    public void RebuildSpriteList()
    {
        if (this.mSprite == null)
        {
            this.mSprite = base.GetComponent<UISprite>();
        }
        this.mSpriteNames.Clear();
        if ((this.mSprite != null) && (this.mSprite.atlas != null))
        {
            List<UISpriteData> spriteList = this.mSprite.atlas.spriteList;
            int num = 0;
            int count = spriteList.Count;
            while (num < count)
            {
                UISpriteData data = spriteList[num];
                if (string.IsNullOrEmpty(this.mPrefix) || data.name.StartsWith(this.mPrefix))
                {
                    this.mSpriteNames.Add(data.name);
                }
                num++;
            }
            this.mSpriteNames.Sort();
        }
    }

    public void ResetToBeginning()
    {
        this.mActive = true;
        this.mIndex = 0;
        if ((this.mSprite != null) && (this.mSpriteNames.Count > 0))
        {
            this.mSprite.spriteName = this.mSpriteNames[this.mIndex];
            if (this.mSnap)
            {
                this.mSprite.MakePixelPerfect();
            }
        }
    }

    protected virtual void Start()
    {
        this.RebuildSpriteList();
    }

    protected virtual void Update()
    {
        if ((this.mActive && (this.mSpriteNames.Count > 1)) && (Application.isPlaying && (this.mFPS > 0)))
        {
            this.mDelta += RealTime.deltaTime;
            float num = 1f / ((float) this.mFPS);
            if (num < this.mDelta)
            {
                this.mDelta = (num <= 0f) ? 0f : (this.mDelta - num);
                if (++this.mIndex >= this.mSpriteNames.Count)
                {
                    this.mIndex = 0;
                    this.mActive = this.mLoop;
                }
                if (this.mActive)
                {
                    this.mSprite.spriteName = this.mSpriteNames[this.mIndex];
                    if (this.mSnap)
                    {
                        this.mSprite.MakePixelPerfect();
                    }
                }
            }
        }
    }

    public int frames =>
        this.mSpriteNames.Count;

    public int framesPerSecond
    {
        get => 
            this.mFPS;
        set
        {
            this.mFPS = value;
        }
    }

    public bool isPlaying =>
        this.mActive;

    public bool loop
    {
        get => 
            this.mLoop;
        set
        {
            this.mLoop = value;
        }
    }

    public string namePrefix
    {
        get => 
            this.mPrefix;
        set
        {
            if (this.mPrefix != value)
            {
                this.mPrefix = value;
                this.RebuildSpriteList();
            }
        }
    }
}

