using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class NGUIFader : MonoBehaviour
{
    public Color color = Color.black;
    public float duration = 1f;
    private float durTime;
    private UIWidget fadeWidget;
    private OnFinished finishedCallback;
    private bool isFadeIn;
    private bool isFading;
    private float offsetTime;

    public void FadeStart(Color col, float dur, bool isIn, OnFinished callback, bool noSetColor = false)
    {
        this.finishedCallback = callback;
        if (!this.isFading)
        {
            if (!noSetColor)
            {
                this.color = col;
            }
            this.duration = dur;
            this.isFadeIn = isIn;
            if (isIn)
            {
                if (this.fadeWidget.color.a == 0f)
                {
                    if (this.finishedCallback != null)
                    {
                        this.finishedCallback();
                    }
                    return;
                }
                this.fadeWidget.color = this.color;
                this.color = new Color(this.color.r, this.color.g, this.color.b, 1f);
            }
            else if (this.fadeWidget.color.a == 1f)
            {
                if (this.finishedCallback != null)
                {
                    this.finishedCallback();
                }
                return;
            }
            this.offsetTime = 0f;
            this.durTime = dur;
            this.isFading = true;
        }
    }

    public bool isFaded()
    {
        if (this.color.a < 0.01f)
        {
            this.color.a = 0f;
            this.isFading = false;
        }
        return (this.color.a != 0f);
    }

    public void setColor(Color col)
    {
        this.color = col;
        this.setup();
        this.fadeWidget.color = col;
    }

    private void setup()
    {
        if (this.fadeWidget == null)
        {
            this.fadeWidget = base.gameObject.GetComponent<UIWidget>();
            this.color = this.fadeWidget.color;
            this.color.a = 0f;
            this.isFadeIn = true;
        }
    }

    private void Start()
    {
        this.setup();
    }

    private void Update()
    {
        this.updateColor(Time.deltaTime);
    }

    public void updateColor(float deltaTime)
    {
        this.offsetTime += deltaTime;
        if (this.isFading)
        {
            float num = 0f;
            if (this.offsetTime > this.durTime)
            {
                num = 1f;
                this.isFading = false;
            }
            else if (this.offsetTime > 0f)
            {
                num = this.offsetTime / this.durTime;
            }
            Color color = this.color;
            if (this.isFadeIn)
            {
                color.a = 1f - num;
            }
            else
            {
                color.a = num;
            }
            this.fadeWidget.color = color;
            if (!this.isFading && (this.finishedCallback != null))
            {
                this.finishedCallback();
            }
        }
    }

    public delegate void OnFinished();
}

