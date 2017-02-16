using System;
using System.Collections.Generic;
using UnityEngine;

public class EventBannerControl : BaseMonoBehaviour
{
    public UIGrid bannerGrid;
    private List<int> bannerIdList = new List<int>();
    private List<GameObject> bannerObjList = new List<GameObject>();
    public GameObject bannerPf;
    public CombineInitData combineData;

    public void ClearBanner()
    {
        base.gameObject.SetActive(false);
        if (this.bannerObjList.Count > 0)
        {
            for (int i = 0; i < this.bannerObjList.Count; i++)
            {
                UnityEngine.Object.Destroy(this.bannerObjList[i]);
            }
            this.bannerObjList.Clear();
        }
    }

    public void setBannerList()
    {
        EventEntity[] entityArray = this.combineData.getCombineEventList();
        if (entityArray != null)
        {
            base.gameObject.SetActive(true);
            for (int i = 0; i < entityArray.Length; i++)
            {
                EventEntity eventData = entityArray[i];
                Debug.Log("**!! Banner Id : " + eventData.bannerId);
                if (eventData.bannerId > 0)
                {
                    this.bannerIdList.Add(eventData.bannerId);
                    GameObject item = base.createObject(this.bannerPf, this.bannerGrid.transform, null);
                    item.transform.localScale = Vector3.one;
                    item.GetComponent<BannerComponent>().SetBanner(eventData);
                    this.bannerObjList.Add(item);
                }
            }
            this.bannerGrid.Reposition();
        }
        else
        {
            base.gameObject.SetActive(false);
        }
    }
}

