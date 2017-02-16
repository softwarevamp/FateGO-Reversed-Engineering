using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PartyEventPointIndicator : ListViewIndicator
{
    protected IEnumerator dispTotalDropItemCRW;
    protected int eventDropItemDispNum;
    protected EventPartyMargeUpValInfo eventMargeItemUpValInfo;
    protected bool isClick = true;
    [SerializeField]
    protected UIPanel pointEventPanel;
    [SerializeField]
    protected UILabel pointEventTotalDataLabel;
    [SerializeField]
    protected UILabel pointEventTotalTitle1Label;
    [SerializeField]
    protected UILabel pointEventTotalTitle2Label;
    [SerializeField]
    protected Transform pointEventTotalTitleBase;
    protected int titleWidth;

    protected void Awake()
    {
        this.titleWidth = this.pointEventTotalTitle1Label.width;
    }

    [DebuggerHidden]
    private IEnumerator DispTotalDropItemCR() => 
        new <DispTotalDropItemCR>c__Iterator36 { <>f__this = this };

    public void OnClick()
    {
        if (!this.isClick)
        {
            this.isClick = true;
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        }
    }

    protected bool SetTotalDropItem(EventMargeItemUpValInfo dropItemInfo)
    {
        if (dropItemInfo != null)
        {
            string[] strArray = string.Format(dropItemInfo.GetNameTotalString(), dropItemInfo.GetItemName(), dropItemInfo.GetServantName()).Replace("\r", string.Empty).Split(new char[] { '뮿', '﻿', '￾', '\n' }, StringSplitOptions.None);
            this.pointEventTotalTitle1Label.width = 0x3e8;
            this.pointEventTotalTitle2Label.width = 0x3e8;
            float titleWidth = this.titleWidth;
            if (strArray.Length <= 0)
            {
                this.pointEventTotalTitle1Label.text = string.Empty;
                this.pointEventTotalTitle2Label.text = string.Empty;
            }
            else if (strArray.Length == 1)
            {
                this.pointEventTotalTitle1Label.text = string.Empty;
                this.pointEventTotalTitle2Label.text = strArray[0];
                Vector2 printedSize = this.pointEventTotalTitle2Label.printedSize;
                if (printedSize.x > this.titleWidth)
                {
                    titleWidth = printedSize.x;
                }
            }
            else
            {
                this.pointEventTotalTitle1Label.text = strArray[0];
                this.pointEventTotalTitle2Label.text = strArray[1];
                Vector2 vector2 = this.pointEventTotalTitle1Label.printedSize;
                Vector2 vector3 = this.pointEventTotalTitle2Label.printedSize;
                titleWidth = (vector2.x <= vector3.x) ? vector3.x : vector2.x;
            }
            this.pointEventTotalTitleBase.localScale = new Vector3(((((float) this.titleWidth) / titleWidth) <= 1f) ? (((float) this.titleWidth) / titleWidth) : 1f, 1f, 1f);
            string eventUpString = dropItemInfo.GetEventUpString();
            if (!string.IsNullOrEmpty(eventUpString))
            {
                this.pointEventTotalDataLabel.text = string.Format(LocalizationManager.Get("PARTY_ORGANIZATION_EVENT_STATE_UP_TOTAL_DATA"), eventUpString);
            }
            else
            {
                this.pointEventTotalDataLabel.text = string.Empty;
            }
            return true;
        }
        this.pointEventTotalTitleBase.localScale = new Vector3(1f, 1f, 1f);
        this.pointEventTotalTitle1Label.text = string.Empty;
        this.pointEventTotalTitle2Label.text = string.Empty;
        this.pointEventTotalDataLabel.text = string.Empty;
        return false;
    }

    public void SetTotalDropItemList(EventPartyMargeUpValInfo margeItemInfo)
    {
        this.eventMargeItemUpValInfo = margeItemInfo;
        if (this.dispTotalDropItemCRW != null)
        {
            base.StopCoroutine(this.dispTotalDropItemCRW);
            this.dispTotalDropItemCRW = null;
        }
        if ((this.eventMargeItemUpValInfo != null) && !this.eventMargeItemUpValInfo.IsEmpry())
        {
            this.dispTotalDropItemCRW = this.DispTotalDropItemCR();
            base.StartCoroutine(this.dispTotalDropItemCRW);
        }
        else
        {
            this.StopTotalDropItemtweenAlpha();
            this.pointEventPanel.alpha = 0f;
            this.SetTotalDropItem(null);
            this.isClick = true;
        }
    }

    protected void StopTotalDropItemtweenAlpha()
    {
        TweenAlpha component = this.pointEventPanel.GetComponent<TweenAlpha>();
        if ((component != null) && component.enabled)
        {
            component.enabled = false;
        }
    }

    [CompilerGenerated]
    private sealed class <DispTotalDropItemCR>c__Iterator36 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal PartyEventPointIndicator <>f__this;
        internal int <margeItemCount>__1;
        internal EventMargeItemUpValInfo[] <margeItemList>__0;
        internal float <nowTime>__4;
        internal float <startTime>__3;
        internal TweenAlpha <ta>__2;

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
                    this.<>f__this.eventDropItemDispNum = -1;
                    this.<>f__this.StopTotalDropItemtweenAlpha();
                    this.<>f__this.pointEventPanel.alpha = 1f;
                    this.<>f__this.isClick = true;
                    this.<margeItemList>__0 = this.<>f__this.eventMargeItemUpValInfo.GetList();
                    this.<margeItemCount>__1 = this.<margeItemList>__0.Length;
                    if (this.<margeItemCount>__1 <= 1)
                    {
                        if (this.<margeItemCount>__1 == 1)
                        {
                            this.<>f__this.SetTotalDropItem(this.<margeItemList>__0[0]);
                        }
                        this.$PC = -1;
                        goto Label_02F5;
                    }
                    this.<ta>__2 = null;
                    break;

                case 1:
                    goto Label_019F;

                case 2:
                    this.<nowTime>__4 = Time.realtimeSinceStartup;
                    if ((this.<nowTime>__4 - this.<startTime>__3) < 3f)
                    {
                        goto Label_01CF;
                    }
                    goto Label_0227;

                case 3:
                    goto Label_02AF;

                default:
                    goto Label_02F5;
            }
        Label_0098:
            this.<>f__this.StopTotalDropItemtweenAlpha();
            this.<>f__this.eventDropItemDispNum++;
            if (this.<>f__this.eventDropItemDispNum >= this.<margeItemCount>__1)
            {
                this.<>f__this.eventDropItemDispNum = 0;
            }
            this.<>f__this.SetTotalDropItem(this.<margeItemList>__0[this.<>f__this.eventDropItemDispNum]);
            if (this.<>f__this.isClick)
            {
                this.<>f__this.pointEventPanel.alpha = 1f;
                this.<>f__this.isClick = false;
                goto Label_01C4;
            }
            this.<ta>__2 = TweenAlpha.Begin(this.<>f__this.pointEventPanel.gameObject, 0.5f, 1f);
            if (this.<ta>__2 == null)
            {
                goto Label_01AF;
            }
            this.<ta>__2.method = UITweener.Method.EaseOutQuad;
        Label_019F:
            while (this.<ta>__2.enabled)
            {
                if (this.<>f__this.isClick)
                {
                    break;
                }
                this.$current = new WaitForEndOfFrame();
                this.$PC = 1;
                goto Label_02F7;
            }
        Label_01AF:
            if (this.<>f__this.isClick)
            {
                goto Label_0098;
            }
        Label_01C4:
            this.<startTime>__3 = Time.realtimeSinceStartup;
        Label_01CF:
            if (!this.<>f__this.isClick)
            {
                this.$current = new WaitForEndOfFrame();
                this.$PC = 2;
                goto Label_02F7;
            }
        Label_0227:
            if (this.<>f__this.isClick)
            {
                goto Label_0098;
            }
            this.<ta>__2 = TweenAlpha.Begin(this.<>f__this.pointEventPanel.gameObject, 0.5f, 0f);
            if (this.<ta>__2 == null)
            {
                goto Label_0098;
            }
            this.<ta>__2.method = UITweener.Method.EaseOutQuad;
        Label_02AF:
            while (this.<ta>__2.enabled)
            {
                if (this.<>f__this.isClick)
                {
                    break;
                }
                this.$current = new WaitForEndOfFrame();
                this.$PC = 3;
                goto Label_02F7;
            }
            goto Label_0098;
        Label_02F5:
            return false;
        Label_02F7:
            return true;
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
}

