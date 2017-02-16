using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

[ExecuteInEditMode]
public class ScrollPageIndicator : MonoBehaviour
{
    [SerializeField]
    protected UISprite[] Indicator;
    [SerializeField]
    protected UIAtlas IndicatorAtlas;
    [SerializeField]
    protected int IndicatorStep = 0x30;
    protected int nowIndex;
    [SerializeField]
    protected string OffIndicatorSpriteName;
    [SerializeField]
    protected string OnIndicatorSpriteName;
    public OnPageChangeCallback onPageChange;
    [SerializeField]
    protected GameObject PageIndicatorPrefab;

    public void CreateIndicator(int count = 3)
    {
        if (count <= 1)
        {
            this.Init();
        }
        else
        {
            this.Indicator = new UISprite[count];
            int indicatorStep = this.IndicatorStep;
            base.transform.localPosition = new Vector3(((float) -((count - 1) * indicatorStep)) / 2f, base.transform.localPosition.y, base.transform.localPosition.z);
            this.nowIndex = 0;
            for (int i = 0; i < count; i++)
            {
                <CreateIndicator>c__AnonStorey5B storeyb = new <CreateIndicator>c__AnonStorey5B {
                    <>f__this = this
                };
                GameObject go = UnityEngine.Object.Instantiate<GameObject>(this.PageIndicatorPrefab);
                go.name = "indicator" + (i + 1);
                go.transform.parent = base.transform;
                go.transform.localPosition = new Vector3((float) (i * indicatorStep), 0f, 0f);
                go.transform.localScale = Vector3.one;
                UISprite component = go.GetComponent<UISprite>();
                this.Indicator[i] = component;
                component.atlas = this.IndicatorAtlas;
                component.spriteName = (i != 0) ? this.OffIndicatorSpriteName : this.OnIndicatorSpriteName;
                component.MakePixelPerfect();
                NGUITools.SetLayer(go, base.gameObject.layer);
                UIButton button = go.GetComponent<UIButton>();
                button.SetState(UIButtonColor.State.Normal, false);
                button.tweenTarget = null;
                storeyb.idx = i;
                EventDelegate item = new EventDelegate(new EventDelegate.Callback(storeyb.<>m__25));
                button.onClick.Add(item);
            }
        }
    }

    public void Init()
    {
        if (this.Indicator != null)
        {
            foreach (UISprite sprite in this.Indicator)
            {
                if (sprite != null)
                {
                    UnityEngine.Object.Destroy(sprite.gameObject);
                }
            }
            this.Indicator = null;
        }
    }

    private void Start()
    {
    }

    private void Update()
    {
    }

    public void UpdateIndicator(int idx)
    {
        if (this.Indicator != null)
        {
            if ((this.nowIndex >= 0) && (this.nowIndex < this.Indicator.Length))
            {
                this.Indicator[this.nowIndex].spriteName = this.OffIndicatorSpriteName;
            }
            if ((idx >= 0) && (idx < this.Indicator.Length))
            {
                this.Indicator[idx].spriteName = this.OnIndicatorSpriteName;
            }
            this.nowIndex = idx;
        }
    }

    [CompilerGenerated]
    private sealed class <CreateIndicator>c__AnonStorey5B
    {
        internal ScrollPageIndicator <>f__this;
        internal int idx;

        internal void <>m__25()
        {
            if (this.<>f__this.onPageChange != null)
            {
                this.<>f__this.onPageChange(this.idx);
            }
        }
    }

    public delegate void OnPageChangeCallback(int pageIndex);
}

