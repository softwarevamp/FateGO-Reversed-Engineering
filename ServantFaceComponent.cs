using System;

public class ServantFaceComponent : BaseMonoBehaviour
{
    public UISprite facesprite;

    public void setFace(int svtId, int limitCount)
    {
        Debug.Log("!!!!!! ServantFaceComponent svtId : " + svtId);
        AtlasManager.SetFace(this.facesprite, svtId, limitCount);
    }

    public void setTargetSprite(UISprite sprite)
    {
        this.facesprite = sprite;
    }
}

