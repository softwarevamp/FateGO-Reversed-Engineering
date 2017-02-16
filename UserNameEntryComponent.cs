using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using UnityEngine;

public class UserNameEntryComponent : BaseMonoBehaviour
{
    [CompilerGenerated]
    private static System.Action <>f__am$cache19;
    private int cellWidth;
    public UICenterOnChild centerChild;
    public CommandSpellIconComponent cmdSpellIconFemale;
    public CommandSpellIconComponent cmdSpellIconMale;
    public UIButton confirmBtn;
    public UISprite confirmTxt;
    public UILineInput entryNameInput;
    private GenderSelectControl genderSel;
    private int genderType;
    public UIWrapContent loopCtr;
    [SerializeField]
    private GameObject mArrowLeftPrefab;
    [SerializeField]
    private GameObject mArrowLeftRoot;
    [SerializeField]
    private GameObject mArrowRightPrefab;
    [SerializeField]
    private GameObject mArrowRightRoot;
    public GameObject[] masterFigureBaseList;
    private UIMasterFullFigureTexture[] masterFigureList;
    private System.Action mClosedAct;
    [SerializeField]
    private UILabel mInfoLabelFlick;
    [SerializeField]
    private UILabel mInfoLabelInput;
    [SerializeField]
    private UILabel mInfoLabelMain;
    [SerializeField]
    private UILabel mInfoLabelSub;
    [SerializeField]
    private UIInput mInput;
    private ScrollArrowComponent mScrollArrowLeft;
    private ScrollArrowComponent mScrollArrowRight;
    public UIScrollView scrollView;

