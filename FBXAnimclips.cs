using System;
using UnityEngine;

public class FBXAnimclips
{
    public static readonly float animFps = 30f;

    public void inAttachAnimationEvents(GameObject gameObject, TextAsset data, int level)
    {
    }

    public void loadAnimationEvents(int svtId, int level)
    {
        Debug.Log("svtId:" + svtId);
        ServantMaster master = (ServantMaster) SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT);
        GameObject gameObject = Resources.Load<GameObject>("Servants/" + svtId + "/chr");
        TextAsset data = Resources.Load("Servants/" + svtId + "/fbxevent", typeof(TextAsset)) as TextAsset;
        this.inAttachAnimationEvents(gameObject, data, 1);
    }
}

