using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class BattlePerformanceMaster : MonoBehaviour
{
    private BattleLogic.UseSkillObject actSkillObject;
    private const float alphatime = 0.3f;
    public CommandSpellIconComponent commandSpellIcon;
    public BattleData data;
    public GameObject face_root;
    public BattleLogic logic;
    public GameObject master_root;
    public GameObject masterBtn;
    public GameObject menuBtn;
    public PlayMakerFSM myFsm;
    public PlayMakerFSM otherFsm;
    public BattlePerformance perf;
    private Vector3 pos_menubtn;
    private Vector3 pos_skillbtn;
    private BattleSelectMainSubServantWindow selectMSWindow;
    private BattleSelectServantWindow selectSvtWindow;
    public GameObject skillBtn;
    public BattleSkillConfComponent skillConfWindow;
    public BattleServantSkillIConComponent[] skillIcon;
    private BattleSelectServantWindow skillselectSvtWindow;
    public UISprite skillSkipBtn;
    public GameObject spellBtn;
    public UILabel stock_label;
    public GameObject stock_root;
    public UISprite tdConstBtn;
    private int tmp_commandSpellId;
    private int tmp_index = -1;
    private BattleLogic.UseSkillObject tmp_useSkill;
    private BattleLogic.UseSkillObject useSkillObject;
    public BattleServantConfConponent win_EnemyConf;
    public BattleMenuWindowComponent win_Menu;
    public BattleMasterSkillWindowComponent win_Skill;
    public CommandSpellWindowComponent win_Spell;

    public void callBackSelectedMainSub(bool flg, int mainUniqueId, int subUniqueId)
    {
        this.selectMSWindow.Close(null);
        if (!flg)
        {
            this.myFsm.SendEvent("CANCEL");
        }
        else
        {
            this.myFsm.SendEvent("END_PROC");
            BattleSkillInfoData skillInfo = this.actSkillObject.skillInfo;
            this.logic.wantUseSkill(skillInfo, mainUniqueId, subUniqueId);
        }
    }

    public void changeShortSkill()
    {
        this.data.toggleSkipSkillConf();
        if (this.data.systemflg_skipskillconf)
        {
            SoundManager.playSe("ba18");
        }
        else
        {
            SoundManager.playSe("ba18");
        }
        this.updateShortSkill();
    }

    public void changeTdConstantVelocity()
    {
        this.data.toggleTdConstantVelocity();
        if (this.data.systemflg_TdConstantvelocity)
        {
            SoundManager.playSe("ba18");
        }
        else
        {
            SoundManager.playSe("ba18");
        }
        this.updateTdConstantVelocity();
    }

    public void checkCommandSpellTarget()
    {
        Debug.Log("checkCommandSpellTarget");
        CommandSpellEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<CommandSpellMaster>(DataNameKind.Kind.COMMAND_SPELL).getEntityFromId<CommandSpellEntity>(this.tmp_commandSpellId);
        Debug.Log("checkSelectFunctionTarget");
        if (this.logic.checkSelectFunctionTarget(entity.funcId))
        {
            Debug.Log("true");
            this.openSelectSvtWindow();
        }
        else
        {
            Debug.Log("false");
            this.myFsm.SendEvent("END_PROC");
            this.logic.useCommandSpell(this.tmp_commandSpellId, -1);
        }
    }

    public void checkSkipFlg()
    {
        if (!this.tmp_useSkill.skillInfo.isChargeOK())
        {
            this.myFsm.SendEvent("NG");
        }
        else if (this.data.systemflg_skipskillconf)
        {
            this.useSkillObject = this.tmp_useSkill;
            this.myFsm.SendEvent("SKIP");
        }
        else
        {
            this.myFsm.SendEvent("END_PROC");
        }
    }

    public void checkTutorial()
    {
        if (this.logic.isTutorialMasterStatus())
        {
            this.myFsm.SendEvent("OK");
        }
        else
        {
            this.myFsm.SendEvent("SKIP");
        }
    }

    public void clickSkillIcon(BattleSkillInfoData skillInfo)
    {
        this.tmp_useSkill = new BattleLogic.UseSkillObject(skillInfo);
        if (this.logic.isTimingUseSkill())
        {
            this.myFsm.SendEvent("CLICK_SKILLICON");
        }
    }

    public void CloseSkillConfComp()
    {
        this.myFsm.SendEvent("END_PROC");
    }

    public void compCloseALL()
    {
        this.stock_root.SetActive(true);
        this.myFsm.SendEvent("END_CLOSEALL");
    }

    public void compCloseEnemyConf()
    {
        this.myFsm.SendEvent("END_PROC");
    }

    public void compCloseMenu()
    {
        this.proclight(this.skillBtn, false);
        this.myFsm.SendEvent("END_PROC");
    }

    public void compCloseSkill()
    {
        this.stock_root.SetActive(true);
        this.myFsm.SendEvent("END_PROC");
    }

    public void compCloseSkillWindow()
    {
        this.proclight(this.skillBtn, false);
        this.myFsm.SendEvent("END_PROC");
    }

    public void compCloseSpellWindow()
    {
        this.proclight(this.skillBtn, false);
        this.myFsm.SendEvent("END_PROC");
    }

    public void compOpenEnemyConf()
    {
        this.myFsm.SendEvent("END_PROC");
    }

    public void compOpenMasterMenu()
    {
        this.proclight(this.skillBtn, true);
        this.proclight(this.menuBtn, false);
        this.myFsm.SendEvent("END_PROC");
    }

    public void compOpenMenu()
    {
        this.proclight(this.menuBtn, true);
        this.myFsm.SendEvent("END_PROC");
    }

    public void compOpenSkillWindow()
    {
        this.proclight(this.skillBtn, true);
        this.myFsm.SendEvent("END_PROC");
    }

    public void compOpenSpellWindow()
    {
        this.myFsm.SendEvent("END_PROC");
    }

    public void endSelectSvtError(bool flg)
    {
        SingletonMonoBehaviour<CommonUI>.Instance.CloseNotificationDialog();
        this.myFsm.SendEvent("CANCEL");
    }

    public void endSkill()
    {
        this.updateCommandSpellIcon();
        this.myFsm.SendEvent("START_TAC");
    }

    public void Initialize(BattlePerformance inperf, BattleData indata, BattleLogic inlogic)
    {
        this.perf = inperf;
        this.data = indata;
        this.logic = inlogic;
        this.pos_skillbtn = this.skillBtn.transform.localPosition;
        this.pos_menubtn = this.menuBtn.transform.localPosition;
        this.win_Skill.setInitData(BattleWindowComponent.ACTIONTYPE.SLIDE, 0.3f, false);
        this.win_Skill.setClose();
        this.win_Spell.setInitData(BattleWindowComponent.ACTIONTYPE.ALPHA, 0.3f, false);
        this.win_Spell.InitializeCommandSpell(CommandSpellWindowComponent.MODE.BATTLE);
        this.win_Spell.setCallBackPushClose(new CommandSpellWindowComponent.CloseButtonCallBack(this.procCloseButtonCommandSpellWindow));
        this.win_Spell.setCallBackUse(new CommandSpellWindowComponent.UseCommandSpellCallBack(this.procUseCommandSpell));
        this.win_Spell.setClose();
        this.win_Menu.setInitData(BattleWindowComponent.ACTIONTYPE.ALPHA, 0.3f, false);
        this.win_Menu.data = this.data;
        this.win_EnemyConf.setInitData(BattleWindowComponent.ACTIONTYPE.ALPHA, 0.3f, false);
        this.win_EnemyConf.setClose();
        this.updateShortSkill();
        this.updateTdConstantVelocity();
    }

    public bool isCloseEnemyConf() => 
        this.win_EnemyConf.isClose();

    public void loadData()
    {
        UserGameEntity entity = UserGameMaster.getSelfUserGame();
        int genderType = (entity == null) ? 0 : entity.genderType;
        int equipId = this.data.getMasterEquipId();
        MasterFaceManager.CreatePrefab(this.face_root, UIMasterFaceRender.DispType.STATUS, genderType, equipId, 2, null);
        this.updateSkillIcon();
        this.updateCommandSpellIcon();
    }

    public void modeCom()
    {
        this.master_root.SetActive(false);
        this.procCloseSkill();
        this.win_Spell.Close(null);
        this.win_Menu.Close(null);
        this.win_EnemyConf.Close(null);
    }

    public void modeTac()
    {
        this.master_root.SetActive(true);
        this.updateSkillIcon();
        this.myFsm.SendEvent("END_PROC");
    }

    public void onClickSkillCancel()
    {
        this.myFsm.SendEvent("CANCEL");
    }

    public void onClickSkillOK(BattleSkillInfoData skillInfo)
    {
        this.useSkillObject = new BattleLogic.UseSkillObject(skillInfo);
        this.myFsm.SendEvent("OK");
    }

    public void onCloseEnemyServantConf()
    {
        this.myFsm.SendEvent("CLOSE_CONF");
    }

    [DebuggerHidden]
    private IEnumerator openRetireDialog() => 
        new <openRetireDialog>c__Iterator2F { <>f__this = this };

    public void openSelectSvtWindow()
    {
        Debug.Log("openSelectSvtWindow");
        this.selectSvtWindow = this.perf.getSelectSvtWindow();
        this.selectSvtWindow.SetCallBack(new BattleSelectServantWindow.SelectServantCallBack(this.selectedSvt));
        this.selectSvtWindow.SetServantData(this.data.getFieldPlayerServantList());
        this.selectSvtWindow.Open(null);
    }

    public void OpenSkillConfComplete()
    {
        Debug.Log("::OpenSkillConfComplete");
        this.myFsm.SendEvent("END_PROC");
    }

    public void openSkillSelectMainSubSvtWindow()
    {
        Debug.Log("openSkillSelectMainSubSvtWindow");
        BattleServantData[] dataArray = this.data.getSubPlayerServantList();
        if (0 < dataArray.Length)
        {
            this.selectMSWindow = this.perf.getSelectMainSubSvtWindow();
            this.selectMSWindow.SetCallBack(new BattleSelectMainSubServantWindow.SelectedCallBack(this.callBackSelectedMainSub));
            this.selectMSWindow.SetServantData(this.data.getFieldPlayerServantList(), this.data.getSubPlayerServantList());
            this.selectMSWindow.Open(null);
        }
        else
        {
            SingletonMonoBehaviour<CommonUI>.Instance.OpenNotificationDialog(LocalizationManager.Get("BATTLE_SELECTSUBERROR_NOSUB_TITLE"), LocalizationManager.Get("BATTLE_SELECTSUBERROR_NOSUB_CONF"), new NotificationDialog.ClickDelegate(this.endSelectSvtError), -1);
        }
    }

    public void openSkillSelectSvtWindow()
    {
        Debug.Log("openSkillSelectSvtWindow");
        this.skillselectSvtWindow = this.perf.getSelectSvtWindow();
        this.skillselectSvtWindow.SetCallBack(new BattleSelectServantWindow.SelectServantCallBack(this.selectedSkillSvt));
        this.skillselectSvtWindow.SetServantData(this.data.getFieldPlayerServantList());
        this.skillselectSvtWindow.Open(null);
    }

    public void procCloseAll()
    {
        this.proclight(this.skillBtn, false);
        this.proclight(this.menuBtn, false);
        this.win_Menu.Close(null);
        this.win_Skill.Close(new BattleWindowComponent.EndCall(this.compCloseALL));
        this.win_Spell.Close(null);
        this.win_EnemyConf.Close(null);
        this.skillConfWindow.Close(null);
    }

    public void procCloseButtonCommandSpellWindow()
    {
        this.myFsm.SendEvent("CLOSE_SPELL");
    }

    public void procCloseEnemyConf()
    {
        this.win_EnemyConf.Close(new BattleWindowComponent.EndCall(this.compCloseEnemyConf));
    }

    public void procCloseMenuWindow()
    {
        SoundManager.playSe("ba19");
        this.proclight(this.menuBtn, false);
        this.win_Menu.Close(new BattleWindowComponent.EndCall(this.compCloseMenu));
    }

    public void procCloseSkill()
    {
        this.proclight(this.skillBtn, false);
        this.proclight(this.menuBtn, false);
        this.win_Skill.Close(null);
    }

    public void procCloseSkillConf()
    {
        SoundManager.playSe("ba19");
        this.skillConfWindow.Close(new BattleWindowComponent.EndCall(this.CloseSkillConfComp));
    }

    public void procCloseSkillWindow()
    {
        SoundManager.playSe("ba19");
        this.proclight(this.skillBtn, false);
        this.win_Skill.Close(new BattleWindowComponent.EndCall(this.compCloseSkillWindow));
    }

    public void procCloseSpellWindow()
    {
        SoundManager.playSe("ba19");
        this.win_Spell.Close(new BattleWindowComponent.EndCall(this.compCloseSpellWindow));
    }

    public void proclight(GameObject obj, bool flg)
    {
        UISprite component = obj.GetComponent<UISprite>();
        if (component != null)
        {
            component.enabled = flg;
        }
    }

    public void procOpenEnemyConf()
    {
        this.otherFsm.SendEvent("START_CLOSE");
        BattleServantData inbsvtData = this.data.getEnemyServantDataIndex(this.tmp_index);
        this.win_EnemyConf.setConfData(inbsvtData);
        this.win_EnemyConf.Open(new BattleWindowComponent.EndCall(this.compOpenEnemyConf));
    }

    public void procOpenMasterMenu()
    {
        this.otherFsm.SendEvent("START_CLOSE");
        SoundManager.playSe("sy8");
        this.win_Skill.Open(null);
        this.stock_root.SetActive(false);
    }

    public void procOpenMenuWindow()
    {
        this.otherFsm.SendEvent("START_CLOSE");
        SoundManager.playSe("ba18");
        this.proclight(this.skillBtn, false);
        this.proclight(this.menuBtn, true);
        this.win_Skill.Close(null);
        this.win_Menu.Open(new BattleWindowComponent.EndCall(this.compOpenMenu));
    }

    public void procOpenSkillConf()
    {
        BattleLogic.UseSkillObject obj2 = this.tmp_useSkill;
        this.skillConfWindow.target = base.gameObject;
        this.skillConfWindow.SetSkillConf(obj2.skillInfo, true);
        this.skillConfWindow.Open(new BattleWindowComponent.EndCall(this.OpenSkillConfComplete));
    }

    public void procOpenSkillWindow()
    {
        this.otherFsm.SendEvent("START_CLOSE");
        SoundManager.playSe("ba18");
        this.proclight(this.skillBtn, true);
        this.proclight(this.menuBtn, false);
        this.win_Skill.Open(new BattleWindowComponent.EndCall(this.compOpenSkillWindow));
    }

    public void procOpenSpellWindow()
    {
        SoundManager.playSe("ba18");
        this.otherFsm.SendEvent("START_CLOSE");
        this.proclight(this.skillBtn, false);
        this.proclight(this.menuBtn, false);
        this.win_Spell.Open(new BattleWindowComponent.EndCall(this.compOpenSpellWindow));
        this.win_Skill.Close(null);
    }

    public void procUseCommandSpell(int commandSpellId)
    {
        Debug.Log("procUseCommandSpell:" + commandSpellId);
        this.tmp_commandSpellId = commandSpellId;
        this.myFsm.SendEvent("CLICK_USESPELL");
    }

    public void retRetireDialog(bool flg)
    {
        SingletonMonoBehaviour<CommonUI>.Instance.CloseNotificationDialog();
        this.logic.sendFsmEvent("OK");
    }

    public void selectedSkillSvt(int uniqueId)
    {
        this.skillselectSvtWindow.Close(null);
        if (uniqueId == -1)
        {
            this.myFsm.SendEvent("CANCEL");
        }
        else
        {
            this.myFsm.SendEvent("END_PROC");
            BattleSkillInfoData skillInfo = this.actSkillObject.skillInfo;
            this.logic.wantUseSkill(skillInfo, uniqueId, 4);
        }
    }

    public void selectedSvt(int uniqueId)
    {
        this.selectSvtWindow.Close(null);
        if (uniqueId == -1)
        {
            this.myFsm.SendEvent("CANCEL");
        }
        else
        {
            this.myFsm.SendEvent("END_PROC");
            this.logic.useCommandSpell(this.tmp_commandSpellId, uniqueId);
        }
    }

    public void showEnemyServant(int index)
    {
        this.tmp_index = index;
        this.myFsm.SendEvent("LONGPRESS_ENEMY");
    }

    public void showRetireDialog()
    {
        this.logic.playRetire();
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE2);
        this.win_Menu.Close(null);
        this.perf.effectFadeOut();
        SoundManager.playSe("ba7");
        base.StartCoroutine(this.openRetireDialog());
    }

    public void startCommand()
    {
        this.myFsm.SendEvent("START_COM");
    }

    public void startSkill(int type = 0)
    {
        this.win_Spell.Close(null);
        this.myFsm.SendEvent("START_SKILL");
    }

    public void startTac()
    {
        this.myFsm.SendEvent("START_TAC");
    }

    public void updateCommandSpellIcon()
    {
        UserGameEntity entity = UserGameMaster.getSelfUserGame();
        this.commandSpellIcon.SetData(entity);
    }

    public void updateDropItemCount()
    {
        this.stock_label.text = string.Empty + this.data.droplist.Count;
    }

    public void updateShortSkill()
    {
        if (this.data.systemflg_skipskillconf)
        {
            this.skillSkipBtn.spriteName = "btn_off";
        }
        else
        {
            this.skillSkipBtn.spriteName = "btn_on";
        }
    }

    public void updateSkillIcon()
    {
        for (int i = 0; i < this.skillIcon.Length; i++)
        {
            BattleSkillInfoData skillInfo = this.data.getMasterSkillInfo(i);
            if (skillInfo != null)
            {
                if (!skillInfo.isUseSkill)
                {
                    this.skillIcon[i].setNoSkill(0);
                }
                else
                {
                    this.skillIcon[i].SetSkillInfo(skillInfo, true);
                }
            }
            this.skillIcon[i].setflashFlg(true);
        }
    }

    public void updateTdConstantVelocity()
    {
        if (this.data.systemflg_TdConstantvelocity)
        {
            this.tdConstBtn.spriteName = "btn_on";
        }
        else
        {
            this.tdConstBtn.spriteName = "btn_off";
        }
    }

    public void UseSkill(bool playSe)
    {
        bool flag;
        bool flag2;
        if (playSe)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE2);
        }
        this.actSkillObject = this.useSkillObject;
        BattleSkillInfoData skillInfo = this.actSkillObject.skillInfo;
        SkillLvEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<SkillLvMaster>(DataNameKind.Kind.SKILL_LEVEL).getEntityFromId<SkillLvEntity>(skillInfo.skillId, skillInfo.skilllv);
        if (this.logic.checkSelectTargetFunction(entity.funcId, out flag, out flag2))
        {
            if (flag2)
            {
                this.skillselectSvtWindow = this.perf.getSelectSvtWindow();
                this.skillselectSvtWindow.setClose();
                this.openSkillSelectMainSubSvtWindow();
            }
            else
            {
                this.selectMSWindow = this.perf.getSelectMainSubSvtWindow();
                this.selectMSWindow.setClose();
                this.openSkillSelectSvtWindow();
            }
        }
        else
        {
            this.logic.wantUseSkill(skillInfo, skillInfo.svtUniqueId, -1);
            this.myFsm.SendEvent("END_PROC");
        }
    }

    [CompilerGenerated]
    private sealed class <openRetireDialog>c__Iterator2F : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal BattlePerformanceMaster <>f__this;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                    this.$current = new WaitForSeconds(0.7f);
                    this.$PC = 1;
                    return true;

                case 1:
                    SingletonMonoBehaviour<CommonUI>.Instance.OpenNotificationDialog(LocalizationManager.Get("BATTLE_DIALOG_RETIRE_TITLE"), LocalizationManager.Get("BATTLE_DIALOG_RETIRE_CONF"), new NotificationDialog.ClickDelegate(this.<>f__this.retRetireDialog), -1);
                    this.$PC = -1;
                    break;
            }
            return false;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current =>
            this.$current;

        object IEnumerator.Current =>
            this.$current;
    }
}

