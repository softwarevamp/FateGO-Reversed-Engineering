using System;
using UnityEngine;

public class ServantStatusVoiceListViewItemDraw : MonoBehaviour
{
    [SerializeField]
    protected UICommonButton baseButton;
    [SerializeField]
    protected UILabel nameLabel;
    [SerializeField]
    protected UISprite playIconSprite;
    [SerializeField]
    protected UISprite typeSpite;

    public void SetInput(ServantStatusVoiceListViewItem item, bool isInput)
    {
        if ((item != null) && item.IsCanPlay)
        {
            this.baseButton.SetEnable(true);
            this.baseButton.SetColliderEnable(isInput, true);
        }
        else
        {
            this.baseButton.SetEnable(false);
        }
    }

    public void SetItem(ServantStatusVoiceListViewItem item, DispMode mode)
    {
        if ((item != null) && (mode != DispMode.INVISIBLE))
        {
            string str = null;
            switch (item.PlayType)
            {
                case SvtVoiceType.Type.HOME:
                    str = "img_txt_myroom";
                    break;

                case SvtVoiceType.Type.GROETH:
                    str = "img_txt_synthesis";
                    break;

                case SvtVoiceType.Type.FIRST_GET:
                case SvtVoiceType.Type.EVENT_JOIN:
                case SvtVoiceType.Type.EVENT_REWARD:
                    str = "img_txt_other";
                    break;

                case SvtVoiceType.Type.BATTLE:
                case SvtVoiceType.Type.TREASURE_DEVICE:
                    str = "img_txt_battle";
                    break;
            }
            this.typeSpite.spriteName = str;
            if (str != null)
            {
                this.typeSpite.MakePixelPerfect();
            }
            this.nameLabel.text = item.Name;
            TweenColor component = this.playIconSprite.GetComponent<TweenColor>();
            if (component != null)
            {
                component.enabled = false;
            }
            this.playIconSprite.color = (!item.IsPlay && item.IsCanPlay) ? Color.white : Color.gray;
        }
    }

    public void SetPlay(ServantStatusVoiceListViewItem item)
    {
        TweenColor.Begin(this.playIconSprite.gameObject, 0.1f, (!item.IsPlay && item.IsCanPlay) ? Color.white : Color.gray);
    }

    public enum DispMode
    {
        INVISIBLE,
        INVALID,
        VALID,
        INPUT
    }
}

