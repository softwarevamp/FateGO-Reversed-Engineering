using System;
using UnityEngine;

public class MultiSvtInfoComponent : BaseMonoBehaviour
{
    private BoxGachaEntity currentBoxGachaEnt;
    private int currentIdx;
    private int currentMoveIdx;
    private int currentSvtId;
    private UIStandFigureR standFigure;
    [SerializeField]
    protected GameObject svtFigurePanel;

    public int getBannerIdx() => 
        this.currentIdx;

    public BoxGachaEntity getCurrentBoxGachaData() => 
        this.currentBoxGachaEnt;

    public int getGuideSvtInfo() => 
        this.currentSvtId;

    public int getMoveBannerIdx() => 
        this.currentMoveIdx;

    public UIStandFigureR getSvtFigureR() => 
        this.standFigure;

    public void setCurrentBoxGachaInfo(BoxGachaEntity boxGachaEnt, int idx, int moveIdx)
    {
        this.currentBoxGachaEnt = boxGachaEnt;
        this.currentIdx = idx;
        this.currentMoveIdx = moveIdx;
        this.currentSvtId = boxGachaEnt.guideImageId;
        int imageLimitCount = ImageLimitCount.GetImageLimitCount(this.currentSvtId, boxGachaEnt.guideLimitCount);
        this.standFigure = StandFigureManager.CreateRenderPrefab(this.svtFigurePanel, this.currentSvtId, imageLimitCount, Face.Type.NORMAL, 10, null);
    }

    public void setEnabledCollider(bool isEnable)
    {
        base.GetComponent<Collider>().enabled = isEnable;
    }
}

