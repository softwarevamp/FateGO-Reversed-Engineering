using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AnnouncementPanel : MonoBehaviour
{
    [CompilerGenerated]
    private static Dictionary<string, int> <>f__switch$map1;
    private AnnouncementData[] data1;
    public string lastName = string.Empty;
    public UIGrid m_BannerGrid;
    public GameObject m_bg;
    private int m_currentPage;
    private ToggleState m_currentToggleState;
    private List<AnnouncementData> m_gengxingList = new List<AnnouncementData>();
    private List<List<AnnouncementData>> m_gengxingRealList = new List<List<AnnouncementData>>();
    private List<AnnouncementData> m_gonggaoList = new List<AnnouncementData>();
    private List<List<AnnouncementData>> m_gonggaoRealList = new List<List<AnnouncementData>>();
    public List<UIGrid> m_grid;
    private List<AnnouncementData> m_guzhangList = new List<AnnouncementData>();
    private List<List<AnnouncementData>> m_guzhangRealList = new List<List<AnnouncementData>>();
    public UIButton m_houyiyeBtn;
    public GameObject m_IndexPanel;
    public GameObject m_IndexPanel1;
    public GameObject m_Item;
    private List<GameObject> m_Items = new List<GameObject>();
    public UILabel m_message;
    public UIGrid m_MessageGrid;
    public GameObject m_MessageItem;
    private List<GameObject> m_MsgItems = new List<GameObject>();
    public UIGrid m_PanelGrid;
    public UIButton m_qianyiyeBtn;
    public GameObject m_screen;
    public GameObject m_ScrollBar;
    public GameObject m_ScrollView;
    public UISprite m_SliderBg;
    public UIWidget m_SliderWidget;
    public GameObject m_test;
    public List<UIToggle> m_toggleList;
    private List<AnnouncementData> m_weihuList = new List<AnnouncementData>();
    private List<List<AnnouncementData>> m_weihuRealList = new List<List<AnnouncementData>>();
    public GameObject m_window;
    private readonly int MsgMaxNum = 6;

    public void Add(params object[] o)
    {
        GameObject item = NGUITools.AddChild(this.m_BannerGrid.gameObject, this.m_Item);
        item.GetComponent<BaseScrollViewItem>().SetItem(o);
        this.m_BannerGrid.Reposition();
        this.m_Items.Add(item);
    }

    public void AddMsgItem(params object[] o)
    {
        GameObject item = NGUITools.AddChild(this.m_MessageGrid.gameObject, this.m_MessageItem);
        item.GetComponent<BaseScrollViewItem>().SetItem(o);
        this.m_MessageGrid.Reposition();
        this.m_MsgItems.Add(item);
    }

    private void Callback(string result)
    {
        this.Sort();
        Debug.LogError("gonggao ::::: " + PlayerPrefs.GetString("announcement"));
        Dictionary<string, object> dictionary = JsonManager.getDictionary(PlayerPrefs.GetString("announcement"));
        if (dictionary["announcement"] != null)
        {
            this.data1 = JsonManager.DeserializeArray<AnnouncementData>(dictionary["announcement"]);
            Debug.Log("huangchen :::" + this.data1[0].title);
            this.SplitMessageByType();
            this.InitIndexPanel();
        }
    }

    private void CopyData(List<AnnouncementData> baseData, List<AnnouncementData> targetData)
    {
        foreach (AnnouncementData data in baseData)
        {
            targetData.Add(data);
        }
    }

    public void DestroyItems()
    {
        if (this.m_Items.Count > 0)
        {
            foreach (GameObject obj2 in this.m_Items)
            {
                UnityEngine.Object.DestroyImmediate(obj2);
            }
        }
    }

    public void DestroyMsgItems()
    {
        if (this.m_MsgItems.Count > 0)
        {
            foreach (GameObject obj2 in this.m_MsgItems)
            {
                UnityEngine.Object.DestroyImmediate(obj2);
            }
        }
    }

    public void HouyiyeBtn()
    {
        this.m_currentPage++;
        this.UpdateMessageContent();
    }

    private void InitIndexPanel()
    {
        if (this.data1.Length > 0)
        {
            this.DestroyItems();
            List<AnnouncementData> list = new List<AnnouncementData>();
            foreach (AnnouncementData data in this.data1)
            {
                if ((data.isshow == 1) && (data.type != 6))
                {
                    list.Add(data);
                }
            }
            if (list.Count > 0)
            {
                foreach (AnnouncementData data2 in list)
                {
                    Debug.Log("1111");
                    object[] o = new object[] { data2 };
                    this.Add(o);
                }
            }
            this.m_SliderBg.SetDimensions(14 * list.Count, 14);
            this.m_SliderWidget.SetDimensions(14 * (list.Count - 1), 14);
            this.UpdateMessageContent();
        }
    }

    private void InitRulesPanel(string id)
    {
        if (!NoticeInfoComponent.messageIsLabel)
        {
            UserTermEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserTermMaster>(DataNameKind.Kind.USER_TERM).getEntityFromId(id);
            if (entity == null)
            {
                this.m_message.text = " ";
                return;
            }
            if (int.Parse(id) == 3)
            {
                this.m_message.alignment = NGUIText.Alignment.Center;
            }
            else
            {
                this.m_message.alignment = NGUIText.Alignment.Automatic;
            }
            this.m_message.text = entity.detail;
        }
        else
        {
            this.m_message.text = id;
        }
        this.m_bg.SetActive(true);
        this.m_screen.SetActive(false);
        this.m_ScrollView.SetActive(true);
        this.m_IndexPanel.SetActive(false);
        this.m_IndexPanel1.SetActive(true);
        BoxCollider component = this.m_IndexPanel1.GetComponent<BoxCollider>();
        component.size = new Vector3(1020f, (float) this.m_message.height, 0f);
        component.center = new Vector3(0f, (float) (((this.m_message.height / 2) * -1) + 300), 0f);
        this.m_grid[0].cellHeight = this.m_message.height;
        this.m_ScrollView.GetComponent<UIScrollView>().ResetPosition();
    }

    private void OnDisable()
    {
        this.DestroyItems();
        this.DestroyMsgItems();
        this.m_bg.SetActive(false);
        this.m_screen.SetActive(true);
        this.m_test.SetActive(true);
        this.m_ScrollBar.SetActive(false);
        this.m_ScrollView.SetActive(false);
    }

    private void OnEnable()
    {
        if (NoticeInfoComponent.TermID == "6")
        {
            this.m_bg.SetActive(true);
            this.m_screen.SetActive(false);
            this.m_ScrollView.SetActive(true);
            this.m_IndexPanel.SetActive(true);
            this.m_IndexPanel1.SetActive(false);
            this.Callback(string.Empty);
            this.m_ScrollView.GetComponent<UIScrollView>().ResetPosition();
        }
        else if (NoticeInfoComponent.TermID == "7")
        {
            this.m_bg.SetActive(true);
            this.m_screen.SetActive(false);
            this.m_ScrollView.SetActive(false);
            this.m_IndexPanel.SetActive(false);
            this.m_IndexPanel1.SetActive(false);
        }
        else
        {
            this.InitRulesPanel(NoticeInfoComponent.TermID);
        }
    }

    public void QianyiyeBtn()
    {
        this.m_currentPage--;
        this.UpdateMessageContent();
    }

    private void SetMsgContent(List<List<AnnouncementData>> data)
    {
        Debug.LogError(string.Concat(new object[] { "ggggg  ", data.Count, " hhhhh ", data[0].Count }));
        if (data.Count > 0)
        {
            if (data.Count <= 1)
            {
                this.m_qianyiyeBtn.gameObject.SetActive(false);
                this.m_houyiyeBtn.gameObject.SetActive(false);
            }
            else if (this.m_currentPage == 0)
            {
                this.m_qianyiyeBtn.gameObject.SetActive(false);
                this.m_houyiyeBtn.gameObject.SetActive(true);
            }
            else if (this.m_currentPage == (data.Count - 1))
            {
                this.m_qianyiyeBtn.gameObject.SetActive(true);
                this.m_houyiyeBtn.gameObject.SetActive(false);
            }
            else
            {
                this.m_qianyiyeBtn.gameObject.SetActive(true);
                this.m_houyiyeBtn.gameObject.SetActive(true);
            }
            for (int i = 0; i < data[this.m_currentPage].Count; i++)
            {
                object[] o = new object[] { data[this.m_currentPage][i] };
                this.AddMsgItem(o);
            }
            this.m_MessageGrid.Reposition();
        }
    }

    private void SetToggleState(GameObject obj)
    {
        this.m_currentPage = 0;
        if (this.lastName == obj.name)
        {
            return;
        }
        string name = obj.name;
        if (name != null)
        {
            int num;
            if (<>f__switch$map1 == null)
            {
                Dictionary<string, int> dictionary = new Dictionary<string, int>(4) {
                    { 
                        "gonggaoBtn",
                        0
                    },
                    { 
                        "weihuBtn",
                        1
                    },
                    { 
                        "gengxingBtn",
                        2
                    },
                    { 
                        "guzhangBtn",
                        3
                    }
                };
                <>f__switch$map1 = dictionary;
            }
            if (<>f__switch$map1.TryGetValue(name, out num))
            {
                switch (num)
                {
                    case 0:
                        this.m_currentToggleState = ToggleState.gonggao;
                        goto Label_00DB;

                    case 1:
                        this.m_currentToggleState = ToggleState.weihu;
                        goto Label_00DB;

                    case 2:
                        this.m_currentToggleState = ToggleState.gengxing;
                        goto Label_00DB;

                    case 3:
                        this.m_currentToggleState = ToggleState.guzhang;
                        goto Label_00DB;
                }
            }
        }
        this.m_currentToggleState = ToggleState.gonggao;
    Label_00DB:
        this.lastName = obj.name;
        this.UpdateMessageContent();
    }

    private void Sort()
    {
        foreach (UIGrid grid in this.m_grid)
        {
            grid.Reposition();
        }
    }

    private void SplitMessageByType()
    {
        this.m_gonggaoList.Clear();
        this.m_weihuList.Clear();
        this.m_gengxingList.Clear();
        this.m_guzhangList.Clear();
        if (this.data1.Length > 0)
        {
            foreach (AnnouncementData data in this.data1)
            {
                if (data.type == 0)
                {
                    this.m_gonggaoList.Add(data);
                }
                else if (data.type == 1)
                {
                    this.m_weihuList.Add(data);
                }
                else if (data.type == 2)
                {
                    this.m_gengxingList.Add(data);
                }
                else if (data.type == 3)
                {
                    this.m_guzhangList.Add(data);
                }
            }
        }
        this.m_gonggaoRealList = this.SplitMsgContent(this.m_gonggaoList);
        this.m_weihuRealList = this.SplitMsgContent(this.m_weihuList);
        this.m_gengxingRealList = this.SplitMsgContent(this.m_gengxingList);
        this.m_guzhangRealList = this.SplitMsgContent(this.m_guzhangList);
    }

    private List<List<AnnouncementData>> SplitMsgContent(List<AnnouncementData> data)
    {
        List<List<AnnouncementData>> list = new List<List<AnnouncementData>> {
            new List<AnnouncementData>()
        };
        int num = 0;
        for (int i = 0; i < data.Count; i++)
        {
            if ((i != 0) && ((i % this.MsgMaxNum) == 0))
            {
                num++;
                list.Add(new List<AnnouncementData>());
            }
            list[num].Add(data[i]);
        }
        return list;
    }

    private void Start()
    {
        foreach (UIToggle toggle in this.m_toggleList)
        {
            toggle.gameObject.AddComponent<UIEventListener>();
            UIEventListener.Get(toggle.gameObject).onClick = new UIEventListener.VoidDelegate(this.SetToggleState);
        }
    }

    private void Update()
    {
    }

    private void UpdateMessageContent()
    {
        this.DestroyMsgItems();
        switch (this.m_currentToggleState)
        {
            case ToggleState.gonggao:
                this.SetMsgContent(this.m_gonggaoRealList);
                break;

            case ToggleState.weihu:
                this.SetMsgContent(this.m_weihuRealList);
                break;

            case ToggleState.gengxing:
                this.SetMsgContent(this.m_gengxingRealList);
                break;

            case ToggleState.guzhang:
                this.SetMsgContent(this.m_guzhangRealList);
                break;
        }
    }

    private enum ToggleState
    {
        gonggao,
        weihu,
        gengxing,
        guzhang
    }
}

