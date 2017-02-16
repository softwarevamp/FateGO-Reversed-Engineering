using System;
using System.Text;
using UnityEngine;

public class BattleResultExpComponent : MonoBehaviour
{
    private bool bondsCountUp;
    public Transform eqConfRoot;
    public UISprite eqLevelwindowSprite;
    public UILabel equip_atexpLabel;
    public UILabel equip_confLabel;
    public UILabel equip_getexpLabel;
    public UILabel equip_Lv;
    public Animation equip_LvupAnim;
    public UILabel equip_nameLabel;
    public UILabel equip_newlevelLabel;
    public UISprite equip_nextSprite;
    public UILabel equip_oldlevelLabel;
    public UIProgressBar equip_slider;
    public UILabel equip_Title;
    public GameObject equipLevelupRoot;
    public BattleWindowComponent equipupwindow;
    public GameObject figureRoot;
    public GameObject masterLevelupRoot;
    public UISprite masterLevelwindowSprite;
    public BattleWindowComponent masterupwindow;
    private SePlayer MeterSePlayer;
    public UILabel mst_atexpLabel;
    public UILabel mst_getexpLabel;
    public UILabel mst_Lv;
    public Animation mst_LvupAnim;
    public UISprite mst_nextSprite;
    public UIProgressBar mst_slider;
    public UILabel mst_Title;
    public PlayMakerFSM myFsm;
    private UserEquipEntity newEquip;
    private UserGameEntity newGame;
    private UserEquipEntity oldEquip;
    private UserGameEntity oldGame;
    public BattleResultComponent parentComp;
    public float time_exptotal = 1f;
    private bool updateFlg;
    public BattleResultMasterUpStatusComponent[] upParamList;
    public BattleWindowComponent window;

    public void activeTouch()
    {
        this.parentComp.setTouch(true);
    }

    public void checkEquipLevelUp()
    {
        if (this.oldEquip.lv < this.newEquip.lv)
        {
            this.equipLevelupRoot.SetActive(true);
            this.equip_LvupAnim["bit_result_levelup01"].time = 0f;
            this.equip_LvupAnim.Play("bit_result_levelup01");
            this.equipupwindow.Open(new BattleWindowComponent.EndCall(this.endOpenEquipUp));
            SoundManager.playSystemSe(SeManager.SystemSeKind.STATUS_OPEN);
        }
        else
        {
            this.myFsm.SendEvent("CLOSE");
        }
    }

    public void checkMasterLevelUp()
    {
        iTween component = base.gameObject.GetComponent<iTween>();
        if (component != null)
        {
            iTween.Stop(base.gameObject);
            UnityEngine.Object.DestroyImmediate(component);
        }
        this.UpdateValue(1f);
        if (this.MeterSePlayer != null)
        {
            this.MeterSePlayer.StopSe(0f);
        }
        this.bondsCountUp = false;
        if (this.oldGame.lv < this.newGame.lv)
        {
            this.masterLevelupRoot.SetActive(true);
            this.mst_LvupAnim["bit_result_levelup01"].time = 0f;
            this.mst_LvupAnim.Play("bit_result_levelup01");
            this.masterupwindow.Open(new BattleWindowComponent.EndCall(this.endOpenMasterUp));
            SoundManager.playSystemSe(SeManager.SystemSeKind.STATUS_OPEN);
        }
        else
        {
            this.myFsm.SendEvent("NEXT");
        }
    }

    public void Close()
    {
        this.window.Close(new BattleWindowComponent.EndCall(this.endClose));
    }

    public void closeEquipUp()
    {
        this.parentComp.setTouch(false);
        this.equipupwindow.Close(new BattleWindowComponent.EndCall(this.endCloseEquipUp));
    }

    public void closeMasterUp()
    {
        this.parentComp.setTouch(false);
        this.masterupwindow.Close(new BattleWindowComponent.EndCall(this.endCloseMasterUp));
    }

    public void endClose()
    {
        base.gameObject.SetActive(false);
        this.myFsm.SendEvent("END_PROC");
    }

    public void endCloseEquipUp()
    {
        this.equipLevelupRoot.SetActive(false);
        this.myFsm.SendEvent("END_PROC");
    }

