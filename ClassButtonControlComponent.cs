using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class ClassButtonControlComponent : MonoBehaviour
{
    [SerializeField]
    protected ClassButtonComponent[] classButton;
    [SerializeField]
    protected UISprite[] classCursor;
    private static readonly int[] classIdTable = new int[] { 0x3e9, 1, 2, 3, 4, 5, 6, 7 };
    protected int currentCursor;
    protected int oldCursor;
    [SerializeField]
    protected UICommonButton tabClassRotation;

    protected event CallbackFunc callbackFunc;

    public int getChangeCursorPos()
    {
        if (this.oldCursor == this.currentCursor)
        {
            return -1;
        }
        return this.currentCursor;
    }

    public void init(CallbackFunc callback, bool defaultPos = false)
    {
        ServantClassMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantClassMaster>(DataNameKind.Kind.SERVANT_CLASS);
        if (defaultPos)
        {
            this.currentCursor = 0;
        }
        this.oldCursor = this.currentCursor;
        this.callbackFunc = callback;
        for (int i = 0; i < BalanceConfig.SupportDeckMax; i++)
        {
            ServantClassEntity entity = master.getEntityFromId<ServantClassEntity>(classIdTable[i]);
            if (i == this.currentCursor)
            {
                this.classCursor[i].gameObject.SetActive(true);
            }
            else
            {
                this.classCursor[i].gameObject.SetActive(false);
            }
            AtlasManager.SetClassIcon(this.classButton[i].GetComponent<UISprite>(), entity.iconImageId, 2);
            this.classButton[i].setClassPos(i, new ClassButtonComponent.CallbackFunc(this.OnSelectButton));
        }
    }

    public void OnRotatetCursor()
    {
        int classPos = this.currentCursor + 1;
        if (classPos == BalanceConfig.SupportDeckMax)
        {
            classPos = 0;
        }
        this.setCursor(classPos);
    }

    public void OnSelectButton(int classPos)
    {
        this.setCursor(classPos);
    }

    public void setCursor(int classPos)
    {
        if (this.currentCursor != classPos)
        {
            this.oldCursor = this.currentCursor;
            this.currentCursor = classPos;
            this.classCursor[this.currentCursor].gameObject.SetActive(true);
            this.classCursor[this.oldCursor].gameObject.SetActive(false);
            if (this.callbackFunc != null)
            {
                this.callbackFunc(classPos);
            }
        }
    }

    public int GetCursorPos =>
        this.currentCursor;

    public delegate void CallbackFunc(int classPos);
}

