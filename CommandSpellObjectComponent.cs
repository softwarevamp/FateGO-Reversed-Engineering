using System;
using UnityEngine;

public class CommandSpellObjectComponent : MonoBehaviour
{
    private int Consume;
    public UISprite coverSprite;
    private int Id;
    public UILabel label_conf;
    public UILabel label_count;
    public UILabel label_name;
    public CommandSpellWindowComponent target;
    private bool touchFlg;

    public void onClickUse()
    {
        if (this.touchFlg)
        {
            SoundManager.playSe("ba18");
            this.target.UseSpell(this.Id);
        }
    }

    public void setData(CommandSpellWindowComponent.MODE mode, int Id, int count)
    {
        this.Id = Id;
        CommandSpellEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<CommandSpellMaster>(DataNameKind.Kind.COMMAND_SPELL).getEntityFromId<CommandSpellEntity>(Id);
        this.label_name.text = entity.name;
        this.label_conf.text = entity.detail;
        this.label_count.text = string.Empty + entity.consume;
        this.Consume = entity.consume;
        this.updateIsUse(mode, count);
    }

    public void setUseButton(bool flg)
    {
        this.touchFlg = flg;
    }

    public void updateIsUse(CommandSpellWindowComponent.MODE mode, int count)
    {
        CommandSpellEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<CommandSpellMaster>(DataNameKind.Kind.COMMAND_SPELL).getEntityFromId<CommandSpellEntity>(this.Id);
        if ((mode == CommandSpellWindowComponent.MODE.BATTLE) && (entity.type == 2))
        {
            count = 0;
        }
        if (this.Consume <= count)
        {
            this.coverSprite.enabled = false;
            this.setUseButton(true);
        }
        else
        {
            this.coverSprite.enabled = true;
            this.setUseButton(false);
        }
    }
}

