using System;
using UnityEngine;

public class MyRoomListControl : MonoBehaviour
{
    public static readonly Vector3 BTN_LABEL_POS = new Vector3(0f, 4f, 0f);
    public GameObject continueDeviceObject;
    public GameObject copyrightObject;
    public GameObject developmentTeamObject;
    public UIButton favoriteChangeMenuBtn;
    public GameObject feedbackQuestionObject;
    public GameObject gameDescriptionObject;
    public GameObject gameNoticeObject;
    private bool isUseFavorite;
    public UIScrollView menuListScrollView;
    public MyRoomControl myRoomControl;
    public PlayMakerFSM myRoomFsm;
    public GameObject[] myRoomItems;
    public UIGrid myRoomListGrid;
    public GameObject[] noticeItems;
    public UIGrid noticeListGrid;
    public UIScrollView noticeListScrollView;
    public GameObject noticeMenuObject;
    public GameObject serialCodeMenuObject;
    public GameObject usetTermsObject;

    private void callBackNotificationDlg()
    {
        SingletonMonoBehaviour<CommonUI>.Instance.CloseNotificationDialog();
    }

    public void ClickFavoriteChange()
    {
        if (!this.isUseFavorite)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
            string longName = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.WAR).getEntityFromId<WarEntity>(ConstantMaster.getValue("FIRST_WAR_ID")).longName;
            string message = string.Format(LocalizationManager.Get("MYROOM_MENU_FAVORITE_INFO_TXT"), longName);
            SingletonMonoBehaviour<CommonUI>.Instance.OpenNotificationDialog(string.Empty, message, new System.Action(this.callBackNotificationDlg), -1);
        }
        else
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.myRoomFsm.SendEvent("CLICK_FAVORITE_CHANGE");
        }
    }

    private void SetBtnName(GameObject[] items, string prefix_str)
    {
        for (int i = 0; i < items.Length; i++)
        {
            SetMenuNameControl component = items[i].GetComponent<SetMenuNameControl>();
            if (component != null)
            {
                component.setMenuName(LocalizationManager.Get(prefix_str + i));
            }
        }
    }

    private void setEnableBtn()
    {
        if (!this.isUseFavorite)
        {
            this.favoriteChangeMenuBtn.defaultColor = Color.gray;
            this.favoriteChangeMenuBtn.hover = Color.gray;
            this.favoriteChangeMenuBtn.disabledColor = Color.gray;
        }
    }

    public void Setup()
    {
        if (BalanceConfig.IsIOS_Examination)
        {
            this.noticeMenuObject.SetActive(!BalanceConfig.IsIOS_Examination);
        }
        this.isUseFavorite = TutorialFlag.Get(TutorialFlag.Id.TUTORIAL_LABEL_FAVORITE2);
        bool flag = BalanceConfig.SerialCodeMenuDispFlg == 1;
        this.serialCodeMenuObject.SetActive(flag);
        if (NetworkManager.PlatformManagement[5] == 0)
        {
            this.continueDeviceObject.SetActive(false);
        }
        if (NetworkManager.PlatformManagement[6] == 0)
        {
            this.gameDescriptionObject.SetActive(false);
        }
        if (NetworkManager.PlatformManagement[1] == 0)
        {
            this.feedbackQuestionObject.SetActive(false);
        }
        if (NetworkManager.PlatformManagement[2] == 0)
        {
            this.usetTermsObject.SetActive(false);
        }
        if (NetworkManager.PlatformManagement[3] == 0)
        {
            this.developmentTeamObject.SetActive(false);
        }
        if (NetworkManager.PlatformManagement[4] == 0)
        {
            this.copyrightObject.SetActive(false);
        }
        this.myRoomListGrid.Reposition();
        this.SetupScrollListBtn(this.myRoomListGrid);
        this.myRoomControl.noticeComp.gameObject.SetActive(true);
        this.noticeListGrid.Reposition();
        this.SetupScrollListBtn(this.noticeListGrid);
        this.SetBtnName(this.myRoomItems, "MYROOM_MENU_NAME_");
        this.SetBtnName(this.noticeItems, "MYROOM_NOTICE_NAME_");
        this.myRoomControl.noticeComp.gameObject.SetActive(false);
        this.menuListScrollView.SetDragAmount(0f, 0f, false);
        this.setEnableBtn();
    }

    private void SetupScrollListBtn(UIGrid grid)
    {
        int childCount = grid.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform child = grid.transform.GetChild(i);
            if (child != null)
            {
                UISprite component = child.GetComponent<UISprite>();
                if (component == null)
                {
                    component = child.GetComponentInChildren<UISprite>();
                }
                if (component != null)
                {
                    component.MakePixelPerfect();
                    UIButton button = child.gameObject.SafeGetComponent<UIButton>();
                    button.tweenTarget = component.gameObject;
                    button.hover = Color.white;
                    button.disabledColor = Color.white;
                }
                UILabel componentInChildren = child.GetComponentInChildren<UILabel>();
                if (componentInChildren != null)
                {
                    componentInChildren.gameObject.SetLocalPosition(BTN_LABEL_POS);
                }
            }
        }
    }
}

