using System;
using System.Collections.Generic;
using UnityEngine;

public class EventBannerComponent : BaseMonoBehaviour
{
    [SerializeField]
    protected UIGrid bannerGrid;
    protected List<GameObject> bannerList = new List<GameObject>();
    [SerializeField]
    protected GameObject bannerPrefab;

    public void ClearBanner()
    {
        base.gameObject.SetActive(false);
        if (this.bannerList.Count > 0)
        {
            for (int i = 0; i < this.bannerList.Count; i++)
            {
                UnityEngine.Object.Destroy(this.bannerList[i]);
            }
            this.bannerList.Clear();
        }
    }

    public void SetBanner()
    {
        EventEntity[] enableEntitiyList = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EventMaster>(DataNameKind.Kind.EVENT).GetEnableEntitiyList(GameEventType.TYPE.SHOP, true);
        this.ClearBanner();
        if (enableEntitiyList != null)
        {
            base.gameObject.SetActive(true);
            for (int i = 0; i < enableEntitiyList.Length; i++)
            {
                EventEntity eventData = enableEntitiyList[i];
                if (eventData.bannerId > 0)
                {
                    GameObject item = base.createObject(this.bannerPrefab, this.bannerGrid.transform, null);
                    item.transform.localScale = Vector3.one;
                    item.GetComponent<BannerComponent>().SetBanner(eventData);
                    this.bannerList.Add(item);
                }
            }
            this.bannerGrid.Reposition();
        }
    }
}

