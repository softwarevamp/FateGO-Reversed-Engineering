using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleNextTDgaugeComponent : BaseMonoBehaviour
{
    public GameObject gaugebackIcon;
    private GameObject[] gaugebackList;
    public Transform gaugeBackRoot;
    public Transform gaugeFrontRoot;
    public GameObject gaugeIcon;
    private GameObject[] gaugeList;
    public float width = 13f;

    public void changeGauge(int index, int type)
    {
        if ((this.gaugeList != null) && (index < this.gaugeList.Length))
        {
            if (type == 2)
            {
                this.gaugeList[index].GetComponent<UISprite>().spriteName = "icon_count_02";
            }
            else if (type == 1)
            {
                this.gaugeList[index].GetComponent<UISprite>().spriteName = "icon_count_01";
            }
            else
            {
                this.gaugeList[index].GetComponent<UISprite>().spriteName = "icon_count_bg";
            }
        }
    }

    public void setHide()
    {
        this.gaugeFrontRoot.gameObject.SetActive(false);
        this.gaugeBackRoot.gameObject.SetActive(false);
    }

    public void setInitGauge(int now, int max)
    {
        if (this.gaugeList != null)
        {
            foreach (GameObject obj2 in this.gaugeList)
            {
                UnityEngine.Object.Destroy(obj2);
            }
        }
        if (this.gaugebackList != null)
        {
            foreach (GameObject obj3 in this.gaugebackList)
            {
                UnityEngine.Object.Destroy(obj3);
            }
        }
        this.gaugeList = null;
        this.gaugebackList = null;
        if (0 < max)
        {
            this.gaugeFrontRoot.gameObject.SetActive(true);
            this.gaugeBackRoot.gameObject.SetActive(true);
        }
        List<GameObject> list = new List<GameObject>();
        List<GameObject> list2 = new List<GameObject>();
        for (int i = 0; i < max; i++)
        {
            GameObject item = base.createObject(this.gaugeIcon, this.gaugeFrontRoot, null);
            item.SetActive(true);
            item.transform.localPosition = new Vector3(this.width * i, 0f);
            list.Add(item);
            GameObject obj5 = base.createObject(this.gaugebackIcon, this.gaugeBackRoot, null);
            obj5.SetActive(true);
            obj5.transform.localPosition = new Vector3(this.width * i, 0f);
            list2.Add(obj5);
        }
        this.gaugeList = list.ToArray();
        this.gaugebackList = list2.ToArray();
        this.setValue(now);
    }

    public void setValue(int nextVal)
    {
        if ((this.gaugeList != null) && (this.gaugeList.Length > 0))
        {
            int num = this.gaugeList.Length - nextVal;
            for (int i = 0; i < this.gaugeList.Length; i++)
            {
                if (num == this.gaugeList.Length)
                {
                    this.changeGauge(i, 2);
                }
                else if (num <= i)
                {
                    this.changeGauge(i, 0);
                }
                else
                {
                    this.changeGauge(i, 1);
                }
            }
            UIWidget component = this.gaugeFrontRoot.GetComponent<UIWidget>();
            TweenAlpha alpha = this.gaugeFrontRoot.GetComponent<TweenAlpha>();
            if ((component != null) && (alpha != null))
            {
                if (nextVal == 0)
                {
                    alpha.duration = 0.3f;
                    alpha.enabled = true;
                }
                else if (nextVal == 1)
                {
                    alpha.duration = 0.7f;
                    alpha.enabled = true;
                }
                else
                {
                    component.alpha = 1f;
                    alpha.enabled = false;
                }
            }
        }
    }
}

