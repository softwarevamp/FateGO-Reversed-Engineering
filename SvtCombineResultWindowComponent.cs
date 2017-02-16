using System;
using System.Collections.Generic;
using UnityEngine;

public class SvtCombineResultWindowComponent : BaseDialog
{
    private int baseAtkVal;
    private int baseHpVal;
    private int baseLimitCnt;
    private int baseLvMax;
    private int baseSvtCollectionLv;
    private ServantEntity baseSvtData;
    private UserServantEntity baseUsrSvtData;
    [SerializeField]
    protected CheckCombineResStatus checkResInfo;
    protected static readonly Color COLOR_VAL = new Color(0.99f, 0.945f, 0.316f);
    [SerializeField]
    protected UILabel currentAtkLb;
    [SerializeField]
    protected UISlider currentExpBar;
    [SerializeField]
    protected UILabel currentExpValLb;
    [SerializeField]
    protected UILabel currentHpLb;
    [SerializeField]
    protected UIGrid currentLimitCntGrid;
    [SerializeField]
    protected UIExtrusionLabel currentLvLb;
    [SerializeField]
    protected UILabel currentMaxLvLb;
    [SerializeField]
    protected GameObject currentStatusInfo;
    private bool endDispLvInfoFlg;
    [SerializeField]
    protected GameObject heroQuestInfo;
    [SerializeField]
    protected UILabel heroQuestInfoDetail;
    [SerializeField]
    protected UILabel heroQuestInfoTitle;
    [SerializeField]
    protected UILabel infoLb;
    private static string INIT_VAL_TXT = "0";
    private bool isGetNewSkill;
    private bool isOpenQuest;
    [SerializeField]
    protected GameObject limitCntInfoObject;
    [SerializeField]
    protected GameObject lvInfo;
    protected System.Action openCallBack;
    [SerializeField]
    protected OpenInfoWindowComponent openInfowindowComp;
    private int PrevLevel;
    [SerializeField]
    protected UILabel resAtkLb;
    private int resAtkVal;
    private UIWidget resAtkWidget;
    [SerializeField]
    protected UISlider resCrExpBar;
    [SerializeField]
    protected UISlider resExpBar;
    [SerializeField]
    protected UILabel resHpLb;
    private int resHpVal;
    private UIWidget resHpWidget;
    private List<GameObject> resInfoList;
    private int resLimitCnt;
    [SerializeField]
    protected UIGrid resLimitCntGrid;
    private List<GameObject> resLimitCntList;
    [SerializeField]
    protected UIExtrusionLabel resLvLb;
    private int resLvMax;
    private UIWidget resLvWidget;
    [SerializeField]
    protected UILabel resMaxLvLb;
    private UIWidget resMaxLvWidget;
    [SerializeField]
    protected GameObject resStatusInfo;
    private UserServantEntity resUsrSvtData;
    [SerializeField]
    protected GameObject skillGetInfo;
    [SerializeField]
    protected UILabel skillGetInfoDetail;
    [SerializeField]
    protected UILabel skillGetInfoTitle;
    [SerializeField]
    protected Collider skipCollider;
    [SerializeField]
    protected GameObject storyQuestInfo;
    [SerializeField]
    protected UILabel storyQuestInfoDetail;
    [SerializeField]
    protected UILabel storyQuestInfoTitle;
    private ServantEntity svtEntity;

