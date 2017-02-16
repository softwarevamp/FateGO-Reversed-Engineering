using System;
using UnityEngine;

public class ServantStatusCharaGraphListViewItemDraw : MonoBehaviour
{
    [SerializeField]
    protected UICommonButton baseButton;
    protected UICharaGraphTexture charaGraph;
    [SerializeField]
    protected GameObject charaGraphBase;

    public void SetItem(ServantStatusCharaGraphListViewItem item, DispMode mode)
    {
        if (item == null)
        {
            mode = DispMode.INVISIBLE;
        }
        if (mode != DispMode.INVISIBLE)
        {
            if (this.charaGraph == null)
            {
                if (item.MainInfo.UserServant != null)
                {
                    this.charaGraph = CharaGraphManager.CreateTexturePrefab(this.charaGraphBase, item.MainInfo.UserServant, item.ImageLimitCount, 100, null);
                }
                else if (item.MainInfo.UserServantCollection != null)
                {
                    this.charaGraph = CharaGraphManager.CreateTexturePrefab(this.charaGraphBase, item.MainInfo.UserServantCollection, item.ImageLimitCount, 100, null);
                }
                else if (item.MainInfo.ServantLeaderData != null)
                {
                    this.charaGraph = CharaGraphManager.CreateTexturePrefab(this.charaGraphBase, item.MainInfo.ServantLeaderData, item.ImageLimitCount, 100, null);
                }
                else if (item.MainInfo.EquipTargetData != null)
                {
                    this.charaGraph = CharaGraphManager.CreateTexturePrefab(this.charaGraphBase, item.MainInfo.EquipTargetData, item.ImageLimitCount, 100, null);
                }
            }
            else if (item.MainInfo.UserServant != null)
            {
                this.charaGraph.SetCharacter(item.MainInfo.UserServant, item.ImageLimitCount, null);
            }
            else if (item.MainInfo.UserServantCollection != null)
            {
                this.charaGraph.SetCharacter(item.MainInfo.UserServantCollection, item.ImageLimitCount, null);
            }
            else if (item.MainInfo.ServantLeaderData != null)
            {
                this.charaGraph.SetCharacter(item.MainInfo.ServantLeaderData, item.ImageLimitCount, null);
            }
            else if (item.MainInfo.EquipTargetData != null)
            {
                this.charaGraph.SetCharacter(item.MainInfo.EquipTargetData, item.ImageLimitCount, null);
            }
            if (this.baseButton != null)
            {
                this.baseButton.SetState(UICommonButtonColor.State.Normal, true);
            }
        }
    }

    public enum DispMode
    {
        INVISIBLE,
        INVALID,
        VALID,
        INPUT
    }
}

