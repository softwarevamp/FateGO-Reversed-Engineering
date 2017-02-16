using System;
using System.Collections.Generic;
using UnityEngine;

public class BoxGachaResultEffectComponent : BaseMonoBehaviour
{
    private Face.Type aftFaceType;
    private string aftMsg;
    private Face.Type befFaceType;
    private string befMsg;
    private System.Action closeCallBack;
    private int currentBaseId;
    private int currentEventId;
    private int currentSvtId;
    private string currentSvtName;
    private static AssetData effectAssetData;
    [SerializeField]
    protected CommonMessageManager messageManager;
    [SerializeField, Range(0.1f, 1f)]
    private float openResultWindowTime = 0.1f;
    private int[] rareIdxList;
    private int[] resultIdList;
    private List<GameObject> resultItemObjList;
    private int[] resultNoList;
    [SerializeField]
    protected ResultListWindow resultWindow;
    private UIStandFigureR standFigure;
    [SerializeField]
    protected PlayMakerFSM targetFSM;

    public void clearResultList(System.Action callBack)
    {
        this.closeCallBack = callBack;
        this.resultWindow.Close(this.closeCallBack);
    }

    public void dispMsgAftResultList()
    {
        this.messageManager.Init();
        this.setFigureFace(this.aftFaceType);
        this.messageManager.SetMessageBlock(this.aftMsg, new System.Action(this.EndMessage));
    }

    public void dispMsgBefResultList()
    {
        this.messageManager.Init();
        this.setFigureFace(this.befFaceType);
        this.messageManager.SetMessageBlock(this.befMsg, new System.Action(this.EndMessage));
    }

    private void endloadEffect(AssetData data)
    {
        if (data != null)
        {
            effectAssetData = data;
            SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, delegate {
                base.gameObject.SetActive(true);
                this.dispMsgBefResultList();
            });
        }
    }

    private void EndMessage()
    {
        this.setFigureFace(Face.Type.NORMAL);
        this.messageManager.Quit();
        this.targetFSM.SendEvent("END_MSG");
    }

    public static GameObject getEffect(string name, Transform parentTr)
    {
        GameObject obj2 = UnityEngine.Object.Instantiate<GameObject>(effectAssetData.GetObject<GameObject>(name));
        obj2.transform.parent = parentTr;
        obj2.transform.localPosition = Vector3.zero;
        obj2.transform.localScale = Vector3.one;
        return obj2;
    }

    public void init(int svtId, BoxGachaTalkEntity talkEnt, int[] resultIds, int[] rareIdxs, int[] baseNos, UIStandFigureR currentFigure, int baseId, int eventId)
    {
        this.currentSvtId = svtId;
        this.resultIdList = resultIds;
        this.rareIdxList = rareIdxs;
        this.resultNoList = baseNos;
        this.standFigure = currentFigure;
        this.currentBaseId = baseId;
        this.currentEventId = eventId;
        ServantEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.currentSvtId);
        this.currentSvtName = entity.name;
        string str = "＠" + this.currentSvtName + "\n";
        string str2 = talkEnt.beforeDetail + "[k]";
        string str3 = talkEnt.afterDetail + "[k]";
        this.befMsg = !talkEnt.beforeDetail.StartsWith(LocalizationManager.Get("SCRIPT_ACTION_CODE_TALK")) ? (str + str2) : str2;
        this.aftMsg = !talkEnt.afterDetail.StartsWith(LocalizationManager.Get("SCRIPT_ACTION_CODE_TALK")) ? (str + str3) : str3;
        this.befFaceType = (Face.Type) talkEnt.beforeFace;
        this.aftFaceType = (Face.Type) talkEnt.afterFace;
        this.loadBoxGachaEffect();
    }

    private void loadBoxGachaEffect()
    {
        string name = "Effect/BoxGacha";
        AssetManager.loadAssetStorage(name, new AssetLoader.LoadEndDataHandler(this.endloadEffect));
    }

    private void openResultCallback()
    {
        this.targetFSM.SendEvent("END_DISP");
    }

    private void openResultListWindow()
    {
        this.resultWindow.OpenResultList(this.resultIdList, this.rareIdxList, this.resultNoList, this.currentBaseId, this.currentEventId, new System.Action(this.openResultCallback));
    }

    private void setFigureFace(Face.Type type)
    {
        if (this.standFigure != null)
        {
            this.standFigure.SetFace(type);
        }
    }

    private void showResultList()
    {
        GameObject obj2 = getEffect("ef_boxitem_start", base.gameObject.transform);
        obj2.transform.localPosition = new Vector3(200f, 70f, 0f);
        EffectComponent component = obj2.GetComponent<EffectComponent>();
        base.Invoke("openResultListWindow", this.openResultWindowTime);
    }
}

