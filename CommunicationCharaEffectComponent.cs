using System;
using UnityEngine;

public class CommunicationCharaEffectComponent : CommonEffectComponent
{
    protected CommunicationCharaEffectParam communicationCharaParam;
    protected UIStandFigureM figure;
    [SerializeField]
    protected GameObject figureBase;
    protected bool isLoad;
    protected float noiseCount;
    [SerializeField]
    protected CommonEffectComponent noiseEffect1;
    [SerializeField]
    protected CommonEffectComponent noiseEffect2;

    protected void EndDispFigure()
    {
        this.isLoad = false;
        if (this.communicationCharaParam.isStartLoop)
        {
            base.ForceLoop();
        }
        else
        {
            base.ForceStart();
        }
        this.noiseEffect1.Stop(true);
        this.noiseCount = 2f;
        if (this.communicationCharaParam.callback != null)
        {
            this.communicationCharaParam.callback();
        }
    }

    public void SetFace(Face.Type faceType)
    {
        if (this.figure != null)
        {
            this.figure.SetFace(faceType);
        }
    }

    public override void SetParam(object param)
    {
        this.communicationCharaParam = param as CommunicationCharaEffectParam;
        base.Stop(false);
        this.noiseEffect1.Stop(false);
        this.noiseEffect2.Stop(false);
        this.isLoad = true;
        this.figure = StandFigureManager.CreateMeshPrefab(this.figureBase, this.communicationCharaParam.svtId, this.communicationCharaParam.limitCount, this.communicationCharaParam.faceType, 0, new System.Action(this.EndDispFigure));
    }

    protected void Update()
    {
        if (!this.isLoad && ((base.status != CommonEffectComponent.Status.INIT) && (base.status != CommonEffectComponent.Status.DESTORY)))
        {
            base.Update();
            this.noiseCount -= RealTime.deltaTime;
            if (this.noiseCount <= 0f)
            {
                this.noiseEffect2.ForceStart();
                this.noiseCount = 2f;
            }
        }
    }
}

