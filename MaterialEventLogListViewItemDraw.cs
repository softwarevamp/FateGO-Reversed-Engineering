using System;
using UnityEngine;

public class MaterialEventLogListViewItemDraw : MonoBehaviour
{
    public static readonly string BASE_SP_NAME_DEFAULT = "img_menuboard_01";
    public static readonly string BASE_SP_NAME_SVT = "img_menuboard_03";
    public static readonly int FACE_MASK_SP_W = 0x73;
    public static readonly int LABEL_MAX_LINE_DEFAULT = 2;
    public static readonly int LABEL_MAX_LINE_SVT = 1;
    public static readonly int LABEL_W_DEFAULT = 0x1a6;
    public static readonly int LABEL_W_SVT = 0x159;
    public static readonly float LABEL_X_DEFAULT;
    public static readonly float LABEL_X_SVT = 40f;
    [SerializeField]
    private UISprite mBaseSp;
    [SerializeField]
    private ItemIconComponent mFaceIcon;
    [SerializeField]
    private UISprite mFaceMaskSp;
    [SerializeField]
    private GameObject mFaceObj;
    [SerializeField]
    private UILabel mLabel;

    public void LateUpdateItem(MaterialEventLogListViewItem item, DispMode mode)
    {
        if ((item != null) && (item.info != null))
        {
            MaterialEventLogListViewItem.Info info = item.info;
            if (mode != DispMode.INVISIBLE)
            {
                Color col = this.mBaseSp.color;
                this.mFaceIcon.SetColor(col);
                this.mFaceMaskSp.color = col;
            }
        }
    }

    private void SetFaceImage(MaterialEventLogListViewItem item, bool is_disp)
    {
        MaterialEventLogListViewItem.Info info = item.info;
        this.mFaceObj.SetActive(is_disp);
        if (is_disp)
        {
            this.mFaceIcon.SetFaceImage(info.svt_id, info.limit_count, -1);
            this.mFaceMaskSp.GetAtlasSprite().width = FACE_MASK_SP_W - 1;
        }
    }

    public void SetInput(MaterialEventLogListViewItem item, bool isInput)
    {
    }

    public void SetItem(MaterialEventLogListViewItem item, DispMode mode)
    {
        if (item != null)
        {
            MaterialEventLogListViewItem.Info info = item.info;
            if (info == null)
            {
                base.gameObject.SetLocalScale(Vector3.zero);
            }
            else if (mode != DispMode.INVISIBLE)
            {
                base.gameObject.SetLocalScale(Vector3.one);
                bool flag = (info.flag & MaterialEventLogListViewItem.Flag.SVT_FACE) != MaterialEventLogListViewItem.Flag.NONE;
                this.mBaseSp.spriteName = !flag ? BASE_SP_NAME_DEFAULT : BASE_SP_NAME_SVT;
                this.mBaseSp.MakePixelPerfect();
                this.mLabel.fontSize = info.font_size;
                float x = !flag ? LABEL_X_DEFAULT : LABEL_X_SVT;
                this.mLabel.gameObject.SetLocalPositionX(x);
                int num2 = !flag ? LABEL_W_DEFAULT : LABEL_W_SVT;
                this.mLabel.gameObject.GetComponent<UIWidget>().width = num2;
                int num3 = !flag ? LABEL_MAX_LINE_DEFAULT : LABEL_MAX_LINE_SVT;
                this.mLabel.maxLineCount = num3;
                this.mLabel.text = info.str;
                this.SetFaceImage(item, flag);
            }
        }
    }

    public enum DispMode
    {
        INVISIBLE,
        INVALID,
        VALID,
        INPUT
    }
}

