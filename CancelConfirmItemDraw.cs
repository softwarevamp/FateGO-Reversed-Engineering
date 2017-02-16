using System;
using UnityEngine;

public class CancelConfirmItemDraw : MonoBehaviour
{
    [SerializeField]
    protected UISprite baseSprite;
    [SerializeField]
    protected ServantFaceIconComponent servantFaceIcon;

    public void SetItem(UserServantEntity userServantEntity, long[] equipIdList)
    {
        string str = null;
        if (userServantEntity != null)
        {
            this.servantFaceIcon.Set(userServantEntity, equipIdList, null);
        }
        else
        {
            this.servantFaceIcon.Clear();
            str = "formation_blank_small";
            if (equipIdList[0] != 0)
            {
                UserServantEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantMaster>(DataNameKind.Kind.USER_SERVANT).getEntityFromId<UserServantEntity>(equipIdList[0]);
                this.servantFaceIcon.SetEquip(entity);
            }
        }
        this.baseSprite.spriteName = str;
    }
}