    public void endCloseMasterUp()
    {
        this.masterLevelupRoot.SetActive(false);
        this.myFsm.SendEvent("END_PROC");
    }

    public void endMoveFigure()
    {
        if (this.bondsCountUp)
        {
            object[] args = new object[] { "from", 0f, "to", 1f, "onupdate", "UpdateValue", "oncomplete", "finishUpdateValue", "time", this.time_exptotal };
            iTween.ValueTo(base.gameObject, iTween.Hash(args));
            if (this.updateFlg)
            {
                this.MeterSePlayer = SoundManager.playSe("ba24");
            }
        }
    }

    public void endOpenEquipUp()
    {
        this.parentComp.setTouch(true);
    }

    public void endOpenMasterUp()
    {
        this.myFsm.SendEvent("WAIT_OPEN");
    }

    public void finishUpdateValue()
    {
        if (this.MeterSePlayer != null)
        {
            this.MeterSePlayer.StopSe(0f);
        }
        this.myFsm.SendEvent("NEXT");
    }

    public void Init()
    {
        this.window.setInitData(BattleWindowComponent.ACTIONTYPE.ALPHA, 0.3f, false);
        this.window.setClose();
        this.masterupwindow.setInitData(BattleWindowComponent.ACTIONTYPE.ALPHA, 0.3f, false);
        this.masterupwindow.setClose();
        this.equipupwindow.setInitData(BattleWindowComponent.ACTIONTYPE.ALPHA, 0.3f, false);
        this.equipupwindow.setClose();
        this.masterLevelupRoot.SetActive(false);
        this.equipLevelupRoot.SetActive(false);
        this.masterLevelupRoot.SetActive(false);
        base.gameObject.SetActive(false);
        string str = LocalizationManager.Get("BATTLE_RESULTEXP_MASTERTITLE");
        if (!str.Equals("BATTLE_RESULTEXP_MASTERTITLE"))
        {
            this.mst_Title.text = str;
        }
        str = LocalizationManager.Get("BATTLE_RESULTEXP_EQUIPTITLE");
        if (!str.Equals("BATTLE_RESULTEXP_EQUIPTITLE"))
        {
            this.equip_Title.text = str;
        }
        this.bondsCountUp = true;
    }

    public void Open()
    {
        base.gameObject.SetActive(true);
        MasterFigureManagerOld.CreatePrefab(this.figureRoot, UIMasterFigureRenderOld.DispType.FULL, this.oldGame.genderType, this.oldEquip.equipId, 60, null);
        object[] args = new object[] { "x", -200f, "time", 0.8f, "islocal", true, "oncompletetarget", base.gameObject, "oncomplete", "endMoveFigure" };
        iTween.MoveFrom(this.figureRoot, iTween.Hash(args));
        this.window.Open(null);
        this.myFsm.SendEvent("END_OPEN");
    }

    public bool setEquipExp(int equipId, int getexp, int nowexp, int start_level)
    {
        Debug.Log(string.Concat(new object[] { "setEquipExp( ", equipId, ", ", getexp, ", ", nowexp, ", ", start_level }));
        this.equip_getexpLabel.text = $"+ {getexp}";
        EquipExpMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EquipExpMaster>(DataNameKind.Kind.EQUIP_EXP);
        int num = master.getLevel(equipId, nowexp, start_level);
        int num2 = master.getLevelMax(equipId);
        EquipExpEntity entity = master.getEntityFromId<EquipExpEntity>(equipId, num);
        if (num == num2)
        {
            this.equip_nextSprite.enabled = false;
            this.equip_atexpLabel.text = LocalizationManager.Get("RESULT_EQUIP_MAXEXP");
        }
        else
        {
            this.equip_nextSprite.enabled = true;
            this.equip_atexpLabel.text = $"{entity.exp - nowexp}";
        }
        this.setEquipLv(num);
        int exp = 0;
        long[] args = new long[] { (long) equipId, (long) (num - 1) };
        if (master.isEntityExistsFromId(args))
        {
            EquipExpEntity entity2 = master.getEntityFromId<EquipExpEntity>(equipId, num - 1);
            if (entity2 != null)
            {
                exp = entity2.exp;
            }
        }
        float num4 = this.equip_slider.value;
        if (num == num2)
        {
            this.equip_slider.value = 1f;
        }
        else
        {
            this.equip_slider.value = 1f - (((float) (entity.exp - nowexp)) / ((float) (entity.exp - exp)));
        }
        return (num4 != this.equip_slider.value);
    }

