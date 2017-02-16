using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SummonResultInfoComponent : MonoBehaviour
{
    protected ClickDelegate clickCallbackFunc;
    [SerializeField]
    protected UIButton iconBtn;
    private bool isNewSvt;
    private GachaInfos resultData;
    [SerializeField]
    protected ServantFaceIconComponent svtFaceInfo;

    public void OnClickSvt()
    {
        if (this.clickCallbackFunc != null)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.clickCallbackFunc(this.resultData.userSvtId);
        }
    }

    private void setNewImg(bool isNew)
    {
    }

    public void setResultData(GachaInfos data, bool isOverlap, ClickDelegate callback)
    {
        this.clickCallbackFunc = callback;
        UserServantEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_SERVANT).getEntityFromId<UserServantEntity>(data.userSvtId);
        this.isNewSvt = false;
        if (data.isNew)
        {
            if (!isOverlap)
            {
                this.isNewSvt = true;
            }
            else
            {
                this.isNewSvt = false;
            }
        }
        else
        {
            this.isNewSvt = false;
        }
        this.resultData = data;
        this.svtFaceInfo.Set(entity.svtId, entity.limitCount, -1, entity.exceedCount, null, CollectionStatus.Kind.GET, this.isNewSvt, false);
        this.iconBtn.normalSprite = this.svtFaceInfo.GetFaceSpriteName();
    }

    public delegate void ClickDelegate(long usrSvtId);
}

