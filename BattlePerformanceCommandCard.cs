using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BattlePerformanceCommandCard : BaseMonoBehaviour
{
    private List<GameObject> aubelist = new List<GameObject>();
    private BattleData Bdata;
    public static int cardsize_w = 200;
    public Transform[] cardTr;
    public GameObject chainBonus;
    private GameObject chainBonusObject;
    public PlayMakerFSM commandfsm;
    private BattleCommandData[] commandlist;
    public GameObject commandprefab;
    public Transform commandrootTransform;
    public Transform criticalpointTr;
    public GameObject cutin_grand_a;
    public GameObject cutin_grand_b;
    public GameObject cutin_grand_q;
    public GameObject cutin_ordererror;
    public GameObject cutin_single;
    public GameObject cutin_trinity_a;
    public GameObject cutin_trinity_b;
    public GameObject cutin_trinity_q;
    private int drawcount;
    public GameObject effect_takecri;
    public Transform[] excardTr;
    public Transform extraPos;
    public GameObject firstaura_a;
    public GameObject firstaura_b;
    public GameObject firstaura_q;
    private int firstAuraType;
    public GameObject firstbonus_a;
    public GameObject firstbonus_b;
    public GameObject firstbonus_q;
    private GameObject firstBonusObject;
    public GameObject highSpeedArrowOff;
    public GameObject highSpeedArrowOn;
    public GameObject highSpeedButton;
    private BattleLogic logic;
    private int maxdrawcount;
    public Transform[] miniPos;
    private Vector3 miniScale = new Vector3(0.3f, 0.3f);
    public Transform[] npcardTr;
    public Transform[] npgaugeTr;
    public Transform npTargetTr;
    public GameObject[] p_commandlist;
    private BattlePerformance perf;
    public BattleSealedCommandWindowComponent sealedWindow;
    private int[] selectcommand;
    public GameObject selectCommandPrefab;
    private BattleCommandComponent[] selectedcomponents;
    public UISprite selectinfo_sprite;
    public BattleTDConfWIndowComponent tdConfWindow;
    private BattleLogic.TutorialStringData[] TSD = new BattleLogic.TutorialStringData[] { new BattleLogic.TutorialStringData(0f, new Vector2(0f, 160f), 0x1a), new BattleLogic.TutorialStringData(0f, new Vector2(0f, 160f), 0x1a), new BattleLogic.TutorialStringData(0f, new Vector2(0f, 160f), 0x1a), new BattleLogic.TutorialStringData(0f, new Vector2(200f, 60f), 0x1c), new BattleLogic.TutorialStringData(180f, new Vector2(310f, -30f), 0x1a), new BattleLogic.TutorialStringData(0f, new Vector2(0f, 140f), 0x16), new BattleLogic.TutorialStringData(0f, new Vector2(0f, 130f), 0x1a) };
    private Vector2[] TutorialArrow01 = new Vector2[] { new Vector2(0f, 0f), new Vector2(-200f, 0f), new Vector2(-400f, 0f) };
    private Vector2[] TutorialArrow05 = new Vector2[] { new Vector2(-167f, 165f), new Vector2(-200f, -80f), new Vector2(-400f, -80f) };
    private Vector2 TutorialArrow22 = new Vector2(395f, 200f);
    private Vector2[] TutorialArrow31 = new Vector2[] { new Vector2(0f, -50f), new Vector2(-200f, -50f), new Vector2(-400f, -50f) };
    private Rect TutorialSquare01 = new Rect(-500f, -230f, 600f, 250f);
    private Rect[] TutorialSquare05 = new Rect[] { new Rect(-270f, -30f, 210f, 250f), new Rect(-500f, -230f, 400f, 250f) };
    private Rect TutorialSquare22 = new Rect(300f, 180f, 200f, 100f);
    private Rect[] TutorialSquare32 = new Rect[] { new Rect(-500f, -230f, 600f, 250f), new Rect(230f, -290f, 130f, 70f) };

    public void callbackCommandTutorial01()
    {
        Vector2[] posList = new Vector2[] { new Vector2(0f, 0f), new Vector2(-200f, 0f), new Vector2(-400f, 0f) };
        Rect rect = new Rect(-500f, -230f, 600f, 250f);
        SingletonMonoBehaviour<CommonUI>.Instance.OpenTutorialArrowMark(posList, (float) 0f, rect, null);
        this.commandfsm.SendEvent("END_PROC");
    }

    public void callbackTutorial50()
    {
        this.commandfsm.SendEvent("END_PROC");
    }

    public void callbackTutorialSpeedNext()
    {
        SingletonMonoBehaviour<CommonUI>.Instance.OpenTutorialNotificationDialogArrow(string.Empty, this.TutorialArrow01, this.TutorialSquare01, (float) 0f, new Vector2(), -1, null);
    }

    public void cancelFirstBonus()
    {
        this.firstAuraType = -1;
        if (this.firstBonusObject != null)
        {
            UnityEngine.Object.Destroy(this.firstBonusObject);
        }
        for (int i = 0; i < this.p_commandlist.Length; i++)
        {
            this.p_commandlist[i].GetComponent<BattleCommandComponent>().stopFirstAura();
        }
    }

    public void changeNoSelectCards()
    {
        for (int i = 0; i < (this.p_commandlist.Length - 1); i++)
        {
            BattleCommandComponent component = this.p_commandlist[i].GetComponent<BattleCommandComponent>();
            component.stopAnimation();
            if (!component.isSelect())
            {
                TweenColor.Begin(this.p_commandlist[i], 0.2f, Color.clear);
                component.stopFirstAura();
            }
        }
    }

    public void checkAutoBattle()
    {
        if (this.perf.data.systemflg_autobattle)
        {
            this.commandfsm.SendEvent("OK");
        }
        else
        {
            this.commandfsm.SendEvent("NG");
        }
    }

    public void checkChainBonus(int targetIndex)
    {
        bool flag = false;
        int num = 0;
        for (int i = 0; i < 3; i++)
        {
            if ((this.selectedcomponents[i] != null) && this.selectedcomponents[i].isTreasureDvc())
            {
                num++;
            }
            else
            {
                num = 0;
            }
            if (2 <= num)
            {
                flag = true;
            }
        }
        if (!flag)
        {
            UnityEngine.Object.Destroy(this.chainBonusObject);
            this.chainBonusObject = null;
        }
        else if (this.chainBonusObject == null)
        {
            this.chainBonusObject = base.createObject(this.chainBonus, base.gameObject.transform, null);
        }
    }

    public void checkDrawCount()
    {
        if (this.maxdrawcount <= this.drawcount)
        {
            this.commandfsm.SendEvent("SELECT_EXE");
        }
        if (this.logic.debug_battlewin || this.logic.debug_wavewin)
        {
            this.selectAutoCard();
        }
    }

    private bool checkSpeedButtonTutorial()
    {
        if ((this.Bdata.isTutorial() && (this.logic.getTutorialId() == 2)) && (this.logic.getTurn() == 2))
        {
            if (this.Bdata.tutorialState != 10)
            {
                return true;
            }
            this.Bdata.tutorialState = 20;
            SingletonMonoBehaviour<CommonUI>.Instance.CloseTutorialNotificationDialogArrow(new System.Action(this.callbackTutorialSpeedNext));
        }
        return false;
    }

    public void checkTutorial()
    {
        int num = this.logic.getTutorialId();
        int num2 = this.logic.getWave();
        int num3 = this.logic.getTurn();
        switch (num)
        {
            case 1:
                switch (num3)
                {
                    case 1:
                        SingletonMonoBehaviour<CommonUI>.Instance.OpenTutorialNotificationDialogArrow(LocalizationManager.Get("TUTORIAL_MESSAGE_BATTLE_111"), this.TutorialArrow01, this.TutorialSquare01, this.TSD[0].way, this.TSD[0].pos, this.TSD[0].size, null);
                        goto Label_02F2;

                    case 2:
                        SingletonMonoBehaviour<CommonUI>.Instance.OpenTutorialNotificationDialogArrow(LocalizationManager.Get("TUTORIAL_MESSAGE_BATTLE_122"), this.TutorialArrow01, this.TutorialSquare01, this.TSD[1].way, this.TSD[1].pos, this.TSD[1].size, null);
                        goto Label_02F2;

                    case 3:
                        SingletonMonoBehaviour<CommonUI>.Instance.OpenTutorialNotificationDialogArrow(LocalizationManager.Get("TUTORIAL_MESSAGE_BATTLE_131"), this.TutorialArrow01, this.TutorialSquare01, this.TSD[2].way, this.TSD[2].pos, this.TSD[2].size, null);
                        goto Label_02F2;

                    case 4:
                        SingletonMonoBehaviour<CommonUI>.Instance.OpenTutorialNotificationDialog(LocalizationManager.Get("TUTORIAL_MESSAGE_BATTLE_141"), TutorialFlag.Id.NULL, new System.Action(this.callbackTutorial50));
                        goto Label_02F2;
                }
                if (num3 == 5)
                {
                    SingletonMonoBehaviour<CommonUI>.Instance.OpenTutorialNotificationDialogArrow(LocalizationManager.Get("TUTORIAL_MESSAGE_BATTLE_153"), this.TutorialArrow05, this.TutorialSquare05, this.TSD[3].way, this.TSD[3].pos, this.TSD[3].size, null);
                }
                break;

            case 2:
                if (num3 == 2)
                {
                    this.highSpeedButton.SetActive(true);
                    this.updateHighSpeedObject(this.perf.data.systemflg_acceleration);
                    SingletonMonoBehaviour<CommonUI>.Instance.OpenTutorialNotificationDialogArrow(LocalizationManager.Get("TUTORIAL_MESSAGE_BATTLE_223"), this.TutorialArrow22, this.TutorialSquare22, this.TSD[4].way, this.TSD[4].pos, this.TSD[4].size, null);
                }
                else
                {
                    this.callbackCommandTutorial01();
                }
                break;

            case 3:
                if ((num2 == 0) && (num3 == 1))
                {
                    SingletonMonoBehaviour<CommonUI>.Instance.OpenTutorialNotificationDialogArrow(LocalizationManager.Get("TUTORIAL_MESSAGE_BATTLE_230"), this.TutorialArrow31, this.TutorialSquare01, this.TSD[5].way, this.TSD[5].pos, this.TSD[5].size, null);
                }
                else if ((num2 == 1) && (num3 == 1))
                {
                    SingletonMonoBehaviour<CommonUI>.Instance.OpenTutorialNotificationDialogArrow(LocalizationManager.Get("TUTORIAL_MESSAGE_BATTLE_321"), this.TutorialArrow31, this.TutorialSquare32, this.TSD[6].way, this.TSD[6].pos, this.TSD[6].size, null);
                }
                else
                {
                    this.callbackCommandTutorial01();
                }
                break;
        }
    Label_02F2:
        this.commandfsm.SendEvent("END_PROC");
    }

    public void closeWindow()
    {
    }

    [DebuggerHidden]
    private IEnumerator colOpenNpCard() => 
        new <colOpenNpCard>c__Iterator2E { <>f__this = this };

    public void comboExecute()
    {
        if (this.maxdrawcount <= this.drawcount)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.CloseTutorialNotificationDialogArrow(null);
            if (this.firstBonusObject != null)
            {
                UnityEngine.Object.Destroy(this.firstBonusObject);
            }
            if (this.chainBonusObject != null)
            {
                UnityEngine.Object.Destroy(this.chainBonusObject);
            }
            this.Bdata.setCommandData(new List<BattleCommandData> { 
                this.commandlist[this.selectcommand[0]],
                this.commandlist[this.selectcommand[1]],
                this.commandlist[this.selectcommand[2]]
            }.ToArray());
            this.logic.endSelectCommand();
            for (int i = 0; i < 3; i++)
            {
                this.selectedcomponents[i].startComboCard();
            }
            SoundManager.playSe("ba11");
        }
    }

    public void countupStarPoint()
    {
    }

    public void createCommandCard()
    {
        int num = 9;
        if (this.p_commandlist != null)
        {
            for (int j = 0; j < this.p_commandlist.Length; j++)
            {
                UnityEngine.Object.Destroy(this.p_commandlist[j]);
            }
        }
        this.p_commandlist = new GameObject[num];
        for (int i = 0; i < this.p_commandlist.Length; i++)
        {
            this.p_commandlist[i] = base.createObject(this.commandprefab, this.commandrootTransform, null);
            BattleCommandComponent component = this.p_commandlist[i].GetComponent<BattleCommandComponent>();
            component.setData(null, null);
            component.setTarget(base.gameObject);
            component.setDepth(30 + (i * 20));
            component.setPerf(this.perf);
            if (i < 5)
            {
                this.p_commandlist[i].transform.parent = this.cardTr[i];
                this.p_commandlist[i].transform.localPosition = Vector3.zero;
                this.p_commandlist[i].transform.localRotation = Quaternion.identity;
            }
            else if (i < 8)
            {
                this.p_commandlist[i].transform.parent = this.npcardTr[i - 5];
                this.p_commandlist[i].transform.localPosition = Vector3.zero;
                this.p_commandlist[i].transform.localRotation = Quaternion.identity;
                this.p_commandlist[i].GetComponent<UIWidget>().color = Color.clear;
            }
            else
            {
                this.p_commandlist[i].transform.parent = this.excardTr[i - 8];
                this.p_commandlist[i].transform.localPosition = Vector3.zero;
                this.p_commandlist[i].transform.localRotation = Quaternion.identity;
                this.p_commandlist[i].GetComponent<UIWidget>().color = Color.clear;
            }
            component.addObject("ef_resistarrow", BattleCommandComponent.ADDOBJECT_TYPE.ARROW_RESIST);
            component.addObject("ef_weakarrow", BattleCommandComponent.ADDOBJECT_TYPE.ARROW_WEAK);
        }
        this.selectedcomponents = new BattleCommandComponent[4];
        this.selectedcomponents[3] = this.p_commandlist[8].GetComponent<BattleCommandComponent>();
        this.selectcommand = new int[num];
        this.drawcount = 0;
    }

    public void endCloseSealedWindow()
    {
        this.commandfsm.SendEvent("CLOSE_WINDOW");
    }

    public void endCloseTdConfWindow()
    {
        this.commandfsm.SendEvent("CLOSE_WINDOW");
    }

    public void endComboEffect()
    {
        this.logic.sendFsmEvent("END_PROC");
    }

    public void endMoveCard()
    {
        for (int i = 0; (i < this.p_commandlist.Length) && (i < 5); i++)
        {
            BattleCommandComponent component = this.p_commandlist[i].GetComponent<BattleCommandComponent>();
            component.startCountUp();
            component.startMoveFloat();
        }
        this.logic.resetCriticalPoint();
        this.perf.statusPerf.updateCriticalPoint();
        for (int j = 0; j < this.p_commandlist.Length; j++)
        {
            this.p_commandlist[j].GetComponent<BattleCommandComponent>().setTouchFlg(true);
        }
    }

    public void endOpenCommandCard()
    {
        UIPanel component = base.transform.parent.gameObject.GetComponent<UIPanel>();
        if (component != null)
        {
            component.Invalidate(true);
        }
    }

    public void endOpenTdConf()
    {
    }

    public void fadeOutAllCard()
    {
        for (int i = 0; i < this.p_commandlist.Length; i++)
        {
            this.p_commandlist[i].GetComponent<BattleCommandComponent>().hideOutCard();
        }
    }

    public void fallStar()
    {
        foreach (GameObject obj2 in this.aubelist)
        {
            this.perf.destroyInstantiate(obj2);
        }
        this.aubelist.Clear();
        for (int i = 0; i < 5; i++)
        {
            int num2 = this.p_commandlist[i].GetComponent<BattleCommandComponent>().getCriticalCount();
            for (int k = 0; k < num2; k++)
            {
                GameObject target = this.perf.getEffectInstantiate(BattleEffectControl.ID.STAR, this.criticalpointTr);
                target.transform.ChangeChildsLayer(this.criticalpointTr.gameObject.layer);
                target.transform.position = this.p_commandlist[i].transform.position;
                target.GetComponent<Rigidbody>().useGravity = false;
                Transform transform = target.transform;
                transform.localPosition += new Vector3(-68f, 70f);
                object[] args = new object[] { 
                    "x", 0, "y", 0, "delay", UnityEngine.Random.Range((float) 0f, (float) 0.2f), "time", UnityEngine.Random.Range((float) 0.3f, (float) 0.5f), "easetype", iTween.EaseType.easeInQuart, "oncomplete", "addCriticalBuff", "oncompletetarget", this.p_commandlist[i], "oncompleteparams", target,
                    "islocal", true
                };
                iTween.MoveFrom(target, iTween.Hash(args));
                target.GetComponent<BattleMoveObject>().setTargetTr(this.p_commandlist[i].transform);
            }
        }
        for (int j = 0; j < this.p_commandlist.Length; j++)
        {
            BattleCommandComponent component = this.p_commandlist[j].GetComponent<BattleCommandComponent>();
            if (component.isActiveAndEnabled)
            {
                BattleServantData data = this.perf.data.getServantData(component.getUniqueID());
                if (data != null)
                {
                    component.setBuffIconList(data.buffData);
                }
            }
        }
    }

    public GameObject getBattleCommandCardObject(int actionIndex)
    {
        if ((this.selectedcomponents.Length > actionIndex) && (actionIndex >= 0))
        {
            return this.selectedcomponents[actionIndex].gameObject;
        }
        return null;
    }

    public Transform getCollectCriticalTransform() => 
        this.criticalpointTr;

    public string getComboCutIn()
    {
        string str = null;
        BattleComboData combodata = this.perf.data.combodata;
        if (combodata.flash)
        {
            if (combodata.samecount == 2)
            {
                return "effect/BitEffect/bit_cut_u2";
            }
            if (combodata.samecount == 3)
            {
                return "effect/BitEffect/bit_cut_u3";
            }
            return "effect/BitEffect/bit_cut_ua";
        }
        if (combodata.samecount == 2)
        {
            return "effect/BitEffect/bit_cut_2a";
        }
        if (combodata.samecount == 3)
        {
            str = "effect/BitEffect/bit_cut_3a";
        }
        return str;
    }

    public void giveoutCard()
    {
        this.playAnimation("anim_giveout");
        if (this.p_commandlist != null)
        {
            for (int i = 0; i < this.p_commandlist.Length; i++)
            {
                if (this.p_commandlist[i] != null)
                {
                    this.p_commandlist[i].transform.parent.gameObject.SetActive(true);
                }
            }
        }
    }

    public void giveoutNobleCard()
    {
        for (int i = 5; i < this.commandlist.Length; i++)
        {
            TweenColor color = this.p_commandlist[i].GetComponent<TweenColor>();
            if (color != null)
            {
                UnityEngine.Object.DestroyImmediate(color);
            }
            BattleCommandComponent component = this.p_commandlist[i].GetComponent<BattleCommandComponent>();
            this.p_commandlist[i].GetComponent<UIWidget>().color = Color.white;
            component.startMoveFloat();
        }
    }

    public void hideCommandCard(bool flg)
    {
        for (int i = 0; i < 5; i++)
        {
            TweenColor.Begin(this.p_commandlist[i], 0.3f, !flg ? Color.clear : Color.white);
        }
    }

    public void initHighSpeedMode()
    {
        BattleData data = this.perf.data;
        if (data.isTutorial())
        {
            this.highSpeedButton.SetActive(false);
        }
        else
        {
            this.highSpeedButton.SetActive(true);
            this.updateHighSpeedObject(data.systemflg_acceleration);
        }
    }

    public void Initialize(BattlePerformance inperf, BattleData data, BattleLogic inlogic)
    {
        this.perf = inperf;
        this.Bdata = data;
        this.logic = inlogic;
        this.sealedWindow.setClose();
        this.tdConfWindow.setInitData(BattleWindowComponent.ACTIONTYPE.ALPHA, 0.3f, false);
        this.tdConfWindow.setClose();
    }

    public void initOpen()
    {
        this.commandfsm.SendEvent("INIT_OPEN");
    }

    public void initQuest()
    {
        this.initHighSpeedMode();
    }

    public void LongPress(int markindex)
    {
        for (int i = 0; i < this.p_commandlist.Length; i++)
        {
            BattleCommandComponent component = this.p_commandlist[i].GetComponent<BattleCommandComponent>();
            if (this.Bdata.isTutorial())
            {
                return;
            }
            if (component.checkMark(markindex) && component.isTreasureDvc())
            {
                this.commandfsm.Fsm.Variables.GetFsmInt("markindex").Value = markindex;
                this.commandfsm.SendEvent("LONG_PRESS");
            }
        }
    }

    public void moveupStars()
    {
        this.aubelist.Clear();
        int num = this.Bdata.getCriticalPoint();
        if (0 < num)
        {
            num = (num / 5) + 1;
            for (int i = 0; i < num; i++)
            {
                GameObject target = this.perf.getEffectInstantiate(BattleEffectControl.ID.STAR, this.criticalpointTr);
                target.transform.ChangeChildsLayer(this.criticalpointTr.gameObject.layer);
                target.transform.parent = this.criticalpointTr;
                target.transform.localPosition = Vector3.zero;
                target.GetComponent<Rigidbody>().useGravity = false;
                object[] args = new object[] { "x", UnityEngine.Random.Range(-780, 110), "y", 500, "delay", UnityEngine.Random.Range((float) 0f, (float) 0.3f), "time", 0.3f, "easetype", iTween.EaseType.easeOutQuad, "islocal", true };
                iTween.MoveTo(target, iTween.Hash(args));
                this.aubelist.Add(target);
            }
            base.createObject("effect/ef_critlaunch", this.criticalpointTr, null);
            this.perf.statusPerf.updateCriticalPoint();
        }
    }

    public void OnCloseSealedWindow()
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        this.sealedWindow.Close(new BattleWindowComponent.EndCall(this.endCloseSealedWindow));
    }

    public void OnCloseTdConfWindow()
    {
        this.tdConfWindow.Close(new BattleWindowComponent.EndCall(this.endCloseTdConfWindow));
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
    }

    public void openCommandCard()
    {
        this.playAnimation("anim_draw");
        SoundManager.playSe("ba10");
        for (int i = 0; i < this.p_commandlist.Length; i++)
        {
            this.p_commandlist[i].GetComponent<BattleCommandComponent>().setTouchFlg(false);
        }
        this.updateCardMag();
    }

    public void OpenSealedWindow(int markIndex)
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
        for (int i = 0; i < this.p_commandlist.Length; i++)
        {
            BattleCommandComponent component = this.p_commandlist[i].GetComponent<BattleCommandComponent>();
            if (component.checkMark(markIndex))
            {
                BattleServantData data = this.Bdata.getServantData(component.getUniqueID());
                if (data.isTDSeraled())
                {
                    if (data.isHeroine())
                    {
                        this.sealedWindow.setLabel(LocalizationManager.Get("BATTLE_COMMANDDIALOG_NOFUNC"));
                    }
                    else
                    {
                        this.sealedWindow.setLabel(LocalizationManager.Get("BATTLE_COMMANDDIALOG_DONTUSE"));
                    }
                }
                else if (!data.isNobleAction())
                {
                    this.sealedWindow.setLabel(LocalizationManager.Get("BATTLE_COMMANDDIALOG_DONTTD"));
                }
                else
                {
                    this.sealedWindow.setLabel(LocalizationManager.Get("BATTLE_COMMANDDIALOG_DONTACT"));
                }
            }
        }
        this.sealedWindow.Open(null);
    }

    public void OpenTdConfWindow(int markIndex)
    {
        for (int i = 0; i < this.p_commandlist.Length; i++)
        {
            BattleCommandComponent component = this.p_commandlist[i].GetComponent<BattleCommandComponent>();
            if (component.checkMark(markIndex))
            {
                BattleServantData data = this.Bdata.getServantData(component.getUniqueID());
                this.tdConfWindow.setData(data.getTreasureDvcId(), data.getTreasureDvcLevel());
            }
        }
        SoundManager.playSe("ba18");
        this.tdConfWindow.Open(new BattleWindowComponent.EndCall(this.endOpenTdConf));
    }

    public void playAnimation(string name)
    {
        base.GetComponent<Animation>().Play(name);
    }

    public void playCommandEffect(int index, bool flg)
    {
        if ((this.selectedcomponents.Length > index) && (index >= 0))
        {
            this.selectedcomponents[index].playAttackEffect(flg);
        }
    }

    public void playNobleCardEffect(int index)
    {
        float ftime = 0.5f;
        if ((this.selectedcomponents.Length > index) && (index >= 0))
        {
            SoundManager.playSe("ba23");
            if (this.selectedcomponents[index] != null)
            {
                object[] args = new object[] { "position", this.npTargetTr, "time", ftime };
                iTween.MoveTo(this.selectedcomponents[index].gameObject, iTween.Hash(args));
                this.selectedcomponents[index].playNpAttackEffect(ftime);
            }
        }
    }

    public void playTypeEffect(bool flg)
    {
        for (int i = 0; i < this.selectedcomponents.Length; i++)
        {
            this.selectedcomponents[i].flashType(flg);
        }
    }

    public void registCommandCard(BattleCommandData[] list)
    {
        this.playAnimation("anim_wait");
        for (int i = 0; i < list.Length; i++)
        {
            BattleServantData insvtData = this.Bdata.getServantData(list[i].getUniqueId());
            this.p_commandlist[i].transform.localPosition = Vector3.zero;
            this.p_commandlist[i].transform.localScale = Vector3.zero;
            this.p_commandlist[i].GetComponent<BattleCommandComponent>().setData(list[i], insvtData);
            this.p_commandlist[i].GetComponent<UIWidget>().color = Color.white;
        }
        for (int j = 0; j < this.p_commandlist.Length; j++)
        {
            this.p_commandlist[j].GetComponent<BattleCommandComponent>().setTouchFlg(false);
            UITweener component = this.p_commandlist[j].GetComponent<UITweener>();
            if (component != null)
            {
                UnityEngine.Object.Destroy(component);
            }
        }
        if (this.p_commandlist[8] != null)
        {
            this.p_commandlist[8].GetComponent<BattleCommandComponent>().setData(null, null);
        }
    }

    public void resetCommandCard()
    {
        this.playAnimation("anim_wait");
        for (int i = 5; i < this.p_commandlist.Length; i++)
        {
            this.p_commandlist[i].GetComponent<BattleCommandComponent>().setTouchFlg(false);
            this.p_commandlist[i].GetComponent<BattleCommandComponent>().initView();
        }
    }

    public void selectAutoCard()
    {
        for (int i = 0; i < this.p_commandlist.Length; i++)
        {
            BattleCommandComponent component = this.p_commandlist[i].GetComponent<BattleCommandComponent>();
            if (!component.isSelect() && (0 <= component.getMarkIndex()))
            {
                int num2 = component.getMarkIndex();
                this.commandfsm.Fsm.Variables.GetFsmInt("markindex").Value = num2;
                this.commandfsm.SendEvent("SELECTCARD");
                return;
            }
        }
    }

    public void selectCommandCard(int atcount)
    {
        this.setCountRemaining(atcount);
        this.perf.setSelectTargetFlg(3 == atcount);
    }

    public void selectOK(int markindex)
    {
        for (int i = 0; i < this.p_commandlist.Length; i++)
        {
            BattleCommandComponent comp = this.p_commandlist[i].GetComponent<BattleCommandComponent>();
            if (comp.checkMark(markindex) && comp.isSelect())
            {
                comp.setSelect(false);
                int index = -1;
                for (int j = 0; j < this.selectcommand.Length; j++)
                {
                    if (this.selectcommand[j] == markindex)
                    {
                        this.selectcommand[j] = -1;
                        index = j;
                    }
                }
                this.drawcount--;
                this.selectCommandCard(this.maxdrawcount - this.drawcount);
                object[] args = new object[] { "y", 0, "time", 0.2f, "islocal", true };
                iTween.MoveTo(this.p_commandlist[i], iTween.Hash(args));
                comp.startMoveFloat();
                if (index == 0)
                {
                    this.cancelFirstBonus();
                }
                else
                {
                    this.setFirstAura(comp, this.firstAuraType);
                }
                this.selectedcomponents[index] = null;
                this.checkChainBonus(index);
                comp.resetSelect();
                this.commandfsm.SendEvent("CANCEL");
                return;
            }
        }
        if (this.maxdrawcount <= this.drawcount)
        {
            this.commandfsm.SendEvent("DISSELECT");
        }
        else
        {
            int num4 = -1;
            for (int k = 0; k < this.selectcommand.Length; k++)
            {
                if (this.selectcommand[k] == -1)
                {
                    num4 = k;
                    break;
                }
            }
            for (int m = 0; m < this.p_commandlist.Length; m++)
            {
                BattleCommandComponent component = this.p_commandlist[m].GetComponent<BattleCommandComponent>();
                if (component.checkMark(markindex))
                {
                    if (component.isSealed)
                    {
                        this.commandfsm.SendEvent("OPEN_SEALED");
                        return;
                    }
                    if (component.isTreasureDvc() && component.isDontAction)
                    {
                        this.commandfsm.SendEvent("OPEN_SEALED");
                        return;
                    }
                    component.setSelect(true);
                    this.selectedcomponents[num4] = component;
                    this.commandfsm.Fsm.Variables.GetFsmGameObject("SELECT" + num4).Value = this.p_commandlist[m];
                    object[] objArray2 = new object[] { "y", 20, "time", 0.2f, "islocal", true };
                    iTween.MoveTo(this.p_commandlist[m], iTween.Hash(objArray2));
                    component.stopAnimation();
                    component.selectCard(num4);
                    GameObject stamp = base.createObject(this.selectCommandPrefab, this.commandrootTransform, this.p_commandlist[m].transform);
                    stamp.GetComponent<BattleSelectCommandComponent>().setIndex(num4);
                    component.setSelectStamp(stamp);
                    if (num4 == 0)
                    {
                        this.startFirstBonus(component.getCommandType());
                    }
                    this.checkChainBonus(num4);
                }
                if (!component.isSelect())
                {
                }
            }
            if (this.drawcount < this.maxdrawcount)
            {
                this.drawcount++;
                this.selectCommandCard(this.maxdrawcount - this.drawcount);
                this.selectcommand[num4] = markindex;
            }
            this.commandfsm.SendEvent("SELECT");
        }
    }

    public void setCommandCard(BattleCommandData[] list, int maxdrawcount)
    {
        this.commandlist = list;
        for (int i = 0; i < this.commandlist.Length; i++)
        {
            if (this.commandlist[i] != null)
            {
                this.commandlist[i].markindex = i;
            }
        }
        for (int j = 0; j < this.commandlist.Length; j++)
        {
            this.p_commandlist[j].transform.localPosition = Vector3.zero;
            this.p_commandlist[j].transform.localScale = Vector3.zero;
            if (this.commandlist[j] == null)
            {
                this.p_commandlist[j].GetComponent<BattleCommandComponent>().setData(this.commandlist[j], null);
            }
            else
            {
                this.p_commandlist[j].GetComponent<BattleCommandComponent>().setData(this.commandlist[j], this.Bdata.getServantData(this.commandlist[j].getUniqueId()));
            }
            if (5 <= j)
            {
                this.p_commandlist[j].GetComponent<UIWidget>().color = Color.clear;
            }
        }
        for (int k = this.commandlist.Length; k < this.p_commandlist.Length; k++)
        {
            this.p_commandlist[k].transform.localPosition = Vector3.zero;
            this.p_commandlist[k].transform.localScale = Vector3.zero;
            this.p_commandlist[k].GetComponent<BattleCommandComponent>().setData(null, null);
            iTween[] components = this.p_commandlist[k].GetComponents<iTween>();
            if (components != null)
            {
                foreach (iTween tween in components)
                {
                    UnityEngine.Object.Destroy(tween);
                }
            }
        }
        for (int m = 0; m < this.selectcommand.Length; m++)
        {
            this.selectcommand[m] = -1;
        }
        for (int n = 0; n < 3; n++)
        {
            this.selectedcomponents[n] = null;
        }
        this.selectedcomponents[3].GetComponent<UIWidget>().color = Color.clear;
        this.drawcount = 0;
        this.maxdrawcount = maxdrawcount;
    }

    public void setCountRemaining(int count)
    {
        if (this.selectinfo_sprite != null)
        {
            this.selectinfo_sprite.spriteName = $"img_battle_select{count:0}";
        }
    }

    public void setFirstAura(BattleCommandComponent comp, int type)
    {
        if (BattleCommand.isQUICK(type))
        {
            comp.addFirstAura(this.firstaura_q);
        }
        else if (BattleCommand.isARTS(type))
        {
            comp.addFirstAura(this.firstaura_a);
        }
        else if (BattleCommand.isBUSTER(type))
        {
            comp.addFirstAura(this.firstaura_b);
        }
    }

    [DebuggerHidden]
    private IEnumerator showComboEffect(BattleComboData combo, string endproc) => 
        new <showComboEffect>c__Iterator2D { 
            combo = combo,
            endproc = endproc,
            <$>combo = combo,
            <$>endproc = endproc,
            <>f__this = this
        };

    public void startComboEffect()
    {
        base.StartCoroutine(this.showComboEffect(this.perf.data.combodata, "END_PROC"));
    }

    public void startFirstBonus(int type)
    {
        if (this.firstBonusObject != null)
        {
            UnityEngine.Object.Destroy(this.firstBonusObject);
        }
        if (BattleCommand.isQUICK(type))
        {
            this.firstBonusObject = base.createObject(this.firstbonus_q, base.gameObject.transform, null);
        }
        else if (BattleCommand.isARTS(type))
        {
            this.firstBonusObject = base.createObject(this.firstbonus_a, base.gameObject.transform, null);
        }
        else if (BattleCommand.isBUSTER(type))
        {
            this.firstBonusObject = base.createObject(this.firstbonus_b, base.gameObject.transform, null);
        }
        this.firstAuraType = type;
        for (int i = 0; i < this.p_commandlist.Length; i++)
        {
            BattleCommandComponent comp = this.p_commandlist[i].GetComponent<BattleCommandComponent>();
            if (!comp.isSelect())
            {
                this.setFirstAura(comp, this.firstAuraType);
            }
        }
    }

    public void startMiniCard()
    {
        for (int i = 0; i < 4; i++)
        {
            this.selectedcomponents[i].setMoveMode();
            object[] args = new object[] { "position", this.miniPos[i].position, "time", 0.2f, "easetype", iTween.EaseType.linear };
            iTween.MoveTo(this.selectedcomponents[i].gameObject, iTween.Hash(args));
            object[] objArray2 = new object[] { "scale", this.miniScale, "time", 0.2f, "easetype", iTween.EaseType.linear };
            iTween.ScaleTo(this.selectedcomponents[i].gameObject, iTween.Hash(objArray2));
        }
    }

    public void startOpenNpCard()
    {
        base.StartCoroutine(this.colOpenNpCard());
    }

    public void stopFirstBonus()
    {
        if (this.firstBonusObject != null)
        {
            UnityEngine.Object.Destroy(this.firstBonusObject);
        }
    }

    public void toggleHighSpeedMode()
    {
        if (!this.checkSpeedButtonTutorial())
        {
            BattleData data = this.perf.data;
            data.toggleHighSpeedMode();
            SoundManager.playSe("ba18");
            this.updateHighSpeedObject(data.systemflg_acceleration);
        }
    }

    public void touchCommandCard(int markindex)
    {
        int num3;
        int num = this.logic.getTutorialId();
        int num2 = this.logic.getTurn();
        switch (num)
        {
            case 2:
            case 3:
                if (((num == 2) && (num2 == 2)) && (this.Bdata.tutorialState == 10))
                {
                    return;
                }
                if (((markindex != 0) && (markindex != 1)) && (markindex != 2))
                {
                    return;
                }
                break;

            case 1:
                switch (num2)
                {
                    case 1:
                    case 2:
                    case 3:
                        if (((markindex != 0) && (markindex != 1)) && (markindex != 2))
                        {
                            return;
                        }
                        goto Label_00B3;
                }
                if (((num2 == 5) && (markindex != 0)) && ((markindex != 1) && (markindex != 5)))
                {
                    return;
                }
                break;
        }
    Label_00B3:
        num3 = 0;
        while (num3 < this.p_commandlist.Length)
        {
            BattleCommandComponent component = this.p_commandlist[num3].GetComponent<BattleCommandComponent>();
            if (component.checkMark(markindex))
            {
                if (this.Bdata.systemflg_selectcancel || !component.isSelect())
                {
                    this.commandfsm.Fsm.Variables.GetFsmInt("markindex").Value = markindex;
                    this.commandfsm.SendEvent("SELECTCARD");
                }
                return;
            }
            num3++;
        }
    }

    public void transformSvtFace(BattleServantData svtData)
    {
        foreach (BattleCommandComponent component in this.selectedcomponents)
        {
            component.transformSvtFace(svtData);
        }
    }

    public void updateCard()
    {
        for (int i = 5; i < 8; i++)
        {
            if (this.p_commandlist[i] != null)
            {
                this.p_commandlist[i].GetComponent<BattleCommandComponent>().setData(null, null);
            }
        }
        foreach (GameObject obj2 in this.p_commandlist)
        {
            BattleCommandComponent component = obj2.GetComponent<BattleCommandComponent>();
            if (component != null)
            {
                component.updateView(false);
            }
        }
    }

    public void updateCardMag()
    {
        BattleServantData data = this.Bdata.getServantData(this.Bdata.globaltargetId);
        if (data != null)
        {
            int targetClass = data.getClassId();
            if (this.p_commandlist != null)
            {
                for (int i = 0; i < this.p_commandlist.Length; i++)
                {
                    this.p_commandlist[i].GetComponent<BattleCommandComponent>().updateClassMag(targetClass);
                }
            }
        }
    }

    public void updateHighSpeedObject(int speedMode)
    {
        if (this.perf.data.systemflg_acceleration == 1)
        {
            this.highSpeedArrowOn.gameObject.SetActive(false);
            this.highSpeedArrowOff.gameObject.SetActive(true);
        }
        else
        {
            this.highSpeedArrowOn.gameObject.SetActive(true);
            this.highSpeedArrowOff.gameObject.SetActive(false);
        }
    }

    [CompilerGenerated]
    private sealed class <colOpenNpCard>c__Iterator2E : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal BattlePerformanceCommandCard <>f__this;
        internal bool[] <flglist>__1;
        internal int <i>__2;
        internal int <i>__3;
        internal GameObject <obj>__0;

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
                    this.<obj>__0 = null;
                    this.<flglist>__1 = new bool[3];
                    this.<i>__2 = 0;
                    while (this.<i>__2 < 3)
                    {
                        this.<flglist>__1[this.<i>__2] = this.<>f__this.p_commandlist[5 + this.<i>__2].GetComponent<BattleCommandComponent>().checkObject();
                        if (this.<flglist>__1[this.<i>__2])
                        {
                            this.<>f__this.createObject("effect/ef_noblegauge01", this.<>f__this.gameObject.transform, this.<>f__this.npgaugeTr[this.<i>__2]);
                            this.<obj>__0 = this.<>f__this.createObject("effect/ef_noblegauge02", this.<>f__this.gameObject.transform, this.<>f__this.npgaugeTr[this.<i>__2]);
                            object[] args = new object[] { "position", this.<>f__this.npcardTr[this.<i>__2], "easetype", iTween.EaseType.easeInExpo, "delay", 0.2f, "time", 0.4f };
                            iTween.MoveTo(this.<obj>__0, iTween.Hash(args));
                        }
                        this.<i>__2++;
                    }
                    this.$current = new WaitForSeconds(0.6f);
                    this.$PC = 1;
                    goto Label_0217;

                case 1:
                    this.<i>__3 = 0;
                    while (this.<i>__3 < 3)
                    {
                        if (this.<flglist>__1[this.<i>__3])
                        {
                            this.<>f__this.p_commandlist[5 + this.<i>__3].GetComponent<BattleCommandComponent>().playOpenNobleCard();
                        }
                        this.<i>__3++;
                    }
                    this.$current = new WaitForSeconds(0.2f);
                    this.$PC = 2;
                    goto Label_0217;

                case 2:
                    this.<>f__this.giveoutNobleCard();
                    this.$PC = -1;
                    break;
            }
            return false;
        Label_0217:
            return true;
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

    [CompilerGenerated]
    private sealed class <showComboEffect>c__Iterator2D : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal BattleComboData <$>combo;
        internal string <$>endproc;
        internal BattlePerformanceCommandCard <>f__this;
        internal BattleCommandData <command>__4;
        internal int <i>__2;
        internal int <i>__3;
        internal GameObject <prefab>__0;
        internal string <sename>__1;
        internal BattleServantData <svtData>__5;
        internal BattleComboData combo;
        internal string endproc;

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
                    this.<prefab>__0 = null;
                    this.<sename>__1 = null;
                    this.<i>__2 = 0;
                    while (this.<i>__2 < 3)
                    {
                        if (BattleCommand.isBLANK(this.<>f__this.selectedcomponents[this.<i>__2].getCommandType()))
                        {
                            TweenColor.Begin(this.<>f__this.selectedcomponents[this.<i>__2].gameObject, 0.2f, Color.clear);
                        }
                        this.<i>__2++;
                    }
                    if (this.combo.isChainError())
                    {
                        this.<sename>__1 = "ba17";
                        this.<prefab>__0 = this.<>f__this.cutin_ordererror;
                    }
                    else if (this.combo.flash)
                    {
                        this.<sename>__1 = "ba13";
                        if (BattleCommand.isARTS(this.combo.flashtype))
                        {
                            if ((this.combo.samecount == 1) || (this.combo.samecount == 2))
                            {
                                this.<prefab>__0 = this.<>f__this.cutin_trinity_a;
                            }
                            else if (this.combo.samecount == 3)
                            {
                                this.<sename>__1 = "ba15";
                                this.<prefab>__0 = this.<>f__this.cutin_grand_a;
                            }
                        }
                        else if (BattleCommand.isBUSTER(this.combo.flashtype))
                        {
                            if ((this.combo.samecount == 1) || (this.combo.samecount == 2))
                            {
                                this.<prefab>__0 = this.<>f__this.cutin_trinity_b;
                            }
                            else if (this.combo.samecount == 3)
                            {
                                this.<sename>__1 = "ba15";
                                this.<prefab>__0 = this.<>f__this.cutin_grand_b;
                            }
                        }
                        else if (BattleCommand.isQUICK(this.combo.flashtype))
                        {
                            if ((this.combo.samecount == 1) || (this.combo.samecount == 2))
                            {
                                this.<prefab>__0 = this.<>f__this.cutin_trinity_q;
                            }
                            else if (this.combo.samecount == 3)
                            {
                                this.<sename>__1 = "ba15";
                                this.<prefab>__0 = this.<>f__this.cutin_grand_q;
                            }
                        }
                    }
                    else if (this.combo.samecount == 3)
                    {
                        this.<sename>__1 = "ba15";
                        this.<prefab>__0 = this.<>f__this.cutin_single;
                    }
                    if (this.<prefab>__0 != null)
                    {
                        if (this.<sename>__1 != null)
                        {
                            SoundManager.playSe(this.<sename>__1);
                        }
                        this.<>f__this.createObject(this.<prefab>__0, this.<>f__this.gameObject.transform, null);
                        this.$current = new WaitForSeconds(0.9f);
                        this.$PC = 1;
                        goto Label_072E;
                    }
                    break;

                case 1:
                    break;

                case 2:
                case 3:
                    goto Label_03CB;

                case 4:
                {
                    this.<i>__3 = 0;
                    while (this.<i>__3 < 3)
                    {
                        object[] objArray1 = new object[] { "x", -1.1f, "time", 0.3f, "easetype", iTween.EaseType.easeOutQuad, "islocal", true };
                        iTween.MoveAdd(this.<>f__this.selectedcomponents[this.<i>__3].gameObject, iTween.Hash(objArray1));
                        this.<i>__3++;
                    }
                    this.<command>__4 = new BattleCommandData(this.<>f__this.selectedcomponents[0].getcommandData());
                    this.<command>__4.setTypeAddAttack();
                    this.<>f__this.selectedcomponents[3].gameObject.transform.localScale = Vector3.one;
                    this.<>f__this.selectedcomponents[3].gameObject.transform.position = this.<>f__this.extraPos.position;
                    this.<>f__this.selectedcomponents[3].gameObject.SetActive(true);
                    this.<>f__this.selectedcomponents[3].setData(this.<command>__4, null);
                    this.<>f__this.selectedcomponents[3].GetComponent<UIWidget>().color = Color.white;
                    this.<>f__this.selectedcomponents[3].attachEffect("ef_excard02", 4);
                    this.<svtData>__5 = this.<>f__this.Bdata.getServantData(this.<>f__this.Bdata.globaltargetId);
                    if (this.<svtData>__5 != null)
                    {
                        this.<>f__this.selectedcomponents[3].updateClassMag(this.<svtData>__5.getClassId());
                    }
                    SoundManager.playSe("ba22");
                    object[] args = new object[] { "x", this.<>f__this.selectedcomponents[3].gameObject.transform.position.x + 250f, "time", 0.4f, "easetype", iTween.EaseType.easeOutExpo, "islocal", true };
                    iTween.MoveFrom(this.<>f__this.selectedcomponents[3].gameObject, iTween.Hash(args));
                    this.$current = new WaitForSeconds(0.4f);
                    this.$PC = 5;
                    goto Label_072E;
                }
                case 5:
                    this.<>f__this.selectedcomponents[3].attachEffect("ef_excard01", 5);
                    this.$current = new WaitForSeconds(1.1f);
                    this.$PC = 6;
                    goto Label_072E;

                case 6:
                    goto Label_06F3;

                case 7:
                    this.<>f__this.commandfsm.SendEvent(this.endproc);
                    this.$PC = -1;
                    goto Label_072C;

                default:
                    goto Label_072C;
            }
            this.<>f__this.logic.procComboAct();
            if (this.combo.flash)
            {
                this.<>f__this.selectedcomponents[0].flashComboType(0, this.combo);
                this.<>f__this.selectedcomponents[1].flashComboType(1, this.combo);
                this.<>f__this.selectedcomponents[2].flashComboType(2, this.combo);
                if (this.combo.samecount == 3)
                {
                    this.$current = new WaitForSeconds(0.3f);
                    this.$PC = 2;
                }
                else
                {
                    this.$current = new WaitForSeconds(1.1f);
                    this.$PC = 3;
                }
                goto Label_072E;
            }
        Label_03CB:
            if (this.combo.samecount == 3)
            {
                this.<>f__this.selectedcomponents[0].flashComboSvt(0, this.combo);
                this.<>f__this.selectedcomponents[1].flashComboSvt(1, this.combo);
                this.<>f__this.selectedcomponents[2].flashComboSvt(2, this.combo);
                this.$current = new WaitForSeconds(0.2f);
                this.$PC = 4;
                goto Label_072E;
            }
        Label_06F3:
            this.$current = new WaitForSeconds(0.1f);
            this.$PC = 7;
            goto Label_072E;
        Label_072C:
            return false;
        Label_072E:
            return true;
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

