using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class CommandSpellWindowComponent : BattleWindowComponent
{
    [CompilerGenerated]
    private static Comparison<CommandSpellEntity> <>f__am$cache11;
    public GameObject btn_ng;
    public GameObject btn_ok;
    public CloseButtonCallBack callback_close;
    public UseCommandSpellCallBack callback_use;
    public UILabel checkuselabel;
    public BattleWindowComponent checkWindow;
    private int commandCount;
    private bool isOpenFlg;
    public UILabel label_count;
    public MODE mode;
    public static readonly int objheight = 120;
    private List<GameObject> objlist = new List<GameObject>();
    public GameObject prefab;
    public UILabel spellnamelabel;
    public UILabel titleLabel;
    private int tmp_Id;
    public Transform tree_root;

    public void cancelSpell()
    {
        if (this.isOpenFlg)
        {
            SoundManager.playSe("ba19");
            this.checkWindow.Close(null);
            this.isOpenFlg = false;
        }
    }

    public override void Close(BattleWindowComponent.EndCall call)
    {
        this.tree_root.gameObject.SetActive(false);
        this.checkWindow.setClose();
        base.Close(call);
    }

    public override void CompOpen()
    {
        this.tree_root.gameObject.SetActive(true);
        base.CompOpen();
    }

    public void endSpellOpened()
    {
        this.isOpenFlg = true;
    }

    public void InitializeCommandSpell(MODE inMode = 0)
    {
        this.checkWindow.setInitialPos();
        this.checkWindow.setInitData(BattleWindowComponent.ACTIONTYPE.POPUP, 0.3f, false);
        this.checkWindow.setClose();
        this.checkuselabel.text = LocalizationManager.Get("BATTLE_CHECKUSE_COMMANDSPELL");
        this.tree_root.DetachChildren();
        this.prefab.SetActive(false);
        this.mode = inMode;
        if (this.mode == MODE.NO_BATTLE)
        {
            this.commandCount = 10;
        }
        CommandSpellEntity[] array = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<CommandSpellMaster>(DataNameKind.Kind.COMMAND_SPELL).getEntitys<CommandSpellEntity>();
        if (this.objlist != null)
        {
            foreach (GameObject obj2 in this.objlist)
            {
                UnityEngine.Object.Destroy(obj2);
            }
            this.objlist.Clear();
        }
        if (<>f__am$cache11 == null)
        {
            <>f__am$cache11 = (a, b) => b.priority - a.priority;
        }
        Array.Sort<CommandSpellEntity>(array, <>f__am$cache11);
        int num = 0;
        for (int i = 0; i < array.Length; i++)
        {
            if ((array[i].type != 0) && ((inMode != MODE.BATTLE) || array[i].isUseBattle()))
            {
                GameObject item = base.createObject(this.prefab, this.tree_root, null);
                item.SetActive(true);
                item.transform.localPosition = new Vector3(0f, (float) (-objheight * num));
                num++;
                CommandSpellObjectComponent component = item.GetComponent<CommandSpellObjectComponent>();
                component.setData(this.mode, array[i].id, this.commandCount);
                component.target = this;
                if (this.mode == MODE.NO_BATTLE)
                {
                    component.setUseButton(false);
                }
                item.SetActive(true);
                this.objlist.Add(item);
            }
        }
        if (this.mode == MODE.BATTLE)
        {
            this.titleLabel.text = LocalizationManager.Get("WINDOWTITLE_COMMANDSPELL_BATTLE");
        }
        else if (this.mode == MODE.NO_BATTLE)
        {
            this.titleLabel.text = LocalizationManager.Get("WINDOWTITLE_COMMANDSPELL_NO_BATTLE");
        }
    }

    public void okSpell()
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE2);
        if (this.callback_use != null)
        {
            this.callback_use(this.tmp_Id);
        }
    }

    public void OnClick()
    {
        this.onCloseButton();
    }

    public void onCloseButton()
    {
        if (this.callback_close != null)
        {
            this.callback_close();
        }
    }

    public override void Open(BattleWindowComponent.EndCall call = null)
    {
        this.isOpenFlg = false;
        this.commandCount = UserGameMaster.getSelfUserGame().getCommandSpell();
        this.label_count.text = string.Empty + this.commandCount;
        if (this.mode == MODE.BATTLE)
        {
            foreach (GameObject obj2 in this.objlist)
            {
                obj2.GetComponent<CommandSpellObjectComponent>().updateIsUse(this.mode, this.commandCount);
            }
        }
        this.tree_root.gameObject.SetActive(false);
        base.Open(call);
    }

    public void setCallBackPushClose(CloseButtonCallBack callback)
    {
        this.callback_close = callback;
    }

    public void setCallBackUse(UseCommandSpellCallBack callback)
    {
        this.callback_use = callback;
    }

    public override void setClose()
    {
        this.checkWindow.setClose();
        base.setClose();
    }

    public void setMode(MODE mode)
    {
        this.mode = mode;
    }

    public void UseSpell(int Id)
    {
        if (base.isOpen())
        {
            this.tmp_Id = Id;
            this.spellnamelabel.text = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<CommandSpellMaster>(DataNameKind.Kind.COMMAND_SPELL).getEntityFromId<CommandSpellEntity>(Id).getName();
            this.checkWindow.Open(new BattleWindowComponent.EndCall(this.endSpellOpened));
        }
    }

    public delegate void CloseButtonCallBack();

    public enum MODE
    {
        NO_BATTLE,
        BATTLE
    }

    public delegate void UseCommandSpellCallBack(int commandSpellId);
}

