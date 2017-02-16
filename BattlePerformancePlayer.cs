using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class BattlePerformancePlayer : MonoBehaviour
{
    private BattleLogic.UseSkillObject actSkillObject;
    private Vector3 conf_pos;
    public BattleServantConfConponent confwindowComp;
    private BattleData data;
    private int[] list_ID = new int[3];
    public BattleServantParamComponent[] list_param;
    private Vector3[] list_pos = new Vector3[3];
    private BattleLogic logic;
    public PlayMakerFSM myfsm;
    public PlayMakerFSM otherFsm;
    private BattlePerformance perf;
    private BattleSelectServantWindow selectSvtWindow;
    public BattleSkillConfComponent skillConfWindow;
    private int tmp_uniqueId;
    private BattleLogic.UseSkillObject tmp_useSkill;
    private BattleLogic.UseSkillObject useSkillObject;

    public void checkSkipFlg()
    {
        if (this.data.getServantData(this.tmp_useSkill.skillInfo.svtUniqueId).isUseSkill() && this.tmp_useSkill.skillInfo.isChargeOK())
        {
            if (this.data.systemflg_skipskillconf)
            {
                this.useSkillObject = this.tmp_useSkill;
                this.myfsm.SendEvent("SKIP");
            }
            else
            {
                this.myfsm.SendEvent("OK");
            }
        }
        else
        {
            this.myfsm.SendEvent("END_PROC");
        }
    }

    public void checkTutorial()
    {
        if (this.data.tutorialId == -1)
        {
            this.myfsm.SendEvent("END_PROC");
        }
        else if ((this.data.tutorialId != 1) && ((this.data.tutorialId != 2) || (this.data.turnCount != 1)))
        {
            if (((this.data.tutorialId == 2) && (this.data.turnCount == 2)) && (this.data.tutorialState == -1))
            {
                this.myfsm.SendEvent("TUTORIAL_SKILL");
            }
            else if ((this.data.tutorialId != 2) || (this.data.turnCount != 2))
            {
                this.myfsm.SendEvent("END_PROC");
            }
        }
    }

    public void CloseSkillConfComp()
    {
        Debug.Log("::CloseSkillConfComp");
        this.myfsm.SendEvent("END_PROC");
    }

    public void closeSvtConfWindow(float alphatime, BattleWindowComponent.EndCall endCall = null)
    {
        this.confwindowComp.Close(endCall);
    }

    public void deleteStatus(int index)
    {
        if (this.list_param[index] != null)
        {
            BattleServantData data = this.list_param[index].getData();
            if (data != null)
            {
                data.delParamObject(this.list_param[index].gameObject);
            }
            this.list_param[index].setData(null);
            this.list_ID[index] = -1;
        }
    }

    public void endSkill()
    {
        this.myfsm.SendEvent("END_SKILL");
    }

    public void Initialize(BattlePerformance inperf, BattleData indata, BattleLogic inlogic)
    {
        this.perf = inperf;
        this.data = indata;
        this.logic = inlogic;
        for (int i = 0; i < this.list_param.Length; i++)
        {
            this.list_pos[i] = this.list_param[i].gameObject.transform.position;
            this.list_param[i].setCloseMode();
        }
        this.initSvtConfWindow();
        this.skillConfWindow.setInit(this.data);
        this.skillConfWindow.setInitialPos();
        this.skillConfWindow.setClose();
    }

    public void initSvtConfWindow()
    {
        this.confwindowComp.Initialize();
        this.confwindowComp.setInitialPos();
        this.confwindowComp.setClose();
    }

    public bool isOpenSvtConf() => 
        this.confwindowComp.isOpen();

    public void modeComPlayerStatus()
    {
        for (int i = 0; i < this.list_param.Length; i++)
        {
            this.list_param[i].GetComponent<UIWidget>().color = Color.white;
            this.list_param[i].transform.position = this.list_pos[i];
            this.list_param[i].transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        }
        this.closeSvtConfWindow(0.1f, null);
        this.skillConfWindow.Close(null);
        for (int j = 0; j < this.list_param.Length; j++)
        {
            if (this.list_param[j] != null)
            {
                this.list_param[j].setCloseMode();
            }
        }
    }

    public void modeTacPlayerStatus()
    {
        this.updateView();
        for (int i = 0; i < this.list_param.Length; i++)
        {
            if (this.list_param[i] != null)
            {
                this.list_param[i].setOpenMode();
            }
        }
    }

    public void onClickConfClose()
    {
        this.myfsm.SendEvent("CLICK_CLOSE");
    }

    public void onClickServant(int uniqueID)
    {
        if (this.confwindowComp.isTargetSvt(uniqueID))
        {
            this.myfsm.SendEvent("CLICK_CLOSE");
        }
        else
        {
            this.tmp_uniqueId = uniqueID;
            this.myfsm.SendEvent("CLICK_SVTWINDOW");
        }
    }

    public void onClickSkillCancel()
    {
        this.myfsm.SendEvent("CANCEL");
    }

    public void onClickSkillIcon(BattleSkillInfoData skillInfo)
    {
        BattleLogic.UseSkillObject obj2 = new BattleLogic.UseSkillObject(skillInfo);
        if (((((!this.data.isTutorial() || (this.data.tutorialId != 2)) || (this.data.turnCount != 2)) || ((skillInfo.type == BattleSkillInfoData.TYPE.SERVANT_SELF) && (skillInfo.index == 1))) && this.perf.statusPerf.masterPerf.isCloseEnemyConf()) && this.logic.isTimingUseSkill())
        {
            this.tmp_useSkill = obj2;
            this.myfsm.SendEvent("CLICK_SKILLICON");
        }
    }

    public void onClickSkillOK(BattleSkillInfoData skillInfo)
    {
        this.useSkillObject = new BattleLogic.UseSkillObject(skillInfo);
        this.myfsm.SendEvent("OK");
    }

    public void onCloseConfComplete()
    {
        this.confwindowComp.gameObject.SetActive(false);
        this.confwindowComp.setConfData(null);
        for (int i = 0; i < this.list_param.Length; i++)
        {
            this.list_param[i].setTouch(true);
        }
        this.perf.changeAttackButton(true, true, true);
        this.myfsm.SendEvent("END_PROC");
    }

    public void onOpenConfComplete()
    {
        Debug.Log("::onOpenConfComplete");
        this.myfsm.SendEvent("END_PROC");
    }

    public void openSelectSvtWindow()
    {
        this.perf.getSelectMainSubSvtWindow().setClose();
        this.selectSvtWindow = this.perf.getSelectSvtWindow();
        if (this.data.isTutorialSelectsvtCancel())
        {
            this.procTurorial(3);
            this.selectSvtWindow.setUseClose(false);
        }
        this.selectSvtWindow.SetCallBack(new BattleSelectServantWindow.SelectServantCallBack(this.selectedSvt));
        this.selectSvtWindow.SetServantData(this.data.getFieldPlayerServantList());
        this.selectSvtWindow.Open(null);
    }

    public void OpenSkillConfComplete()
    {
        Debug.Log("::OpenSkillConfComplete");
    }

    public void openSvtConfWindow(float alphatime)
    {
        this.confwindowComp.Open(new BattleWindowComponent.EndCall(this.onOpenConfComplete));
    }

    public void playAttackEffect(int uniqueID)
    {
        for (int i = 0; i < this.list_ID.Length; i++)
        {
            if (this.list_ID[i] == uniqueID)
            {
                this.list_param[i].playAttackEffect();
            }
        }
    }

    public void procCloseAll()
    {
        this.procCloseConf(false);
        this.perf.statusPerf.CloseBuffConf();
        this.myfsm.SendEvent("END_PROC");
    }

    public void procCloseConf(bool flg)
    {
        float alphatime = 0.3f;
        if (flg)
        {
            SoundManager.playSe("ba21");
        }
        for (int i = 0; i < this.list_param.Length; i++)
        {
            this.list_param[i].playEndShowServant();
        }
        this.perf.statusPerf.CloseBuffConf();
        this.perf.changeAttackButton(true, false, true);
        this.closeSvtConfWindow(alphatime, new BattleWindowComponent.EndCall(this.onCloseConfComplete));
    }

    public void procCloseSkillConf()
    {
        SoundManager.playSe("ba19");
        this.skillConfWindow.Close(new BattleWindowComponent.EndCall(this.CloseSkillConfComp));
    }

    public void procOpenSkillConf(bool cancelFlg = true)
    {
        BattleLogic.UseSkillObject obj2 = this.tmp_useSkill;
        this.otherFsm.SendEvent("CLOSE_CONF");
        this.skillConfWindow.target = base.gameObject;
        this.skillConfWindow.SetSkillConf(obj2.skillInfo, cancelFlg);
        this.skillConfWindow.Open(new BattleWindowComponent.EndCall(this.OpenSkillConfComplete));
    }

    public void procSelectServant()
    {
        if ((!this.data.isTutorial() || (this.data.tutorialId != 3)) || ((this.data.wavecount != 1) || (this.data.turnCount != 1)))
        {
            int num = this.tmp_uniqueId;
            this.otherFsm.SendEvent("START_CLOSE");
            SoundManager.playSe("ba20");
            float alphatime = 0.3f;
            for (int i = 0; i < this.list_param.Length; i++)
            {
                if (num == this.list_ID[i])
                {
                    this.list_param[i].playSelectServant();
                    this.confwindowComp.setConfData(this.list_param[i].getData());
                }
                else
                {
                    this.list_param[i].playCloseSelectServant();
                }
            }
            this.perf.statusPerf.CloseBuffConf();
            this.openSvtConfWindow(alphatime);
            this.perf.changeAttackButton(false, false, false);
        }
    }

    public void procTurorial(int param)
    {
        if (param == 0)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.CloseTutorialNotificationDialogArrow(null);
        }
        else if (param == 2)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.CloseTutorialArrowMark(new System.Action(this.tutorialSetSelect));
        }
        else if (param == 3)
        {
            this.data.tutorialState = 5;
            SingletonMonoBehaviour<CommonUI>.Instance.CloseTutorialArrowMark(new System.Action(this.tutorialSetSelectSvt));
        }
    }

    public void selectedSvt(int uniqueId)
    {
        this.selectSvtWindow.Close(null);
        if (uniqueId == -1)
        {
            if ((this.data.tutorialId != 2) || (this.data.tutorialState != -1))
            {
                this.myfsm.SendEvent("CANCEL");
            }
        }
        else
        {
            BattleSkillInfoData skillInfo = this.actSkillObject.skillInfo;
            this.logic.wantUseSkill(skillInfo, uniqueId, -1);
            this.myfsm.SendEvent("END_PROC");
        }
    }

    public void setParam(int index, BattleServantData svtdata)
    {
        if (this.list_param[index] != null)
        {
            this.list_param[index].setData(svtdata);
            svtdata.addParamObject(this.list_param[index].gameObject);
            this.list_param[index].index = index;
            this.list_ID[index] = svtdata.getUniqueID();
        }
    }

    public void startCommand()
    {
        this.myfsm.SendEvent("START_COM");
    }

    public void startSkill()
    {
        Debug.Log("::startSkill");
        this.myfsm.SendEvent("START_SKILL");
    }

    public void startTac()
    {
        this.myfsm.SendEvent("START_TAC");
    }

    public void tutorialSetArrowIcon()
    {
        Vector2 pos = new Vector2(-378f, -151f);
        Rect rect = new Rect(-230f, -270f, 90f, 90f);
        SingletonMonoBehaviour<CommonUI>.Instance.OpenTutorialArrowMark(pos, (float) 0f, rect, null);
    }

    public void tutorialSetSelect()
    {
        Vector2 pos = new Vector2(310f, -50f);
        Rect rect = new Rect(-410f, -104f, 820f, 280f);
        SingletonMonoBehaviour<CommonUI>.Instance.OpenTutorialArrowMark(pos, (float) -90f, rect, null);
    }

    public void tutorialSetSelectSvt()
    {
        Vector2 pos = new Vector2(0f, 0f);
        Rect rect = new Rect(-200f, -175f, 400f, 350f);
        SingletonMonoBehaviour<CommonUI>.Instance.OpenTutorialArrowMark(pos, (float) 0f, rect, null);
    }

    public void updateBuff()
    {
    }

    public void updateView()
    {
        for (int i = 0; i < this.list_param.Length; i++)
        {
            if (this.list_param[i] != null)
            {
                this.list_param[i].updateView();
            }
        }
    }

    public void UseSkill(bool playSe)
    {
        if (playSe)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE2);
        }
        this.actSkillObject = this.useSkillObject;
        BattleSkillInfoData skillInfo = this.actSkillObject.skillInfo;
        SkillLvEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<SkillLvMaster>(DataNameKind.Kind.SKILL_LEVEL).getEntityFromId<SkillLvEntity>(skillInfo.skillId, skillInfo.skilllv);
        if (this.logic.checkSelectFunctionTarget(entity.funcId))
        {
            this.openSelectSvtWindow();
        }
        else
        {
            this.logic.wantUseSkill(skillInfo, skillInfo.svtUniqueId, -1);
            this.myfsm.SendEvent("END_PROC");
        }
    }

    public void useSkillIcon(BattleSkillInfoData skillInfo)
    {
        this.useSkillObject = new BattleLogic.UseSkillObject(skillInfo);
        this.myfsm.SendEvent("CLICK_SKILLICON");
    }
}

