using System;
using UnityEngine;

public class SetSelectSvtInfoComponent : MonoBehaviour
{
    public ServantFaceIconComponent svtFaceInfo;

    public void setSvtInfo(long usrSvtId)
    {
        this.svtFaceInfo.gameObject.SetActive(true);
        this.svtFaceInfo.Set(usrSvtId, null);
    }
}

