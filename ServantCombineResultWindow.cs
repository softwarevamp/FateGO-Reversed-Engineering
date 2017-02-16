using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServantCombineResultWindow : BaseMonoBehaviour
{
    [CompilerGenerated]
    private static Dictionary<string, int> <>f__switch$map32;
    [CompilerGenerated]
    private static Dictionary<string, int> <>f__switch$map33;
    private string asstName;
    private ServantEntity baseSvtData;
    protected int baseSvtId;
    protected UserServantEntity baseUsrSvtData;
    [SerializeField]
    protected Collider bgCollider;
    protected static readonly float CARD_OPEN_POS_X = -370f;
    [SerializeField]
    protected UISprite cardInfoImg;
    protected UICharaGraphTexture charaGraph;
    [SerializeField]
    protected PlayMakerFSM combineFsm;
    protected int currentImgLimitCnt;
    [SerializeField]
    protected UILabel currentMaxLvLb;
    protected UIStandFigureR currentSvtFigure;
    [SerializeField]
    protected UISprite expSlider;
    [SerializeField]
    protected GameObject getExpInfo;
    [SerializeField]
    protected GameObject heroQuestInfo;
    [SerializeField]
    protected UILabel heroQuestInfoDetail;
    [SerializeField]
    protected UILabel heroQuestInfoTitle;
    protected static readonly float IN_DURATION = 2f;
    protected bool isCardChange;
    protected bool isGetNewSkill;
    private bool isLimitCntUp;
    private bool isLimitMax;
    protected bool isOpenQuest;
    private bool isSvtLvMax;
    [SerializeField]
    protected GameObject limitCntUpInfo;
    protected UIStandFigureR limitUpSvtFigure;
    [SerializeField]
    protected GameObject lvUpNoticeInfo;
    [SerializeField]
    protected GameObject maxLevelUpInfo;
    private int maxPlayCnt;
    [SerializeField]
    protected GameObject nextClickImg;
    protected int oldImgLimitCnt;
    [SerializeField]
    protected UILabel oldMaxLvLb;
    protected static readonly float OPEN_DELAY_TIME = 0.2f;
    protected static readonly float OPEN_POS_X = 193f;
    protected static readonly float OPEN_TIME = 0.5f;
    protected static readonly float OUT_DURATION = 3f;
    private int playCnt;
    private SePlayer player;
    private ServantVoiceData[] playVoiceList;
    [SerializeField]
    protected GameObject resCardBase;
    [SerializeField]
    protected GameObject resCardInfo;
    protected Vector3 resCardInfoPos;
    protected List<GameObject> resInfoList;
    [SerializeField]
    protected ServantCombineResultInfoComponent resultInfoDlg;
    protected GameObject resultInfoObject;
    [SerializeField]
    protected UISprite resultSprite;
    protected Vector3 resultSpritePos;
    protected UserServantEntity resUsrSvtData;
    [SerializeField]
    protected GameObject skillGetInfo;
    [SerializeField]
    protected UILabel skillGetInfoDetail;
    [SerializeField]
    protected UILabel skillGetInfoTitle;
    [SerializeField]
    protected UIButton skipBtn;
    [SerializeField]
    protected GameObject skipBtnObj;
    protected UICharaGraphTexture specialCharaGraph;
    protected State state;
    [SerializeField]
    protected GameObject storyQuestInfo;
    [SerializeField]
    protected UILabel storyQuestInfoDetail;
    [SerializeField]
    protected UILabel storyQuestInfoTitle;
    protected string successInfo;
    [SerializeField]
    protected GameObject svtBase;
    [SerializeField]
    protected GameObject svtEqCardBase;
    private ServantVoiceEntity svtVoiceEntity;
    protected Hashtable table = new Hashtable();
    protected static int TEST_FIGURE_ID = 0x18bb4;
    private string vcName;
    private float volume = 1f;

    [DebuggerHidden]
    private IEnumerator changeImgLimitCnt() => 
        new <changeImgLimitCnt>c__Iterator32 { <>f__this = this };

    [DebuggerHidden]
    private IEnumerator changeSpecialCard() => 
        new <changeSpecialCard>c__Iterator33 { <>f__this = this };

    protected void checkGetSkill()
    {
        bool flag = true;
        this.isGetNewSkill = false;
        int[] numArray = this.baseUsrSvtData.getSkillIdList();
        int[] numArray2 = this.resUsrSvtData.getSkillIdList();
        int index = 0;
        for (int i = 0; i < numArray.Length; i++)
        {
            if (!numArray[i].Equals(numArray2[i]))
            {
                flag = false;
                index = i;
                break;
            }
        }
        if (!flag)
        {
            int id = numArray2[index];
            SkillEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SKILL).getEntityFromId<SkillEntity>(id);
            this.skillGetInfoDetail.text = string.Format(LocalizationManager.Get("GET_SKILL_NAME"), entity.name);
            this.resInfoList.Add(this.skillGetInfo);
            this.isGetNewSkill = true;
        }
    }

    public void checkIsCountStop()
    {
        this.isLimitMax = this.resUsrSvtData.isLimitCountMax();
        this.isSvtLvMax = this.resUsrSvtData.isLevelMax();
        if (this.isLimitMax && this.isSvtLvMax)
        {
            this.setSpecialSvtCardImg();
            base.StartCoroutine(this.changeSpecialCard());
        }
        else
        {
            this.ResetDisp();
            this.combineFsm.SendEvent("END_DISP");
        }
    }

    public bool checkIsLimitCntUp()
    {
        int limitCount = this.baseUsrSvtData.limitCount;
        int num2 = this.resUsrSvtData.limitCount;
        this.isLimitCntUp = num2 > limitCount;
        return this.isLimitCntUp;
    }

    public void checkOpenResultInfo()
    {
        this.Init();
        this.resInfoList = new List<GameObject>();
        this.checkQuestOpen();
        this.checkGetSkill();
        if (this.isGetNewSkill || this.isOpenQuest)
        {
            this.combineFsm.SendEvent("SHOW_INFO");
        }
        else
        {
            this.ResetDisp();
            this.combineFsm.SendEvent("SHOW_NEXT");
        }
    }

    protected void checkQuestOpen()
    {
        this.isOpenQuest = false;
        SingletonTemplate<clsQuestCheck>.Instance.mfInit();
        List<int> releaseQuestIdByServantLimit = new List<int>();
        if (this.state == State.LIMITCNT)
        {
            releaseQuestIdByServantLimit = SingletonTemplate<clsQuestCheck>.Instance.GetReleaseQuestIdByServantLimit(this.resUsrSvtData.svtId);
        }
        else if (this.state == State.LEVELUP)
        {
            releaseQuestIdByServantLimit = SingletonTemplate<clsQuestCheck>.Instance.GetReleaseQuestIdByServantLv(this.resUsrSvtData.svtId);
        }
        QuestEntity entity = null;
        this.storyQuestInfoTitle.text = LocalizationManager.Get("OPEN_STORY_QUEST_TITLE");
        this.heroQuestInfoTitle.text = LocalizationManager.Get("OPEN_HERO_QUEST_TITLE");
        if ((releaseQuestIdByServantLimit != null) && (releaseQuestIdByServantLimit.Count > 0))
        {
            for (int i = 0; i < releaseQuestIdByServantLimit.Count; i++)
            {
                if (releaseQuestIdByServantLimit[i] > 0)
                {
                    entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.QUEST).getEntityFromId<QuestEntity>(releaseQuestIdByServantLimit[i]);
                    if (entity.type == 3)
                    {
                        this.storyQuestInfoDetail.text = string.Format(LocalizationManager.Get("OPEN_QUEST_NAME"), entity.name);
                        this.resInfoList.Add(this.storyQuestInfo);
                    }
                    else if (entity.type == 6)
                    {
                        this.heroQuestInfoDetail.text = string.Format(LocalizationManager.Get("OPEN_QUEST_NAME"), entity.name);
                        this.resInfoList.Add(this.heroQuestInfo);
                    }
                }
            }
            this.isOpenQuest = true;
        }
    }

    public void checkSvtEqLimitUp()
    {
        int num = this.baseUsrSvtData.getLimitCount();
        int num2 = this.resUsrSvtData.getLimitCount();
        if (num < num2)
        {
            this.combineFsm.SendEvent("LIMITCOUNT_UP");
        }
        else
        {
            this.combineFsm.SendEvent("LIMITCOUNT_STAY");
        }
    }

    private void ClearAlpha(GameObject target, string callback)
    {
        float duration = 0.2f;
        if (target != null)
        {
            TweenScale.Begin(target, duration, Vector3.zero);
        }
        TweenAlpha alpha = TweenAlpha.Begin(target, duration, 0f);
        if (alpha != null)
        {
            alpha.method = UITweener.Method.EaseInOut;
            alpha.eventReceiver = base.gameObject;
            alpha.callWhenFinished = callback;
        }
    }

    protected void clearCardInfo()
    {
        this.ClearAlpha(this.cardInfoImg.gameObject, string.Empty);
    }

    public void clickNext()
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        this.combineFsm.SendEvent("END_DISP");
    }

    public void ClickSkip()
    {
        this.ResetDisp();
        this.combineFsm.SendEvent("SKIP_EVENT");
    }

    public void closeResultInfoDlg()
    {
        this.resultInfoDlg.Close();
        this.Init();
    }

    private void createResSvtFigure()
    {
        if (this.currentSvtFigure == null)
        {
            this.svtBase.SetActive(true);
            this.oldImgLimitCnt = ImageLimitCount.GetImageLimitCount(this.baseSvtId, this.baseUsrSvtData.limitCount);
            this.currentSvtFigure = StandFigureManager.CreateRenderPrefab(this.svtBase, this.baseSvtId, this.oldImgLimitCnt, Face.Type.NORMAL, 1, null);
        }
    }

    private void destroylimitUpSvtFigure()
    {
        if (this.limitUpSvtFigure != null)
        {
            UnityEngine.Object.Destroy(this.limitUpSvtFigure.gameObject);
            this.limitUpSvtFigure = null;
        }
    }

    private void destroySpecialSvtCardImg()
    {
        if (this.specialCharaGraph != null)
        {
            this.resCardInfo.SetActive(false);
            UnityEngine.Object.Destroy(this.specialCharaGraph.gameObject);
            this.specialCharaGraph = null;
        }
    }

    private void destroySvtCardImg()
    {
        if (this.charaGraph != null)
        {
            this.resCardInfo.SetActive(false);
            UnityEngine.Object.Destroy(this.charaGraph.gameObject);
            this.charaGraph = null;
        }
    }

    private void destroySvtFigure()
    {
        if (this.currentSvtFigure != null)
        {
            UnityEngine.Object.Destroy(this.currentSvtFigure.gameObject);
            this.currentSvtFigure = null;
        }
    }

    public void dispLevelUpResult()
    {
        this.state = State.LEVELUP;
        base.gameObject.SetActive(true);
        this.skipBtnObj.SetActive(true);
        List<ServantVoiceData[]> list = this.svtVoiceEntity.getLevelUpVoiceList();
        this.playVoiceList = list[0];
        int lv = this.baseUsrSvtData.lv;
        int num2 = this.resUsrSvtData.lv;
        if (!lv.Equals(num2))
        {
            SingletonMonoBehaviour<SoundManager>.Instance.LoadAudioAssetStorage(this.asstName, new System.Action(this.EndLoad), SoundManager.CueType.ALL);
        }
        string str = "img_txt_success";
        string successInfo = this.successInfo;
        if (successInfo != null)
        {
            int num3;
            if (<>f__switch$map32 == null)
            {
                Dictionary<string, int> dictionary = new Dictionary<string, int>(3) {
                    { 
                        "1",
                        0
                    },
                    { 
                        "2",
                        1
                    },
                    { 
                        "3",
                        2
                    }
                };
                <>f__switch$map32 = dictionary;
            }
            if (<>f__switch$map32.TryGetValue(successInfo, out num3))
            {
                switch (num3)
                {
                    case 0:
                        this.resultSprite.width = 330;
                        this.resultSprite.height = 100;
                        break;

                    case 1:
                        str = "img_txt_greatsuccess";
                        this.resultSprite.width = 460;
                        this.resultSprite.height = 100;
                        break;

                    case 2:
                        str = "img_txt_supersuccess";
                        this.resultSprite.width = 510;
                        this.resultSprite.height = 0x9b;
                        break;
                }
            }
        }
        this.resultSprite.spriteName = str;
        Vector3 movePos = new Vector3(OPEN_POS_X, this.resultSpritePos.y, this.resultSpritePos.z);
        this.showMove(this.resultInfoObject, movePos, OPEN_TIME, OPEN_DELAY_TIME, "showGetExp");
    }

    public void dispLimitCntResult()
    {
        this.state = State.LIMITCNT;
        this.limitCntUpInfo.SetActive(true);
        base.gameObject.SetActive(true);
        int num = this.baseUsrSvtData.getLevelMax();
        this.oldMaxLvLb.text = string.Format(LocalizationManager.Get("LEVEL_INFO"), num);
        this.currentMaxLvLb.text = this.resUsrSvtData.getLevelMax().ToString();
        this.resultSprite.spriteName = "img_txt_limitover";
        Vector3 movePos = new Vector3(OPEN_POS_X, this.resultSpritePos.y, this.resultSpritePos.z);
        this.showMove(this.resultInfoObject, movePos, OPEN_TIME, OPEN_DELAY_TIME, "showLevelUpInfo");
    }

    public void dispSvtEqLevelUpResult()
    {
        this.ResetDisp();
        this.state = State.LEVELUP;
        base.gameObject.SetActive(true);
        this.skipBtnObj.SetActive(true);
        string str = "img_txt_success";
        string successInfo = this.successInfo;
        if (successInfo != null)
        {
            int num;
            if (<>f__switch$map33 == null)
            {
                Dictionary<string, int> dictionary = new Dictionary<string, int>(3) {
                    { 
                        "1",
                        0
                    },
                    { 
                        "2",
                        1
                    },
                    { 
                        "3",
                        2
                    }
                };
                <>f__switch$map33 = dictionary;
            }
            if (<>f__switch$map33.TryGetValue(successInfo, out num))
            {
                switch (num)
                {
                    case 0:
                        str = "img_txt_success";
                        break;

                    case 1:
                        str = "img_txt_greatsuccess";
                        break;

                    case 2:
                        str = "img_txt_supersuccess";
                        break;
                }
            }
        }
        this.resultSprite.spriteName = str;
        this.resultSprite.MakePixelPerfect();
        Vector3 movePos = new Vector3(OPEN_POS_X, this.resultSpritePos.y, this.resultSpritePos.z);
        this.showMove(this.resultInfoObject, movePos, OPEN_TIME, OPEN_DELAY_TIME, "showGetExp");
    }

    public void dispSvtEqLimitCntResult()
    {
        this.state = State.LIMITCNT;
        this.limitCntUpInfo.SetActive(true);
        base.gameObject.SetActive(true);
        int num = this.baseUsrSvtData.getLevelMax();
        this.oldMaxLvLb.text = string.Format(LocalizationManager.Get("LEVEL_INFO"), num);
        this.currentMaxLvLb.text = this.resUsrSvtData.getLevelMax().ToString();
        this.resultSprite.spriteName = "img_txt_limitover";
        Vector3 movePos = new Vector3(OPEN_POS_X, this.resultSpritePos.y, this.resultSpritePos.z);
        this.showMove(this.resultInfoObject, movePos, OPEN_TIME, OPEN_DELAY_TIME, "showMaxLvInfo");
    }

    private void EndDisp()
    {
        this.bgCollider.enabled = true;
        this.nextClickImg.gameObject.SetActive(true);
    }

    private void EndLoad()
    {
        this.maxPlayCnt = this.playVoiceList.Length - 1;
        this.playVoice();
    }

    private void EndPlay()
    {
        if (this.playCnt <= this.maxPlayCnt)
        {
            float delay = this.playVoiceList[this.playCnt].delay;
            base.Invoke("playVoice", delay);
        }
        else
        {
            if (this.currentSvtFigure != null)
            {
                this.currentSvtFigure.SetFace(Face.Type.NORMAL);
            }
            else if (this.limitUpSvtFigure != null)
            {
                this.limitUpSvtFigure.SetFace(Face.Type.NORMAL);
            }
            this.playCnt = 0;
            this.playVoiceList = null;
            this.stopVoice();
        }
    }

    public void hideLvUpInfo()
    {
        this.lvUpNoticeInfo.SetActive(false);
    }

    protected void Init()
    {
        this.bgCollider.enabled = false;
        this.nextClickImg.SetActive(false);
    }

    public void initDisp()
    {
        this.bgCollider.enabled = false;
        base.gameObject.SetActive(false);
        this.baseUsrSvtData = null;
        this.resUsrSvtData = null;
        this.resultInfoObject.transform.localPosition = this.resultSpritePos;
        this.resCardInfo.transform.localPosition = this.resCardInfoPos;
        this.getExpInfo.SetActive(false);
        this.lvUpNoticeInfo.SetActive(false);
        this.resultInfoDlg.Close();
        this.resultInfoDlg.Init();
        this.destroySvtFigure();
        this.destroylimitUpSvtFigure();
        this.destroySvtCardImg();
        this.destroySpecialSvtCardImg();
        this.isGetNewSkill = false;
        this.isOpenQuest = false;
        this.isCardChange = false;
        this.stopVoice();
        this.skipBtnObj.SetActive(false);
        this.state = State.INIT;
    }

    private void MoveAlpha(GameObject target, string callback)
    {
        float duration = 0.2f;
        target.transform.localScale = Vector3.zero;
        if (!target.activeSelf)
        {
            target.SetActive(true);
        }
        TweenScale.Begin(target, duration, Vector3.one);
        target.GetComponent<UIWidget>().alpha = 0f;
        TweenAlpha alpha = TweenAlpha.Begin(target, duration, 1f);
        if (alpha != null)
        {
            alpha.method = UITweener.Method.EaseInOut;
            alpha.eventReceiver = base.gameObject;
            alpha.callWhenFinished = callback;
        }
    }

    public void openResultInfoDlg()
    {
        this.ResetDisp();
        this.resultInfoDlg.OpenResultInfo(this.resInfoList, new System.Action(this.EndDisp));
    }

    private void playVoice()
    {
        int[] numArray = new int[] { 1, 2, 1 };
        Debug.Log("!! ** !! playCnt: " + this.playCnt);
        this.vcName = this.playVoiceList[this.playCnt].id;
        Debug.Log("!! ** !! playVoice: " + this.vcName);
        int face = this.playVoiceList[this.playCnt].face;
        Face.Type nORMAL = Face.Type.NORMAL;
        switch (face)
        {
            case 1:
                nORMAL = Face.Type.PLEASURE;
                break;

            case 2:
                nORMAL = Face.Type.ANGRY;
                break;

            case 3:
                nORMAL = Face.Type.EMBARRASSED;
                break;

            default:
                nORMAL = Face.Type.PLEASURE;
                break;
        }
        if (this.currentSvtFigure != null)
        {
            this.currentSvtFigure.SetFace(nORMAL);
        }
        if (this.limitUpSvtFigure != null)
        {
            this.limitUpSvtFigure.SetFace(nORMAL);
            Debug.Log("!! ** !! limitUpSvtFigure: " + nORMAL);
        }
        this.player = SoundManager.playVoice(this.asstName, this.vcName, this.volume, new System.Action(this.EndPlay));
        this.playCnt++;
    }

    protected void procAfterChangeSvtImg()
    {
        List<ServantVoiceData[]> list = this.svtVoiceEntity.getSpecificLimitCntUpVoiceList(this.resUsrSvtData.limitCount);
        if ((list != null) && (list.Count > 0))
        {
            this.playVoiceList = list[0];
            SingletonMonoBehaviour<SoundManager>.Instance.LoadAudioAssetStorage(this.asstName, new System.Action(this.EndLoad), SoundManager.CueType.ALL);
        }
        this.setBaseSvtCardImg();
    }

    protected void ResetDisp()
    {
        this.resultInfoObject.SetActive(false);
        this.resultInfoObject.transform.localPosition = this.resultSpritePos;
        this.Init();
        switch (this.state)
        {
            case State.LIMITCNT:
                this.maxLevelUpInfo.SetActive(false);
                break;

            case State.LEVELUP:
                this.getExpInfo.SetActive(false);
                this.skipBtnObj.SetActive(false);
                this.resultInfoDlg.Init();
                break;
        }
    }

    private void setBaseSvtCardImg()
    {
        if (this.charaGraph == null)
        {
            this.charaGraph = CharaGraphManager.CreateTexturePrefab(this.resCardBase, this.resUsrSvtData, this.currentImgLimitCnt, 10, null);
            this.resCardInfo.SetActive(true);
        }
    }

    public void setLimitUpResultInfo(UserServantEntity baseData)
    {
        this.bgCollider.enabled = false;
        this.baseUsrSvtData = baseData;
        this.baseSvtId = baseData.svtId;
        this.resUsrSvtData = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_SERVANT).getEntityFromId<UserServantEntity>(this.baseUsrSvtData.id);
        ServantLimitAddMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantLimitAddMaster>(DataNameKind.Kind.SERVANT_LIMIT_ADD);
        int num = master.getVoiceId(this.resUsrSvtData.svtId, this.resUsrSvtData.limitCount);
        int num2 = master.getVoicePrefix(this.resUsrSvtData.svtId, this.resUsrSvtData.limitCount);
        this.svtVoiceEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantVoiceMaster>(DataNameKind.Kind.SERVANT_VOICE).getEntityFromId<ServantVoiceEntity>(num, num2, 2);
        this.asstName = this.svtVoiceEntity.getVoiceAssetName();
        this.createResSvtFigure();
        this.resultInfoObject = this.resultSprite.gameObject;
        this.resultSpritePos = this.resultSprite.gameObject.transform.localPosition;
        this.resCardInfoPos = this.resCardInfo.gameObject.transform.localPosition;
    }

    private void setSpecialSvtCardImg()
    {
        if (this.specialCharaGraph == null)
        {
            this.specialCharaGraph = CharaGraphManager.CreateTexturePrefab(this.resCardBase, this.resUsrSvtData, true, 10, null);
            this.specialCharaGraph.SetAlpha(0f);
        }
    }

    public void setSuccessInfo(string infoIdx, UserServantEntity baseData)
    {
        this.bgCollider.enabled = false;
        this.baseUsrSvtData = baseData;
        this.baseSvtId = baseData.svtId;
        this.resUsrSvtData = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_SERVANT).getEntityFromId<UserServantEntity>(this.baseUsrSvtData.id);
        ServantLimitAddMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantLimitAddMaster>(DataNameKind.Kind.SERVANT_LIMIT_ADD);
        int num = master.getVoiceId(this.resUsrSvtData.svtId, this.resUsrSvtData.limitCount);
        int num2 = master.getVoicePrefix(this.resUsrSvtData.svtId, this.resUsrSvtData.limitCount);
        this.svtVoiceEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantVoiceMaster>(DataNameKind.Kind.SERVANT_VOICE).getEntityFromId<ServantVoiceEntity>(num, num2, 2);
        this.asstName = this.svtVoiceEntity.getVoiceAssetName();
        this.successInfo = infoIdx;
        this.createResSvtFigure();
        this.resultInfoObject = this.resultSprite.gameObject;
        this.resultSpritePos = this.resultSprite.gameObject.transform.localPosition;
        this.resCardInfoPos = this.resCardInfo.gameObject.transform.localPosition;
    }

    private void setSvtEqCardImg()
    {
        if (this.charaGraph == null)
        {
            this.charaGraph = CharaGraphManager.CreateTexturePrefab(this.svtEqCardBase, this.resUsrSvtData, 0, 2, null);
            this.resCardInfo.SetActive(true);
        }
    }

    public void setSvtEqCombineInfo(string infoIdx, UserServantEntity baseData)
    {
        this.bgCollider.enabled = false;
        this.baseUsrSvtData = baseData;
        this.baseSvtId = baseData.svtId;
        this.resUsrSvtData = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_SERVANT).getEntityFromId<UserServantEntity>(this.baseUsrSvtData.id);
        this.successInfo = infoIdx;
        this.setSvtEqCardImg();
        this.resultInfoObject = this.resultSprite.gameObject;
        this.resultSpritePos = this.resultSprite.gameObject.transform.localPosition;
    }

    private void setSvtExp(int nowExp, int startLv)
    {
        int num = this.resUsrSvtData.getLevelMax();
        ServantExpMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantExpMaster>(DataNameKind.Kind.SERVANT_EXP);
        int num2 = master.getLevel(nowExp, this.baseSvtData.expType, num, startLv);
        ServantExpEntity entity = master.getEntityFromId<ServantExpEntity>(this.baseSvtData.expType, num2);
        int exp = 0;
        ServantExpEntity entity2 = master.getEntityFromId<ServantExpEntity>(this.baseSvtData.expType, num2 - 1);
        if (entity2 != null)
        {
            exp = entity2.exp;
        }
        this.expSlider.fillAmount = 1f - (((float) (entity.exp - nowExp)) / ((float) (entity.exp - exp)));
    }

    protected void showCardInfo()
    {
        this.MoveAlpha(this.cardInfoImg.gameObject, "showMaxLvInfo");
    }

    private void showGetExp()
    {
        this.getExpInfo.SetActive(true);
        this.baseSvtData = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.baseUsrSvtData.svtId);
        this.setSvtExp(this.baseUsrSvtData.exp, this.baseUsrSvtData.lv);
        object[] args = new object[] { "from", 0f, "to", 1f, "onupdate", "UpdateValue", "oncomplete", "EndDisp", "time", 2.8f };
        iTween.ValueTo(base.gameObject, iTween.Hash(args));
    }

    protected void showLevelUpInfo()
    {
        this.currentImgLimitCnt = ImageLimitCount.GetImageLimitCount(this.resUsrSvtData.svtId, this.resUsrSvtData.limitCount);
        this.isCardChange = false;
        if (this.oldImgLimitCnt != this.currentImgLimitCnt)
        {
            this.limitUpSvtFigure = StandFigureManager.CreateRenderPrefab(this.svtBase, this.resUsrSvtData.svtId, this.currentImgLimitCnt, Face.Type.NORMAL, 1, null);
            this.limitUpSvtFigure.SetAlpha(0f);
            this.setBaseSvtCardImg();
            base.StartCoroutine(this.changeImgLimitCnt());
            this.isCardChange = true;
        }
        else
        {
            List<ServantVoiceData[]> list = this.svtVoiceEntity.getLimitCntUpVoiceList();
            this.playVoiceList = list[0];
            SingletonMonoBehaviour<SoundManager>.Instance.LoadAudioAssetStorage(this.asstName, new System.Action(this.EndLoad), SoundManager.CueType.ALL);
            this.showMaxLvInfo();
        }
    }

    protected void showMaxLvInfo()
    {
        base.Invoke("clearCardInfo", 1f);
        this.MoveAlpha(this.maxLevelUpInfo, "EndDisp");
    }

    private void showMove(GameObject target, Vector3 movePos, float time, float delay, string callBack)
    {
        target.SetActive(true);
        this.table.Clear();
        this.table.Add("isLocal", true);
        this.table.Add("position", movePos);
        this.table.Add("easetype", "linear");
        this.table.Add("oncomplete", callBack);
        this.table.Add("oncompletetarget", base.gameObject);
        this.table.Add("time", time);
        this.table.Add("delay", delay);
        iTween.MoveTo(target, this.table);
        if (!this.isCardChange)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.STATUS_OPEN);
        }
    }

    protected void showSpecialCardInfo()
    {
        this.MoveAlpha(this.cardInfoImg.gameObject, "EndDisp");
    }

    public void showStatusInfo()
    {
        this.ResetDisp();
        int num = 0;
        int num2 = 0;
        if (this.resUsrSvtData.hp > this.baseUsrSvtData.hp)
        {
            num = this.resUsrSvtData.hp - this.baseUsrSvtData.hp;
        }
        if (this.resUsrSvtData.atk > this.baseUsrSvtData.atk)
        {
            num2 = this.resUsrSvtData.atk - this.baseUsrSvtData.atk;
        }
        LevelUpInfoData infoData = new LevelUpInfoData {
            oldLv = this.baseUsrSvtData.lv,
            currentLv = this.resUsrSvtData.lv,
            currentHp = this.resUsrSvtData.hp,
            increHpVal = num,
            currentAtk = this.resUsrSvtData.atk,
            increAtkVal = num2
        };
        this.resultInfoDlg.OpenLevelUpInfo(infoData, new System.Action(this.EndDisp));
    }

    public void stopVoice()
    {
        if (this.player != null)
        {
            SoundManager.stopVoice(this.asstName, this.vcName, 0f);
            this.player = null;
            this.playCnt = 0;
            this.maxPlayCnt = 0;
            SingletonMonoBehaviour<SoundManager>.Instance.ReleaseAudioAssetStorage(this.asstName);
        }
    }

    public void UpdateValue(float val)
    {
        int nowExp = Mathf.FloorToInt(Mathf.Lerp((float) this.baseUsrSvtData.exp, (float) this.resUsrSvtData.exp, val));
        this.setSvtExp(nowExp, this.baseUsrSvtData.lv);
        float fillAmount = this.expSlider.fillAmount;
        SoundManager.playSystemSe(SeManager.SystemSeKind.METER);
        if (fillAmount == 1f)
        {
            this.lvUpNoticeInfo.SetActive(true);
        }
        else
        {
            this.lvUpNoticeInfo.SetActive(false);
        }
    }

    [CompilerGenerated]
    private sealed class <changeImgLimitCnt>c__Iterator32 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal ServantCombineResultWindow <>f__this;
        internal Vector3 <openPos>__0;

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
                    this.<>f__this.currentSvtFigure.MoveAlpha(ServantCombineResultWindow.OUT_DURATION, 0f, this.<>f__this.gameObject, "destroySvtFigure");
                    this.<>f__this.limitUpSvtFigure.MoveAlpha(ServantCombineResultWindow.IN_DURATION, 1f, this.<>f__this.gameObject, "procAfterChangeSvtImg");
                    this.$current = new WaitForSeconds(0.5f);
                    this.$PC = 1;
                    goto Label_012B;

                case 1:
                    this.<openPos>__0 = new Vector3(ServantCombineResultWindow.CARD_OPEN_POS_X, this.<>f__this.resCardInfo.transform.localPosition.y, this.<>f__this.resCardInfo.transform.localPosition.z);
                    this.<>f__this.showMove(this.<>f__this.resCardInfo, this.<openPos>__0, ServantCombineResultWindow.OPEN_TIME, ServantCombineResultWindow.OPEN_DELAY_TIME, "showCardInfo");
                    this.$current = 0;
                    this.$PC = 2;
                    goto Label_012B;

                case 2:
                    this.$PC = -1;
                    break;
            }
            return false;
        Label_012B:
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
    private sealed class <changeSpecialCard>c__Iterator33 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal ServantCombineResultWindow <>f__this;

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
                    SoundManager.playSystemSe(SeManager.SystemSeKind.STATUS_OPEN);
                    this.<>f__this.ClearAlpha(this.<>f__this.charaGraph.gameObject, "distroySvtCardImg");
                    this.$current = new WaitForSeconds(0.2f);
                    this.$PC = 1;
                    goto Label_00A8;

                case 1:
                    this.<>f__this.MoveAlpha(this.<>f__this.specialCharaGraph.gameObject, "showSpecialCardInfo");
                    this.$current = 0;
                    this.$PC = 2;
                    goto Label_00A8;

                case 2:
                    this.$PC = -1;
                    break;
            }
            return false;
        Label_00A8:
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

    protected enum QUESTTYPE
    {
        FRIENDSHIP = 3,
        HEROBALLAD = 6
    }

    protected enum State
    {
        INIT,
        LIMITCNT,
        GETEXP,
        LEVELUP,
        OPENQUEST,
        SVTEQ
    }
}

