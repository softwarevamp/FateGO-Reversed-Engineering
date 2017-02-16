using System;
using UnityEngine;

public class EventItemListComponent : MonoBehaviour
{
    protected int eventId;
    [SerializeField]
    protected EventItemComponent[] eventItemDrawList;

    public void Clear()
    {
        base.gameObject.SetActive(false);
        if (this.eventItemDrawList != null)
        {
            for (int i = 0; i < this.eventItemDrawList.Length; i++)
            {
                this.eventItemDrawList[i].Clear();
            }
        }
        this.eventId = 0;
    }

    public static void GoToShopEventItem(int eventId)
    {
        SceneJumpInfo data = new SceneJumpInfo("EventItem", eventId);
        data.SetReturnNowScene();
        SingletonMonoBehaviour<SceneManager>.Instance.transitionScene(SceneList.Type.Shop, SceneManager.FadeType.BLACK, data);
    }

    public void OnClickShopEventItem()
    {
        GoToShopEventItem(this.eventId);
    }

    public void Set(int eventId)
    {
        if (eventId > 0)
        {
            base.gameObject.SetActive(true);
            int[] eventItemList = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ShopMaster>(DataNameKind.Kind.SHOP).GetEventItemList(eventId);
            if (this.eventItemDrawList != null)
            {
                for (int i = 0; i < this.eventItemDrawList.Length; i++)
                {
                    if (i < eventItemList.Length)
                    {
                        this.eventItemDrawList[i].Set(eventItemList[i]);
                    }
                    else
                    {
                        this.eventItemDrawList[i].Clear();
                    }
                }
            }
            this.eventId = eventId;
        }
        else
        {
            this.Clear();
        }
    }
}

