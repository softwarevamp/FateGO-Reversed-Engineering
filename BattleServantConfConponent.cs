using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class BattleServantConfConponent : BattleWindowComponent
{
    private BattleServantData bsvtData;
    public BattleViewBufflistComponent buffListView;
    public BattleCommandComponent commandCard;
    public UILabel equipSkillConfLabel;
    public UILabel equipSkillNameLabel;
    public SkillIconComponent equipSkillSprite;
    public UITexture facetex;
    public UILabel havenotTdLabel;
    public UILabel maxNp;
    public UILabel npdetail;
    public UILabel nplevel;
    public GameObject npRoot;

    public void Close(BattleWindowComponent.EndCall call = null)
    {
        this.buffListView.setHide();
        base.Close(call);
    }

    public void CompOpen()
    {
        this.buffListView.setShow();
        base.CompOpen();
    }

    public void Initialize()
    {
        base.gameObject.transform.localPosition = new Vector3(0f, base.gameObject.transform.localPosition.y);
        if (this.commandCard != null)
        {
            this.commandCard.setDepth(200);
            this.commandCard.GetComponent<Collider>().enabled = false;
        }
        this.havenotTdLabel.text = LocalizationManager.Get("BATTLE_HASNOT_TD");
        base.setInitData(BattleWindowComponent.ACTIONTYPE.ALPHA, 0.3f, false);
    }

    public bool isTargetSvt(int uniqueId) => 
        ((this.bsvtData != null) && (this.bsvtData.getUniqueID() == uniqueId));

    public void Open(BattleWindowComponent.EndCall call = null)
    {
        this.buffListView.setHide();
        base.Open(call);
    }

    public void setConfData(BattleServantData inbsvtData)
    {
        this.bsvtData = inbsvtData;
        if (this.bsvtData != null)
        {
            this.buffListView.setBuffList(this.bsvtData.getBuffData().getShowBuffList());
            this.setEquipList();
            int num = this.bsvtData.getTreasureDvcId();
            int num2 = this.bsvtData.getTreasureDvcLevel();
            if (0 < num)
            {
                if (this.npRoot != null)
                {
                    this.npRoot.SetActive(true);
                    this.commandCard.gameObject.SetActive(true);
                    TreasureDvcLvEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<TreasureDvcLvMaster>(DataNameKind.Kind.TREASUREDEVICE_LEVEL).getEntityFromId<TreasureDvcLvEntity>(num, num2);
                    if (entity != null)
                    {
                        WrapControlText.textAdjust(this.npdetail, entity.getDetalShort(num2));
                    }
                    this.nplevel.text = string.Empty + num2;
                    this.maxNp.text = $"Max {entity.gaugeCount * 100}%";
                    if (this.commandCard != null)
                    {
                        BattleCommandData indata = new BattleCommandData {
                            type = this.bsvtData.getTreasureDvcCardId(),
                            svtlimit = this.bsvtData.getCommandDispLimitCount(),
                            loadSvtLimit = this.bsvtData.getDispLimitCount(),
                            uniqueId = this.bsvtData.getUniqueID(),
                            svtId = this.bsvtData.getSvtId(),
                            treasureDvc = this.bsvtData.getTreasureDvcId()
                        };
                        this.commandCard.setData(indata, null);
                    }
                    this.havenotTdLabel.gameObject.SetActive(false);
                }
            }
            else
            {
                if (this.npRoot != null)
                {
                    this.npRoot.SetActive(false);
                    this.commandCard.gameObject.SetActive(false);
                }
                this.facetex = ServantAssetLoadManager.loadCommandCard(this.facetex, this.bsvtData.getSvtId(), this.bsvtData.getDispLimitCount(), this.bsvtData.getCommandDispLimitCount());
                if (this.havenotTdLabel != null)
                {
                    this.havenotTdLabel.gameObject.SetActive(true);
                }
            }
        }
    }

    public void setEquipList()
    {
        if (this.equipSkillSprite != null)
        {
            this.equipSkillSprite.Clear();
        }
        if (this.equipSkillNameLabel != null)
        {
            this.equipSkillNameLabel.text = string.Empty;
        }
        if (this.equipSkillConfLabel != null)
        {
            this.equipSkillConfLabel.text = string.Empty;
        }
        if (this.bsvtData != null)
        {
            SkillMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<SkillMaster>(DataNameKind.Kind.SKILL);
            SkillLvMaster master2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<SkillLvMaster>(DataNameKind.Kind.SKILL_LEVEL);
            BattleUserServantData[] dataArray = this.bsvtData.getEquipBattleUserServantList();
            for (int i = 0; i < dataArray.Length; i++)
            {
                if (dataArray[i] != null)
                {
                    int[] numArray = dataArray[i].getBattleSkillIdList();
                    int[] numArray2 = dataArray[i].getSkillLevelList();
                    int index = 0;
                    while (index < numArray.Length)
                    {
                        int id = numArray[index];
                        int num4 = numArray2[index];
                        SkillEntity entity = master.getEntityFromId<SkillEntity>(id);
                        SkillLvEntity entity2 = master2.getEntityFromId<SkillLvEntity>(id, num4);
                        this.equipSkillNameLabel.text = $"{entity.getName()}";
                        WrapControlText.textAdjust(this.equipSkillConfLabel, entity2.getDetail(num4));
                        this.equipSkillSprite.Set(id);
                        return;
                    }
                }
            }
        }
    }
}

