using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class UIScriptChara : MonoBehaviour
{
    [CompilerGenerated]
    private static Dictionary<string, int> <>f__switch$map2C;
    [CompilerGenerated]
    private static Dictionary<string, int> <>f__switch$map2D;
    [CompilerGenerated]
    private static Dictionary<string, int> <>f__switch$map2E;
    protected int backIndex = -1;
    [SerializeField]
    protected GameObject baseCutEffect;
    [SerializeField]
    protected Transform baseDepth;
    [SerializeField]
    protected GameObject baseEffect;
    [SerializeField]
    protected GameObject baseEffectBack;
    protected Vector3 basePosition;
    [SerializeField]
    protected Transform baseScale;
    [SerializeField]
    protected GameObject baseShadowEffect;
    [SerializeField]
    protected Transform baseShake;
    [SerializeField]
    protected GameObject baseSpecialEffect;
    protected System.Action changeCallback;
    protected int changeCount;
    protected ChangeKind changeKind;
    protected float changeRange;
    protected float changeSpeed;
    protected float changeStep;
    protected float changeTotal;
    protected Color filterColor;
    protected string filterName;
    protected bool isDisp;
    protected bool isMove;
    protected bool isShadow;
    protected bool isShadowEffect;
    protected int mainIndex;
    protected float returnDuration;
    protected System.Action setCallback;
    protected float shakeCycle;
    protected float shakeTime;
    protected float shakeX;
    protected float shakeY;
    protected static readonly float UNIT_SPEED = 0.015f;

    public void ChangeCharacter(string kind, float speed, string imageName, Face.Type faceType, System.Action callback)
    {
        ChangeKind nONE = ChangeKind.NONE;
        string key = kind;
        if (key != null)
        {
            int num;
            if (<>f__switch$map2C == null)
            {
                Dictionary<string, int> dictionary = new Dictionary<string, int>(2) {
                    { 
                        "fade",
                        0
                    },
                    { 
                        "blink",
                        1
                    }
                };
                <>f__switch$map2C = dictionary;
            }
            if (<>f__switch$map2C.TryGetValue(key, out num))
            {
                if (num == 0)
                {
                    nONE = ChangeKind.FADE;
                    goto Label_0072;
                }
                if (num == 1)
                {
                    nONE = ChangeKind.BLINK;
                    goto Label_0072;
                }
            }
        }
        nONE = ChangeKind.NORMAL;
    Label_0072:
        this.ChangeCharacter(nONE, speed, imageName, faceType, callback);
    }

    public virtual void ChangeCharacter(ChangeKind kind, float speed, string imageName, Face.Type faceType, System.Action callback)
    {
        this.changeKind = kind;
        this.changeCallback = callback;
        this.backIndex = this.mainIndex;
        this.mainIndex = (this.mainIndex != 0) ? 0 : 1;
        this.changeSpeed = speed;
        this.changeCount = 0;
        this.changeRange = 0f;
        this.changeTotal = 0f;
        if (this.changeKind == ChangeKind.BLINK)
        {
            this.changeRange = ((speed < 10f) ? speed : 10f) / 5f;
        }
        this.isDisp = false;
    }

    protected void EndMove()
    {
        this.isMove = false;
    }

    protected void EndMoveAttack()
    {
        this.isMove = false;
    }

    protected void EndMoveReturn()
    {
        this.isMove = false;
    }

    protected void EndMoveReturnHalf()
    {
        this.MoveReturnPosition(this.returnDuration);
    }

    protected virtual void EndSet()
    {
        Vector2 zero = Vector2.zero;
        float z = this.baseSpecialEffect.transform.localPosition.z;
        this.baseSpecialEffect.transform.localPosition = new Vector3(zero.x, zero.y, z);
        z = this.baseEffect.transform.localPosition.z;
        this.baseEffect.transform.localPosition = new Vector3(zero.x, zero.y, z);
        z = this.baseEffectBack.transform.localPosition.z;
        this.baseEffectBack.transform.localPosition = new Vector3(zero.x, zero.y, z);
        z = this.baseShadowEffect.transform.localPosition.z;
        this.baseShadowEffect.transform.localPosition = new Vector3(zero.x, zero.y, z);
        if (this.setCallback != null)
        {
            System.Action setCallback = this.setCallback;
            this.setCallback = null;
            setCallback();
        }
    }

    public Vector3 GetBasePosition() => 
        this.basePosition;

    public bool IsBackEffect() => 
        CommonEffectManager.IsBusy(this.baseEffectBack);

    public bool IsBackEffect(string n) => 
        CommonEffectManager.IsBusy(this.baseEffectBack, n);

    public virtual bool IsBusyMoveAlpha() => 
        false;

    public bool IsChange() => 
        (this.changeKind != ChangeKind.NONE);

    public bool IsCut()
    {
        foreach (ProgramEffectComponent component in ProgramEffectManager.Get(this.baseCutEffect))
        {
            CharaCutEffectComponent component2 = component as CharaCutEffectComponent;
            if ((component2 != null) && component2.IsBusyCut())
            {
                return true;
            }
        }
        return false;
    }

    public bool IsEffect() => 
        CommonEffectManager.IsBusy(this.baseEffect);

    public bool IsEffect(string n) => 
        CommonEffectManager.IsBusy(this.baseEffect, n);

    public bool IsMove() => 
        this.isMove;

    public bool IsShake() => 
        (this.shakeCycle > 0f);

    public bool IsSpecialEffect() => 
        ProgramEffectManager.IsBusy(this.baseSpecialEffect);

    public bool IsSpecialEffect(string n) => 
        ProgramEffectManager.IsBusy(this.baseSpecialEffect, n);

    public virtual void MoveAlpha(float duration, float a)
    {
        this.isDisp = a > 0f;
        this.RecoverShadowEffect();
    }

    public void MoveAttack(string kind, float duration, Vector3 v)
    {
        this.isMove = true;
        if (duration <= 0f)
        {
            duration = 0.5f;
        }
        string key = kind;
        if (key != null)
        {
            int num;
            if (<>f__switch$map2D == null)
            {
                Dictionary<string, int> dictionary = new Dictionary<string, int>(1) {
                    { 
                        "normal",
                        0
                    }
                };
                <>f__switch$map2D = dictionary;
            }
            if (<>f__switch$map2D.TryGetValue(key, out num) && (num != 0))
            {
            }
        }
        TweenPosition position = TweenPosition.Begin(base.gameObject, duration, v);
        if (position != null)
        {
            position.method = UITweener.Method.EaseIn;
            position.eventReceiver = base.gameObject;
            position.callWhenFinished = "EndMoveAttack";
        }
        else
        {
            base.gameObject.transform.localPosition = v;
            this.EndMoveAttack();
        }
    }

    public void MovePosition(float duration, Vector3 v)
    {
        this.isMove = true;
        if (duration > 0f)
        {
            TweenPosition position = TweenPosition.Begin(base.gameObject, duration, v);
            if (position != null)
            {
                position.eventReceiver = base.gameObject;
                position.callWhenFinished = "EndMove";
                return;
            }
        }
        base.gameObject.transform.localPosition = v;
        this.EndMove();
    }

    public void MoveReturnPosition(float duration)
    {
        this.isMove = true;
        if (duration > 0f)
        {
            TweenPosition position = TweenPosition.Begin(base.gameObject, duration, this.basePosition);
            if (position != null)
            {
                position.eventReceiver = base.gameObject;
                position.callWhenFinished = "EndMoveReturn";
                return;
            }
        }
        base.gameObject.transform.localPosition = this.basePosition;
        this.EndMoveReturn();
    }

    public void MoveReturnPosition(float duration, Vector3 v)
    {
        this.isMove = true;
        if (duration > 0f)
        {
            this.returnDuration = duration / 2f;
            this.basePosition = base.gameObject.transform.localPosition;
            TweenPosition position = TweenPosition.Begin(base.gameObject, this.returnDuration, v);
            if (position != null)
            {
                position.eventReceiver = base.gameObject;
                position.callWhenFinished = "EndMoveReturnHalf";
                return;
            }
        }
        base.gameObject.transform.localPosition = this.basePosition;
        this.EndMoveReturnHalf();
    }

    protected void OnShake()
    {
        if ((this.shakeCycle > 0f) && ((this.shakeTime == 0f) || (Time.time < this.shakeTime)))
        {
            this.baseShake.localPosition = new Vector3(UnityEngine.Random.Range(-this.shakeX, this.shakeX), UnityEngine.Random.Range(-this.shakeY, this.shakeY), 0f);
            base.Invoke("OnShake", this.shakeCycle);
        }
        else
        {
            base.CancelInvoke("OnShake");
            this.baseShake.localPosition = Vector3.zero;
            this.shakeCycle = 0f;
        }
    }

    public void RecoverShadowEffect()
    {
        if (this.isShadow)
        {
            this.StartShadowEffect();
        }
        else
        {
            this.StopShadowEffect();
        }
    }

    public virtual void SetAlpha(float a)
    {
        this.isDisp = a > 0f;
        this.RecoverShadowEffect();
    }

    public void SetBackEffect(string n, bool isSkip)
    {
        this.SetBackEffect(n, Vector3.zero, isSkip);
    }

    public void SetBackEffect(string n, Vector3 p, bool isSkip)
    {
        CommonEffectManager.Create(this.baseEffectBack, n, p, isSkip);
    }

    public void SetBasePosition(Vector3 v)
    {
        base.transform.localPosition = this.basePosition = v;
    }

    public virtual void SetCharacter(string imageName, Face.Type faceType, System.Action callback)
    {
        this.setCallback = callback;
    }

    public virtual void SetCutin(string n, float time, float mgd, bool isSkip)
    {
    }

    public virtual void SetCutout(float time, bool isSkip)
    {
    }

    public virtual void SetDepth(int d)
    {
        Vector3 localPosition = this.baseDepth.localPosition;
        localPosition.z = -d * 10f;
        this.baseDepth.localPosition = localPosition;
    }

    public void SetEffect(string n, bool isSkip)
    {
        this.SetEffect(n, Vector3.zero, isSkip);
    }

    public void SetEffect(string n, Vector3 p, bool isSkip)
    {
        CommonEffectManager.Create(this.baseEffect, n, p, isSkip);
    }

    public virtual void SetFace(Face.Type faceType)
    {
    }

    public virtual void SetFilter(string filterName, Color filterColor)
    {
        this.filterName = filterName;
        this.filterColor = filterColor;
    }

    public void SetPosition(Vector3 v)
    {
        base.transform.localPosition = v;
    }

    public virtual void SetScale(float v)
    {
        this.baseScale.localScale = new Vector3(v, v, 1f);
    }

    public virtual void SetShadow(bool isShadow)
    {
        if (this.isShadow != isShadow)
        {
            this.isShadow = isShadow;
            if (this.backIndex >= 0)
            {
            }
            this.RecoverShadowEffect();
        }
    }

    public virtual void SetSpecialEffect(string n, Vector3 pos, float time, Color color, bool isSkip)
    {
        GameObject obj2 = ProgramEffectManager.CreateCharaEffect(this.baseSpecialEffect, n, pos, time, color, isSkip);
        if (obj2 != null)
        {
            string key = n;
            if (key != null)
            {
                int num;
                if (<>f__switch$map2E == null)
                {
                    Dictionary<string, int> dictionary = new Dictionary<string, int>(10) {
                        { 
                            "erasure",
                            0
                        },
                        { 
                            "flashErasure",
                            0
                        },
                        { 
                            "enemyErasure",
                            0
                        },
                        { 
                            "darkEnemyErasure",
                            0
                        },
                        { 
                            "erasureReverse",
                            1
                        },
                        { 
                            "appearance",
                            2
                        },
                        { 
                            "appearanceReverse",
                            3
                        },
                        { 
                            "wipe",
                            4
                        },
                        { 
                            "darkWipe",
                            4
                        },
                        { 
                            "flash",
                            5
                        }
                    };
                    <>f__switch$map2E = dictionary;
                }
                if (<>f__switch$map2E.TryGetValue(key, out num))
                {
                    switch (num)
                    {
                        case 0:
                        {
                            CharaErasureEffectComponent component = obj2.GetComponent<CharaErasureEffectComponent>();
                            this.StopShadowEffect();
                            break;
                        }
                        case 1:
                        {
                            CharaErasureReverseEffectComponent component2 = obj2.GetComponent<CharaErasureReverseEffectComponent>();
                            this.StopShadowEffect();
                            break;
                        }
                        case 2:
                        {
                            CharaAppearanceEffectComponent component3 = obj2.GetComponent<CharaAppearanceEffectComponent>();
                            this.StopShadowEffect();
                            break;
                        }
                        case 3:
                        {
                            CharaAppearanceReverseEffectComponent component4 = obj2.GetComponent<CharaAppearanceReverseEffectComponent>();
                            this.StopShadowEffect();
                            break;
                        }
                        case 4:
                        {
                            CharaWipeEffectComponent component5 = obj2.GetComponent<CharaWipeEffectComponent>();
                            this.StopShadowEffect();
                            break;
                        }
                        case 5:
                        {
                            CharaFlashEffectComponent component6 = obj2.GetComponent<CharaFlashEffectComponent>();
                            break;
                        }
                    }
                }
            }
        }
    }

    public virtual void SetTalkMask(bool isMask)
    {
    }

    public void Shake(float duration, float cycle, float x, float y)
    {
        this.shakeTime = (duration <= 0f) ? 0f : (Time.time + duration);
        this.shakeCycle = cycle;
        this.shakeX = x;
        this.shakeY = y;
        this.OnShake();
    }

    public void StartShadowEffect()
    {
        if ((this.isShadow && this.isDisp) && !this.isShadowEffect)
        {
            this.isShadowEffect = true;
            CommonEffectManager.Create(this.baseShadowEffect, "Talk/bit_talk_11");
        }
    }

    public void StopBackEffect(bool isSkip)
    {
        CommonEffectManager.Stop(this.baseEffectBack, isSkip, false);
    }

    public void StopBackEffect(string n, bool isSkip)
    {
        CommonEffectManager.Stop(this.baseEffectBack, n, isSkip, false);
    }

    public virtual void StopCut()
    {
    }

    public void StopEffect(bool isSkip)
    {
        CommonEffectManager.Stop(this.baseEffect, isSkip, false);
    }

    public void StopEffect(string n, bool isSkip)
    {
        CommonEffectManager.Stop(this.baseEffect, n, isSkip, false);
    }

    public void StopShadowEffect()
    {
        this.isShadowEffect = false;
        CommonEffectManager.Stop(this.baseShadowEffect, false, false);
    }

    public void StopSpecialEffect()
    {
        ProgramEffectManager.Destory(this.baseSpecialEffect);
    }

    public void StopSpecialEffect(string n)
    {
        ProgramEffectManager.Destory(this.baseSpecialEffect, n);
    }

    public enum ChangeKind
    {
        NONE,
        NORMAL,
        FADE,
        BLINK,
        MAX
    }
}

