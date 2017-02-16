using System;
using UnityEngine;

public class SetNoticeNumControl : BaseMonoBehaviour
{
    public GameObject noticeNumberPrefab;
    private GameObject noticeNumObj;

    private void clear()
    {
        if (this.noticeNumObj != null)
        {
            UnityEngine.Object.Destroy(this.noticeNumObj);
            this.noticeNumObj = null;
        }
    }

    public void setNoticeNum(int resNum)
    {
        this.clear();
        if (resNum > 0)
        {
            this.noticeNumObj = base.createObject(this.noticeNumberPrefab, base.transform, null);
            this.noticeNumObj.transform.localPosition = Vector3.zero;
            this.noticeNumObj.GetComponent<NoticeNumberComponent>().SetNumber(resNum);
            base.gameObject.SetActive(true);
        }
        else
        {
            base.gameObject.SetActive(false);
        }
    }
}

