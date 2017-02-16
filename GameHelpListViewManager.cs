using System;
using System.Collections.Generic;
using UnityEngine;

public class GameHelpListViewManager : MonoBehaviour
{
    protected float BonderY = -72f;
    public GameObject childLabelPrefab;
    public Dictionary<int, GameHelpItem> itemDic;
    public GameObject mainLabelPrefab;
    public GameObject parentPrefab;
    protected float SpaceY;
    public GameObject textLabelPrefab;

    private void Awake()
    {
        Debug.LogError("Awake");
        this.itemDic = new Dictionary<int, GameHelpItem>();
    }

    public void CreateGameObject(GameHelpItem item)
    {
        GameObject obj2 = UnityEngine.Object.Instantiate<GameObject>(this.mainLabelPrefab);
        obj2.transform.parent = this.parentPrefab.transform;
        obj2.transform.localScale = new Vector3(1f, 1f, 1f);
        GameHelpListViewObject component = obj2.GetComponent<GameHelpListViewObject>();
        component.init(item, GameHelpListViewObject.LabelType.MAIN);
        item.gameObj = obj2;
        foreach (KeyValuePair<int, GameHelpItem> pair in item.childDic)
        {
            GameObject obj4 = UnityEngine.Object.Instantiate<GameObject>(this.childLabelPrefab);
            obj4.transform.parent = this.parentPrefab.transform;
            obj4.transform.localScale = new Vector3(1f, 1f, 1f);
            GameHelpListViewObject obj5 = obj4.GetComponent<GameHelpListViewObject>();
            obj5.init(pair.Value, GameHelpListViewObject.LabelType.CHILD);
            pair.Value.gameObj = obj4;
            component.childList.Add(obj4);
            GameObject obj6 = UnityEngine.Object.Instantiate<GameObject>(this.textLabelPrefab);
            obj6.GetComponent<GameHelpListViewObject>().init(pair.Value, GameHelpListViewObject.LabelType.TEXT);
            obj6.transform.parent = this.parentPrefab.transform;
            obj6.transform.localScale = new Vector3(1f, 1f, 1f);
            pair.Value.textgameObj = obj6;
            obj5.childList.Add(obj6);
        }
    }

    private void CreateGameObjectList()
    {
        foreach (KeyValuePair<int, GameHelpItem> pair in this.itemDic)
        {
            this.CreateGameObject(pair.Value);
        }
        this.GameHelpInit();
    }

    public void CreateList()
    {
        foreach (GameIllustrationEntity entity in SingletonMonoBehaviour<DataManager>.Instance.getMasterData<GameIllustrationMaster>(DataNameKind.Kind.GAME_ILLUSTRATION).getEntitys<GameIllustrationEntity>())
        {
            if (!this.itemDic.ContainsKey(entity.type))
            {
                GameHelpItem item = new GameHelpItem(entity.type, entity.num, entity.titleName, entity.labelName, entity.detial) {
                    childDic = new Dictionary<int, GameHelpItem>()
                };
                this.itemDic.Add(entity.type, item);
                item.childDic.Add(entity.num, item);
            }
            else
            {
                GameHelpItem item2 = new GameHelpItem(entity.type, entity.num, entity.titleName, entity.labelName, entity.detial);
                this.itemDic[entity.type].childDic.Add(entity.num, item2);
            }
        }
        this.CreateGameObjectList();
    }

    private void GameHelpInit()
    {
        foreach (KeyValuePair<int, GameHelpItem> pair in this.itemDic)
        {
            pair.Value.gameObj.SetActive(true);
            foreach (KeyValuePair<int, GameHelpItem> pair2 in pair.Value.childDic)
            {
                pair2.Value.gameObj.SetActive(false);
                if (pair2.Value.textgameObj != null)
                {
                    pair2.Value.textgameObj.SetActive(false);
                }
            }
        }
        this.parentPrefab.GetComponent<UITable>().Reposition();
    }

    public float GetPostionByItem(int mainIndex, int index, GameHelpItem item)
    {
        int num = 0;
        for (int i = 1; i < mainIndex; i++)
        {
            if (this.itemDic.ContainsKey(i))
            {
                num += this.itemDic[i].childDic.Count + 1;
            }
            else
            {
                Debug.LogError("itemDic type is error    " + i);
            }
        }
        num += 1 + (index - 1);
        return (num * this.BonderY);
    }

    private void OnEnable()
    {
        Debug.LogError("OnEnable");
    }

    private void Start()
    {
        this.CreateList();
    }
}

