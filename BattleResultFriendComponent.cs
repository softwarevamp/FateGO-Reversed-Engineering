using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleResultFriendComponent : BaseMonoBehaviour
{
    public ClassButtonControlComponent classButtonControl;
    public UILabel confLabel;
    public UILabel countLabel;
    private int dispClassId;
    public UILabel errLabel;
    private int followerClassId;
    private int followerStatus;
    private Follower.Type followerType;
    public FriendIconComponent friendIconComp;
    public PlayMakerFSM myFsm;
    public UILabel noButtonLabel;
    private OtherUserGameEntity otherUser;
    public BattleResultComponent parentComp;
    public GameObject root;
    private long targetId;
    public UILabel titleLabel;
    public UILabel yesButtonLabel;

    public void callBackBeginResume()
    {
        this.myFsm.SendEvent("CLOSE");
    }

    public void changeClass(int classPos)
    {
        this.dispClassId = classPos;
        this.friendIconComp.Set(this.otherUser, true, classPos);
    }

    public void ChangeSupportScene()
    {
        if (SingletonMonoBehaviour<SceneManager>.Instance.targetRoot.checkSceneName("BattleScene"))
        {
            ((BattleRootComponent) SingletonMonoBehaviour<SceneManager>.Instance.targetRoot).setCallbackBeginResume(new BattleRootComponent.callBackBeginResume(this.callBackBeginResume));
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            SupportInfoJump data = new SupportInfoJump(this.otherUser, FriendStatus.Kind.SEARCH, false);
            SingletonMonoBehaviour<SceneManager>.Instance.pushScene(SceneList.Type.SupportSelect, SceneManager.FadeType.BLACK, data);
        }
    }

    public void endCloseSHowServant()
    {
        this.myFsm.SendEvent("CLOSE");
    }

    private void EndRequestFriend(string result)
    {
        Debug.Log("EndRequestFriend");
        if (result != "ng")
        {
            Dictionary<string, object> dictionary = JsonManager.getDictionary(result);
            if (dictionary.ContainsKey("message"))
            {
                string str = dictionary["message"].ToString();
                if (!dictionary["status"].ToString().Equals("0") && !string.IsNullOrEmpty(str))
                {
                    SingletonMonoBehaviour<CommonUI>.Instance.OpenNotificationDialog(null, str, new NotificationDialog.ClickDelegate(this.OnEndRequestDialog), -1);
                    return;
                }
            }
            this.myFsm.SendEvent("REQUEST_OK");
        }
        else
        {
            this.myFsm.SendEvent("REQUEST_NG");
        }
    }

    public void EndShowServant(bool flg)
    {
        SingletonMonoBehaviour<CommonUI>.Instance.CloseServantStatusDialog(new System.Action(this.endCloseSHowServant));
    }

    public void friendOffer()
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE2);
        NetworkManager.getRequest<FriendOfferRequest>(new NetworkManager.ResultCallbackFunc(this.EndRequestFriend)).beginRequest(this.targetId);
    }

    public int getExistLeaderInfo(int firstClassId)
    {
        ServantLeaderInfo info = this.otherUser.getServantLeaderInfo(firstClassId);
        if ((info != null) && (info.userSvtId != 0))
        {
            return firstClassId;
        }
        foreach (ServantLeaderInfo info2 in this.otherUser.userSvtLeaderHash)
        {
            if ((info2 != null) && (info2.userSvtId != 0))
            {
                return info2.classId;
            }
        }
        return -1;
    }

    public void Init()
    {
        this.root.SetActive(false);
    }

    public void NoOnClick()
    {
        this.myFsm.SendEvent("NEXT");
    }

    public void onChangeClass(int classPos)
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        this.changeClass(classPos);
    }

    public void OnClickSupportInfo()
    {
        this.myFsm.SendEvent("CHANGE_SUPPORT");
    }

    protected void OnEndRequestDialog(bool isDecide)
    {
        SingletonMonoBehaviour<CommonUI>.Instance.CloseNotificationDialog();
        this.myFsm.SendEvent("REQUEST_OK");
    }

    public void Open()
    {
        if (this.followerType != Follower.Type.NOT_FRIEND)
        {
            this.myFsm.SendEvent("NEXT");
        }
        else if (this.followerType == Follower.Type.NONE)
        {
            this.myFsm.SendEvent("NEXT");
        }
        else
        {
            UserGameEntity entity = UserGameMaster.getSelfUserGame();
            this.countLabel.text = $"{SingletonMonoBehaviour<DataManager>.Instance.getMasterData<TblFriendMaser>(DataNameKind.Kind.TBL_FRIEND).GetFriendSum()}/{entity.friendKeep}";
            this.titleLabel.text = LocalizationManager.Get("RESULT_FRIEND_TITLE");
            string key = $"RESULT_FRIEND_ERR_{this.followerStatus}";
            string str2 = LocalizationManager.Get(key);
            if (!key.Equals(str2))
            {
                this.errLabel.text = str2;
            }
            else
            {
                this.errLabel.text = string.Empty;
            }
            this.yesButtonLabel.text = LocalizationManager.Get("RESULT_FRIEND_REQUEST_YES");
            this.noButtonLabel.text = LocalizationManager.Get("RESULT_FRIEND_REQUEST_NO");
            this.confLabel.text = LocalizationManager.Get("RESULT_FRIEND_CONF");
            this.otherUser = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<OtherUserGameMaster>(DataNameKind.Kind.OTHER_USER_GAME).getEntityFromId<OtherUserGameEntity>(this.targetId);
            this.followerClassId = this.getExistLeaderInfo(this.followerClassId);
            this.friendIconComp.Set(this.otherUser, false, this.followerClassId);
            this.changeClass(this.followerClassId);
            this.root.SetActive(true);
            this.myFsm.SendEvent("END_PROC");
        }
    }

    public void ServantConfClick()
    {
        this.myFsm.SendEvent("OPEN_SERVANT");
    }

    public void setResultData(int infollowerType, int infollowerStatus, long followerId, int infollowerClassId)
    {
        this.followerType = Follower.getType(infollowerType);
        this.followerStatus = infollowerStatus;
        this.targetId = followerId;
        this.followerClassId = infollowerClassId;
        if (this.classButtonControl != null)
        {
            this.classButtonControl.setCursor(infollowerClassId);
            this.classButtonControl.init(new ClassButtonControlComponent.CallbackFunc(this.onChangeClass), false);
        }
    }

    public void ShowServantConf()
    {
        ServantLeaderInfo servantLeaderInfo = this.otherUser.getServantLeaderInfo(this.followerClassId);
        if (servantLeaderInfo != null)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            SingletonMonoBehaviour<CommonUI>.Instance.OpenServantStatusDialog(ServantStatusDialog.Kind.FOLLOWER, servantLeaderInfo, new ServantStatusDialog.ClickDelegate(this.EndShowServant));
        }
    }

    public void YesOnClick()
    {
        this.myFsm.SendEvent("CONNECT");
    }
}

