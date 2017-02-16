using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SetBoxGachaResourceControl : MonoBehaviour
{
    public UIButton boxGachaDetailBtn;
    protected ClickDelegate clickCallbackFunc;
    public EventItemComponent eventBoxGachaItemInfo;
    private int gachaTime;
    private int itemId;
    public BoxGachaItemComponent multiBoxGachaInfo;
    public GameObject multiBoxGachaObj;
    public UISprite multiPointNumImg;
    public UIButton oneBoxGachaBtn;
    public BoxGachaItemComponent oneBoxGachaInfo;
    public GameObject oneBoxGachaObj;
    private int payValue;
    public BoxGachaItemComponent singleBoxGachaInfo;

    public void ClickMultiGacha()
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE2);
        Debug.Log("!!**!! ClickMultiGacha GachaTime: " + this.gachaTime);
        if (this.clickCallbackFunc != null)
        {
            this.clickCallbackFunc(this.gachaTime);
        }
    }

    public void ClickOneGacha()
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE2);
        Debug.Log("!!**!! ClickOneGacha GachaTime: " + this.gachaTime);
        this.gachaTime = 1;
        if (this.clickCallbackFunc != null)
        {
            this.clickCallbackFunc(this.gachaTime);
        }
    }

    public void init(BoxGachaEntity boxGachaEnt)
    {
        this.gachaTime = 10;
        this.itemId = boxGachaEnt.payTargetId;
        this.payValue = boxGachaEnt.payValue;
        this.eventBoxGachaItemInfo.Set(this.itemId);
        this.oneBoxGachaInfo.Set(this.itemId, this.payValue);
        this.singleBoxGachaInfo.Set(this.itemId, this.payValue);
        this.multiBoxGachaInfo.Set(this.itemId, this.payValue * this.gachaTime);
    }

    public void setBoxGachaItemInfo(int canDrawNum, ClickDelegate callback)
    {
        this.clickCallbackFunc = callback;
        this.gachaTime = canDrawNum;
        this.oneBoxGachaObj.SetActive(false);
        this.multiBoxGachaObj.SetActive(false);
        if (canDrawNum >= 2)
        {
            this.multiBoxGachaObj.SetActive(true);
            this.multiBoxGachaInfo.Set(this.itemId, this.payValue * this.gachaTime);
            string str = "img_gachatxt_0" + this.gachaTime;
            if (this.gachaTime >= 10)
            {
                this.gachaTime = 10;
                str = "img_gachatxt_10";
            }
            this.multiPointNumImg.spriteName = str;
            this.multiPointNumImg.MakePixelPerfect();
        }
        else
        {
            this.oneBoxGachaBtn.isEnabled = this.gachaTime > 0;
            this.oneBoxGachaObj.SetActive(true);
        }
    }

    public delegate void ClickDelegate(int gachaTime);
}

