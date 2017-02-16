using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class ServantStatusListViewItemDrawFace : ServantStatusListViewItemDraw
{
    [SerializeField]
    protected UICommonButton baseButton;
    [SerializeField]
    protected UICommonButton[] charaLevelButtonList;
    [SerializeField]
    protected UISprite[] charaLevelSpriteList;
    [SerializeField]
    protected UISprite[] charaLevelTitleSpriteList;
    [SerializeField]
    protected UILabel explanationLabel;
    [SerializeField]
    protected UICommonButton[] faceButtonList;
    [SerializeField]
    protected UISprite[] faceFrameSpriteList;
    [SerializeField]
    protected UISprite[] faceSpriteList;

    public override ServantStatusListViewItemDraw.Kind GetKind() => 
        ServantStatusListViewItemDraw.Kind.FACE;

    public override void ModifyFace(ServantStatusListViewItem item)
    {
        this.SetupButton(item, false);
    }

    public override void SetItem(ServantStatusListViewItem item, ServantStatusListViewItemDraw.DispMode mode)
    {
        base.SetItem(item, mode);
        if ((item != null) && (mode != ServantStatusListViewItemDraw.DispMode.INVISIBLE))
        {
            if (item.UserServant != null)
            {
                this.explanationLabel.text = LocalizationManager.Get("SERVANT_STATUS_EXPLANATION_FACE");
            }
            else if (item.UserServantCollection != null)
            {
                this.explanationLabel.text = LocalizationManager.Get("SERVANT_STATUS_EXPLANATION_FACE2");
            }
            else
            {
                this.explanationLabel.text = LocalizationManager.Get("SERVANT_STATUS_EXPLANATION_FACE3");
            }
            int faceLimitCount = item.FaceLimitCount;
            int maxFaceLimitCount = item.MaxFaceLimitCount;
            for (int i = 0; i < this.faceSpriteList.Length; i++)
            {
                if (i <= maxFaceLimitCount)
                {
                    AtlasManager.SetFaceImage(this.faceSpriteList[i], item.SvtId, i);
                    this.faceFrameSpriteList[i].enabled = true;
                }
                else
                {
                    AtlasManager.SetNoMountFace(this.faceSpriteList[i]);
                    this.faceFrameSpriteList[i].enabled = false;
                }
            }
            this.SetupButton(item, true);
        }
    }

    protected void SetupButton(ServantStatusListViewItem item, bool isInit = false)
    {
        int faceLimitCount = item.FaceLimitCount;
        int maxFaceLimitCount = item.MaxFaceLimitCount;
        for (int i = 0; i < this.charaLevelButtonList.Length; i++)
        {
            bool flag = i == faceLimitCount;
            bool flag2 = i <= maxFaceLimitCount;
            bool flag3 = flag2 && ((item.UserServant != null) || (item.UserServantCollection != null));
            if (flag2)
            {
                this.charaLevelTitleSpriteList[i].spriteName = !flag ? ("btn_txt_" + (i + 1) + "_off") : ("btn_txt_" + (i + 1) + "_on");
            }
            else
            {
                this.charaLevelTitleSpriteList[i].spriteName = "btn_txt_4";
            }
            this.charaLevelTitleSpriteList[i].MakePixelPerfect();
            this.charaLevelSpriteList[i].spriteName = !flag ? "btn_bg_20" : "btn_bg_21";
            if (flag && flag3)
            {
                this.faceButtonList[i].SetColliderEnable(false, isInit || !flag3);
                this.charaLevelButtonList[i].SetColliderEnable(false, isInit || !flag3);
            }
            else
            {
                this.faceButtonList[i].SetButtonEnable(!flag && flag3, isInit || !flag3);
                this.charaLevelButtonList[i].SetButtonEnable(!flag && flag3, isInit || !flag3);
            }
        }
    }
}

