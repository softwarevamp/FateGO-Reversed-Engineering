using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MstProfileComponent : MonoBehaviour
{
    public GameObject birthDayInfo;
    public UILabel birthDayLb;
    public ChangeGenderTypeWindow changeGenderWindow;
    public ChangeUserNameWindow changeNameWindow;
    private int cmdCnt;
    public CommandSpellIconComponent cmdSpellIconFemale;
    public CommandSpellIconComponent cmdSpellIconMale;
    public GameObject cmdSpellIconObject;
    public GameObject cmdSpellRecTimeInfo;
    public CommandSpellWindowComponent cmdSpellWinComp;
    public GameObject cmdSpellWinInfo;
    private int currentGenderType;
    public UILabel currentManaLb;
    public UILabel currentQpLb;
    public UILabel currentStoneLb;
    private DateTime dtBirthDay;
    public UISlider expBar;
    public MstExpInfoComponent expInfoWindow;
    public UILabel friendPointLb;
    public UILabel genderTypeLb;
    public InputUserBirthDayWindow inputBirthDayWindow;
    private UIMasterFigureTextureOld masterFigure;
    public GameObject mstImgBase;
    public MyRoomData myRoomData;
    public PlayMakerFSM myRoomFsm;
    public UILabel recoverTimeLb;
    public UILabel recoverTitleLb;
    public UILabel seqLoginLb;
    public GameObject setBirthBtn;
    public UILabel svtEqNumLb;
    public UILabel svtNumLb;
    public UILabel totalLoginLb;
    public UILabel userExpLb;
    public UIExtrusionLabel userLevelLb;
    public UILabel userMaxLvLb;
    public UILabel userNameTxt;
    public UILabel userSvtNumLb;
    private MstProfileData usrData;
    public UILabel usrEquipNameLb;
    private UserGameEntity usrGameEnt;

    private void callbackGenderChange(string result)
    {
        this.myRoomData.setUserInfoData();
        this.setGenderInfo();
    }

    private void callbackSetBirthDay(string result)
    {
        this.myRoomData.setUserInfoData();
        this.setBirthDayInfo();
    }

    public void checkInput()
    {
        this.myRoomFsm.SendEvent("CHECK_OK");
    }

    public void closeChangeDlg()
    {
        this.changeNameWindow.Close();
    }

    public void CloseCmdSpell()
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
        this.cmdSpellWinInfo.SetActive(false);
        this.cmdSpellWinComp.Close(null);
    }

    public void closeGenderChangeDlg()
    {
        this.changeGenderWindow.Close();
    }

    public void closeSetBirthDayDlg()
    {
        this.inputBirthDayWindow.Close();
    }

    private void dispSetBirth(System.Action callback)
    {
        int month = this.dtBirthDay.Month;
        int day = this.dtBirthDay.Day;
        this.birthDayLb.text = $"{month}月 {day}日";
        this.birthDayInfo.SetActive(true);
        callback();
    }

    private void endOpenChangeDlg(bool res, string changeName)
    {
        if (res)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            if (SingletonMonoBehaviour<AssetManager>.Instance.IsShieldingWord(changeName))
            {
                this.myRoomFsm.SendEvent("CLICK_CANCLE");
                SingletonMonoBehaviour<CommonUI>.Instance.OpenWarningDialog("[FFFF80]用户名不正确", "用户名包含敏感词", null, false);
            }
            else
            {
                this.OnClickSubmit(changeName);
            }
        }
        else
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
            this.myRoomFsm.SendEvent("CLICK_CANCLE");
        }
    }

    private void endOpenGenderChange(bool res, int genderType)
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        if (res)
        {
            this.requestGenderChange(genderType);
        }
        else
        {
            this.closeGenderChangeDlg();
        }
    }

    private void endOpenSetBirthDay(bool res, int[] paramList)
    {
        if (res)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.requestSetBirthDay(paramList[0], paramList[1]);
        }
        else
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
            this.closeSetBirthDayDlg();
        }
    }

    private void getCurrentUserData()
    {
        this.usrData = this.myRoomData.getMstInfoData();
    }

    public void hideMstProfile()
    {
        if (this.masterFigure != null)
        {
            UnityEngine.Object.Destroy(this.masterFigure.gameObject);
            this.masterFigure = null;
        }
        this.expInfoWindow.Close();
    }

    public void OnClickCmdSpell()
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        Debug.Log("!! ** OnClickCmdSpell");
        this.cmdSpellWinInfo.SetActive(true);
        this.cmdSpellWinComp.Open(null);
    }

    public void OnClickSubmit(string changeName)
    {
        this.myRoomFsm.Fsm.Variables.GetFsmString("ChangeUserName").Value = changeName;
        this.myRoomFsm.SendEvent("CLICK_SUBMIT");
    }

    public void OpenExpInfo()
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        this.expInfoWindow.openExpInfo(this.usrGameEnt.exp, false);
    }

    public void requestGenderChange(int genderType)
    {
        string userName = this.usrData.userName;
        NetworkManager.getRequest<UserNameChangeRequest>(new NetworkManager.ResultCallbackFunc(this.callbackGenderChange)).beginRequest(userName, genderType, 0);
    }

    public void requestSetBirthDay(int month, int day)
    {
        string userName = this.usrData.userName;
        int genderType = this.usrData.genderType;
        NetworkManager.getRequest<SetUserBirthDayRequest>(new NetworkManager.ResultCallbackFunc(this.callbackSetBirthDay)).beginRequest(userName, genderType, month, day);
    }

    public void resetInput()
    {
        this.changeNameWindow.resetInputVal();
    }

    public void setBirthDayInfo()
    {
        this.getCurrentUserData();
        this.setUserBirthDay();
    }

    private void setCmdSpellImg()
    {
        int genderType = this.usrGameEnt.genderType;
        this.setCmdSpellInfo(genderType);
        this.cmdSpellWinComp.InitializeCommandSpell(CommandSpellWindowComponent.MODE.NO_BATTLE);
        this.cmdSpellWinComp.setCallBackPushClose(new CommandSpellWindowComponent.CloseButtonCallBack(this.CloseCmdSpell));
    }

    private void setCmdSpellInfo(int genderType)
    {
        if (genderType == 1)
        {
            this.cmdSpellIconFemale.gameObject.SetActive(false);
            this.cmdSpellIconMale.SetData(this.usrGameEnt);
            Vector2 sz = new Vector2(130f, 130f);
            this.cmdSpellIconMale.SetSize(sz);
            this.cmdSpellIconMale.gameObject.SetActive(true);
        }
        else if (genderType == 2)
        {
            this.cmdSpellIconMale.gameObject.SetActive(false);
            this.cmdSpellIconFemale.SetData(this.usrGameEnt);
            Vector2 vector2 = new Vector2(130f, 130f);
            this.cmdSpellIconFemale.SetSize(vector2);
            this.cmdSpellIconFemale.gameObject.SetActive(true);
        }
    }

    private void setCmdSpellRecoverTime(long recoverTime)
    {
        if (recoverTime <= 0L)
        {
            this.cmdSpellRecTimeInfo.SetActive(false);
        }
        else
        {
            int num = (int) (recoverTime / 0xe10L);
            int num2 = ((int) (recoverTime / 60L)) % 60;
            int num3 = (int) (recoverTime % 60L);
            this.recoverTitleLb.text = LocalizationManager.Get("COMMNAD_SPELL_RECV_TXT");
            this.recoverTimeLb.text = string.Format(LocalizationManager.Get("COMMAND_SPELL_RECOVER_TIME"), num, num2, num3);
        }
    }

    private void setCommandSpellInfo()
    {
        int num;
        long num2;
        this.usrGameEnt.getCmdSpellInfo(out num, out num2);
        this.setCmdSpellRecoverTime(num2);
        if (this.cmdCnt != num)
        {
            this.cmdCnt = num;
            this.setCmdSpellImg();
        }
    }

    private void setCurrentMana()
    {
        this.currentManaLb.text = LocalizationManager.GetUnitInfo(this.usrData.currentMana);
    }

    private void setCurrentQp()
    {
        this.currentQpLb.text = string.Format(LocalizationManager.Get("CURRENT_QP_UNIT"), this.usrData.currentQp);
    }

    private void setCurrentStone()
    {
        this.currentStoneLb.text = LocalizationManager.GetUnitInfo(this.usrData.currentStone);
    }

    private void setFriendPoint()
    {
        this.friendPointLb.text = string.Format(LocalizationManager.Get("CURRENT_FRIEND_POINT_UNIT"), this.usrData.friendPoint);
    }

    public void setGenderInfo()
    {
        this.usrGameEnt = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
        this.getCurrentUserData();
        this.setUserGender();
        this.setMstImg();
    }

    private void setMstImg()
    {
        <setMstImg>c__AnonStorey88 storey = new <setMstImg>c__AnonStorey88 {
            <>f__this = this,
            genderType = this.usrData.genderType
        };
        int equipId = 0;
        if (this.usrData.userEquipId > 0L)
        {
            equipId = SingletonMonoBehaviour<DataManager>.getInstance().getMasterData(DataNameKind.Kind.USER_EQUIP).getEntityFromId<UserEquipEntity>(this.usrData.userEquipId).equipId;
        }
        if (this.masterFigure == null)
        {
            this.masterFigure = MasterFigureManagerOld.CreatePrefab(this.mstImgBase, UIMasterFigureRenderOld.DispType.MY_ROOM, storey.genderType, equipId, 10, null);
        }
        else
        {
            this.masterFigure.SetCharacter(UIMasterFigureRenderOld.DispType.MY_ROOM, storey.genderType, equipId, new System.Action(storey.<>m__11A));
        }
    }

    private void setSvtNum()
    {
        this.svtNumLb.text = string.Format(LocalizationManager.Get("SUM_INFO"), this.usrData.currentSvtNum, this.usrData.maxSvtNum);
        this.userSvtNumLb.text = string.Format(LocalizationManager.Get("SUM_INFO"), this.usrData.currentSvtNum, this.usrData.maxSvtNum);
        this.svtEqNumLb.text = string.Format(LocalizationManager.Get("SUM_INFO"), this.usrData.currentSvtEpNum, this.usrData.maxSvtEqNum);
    }

    private void setUserBirthDay()
    {
        long birthDayVal = this.usrData.birthDayVal;
        if (birthDayVal <= 0L)
        {
            this.birthDayInfo.SetActive(false);
        }
        else
        {
            this.setBirthBtn.SetActive(false);
            this.dtBirthDay = NetworkManager.getDateTime(birthDayVal);
            this.dispSetBirth(new System.Action(this.closeSetBirthDayDlg));
        }
    }

    private void setUserExp()
    {
        this.userExpLb.text = this.usrData.lateExp.ToString("#,0");
        this.expBar.value = this.usrData.barExp;
    }

    private void setUserGender()
    {
        this.currentGenderType = this.usrData.genderType;
        this.genderTypeLb.text = Gender.ToName((Gender.Type) this.currentGenderType);
    }

    private void setUserLv()
    {
        this.userLevelLb.text = string.Empty + this.usrData.userLv;
        this.userMaxLvLb.text = string.Empty + BalanceConfig.UserLevelMax;
    }

    private void setUserName()
    {
        this.userNameTxt.text = this.usrData.userName;
    }

    public void showChangeDlg()
    {
        this.changeNameWindow.OpenChangeNameWindow(this.usrData.userName, new ChangeUserNameWindow.CallbackFunc(this.endOpenChangeDlg));
    }

    public void showGenderChangeDlg()
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        this.changeGenderWindow.OpenChangeGenderWindow(this.currentGenderType, new ChangeGenderTypeWindow.CallbackFunc(this.endOpenGenderChange));
    }

    private void showLoginCountInfo()
    {
        UserLoginEntity entity = SingletonMonoBehaviour<DataManager>.getInstance().getMasterData(DataNameKind.Kind.USER_LOGIN).getEntityFromId<UserLoginEntity>(NetworkManager.UserId);
        int seqLoginCount = 0;
        int totalLoginCount = 0;
        if (entity != null)
        {
            seqLoginCount = entity.seqLoginCount;
            totalLoginCount = entity.totalLoginCount;
        }
        this.seqLoginLb.text = $"{seqLoginCount:N0}";
        this.totalLoginLb.text = $"{totalLoginCount:N0}";
    }

    public void showMstProfile(MstProfileData data)
    {
        int num;
        long num2;
        this.usrData = data;
        this.usrGameEnt = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
        if (this.usrGameEnt.getCmdSpellInfo(out num, out num2))
        {
            this.cmdCnt = num;
            this.setCmdSpellRecoverTime(num2);
        }
        this.setMstImg();
        this.setUserName();
        this.setUserGender();
        this.setUserBirthDay();
        this.setUserLv();
        this.setUserExp();
        this.setFriendPoint();
        this.setSvtNum();
        this.setCurrentQp();
        this.setCurrentMana();
        this.setCurrentStone();
        this.setCmdSpellImg();
        this.showLoginCountInfo();
    }

    public void showSetBirthDayDlg()
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        this.inputBirthDayWindow.OpenInputBirthDayWindow(new InputUserBirthDayWindow.CallbackFunc(this.endOpenSetBirthDay));
    }

    private void Start()
    {
        base.gameObject.transform.localPosition = new Vector3(1200f, 0f, 0f);
    }

    private void Update()
    {
        if (this.usrGameEnt != null)
        {
            this.setCommandSpellInfo();
        }
    }

    [CompilerGenerated]
    private sealed class <setMstImg>c__AnonStorey88
    {
        internal MstProfileComponent <>f__this;
        internal int genderType;

        internal void <>m__11A()
        {
            this.<>f__this.setCmdSpellInfo(this.genderType);
            this.<>f__this.closeGenderChangeDlg();
        }
    }
}

