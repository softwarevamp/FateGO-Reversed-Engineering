using System;
using UnityEngine;

public class BattlePerformanceContinue : BaseMonoBehaviour
{
    public GameObject bg;
    public UILabel checkGiveUpLabel;
    public UILabel checkUseBuyGem;
    public UILabel checkUseCommandSpell;
    public UILabel checkUseGem;
    public UILabel commandspell_now;
    [Space(4f)]
    public GameObject commandspellObject;
    public bool continueRetryFlg;
    private BattleData data;
    public UILabel gem_now;
    public GameObject giveUpWindow;
    public BattleViewItemlistComponent itemWindow;
    private BattleLogic logic;
    public PlayMakerFSM myFsm;
    public GameObject nostoneObject;
    private BattlePerformance perf;
    public UILabel stone_now;
    public UILabel stoneBtnLabel;
    [Space(4f)]
    public GameObject stoneObject;
    public UILabel usecheckLabel;
    public BattleWindowComponent useCheckWindow;
    public BattleWindowComponent window;

    public void callBackCommandSpell(string ret)
    {
        if (ret.Equals("ok"))
        {
            this.continueRetryFlg = false;
            BattleData.setContinueBattleFlg(0, true);
            this.myFsm.SendEvent("CONNECT_OK");
        }
        else if (ret.Equals("ng"))
        {
            this.continueRetryFlg = true;
            this.myFsm.SendEvent("CONNECT_NG");
        }
    }

    public void callBackStone(string ret)
    {
        if (ret.Equals("ok"))
        {
            this.continueRetryFlg = false;
            BattleData.setContinueBattleFlg(0, true);
            this.myFsm.SendEvent("CONNECT_OK");
        }
        else if (ret.Equals("ng"))
        {
            this.continueRetryFlg = true;
            this.myFsm.SendEvent("CONNECT_NG");
        }
    }

    public void callbaclStoneShop(StonePurchaseMenu.Result result)
    {
        SingletonMonoBehaviour<CommonUI>.Instance.CloseStonePurchaseMenu();
        this.closeNoStone();
        if ((result == StonePurchaseMenu.Result.PURCHASE) || (result == StonePurchaseMenu.Result.ERROR))
        {
            this.myFsm.SendEvent("CONNECT_OK");
        }
        else
        {
            this.myFsm.SendEvent("CONNECT_NG");
        }
    }

    public void checkCountStone()
    {
        if (UserGameMaster.getSelfUserGame().stone <= 0)
        {
            this.myFsm.SendEvent("NEXT");
        }
    }

    public void checkSpell()
    {
        int num = UserGameMaster.getSelfUserGame().getCommandSpell();
        if (3 <= num)
        {
            this.myFsm.SendEvent("OK");
        }
        else
        {
            this.myFsm.SendEvent("NG");
        }
    }

    public void checkStone()
    {
        UserGameEntity entity = UserGameMaster.getSelfUserGame();
        this.gem_now.text = $"{entity.stone:#,0}";
        if (0 < entity.stone)
        {
            this.myFsm.SendEvent("OK");
        }
        else
        {
            this.myFsm.SendEvent("NG");
        }
    }

    public void CloseGiveUp()
    {
        this.giveUpWindow.SetActive(false);
        this.myFsm.SendEvent("NEXT");
    }

    public void closeNoStone()
    {
        this.itemWindow.setHide();
        this.window.Close(new BattleWindowComponent.EndCall(this.endCloseNoStone));
    }

    public void closeSpell()
    {
        this.itemWindow.setHide();
        this.window.Close(new BattleWindowComponent.EndCall(this.endCloseSpell));
    }

    public void closeStone()
    {
        this.itemWindow.setHide();
        this.window.Close(new BattleWindowComponent.EndCall(this.endCloseStone));
    }

    public void closeUseCheck()
    {
        this.useCheckWindow.gameObject.SetActive(false);
    }

