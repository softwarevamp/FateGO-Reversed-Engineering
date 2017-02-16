using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleViewBufflistComponent : BaseMonoBehaviour
{
    public GameObject barObject;
    public float height = 120f;
    public Transform listRoot;
    public UILabel nothingLabel;
    private List<GameObject> objList = new List<GameObject>();
    public GameObject prefabBuffObject;
    public UIScrollView uiScrollView;

    public void setBuffList(BattleBuffData.BuffData[] buffList)
    {
        this.prefabBuffObject.SetActive(false);
        foreach (GameObject obj2 in this.objList)
        {
            UnityEngine.Object.Destroy(obj2);
        }
        this.objList.Clear();
        if (buffList != null)
        {
            for (int i = 0; i < buffList.Length; i++)
            {
                BattleBuffData.BuffData buffData = buffList[i];
                Vector3 vector = new Vector3(0f, -this.height * i);
                GameObject item = base.createObject(this.prefabBuffObject, this.listRoot, null);
                item.transform.localPosition = vector;
                if (item != null)
                {
                    item.GetComponent<BattleBuffListObjectComponent>().setData(buffData);
                }
                item.SetActive(true);
                this.objList.Add(item);
            }
            if (this.nothingLabel != null)
            {
                string key = "BATTLE_NOBUFFLIST";
                string str2 = LocalizationManager.Get(key);
                if (!str2.Equals(key))
                {
                    this.nothingLabel.text = str2;
                }
                this.nothingLabel.gameObject.SetActive(buffList.Length <= 0);
            }
        }
    }

    public void setHide()
    {
        this.barObject.SetActive(false);
        this.listRoot.gameObject.SetActive(false);
    }

    public void setShow()
    {
        this.listRoot.gameObject.SetActive(true);
        this.uiScrollView.ResetPosition();
        this.barObject.SetActive(true);
    }
}

