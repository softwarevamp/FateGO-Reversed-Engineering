using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ScrTerminalMap : MonoBehaviour
{
    private AssetData mapAssetData;
    private UIAtlas mcAssetsAtlasP;
    private Texture2D mcAssetsTex2dP;
    [SerializeField]
    private GameObject mDispRoot;
    [SerializeField]
    private EarthCore mEarthCore;
    private float mfMap2dOffsetX;
    private float mfMap2dOffsetY;
    private SrcSpotBasePrefab mFocusSpot;
    private int miCurCapIndex = -1;
    private int miCurTexH;
    private int miCurTexID;
    private int miCurTexW;
    public MapCamera mMapCamera;
    private List<MapEffectComponent> mMapEffectList = new List<MapEffectComponent>();
    private List<GameObject> mMapEffPrefabList = new List<GameObject>();
    private GameObject mMapGimmickPrefab;
    private GameObject mPlayerIcon;
    public static readonly string msAssetsNameAtlas = "QMap_Cap{0:D4}_Atlas";
    public static readonly string msAssetsNameBG = "img_questmap_{0:D4}";
    public static readonly string msAssetsNameEffect = "MapEffect_{0:D4}_{1:D2}";
    public static readonly string msAssetsNameGimmick = "MapGimmick_{0:D4}";
    public static readonly string msAssetsNamePack = "Terminal/QuestMap/Capter{0:D4}";
    public static readonly string msAssetsNameSpotRoad = "img_road{0:D4}_{1:D2}";
    [SerializeField]
    private GameObject mServantRoot;
    private System.Action mSpotClickAct;
    [SerializeField]
    private SpotLargeComponent mSpotLargeComponent;
    [SerializeField]
    private GameObject mSpotMaskObj;
    private GameObject mSpotTchObj;
    private PlayMakerFSM mTargetFsm;
    [SerializeField]
    private TerminalSceneComponent mTerminalScene;
    public TitleInfoControl mTitleInfo;
    [SerializeField]
    private GameObject mWorldBg;
    [SerializeField]
    private Camera mWorldCamera;
    [SerializeField]
    private GameObject pfbBaseP;
    [SerializeField]
    private GameObject pfbLineP;
    [SerializeField]
    private GameObject pfbPlayerP;
    [SerializeField]
    private GameObject rootEffectP;
    [SerializeField]
    private GameObject rootGimmickP;
    [SerializeField]
    private GameObject rootRoadP;
    [SerializeField]
    private GameObject rootSpotP;
    public const string SAVE_KRY_PLAYER_ICON = "TerminalPlayerIcon";
    private const float SPOT_MASK_FADE_TIME = 0.25f;

    private void Awake()
    {
        this.mSpotMaskObj.SetActive(false);
    }

    private void CreateMapEff()
    {
        this.DestroyMapEff();
        foreach (GameObject obj2 in this.mMapEffPrefabList)
        {
            MapEffectComponent item = UnityEngine.Object.Instantiate<GameObject>(obj2).GetComponent<MapEffectComponent>();
            if (item != null)
            {
                item.Setup(this.rootEffectP, this.mMapCamera);
                this.mMapEffectList.Add(item);
            }
        }
    }

    private void CreateMapGimmick(clsMapCtrl_MapGimmickInfo mgi)
    {
        if (this.mMapGimmickPrefab != null)
        {
            MapGimmickEntity entity = mgi.mfGetMine();
            GameObject self = (GameObject) UnityEngine.Object.Instantiate(this.mMapGimmickPrefab, Vector3.zero, Quaternion.identity);
            self.name = MapGimmickComponent.GetGobjName(entity.id);
            self.SafeSetParent(this.rootGimmickP);
            self.SetLocalPosition(new Vector3(entity.x - this.mfMap2dOffsetX, (this.miCurTexH - entity.y) - this.mfMap2dOffsetY, 0f));
            self.GetComponent<MapGimmickComponent>().Setup(mgi, this.mcAssetsAtlasP);
        }
    }

    private void DestroyMapEff()
    {
        foreach (MapEffectComponent component in this.mMapEffectList)
        {
            GameObject gameObject = component.gameObject;
            if (gameObject != null)
            {
                UnityEngine.Object.Destroy(gameObject);
            }
        }
        this.mMapEffectList.Clear();
    }

    public AssetData GetAssetData() => 
        this.mapAssetData;

    public UIAtlas GetAtlas() => 
        this.mcAssetsAtlasP;

    public int GetCurTexH() => 
        this.miCurTexH;

    public int GetCurTexW() => 
        this.miCurTexW;

    public SrcSpotBasePrefab GetFocusSpot()
    {
        SrcSpotBasePrefab lastDispSpot = SingletonMonoBehaviour<QuestAfterAction>.Instance.GetLastDispSpot();
        return ((lastDispSpot == null) ? this.mFocusSpot : lastDispSpot);
    }

    private string GetSaveKey_PlayerIcon() => 
        ("TerminalPlayerIcon" + this.miCurCapIndex);

    public GameObject GetSpotMaskObj() => 
        this.mSpotMaskObj;

    private void InitTitleInfo()
    {
        this.mTitleInfo.setHeaderBgImg(false);
        this.mTitleInfo.setTitleImg(TitleInfoControl.TitleKind.TERMINAL, true);
    }

    public void mcbfCheckSceneStatus()
    {
        if ((TerminalPramsManager.mfGetSceneStatus() != TerminalPramsManager.enSceneStatus.enResume) || (TerminalPramsManager.SpotId == SpotEntity.CALDEAGATE_ID))
        {
            this.mfCallFsmEvent("EV_SCENE_STATUS_INIT");
        }
        else
        {
            this.mfCallFsmEvent("EV_SCENE_STATUS_RESUME");
        }
    }

    private void mcbfMap2d_LoadFinish_Pack(AssetData asdata)
    {
        this.mapAssetData = asdata;
        string name = string.Format(msAssetsNameBG, this.miCurTexID);
        this.mcAssetsTex2dP = asdata.GetObject<Texture2D>(name);
        this.mMapCamera.SetMapTexture(this.mcAssetsTex2dP, this.miCurTexW, this.miCurTexH);
        string str2 = string.Format(msAssetsNameAtlas, this.miCurTexID);
        this.mcAssetsAtlasP = asdata.GetObject<GameObject>(str2).GetComponent<UIAtlas>();
        this.mMapEffPrefabList.Clear();
        int num = 1;
        while (true)
        {
            string str3 = string.Format(msAssetsNameEffect, this.miCurTexID, num);
            GameObject item = asdata.GetObject<GameObject>(str3);
            if (item == null)
            {
                break;
            }
            this.mMapEffPrefabList.Add(item);
            num++;
        }
        this.CreateMapEff();
        string str4 = string.Format(msAssetsNameGimmick, this.miCurTexID);
        this.mMapGimmickPrefab = asdata.GetObject<GameObject>(str4);
        this.mfCallFsmEvent("evTexLoad_Finish");
    }

    private void mcbfMapTouchDisable()
    {
        if (TerminalPramsManager.IsAuto())
        {
            TerminalPramsManager.AutoOff();
            this.mTerminalScene.Fadein_MapDisp(SceneManager.DEFAULT_FADE_TIME, null);
        }
    }

    private void mcbfMapTouchEnable()
    {
        if (TutorialFlag.Get(TutorialFlag.Id.TUTORIAL_LABEL_END))
        {
            MainMenuBar.SetMenuBtnColliderEnable(true);
        }
        if (TerminalPramsManager.IsAuto())
        {
            if (TerminalPramsManager.SpotId >= 0)
            {
                string gobjName = SrcSpotBasePrefab.GetGobjName(TerminalPramsManager.SpotId);
                Transform transform = this.rootSpotP.transform.FindChild(gobjName);
                if ((transform != null) && transform.gameObject.GetComponent<BoxCollider>().enabled)
                {
                    this.smfSpotBtn_Click(transform.gameObject);
                    return;
                }
            }
            TerminalPramsManager.AutoOff();
            this.mTerminalScene.Fadein_MapDisp(SceneManager.DEFAULT_FADE_TIME, () => SingletonMonoBehaviour<QuestAfterAction>.Instance.Play(() => this.mTerminalScene.PlayTutorial()));
        }
        else
        {
            TerminalPramsManager.SpotId = -1;
        }
    }

    private void mcbfSpotMaskEnd()
    {
        if (!this.mSpotMaskObj.activeSelf)
        {
            this.SpotMaskEnd_FadeEnd();
        }
        else
        {
            this.mSpotMaskObj.SafeGetComponent<TweenAlpha>();
            TweenAlpha.Begin(this.mSpotMaskObj, 0.25f, 0f);
            this.mSpotLargeComponent.LargeOut(() => this.SpotMaskEnd_FadeEnd());
        }
    }

    private void mcbfSpotMaskStart()
    {
        if (this.mSpotMaskObj.activeSelf)
        {
            this.SpotMaskStart_FadeEnd();
        }
        else
        {
            this.mSpotMaskObj.SetActive(true);
            this.mSpotMaskObj.SafeGetComponent<TweenAlpha>();
            TweenAlpha.Begin(this.mSpotMaskObj, 0.25f, 1f);
            this.mSpotLargeComponent.LargeIn(this.mSpotTchObj.GetComponent<SrcSpotBasePrefab>(), this.mMapCamera, () => this.SpotMaskStart_FadeEnd());
        }
    }

    private void mcbfWhiteIn_ToMap()
    {
        if (TerminalPramsManager.IsAuto())
        {
            this.mfCallFsmEvent("GO_NEXT");
        }
        else
        {
            this.mTerminalScene.Fadein_MapDisp(SceneManager.DEFAULT_FADE_TIME, () => this.mfCallFsmEvent("GO_NEXT"));
        }
    }

    private void mcbfWhiteOut_ToMap()
    {
        int warId = TerminalPramsManager.WarId;
        this.mTitleInfo.setHeaderBgImg(true);
        int headerImageId = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<WarMaster>(DataNameKind.Kind.WAR).getEntityFromId<WarEntity>(warId).headerImageId;
        this.mTitleInfo.setTitleImg((TitleInfoControl.TitleKind) headerImageId, true);
        this.mTerminalScene.PlayChapterStart(() => this.mcbfWhiteOut_ToMap_End());
    }

    private void mcbfWhiteOut_ToMap_End()
    {
        this.mfCallFsmEvent("GO_NEXT");
    }

    private void mcbfWhiteOut_ToWorld()
    {
        SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.WHITE, SceneManager.DEFAULT_FADE_TIME, delegate {
            this.InitTitleInfo();
            this.mfCallFsmEvent("GO_NEXT");
        });
    }

    public void mfCallFsmEvent(string sEventStr)
    {
        PlayMakerFSM rfsm = this.mfGetMyFsmP();
        if (null != rfsm)
        {
            rfsm.Fsm.Event(sEventStr);
        }
    }

    private int mfGetFsmValueInt(string sValueStr)
    {
        int num = 0;
        PlayMakerFSM rfsm = this.mfGetMyFsmP();
        if (null != rfsm)
        {
            num = rfsm.Fsm.Variables.GetFsmInt(sValueStr).Value;
        }
        return num;
    }

    public PlayMakerFSM mfGetMyFsmP()
    {
        if (null == this.mTargetFsm)
        {
            this.mTargetFsm = base.GetComponent<PlayMakerFSM>();
        }
        return this.mTargetFsm;
    }

    private void mfMap2d_Create_LineBySpotID(clsMapCtrl_SpotRoadInfo rinf)
    {
        SpotRoadEntity entity = rinf.mfGetMine();
        int id = entity.getSrcSpotId();
        int num2 = entity.getDstSpotId();
        SpotEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<SpotMaster>(DataNameKind.Kind.SPOT).getEntityFromId<SpotEntity>(id);
        SpotEntity entity3 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<SpotMaster>(DataNameKind.Kind.SPOT).getEntityFromId<SpotEntity>(num2);
        GameObject obj2 = null;
        obj2 = (GameObject) UnityEngine.Object.Instantiate(this.pfbLineP, Vector3.zero, Quaternion.identity);
        obj2.name = srcLineSprite.GetGobjName(entity.id);
        obj2.transform.parent = this.rootRoadP.transform;
        obj2.transform.localScale = Vector3.one;
        srcLineSprite component = null;
        component = obj2.GetComponent<srcLineSprite>();
        if (null != component)
        {
            component.SetMapCtrl_SpotRoadInfo(rinf);
            float fSrcW = (rinf.mfGetDispType() != clsMapCtrl_SpotRoadInfo.enDispType.None) ? 1f : 0f;
            component.mfSetITweenSize(fSrcW, fSrcW, 0f);
            component.mfSetPos2(new Vector2(entity2.x - this.mfMap2dOffsetX, (this.miCurTexH - entity2.y) - this.mfMap2dOffsetY), new Vector2(entity3.x - this.mfMap2dOffsetX, (this.miCurTexH - entity3.y) - this.mfMap2dOffsetY));
            string sSpriteName = string.Format(msAssetsNameSpotRoad, this.miCurTexID, entity.getImageId());
            component.mfSetAtlas(this.mcAssetsAtlasP, sSpriteName);
            component.SetContrast((rinf.mfGetDispType() != clsMapCtrl_SpotRoadInfo.enDispType.Glay) ? 1f : 0.5f);
        }
    }

    private SrcSpotBasePrefab mfMap2d_Create_SpotBySpotInfo(clsMapCtrl_SpotInfo cSpotInfoP)
    {
        int iQuestCount = 0;
        SpotEntity entity = null;
        int iSpotID = 0;
        iQuestCount = cSpotInfoP.mfGetQuestcount();
        entity = cSpotInfoP.mfGetMine();
        iSpotID = cSpotInfoP.mfGetSpotID();
        GameObject obj2 = null;
        obj2 = (GameObject) UnityEngine.Object.Instantiate(this.pfbBaseP, Vector3.zero, Quaternion.identity);
        obj2.name = SrcSpotBasePrefab.GetGobjName(entity.id);
        obj2.transform.parent = this.rootSpotP.transform;
        obj2.transform.localScale = (cSpotInfoP.mfGetDispType() != clsMapCtrl_SpotInfo.enDispType.None) ? Vector3.one : Vector3.zero;
        obj2.transform.localPosition = new Vector3(entity.x - this.mfMap2dOffsetX, ((this.miCurTexH - entity.y) - this.mfMap2dOffsetY) + -32f, 0f);
        SrcSpotBasePrefab component = null;
        component = obj2.GetComponent<SrcSpotBasePrefab>();
        if (null != component)
        {
            component.mfSetCommopn(base.gameObject);
            component.mfSetSpotID(iSpotID);
            component.mfSetQuestCount(iQuestCount);
            component.mfSetAtlas(this.mcAssetsAtlasP);
            component.mfSetSpotName(entity.name, entity.nameOfsX, -entity.nameOfsY);
            component.SetMapCtrl_SpotInfo(cSpotInfoP);
            component.SetContrast((cSpotInfoP.mfGetDispType() != clsMapCtrl_SpotInfo.enDispType.Glay) ? 1f : 0.5f);
            component.SetTouchType();
            component.SetBtnColliderEnable(!this.mTerminalScene.IsTutorialActive);
        }
        return component;
    }

    private void mfSetFsmValueBool(string sValueStr, bool tValueBool)
    {
        PlayMakerFSM rfsm = this.mfGetMyFsmP();
        if (null != rfsm)
        {
            rfsm.Fsm.Variables.GetFsmBool(sValueStr).Value = tValueBool;
        }
    }

    private void mfSetFsmValueInt(string sValueStr, int iValueInt)
    {
        PlayMakerFSM rfsm = this.mfGetMyFsmP();
        if (null != rfsm)
        {
            rfsm.Fsm.Variables.GetFsmInt(sValueStr).Value = iValueInt;
        }
    }

    private void mfSetFsmValueVector3(string sValueStr, Vector3 vValueV3)
    {
        PlayMakerFSM rfsm = this.mfGetMyFsmP();
        if (null != rfsm)
        {
            rfsm.Fsm.Variables.GetFsmVector3(sValueStr).Value = vValueV3;
        }
    }

    private void OnDestroy()
    {
        this.ReleaseMap();
    }

    public void ReleaseMap()
    {
        if (this.mapAssetData != null)
        {
            AssetManager.releaseAsset(this.mapAssetData);
            this.mapAssetData = null;
        }
    }

    public void SetDisp(bool is_disp)
    {
        this.mDispRoot.SetActive(is_disp);
        this.mMapCamera.SetEnable(is_disp);
        this.mWorldCamera.gameObject.SetActive(!is_disp);
        this.mEarthCore.SetDisp(!is_disp);
        this.mServantRoot.SetActive(!is_disp);
        this.mWorldBg.SetActive(!is_disp);
    }

    public void SetMapCamera_FocusSpot(SrcSpotBasePrefab spot, float time = 0, System.Action end_act = null)
    {
        Vector3 localPosition = spot.gameObject.GetLocalPosition();
        localPosition.y += spot.mcSpotSprite.height / 2;
        this.mMapCamera.Zoom.SetZoomSize(1f, true);
        this.mMapCamera.Scrl.StartAutoMove(localPosition, time, end_act);
    }

    private void SetPlayerIcon(GameObject spot_obj)
    {
        if (this.mPlayerIcon == null)
        {
            this.mPlayerIcon = UnityEngine.Object.Instantiate<GameObject>(this.pfbPlayerP);
        }
        this.mPlayerIcon.transform.parent = spot_obj.transform;
        this.mPlayerIcon.transform.localPosition = new Vector3(0f, 72f, 0f);
        this.mPlayerIcon.transform.localRotation = Quaternion.identity;
        this.mPlayerIcon.transform.localScale = new Vector3(0.8f, 0.8f, 1f);
        this.mPlayerIcon.name = "Spot_Player";
    }

    public void SetSpotClickAct(System.Action act)
    {
        this.mSpotClickAct = act;
    }

    private void smfMap2d_LoadStart()
    {
        this.ReleaseMap();
        this.miCurCapIndex = TerminalPramsManager.WarId;
        WarEntity entity = null;
        if (this.mQuestTree != null)
        {
            entity = this.mQuestTree.mfGetWarEntityByWarID(this.miCurCapIndex);
        }
        this.miCurTexID = entity.getWarId();
        this.miCurTexW = entity.mapImageW;
        this.miCurTexH = entity.mapImageH;
        this.mfMap2dOffsetX = ((float) this.miCurTexW) / 2f;
        this.mfMap2dOffsetY = ((float) this.miCurTexH) / 2f;
        AssetManager.loadAssetStorage(string.Format(msAssetsNamePack, this.miCurTexID), new AssetLoader.LoadEndDataHandler(this.mcbfMap2d_LoadFinish_Pack));
    }

    private void smfMap2d_SpotMarkCreate()
    {
        this.miCurCapIndex = TerminalPramsManager.WarId;
        clsMapCtrl_WarInfo info = null;
        if (this.mQuestTree != null)
        {
            info = this.mQuestTree.mfGetWarInfoByWarID(this.miCurCapIndex);
        }
        int @int = 0;
        string key = this.GetSaveKey_PlayerIcon();
        if (PlayerPrefs.HasKey(key))
        {
            @int = PlayerPrefs.GetInt(key);
        }
        for (int i = 0; i < this.rootSpotP.transform.childCount; i++)
        {
            Transform child = this.rootSpotP.transform.GetChild(i);
            if (child != null)
            {
                UnityEngine.Object.Destroy(child.gameObject);
            }
        }
        SrcSpotBasePrefab prefab = null;
        SrcSpotBasePrefab prefab2 = null;
        SrcSpotBasePrefab prefab3 = null;
        foreach (clsMapCtrl_SpotInfo info2 in info.mfGetSpotListsP())
        {
            SrcSpotBasePrefab prefab4 = this.mfMap2d_Create_SpotBySpotInfo(info2);
            if (info2.mfGetDispType() == clsMapCtrl_SpotInfo.enDispType.Normal)
            {
                if ((info2.mfGetSpotID() == @int) && (info2.mfGetDispType() == clsMapCtrl_SpotInfo.enDispType.Normal))
                {
                    prefab = prefab4;
                }
                if (info2.IsNextDisp())
                {
                    prefab2 = prefab4;
                }
                prefab3 = prefab4;
            }
        }
        for (int j = 0; j < this.rootRoadP.transform.childCount; j++)
        {
            Transform transform2 = this.rootRoadP.transform.GetChild(j);
            if (transform2 != null)
            {
                UnityEngine.Object.Destroy(transform2.gameObject);
            }
        }
        foreach (clsMapCtrl_SpotRoadInfo info3 in info.mfGetSpotRoadListsP())
        {
            this.mfMap2d_Create_LineBySpotID(info3);
        }
        for (int k = 0; k < this.rootGimmickP.transform.childCount; k++)
        {
            Transform transform3 = this.rootGimmickP.transform.GetChild(k);
            if (transform3 != null)
            {
                UnityEngine.Object.Destroy(transform3.gameObject);
            }
        }
        foreach (clsMapCtrl_MapGimmickInfo info4 in info.mfGetMapGimmickListsP())
        {
            this.CreateMapGimmick(info4);
        }
        if (prefab == null)
        {
            prefab = prefab2;
        }
        if (prefab == null)
        {
            prefab = prefab3;
        }
        this.SetPlayerIcon(prefab.gameObject);
        this.mFocusSpot = prefab2;
        if (TerminalPramsManager.IsAuto() && (TerminalPramsManager.SpotId >= 0))
        {
            string gobjName = SrcSpotBasePrefab.GetGobjName(TerminalPramsManager.SpotId);
            Transform transform4 = this.rootSpotP.transform.FindChild(gobjName);
            if (transform4 != null)
            {
                this.mFocusSpot = transform4.gameObject.GetComponent<SrcSpotBasePrefab>();
            }
        }
        if (this.mFocusSpot == null)
        {
            this.mFocusSpot = prefab;
        }
        if (this.mFocusSpot == null)
        {
            this.mFocusSpot = prefab3;
        }
        this.SetMapCamera_FocusSpot(this.mFocusSpot, 0f, null);
        this.mfCallFsmEvent("evMap2D_Mark_Create_Finish");
    }

    public void smfSpot_SetPos(int iSpotID)
    {
        this.mfCallFsmEvent("evSpotSet_Finish");
    }

    public void smfSpotBtn_Click(GameObject cSpotObjP)
    {
        this.mSpotTchObj = cSpotObjP;
        SrcSpotBasePrefab component = null;
        component = cSpotObjP.GetComponent<SrcSpotBasePrefab>();
        TerminalPramsManager.SpotId = component.miSpotID;
        this.SetPlayerIcon(cSpotObjP);
        PlayerPrefs.SetInt(this.GetSaveKey_PlayerIcon(), component.miSpotID);
        this.mTitleInfo.setBackBtnSprite(true);
        this.mSpotClickAct.Call();
        this.mSpotClickAct = null;
        this.mfCallFsmEvent("evSpotSelect_GoQuestSelect");
    }

    private void SpotMaskEnd_FadeEnd()
    {
        this.mSpotMaskObj.SetActive(false);
        this.mfCallFsmEvent("SEND_MES_END");
    }

    private void SpotMaskStart_FadeEnd()
    {
        this.mfCallFsmEvent("SEND_MES_END");
    }

    private void Start()
    {
        this.mTargetFsm = base.GetComponent<PlayMakerFSM>();
        this.InitTitleInfo();
    }

    private QuestTree mQuestTree =>
        SingletonTemplate<QuestTree>.Instance;
}

