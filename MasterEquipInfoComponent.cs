using System;
using UnityEngine;

public class MasterEquipInfoComponent : BaseMonoBehaviour
{
    private int currentIdx;
    private GameObject effectObj;
    [SerializeField]
    protected UILabel equipChangeHelpLb;
    [SerializeField]
    protected UILabel equipCondLvLb;
    [SerializeField]
    protected UILabel equipDetailLb;
    [SerializeField]
    protected GameObject equipEffectPrefab;
    [SerializeField]
    protected UISlider equipExpBar;
    [SerializeField]
    protected UILabel equipExpLb;
    [SerializeField]
    protected ItemIconComponent equipIconComp;
    [SerializeField]
    protected UISprite equipIconImg;
    [SerializeField]
    protected UILabel equipLevelLb;
    [SerializeField]
    protected UILabel equipMaxLvLb;
    [SerializeField]
    protected UILabel equipNameLb;
    [SerializeField]
    protected UISprite equipStatusImg;
    [SerializeField]
    protected GameObject equipStatusInfo;
    private bool isChange;
    private int moveEqIdx;
    [SerializeField]
    protected UILabel skillCheckHelpLb;
    [SerializeField]
    protected UIGrid skillInfoGrid;
    [SerializeField]
    protected GameObject skillInfoPrefab;
    private UserEquipEntity usrEquipEnt;

    public int getCurrentIdx() => 
        this.currentIdx;

    public int getEquipId() => 
        this.usrEquipEnt.equipId;

    public int getMoveBannerIdx() => 
        this.moveEqIdx;

    public long getUsrEquipId() => 
        this.usrEquipEnt.id;

    public bool isChangeEquip() => 
        this.isChange;

    public void setDispEffectObj(bool isDisp)
    {
        if (this.effectObj != null)
        {
            this.effectObj.SetActive(isDisp);
        }
    }

    public void setEquipInfo(UserEquipEntity usrEquipData, long usrEquipId, int userLv, int idx, int moveIdx)
    {
        int num;
        int num2;
        string str;
        string str2;
        int num3;
        int num4;
        int num5;
        float num6;
        this.equipStatusInfo.SetActive(false);
        this.isChange = true;
        this.currentIdx = idx;
        this.moveEqIdx = moveIdx;
        usrEquipData.getEquipInfo(out num, out num2, out str, out str2, out num3);
        this.equipIconComp.SetEquipItem(num3);
        if (usrEquipData.id == usrEquipId)
        {
            this.equipStatusInfo.SetActive(true);
            this.effectObj = UnityEngine.Object.Instantiate<GameObject>(this.equipEffectPrefab);
            this.effectObj.transform.parent = this.equipStatusInfo.transform;
            this.effectObj.transform.localPosition = Vector3.zero;
            this.effectObj.transform.localScale = Vector3.one;
        }
        this.equipLevelLb.text = usrEquipData.lv.ToString();
        this.equipMaxLvLb.text = num2.ToString();
        this.equipNameLb.text = str;
        usrEquipData.getExpInfo(out num4, out num5, out num6);
        this.equipExpLb.text = num5.ToString();
        this.equipExpBar.value = num6;
        this.equipDetailLb.text = str2;
        this.usrEquipEnt = usrEquipData;
        this.setEquipSkillInfo();
        this.skillCheckHelpLb.text = LocalizationManager.Get("MASTER_EQUIP_SKILL_INFO_TXT");
        this.equipChangeHelpLb.text = LocalizationManager.Get("MASTER_EQUIP_EXPLANATION_TXT");
    }

    public void setEquipSkillInfo()
    {
        int[] numArray = this.usrEquipEnt.getSkillIdList();
        bool isNew = false;
        for (int i = 0; i < numArray.Length; i++)
        {
            int id = numArray[i];
            int skillLv = this.usrEquipEnt.getSkillLv(i);
            isNew = this.usrEquipEnt.IsNew();
            int iconId = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<SkillMaster>(DataNameKind.Kind.SKILL).getEntityFromId<SkillEntity>(id).iconId;
            GameObject obj2 = base.createObject(this.skillInfoPrefab, this.skillInfoGrid.transform, null);
            obj2.transform.localScale = Vector3.one;
            obj2.transform.localPosition = this.skillInfoGrid.transform.localPosition;
            obj2.GetComponent<EquipSkillInfoComponent>().setEquipSkillInfo(i, id, skillLv, iconId, isNew, new EquipSkillInfoComponent.ClickDelegate(this.setSkillCallBack));
        }
    }

    private void setSkillCallBack(int skillId, int skillLv)
    {
        string str;
        string str2;
        SkillEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<SkillMaster>(DataNameKind.Kind.SKILL).getEntityFromId<SkillEntity>(skillId);
        SkillLvEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<SkillLvMaster>(DataNameKind.Kind.SKILL_LEVEL).getEntityFromId<SkillLvEntity>(skillId, skillLv);
        entity.getSkillMessageInfo(out str, out str2, skillLv);
        str = str + " " + string.Format(LocalizationManager.Get("MASTER_EQSKILL_LV_TXT"), skillLv);
        string info = string.Format(LocalizationManager.Get("BATTLE_SKILLCHARGETURN"), entity2.chargeTurn);
        SingletonMonoBehaviour<CommonUI>.Instance.OpenDetailLongInfoDialog(str, info, str2);
    }
}