    public void connectSpell()
    {
        this.closeSpell();
        BattleData.setContinueBattleFlg(2, false);
        this.data.procPlayerContinue();
        BattleCommandSpellRequest request = NetworkManager.getRequest<BattleCommandSpellRequest>(new NetworkManager.ResultCallbackFunc(this.callBackCommandSpell));
        BattleEntity entity = this.data.getBattleEntity();
        int commandSpellId = ConstantMaster.getValue("GAME_OVER_COMMAND_SPELL_ID");
        if (this.continueRetryFlg)
        {
            request.beginRetryRequest(false);
        }
        else
        {
            request.beginRequest(entity.id, commandSpellId, true);
        }
    }

    public void connectStone()
    {
        this.closeStone();
        BattleData.setContinueBattleFlg(1, false);
        this.data.procPlayerContinue();
        BattleUseContinueRequest request = NetworkManager.getRequest<BattleUseContinueRequest>(new NetworkManager.ResultCallbackFunc(this.callBackStone));
        BattleEntity entity = this.data.getBattleEntity();
        if (this.continueRetryFlg)
        {
            request.beginRetryRequest(false);
        }
        else
        {
            request.beginRequest(entity.id);
        }
    }

    public void endCloseNoStone()
    {
        this.nostoneObject.SetActive(false);
        this.myFsm.SendEvent("END_CLOSE");
    }

    public void endCloseSpell()
    {
        this.commandspellObject.SetActive(false);
        this.myFsm.SendEvent("END_CLOSE");
    }

    public void endCloseStone()
    {
        this.stoneObject.SetActive(false);
        this.myFsm.SendEvent("END_CLOSE");
    }

    public void endDialog(bool flg)
    {
        SingletonMonoBehaviour<CommonUI>.Instance.CloseNotificationDialog();
        this.logic.sendFsmEvent("NG");
    }

    protected void endLoadCommandSPell(AssetData aData)
    {
        UserGameEntity entity = UserGameMaster.getSelfUserGame();
        int spellImageId = entity.SpellImageId;
        int num2 = entity.getCommandSpell();
        GameObject prefab = aData.GetObject<GameObject>($"ef_commandspell{spellImageId:D4}");
        Animation component = base.createObject(prefab, this.perf.popupTr, null).GetComponent<Animation>();
        string animation = $"ef_commandspell_{num2:D2}";
        component.Play(animation);
        this.perf.playMasterCommandSpellCutIn();
        this.myFsm.SendEvent("END_PROC");
    }

    public void endOpenNoStone()
    {
        this.itemWindow.setShow();
        this.myFsm.SendEvent("END_OPEN");
    }

    public void endOpenSpell()
    {
        this.itemWindow.setShow();
        this.myFsm.SendEvent("END_OPEN");
    }

    public void endOpenStone()
    {
        this.itemWindow.setShow();
        this.myFsm.SendEvent("END_OPEN");
    }

    public void endOpenUsecheck()
    {
        this.myFsm.SendEvent("END_OPEN");
    }

    public void endRetryDialog(bool flg)
    {
        this.logic.sendFsmEvent("OK");
    }

    public void Initialize(BattlePerformance inperf, BattleData indata, BattleLogic inlogic)
    {
        this.perf = inperf;
        this.data = indata;
        this.logic = inlogic;
        this.window.transform.localPosition = Vector3.zero;
        this.window.setInitialPos();
        this.window.setClose();
        this.useCheckWindow.gameObject.SetActive(false);
        this.giveUpWindow.SetActive(false);
        this.commandspellObject.SetActive(false);
        this.stoneObject.SetActive(false);
        this.nostoneObject.SetActive(false);
        this.bg.SetActive(false);
        this.checkGiveUpLabel.text = LocalizationManager.Get("BATTLE_RETIRE_CHECKSTR");
        this.checkUseCommandSpell.text = LocalizationManager.Get("BATTLE_CONTINUE_CHECK_SPELL");
        this.checkUseGem.text = LocalizationManager.Get("BATTLE_CONTINUE_CHECK_STONE");
        this.checkUseBuyGem.text = LocalizationManager.Get("BATTLE_CONTINUE_NO_STONE");
    }

    public void OpenGiveUp()
    {
        this.giveUpWindow.SetActive(true);
    }

