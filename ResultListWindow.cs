using System;
using System.Collections.Generic;
using UnityEngine;

public class ResultListWindow : BaseDialog
{
    private string aniName = "ef_boxitem_fall";
    private int[] baseNoList;
    private System.Action closeCallBack;
    private int currentBaseId;
    private int currentEventId;
    private GameObject dispObj;
    private List<GameObject> effectObjList;
    [SerializeField]
    protected UIGrid firstLineGrid;
    [SerializeField]
    protected UIGrid firstResultGrid;
    private bool isRare;
    [SerializeField]
    protected float itemDispTime = 0.001f;
    [SerializeField]
    protected float itemDispWaitTime = 0.07f;
    [SerializeField]
    protected float itemDropTime = 0.025f;
    private int maxPlayCnt;
    private System.Action openCallBack;
    private int playCnt;
    private int[] rareIdxList;
    private int[] resultIdList;
    private List<GameObject> resultItemObjList;
    [SerializeField]
    protected GameObject resultListItemPrefab;
    [SerializeField]
    protected UIGrid sceLineGrid;
    [SerializeField]
    protected UIGrid sceResultGrid;
    private State state;
    [SerializeField]
    protected UIGrid thrLineGrid;
    [SerializeField]
    protected UIGrid thrResultGrid;

    private bool checkRare(int idx)
    {
        if ((this.rareIdxList != null) && (this.rareIdxList.Length > 0))
        {
            for (int i = 0; i < this.rareIdxList.Length; i++)
            {
                int num2 = this.rareIdxList[i];
                if (num2.Equals(idx))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void clearGrid()
    {
        int childCount = this.firstResultGrid.transform.childCount;
        if (childCount > 0)
        {
            for (int i = childCount - 1; i >= 0; i--)
            {
                UnityEngine.Object.DestroyObject(this.firstResultGrid.transform.GetChild(i).gameObject);
            }
        }
        int num3 = this.sceResultGrid.transform.childCount;
        if (num3 > 0)
        {
            for (int j = num3 - 1; j >= 0; j--)
            {
                UnityEngine.Object.DestroyObject(this.sceResultGrid.transform.GetChild(j).gameObject);
            }
        }
        int num5 = this.thrResultGrid.transform.childCount;
        if (num5 > 0)
        {
            for (int k = num5 - 1; k >= 0; k--)
            {
                UnityEngine.Object.DestroyObject(this.thrResultGrid.transform.GetChild(k).gameObject);
            }
        }
    }

    public void Close()
    {
        this.Close(new System.Action(this.EndClose));
    }

    public void Close(System.Action callback)
    {
        this.closeCallBack = callback;
        base.Close(new System.Action(this.EndClose));
    }

    protected void EndClose()
    {
        System.Action closeCallBack = this.closeCallBack;
        this.closeCallBack = null;
        closeCallBack();
        this.Init();
        this.clearGrid();
        base.gameObject.SetActive(false);
    }

    private void endDisp()
    {
        if (this.playCnt < this.maxPlayCnt)
        {
            base.Invoke("setDisp", 1E-05f);
        }
        else
        {
            this.playCnt = 0;
            base.Invoke("EndOpen", 1f);
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

    public void Init()
    {
        base.gameObject.SetActive(false);
        this.maxPlayCnt = 0;
        this.playCnt = 0;
        this.isRare = false;
        this.state = State.INIT;
        base.Init();
    }

    public void OpenResultList(int[] resultIds, int[] rareIdxs, int[] baseNos, int baseId, int eventId, System.Action callback)
    {
        if (this.state == State.INIT)
        {
            base.gameObject.SetActive(true);
            this.setDispResultListBg(true);
            this.effectObjList = new List<GameObject>();
            this.openCallBack = callback;
            this.resultIdList = resultIds;
            this.rareIdxList = rareIdxs;
            this.baseNoList = baseNos;
            this.currentBaseId = baseId;
            this.currentEventId = eventId;
            this.maxPlayCnt = resultIds.Length;
            this.setResultItem();
            base.Open(null, true);
        }
    }

    private void playEffectDrop()
    {
        this.playCnt++;
        string name = !this.isRare ? "ef_boxitem_drop" : "ef_boxitem_drop_rare";
        BoxGachaResultEffectComponent.getEffect(name, this.dispObj.GetParent().transform).transform.localPosition = this.dispObj.transform.localPosition;
        base.Invoke("showResultItem", this.itemDispWaitTime);
    }

    private void setDisp()
    {
        this.dispObj = this.resultItemObjList[this.playCnt];
        this.isRare = this.checkRare(this.playCnt);
        GameObject obj2 = BoxGachaResultEffectComponent.getEffect("ef_boxitem_fall", this.dispObj.GetParent().transform);
        obj2.transform.localPosition = this.dispObj.transform.localPosition;
        obj2.GetComponentInChildren<Animation>().Play(this.aniName);
        base.Invoke("playEffectDrop", this.itemDropTime);
    }

    private void setDispResultListBg(bool isDisp)
    {
        this.firstLineGrid.gameObject.SetActive(isDisp);
        this.sceLineGrid.gameObject.SetActive(isDisp);
        this.thrLineGrid.gameObject.SetActive(isDisp);
    }

    private void setResultItem()
    {
        this.resultItemObjList = new List<GameObject>();
        int num = this.firstResultGrid.maxPerLine - 1;
        int num2 = num + this.sceResultGrid.maxPerLine;
        GiftMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<GiftMaster>(DataNameKind.Kind.GIFT);
        for (int i = 0; i < this.resultIdList.Length; i++)
        {
            UIGrid firstResultGrid;
            int id = this.resultIdList[i];
            if (i <= num)
            {
                firstResultGrid = this.firstResultGrid;
            }
            else if (i <= num2)
            {
                firstResultGrid = this.sceResultGrid;
            }
            else
            {
                firstResultGrid = this.thrResultGrid;
            }
            BoxGachaBaseEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.BOX_GACHA_BASE).getEntityFromId<BoxGachaBaseEntity>(this.currentBaseId, this.baseNoList[i]);
            GameObject item = base.createObject(this.resultListItemPrefab, firstResultGrid.transform, null);
            item.transform.localScale = Vector3.one;
            ResultItemComponent component = item.GetComponent<ResultItemComponent>();
            if (entity != null)
            {
                switch (entity.type)
                {
                    case 1:
                    {
                        GiftEntity entity2 = master.getDataById(id);
                        if (entity2 != null)
                        {
                            component.Set((Gift.Type) entity2.type, entity2.objectId, entity2.num);
                            item.SetActive(false);
                            this.resultItemObjList.Add(item);
                        }
                        break;
                    }
                    case 2:
                    case 3:
                    {
                        EventRewardSetEntity entity3 = entity.getRewardSetData(this.currentEventId);
                        if (entity3 != null)
                        {
                            component.SetExtra(entity3.iconId);
                            item.SetActive(false);
                            this.resultItemObjList.Add(item);
                        }
                        break;
                    }
                }
            }
        }
        this.firstResultGrid.repositionNow = true;
        this.sceResultGrid.repositionNow = true;
        this.thrResultGrid.repositionNow = true;
        this.showResultEffect();
    }

    private void showResultEffect()
    {
        base.Invoke("setDisp", 0.3f);
    }

    private void showResultItem()
    {
        this.dispObj.SetActive(true);
        base.Invoke("endDisp", this.itemDispTime);
    }

    private enum State
    {
        INIT
    }
}

