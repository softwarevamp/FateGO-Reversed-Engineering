using System;
using UnityEngine;

public class DebugMenuComponent : MonoBehaviour
{
    private bool flg;
    private int max;
    private DebugPanelRootComponent.menuDelegate menudel;
    private int min;
    private int param;
    private DebugPanelRootComponent.paramDelegate paramdel;
    private DebugPanelRootComponent.paramStrDelegate paramStrdel;
    private DebugPanelRootComponent.paramtgrDelegate paramtgrdel;
    private string strParam;
    private DebugPanelRootComponent.tgrDelegate tgrdel;
    private string title;
    public UILabel titlelabel;
    private TYPE type;

    public void OnClickMenu()
    {
        if (this.type == TYPE.SELECT)
        {
            if (this.menudel != null)
            {
                this.menudel();
            }
        }
        else if (this.type == TYPE.TGR)
        {
            this.flg = !this.flg;
            this.tgrdel(this.flg);
            this.updateTitleLabel();
        }
        else if (this.type == TYPE.PARAM)
        {
            this.paramdel(this.param);
        }
        else if (this.type == TYPE.PARAM_STR)
        {
            this.paramStrdel(this.strParam);
        }
        else if (this.type == TYPE.PARAM_TGR)
        {
            this.flg = !this.flg;
            this.paramtgrdel(this.param, this.flg);
            this.updateTitleLabel();
        }
        else if (this.type == TYPE.PARAM_CHANGE)
        {
            this.param++;
            if (this.max < this.param)
            {
                this.param = this.min;
            }
            this.paramdel(this.param);
            this.updateTitleLabel();
        }
    }

    public void setInitDlg(string txt, DebugPanelRootComponent.menuDelegate del)
    {
        this.menudel = del;
        this.type = TYPE.SELECT;
        this.setTitle(txt);
        this.updateTitleLabel();
    }

    public void setInitDlg(string txt, DebugPanelRootComponent.paramDelegate del, int param)
    {
        this.paramdel = del;
        this.type = TYPE.PARAM;
        this.setTitle(txt);
        this.setParam(param);
        this.updateTitleLabel();
    }

    public void setInitDlg(string txt, DebugPanelRootComponent.paramStrDelegate del, string param)
    {
        this.paramStrdel = del;
        this.type = TYPE.PARAM_STR;
        this.setTitle(txt);
        this.setStringParam(param);
        this.updateTitleLabel();
    }

    public void setInitDlg(string txt, DebugPanelRootComponent.tgrDelegate del, bool flg)
    {
        this.tgrdel = del;
        this.type = TYPE.TGR;
        this.setTitle(txt);
        this.setTgr(flg);
        this.updateTitleLabel();
    }

    public void setInitDlg(string txt, DebugPanelRootComponent.paramtgrDelegate del, int param, bool flg)
    {
        this.paramtgrdel = del;
        this.type = TYPE.PARAM_TGR;
        this.setTitle(txt);
        this.setTgr(flg);
        this.setParam(param);
        this.updateTitleLabel();
    }

    public void setInitDlg(string txt, DebugPanelRootComponent.paramDelegate del, int param, int min, int max)
    {
        this.paramdel = del;
        this.type = TYPE.PARAM_CHANGE;
        this.setTitle(txt);
        this.setParam(param);
        this.min = min;
        this.max = max;
        this.updateTitleLabel();
    }

    public void setParam(int param)
    {
        this.param = param;
    }

    public void setStringParam(string param)
    {
        this.strParam = param;
    }

    public void setTgr(bool initFlg)
    {
        this.flg = initFlg;
    }

    public void setTitle(string txt)
    {
        this.title = txt;
    }

    public void updateTitleLabel()
    {
        string str2;
        string title = this.title;
        if (this.type == TYPE.TGR)
        {
            str2 = title;
            object[] objArray1 = new object[] { str2, " [", this.flg, "]" };
            title = string.Concat(objArray1);
        }
        else if (this.type == TYPE.PARAM_CHANGE)
        {
            str2 = title;
            object[] objArray2 = new object[] { str2, "[", this.param, "]" };
            title = string.Concat(objArray2);
        }
        this.titlelabel.text = title;
    }

    private enum TYPE
    {
        SELECT,
        TGR,
        PARAM,
        PARAM_TGR,
        PARAM_STR,
        PARAM_CHANGE
    }
}