    private void callbackUserNameChange(string result)
    {
        if (result == "ok")
        {
            UserGameEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_GAME).getSingleEntity<UserGameEntity>();
            if (entity != null)
            {
                SingletonMonoBehaviour<NetworkManager>.Instance.SetSignup(entity.name, entity.genderType);
                SingletonMonoBehaviour<NetworkManager>.Instance.WriteSignup();
            }
            SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, delegate {
                this.close();
                Input.imeCompositionMode = IMECompositionMode.Auto;
                this.mClosedAct.Call();
            });
        }
        else
        {
            this.mInput.value = string.Empty;
            this.entryNameInput.SetInputEnable(true);
        }
    }

    protected void close()
    {
        if (this.masterFigureList != null)
        {
            for (int i = 0; i < this.masterFigureList.Length; i++)
            {
                UnityEngine.Object.Destroy(this.masterFigureList[i].gameObject);
            }
            this.masterFigureList = null;
        }
    }

    private void endConfirm(bool res)
    {
        if (res)
        {
            this.requestUserNameChange();
        }
        SingletonMonoBehaviour<CommonUI>.Instance.CloseConfirmDialog();
    }

    public int getGendetType() => 
        this.genderType;

    private void OnCenterOnChildFinished()
    {
        this.genderSel = this.centerChild.centeredObject.GetComponent<GenderSelectControl>();
        this.genderType = this.genderSel.getGenderType();
    }

    public void onChangeInput()
    {
        string text = this.entryNameInput.GetText();
        bool flag = string.IsNullOrEmpty(text) || (text.Trim() == string.Empty);
        Regex regex = new Regex(@"^[a-zA-Z\u4e00-\u9fa5]+$");
        int num = 0;
        if (!flag && regex.IsMatch(text))
        {
            foreach (char ch in text)
            {
                if ((ch.GetHashCode() >= 0) && (ch.GetHashCode() < 0xff))
                {
                    num++;
                }
                else
                {
                    num += 2;
                }
            }
            if (num > 12)
            {
                flag = true;
            }
        }
        this.confirmBtn.isEnabled = !flag;
        Color color = !flag ? Color.white : Color.gray;
        this.confirmTxt.color = color;
    }

    public void onClickInput()
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        this.openConfirm();
    }

    public void open(System.Action closed_act)
    {
        this.mClosedAct = closed_act;
        Input.imeCompositionMode = IMECompositionMode.On;
        this.mInfoLabelMain.text = LocalizationManager.Get("INPUT_NAME_ANNOUNCE");
        this.mInfoLabelSub.text = LocalizationManager.Get("INPUT_NAME_INFO");
        this.mInfoLabelInput.text = LocalizationManager.Get("INPUT_NAME_LIMIT_NUM");
        this.mInfoLabelFlick.text = LocalizationManager.Get("SELECT_FIGURE_INFO");
        if (this.mScrollArrowRight == null)
        {
            GameObject self = UnityEngine.Object.Instantiate<GameObject>(this.mArrowRightPrefab);
            self.SafeSetParent(this.mArrowRightRoot);
            this.mScrollArrowRight = self.GetComponent<ScrollArrowComponent>();
        }
        if (this.mScrollArrowLeft == null)
        {
            GameObject obj3 = UnityEngine.Object.Instantiate<GameObject>(this.mArrowLeftPrefab);
            obj3.SafeSetParent(this.mArrowLeftRoot);
            this.mScrollArrowLeft = obj3.GetComponent<ScrollArrowComponent>();
        }
        this.setEntry();
        this.entryNameInput.SetInputEnable(true);
        if (this.masterFigureList == null)
        {
            this.masterFigureList = new UIMasterFullFigureTexture[this.masterFigureBaseList.Length];
            for (int i = 0; i < this.masterFigureBaseList.Length; i++)
            {
                this.masterFigureList[i] = MasterFullFigureManager.CreatePrefab(this.masterFigureBaseList[i], UIMasterFullFigureRender.DispType.USER_SELECT, ((i % 2) != 0) ? 1 : 2, 0, 1, null);
            }
        }
        this.onChangeInput();
        int id = 1;
        EquipEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.EQUIP).getEntityFromId<EquipEntity>(id);
        Vector2 sz = new Vector2(120f, 120f);
        this.cmdSpellIconFemale.SetChangeCmdSpellData(entity.femaleSpellId);
        this.cmdSpellIconFemale.SetSize(sz);
        this.cmdSpellIconMale.SetChangeCmdSpellData(entity.maleSpellId);
        this.cmdSpellIconMale.SetSize(sz);
        if (<>f__am$cache19 == null)
        {
            <>f__am$cache19 = delegate {
            };
        }
        SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, <>f__am$cache19);
    }

    private void openConfirm()
    {
        string text = this.entryNameInput.GetText();
        string str2 = (this.getGendetType() != 1) ? LocalizationManager.Get("ENTRY_GENDER_WOMAN") : LocalizationManager.Get("ENTRY_GENDER_MAN");
        string message = string.Format(LocalizationManager.Get("CONFIRM_INFO_MESSAGE"), str2, text);
        if (SingletonMonoBehaviour<AssetManager>.Instance.IsShieldingWord(text))
        {
            SingletonMonoBehaviour<CommonUI>.Instance.OpenWarningDialog("[FFFF80]用户名不正确", "用户名包含敏感词", null, false);
        }
        else
        {
            SingletonMonoBehaviour<CommonUI>.Instance.OpenConfirmDialog(LocalizationManager.Get("CONFIRM_TITLE_MESSAGE"), message, new CommonConfirmDialog.ClickDelegate(this.endConfirm));
        }
    }

    private void requestUserNameChange()
    {
        if (!ManagerConfig.UseMock && NetworkManager.IsLogin)
        {
            NetworkManager.getRequest<UserNameChangeRequest>(new NetworkManager.ResultCallbackFunc(this.callbackUserNameChange)).beginRequest(this.entryNameInput.GetText(), this.getGendetType(), 1);
        }
        else
        {
            SingletonMonoBehaviour<NetworkManager>.Instance.SetSignup(this.entryNameInput.GetText(), this.getGendetType());
            SingletonMonoBehaviour<NetworkManager>.Instance.WriteSignup();
            this.callbackUserNameChange("ok");
        }
        this.confirmBtn.isEnabled = false;
        this.confirmTxt.color = Color.gray;
        this.entryNameInput.SetInputEnable(false);
    }

    private void setCmdSpellImg(Gender.Type gtype)
    {
        if (this.genderType != gtype)
        {
            this.genderType = (int) gtype;
            if (this.genderType == 2)
            {
                this.cmdSpellIconFemale.gameObject.SetActive(true);
                this.cmdSpellIconMale.gameObject.SetActive(false);
            }
            else
            {
                this.cmdSpellIconFemale.gameObject.SetActive(false);
                this.cmdSpellIconMale.gameObject.SetActive(true);
            }
        }
    }

    protected void setEntry()
    {
        this.cellWidth = this.loopCtr.itemSize;
        if (this.centerChild == null)
        {
            this.centerChild = this.loopCtr.gameObject.AddComponent<UICenterOnChild>();
        }
        this.centerChild.onFinished = (SpringPanel.OnFinished) Delegate.Combine(this.centerChild.onFinished, new SpringPanel.OnFinished(this.OnCenterOnChildFinished));
        int childCount = this.loopCtr.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            this.genderSel = this.loopCtr.transform.GetChild(i).gameObject.GetComponent<GenderSelectControl>();
            this.genderSel.setIdx(i);
        }
    }

    private void Update()
    {
        if ((Mathf.RoundToInt(Mathf.Abs(this.scrollView.transform.localPosition.x) / ((float) this.cellWidth)) % 2) == 0)
        {
            this.setCmdSpellImg(Gender.Type.FEMALE);
        }
        else
        {
            this.setCmdSpellImg(Gender.Type.MALE);
        }
    }
}

