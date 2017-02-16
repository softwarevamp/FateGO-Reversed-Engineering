using System;
using UnityEngine;

public class TitleInfoEventItemComponent : MonoBehaviour
{
    private static readonly string[] DispStrSpNameTbl = new string[] { "img_event_getitem", "img_event_getpoint", "img_event_mission" };
    public static readonly float EVENT_ITEM_SCALE = 0.75f;
    public static readonly int GROUP_DISP_OBJ_MAX = 4;
    private EventItemComponent[] mEventItemComponents;
    [SerializeField]
    private GameObject mEventItemPrefab;
    [SerializeField]
    private GameObject[] mItemObjParents;
    [SerializeField]
    private UISprite mStrSp;

    public void Destroy()
    {
        if (this.mEventItemComponents != null)
        {
            int length = this.mEventItemComponents.Length;
            for (int i = 0; i < length; i++)
            {
                if (this.mEventItemComponents[i] != null)
                {
                    UnityEngine.Object.Destroy(this.mEventItemComponents[i].gameObject);
                }
                this.mEventItemComponents[i] = null;
            }
            this.mEventItemComponents = null;
        }
    }

    public void OnDestroy()
    {
        this.Destroy();
    }

    public void Setup(int[] item_ids, DispType disp_type)
    {
        this.Destroy();
        this.mEventItemComponents = new EventItemComponent[this.mItemObjParents.Length];
        int length = this.mItemObjParents.Length;
        int num2 = item_ids.Length - 1;
        if (num2 >= length)
        {
            num2 = length - 1;
        }
        int index = 0;
        for (int i = num2; i >= 0; i--)
        {
            GameObject self = UnityEngine.Object.Instantiate<GameObject>(this.mEventItemPrefab);
            self.SafeSetParent(this.mItemObjParents[index]);
            self.SetLocalScale(EVENT_ITEM_SCALE);
            EventItemComponent component = self.GetComponent<EventItemComponent>();
            switch (disp_type)
            {
                case DispType.ITEM:
                    component.Set(item_ids[i]);
                    break;

                case DispType.POINT:
                    component.SetPointEvent(item_ids[i]);
                    break;

                case DispType.MISSION:
                    component.SetMissionEvent(item_ids[i]);
                    break;
            }
            this.mEventItemComponents[i] = component;
            index++;
        }
        this.mStrSp.gameObject.SetActive(index <= 2);
        this.mStrSp.spriteName = DispStrSpNameTbl[(int) disp_type];
        this.mStrSp.MakePixelPerfect();
    }

    public void UpdateDisp()
    {
        if (this.mEventItemComponents != null)
        {
            foreach (EventItemComponent component in this.mEventItemComponents)
            {
                if (component != null)
                {
                    component.UpdateDisp();
                }
            }
        }
    }

    public enum DispType
    {
        ITEM,
        POINT,
        MISSION
    }
}

