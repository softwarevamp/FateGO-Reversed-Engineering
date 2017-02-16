using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SummonResultComponent : BaseMonoBehaviour
{
    public GameObject backImg;
    private List<int> befSvtList;
    private GameObject blocker;
    private Vector3 center;
    public GameObject changeSceneBtnInfo;
    public GameObject closeInfo;
    private int dispType = 1;
    public PlayMakerFSM fsm;
    public UIGrid fstGrid;
    public UIWidget mWidget;
    public UIGrid scdGrid;
    public UILabel svtEqMaxLb;
    public UILabel svtEqNumLb;
    public UILabel svtEqTitleLb;
    public UILabel svtMaxLb;
    public UILabel svtNumLb;
    public UILabel svtTitleLb;
    public GameObject targetGo;
    public GameObject touchBlocker;

    private bool checkOverlapSvt(int svtId)
    {
        int count = this.befSvtList.Count;
        if (count > 0)
        {
            for (int i = 0; i < count; i++)
            {
                int num3 = this.befSvtList[i];
                if (num3 == svtId)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void clearResultList()
    {
        int childCount = this.fstGrid.transform.childCount;
        if (childCount > 0)
        {
            for (int i = childCount - 1; i >= 0; i--)
            {
                UnityEngine.Object.DestroyObject(this.fstGrid.transform.GetChild(i).gameObject);
            }
        }
        int num3 = this.scdGrid.transform.childCount;
        if (num3 > 0)
        {
            for (int j = num3 - 1; j >= 0; j--)
            {
                UnityEngine.Object.DestroyObject(this.scdGrid.transform.GetChild(j).gameObject);
            }
        }
    }

    public void ClearTouchBlocker()
    {
        if (this.blocker != null)
        {
            UnityEngine.Object.Destroy(this.blocker);
            this.blocker = null;
        }
    }

    private void DialogCallBack(bool flg)
    {
        SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, delegate {
            SingletonMonoBehaviour<CommonUI>.Instance.CloseServantStatusDialog(new System.Action(this.EndCloseDialogCallBack));
            SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, null);
        });
    }

    private void EndCloseDialogCallBack()
    {
    }

    public void initGachaResultList(GachaInfos[] resultList, int type)
    {
        this.changeSceneBtnInfo.SetActive(false);
        this.closeInfo.SetActive(false);
        this.backImg.GetComponent<Collider>().enabled = false;
        this.dispType = type;
        if (resultList != null)
        {
            this.setCenter();
            float length = resultList.Length;
            float num2 = this.fstGrid.cellWidth * 0.5f;
            float num3 = this.fstGrid.cellHeight * 0.5f;
            float num4 = this.scdGrid.cellWidth * 0.5f;
            float num5 = this.scdGrid.cellHeight * 0.5f;
            float maxPerLine = this.fstGrid.maxPerLine;
            this.befSvtList = new List<int>();
            for (int i = 0; i < resultList.Length; i++)
            {
                GachaInfos data = resultList[i];
                UIGrid grid = (i > 5) ? this.scdGrid : this.fstGrid;
                GameObject obj2 = base.createObject(this.targetGo, grid.transform, null);
                obj2.transform.localScale = Vector3.one;
                Debug.Log(string.Concat(new object[] { "-*-* initGachaResultList Data : ", data.userSvtId, " _ ", data.isNew }));
                bool isOverlap = true;
                if (data.isNew)
                {
                    int objectId = data.objectId;
                    isOverlap = this.checkOverlapSvt(objectId);
                    if (!isOverlap)
                    {
                        this.befSvtList.Add(objectId);
                    }
                }
                obj2.GetComponent<SummonResultInfoComponent>().setResultData(data, isOverlap, new SummonResultInfoComponent.ClickDelegate(this.showResSvtDetail));
            }
            float num9 = (length <= maxPerLine) ? length : maxPerLine;
            float y = (length <= maxPerLine) ? this.center.y : num3;
            this.fstGrid.transform.localPosition = new Vector3(-(num2 * (num9 - 1f)), y, this.center.z);
            this.fstGrid.repositionNow = true;
            float num11 = (length <= maxPerLine) ? 0f : (length - maxPerLine);
            this.scdGrid.transform.localPosition = new Vector3(-(num4 * (num11 - 1f)), -num5, this.center.z);
            this.scdGrid.repositionNow = true;
            base.gameObject.SetActive(true);
        }
        this.setListByType(this.dispType);
    }

    public void onClickNext()
    {
        if (this.dispType != 1)
        {
            this.fsm.SendEvent("SHOW_TALK");
        }
    }

    private void setCenter()
    {
        Vector3[] worldCorners = this.mWidget.worldCorners;
        for (int i = 0; i < 4; i++)
        {
            Vector3 position = worldCorners[i];
            worldCorners[i] = this.mWidget.transform.InverseTransformPoint(position);
        }
        this.center = Vector3.Lerp(worldCorners[0], worldCorners[2], 0.5f);
        Debug.Log("**!! Center: " + this.center);
    }

    public void setListByType(int type)
    {
        if (type == 1)
        {
            this.closeInfo.SetActive(false);
            this.changeSceneBtnInfo.SetActive(true);
            if (!TutorialFlag.IsProgressDone(TutorialFlag.Progress._2))
            {
                this.blocker = UnityEngine.Object.Instantiate<GameObject>(this.touchBlocker);
                this.blocker.SetParent(base.gameObject.GetParent());
                this.blocker.GetComponent<MaskWithOpening>().SetOpening(new Rect(0f, -300f, 200f, 100f), 30);
                SingletonMonoBehaviour<CommonUI>.Instance.OpenTutorialArrowMark(new Vector2(100f, -200f), (float) 0f, new Rect(0f, -300f, 200f, 100f), null);
            }
        }
        else
        {
            this.changeSceneBtnInfo.SetActive(false);
            this.closeInfo.SetActive(true);
            this.backImg.GetComponent<Collider>().enabled = false;
        }
        this.dispType = type;
        this.setSvtNum();
    }

    private void setSvtNum()
    {
        int num3;
        int num4;
        UserGameEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
        int svtKeep = entity.svtKeep;
        int svtEquipKeep = entity.svtEquipKeep;
        SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantMaster>(DataNameKind.Kind.USER_SERVANT).getCount(out num3, out num4);
        this.svtTitleLb.text = LocalizationManager.Get("HAVE_SVT_NUM_TITLE");
        this.svtNumLb.text = num3.ToString();
        this.svtMaxLb.text = svtKeep.ToString();
        this.svtEqTitleLb.text = LocalizationManager.Get("HAVE_SVTEQ_NUM_TITLE");
        this.svtEqNumLb.text = num4.ToString();
        this.svtEqMaxLb.text = svtEquipKeep.ToString();
    }

    private void showResSvtDetail(long usrSvtId)
    {
        <showResSvtDetail>c__AnonStoreyA9 ya = new <showResSvtDetail>c__AnonStoreyA9 {
            usrSvtId = usrSvtId,
            <>f__this = this
        };
        SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, new System.Action(ya.<>m__190));
    }

    [CompilerGenerated]
    private sealed class <showResSvtDetail>c__AnonStoreyA9
    {
        internal SummonResultComponent <>f__this;
        internal long usrSvtId;

        internal void <>m__190()
        {
            SingletonMonoBehaviour<CommonUI>.Instance.OpenServantStatusDialog(ServantStatusDialog.Kind.SUMMON, this.usrSvtId, new ServantStatusDialog.ClickDelegate(this.<>f__this.DialogCallBack));
            SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, null);
        }
    }
}