    public void setEquipLv(int level)
    {
        string format = LocalizationManager.Get("BATTLE_RESULTEXP_EQUIPLV");
        if (!format.Equals("BATTLE_RESULTEXP_EQUIPLV"))
        {
            this.equip_Lv.text = string.Format(format, level);
        }
        else
        {
            this.equip_Lv.text = $"{level}";
        }
    }

    public bool setMasterExp(int getexp, int nowexp, int start_level)
    {
        Debug.Log(string.Concat(new object[] { "setMasterExp( ", getexp, ", ", nowexp, ", ", start_level }));
        int id = start_level;
        bool flag = false;
        this.mst_getexpLabel.text = $"+ {getexp}";
        float num2 = this.mst_slider.value;
        if (start_level == ConstantMaster.getValue("MAX_USER_LV"))
        {
            this.mst_nextSprite.enabled = false;
            this.mst_atexpLabel.text = LocalizationManager.Get("RESULT_EQUIP_MAXEXP");
            this.mst_slider.value = 1f;
            flag = false;
        }
        else
        {
            this.mst_nextSprite.enabled = true;
            UserExpMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserExpMaster>(DataNameKind.Kind.USER_EXP);
            id = master.getLevel(nowexp, start_level);
            UserExpEntity entity = master.getEntityFromId<UserExpEntity>(id);
            this.mst_atexpLabel.text = $"{entity.exp - nowexp}";
            int exp = 0;
            long[] args = new long[] { (long) (id - 1) };
            if (master.isEntityExistsFromId(args))
            {
                UserExpEntity entity2 = master.getEntityFromId<UserExpEntity>((int) (id - 1));
                if (entity2 != null)
                {
                    exp = entity2.exp;
                }
            }
            this.mst_slider.value = 1f - (((float) (entity.exp - nowexp)) / ((float) (entity.exp - exp)));
            flag = num2 != this.mst_slider.value;
        }
        this.setMasterLv(id);
        if (id == ConstantMaster.getValue("MAX_USER_LV"))
        {
            this.mst_nextSprite.enabled = false;
            this.mst_atexpLabel.text = LocalizationManager.Get("RESULT_EQUIP_MAXEXP");
            this.mst_slider.value = 1f;
        }
        return flag;
    }

    public void setMasterLv(int level)
    {
        string format = LocalizationManager.Get("BATTLE_RESULTEXP_MASTERLV");
        if (!format.Equals("BATTLE_RESULTEXP_MASTERLV"))
        {
            this.mst_Lv.text = string.Format(format, level);
        }
        else
        {
            this.mst_Lv.text = $"{level}";
        }
    }

