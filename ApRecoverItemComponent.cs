using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ApRecoverItemComponent : MonoBehaviour
{
    private int apRcvRate;
    public UISprite bgImg;
    public GameObject cmdSpellBg;
    public CommandSpellIconComponent cmdSpellIcon;
    protected static readonly float COLOR_VAL = 0.375f;
    public UILabel currentInfoLb;
    public UILabel currentNumLb;
    private RecoverType.Type currentType;
    public const float FIXED_VAL = 1000f;
    private bool isAddAp;
    private bool isEnableSelect;
    public UILabel itemDetailLb;
    public ItemIconComponent itemIconInfo;
    public UILabel itemNameLb;
    public GameObject maskImg;
    private int recvApNum;
    private int recvSum;
    public UILabel spendInfoLb;
    private int spendNum;
    public UILabel spendNumLb;
    private int targetId;
    private UserGameEntity userEntity;
    private int usrCurrentAp;
    private int usrMaxAp;

    protected event CallbackFunc callbackFunc;

    private void closeNotificationDlg()
    {
        SingletonMonoBehaviour<CommonUI>.Instance.CloseNotificationDialog();
    }

    public void OnClickItem()
    {
        if (this.isEnableSelect)
        {
            string title = string.Empty;
            string str2 = string.Format(LocalizationManager.Get("UNIT_INFO"), this.spendNum);
            string str3 = (this.currentType != RecoverType.Type.COMMAND_SPELL) ? this.itemNameLb.text : LocalizationManager.Get("COMMAND_SPELL_TXT");
            if (this.currentType == RecoverType.Type.COMMAND_SPELL)
            {
                str2 = string.Format(LocalizationManager.Get("CMDSPELL_CURRENT_NUM"), this.spendNum);
            }
            string message = string.Format(LocalizationManager.Get("AP_RECOVER_CONFIRM_MSG"), new object[] { str3, str2, this.apRcvRate, this.recvApNum, this.usrCurrentAp, this.usrMaxAp, this.recvSum, this.usrMaxAp });
            if (this.isAddAp)
            {
                message = string.Format(LocalizationManager.Get("AP_ADD_CONFIRM_MSG"), new object[] { str3, str2, this.recvApNum, this.usrCurrentAp, this.usrMaxAp, this.recvSum, this.usrMaxAp });
            }
            string decideTxt = LocalizationManager.Get("COMMON_CONFIRM_DECIDE");
            string cancleTxt = LocalizationManager.Get("COMMON_CONFIRM_CANCEL");
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            if (this.userEntity.getAct() >= this.usrMaxAp)
            {
                string str7 = string.Format(LocalizationManager.Get("AP_FULL_MSG"), str3);
                SingletonMonoBehaviour<CommonUI>.Instance.OpenNotificationDialog(string.Empty, str7, new System.Action(this.closeNotificationDlg), -1);
            }
            else
            {
                SingletonMonoBehaviour<CommonUI>.Instance.OpenConfirmDecideDlg(title, message, decideTxt, cancleTxt, new CommonConfirmDialog.ClickDelegate(this.spendItemDlg));
            }
        }
    }

    private void setEnableSelectItem()
    {
        if (!this.isEnableSelect)
        {
            this.maskImg.SetActive(true);
        }
    }

    public void setRecvItemInfo(ApRecoverEntity data, int needAp, CallbackFunc callback)
    {
        int num8;
        this.currentType = (RecoverType.Type) data.recoverType;
        this.targetId = data.targetId;
        this.isEnableSelect = false;
        this.isAddAp = false;
        this.callbackFunc = callback;
        this.maskImg.SetActive(false);
        this.userEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_GAME).getSingleEntity<UserGameEntity>();
        this.usrCurrentAp = this.userEntity.getAct();
        this.usrMaxAp = this.userEntity.actMax;
        this.recvApNum = this.usrMaxAp;
        this.spendNum = 1;
        this.apRcvRate = 100;
        this.itemNameLb.text = "-";
        this.currentNumLb.text = "-";
        this.spendNumLb.text = "-";
        this.itemDetailLb.text = "-";
        this.currentInfoLb.gameObject.SetActive(false);
        this.currentInfoLb.text = LocalizationManager.Get("CURRENT_NUM_TXT");
        this.spendInfoLb.text = LocalizationManager.Get("SPEND_NUM_TXT");
        switch (this.currentType)
        {
            case RecoverType.Type.COMMAND_SPELL:
            {
                this.itemIconInfo.gameObject.SetActive(false);
                this.cmdSpellBg.SetActive(true);
                this.currentInfoLb.gameObject.SetActive(true);
                this.currentInfoLb.text = LocalizationManager.Get("CMDSPELL_CURRENT_NUM_TXT");
                this.spendInfoLb.text = LocalizationManager.Get("CMDSPELL_SPEND_NUM_TXT");
                CommandSpellEntity entity5 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<CommandSpellMaster>(DataNameKind.Kind.COMMAND_SPELL).getEntityFromId<CommandSpellEntity>(this.targetId);
                if (entity5 == null)
                {
                    goto Label_07FA;
                }
                this.cmdSpellIcon.SetData(this.userEntity);
                this.itemNameLb.text = entity5.name;
                num8 = this.userEntity.getCommandSpell();
                this.spendNum = entity5.consume;
                int[] numArray2 = entity5.getValues()[0];
                if (entity5.type == 3)
                {
                    float num9 = (((float) numArray2[0]) / 1000f) * 100f;
                    this.apRcvRate = (int) num9;
                    this.recvApNum = Mathf.CeilToInt(this.usrMaxAp * (((float) numArray2[0]) / 1000f));
                    string str4 = string.Format(LocalizationManager.Get("CMDSPELL_CURRENT_NUM"), this.spendNum);
                    this.itemDetailLb.text = string.Format(LocalizationManager.Get("AP_RECOVER_NUM_TXT"), str4, this.apRcvRate, this.recvApNum);
                }
                if (entity5.type != 4)
                {
                    goto Label_07A0;
                }
                int num10 = numArray2[0];
                if (needAp <= 0)
                {
                    this.recvApNum = this.spendNum * num10;
                    break;
                }
                float f = ((float) (needAp - this.usrCurrentAp)) / ((float) num10);
                if (f <= 1f)
                {
                    f = 1f;
                }
                this.spendNum = Mathf.CeilToInt(f);
                this.recvApNum = this.spendNum * num10;
                break;
            }
            case RecoverType.Type.STONE:
            {
                this.itemIconInfo.gameObject.SetActive(true);
                this.cmdSpellBg.SetActive(false);
                ItemEntity entityByType = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ItemMaster>(DataNameKind.Kind.ITEM).GetEntityByType(2);
                this.itemIconInfo.SetItemImage((ImageItem.Id) entityByType.imageId, entityByType.bgImageId, entityByType.type, 0);
                this.itemIconInfo.gameObject.SetActive(true);
                StoneShopEntity entity2 = (SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.STONE_SHOP) as StoneShopMaster).getEntityFromId<StoneShopEntity>(this.targetId);
                this.itemNameLb.text = entityByType.name;
                int stone = this.userEntity.stone;
                if (entity2 != null)
                {
                    this.spendNum = entity2.price;
                }
                this.currentNumLb.text = string.Format(LocalizationManager.Get("CURRENT_NUM_TXT"), stone);
                this.spendNumLb.text = this.spendNum.ToString();
                string str = string.Format(LocalizationManager.Get("UNIT_INFO"), this.spendNum);
                this.itemDetailLb.text = string.Format(LocalizationManager.Get("AP_RECOVER_NUM_TXT"), str, this.apRcvRate, this.recvApNum);
                this.isEnableSelect = stone >= this.spendNum;
                this.recvSum = this.usrCurrentAp + this.usrMaxAp;
                goto Label_07FA;
            }
            case RecoverType.Type.ITEM:
            {
                this.itemIconInfo.gameObject.SetActive(true);
                this.cmdSpellBg.SetActive(false);
                ItemEntity entity3 = data.getApRecvItemData();
                if (entity3 != null)
                {
                    this.itemIconInfo.SetItemImage((ImageItem.Id) entity3.imageId, entity3.bgImageId, entity3.type, 0);
                    this.itemIconInfo.gameObject.SetActive(true);
                    this.itemNameLb.text = entity3.name;
                    long[] args = new long[] { this.userEntity.userId, (long) this.targetId };
                    UserItemEntity entity4 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_ITEM).getEntityFromId<UserItemEntity>(args);
                    int num = 0;
                    this.spendNum = 1;
                    if (entity4 != null)
                    {
                        num = entity4.num;
                    }
                    if (entity3.type == 3)
                    {
                        this.spendNum = BalanceConfig.SpendApRecvItemNum;
                        float num3 = ((float) entity3.value) / 1000f;
                        float num4 = num3 * 100f;
                        this.apRcvRate = (int) num4;
                        this.recvApNum = Mathf.CeilToInt(this.usrMaxAp * num3);
                        if (needAp > 0)
                        {
                            float num5 = ((float) (needAp - this.usrCurrentAp)) / ((float) this.recvApNum);
                            if (num5 <= 1f)
                            {
                                num5 = 1f;
                            }
                            this.spendNum = Mathf.CeilToInt(num5);
                            this.apRcvRate *= this.spendNum;
                            this.recvApNum *= this.spendNum;
                            Debug.Log("!! ** !! Item Ratio this.spendNum: " + this.spendNum);
                        }
                        string str2 = string.Format(LocalizationManager.Get("UNIT_INFO"), this.spendNum);
                        this.itemDetailLb.text = string.Format(LocalizationManager.Get("AP_RECOVER_NUM_TXT"), str2, this.apRcvRate, this.recvApNum);
                    }
                    if (entity3.type == 4)
                    {
                        int num6 = entity3.value;
                        if (needAp > 0)
                        {
                            float num7 = ((float) (needAp - this.usrCurrentAp)) / ((float) num6);
                            if (num7 <= 1f)
                            {
                                num7 = 1f;
                            }
                            this.spendNum = Mathf.CeilToInt(num7);
                            this.recvApNum = this.spendNum * num6;
                        }
                        else
                        {
                            this.spendNum = 1;
                            this.recvApNum = this.spendNum * num6;
                        }
                        string str3 = string.Format(LocalizationManager.Get("UNIT_INFO"), this.spendNum);
                        this.itemDetailLb.text = string.Format(LocalizationManager.Get("ADD_RECOVER_NUM_TXT"), str3, this.recvApNum);
                        this.isAddAp = true;
                    }
                    this.currentNumLb.text = string.Format(LocalizationManager.Get("CURRENT_NUM_TXT"), num);
                    this.spendNumLb.text = this.spendNum.ToString();
                    this.isEnableSelect = num >= this.spendNum;
                    this.recvSum = this.usrCurrentAp + this.recvApNum;
                }
                goto Label_07FA;
            }
            default:
                goto Label_07FA;
        }
        string str5 = string.Format(LocalizationManager.Get("CMDSPELL_CURRENT_NUM"), this.spendNum);
        this.itemDetailLb.text = string.Format(LocalizationManager.Get("ADD_RECOVER_NUM_TXT"), str5, this.recvApNum);
        this.isAddAp = true;
    Label_07A0:
        this.currentNumLb.text = num8.ToString();
        this.spendNumLb.text = this.spendNum.ToString();
        this.isEnableSelect = num8 >= this.spendNum;
        this.recvSum = this.usrCurrentAp + this.recvApNum;
    Label_07FA:
        this.setEnableSelectItem();
    }

    private void setRequestInfo()
    {
        CallbackFunc callbackFunc = this.callbackFunc;
        if (callbackFunc != null)
        {
            this.callbackFunc = null;
            callbackFunc(this.currentType, this.targetId, this.spendNum);
        }
    }

    private void spendItemDlg(bool isRes)
    {
        if (isRes)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.CloseConfirmDialog(new System.Action(this.setRequestInfo));
        }
        else
        {
            SingletonMonoBehaviour<CommonUI>.Instance.CloseConfirmDialog();
        }
    }

    public delegate void CallbackFunc(RecoverType.Type type, int id, int num);

    public enum CMDSPELL_TYPE
    {
        AP_ADD = 4,
        AP_RECOVER = 3
    }
}

