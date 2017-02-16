using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class TerminalBannerComponent : MonoBehaviour
{
    public static readonly float BANNER_AUTO_MOVE_TIME_INTERVAL = 3f;
    public static readonly float BANNER_AUTO_MOVE_TIME_START = 4f;
    public static readonly string BANNER_EXT = ".png";
    public static readonly float BANNER_INTERVAL_POS = 480f;
    public static readonly string BANNER_PATH = "/banners/terminal/";
    public static readonly string BANNER_PREFIX = "terminal_banner_";
    public static readonly int BANNER_RETRY_MAX = 3;
    private int mBannerCount;
    private int mBannerId;
    private TerminalBannerComponent mBannerNext;
    private int mBannerRetryCount;
    [SerializeField]
    private UISprite mBannerTex;
    private Texture2D mBannerTex2D;
    private string mBannerUrl;
    private WWW mBannerWWW;
    private int mIdx;
    [SerializeField]
    private UISprite mLoadingSp;
    private float mPosMax;
    private float mPosMin;
    private float mTgtPos;
    public static readonly float TGT_SPD_RATE = 0.25f;

    private void DestroyBanner()
    {
        if (this.mBannerWWW != null)
        {
            this.mBannerWWW.Dispose();
            this.mBannerWWW = null;
        }
        if (this.mBannerTex2D != null)
        {
            UnityEngine.Object.Destroy(this.mBannerTex2D);
            this.mBannerTex2D = null;
        }
    }

    public float GetPos() => 
        base.gameObject.GetLocalPositionX();

    public int GetPosIdx()
    {
        float f = (this.GetPos() + (BANNER_INTERVAL_POS / 2f)) / BANNER_INTERVAL_POS;
        return (int) Mathf.Floor(f);
    }

    public bool IsFocus() => 
        (this.GetPosIdx() == 0);

    public void Move(QuestBoardListViewItemDraw qdraw)
    {
        float pos = this.GetPos();
        if (qdraw.IsEnableDragX)
        {
            this.mTgtPos += qdraw.TouchPosDif;
        }
        float num2 = this.mTgtPos - pos;
        if (!qdraw.IsEnableDragX)
        {
            num2 *= TGT_SPD_RATE;
        }
        pos += num2;
        if (pos <= this.mPosMin)
        {
            pos = this.mPosMax + (pos - this.mPosMin);
            this.mTgtPos = this.mPosMax + (this.mTgtPos - this.mPosMin);
        }
        else if (pos > this.mPosMax)
        {
            pos = this.mPosMin + (pos - this.mPosMax);
            this.mTgtPos = this.mPosMin + (this.mTgtPos - this.mPosMax);
        }
        this.SetPos(pos);
    }

    private void OnDisable()
    {
        base.StopCoroutine(this.StartDownloadBanner());
        this.DestroyBanner();
    }

    public void OnPress(QuestBoardListViewItemDraw qdraw)
    {
        this.mTgtPos = this.GetPos();
    }

    public void OnPull(QuestBoardListViewItemDraw qdraw)
    {
        DIR nONE = DIR.NONE;
        if (qdraw.IsFlickL())
        {
            nONE = DIR.L;
        }
        else if (qdraw.IsFlickR())
        {
            nONE = DIR.R;
        }
        this.StartAutoMove(nONE);
    }

    private void SetBannerTexture(Texture2D tex)
    {
        this.mBannerTex.mainTexture = tex;
        this.mBannerTex.MakePixelPerfect();
        this.mLoadingSp.gameObject.SetActive(false);
        this.StartLoadAndDisp_BannerNext();
    }

    private void SetPos(float pos)
    {
        base.gameObject.SetLocalPositionX(pos);
    }

    private void SetTgtPos_ByPosIdx(int pos_idx)
    {
        this.mTgtPos = pos_idx * BANNER_INTERVAL_POS;
    }

    public void Setup(int idx, int banner_id, int banner_count, TerminalBannerComponent banner_next)
    {
        this.mIdx = idx;
        this.mBannerId = banner_id;
        this.mBannerCount = banner_count;
        this.mBannerNext = banner_next;
        this.mTgtPos = idx * BANNER_INTERVAL_POS;
        base.gameObject.SetLocalPositionX(this.mTgtPos);
        this.mPosMin = -BANNER_INTERVAL_POS;
        this.mPosMax = BANNER_INTERVAL_POS * (banner_count - 1);
    }

    private void StartAutoMove(DIR dir = 0)
    {
        int posIdx = this.GetPosIdx();
        if (dir != DIR.NONE)
        {
            posIdx += (dir != DIR.L) ? 1 : -1;
        }
        this.SetTgtPos_ByPosIdx(posIdx);
    }

    public void StartAutoMoveL()
    {
        this.StartAutoMove(DIR.L);
    }

    public void StartAutoMoveR()
    {
        this.StartAutoMove(DIR.R);
    }

    [DebuggerHidden]
    private IEnumerator StartDownloadBanner() => 
        new <StartDownloadBanner>c__Iterator38 { <>f__this = this };

    public void StartLoadAndDisp()
    {
        this.mLoadingSp.gameObject.SetActive(true);
        if ((this.mBannerId > 0) && AtlasManager.SetShopBanner(this.mBannerTex, "terminal_banner_" + this.mBannerId))
        {
            this.mLoadingSp.gameObject.SetActive(false);
        }
    }

    private void StartLoadAndDisp_BannerNext()
    {
        if (this.mBannerNext != null)
        {
            this.mBannerNext.StartLoadAndDisp();
        }
    }

    public int Idx =>
        this.mIdx;

    [CompilerGenerated]
    private sealed class <StartDownloadBanner>c__Iterator38 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal TerminalBannerComponent <>f__this;

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
                    this.<>f__this.DestroyBanner();
                    this.<>f__this.mBannerWWW = new WWW(this.<>f__this.mBannerUrl);
                    this.$current = this.<>f__this.mBannerWWW;
                    this.$PC = 1;
                    return true;

                case 1:
                    if (((((this.<>f__this.mBannerWWW != null) && string.IsNullOrEmpty(this.<>f__this.mBannerWWW.error)) && (this.<>f__this.mBannerWWW.bytes != null)) && ((this.<>f__this.mBannerWWW.bytes == null) || (this.<>f__this.mBannerWWW.bytes.Length != 0))) && (this.<>f__this.mBannerWWW.texture != null))
                    {
                        this.<>f__this.mBannerTex2D = this.<>f__this.mBannerWWW.texture;
                        this.<>f__this.SetBannerTexture(this.<>f__this.mBannerTex2D);
                        break;
                    }
                    this.<>f__this.mBannerRetryCount++;
                    if (this.<>f__this.mBannerRetryCount <= TerminalBannerComponent.BANNER_RETRY_MAX)
                    {
                        this.<>f__this.StartCoroutine(this.<>f__this.StartDownloadBanner());
                    }
                    else
                    {
                        this.<>f__this.StartLoadAndDisp_BannerNext();
                    }
                    break;

                default:
                    goto Label_01A2;
            }
            if (this.<>f__this.mBannerWWW != null)
            {
                this.<>f__this.mBannerWWW.Dispose();
                this.<>f__this.mBannerWWW = null;
            }
            this.$PC = -1;
        Label_01A2:
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

    public enum DIR
    {
        NONE,
        L,
        R
    }
}

