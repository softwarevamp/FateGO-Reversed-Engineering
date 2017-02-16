using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleServantShowBuffComponent : BaseMonoBehaviour
{
    public Vector2 addPos;
    private const float buff_changeTime = 10f;
    private float buff_time;
    public GameObject IconClone;
    private bool isUpdate;
    public Transform listTr;
    public int maxCount;
    public int maxLine = 1;
    private List<GameObject> objList = new List<GameObject>();
    private int wk_buffindex;
    private int[] wk_bufflist = new int[0];

    public void setBuffList(BattleBuffData.BuffData[] buffList)
    {
        int[] numArray = new int[buffList.Length];
        for (int i = 0; i < numArray.Length; i++)
        {
            numArray[i] = buffList[i].buffId;
        }
        this.wk_bufflist = numArray;
        for (int j = 0; j < this.wk_bufflist.Length; j++)
        {
            if (this.maxCount <= j)
            {
                break;
            }
            GameObject item = null;
            if (this.objList.Count <= j)
            {
                Debug.Log(" i" + j);
                item = base.createObject(this.IconClone, this.listTr, null);
                item.transform.localPosition = new Vector3(this.addPos.x * j, 0f);
                item.SetActive(true);
                this.objList.Add(item);
            }
            this.objList[j].GetComponent<BattleServantBuffIconComponent>().setIcon(this.wk_bufflist[j]);
        }
        for (int k = this.wk_bufflist.Length; k < this.objList.Count; k++)
        {
            this.objList[k].GetComponent<BattleServantBuffIconComponent>().setIcon(-1);
        }
    }

    public void Update()
    {
        if (this.isUpdate && (this.wk_bufflist.Length >= 2))
        {
            this.buff_time += Time.deltaTime;
            if (10f < this.buff_time)
            {
                this.buff_time = 0f;
                this.wk_buffindex = ((this.wk_buffindex + 1) >= this.wk_bufflist.Length) ? 0 : (this.wk_buffindex + 1);
            }
        }
    }
}

