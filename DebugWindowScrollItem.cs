using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class DebugWindowScrollItem : MonoBehaviour
{
    private Action<DebugWindowScrollItem, CallBackMessage> mCallBackAct;
    private DebugWindow mDebugChildWin;
    private DebugWindow mDebugWin;
    private int mIdx;
    private bool mIsPressPlus;
    [SerializeField]
    private UILabel mItemLabel;
    private float mPressIntervalTime;
    private float mPressTime;
    private string mStr;
    private Type mType;
    [SerializeField]
    private UIInput mValInput;
    [SerializeField]
    private UILabel mValLabel;
    [SerializeField]
    private GameObject mValPlusMinus;
    public const float ONPRESS_REPEAT_INTERVAL_TIME = 0.025f;
    public const float ONPRESS_REPEAT_START_TIME = 0.5f;

    private void DecrementVal()
    {
        this.mStr = (this.Val - 1).ToString();
        this.mValInput.value = this.mStr;
    }

    private void IncrementVal()
    {
        this.mStr = (this.Val + 1).ToString();
        this.mValInput.value = this.mStr;
    }

    private void KeyRepeat()
    {
        if (CTouch.isDrag())
        {
            this.mPressTime = 0f;
        }
        if (this.mPressTime > 0f)
        {
            float time = Time.time;
            float num2 = time - this.mPressTime;
            if (num2 > 0.5f)
            {
                num2 = time - this.mPressIntervalTime;
                if (num2 > 0.025f)
                {
                    this.mPressIntervalTime = time;
                    if (this.mIsPressPlus)
                    {
                        this.IncrementVal();
                    }
                    else
                    {
                        this.DecrementVal();
                    }
                }
            }
        }
    }

    public void OnClickInputChange()
    {
        this.mStr = this.mValInput.value;
        if (this.mDebugWin.IsActive())
        {
            this.mCallBackAct.Call<DebugWindowScrollItem, CallBackMessage>(this, CallBackMessage.EDIT);
        }
    }

    public void OnClickItem()
    {
        switch (this.mType)
        {
            case Type.DISP_ONLY:
            case Type.PUSH_ONLY:
            case Type.VAL_EDIT:
            case Type.STR_EDIT:
                break;

            case Type.TOGGLE:
                this.Toggle();
                break;

            case Type.OPEN_CHILD:
                this.mDebugChildWin.Open();
                break;

            default:
                return;
        }
        if (this.mDebugWin.IsActive())
        {
            this.mCallBackAct.Call<DebugWindowScrollItem, CallBackMessage>(this, CallBackMessage.ON_CLICK);
        }
    }

    public void OnClickMinus()
    {
        this.DecrementVal();
    }

    public void OnClickPlus()
    {
        this.IncrementVal();
    }

    public void OnPressMinus()
    {
        this.mPressTime = Time.time;
        this.mIsPressPlus = false;
    }

    public void OnPressPlus()
    {
        this.mPressTime = Time.time;
        this.mIsPressPlus = true;
    }

    public void OnReleaseMinus()
    {
        this.mPressTime = 0f;
    }

    public void OnReleasePlus()
    {
        this.mPressTime = 0f;
    }

    public void Setup(DebugWindow dwin, int idx, ItemInfo iinf, Action<DebugWindowScrollItem, CallBackMessage> cb_act = null)
    {
        this.mDebugWin = dwin;
        this.mDebugChildWin = iinf.debug_win_child;
        if (this.mDebugChildWin != null)
        {
            this.mDebugChildWin.ParentWin = this.mDebugWin;
        }
        this.mIdx = idx;
        base.gameObject.name = string.Empty + idx;
        this.mItemLabel.text = base.gameObject.name + ". " + iinf.item_name;
        this.mType = iinf.type;
        this.mStr = iinf.str;
        this.mCallBackAct = cb_act;
        this.UpdateDisp();
        bool flag = (this.mType == Type.VAL_EDIT) || (this.mType == Type.STR_EDIT);
        this.mValInput.enabled = flag;
        this.mValInput.gameObject.GetComponent<BoxCollider>().enabled = flag;
        if (flag)
        {
            this.mValInput.value = this.mStr;
        }
        this.mValPlusMinus.SetActive(this.mType == Type.VAL_EDIT);
        CTouch.init();
    }

    public void Toggle()
    {
        this.mStr = (this.Val ^ 1).ToString();
        this.UpdateDisp();
    }

    private void Update()
    {
        CTouch.process();
        this.KeyRepeat();
        if (this.mDebugWin.IsActive())
        {
            this.mCallBackAct.Call<DebugWindowScrollItem, CallBackMessage>(this, CallBackMessage.UPDATE);
        }
    }

    private void UpdateDisp()
    {
        switch (this.mType)
        {
            case Type.DISP_ONLY:
                this.mValLabel.text = this.mStr;
                break;

            case Type.PUSH_ONLY:
            case Type.OPEN_CHILD:
                this.mValLabel.text = string.Empty;
                break;

            case Type.TOGGLE:
                this.mValLabel.text = (this.Val != 0) ? "✓" : "□";
                break;
        }
    }

    public int Idx =>
        this.mIdx;

    public bool IsEnable =>
        (this.Val != 0);

    public string Str
    {
        get => 
            this.mStr;
        set
        {
            this.mStr = value;
            this.UpdateDisp();
        }
    }

    public int Val
    {
        get
        {
            int result = 0;
            int.TryParse(this.mStr, out result);
            return result;
        }
        set
        {
            this.Str = value.ToString();
        }
    }

    public enum CallBackMessage
    {
        UPDATE,
        ON_CLICK,
        EDIT,
        SIZEOF
    }

    public class ItemInfo
    {
        public DebugWindow debug_win_child;
        public string item_name;
        public string str;
        public DebugWindowScrollItem.Type type;

        public ItemInfo(DebugWindow _debug_win_child)
        {
            this.debug_win_child = _debug_win_child;
            this.Init(this.debug_win_child.InfoStr + " >>", DebugWindowScrollItem.Type.OPEN_CHILD, string.Empty);
        }

        public ItemInfo(string _item_name, DebugWindowScrollItem.Type _type, bool _val)
        {
            this.Init(_item_name, _type, !_val ? "0" : "1");
        }

        public ItemInfo(string _item_name, DebugWindowScrollItem.Type _type, int _val)
        {
            this.Init(_item_name, _type, _val.ToString());
        }

        public ItemInfo(string _item_name, DebugWindowScrollItem.Type _type, string _str = "")
        {
            this.Init(_item_name, _type, _str);
        }

        private void Init(string _item_name, DebugWindowScrollItem.Type _type, string _str)
        {
            this.item_name = _item_name;
            this.type = _type;
            this.str = _str;
        }
    }

    public enum Type
    {
        NONE,
        DISP_ONLY,
        PUSH_ONLY,
        TOGGLE,
        VAL_EDIT,
        STR_EDIT,
        OPEN_CHILD,
        SIZEOF
    }
}

