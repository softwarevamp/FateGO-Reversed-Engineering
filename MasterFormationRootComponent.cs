using System;
using UnityEngine;

public class MasterFormationRootComponent : SceneRootComponent
{
    [SerializeField]
    protected MasterFormationComponent mstFormationComp;
    [SerializeField]
    protected TitleInfoControl titleInfo;
    protected UserGameEntity usrGameEnt;

    public override void beginFinish()
    {
        Debug.Log("Call beginFinish");
    }

    public override void beginInitialize()
    {
        base.beginInitialize();
        SingletonMonoBehaviour<SceneManager>.Instance.endInitialize(this);
    }

    public override void beginResume()
    {
        Debug.Log("Call beginResume");
        base.beginResume();
    }

    public override void beginStartUp()
    {
        SoundManager.playBgm("BGM_CHALDEA_1");
        this.titleInfo.setTitleInfo(base.myFSM, true, null, TitleInfoControl.TitleKind.FORM_MASTER);
        this.titleInfo.setBackBtnSprite(true);
        this.titleInfo.setBackBtnDepth(0x1d);
        base.sendMessageStartUp();
        Debug.Log("Call beginStartUp");
    }

    private void changeUserEquipCallback(string res)
    {
        base.myFSM.SendEvent("REQUEST_OK");
    }

    public void closeMasterFormation()
    {
        SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, delegate {
            this.mstFormationComp.closeMasterFormation();
            base.myFSM.SendEvent("GO_NEXT");
        });
    }

    public void gotoBack()
    {
        if (SingletonMonoBehaviour<SceneManager>.Instance.IsStackScene())
        {
            SingletonMonoBehaviour<SceneManager>.Instance.popScene(SceneManager.FadeType.BLACK, null);
        }
        else
        {
            SingletonMonoBehaviour<SceneManager>.Instance.transitionScene(SceneList.Type.Formation, SceneManager.FadeType.BLACK, null);
        }
    }

    public void Init()
    {
        this.usrGameEnt = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
        base.myFSM.SendEvent("GO_NEXT");
    }

    public void OnClickBack()
    {
        this.titleInfo.sendEvent("CLICK_BACK");
    }

    public void requestChangeUsrEquip()
    {
        long userEquipId = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME).userEquipId;
        long num2 = this.mstFormationComp.getCurrentUsrEquipId();
        bool flag = this.mstFormationComp.isChangeEquip();
        if ((userEquipId != num2) && flag)
        {
            NetworkManager.getRequest<UserFormationRequest>(new NetworkManager.ResultCallbackFunc(this.changeUserEquipCallback)).beginRequest(num2);
        }
        else
        {
            base.myFSM.SendEvent("NO_CHANGE_EQUIP");
        }
    }

    public void showMasterFormation()
    {
        this.mstFormationComp.setCmdSpellImg();
        this.mstFormationComp.setMasterFormation(this.usrGameEnt);
        this.titleInfo.setBackBtnColliderEnable(true);
        SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, null);
    }
}