    public void setResultData(UserGameEntity oldGame, UserEquipEntity oldEquip)
    {
        this.oldGame = oldGame;
        this.oldEquip = oldEquip;
        this.updateFlg = false;
        this.newGame = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserGameMaster>(DataNameKind.Kind.USER_GAME).getEntityFromId<UserGameEntity>(this.oldGame.userId);
        this.newEquip = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserEquipMaster>(DataNameKind.Kind.USER_EQUIP).getEntityFromId<UserEquipEntity>(oldEquip.id);
        this.setMasterExp(this.newGame.exp - oldGame.exp, oldGame.exp, oldGame.lv);
        this.equip_nameLabel.text = EquipMaster.getEquipName(oldEquip.equipId);
        this.setEquipExp(oldEquip.equipId, this.newEquip.exp - oldEquip.exp, oldEquip.exp, oldEquip.lv);
        this.setMasterLv(oldGame.getLv());
        this.upParamList[0].setData(oldGame.lv, this.newGame.lv);
        int num = 0;
        float num2 = -186f;
        if (oldGame.actMax < this.newGame.actMax)
        {
            this.upParamList[1].gameObject.SetActive(true);
            this.upParamList[1].setTitle("BATTLE_RESULTEXP_APMAX");
            this.upParamList[1].setData(oldGame.actMax, this.newGame.actMax);
            num++;
        }
        else
        {
            this.upParamList[1].gameObject.SetActive(false);
        }
        if (oldGame.costMax < this.newGame.costMax)
        {
            this.upParamList[2].gameObject.SetActive(true);
            this.upParamList[2].setTitle("BATTLE_RESULTEXP_COSTMAX");
            this.upParamList[2].setData(oldGame.costMax, this.newGame.costMax);
            this.upParamList[2].gameObject.transform.localPosition = new Vector3(0f, num2 - (0x2d * num));
            num++;
        }
        else
        {
            this.upParamList[2].gameObject.SetActive(false);
        }
        if (oldGame.friendKeep < this.newGame.friendKeep)
        {
            this.upParamList[3].gameObject.SetActive(true);
            this.upParamList[3].setTitle("BATTLE_RESULTEXP_FRIENDMAX");
            this.upParamList[3].setData(oldGame.friendKeep, this.newGame.friendKeep);
            this.upParamList[3].gameObject.transform.localPosition = new Vector3(0f, num2 - (0x2d * num));
            num++;
        }
        else
        {
            this.upParamList[3].gameObject.SetActive(false);
        }
        this.upParamList[4].gameObject.SetActive(true);
        this.upParamList[4].setTitle("BATTLE_RESULTEXP_FULLAP");
        this.upParamList[4].gameObject.transform.localPosition = new Vector3(0f, num2 - (0x2d * num));
        num++;
        this.masterLevelwindowSprite.height = 0x86 + (0x2d * num);
        this.updateFlg |= oldGame.exp != this.newGame.exp;
        this.setEquipLv(oldEquip.lv);
        this.equip_oldlevelLabel.text = string.Empty + oldEquip.lv;
        this.equip_newlevelLabel.text = string.Empty + this.newEquip.lv;
        StringBuilder builder = new StringBuilder();
        num = 0;
        int[] numArray = oldEquip.getSkillIdList();
        int[] numArray2 = this.newEquip.getSkillIdList();
        int[] numArray3 = oldEquip.getSkillLvList();
        int[] numArray4 = this.newEquip.getSkillLvList();
        SkillMaster master3 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<SkillMaster>(DataNameKind.Kind.SKILL);
        for (int i = 0; i < numArray.Length; i++)
        {
            SkillEntity entity = null;
            long[] args = new long[] { (long) numArray2[i] };
            if (master3.isEntityExistsFromId(args))
            {
                entity = master3.getEntityFromId<SkillEntity>(numArray2[i]);
                if (entity != null)
                {
                    if (numArray[i] != numArray2[i])
                    {
                        builder.AppendLine(string.Format(LocalizationManager.Get("RESULT_EQEXP_GETSKILL"), entity.getName()));
                        num += 3;
                    }
                    else if (numArray3[i] != numArray4[i])
                    {
                        builder.AppendLine(string.Format(LocalizationManager.Get("RESULT_EQEXP_UPSKILLLV"), entity.getName(), numArray4[i]));
                        num += 3;
                    }
                }
            }
        }
        this.eqConfRoot.localPosition = new Vector3(this.eqConfRoot.localPosition.x, 10f * num);
        this.eqLevelwindowSprite.height = 0x86 + (0x16 * num);
        this.equip_confLabel.text = builder.ToString();
        this.updateFlg |= oldEquip.exp != this.newEquip.exp;
    }

    public void UpdateValue(float val)
    {
        bool flag = false;
        int nowexp = Mathf.FloorToInt(Mathf.Lerp((float) this.oldGame.exp, (float) this.newGame.exp, val));
        flag |= this.setMasterExp(this.newGame.exp - this.oldGame.exp, nowexp, this.oldGame.lv);
        nowexp = Mathf.FloorToInt(Mathf.Lerp((float) this.oldEquip.exp, (float) this.newEquip.exp, val));
        flag |= this.setEquipExp(this.newEquip.equipId, this.newEquip.exp - this.oldEquip.exp, nowexp, this.oldEquip.lv);
    }

    private enum ParamList
    {
        MASTER_LEVEL,
        AP,
        COST,
        FRIENDKEEP,
        FULL_AP
    }
}

