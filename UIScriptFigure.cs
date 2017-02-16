﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class UIScriptFigure : UIScriptChara
{
    [CompilerGenerated]
    private static Dictionary<string, int> <>f__switch$map30;
    [SerializeField]
    protected UIStandFigureM[] standFigureList = new UIStandFigureM[2];

    public override void ChangeCharacter(UIScriptChara.ChangeKind kind, float speed, string imageName, Face.Type faceType, System.Action callback)
    {
        int servantId = UIStandFigureRender.GetServantId(imageName);
        int imageLimitCount = UIStandFigureRender.GetImageLimitCount(imageName);
        base.ChangeCharacter(kind, speed, imageName, faceType, callback);
        UIStandFigureM em = this.standFigureList[base.mainIndex];
        UIStandFigureM em2 = this.standFigureList[base.backIndex];
        base.isDisp = false;
        em.SetActive(true);
        em.SetAlpha(0f);
        em.SetDepth(0.1f);
        em2.SetDepth(0f);
        em.SetShadow(base.isShadow);
        em.SetCharacter(servantId, imageLimitCount, faceType, new System.Action(this.UpdateChange));
    }

    protected void ContinueChangeFade()
    {
        this.SetOverlap(1f);
        base.Invoke("UpdateChange", base.changeStep);
    }

    protected void EndChange()
    {
        UIStandFigureM em = this.standFigureList[base.mainIndex];
        UIStandFigureM em2 = this.standFigureList[base.backIndex];
        em.SetAlpha(1f);
        em2.SetAlpha(0f);
        em2.ReleaseCharacter();
        base.changeKind = UIScriptChara.ChangeKind.NONE;
        if (base.changeCallback != null)
        {
            System.Action changeCallback = base.changeCallback;
            base.changeCallback = null;
            changeCallback();
        }
    }

    protected override void EndSet()
    {
        UIStandFigureM em = this.standFigureList[base.mainIndex];
        em.SetDispOffset();
        Vector2 centerOffset = em.GetCenterOffset();
        float z = base.baseSpecialEffect.transform.localPosition.z;
        base.baseSpecialEffect.transform.localPosition = new Vector3(centerOffset.x, centerOffset.y, z);
        z = base.baseEffect.transform.localPosition.z;
        base.baseEffect.transform.localPosition = new Vector3(centerOffset.x, centerOffset.y, z);
        z = base.baseEffectBack.transform.localPosition.z;
        base.baseEffectBack.transform.localPosition = new Vector3(centerOffset.x, centerOffset.y, z);
        z = base.baseShadowEffect.transform.localPosition.z;
        base.baseShadowEffect.transform.localPosition = new Vector3(centerOffset.x, centerOffset.y, z);
        if (base.setCallback != null)
        {
            System.Action setCallback = base.setCallback;
            base.setCallback = null;
            setCallback();
        }
    }

    public override bool IsBusyMoveAlpha()
    {
        UIStandFigureM em = this.standFigureList[base.mainIndex];
        return em.IsBusyMoveAlpha();
    }

    public override void MoveAlpha(float duration, float a)
    {
        base.isDisp = a > 0f;
        this.standFigureList[base.mainIndex].MoveAlpha(duration, a, null, null);
        base.RecoverShadowEffect();
    }

    public override void SetAlpha(float a)
    {
        base.isDisp = a > 0f;
        this.standFigureList[base.mainIndex].SetAlpha(a);
        base.RecoverShadowEffect();
    }

    public override void SetCharacter(string imageName, Face.Type faceType, System.Action callback)
    {
        base.setCallback = callback;
        int servantId = UIStandFigureRender.GetServantId(imageName);
        int imageLimitCount = UIStandFigureRender.GetImageLimitCount(imageName);
        UIStandFigureM em = this.standFigureList[base.mainIndex];
        em.SetFilter(base.filterName, base.filterColor);
        em.SetCharacter(servantId, imageLimitCount, faceType, new System.Action(this.EndSet));
    }

    public override void SetDepth(int d)
    {
        UIStandFigureM em = this.standFigureList[base.mainIndex];
        Vector3 localPosition = base.baseDepth.localPosition;
        localPosition.z = -d * 10f;
        base.baseDepth.localPosition = localPosition;
        em.SetDepth(0.1f);
    }

    public override void SetFace(Face.Type faceType)
    {
        this.standFigureList[base.mainIndex].SetFace(faceType);
    }

    public override void SetFilter(string filterName, Color filterColor)
    {
        base.filterName = filterName;
        base.filterColor = filterColor;
        this.standFigureList[base.mainIndex].SetFilter(base.filterName, base.filterColor);
    }

    protected void SetOverlap(float a)
    {
        UIStandFigureM em = this.standFigureList[base.mainIndex];
        UIStandFigureM em2 = this.standFigureList[base.backIndex];
        em.SetAlpha(a);
        em2.SetAlpha(1f - a);
    }

    public override void SetShadow(bool isShadow)
    {
        if (base.isShadow != isShadow)
        {
            base.isShadow = isShadow;
            this.standFigureList[base.mainIndex].SetShadow(isShadow);
            if (base.backIndex >= 0)
            {
                this.standFigureList[base.backIndex].SetShadow(isShadow);
            }
            base.RecoverShadowEffect();
        }
    }

    public override void SetSpecialEffect(string n, Vector3 pos, float time, Color color, bool isSkip)
    {
        GameObject obj2 = ProgramEffectManager.CreateCharaEffect(base.baseSpecialEffect, n, pos, time, color, isSkip);
        if (obj2 != null)
        {
            UIStandFigureM figure = this.standFigureList[base.mainIndex];
            string key = n;
            if (key != null)
            {
                int num;
                if (<>f__switch$map30 == null)
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
                    <>f__switch$map30 = dictionary;
                }
                if (<>f__switch$map30.TryGetValue(key, out num))
                {
                    switch (num)
                    {
                        case 0:
                            obj2.GetComponent<CharaErasureEffectComponent>().ErasureStart(figure);
                            base.StopShadowEffect();
                            break;

                        case 1:
                            obj2.GetComponent<CharaErasureReverseEffectComponent>().ErasureStart(figure);
                            base.StopShadowEffect();
                            break;

                        case 2:
                            obj2.GetComponent<CharaAppearanceEffectComponent>().AppearanceStart(figure);
                            base.StopShadowEffect();
                            break;

                        case 3:
                            obj2.GetComponent<CharaAppearanceReverseEffectComponent>().AppearanceStart(figure);
                            base.StopShadowEffect();
                            break;

                        case 4:
                            obj2.GetComponent<CharaWipeEffectComponent>().WipeStart(figure);
                            base.StopShadowEffect();
                            break;

                        case 5:
                            obj2.GetComponent<CharaFlashEffectComponent>().FlashStart(figure);
                            break;
                    }
                }
            }
        }
    }

    public override void SetTalkMask(bool isMask)
    {
        this.standFigureList[base.mainIndex].SetTalkMask(isMask);
    }

    protected void UpdateChange()
    {
        UIStandFigureM em = this.standFigureList[base.mainIndex];
        UIStandFigureM em2 = this.standFigureList[base.backIndex];
        em.SetDispOffset();
        switch (base.changeKind)
        {
            case UIScriptChara.ChangeKind.NORMAL:
                this.EndChange();
                break;

            case UIScriptChara.ChangeKind.FADE:
                em2.MoveAlpha(base.changeSpeed, 0f, null, null);
                em.MoveAlpha(base.changeSpeed, 1f, base.gameObject, "EndChange");
                break;

            case UIScriptChara.ChangeKind.BLINK:
            {
                float num = base.changeTotal / base.changeSpeed;
                if (num < 1f)
                {
                    float num3;
                    this.SetOverlap(0f);
                    float num2 = 1f - num;
                    if (num <= 0.5f)
                    {
                        num3 = (0.5f - num) * base.changeRange;
                        if (num3 > 1f)
                        {
                            num3 = 1f;
                        }
                        base.changeTotal += num3 + (UIScriptChara.UNIT_SPEED * 2f);
                        base.changeStep = UIScriptChara.UNIT_SPEED;
                        Debug.Log(string.Concat(new object[] { "charaChange BLINK ", base.changeTotal, " A[", num3, "]" }));
                        base.Invoke("ContinueChangeFade", num3 + UIScriptChara.UNIT_SPEED);
                    }
                    else
                    {
                        num3 = (0.5f - num2) * base.changeRange;
                        if (num3 > 1f)
                        {
                            num3 = 1f;
                        }
                        base.changeTotal += num3 + (UIScriptChara.UNIT_SPEED * 2f);
                        base.changeStep = num3 + UIScriptChara.UNIT_SPEED;
                        Debug.Log(string.Concat(new object[] { "charaChange BLINK ", base.changeTotal, " B[", num3, "]" }));
                        base.Invoke("ContinueChangeFade", UIScriptChara.UNIT_SPEED);
                    }
                    break;
                }
                this.EndChange();
                break;
            }
        }
    }
}