    public void openNoStone()
    {
        this.nostoneObject.SetActive(true);
        this.window.Open(new BattleWindowComponent.EndCall(this.endOpenNoStone));
    }

    public void openReTry()
    {
        SingletonMonoBehaviour<CommonUI>.Instance.OpenNotificationDialog("连接失败", "与服务器连接中断", new NotificationDialog.ClickDelegate(this.endRetryDialog), -1);
    }

    public void openSpell()
    {
        UserGameEntity entity = UserGameMaster.getSelfUserGame();
        if (0 < entity.stone)
        {
            this.stoneBtnLabel.text = LocalizationManager.Get("BATTLE_CONTINUE_BUTTON_USESTONE");
        }
        else
        {
            this.stoneBtnLabel.text = LocalizationManager.Get("BATTLE_CONTINUE_BUTTON_BUYSTONE");
        }
        this.commandspellObject.SetActive(true);
        this.window.Open(new BattleWindowComponent.EndCall(this.endOpenSpell));
    }

    public void openStone()
    {
        this.stoneObject.SetActive(true);
        this.window.Open(new BattleWindowComponent.EndCall(this.endOpenStone));
    }

    public void openStoneShop()
    {
        this.closeNoStone();
        SingletonMonoBehaviour<CommonUI>.Instance.OpenStonePurchaseMenu(new StonePurchaseMenu.CallbackFunc(this.callbaclStoneShop), null);
    }

    public void openUseCheck(int type)
    {
        if (type == 0)
        {
            this.usecheckLabel.text = LocalizationManager.Get("BATTLE_CONTINUE_USECHECK_SPELL");
        }
        else if (type == 1)
        {
            this.usecheckLabel.text = LocalizationManager.Get("BATTLE_CONTINUE_USECHECK_STONE");
        }
        this.useCheckWindow.gameObject.SetActive(true);
        this.endOpenUsecheck();
    }

    public void procNG()
    {
        SingletonMonoBehaviour<CommonUI>.Instance.OpenNotificationDialog(LocalizationManager.Get("BATTLE_DIALOG_RETIRE_TITLE"), LocalizationManager.Get("BATTLE_DIALOG_RETIRE_CONF"), new NotificationDialog.ClickDelegate(this.endDialog), -1);
    }

    public void procNGNoStone()
    {
        this.closeNoStone();
        this.myFsm.SendEvent("END_PROC");
    }

    public void procNGSpell()
    {
        this.closeSpell();
        this.myFsm.SendEvent("END_PROC");
    }

    public void procNGStone()
    {
        this.closeStone();
        this.myFsm.SendEvent("END_PROC");
    }

    public void procOK()
    {
        this.perf.statusPerf.masterPerf.updateCommandSpellIcon();
        this.logic.sendFsmEvent("OK");
    }

    public void procSpell()
    {
        int spellImageId = UserGameMaster.getSelfUserGame().SpellImageId;
        AssetManager.loadAssetStorage($"CommandSpellEffect/ef_commandspell{spellImageId:D4}", new AssetLoader.LoadEndDataHandler(this.endLoadCommandSPell));
    }

    public void procStone()
    {
        this.myFsm.SendEvent("END_PROC");
    }

    public void showConf(BattleDropItem drop)
    {
    }

    public void StartContinue()
    {
        UserGameEntity entity = UserGameMaster.getSelfUserGame();
        this.itemWindow.setListData(this.data.getDropItems(), new BattleDropItemComponent.ClickDelegate(this.showConf), 200);
        this.itemWindow.setHide();
        this.commandspell_now.text = string.Empty + entity.getCommandSpell();
        this.gem_now.text = $"{entity.stone:#,0}";
        this.stone_now.text = $"{entity.stone:#,0}";
        this.commandspellObject.SetActive(false);
        this.stoneObject.SetActive(false);
        this.nostoneObject.SetActive(false);
        this.bg.SetActive(true);
        SoundManager.playSystemSe(SeManager.SystemSeKind.WINDOW_SLIDE);
        this.myFsm.SendEvent("START_CONTINUE");
    }
}

