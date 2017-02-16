using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class CombineResultEffectComponent : MonoBehaviour
{
    private string asstName;
    private int baseSvtId;
    private int baseUsrSvtCollectionLimitCnt;
    private int baseUsrSvtCollictionLv;
    private UserServantEntity baseUsrSvtData;
    public Collider bgCollider;
    protected ClickDelegate callbackFunc;
    private int currentImgLimitCnt;
    private UIStandFigureR currentSvtFigure;
    private GameObject effect;
    private PlayMakerFSM fsm;
    private bool isChangeCardImg;
    private Kind kind;
    public LimitUpResultCheckComponent limitUpResultCheck;
    private UIStandFigureR limitUpSvtFigure;
    private int maxPlayCnt;
    private int oldFriendShipRank;
    private int oldImgLimitCnt;
    private int playCnt;
    private SePlayer player;
    private ServantVoiceData[] playVoiceList;
    private UserServantEntity resUsrSvtData;
    public SkillUpResultWindowComponent skillResultInfoWindow;
    private readonly string[] startAniName = new string[] { "bit_result01", "bit_result02", "bit_result03" };
    private int successInfo;
    public SvtCombineResultWindowComponent svtResultInfoWindow;
    private ServantVoiceEntity svtVoiceEntity;
    public Coroutine Talkdate;
    private int targetId;
    private int targetIdOld;
    private int targetLv;
    private int targetLvOld;
    public GameObject touchInfo;
    private string vcName;
    private float volume = 1f;

    private void Awake()
    {
        this.fsm = base.GetComponent<PlayMakerFSM>();
    }

    public void clickNext()
    {
        if (this.bgCollider.enabled)
        {
            if (this.kind == Kind.SVT_COMBINE)
            {
                this.stopVoice();
            }
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            if (this.callbackFunc != null)
            {
                SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, () => this.callbackFunc(true));
            }
        }
    }

    public void Close()
    {
        this.destroySvtFigure();
        if (this.svtResultInfoWindow.gameObject.activeSelf)
        {
            this.svtResultInfoWindow.Close();
        }
        if (this.skillResultInfoWindow.gameObject.activeSelf)
        {
            this.skillResultInfoWindow.Close();
        }
        base.gameObject.SetActive(false);
    }

    private void destroySvtFigure()
    {
        if (this.currentSvtFigure != null)
        {
            UnityEngine.Object.Destroy(this.currentSvtFigure.gameObject);
            this.currentSvtFigure = null;
        }
        if (this.limitUpSvtFigure != null)
        {
            UnityEngine.Object.Destroy(this.limitUpSvtFigure.gameObject);
            this.limitUpSvtFigure = null;
        }
    }

    private void EndDisp()
    {
        this.bgCollider.enabled = true;
        this.touchInfo.SetActive(true);
    }

    private void EndLoad()
    {
        this.fsm.SendEvent("START_ANIMATION");
    }

    private void EndPlay()
    {
        if (this.playCnt < this.maxPlayCnt)
        {
            float delay = this.playVoiceList[this.playCnt].delay;
            base.Invoke("playVoice", delay);
        }
        else
        {
            if (this.player != null)
            {
                this.stopVoice();
            }
            SingletonMonoBehaviour<ScriptManager>.Instance.closeTalk(this.Talkdate);
            this.Talkdate = null;
            this.playCnt = 0;
            this.playVoiceList = null;
            if ((this.kind == Kind.LIMITUP) || (this.kind == Kind.MATERIAL_LIMIT_UP))
            {
                this.EndDisp();
            }
        }
    }

    private ServantVoiceData[] getLimitUpSvtVoiceList()
    {
        ServantVoiceData[] dataArray = null;
        if (this.isChangeCardImg)
        {
            if (this.resUsrSvtData.isLimitCountMax())
            {
                List<ServantVoiceData[]> list = this.svtVoiceEntity.getCntStopVoiceList();
                if ((list != null) && (list.Count > 0))
                {
                    return list[0];
                }
                return null;
            }
            if (this.svtVoiceEntity != null)
            {
                List<ServantVoiceData[]> list2 = this.svtVoiceEntity.getSpecificLimitCntUpVoiceList(this.resUsrSvtData.limitCount);
                if ((list2 != null) && (list2.Count > 0))
                {
                    return list2[0];
                }
            }
            return null;
        }
        if (this.svtVoiceEntity == null)
        {
            return dataArray;
        }
        List<ServantVoiceData[]> list3 = this.svtVoiceEntity.getLimitCntUpVoiceList();
        if ((list3 != null) && (list3.Count > 0))
        {
            return list3[0];
        }
        return null;
    }

    private void getSvtVoiceData()
    {
        this.playCnt = 0;
        this.maxPlayCnt = 0;
        this.asstName = null;
        this.playVoiceList = null;
        ServantLimitAddMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantLimitAddMaster>(DataNameKind.Kind.SERVANT_LIMIT_ADD);
        int num = master.getVoiceId(this.resUsrSvtData.svtId, this.resUsrSvtData.limitCount);
        int num2 = master.getVoicePrefix(this.resUsrSvtData.svtId, this.resUsrSvtData.limitCount);
        this.svtVoiceEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantVoiceMaster>(DataNameKind.Kind.SERVANT_VOICE).getEntityFromId<ServantVoiceEntity>(num, num2, 2);
        if (this.svtVoiceEntity != null)
        {
            this.asstName = this.svtVoiceEntity.getVoiceAssetName();
        }
    }

    public void InitCombineEffect()
    {
        this.effect = this.fsm.FsmVariables.GetFsmGameObject("ResultEffect").Value;
        this.effect.transform.localScale = Vector3.one;
        Debug.Log("** !! ** InitCombineEffect effect: " + this.effect);
        this.bgCollider.enabled = false;
        this.touchInfo.SetActive(false);
        SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, null);
    }

    private void loadVoiceData()
    {
        if (this.asstName != null)
        {
            SingletonMonoBehaviour<SoundManager>.Instance.LoadAudioAssetStorage(this.asstName, new System.Action(this.EndLoad), SoundManager.CueType.ALL);
        }
        else
        {
            this.EndLoad();
        }
    }

    private void playVoice()
    {
        if (((this.asstName != null) && (this.maxPlayCnt != 0)) && (this.playVoiceList.Length > this.playCnt))
        {
            this.vcName = this.playVoiceList[this.playCnt].id;
            Debug.Log("!! ** !! playVoice: " + this.vcName);
            int face = this.playVoiceList[this.playCnt].face;
            if (this.currentSvtFigure != null)
            {
                this.currentSvtFigure.SetFace((Face.Type) face);
                string text = string.Empty;
                if (this.playVoiceList.Length > 0)
                {
                    for (int i = 0; i < this.playVoiceList.Length; i++)
                    {
                        Debug.LogError(this.playVoiceList[i].id);
                        text = text + this.playVoiceList[i].text;
                    }
                }
                if (this.Talkdate == null)
                {
                    this.Talkdate = base.StartCoroutine(SingletonMonoBehaviour<ScriptManager>.Instance.settalk(1, text, this.playVoiceList[this.playCnt].delay));
                }
            }
            if ((this.limitUpSvtFigure != null) && this.limitUpSvtFigure.gameObject.activeSelf)
            {
                this.limitUpSvtFigure.SetFace((Face.Type) face);
            }
            this.player = SoundManager.playVoice(this.asstName, this.vcName, this.volume, new System.Action(this.EndPlay));
            this.playCnt++;
        }
    }

    private void setBaseSvtFigure()
    {
        string nodename = this.fsm.FsmVariables.GetFsmString("BaseSvtNodeName").Value;
        UIPanel component = this.effect.transform.getNodeFromName(nodename, true).GetComponent<UIPanel>();
        int imageLimitCount = ImageLimitCount.GetImageLimitCount(this.baseUsrSvtData.svtId, this.baseUsrSvtData.limitCount);
        this.currentSvtFigure = StandFigureManager.CreateRenderPrefab(component.gameObject, this.baseUsrSvtData.svtId, imageLimitCount, Face.Type.NORMAL, 1, null);
        this.currentSvtFigure.transform.localPosition = new Vector3(-306f, 380f, 0f);
    }

    private void setBaseSvtGraphCard()
    {
        string nodename = this.fsm.FsmVariables.GetFsmString("BaseSvtCardNodeName").Value;
        Transform transform = this.effect.transform.getNodeFromName(nodename, true);
        float x = this.fsm.FsmVariables.GetFsmFloat("CardScale").Value;
        int imageLimitCount = ImageLimitCount.GetCardImageLimitCount(this.baseUsrSvtData.svtId, this.baseUsrSvtData.limitCount, true, true);
        UICharaGraphTexture texture = CharaGraphManager.CreateTexturePrefab(transform.gameObject, this.baseUsrSvtData, imageLimitCount, 10, null);
        texture.transform.localPosition = Vector3.zero;
        texture.transform.localScale = new Vector3(x, x, x);
    }

    public void SetCardParam()
    {
        Debug.Log("** !! ** SetCardParam effect: " + this.effect);
        this.resUsrSvtData = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_SERVANT).getEntityFromId<UserServantEntity>(this.baseUsrSvtData.id);
        string str = "bit_result_skill";
        switch (this.kind)
        {
            case Kind.SVT_COMBINE:
                str = this.startAniName[this.successInfo - 1];
                this.fsm.FsmVariables.GetFsmString("StartAnimationName").Value = str;
                this.setBaseSvtFigure();
                this.svtResultInfoWindow.setBefResultState(this.baseUsrSvtData);
                this.getSvtVoiceData();
                this.loadVoiceData();
                return;

            case Kind.LIMITUP:
                break;

            case Kind.SKILL_LEVELUP:
                this.SetCardParam_Skill("bit_result_skill");
                return;

            case Kind.SKILLOPEN:
                this.SetCardParam_Skill("bit_result_skilldeliverance");
                return;

            case Kind.TREASUREDVC_LEVELUP:
                this.SetCardParam_TreasureDvc("bit_result_skill");
                return;

            case Kind.TREASUREDVCOPEN:
                this.SetCardParam_TreasureDvc("bit_result_nobledeliverance");
                return;

            case Kind.SVTEQ_COMBINE:
            {
                string str2 = this.fsm.FsmVariables.GetFsmString("BaseSvtNodeName").Value;
                Transform transform = this.effect.transform.getNodeFromName(str2, true);
                float x = this.fsm.FsmVariables.GetFsmFloat("SvtEqCardScale").Value;
                UICharaGraphTexture texture = CharaGraphManager.CreateTexturePrefab(transform.gameObject, this.resUsrSvtData, true, 10, null);
                texture.transform.localPosition = new Vector3(-50f, 92f, 0f);
                texture.transform.localScale = new Vector3(x, x, x);
                this.svtResultInfoWindow.setBefResultState(this.baseUsrSvtData);
                str = this.startAniName[this.successInfo - 1];
                this.fsm.FsmVariables.GetFsmString("StartAnimationName").Value = str;
                this.fsm.SendEvent("START_ANIMATION");
                return;
            }
            case Kind.SKILL_RANKUP:
                this.SetCardParam_Skill("bit_result_skillrank");
                return;

            case Kind.TREASUREDVC_RANKUP:
                this.SetCardParam_TreasureDvc("bit_result_skillrank");
                return;

            case Kind.FRIENDSHIP_UP:
                this.setBaseSvtFigure();
                this.fsm.FsmVariables.GetFsmString("StartAnimationName").Value = "bit_result_skill";
                this.fsm.SendEvent("START_ANIMATION");
                return;

            case Kind.MATERIAL_LIMIT_UP:
            {
                UserServantEntity entity = new UserServantEntity(this.baseUsrSvtData);
                entity.limitCount++;
                this.resUsrSvtData = entity;
                break;
            }
            case Kind.POWER_UP:
                this.setBaseSvtFigure();
                this.fsm.FsmVariables.GetFsmString("StartAnimationName").Value = "bit_result_powerup";
                this.fsm.SendEvent("START_ANIMATION");
                return;

            default:
                return;
        }
        this.setLimitUpBaseSvtFigure();
        this.isChangeCardImg = false;
        string nodename = this.fsm.FsmVariables.GetFsmString("CrtMaxLvNodeName").Value;
        UIExtrusionLabel component = this.effect.transform.getNodeFromName(nodename, true).GetComponent<UIExtrusionLabel>();
        component.text = this.baseUsrSvtData.getLevelMax().ToString();
        string str4 = this.fsm.FsmVariables.GetFsmString("ResMaxLvNodeName").Value;
        UIExtrusionLabel label2 = this.effect.transform.getNodeFromName(str4, true).GetComponent<UIExtrusionLabel>();
        label2.text = this.resUsrSvtData.getLevelMax().ToString();
        this.oldImgLimitCnt = ImageLimitCount.GetImageLimitCount(this.baseUsrSvtData.svtId, this.baseUsrSvtData.limitCount);
        this.currentImgLimitCnt = ImageLimitCount.GetImageLimitCount(this.resUsrSvtData.svtId, this.resUsrSvtData.limitCount);
        this.getSvtVoiceData();
        if ((this.oldImgLimitCnt != this.currentImgLimitCnt) || this.resUsrSvtData.isLimitCountMax())
        {
            this.isChangeCardImg = true;
            this.setLimitUpResSvtFigure();
            this.setBaseSvtGraphCard();
            this.setResultSvtGraphCard();
            str = "bit_result_advent02";
        }
        else
        {
            ServantVoiceData[] dataArray = this.getLimitUpSvtVoiceList();
            int face = 0;
            if (dataArray != null)
            {
                face = dataArray[0].face;
            }
            if (this.currentSvtFigure != null)
            {
                this.currentSvtFigure.SetFace((Face.Type) face);
            }
            str = "bit_result_advent01";
        }
        this.fsm.FsmVariables.GetFsmString("StartAnimationName").Value = str;
        this.loadVoiceData();
    }

    private void SetCardParam_Skill(string startName)
    {
        this.setBaseSvtFigure();
        Debug.Log("** !! ** StartAnimationName: " + startName);
        this.fsm.FsmVariables.GetFsmString("StartAnimationName").Value = startName;
        this.fsm.SendEvent("START_ANIMATION");
    }

    private void SetCardParam_TreasureDvc(string startName)
    {
        this.setBaseSvtFigure();
        Debug.Log("** !! ** StartAnimationName: " + startName);
        this.fsm.FsmVariables.GetFsmString("StartAnimationName").Value = startName;
        this.fsm.SendEvent("START_ANIMATION");
    }

    public void setEffectEnable()
    {
        this.effect.SetActive(true);
    }

    public void SetFriendshipUpInfo(UserServantEntity usrSvtData, int oldFriendShipRank, ClickDelegate callback)
    {
        this.kind = Kind.FRIENDSHIP_UP;
        this.baseUsrSvtData = usrSvtData;
        this.targetId = 0;
        this.targetLv = 0;
        this.oldFriendShipRank = oldFriendShipRank;
        this.callbackFunc = callback;
        base.gameObject.SetActive(true);
        this.fsm.SendEvent("START_FRIENDSHIPUP_EFFECT");
    }

    private void setLimitUpBaseSvtFigure()
    {
        string nodename = this.fsm.FsmVariables.GetFsmString("LimitUpBaseSvtNodeName").Value;
        Transform transform = this.effect.transform.getNodeFromName(nodename, true);
        Debug.Log("** !! ** SetCardParam cardNode: " + transform);
        UIPanel component = transform.GetComponent<UIPanel>();
        int imageLimitCount = ImageLimitCount.GetImageLimitCount(this.baseUsrSvtData.svtId, this.baseUsrSvtData.limitCount);
        this.currentSvtFigure = StandFigureManager.CreateRenderPrefab(component.gameObject, this.baseUsrSvtData.svtId, imageLimitCount, Face.Type.NORMAL, 1, null);
        this.currentSvtFigure.transform.localPosition = new Vector3(-306f, 380f, 0f);
    }

    public void SetLimitUpCombineInfo(Kind kind, UserServantEntity baseData, int baseCollectionLimitCnt, ClickDelegate callback)
    {
        this.kind = kind;
        this.baseUsrSvtData = baseData;
        this.baseUsrSvtCollectionLimitCnt = baseCollectionLimitCnt;
        this.callbackFunc = callback;
        base.gameObject.SetActive(true);
        this.fsm.SendEvent("START_LIMITUP_EFFECT");
    }

    private void setLimitUpResSvtFigure()
    {
        string nodename = this.fsm.FsmVariables.GetFsmString("LimitUpResSvtNodeName").Value;
        Transform transform = this.effect.transform.getNodeFromName(nodename, true);
        Debug.Log("** !! ** SetCardParam cardNode: " + transform);
        UIPanel component = transform.GetComponent<UIPanel>();
        ServantVoiceData[] dataArray = this.getLimitUpSvtVoiceList();
        int face = 0;
        if (dataArray != null)
        {
            face = dataArray[0].face;
        }
        int imageLimitCount = ImageLimitCount.GetImageLimitCount(this.resUsrSvtData.svtId, this.resUsrSvtData.limitCount);
        this.limitUpSvtFigure = StandFigureManager.CreateRenderPrefab(component.gameObject, this.resUsrSvtData.svtId, imageLimitCount, (Face.Type) face, 1, null);
        this.limitUpSvtFigure.transform.localPosition = new Vector3(-306f, 380f, 0f);
    }

    public void SetNobleCombineInfo(Kind kind, UserServantEntity usrSvtData, int targetId, int targetLv, ClickDelegate callback, int targetIdOld = 0, int targetLvOld = 0)
    {
        Debug.LogError("!!");
        this.kind = kind;
        this.baseUsrSvtData = usrSvtData;
        this.targetId = targetId;
        this.targetIdOld = targetIdOld;
        this.targetLv = targetLv;
        this.targetLvOld = targetLvOld;
        this.callbackFunc = callback;
        base.gameObject.SetActive(true);
        this.fsm.SendEvent("START_NOBLEUP_EFFECT");
    }

    public void SetPowerUpInfo(UserServantEntity usrSvtData, ClickDelegate callback)
    {
        this.kind = Kind.POWER_UP;
        this.baseUsrSvtData = usrSvtData;
        this.targetId = 0;
        this.targetLv = 0;
        this.callbackFunc = callback;
        base.gameObject.SetActive(true);
        this.fsm.SendEvent("START_POWERUP_EFFECT");
    }

    private void setResultSvtGraphCard()
    {
        string nodename = this.fsm.FsmVariables.GetFsmString("ResultSvtCardNodeName").Value;
        Transform transform = this.effect.transform.getNodeFromName(nodename, true);
        float x = this.fsm.FsmVariables.GetFsmFloat("CardScale").Value;
        int imageLimitCount = ImageLimitCount.GetCardImageLimitCount(this.resUsrSvtData.svtId, this.resUsrSvtData.limitCount, true, true);
        UICharaGraphTexture texture = CharaGraphManager.CreateTexturePrefab(transform.gameObject, this.resUsrSvtData, imageLimitCount, 10, null);
        texture.transform.localPosition = Vector3.zero;
        texture.transform.localScale = new Vector3(x, x, x);
    }

    public void SetSkillCombineInfo(Kind kind, UserServantEntity usrSvtData, int targetId, int targetLv, ClickDelegate callback, int targetIdOld = 0, int targetLvOld = 0)
    {
        this.kind = kind;
        this.baseUsrSvtData = usrSvtData;
        this.targetId = targetId;
        this.targetIdOld = targetIdOld;
        this.targetLv = targetLv;
        this.targetLvOld = targetLvOld;
        this.callbackFunc = callback;
        base.gameObject.SetActive(true);
        Debug.Log("!!!! CombineResultEffectComponent SetSkillCombineInfo targetId: " + targetId);
        this.fsm.SendEvent("START_SKILLUP_EFFECT");
    }

    public void setSkillResultInfo()
    {
        switch (this.kind)
        {
            case Kind.SVT_COMBINE:
            {
                this.resUsrSvtData = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_SERVANT).getEntityFromId<UserServantEntity>(this.baseUsrSvtData.id);
                if (this.svtVoiceEntity != null)
                {
                    List<ServantVoiceData[]> list = this.svtVoiceEntity.getLevelUpVoiceList();
                    this.playVoiceList = list[0];
                }
                int lv = this.baseUsrSvtData.lv;
                int num2 = this.resUsrSvtData.lv;
                if ((!lv.Equals(num2) && (this.asstName != null)) && (this.svtVoiceEntity != null))
                {
                    this.maxPlayCnt = this.playVoiceList.Length;
                    this.playVoice();
                }
                this.svtResultInfoWindow.setAfterSvtResultState(this.resUsrSvtData, this.baseUsrSvtCollictionLv, new System.Action(this.EndDisp));
                break;
            }
            case Kind.LIMITUP:
            case Kind.MATERIAL_LIMIT_UP:
                this.limitUpResultCheck.checkResultLimitUp(this.baseUsrSvtData, this.resUsrSvtData, this.baseUsrSvtCollectionLimitCnt);
                this.playVoiceList = this.getLimitUpSvtVoiceList();
                if (this.playVoiceList == null)
                {
                    this.EndDisp();
                    break;
                }
                if (this.asstName != null)
                {
                    this.maxPlayCnt = this.playVoiceList.Length;
                    this.playVoice();
                }
                break;

            case Kind.SKILL_LEVELUP:
            case Kind.SKILLOPEN:
            case Kind.SKILL_RANKUP:
                this.skillResultInfoWindow.OpenSkillUpResultInfo(this.targetId, this.targetLv, new System.Action(this.EndDisp), this.targetIdOld, this.targetLvOld, this.kind == Kind.SKILLOPEN);
                break;

            case Kind.TREASUREDVC_LEVELUP:
            case Kind.TREASUREDVCOPEN:
            case Kind.TREASUREDVC_RANKUP:
                this.skillResultInfoWindow.OpenNpUpResultInfo(this.baseUsrSvtData, this.targetId, this.targetLv, new System.Action(this.EndDisp), this.targetIdOld, this.targetLvOld, this.kind);
                break;

            case Kind.SVTEQ_COMBINE:
                this.resUsrSvtData = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_SERVANT).getEntityFromId<UserServantEntity>(this.baseUsrSvtData.id);
                this.svtResultInfoWindow.setAfterSvtResultState(this.resUsrSvtData, this.baseUsrSvtCollictionLv, new System.Action(this.EndDisp));
                break;

            case Kind.FRIENDSHIP_UP:
                this.skillResultInfoWindow.OpenFriendshipUpResultInfo(this.baseUsrSvtData, this.oldFriendShipRank, new System.Action(this.EndDisp));
                break;

            case Kind.POWER_UP:
                this.skillResultInfoWindow.OpenPowerUpResultInfo(this.baseUsrSvtData, new System.Action(this.EndDisp));
                break;
        }
    }

    public void SetSvtCombineInfo(Kind kind, int infoIdx, UserServantEntity baseData, int baseCollectionLv, ClickDelegate callback)
    {
        this.kind = kind;
        this.successInfo = infoIdx;
        this.baseUsrSvtData = baseData;
        this.baseUsrSvtCollictionLv = baseCollectionLv;
        this.callbackFunc = callback;
        base.gameObject.SetActive(true);
        this.fsm.SendEvent("START_SVTCOMBINE_EFFECT");
    }

    public void SetSvtEqCombineInfo(Kind kind, int infoIdx, UserServantEntity baseData, ClickDelegate callback)
    {
        this.kind = kind;
        this.successInfo = infoIdx;
        this.baseUsrSvtData = baseData;
        this.callbackFunc = callback;
        base.gameObject.SetActive(true);
        this.fsm.SendEvent("START_SVTCOMBINE_EFFECT");
    }

    public void stopVoice()
    {
        SingletonMonoBehaviour<ScriptManager>.Instance.closeTalk(this.Talkdate);
        this.Talkdate = null;
        if (this.player != null)
        {
            SoundManager.stopVoice(this.asstName, this.vcName, 0f);
            this.player = null;
            this.playCnt = 0;
            this.maxPlayCnt = 0;
            SingletonMonoBehaviour<SoundManager>.Instance.ReleaseAudioAssetStorage(this.asstName);
        }
    }

    public delegate void ClickDelegate(bool isDecide);

    public enum Kind
    {
        SVT_COMBINE,
        LIMITUP,
        SKILL_LEVELUP,
        SKILLOPEN,
        TREASUREDVC_LEVELUP,
        TREASUREDVCOPEN,
        SVTEQ_COMBINE,
        SKILL_RANKUP,
        TREASUREDVC_RANKUP,
        FRIENDSHIP_UP,
        MATERIAL_LIMIT_UP,
        POWER_UP
    }
}

