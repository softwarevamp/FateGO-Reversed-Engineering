using System;
using UnityEngine;

public class ClassCompatibilityInfoDialog : BaseDialog
{
    protected System.Action callbackFunc;
    [SerializeField]
    protected ServantClassIconComponent[] classIconList;
    [SerializeField]
    protected UICommonButton closeButton;
    protected System.Action closeCallbackFunc;
    [SerializeField]
    protected UILabel closeLabel;
    [SerializeField]
    protected UILabel messageLabel;
    protected int questId;
    protected int questPahase;
    protected State state;
    [SerializeField]
    protected UILabel titleLabel;
    [SerializeField]
    protected GameObject type1Base;
    [SerializeField]
    protected GameObject type2Base;

    public void Close()
    {
        this.Close(null);
    }

    public void Close(System.Action callback)
    {
        this.closeCallbackFunc = callback;
        this.state = State.CLOSE;
        base.Close(new System.Action(this.EndClose));
    }

    protected void EndClose()
    {
        this.Init();
        System.Action closeCallbackFunc = this.closeCallbackFunc;
        if (closeCallbackFunc != null)
        {
            this.closeCallbackFunc = null;
            closeCallbackFunc();
        }
    }

    protected void EndOpen()
    {
        this.state = State.INPUT;
    }

    public void Init()
    {
        this.messageLabel.text = string.Empty;
        this.closeLabel.text = string.Empty;
        this.state = State.INIT;
        base.Init();
    }

    public void OnClickClose()
    {
        if (this.state == State.INPUT)
        {
            this.state = State.SELECTED;
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            System.Action callbackFunc = this.callbackFunc;
            if (callbackFunc != null)
            {
                this.callbackFunc = null;
                callbackFunc();
            }
        }
    }

    public void Open(int questId, int questPhase, System.Action callback)
    {
        if (this.state == State.INIT)
        {
            this.questId = questId;
            this.questPahase = questPhase;
            this.callbackFunc = callback;
            base.gameObject.SetActive(true);
            this.titleLabel.text = LocalizationManager.Get("SERVANT_FRAME_PURCHASE_TITLE");
            if (this.questId > 0)
            {
                this.messageLabel.text = string.Format(LocalizationManager.Get("SERVANT_FRAME_PURCHASE_MESSAGE_START"), BalanceConfig.ServantFrameMax);
            }
            else
            {
                this.closeLabel.text = LocalizationManager.Get("SERVANT_FRAME_PURCHASE_CLOSE");
            }
            this.closeLabel.text = LocalizationManager.Get("COMMON_CONFIRM_CLOSE");
            QuestPhaseEntity entity = (this.questId <= 0) ? null : SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.QUEST_PHASE).getEntityFromId<QuestPhaseEntity>(questId, questPhase);
            if (((entity != null) && (entity.classIds != null)) && (entity.classIds.Length > 0))
            {
                this.type1Base.SetActive(false);
                this.type2Base.SetActive(true);
                for (int i = 0; i < this.classIconList.Length; i++)
                {
                    ServantClassIconComponent component = this.classIconList[i];
                    if (((entity.classIds != null) && (entity.classIds.Length > i)) && (entity.classIds[i] > 0))
                    {
                        component.Set(entity.classIds[i]);
                    }
                    else
                    {
                        component.Clear();
                    }
                }
            }
            else
            {
                this.type1Base.SetActive(true);
                this.type2Base.SetActive(false);
            }
            this.state = State.OPEN;
            base.Open(new System.Action(this.EndOpen), true);
        }
    }

    protected enum State
    {
        INIT,
        OPEN,
        INPUT,
        SELECTED,
        CLOSE
    }
}

