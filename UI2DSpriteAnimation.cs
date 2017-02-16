using System;
using UnityEngine;

public class UI2DSpriteAnimation : MonoBehaviour
{
    [SerializeField]
    protected int framerate = 20;
    public UnityEngine.Sprite[] frames;
    public bool ignoreTimeScale = true;
    public bool loop = true;
    private int mIndex;
    private UI2DSprite mNguiSprite;
    private SpriteRenderer mUnitySprite;
    private float mUpdate;

    public void Pause()
    {
        base.enabled = false;
    }

    public void Play()
    {
        if ((this.frames != null) && (this.frames.Length > 0))
        {
            if (!base.enabled && !this.loop)
            {
                int num = (this.framerate <= 0) ? (this.mIndex - 1) : (this.mIndex + 1);
                if ((num < 0) || (num >= this.frames.Length))
                {
                    this.mIndex = (this.framerate >= 0) ? 0 : (this.frames.Length - 1);
                }
            }
            base.enabled = true;
            this.UpdateSprite();
        }
    }

    public void ResetToBeginning()
    {
        this.mIndex = (this.framerate >= 0) ? 0 : (this.frames.Length - 1);
        this.UpdateSprite();
    }

    private void Start()
    {
        this.Play();
    }

    private void Update()
    {
        if ((this.frames == null) || (this.frames.Length == 0))
        {
            base.enabled = false;
        }
        else if (this.framerate != 0)
        {
            float num = !this.ignoreTimeScale ? Time.time : RealTime.time;
            if (this.mUpdate < num)
            {
                this.mUpdate = num;
                int val = (this.framerate <= 0) ? (this.mIndex - 1) : (this.mIndex + 1);
                if (!this.loop && ((val < 0) || (val >= this.frames.Length)))
                {
                    base.enabled = false;
                }
                else
                {
                    this.mIndex = NGUIMath.RepeatIndex(val, this.frames.Length);
                    this.UpdateSprite();
                }
            }
        }
    }

    private void UpdateSprite()
    {
        if ((this.mUnitySprite == null) && (this.mNguiSprite == null))
        {
            this.mUnitySprite = base.GetComponent<SpriteRenderer>();
            this.mNguiSprite = base.GetComponent<UI2DSprite>();
            if ((this.mUnitySprite == null) && (this.mNguiSprite == null))
            {
                base.enabled = false;
                return;
            }
        }
        float num = !this.ignoreTimeScale ? Time.time : RealTime.time;
        if (this.framerate != 0)
        {
            this.mUpdate = num + Mathf.Abs((float) (1f / ((float) this.framerate)));
        }
        if (this.mUnitySprite != null)
        {
            this.mUnitySprite.sprite = this.frames[this.mIndex];
        }
        else if (this.mNguiSprite != null)
        {
            this.mNguiSprite.nextSprite = this.frames[this.mIndex];
        }
    }

    public int framesPerSecond
    {
        get => 
            this.framerate;
        set
        {
            this.framerate = value;
        }
    }

    public bool isPlaying =>
        base.enabled;
}

