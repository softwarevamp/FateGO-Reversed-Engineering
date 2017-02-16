using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class BattleResultBondsComponent : MonoBehaviour
{
    private bool bondsCountUp;
    private int colIndex;
    public BattleResultBondsIconComponent[] collects;
    public Transform confRoot;
    public UISprite confSprite;
    public BattleWindowComponent confwindow;
    public Transform figureRoot;
    public Animation levelUpAnim;
    public BattleResultMasterUpStatusComponent lvComp;
    private SePlayer MeterSePlayer;
    public PlayMakerFSM myFsm;
    public UserServantCollectionEntity[] oldCollections;
    private bool openBoundsFlg;
    private BattleResultBondsIconComponent openCollect;
    public BattleResultComponent parentComp;
    public UILabel rankupConfLabel;
    private UIStandFigureR standfigure;
    private bool updateFlg;
    public GameObject upRoot;
    public BattleWindowComponent window;

    public void checkBondsUp()
    {
        for (int i = this.colIndex; i < this.collects.Length; i++)
        {
            this.colIndex = i;
            if (this.collects[i].isChangeRank())
            {
                this.upRoot.SetActive(true);
                this.levelUpAnim["bit_result_levelup01"].time = 0f;
                this.levelUpAnim.Play("bit_result_levelup01");
                this.openCollect = this.collects[i];
                this.standfigure = StandFigureManager.CreateRenderPrefab(this.figureRoot.gameObject, this.collects[i].getSvtId(), this.collects[i].getSvtLimitCount(), this.collects[i].getLv(), Face.Type.NORMAL, 0x33, null);
                object[] args = new object[] { "x", -500f, "time", 0.8f, "islocal", true, "oncompletetarget", base.gameObject, "oncomplete", "endMoveFigure" };
                iTween.MoveFrom(this.standfigure.gameObject, iTween.Hash(args));
                this.openBoundsFlg = true;
                this.myFsm.SendEvent("OPEN");
                return;
            }
        }
        if (this.openBoundsFlg)
        {
            this.myFsm.SendEvent("END_PROC");
        }
        else
        {
            this.myFsm.SendEvent("CLOSE");
        }
    }

    public void Close()
    {
        this.window.Close(new BattleWindowComponent.EndCall(this.endClose));
    }

    public void closeBondUp()
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        this.parentComp.setTouch(false);
        this.standfigure.ReleaseCharacter();
        this.confwindow.Close(new BattleWindowComponent.EndCall(this.endCloseBondUp));
    }

    public void endClose()
    {
        base.gameObject.SetActive(false);
        this.myFsm.SendEvent("END_PROC");
    }

    public void endCloseBondUp()
    {
        this.myFsm.SendEvent("END_PROC");
    }

    public void endMoveFigure()
    {
        StringBuilder builder = new StringBuilder();
        int num = 0;
        SoundManager.playSystemSe(SeManager.SystemSeKind.STATUS_OPEN);
        this.lvComp.setData(this.openCollect.getPrevFriendShipRank(), this.openCollect.getNextFriendShipRank());
        List<QuestEntity> list = SingletonTemplate<clsQuestCheck>.Instance.GetReleaseQuestEntityByServantFriendShip(this.openCollect.getSvtId(), this.openCollect.getPrevFriendShipRank(), QuestEntity.TypeFlag.FRIENDSHIP);
        if (list != null)
        {
            foreach (QuestEntity entity in list)
            {
                builder.AppendLine(string.Format(LocalizationManager.Get("RESULT_BOUNDS_OPENQUEST"), entity.getQuestName()));
                num += 3;
            }
        }
        if (ServantCommentManager.IsOpenByServantFriendShip(this.openCollect.getSvtId(), this.openCollect.getPrevFriendShipRank()))
        {
            builder.AppendLine(LocalizationManager.Get("RESULT_BOUNDS_UPDATE_MATERIAL"));
            num += 2;
        }
        if (ServantVoiceMaster.isOpenByServantFriendShip(this.openCollect.getSvtId(), this.openCollect.getMaxLimitCount(), this.openCollect.getPrevFriendShipRank()))
        {
            builder.AppendLine(LocalizationManager.Get("RESULT_BOUNDS_GETVOICE"));
            num += 2;
        }
        this.confSprite.height = 120 + (num * 0x16);
        this.confRoot.localPosition = new Vector3(this.confRoot.localPosition.x, 11f * num);
        this.rankupConfLabel.text = builder.ToString();
        this.confwindow.Open(new BattleWindowComponent.EndCall(this.openedBondUp));
    }

    public void finishUpdateValue()
    {
        iTween component = base.gameObject.GetComponent<iTween>();
        if (component != null)
        {
            iTween.Stop(base.gameObject);
            UnityEngine.Object.DestroyImmediate(component);
        }
        bool flag = false;
        for (int i = 0; i < this.collects.Length; i++)
        {
            flag |= this.collects[i].changeGauge(1f);
        }
        this.colIndex = 0;
        if (this.MeterSePlayer != null)
        {
            this.MeterSePlayer.StopSe(0f);
        }
        this.bondsCountUp = false;
        this.myFsm.SendEvent("NEXT");
    }

    public UserServantCollectionEntity getServantCollection(UserServantCollectionEntity[] collects, int svtId)
    {
        foreach (UserServantCollectionEntity entity in collects)
        {
            if (entity.getSvtId() == svtId)
            {
                return entity;
            }
        }
        return null;
    }

    public long getUsetSvtId(DeckData deck, int index)
    {
        foreach (BattleDeckServantData data in deck.svts)
        {
            if (data.id == (index + 1))
            {
                return data.getUserServantID();
            }
        }
        return 0L;
    }

    public void Init()
    {
        this.window.setInitData(BattleWindowComponent.ACTIONTYPE.ALPHA, 0.3f, false);
        this.window.setClose();
        this.confwindow.setInitData(BattleWindowComponent.ACTIONTYPE.ALPHA, 0.3f, false);
        this.confwindow.setClose();
        this.upRoot.SetActive(false);
        base.gameObject.SetActive(false);
    }

    public bool isCollectsSvt() => 
        (1 < this.oldCollections.Length);

    public void Open()
    {
        base.gameObject.SetActive(true);
        this.openBoundsFlg = false;
        this.bondsCountUp = true;
        this.window.Open(new BattleWindowComponent.EndCall(this.OpenEnd));
        this.myFsm.SendEvent("END_OPEN");
    }

    public void openedBondUp()
    {
        this.parentComp.setTouch(true);
        this.colIndex++;
        this.myFsm.SendEvent("END_OPEN");
    }

    public void OpenEnd()
    {
        if (this.bondsCountUp)
        {
            object[] args = new object[] { "from", 0f, "to", 1f, "onupdate", "UpdateValue", "oncomplete", "finishUpdateValue", "time", 1.8f };
            iTween.ValueTo(base.gameObject, iTween.Hash(args));
            if (this.updateFlg)
            {
                this.MeterSePlayer = SoundManager.playSe("ba24");
            }
        }
    }

    public void setResultData(DeckData myDeck, UserServantCollectionEntity[] oldCollects)
    {
        this.oldCollections = oldCollects;
        this.updateFlg = false;
        UserServantCollectionMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantCollectionMaster>(DataNameKind.Kind.USER_SERVANT_COLLECTION);
        UserServantMaster master2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantMaster>(DataNameKind.Kind.USER_SERVANT);
        ServantMaster master3 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantMaster>(DataNameKind.Kind.SERVANT);
        int index = 0;
        foreach (BattleDeckServantData data in myDeck.svts)
        {
            long id = data.getUserServantID();
            if (0L < id)
            {
                UserServantEntity usetSvtEnt = master2.getEntityFromId<UserServantEntity>(id);
                UserServantCollectionEntity userSvtCol = this.getServantCollection(oldCollects, usetSvtEnt.getSvtId());
                ServantEntity entity3 = master3.getEntityFromId<ServantEntity>(usetSvtEnt.getSvtId());
                this.collects[index].setServantData(userSvtCol, usetSvtEnt);
                if (entity3.checkIsHeroineSvt())
                {
                    this.updateFlg |= this.collects[index].setNextServantData(userSvtCol);
                    this.collects[index].setHeroine();
                }
                else
                {
                    UserServantCollectionEntity entity4 = master.getEntityFromId(this.collects[index].getUserId(), this.collects[index].getSvtId());
                    this.updateFlg |= this.collects[index].setNextServantData(entity4);
                }
                this.collects[index].changeGauge(0f);
                index++;
            }
        }
        for (int i = index; i < this.collects.Length; i++)
        {
            this.collects[i].setServantData(null, null);
            this.collects[i].changeGauge(0f);
        }
    }

    public void UpdateValue(float val)
    {
        bool flag = false;
        for (int i = 0; i < this.collects.Length; i++)
        {
            flag |= this.collects[i].changeGauge(val);
        }
    }
}

