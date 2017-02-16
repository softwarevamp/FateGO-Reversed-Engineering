using System;
using System.Collections.Generic;
using UnityEngine;

public class GameHelpListViewObject : ListViewObject
{
    [SerializeField]
    public List<GameObject> childList = new List<GameObject>();
    protected GameObject dragObject;
    public GameHelpItem gameHelpItem;
    [SerializeField]
    public GameObject jianTou;
    public UISprite jiaSprite;
    protected LabelState labelState = LabelState.CLOSE;
    [SerializeField]
    public UILabel mLabel;
    public LabelType mLabelType;
    public UISprite mSprite;
    public UITable myGameHelpListGrid;
    [SerializeField]
    public NoticeInfoComponent noticeInfoComponent;

    protected void Awake()
    {
        base.Awake();
    }

    private void ChangeSpriteColor(bool isGray)
    {
        this.mSprite.color = !isGray ? Color.white : Color.gray;
        this.jiaSprite.spriteName = !isGray ? "jh2" : "jh";
    }

    public void init(GameHelpItem itemData, LabelType type)
    {
        this.gameHelpItem = itemData;
        this.mLabelType = type;
        if (type != LabelType.TEXT)
        {
            this.mLabel.text = (type != LabelType.MAIN) ? itemData.mLabelName : itemData.mTitleName;
        }
        else
        {
            this.mLabel.text = itemData.mDetial;
            this.SetSpriteBgSize();
        }
    }

    public void OnClick()
    {
        if (base.mPressState != ListViewObject.PressState.STATE_STOP)
        {
            if (this.mLabelType == LabelType.MAIN)
            {
                this.labelState = (this.labelState != LabelState.CLOSE) ? LabelState.CLOSE : LabelState.OPEN;
                this.UpdataMainLabelMarkDirection(this.labelState);
                this.UpdataChildLabel();
            }
            else if (this.mLabelType == LabelType.CHILD)
            {
                this.labelState = (this.labelState != LabelState.CLOSE) ? LabelState.CLOSE : LabelState.OPEN;
                this.ChangeSpriteColor(this.labelState == LabelState.OPEN);
                this.UpdataChildLabel();
            }
        }
    }

    private void SetSpriteBgSize()
    {
        this.mSprite.SetDimensions(0x35e, this.mLabel.height + 20);
        base.gameObject.transform.GetComponent<BoxCollider>().size = new Vector3(862f, (float) (this.mLabel.height + 20), 0f);
    }

    public void ShowHelpInfo(bool isShow)
    {
        Debug.LogError("info is Show");
        this.noticeInfoComponent.title = this.gameHelpItem.mLabelName;
        this.noticeInfoComponent.path = "userpolicy/index.html";
        this.noticeInfoComponent.dynamicPath = string.Empty;
        NoticeInfoComponent.TermID = this.gameHelpItem.mDetial;
        NoticeInfoComponent.messageIsLabel = true;
        this.noticeInfoComponent.myRoomFsm.SendEvent("CLICK_OPENING");
    }

    public void UpdataChildLabel()
    {
        if (this.childList.Count > 0)
        {
            foreach (GameObject obj2 in this.childList)
            {
                obj2.SetActive(this.labelState == LabelState.OPEN);
                if ((this.mLabelType == LabelType.MAIN) && (this.labelState == LabelState.CLOSE))
                {
                    GameHelpListViewObject component = obj2.transform.GetComponent<GameHelpListViewObject>();
                    if ((component != null) && (component.childList.Count > 0))
                    {
                        component.labelState = LabelState.CLOSE;
                        component.ChangeSpriteColor(false);
                        foreach (GameObject obj4 in component.childList)
                        {
                            obj4.SetActive(false);
                        }
                    }
                }
            }
            this.myGameHelpListGrid.Reposition();
        }
    }

    private void UpdataMainLabelMarkDirection(LabelState state)
    {
        this.jianTou.transform.Rotate(new Vector3(0f, 0f, (state != LabelState.CLOSE) ? ((float) (-90)) : ((float) 90)));
    }

    public enum LabelState
    {
        OPEN,
        CLOSE
    }

    public enum LabelType
    {
        MAIN,
        CHILD,
        TEXT
    }
}