    private void checkGetSkill()
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
            this.skillGetInfoTitle.text = LocalizationManager.Get("GET_SKILL_TITLE");
            this.skillGetInfoDetail.text = string.Format(LocalizationManager.Get("GET_SKILL_NAME"), entity.name);
            this.resInfoList.Add(this.skillGetInfo);
            this.isGetNewSkill = true;
        }
    }

    private void checkQuestOpen()
    {
        this.isOpenQuest = false;
        SingletonTemplate<clsQuestCheck>.Instance.mfInit();
        List<int> releaseQuestIdByServantLv = new List<int>();
        releaseQuestIdByServantLv = SingletonTemplate<clsQuestCheck>.Instance.GetReleaseQuestIdByServantLv(this.resUsrSvtData.svtId, this.baseSvtCollectionLv);
        QuestEntity entity = null;
        this.storyQuestInfoTitle.text = LocalizationManager.Get("OPEN_STORY_QUEST_TITLE");
        this.heroQuestInfoTitle.text = LocalizationManager.Get("OPEN_HERO_QUEST_TITLE");
        if ((releaseQuestIdByServantLv != null) && (releaseQuestIdByServantLv.Count > 0))
        {
            for (int i = 0; i < releaseQuestIdByServantLv.Count; i++)
            {
                if (releaseQuestIdByServantLv[i] > 0)
                {
                    entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.QUEST).getEntityFromId<QuestEntity>(releaseQuestIdByServantLv[i]);
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

    public void ClickSkip()
    {
        if (this.skipCollider.enabled)
        {
            this.skipExp(new System.Action(this.EndDisp));
        }
    }

    public void Close()
    {
        this.Close(new System.Action(this.EndClose));
    }

    public void Close(System.Action callback)
    {
        base.Close(new System.Action(this.EndClose));
    }

    private void destroyCurrentStatusGrid()
    {
        int childCount = this.currentLimitCntGrid.transform.childCount;
        if (childCount > 0)
        {
            for (int i = childCount - 1; i >= 0; i--)
            {
                UnityEngine.Object.DestroyObject(this.currentLimitCntGrid.transform.GetChild(i).gameObject);
            }
        }
    }

    private void destroyResStatusGrid()
    {
        int childCount = this.resLimitCntGrid.transform.childCount;
        if (childCount > 0)
        {
            for (int i = childCount - 1; i >= 0; i--)
            {
                UnityEngine.Object.DestroyObject(this.resLimitCntGrid.transform.GetChild(i).gameObject);
            }
        }
    }

    public void enabledLvInfo()
    {
        this.lvInfo.SetActive(this.endDispLvInfoFlg);
    }

    protected void EndClose()
    {
        base.Init();
        base.gameObject.SetActive(false);
        this.destroyCurrentStatusGrid();
        this.destroyResStatusGrid();
    }

    private void EndDisp()
    {
        this.skipCollider.enabled = false;
        if (this.isGetNewSkill || this.isOpenQuest)
        {
            this.openInfowindowComp.OpenResultInfo(this.resInfoList, new System.Action(this.EndOpen));
        }
        else
        {
            this.EndOpen();
        }
    }

    protected void EndOpen()
    {
        if (this.openCallBack != null)
        {
            System.Action openCallBack = this.openCallBack;
            this.openCallBack = null;
            openCallBack();
        }
    }

    public void InitStateInfo()
    {
        this.currentLvLb.text = INIT_VAL_TXT;
        this.currentMaxLvLb.text = INIT_VAL_TXT;
        this.currentExpBar.value = 0f;
        this.currentExpValLb.text = INIT_VAL_TXT;
        this.currentHpLb.text = INIT_VAL_TXT;
        this.currentAtkLb.text = INIT_VAL_TXT;
        this.resLvLb.text = INIT_VAL_TXT;
        this.resMaxLvLb.text = INIT_VAL_TXT;
        this.resCrExpBar.value = 0f;
        this.resHpLb.text = INIT_VAL_TXT;
        this.resAtkLb.text = INIT_VAL_TXT;
    }

    public void setAfterSvtResultState(UserServantEntity resData, int svtCollectionLv, System.Action callback)
    {
        this.resUsrSvtData = resData;
        this.baseSvtCollectionLv = svtCollectionLv;
        Debug.Log("******!! setAfterSvtResultState resUsrSvtData : " + this.resUsrSvtData.exp);
        this.resLvMax = this.resUsrSvtData.getLevelMax();
        int childCount = this.resLimitCntGrid.transform.childCount;
        this.resLimitCntList = new List<GameObject>();
        if (childCount > 0)
        {
            for (int i = 0; i < childCount; i++)
            {
                GameObject gameObject = this.resLimitCntGrid.transform.GetChild(i).gameObject;
                this.resLimitCntList.Add(gameObject);
            }
        }
        this.openCallBack = callback;
        this.resInfoList = new List<GameObject>();
        this.isGetNewSkill = false;
        this.isOpenQuest = false;
        if (SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.resUsrSvtData.svtId).IsServant)
        {
            this.checkQuestOpen();
            this.checkGetSkill();
        }
        this.showGetExp();
    }

    public void setBefResultState(UserServantEntity baseData)
    {
        int num;
        int num2;
        float num3;
        this.skipCollider.enabled = false;
        this.baseUsrSvtData = baseData;
        base.gameObject.SetActive(true);
        this.baseLvMax = this.baseUsrSvtData.getLevelMax();
        this.currentLvLb.text = this.baseUsrSvtData.lv.ToString();
        this.currentMaxLvLb.text = this.baseLvMax.ToString();
        this.resLvLb.text = this.baseUsrSvtData.lv.ToString();
        this.resMaxLvLb.text = this.baseLvMax.ToString();
        this.resLvWidget = this.resLvLb.GetComponent<UIWidget>();
        this.resMaxLvWidget = this.resMaxLvLb.GetComponent<UIWidget>();
        this.resHpWidget = this.resHpLb.GetComponent<UIWidget>();
        this.resAtkWidget = this.resAtkLb.GetComponent<UIWidget>();
        this.resLvWidget.color = Color.white;
        this.resMaxLvWidget.color = Color.white;
        this.resHpWidget.color = Color.white;
        this.resAtkWidget.color = Color.white;
        bool flag = this.baseUsrSvtData.getExpInfo(out num, out num2, out num3);
        this.currentExpBar.value = num3;
        this.currentExpValLb.text = num2.ToString();
        this.resCrExpBar.value = num3;
        this.resExpBar.gameObject.SetActive(false);
        int maxLimitCnt = this.baseUsrSvtData.getLimitCntMax();
        this.baseLimitCnt = this.baseUsrSvtData.limitCount;
        this.baseHpVal = this.baseUsrSvtData.hp;
        this.baseAtkVal = this.baseUsrSvtData.atk;
        this.setLimitCntInfo(maxLimitCnt, this.baseLimitCnt, this.currentLimitCntGrid.transform);
        this.currentLimitCntGrid.repositionNow = true;
        this.setLimitCntInfo(maxLimitCnt, this.baseLimitCnt, this.resLimitCntGrid.transform);
        this.resLimitCntGrid.repositionNow = true;
        this.currentHpLb.text = this.baseUsrSvtData.hp.ToString();
        this.resHpLb.text = this.baseUsrSvtData.hp.ToString();
        this.currentAtkLb.text = this.baseUsrSvtData.atk.ToString();
        this.resAtkLb.text = this.baseUsrSvtData.atk.ToString();
        base.Open(new System.Action(this.EndOpen), true);
    }

    private void setLimitCntInfo(int maxLimitCnt, int svtLimitCnt, Transform root)
    {
        if (maxLimitCnt > 0)
        {
            for (int i = 0; i < maxLimitCnt; i++)
            {
                GameObject obj2 = base.createObject(this.limitCntInfoObject, root, null);
                obj2.transform.localPosition = Vector3.zero;
                SetLimitCntInfoComponent component = obj2.GetComponent<SetLimitCntInfoComponent>();
                component.setEnableOnImg(true);
                if (i > (svtLimitCnt - 1))
                {
                    component.setEnableOnImg(false);
                }
            }
        }
    }

    private int setSvtExp(int getExp, int nowExp, int startLv)
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
        if (startLv == num)
        {
            this.resCrExpBar.value = 1f;
            return num2;
        }
        this.resCrExpBar.value = 1f - (((float) (entity.exp - nowExp)) / ((float) (entity.exp - exp)));
        return num2;
    }

    private void showGetExp()
    {
        this.baseSvtData = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.baseUsrSvtData.svtId);
        this.resLimitCnt = this.resUsrSvtData.limitCount;
        this.resHpVal = this.resUsrSvtData.hp;
        this.resAtkVal = this.resUsrSvtData.atk;
        this.setSvtExp(this.resUsrSvtData.exp - this.baseUsrSvtData.exp, this.baseUsrSvtData.exp, this.baseUsrSvtData.lv);
        this.PrevLevel = this.baseUsrSvtData.lv;
        object[] args = new object[] { "from", 0f, "to", 1f, "onupdate", "UpdateValue", "oncomplete", "EndDisp", "time", 2f };
        iTween.ValueTo(base.gameObject, iTween.Hash(args));
        if (this.baseLimitCnt != this.resLimitCnt)
        {
            this.resMaxLvLb.text = this.resLvMax.ToString();
            this.resMaxLvWidget.color = COLOR_VAL;
            this.infoLb.text = string.Format(LocalizationManager.Get("MAX_LVUP_TXT"), this.baseLvMax, this.resLvMax);
            this.infoLb.gameObject.SetActive(true);
            this.infoLb.GetComponent<TweenPosition>().Play();
            for (int i = 0; i < this.resLimitCntList.Count; i++)
            {
                SetLimitCntInfoComponent component = this.resLimitCntList[i].GetComponent<SetLimitCntInfoComponent>();
                component.setEnableOnImg(true);
                if (i > (this.resLimitCnt - 1))
                {
                    component.setEnableOnImg(false);
                }
            }
        }
        if (this.baseUsrSvtData.lv == this.resUsrSvtData.lv)
        {
            if (this.baseHpVal != this.resHpVal)
            {
                this.resHpLb.text = this.resHpVal.ToString();
                this.resHpWidget.color = COLOR_VAL;
            }
            if (this.baseAtkVal != this.resAtkVal)
            {
                this.resAtkLb.text = this.resAtkVal.ToString();
                this.resAtkWidget.color = COLOR_VAL;
            }
        }
    }

    private void skipExp(System.Action callback)
    {
        iTween component = base.gameObject.GetComponent<iTween>();
        if (component != null)
        {
            iTween.Stop(base.gameObject);
            UnityEngine.Object.DestroyImmediate(component);
        }
        this.UpdateValue(1f);
        callback.Call();
    }

    public void UpdateValue(float val)
    {
        int num = this.resUsrSvtData.getLevelMax();
        this.skipCollider.enabled = true;
        int nowExp = Mathf.FloorToInt(Mathf.Lerp((float) this.baseUsrSvtData.exp, (float) this.resUsrSvtData.exp, val));
        int increLv = this.setSvtExp(this.resUsrSvtData.exp - this.baseUsrSvtData.exp, nowExp, this.baseUsrSvtData.lv);
        if (this.baseUsrSvtData.lv != num)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.METER);
            if (increLv != this.PrevLevel)
            {
                int num4;
                int num5;
                this.lvInfo.SetActive(true);
                TweenPosition component = this.lvInfo.GetComponent<TweenPosition>();
                component.ResetToBeginning();
                component.Play();
                this.PrevLevel = increLv;
                this.endDispLvInfoFlg = false;
                this.resLvLb.text = increLv.ToString();
                this.resLvWidget.color = COLOR_VAL;
                this.checkResInfo.getCombineResStatus(out num4, out num5, this.resUsrSvtData, increLv);
                if (this.baseHpVal != this.resHpVal)
                {
                    this.resHpLb.text = num4.ToString();
                    this.resHpWidget.color = COLOR_VAL;
                }
                if (this.baseAtkVal != this.resAtkVal)
                {
                    this.resAtkLb.text = num5.ToString();
                    this.resAtkWidget.color = COLOR_VAL;
                }
            }
            else
            {
                this.endDispLvInfoFlg = true;
            }
        }
    }

    protected enum QUESTTYPE
    {
        FRIENDSHIP = 3,
        HEROBALLAD = 6
    }
}

