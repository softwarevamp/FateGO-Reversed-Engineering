using System;
using UnityEngine;

public class UserStatusComponet : MonoBehaviour
{
    protected UIMasterFaceTexture masterFace;
    public GameObject masterFaceBase;
    public UILabel rcvTimeTxt;
    public UILabel usrAp;
    public UISlider usrApBar;
    public UISlider usrApBar2;
    private UserGameEntity usrData;
    public UILabel usrExp;
    public UISlider usrExpBar;
    private UserExpEntity usrExpData;
    public UILabel usrLv;

    public void HideUserStatus()
    {
        if (this.masterFace != null)
        {
            UnityEngine.Object.Destroy(this.masterFace.gameObject);
            this.masterFace = null;
        }
        if (base.gameObject.activeSelf)
        {
            base.gameObject.SetActive(false);
        }
    }

    private void SetFaceImage()
    {
        int genderType = this.usrData.genderType;
        int equipId = 0;
        if (this.usrData.userEquipId > 0L)
        {
            equipId = SingletonMonoBehaviour<DataManager>.getInstance().getMasterData(DataNameKind.Kind.USER_EQUIP).getEntityFromId<UserEquipEntity>(this.usrData.userEquipId).equipId;
        }
        if (this.masterFace == null)
        {
            this.masterFace = MasterFaceManager.CreatePrefab(this.masterFaceBase, UIMasterFaceRender.DispType.STATUS, genderType, equipId, 2, null);
        }
        else
        {
            this.masterFace.SetCharacter(UIMasterFaceRender.DispType.STATUS, genderType, equipId);
        }
    }

    private void SetUserInfo()
    {
        this.SetUserLv();
        this.SetUsrExp();
        this.SetUsrAp();
        this.SetFaceImage();
    }

    private void SetUserLv()
    {
        string str = this.usrData.lv.ToString();
        this.usrLv.text = str;
    }

    public void SetUserStatusData()
    {
        base.gameObject.SetActive(true);
        this.usrData = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
        int lv = this.usrData.lv;
        this.usrExpData = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserExpMaster>(DataNameKind.Kind.USER_EXP).getEntityFromLevel(lv + 1);
        this.SetUserInfo();
    }

    private void SetUsrAp()
    {
        int actMax = this.usrData.actMax;
        int num2 = this.usrData.getAct();
        float num3 = ((float) num2) / ((float) actMax);
        this.usrApBar.value = (num3 <= 1f) ? num3 : 1f;
        this.usrApBar2.value = (num3 <= 1f) ? 0f : (num3 - 1f);
        string str = string.Format(LocalizationManager.Get("USER_ACTION_POINT"), num2, actMax);
        this.usrAp.text = str;
        long num4 = this.usrData.getActNextRecoverTime();
        if (num4 > 0L)
        {
            int num5 = (int) (num4 / 60L);
            int num6 = (int) (num4 % 60L);
            this.rcvTimeTxt.text = string.Format(LocalizationManager.Get("USER_ACTION_POINT_RECOVER"), num5, num6);
        }
        else
        {
            this.rcvTimeTxt.text = string.Empty;
        }
    }

    private void SetUsrExp()
    {
        int exp = this.usrData.exp;
        int num2 = this.usrExpData.exp;
        this.usrExp.text = num2.ToString();
        float num3 = ((float) exp) / ((float) num2);
        this.usrExpBar.value = num3;
    }

    private void Update()
    {
        if (this.usrData != null)
        {
            this.SetUsrAp();
        }
    }
}

