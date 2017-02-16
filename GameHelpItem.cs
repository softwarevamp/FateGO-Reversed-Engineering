using System;
using System.Collections.Generic;
using UnityEngine;

public class GameHelpItem
{
    public Dictionary<int, GameHelpItem> childDic;
    public GameObject gameObj;
    public int index;
    public bool isMainLabel;
    public Vector3 localVector;
    public string mDetial;
    public string mLabelName;
    public int mNum;
    public string mTitleName;
    public int mType;
    public GameObject textgameObj;

    public GameHelpItem(int type, int num, string titleName, string labelName, string detial)
    {
        this.mType = type;
        this.mNum = num;
        this.mTitleName = titleName;
        this.mLabelName = labelName;
        this.mDetial = detial;
    }
}

