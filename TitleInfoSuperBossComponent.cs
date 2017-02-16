using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TitleInfoSuperBossComponent : TitleInfoEventInfoComponent
{
    private static readonly float ANIM_DURATION_BOSS_ALPHA = 2.5f;
    private static readonly float ANIM_DURATION_FRAME_IN_BOSS = 0.3f;
    private static readonly float ANIM_DURATION_HP_CUT = 1.2f;
    private static readonly float ANIM_DURATION_HP_DELETION = 0.7f;
    private static readonly float ANIM_DURATION_HP_NAME_ALPHA = 0.4f;
    private static readonly float ANIM_DURATION_SPLIT_ALPHA = 0.5f;
    private System.Action animEndCall;
    private static readonly float CLEAR_CALLBACK_DELAY = 1.8f;
    private static readonly float CLEAR_END_CALLBACK_DELAY = 0.5f;
    private GameObject clearBossEffect;
    private EventSuperBossEntity eventSuperBossEntity;
    [SerializeField]
    private UISlider hpBarSlider;
    [SerializeField]
    private UISlider hpBarSliderUnder;
    [SerializeField]
    private UISlider hpBarSliderWhite;
    [SerializeField]
    private GameObject hpBarSplitPoint;
    private UISprite[] hpBarSplitPoints;
    [SerializeField]
    private GameObject hpBarSplitter;
    private UISprite[] hpBarSplitters;
    private float HPfrom;
    private int HPsplitNo;
    private float HPsplitPoint;
    private float HPto;
    private bool isAnimate;
    private bool isCheckTutorial;
    private bool isEncountSuperBoss;
    private System.Action onClearCall;
    private Vector3 OriginPos;
    private GameObject splitHpEffect;
    private static readonly Vector3 SUPERBOSS_ANIM_ROOT_POS = new Vector3(0f, 78f, 0f);
    private static readonly string SUPERBOSS_CLEAR_EFFECT_PREFAB = "TitleInfoEventSuperBossEffect_Dead";
    private static readonly int SUPERBOSS_HP_BAR_LENGTH = 0x108;
    private static readonly string SUPERBOSS_ICON_SPNAME_PREFIX = "event_superboss_icon_";
    private static readonly string SUPERBOSS_SPLIT_EFFECT_PREFAB = "TitleInfoEventSuperBossEffect_Split";
    private static readonly Vector3 SUPERBOSS_SPLIT_POS_DELTA = new Vector3(-0.5f, 10f, 0f);
    [SerializeField]
    private UISprite superBossIconSp;
    [SerializeField]
    private UILabel totalHpLabel;

    private void Destroy()
    {
    }

    public void DisableCheckTutorial()
    {
        this.isCheckTutorial = false;
    }

    private void DoClearCallback()
    {
        System.Action onClearCall = this.onClearCall;
        this.onClearCall = null;
        onClearCall.Call();
        TweenAlpha alpha = UITweener.Begin<TweenAlpha>(this.superBossIconSp.gameObject, ANIM_DURATION_SPLIT_ALPHA);
        alpha.from = 1f;
        alpha.to = 0f;
        alpha.method = UITweener.Method.EaseIn;
        base.Invoke("OnEndAnimation", CLEAR_END_CALLBACK_DELAY);
    }

    public override bool IsDispPossible() => 
        this.isEncountSuperBoss;

    public void OnDestroy()
    {
        this.Destroy();
    }

    private void OnEndAnimation()
    {
        System.Action animEndCall = this.animEndCall;
        this.animEndCall = null;
        animEndCall.Call();
        this.SetDisp();
    }

    private void PlayFrameInBoss()
    {
        TweenPosition position = UITweener.Begin<TweenPosition>(base.gameObject, ANIM_DURATION_FRAME_IN_BOSS);
        position.from = this.OriginPos + SUPERBOSS_ANIM_ROOT_POS;
        position.to = this.OriginPos;
        position.method = UITweener.Method.EaseOut;
        position.eventReceiver = base.gameObject;
        position.callWhenFinished = "PlayHpNameAlpha";
    }

    private void PlayHpCut()
    {
        <PlayHpCut>c__AnonStorey5A storeya = new <PlayHpCut>c__AnonStorey5A {
            <>f__this = this
        };
        SoundManager.playSeLoop("ar12");
        storeya.eo = base.gameObject.SafeGetComponent<EasingObject>();
        storeya.eo.Play(ANIM_DURATION_HP_CUT, new System.Action(storeya.<>m__24), new System.Action(this.PlayHpDeletion), 0f, Easing.TYPE.LINER);
    }

    private void PlayHpDeletion()
    {
        TweenAlpha alpha = UITweener.Begin<TweenAlpha>(this.hpBarSliderUnder.gameObject, ANIM_DURATION_HP_DELETION);
        alpha.from = 1f;
        alpha.to = 0f;
        alpha.method = UITweener.Method.EaseIn;
        alpha.eventReceiver = base.gameObject;
        if (this.HPto == 0f)
        {
            if (this.clearBossEffect != null)
            {
                GameObject self = UnityEngine.Object.Instantiate<GameObject>(this.clearBossEffect);
                self.SafeSetParent(this);
                self.SetLocalPosition(Vector3.zero);
            }
            alpha.callWhenFinished = "SetClearAlpha";
        }
        else
        {
            alpha.callWhenFinished = "OnEndAnimation";
        }
        SoundManager.stopSe("ar12", 0f);
        this.totalHpLabel.text = string.Format(LocalizationManager.Get("TITLE_INFO_SUPERBOSS_TOTALHP"), this.eventSuperBossEntity.maxHp - this.eventSuperBossEntity.GetUserSuperBossEntity().damage);
    }

    private void PlayHpNameAlpha()
    {
        TweenAlpha alpha = UITweener.Begin<TweenAlpha>(this.totalHpLabel.gameObject, ANIM_DURATION_HP_NAME_ALPHA);
        alpha.from = 0f;
        alpha.to = 1f;
        alpha.method = UITweener.Method.EaseIn;
        alpha.eventReceiver = base.gameObject;
        alpha.callWhenFinished = "PlayHpCut";
    }

    private void SetClearAlpha()
    {
        this.isEncountSuperBoss = false;
        base.Invoke("DoClearCallback", CLEAR_CALLBACK_DELAY);
    }

    public void SetDamageAnimation(long damage, System.Action onClearCallback)
    {
        this.onClearCall = onClearCallback;
        long num = this.eventSuperBossEntity.GetUserSuperBossEntity().damage;
        long num2 = damage;
        double maxHp = this.eventSuperBossEntity.maxHp;
        this.HPfrom = 1f - (((float) num2) / ((float) maxHp));
        this.HPto = 1f - (((float) num) / ((float) maxHp));
        this.HPfrom = Mathf.Clamp01(this.HPfrom);
        this.HPto = Mathf.Clamp01(this.HPto);
        this.HPsplitPoint = -1f;
        this.HPsplitNo = -1;
        for (int i = 0; i < this.eventSuperBossEntity.splitHp.Length; i++)
        {
            if ((this.eventSuperBossEntity.splitHp[i] > num2) && (this.eventSuperBossEntity.splitHp[i] <= num))
            {
                this.HPsplitPoint = 1f - ((float) (((double) this.eventSuperBossEntity.splitHp[i]) / maxHp));
                this.HPsplitNo = i;
                break;
            }
        }
        this.hpBarSlider.value = this.HPfrom;
        this.hpBarSliderWhite.gameObject.SetActive(true);
        this.hpBarSliderUnder.gameObject.SetActive(true);
        this.hpBarSliderWhite.value = this.HPfrom;
        this.hpBarSliderWhite.alpha = 0f;
        this.hpBarSliderUnder.value = this.HPfrom;
        this.OriginPos = base.gameObject.transform.localPosition;
        Transform transform = base.gameObject.transform;
        transform.localPosition += SUPERBOSS_ANIM_ROOT_POS;
        this.totalHpLabel.alpha = 0f;
        for (int j = 0; j < this.eventSuperBossEntity.splitHp.Length; j++)
        {
            if (this.eventSuperBossEntity.splitHp[j] <= num2)
            {
                this.hpBarSplitPoints[j].alpha = 0f;
            }
            else
            {
                this.hpBarSplitPoints[j].alpha = 1f;
            }
        }
    }

    public void SetDisp()
    {
        if (this.isEncountSuperBoss)
        {
            UserSuperBossEntity userSuperBossEntity = this.eventSuperBossEntity.GetUserSuperBossEntity();
            string bannerName = SUPERBOSS_ICON_SPNAME_PREFIX + this.eventSuperBossEntity.iconId.ToString();
            this.superBossIconSp.enabled = AtlasManager.SetBanner(this.superBossIconSp, bannerName);
            this.superBossIconSp.MakePixelPerfect();
            if (this.eventSuperBossEntity.splitHp.Length > 0)
            {
                bool flag = true;
                if ((this.hpBarSplitters == null) || (this.hpBarSplitPoints == null))
                {
                    this.hpBarSplitters = new UISprite[this.eventSuperBossEntity.splitHp.Length];
                    this.hpBarSplitPoints = new UISprite[this.eventSuperBossEntity.splitHp.Length];
                    flag = false;
                }
                Transform parent = this.hpBarSplitter.GetParent();
                Transform transform2 = this.hpBarSplitPoint.GetParent();
                for (int i = 0; i < this.eventSuperBossEntity.splitHp.Length; i++)
                {
                    if (!flag)
                    {
                        if (i == 0)
                        {
                            this.hpBarSplitters[0] = this.hpBarSplitter.GetComponent<UISprite>();
                            this.hpBarSplitPoints[0] = this.hpBarSplitPoint.GetComponent<UISprite>();
                        }
                        else
                        {
                            this.hpBarSplitters[i] = UnityEngine.Object.Instantiate<GameObject>(this.hpBarSplitter).GetComponent<UISprite>();
                            this.hpBarSplitPoints[i] = UnityEngine.Object.Instantiate<GameObject>(this.hpBarSplitPoint).GetComponent<UISprite>();
                            this.hpBarSplitters[i].gameObject.SafeSetParent(parent);
                            this.hpBarSplitPoints[i].gameObject.SafeSetParent(transform2);
                        }
                    }
                    Vector3 vector = new Vector3(-(((float) this.eventSuperBossEntity.splitHp[i]) / ((float) this.eventSuperBossEntity.maxHp)) * SUPERBOSS_HP_BAR_LENGTH, 0f, 0f);
                    this.hpBarSplitters[i].transform.localPosition = vector;
                    this.hpBarSplitPoints[i].transform.localPosition = vector;
                    if (this.eventSuperBossEntity.splitHp[i] <= userSuperBossEntity.damage)
                    {
                        this.hpBarSplitPoints[i].alpha = 0f;
                    }
                    else
                    {
                        this.hpBarSplitPoints[i].alpha = 1f;
                    }
                }
            }
            this.hpBarSliderWhite.gameObject.SetActive(false);
            this.hpBarSliderUnder.gameObject.SetActive(false);
            long maxHp = 0L;
            long damage = 0L;
            maxHp = this.eventSuperBossEntity.maxHp;
            if (this.eventSuperBossEntity.maxHp <= userSuperBossEntity.damage)
            {
                damage = this.eventSuperBossEntity.maxHp;
            }
            else
            {
                damage = userSuperBossEntity.damage;
            }
            if (this.totalHpLabel.gameObject.activeSelf)
            {
                string format = LocalizationManager.Get("TITLE_INFO_SUPERBOSS_TOTALHP");
                long num4 = maxHp - damage;
                string str3 = string.Format(format, num4);
                this.totalHpLabel.text = str3;
            }
            this.totalHpLabel.effectColor = this.eventSuperBossEntity.GetBossColor();
            float num5 = 1f - ((float) (((double) damage) / ((double) maxHp)));
            this.hpBarSlider.value = num5;
        }
    }

    public void Setup(EventSuperBossEntity superBossEntity)
    {
        this.eventSuperBossEntity = superBossEntity;
        this.Destroy();
        this.isEncountSuperBoss = false;
        this.isCheckTutorial = true;
        if (this.eventSuperBossEntity != null)
        {
            this.isEncountSuperBoss = this.eventSuperBossEntity.IsEncounted();
            this.SetDisp();
        }
    }

    public void StartDamageAnimation(AssetData mapAssetData, System.Action callBack)
    {
        this.animEndCall = callBack;
        this.splitHpEffect = mapAssetData.GetObject<GameObject>(SUPERBOSS_SPLIT_EFFECT_PREFAB);
        this.clearBossEffect = mapAssetData.GetObject<GameObject>(SUPERBOSS_CLEAR_EFFECT_PREFAB);
        this.PlayFrameInBoss();
    }

    public override void UpdateDisp()
    {
    }

    [CompilerGenerated]
    private sealed class <PlayHpCut>c__AnonStorey5A
    {
        internal TitleInfoSuperBossComponent <>f__this;
        internal EasingObject eo;

        internal void <>m__24()
        {
            float t = this.eo.Now();
            float num2 = Mathf.Lerp(this.<>f__this.HPfrom, this.<>f__this.HPto, t);
            this.<>f__this.hpBarSlider.value = num2;
            this.<>f__this.hpBarSliderWhite.value = num2;
            this.<>f__this.hpBarSliderWhite.alpha = Mathf.Sin(t * 3.141593f) * 0.7f;
            this.<>f__this.totalHpLabel.text = string.Format(LocalizationManager.Get("TITLE_INFO_SUPERBOSS_TOTALHP"), (double) (num2 * this.<>f__this.eventSuperBossEntity.maxHp));
            if ((this.<>f__this.HPsplitPoint > -1f) && (num2 <= this.<>f__this.HPsplitPoint))
            {
                if (this.<>f__this.splitHpEffect != null)
                {
                    GameObject self = UnityEngine.Object.Instantiate<GameObject>(this.<>f__this.splitHpEffect);
                    self.SafeSetParent(this.<>f__this.hpBarSplitter.GetParent());
                    self.SetLocalPosition(this.<>f__this.hpBarSplitters[this.<>f__this.HPsplitNo].transform.localPosition + TitleInfoSuperBossComponent.SUPERBOSS_SPLIT_POS_DELTA);
                }
                TweenAlpha alpha = UITweener.Begin<TweenAlpha>(this.<>f__this.hpBarSplitPoints[this.<>f__this.HPsplitNo].gameObject, TitleInfoSuperBossComponent.ANIM_DURATION_SPLIT_ALPHA);
                alpha.from = 1f;
                alpha.to = 0f;
                alpha.method = UITweener.Method.EaseOut;
                this.<>f__this.HPsplitPoint = -1f;
                this.<>f__this.HPsplitNo = -1;
            }
        }
    }
}

