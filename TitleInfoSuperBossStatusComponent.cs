using System;
using UnityEngine;

public class TitleInfoSuperBossStatusComponent : TitleInfoEventInfoComponent
{
    private static readonly float ENTRY_ANIM_DELAY = 0.5f;
    private static readonly float FRAME_ANIM_DURATION = 0.3f;
    private static readonly Vector3 FRAME_ANIM_OUT_POS_DELTA = new Vector3(-70f, 0f, 0f);
    private Vector3 inPos;
    private Vector3 outPos;
    private static readonly string SUPERBOSS_ICON_SPNAME_PREFIX_BATTLE = "event_superboss_status_battle_";
    private static readonly string SUPERBOSS_ICON_SPNAME_PREFIX_WIN = "event_superboss_status_win_";
    private EventSuperBossEntity superBossEntity;
    [SerializeField]
    private UISprite superBossIconSp;

    private void AnimFrameOutEnd()
    {
        string bannerName = SUPERBOSS_ICON_SPNAME_PREFIX_WIN + this.superBossEntity.iconId.ToString();
        this.superBossIconSp.enabled = AtlasManager.SetBanner(this.superBossIconSp, bannerName);
        this.superBossIconSp.MakePixelPerfect();
        this.FrameIn(null);
    }

    private void Destroy()
    {
    }

    private void EntryAnim()
    {
        this.FrameIn(null);
    }

    public void FrameIn(string callFinished)
    {
        TweenPosition position = UITweener.Begin<TweenPosition>(base.gameObject, FRAME_ANIM_DURATION);
        position.from = this.outPos;
        position.to = this.inPos;
        position.method = UITweener.Method.EaseOutQuad;
        position.eventReceiver = base.gameObject;
        position.callWhenFinished = callFinished;
        TweenAlpha alpha = UITweener.Begin<TweenAlpha>(this.superBossIconSp.gameObject, FRAME_ANIM_DURATION);
        alpha.from = 0f;
        alpha.to = 1f;
        alpha.method = UITweener.Method.EaseOutQuad;
    }

    public void FrameOut(string callFinished)
    {
        TweenPosition position = UITweener.Begin<TweenPosition>(base.gameObject, FRAME_ANIM_DURATION);
        position.from = this.inPos;
        position.to = this.outPos;
        position.method = UITweener.Method.EaseOut;
        position.eventReceiver = base.gameObject;
        position.callWhenFinished = callFinished;
        TweenAlpha alpha = UITweener.Begin<TweenAlpha>(this.superBossIconSp.gameObject, FRAME_ANIM_DURATION);
        alpha.from = 1f;
        alpha.to = 0f;
        alpha.method = UITweener.Method.EaseOut;
    }

    public override bool IsDispPossible() => 
        true;

    public void OnDestroy()
    {
        this.Destroy();
    }

    public void SetClearAnim()
    {
        base.gameObject.SetActive(true);
        string bannerName = SUPERBOSS_ICON_SPNAME_PREFIX_BATTLE + this.superBossEntity.iconId.ToString();
        this.superBossIconSp.enabled = AtlasManager.SetBanner(this.superBossIconSp, bannerName);
        this.superBossIconSp.MakePixelPerfect();
    }

    public void SetEntryAnim()
    {
        this.inPos = new Vector3(0f, base.transform.localPosition.y);
        this.outPos = this.inPos + FRAME_ANIM_OUT_POS_DELTA;
        base.transform.localPosition = this.outPos;
        this.superBossIconSp.alpha = 0f;
        base.Invoke("EntryAnim", (-this.inPos.y / 500f) + ENTRY_ANIM_DELAY);
    }

    public void Setup(EventSuperBossEntity eventSuperBossEntity)
    {
        this.Destroy();
        this.superBossEntity = eventSuperBossEntity;
        GameObject gameObject = base.gameObject;
        gameObject.name = gameObject.name + this.superBossEntity.id.ToString();
        this.UpdateDisp();
    }

    public void StartClearAnim()
    {
        this.inPos = new Vector3(0f, base.transform.localPosition.y);
        this.outPos = this.inPos + FRAME_ANIM_OUT_POS_DELTA;
        this.FrameOut("AnimFrameOutEnd");
    }

    public int SuperBossId() => 
        this.superBossEntity.id;

    public override void UpdateDisp()
    {
        if (this.superBossEntity != null)
        {
            if (!this.superBossEntity.IsEncounted())
            {
                base.gameObject.SetActive(false);
            }
            else
            {
                base.gameObject.SetActive(true);
                string str = !this.superBossEntity.IsCleard() ? SUPERBOSS_ICON_SPNAME_PREFIX_BATTLE : SUPERBOSS_ICON_SPNAME_PREFIX_WIN;
                string bannerName = str + this.superBossEntity.iconId.ToString();
                this.superBossIconSp.enabled = AtlasManager.SetBanner(this.superBossIconSp, bannerName);
                this.superBossIconSp.MakePixelPerfect();
            }
        }
    }
}

