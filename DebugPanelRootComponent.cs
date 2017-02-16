using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

public class DebugPanelRootComponent : MonoBehaviour
{
    public UILabel debugStr;
    public List<GameObject> list = new List<GameObject>();
    public Transform menuRoot;
    public GameObject mst_selectmenu;
    public List<string> strlist = new List<string>();

    public void ClearMenu()
    {
        foreach (GameObject obj2 in this.list)
        {
            UnityEngine.Object.Destroy(obj2);
        }
        this.list.Clear();
    }

    public void CloseWindow()
    {
        base.gameObject.SetActive(false);
    }

    private DebugMenuComponent createMenu()
    {
        GameObject item = UnityEngine.Object.Instantiate<GameObject>(this.mst_selectmenu);
        item.SetActive(true);
        int count = this.list.Count;
        item.transform.parent = this.menuRoot;
        item.transform.localPosition = new Vector3(0f, (float) (-70 * count));
        item.transform.localScale = Vector3.one;
        this.list.Add(item);
        return item.GetComponent<DebugMenuComponent>();
    }

    public void OpenWindow()
    {
        base.gameObject.SetActive(true);
    }

    public void setLog(string str)
    {
        this.debugStr.text = str;
    }

    public void setLog(string[] strlist)
    {
        StringBuilder builder = new StringBuilder();
        if (strlist != null)
        {
            foreach (string str in strlist)
            {
                builder.AppendLine(str);
            }
            this.debugStr.text = builder.ToString();
        }
    }

    public void setMenu(string title, menuDelegate dg)
    {
        this.createMenu().setInitDlg(title, dg);
    }

    public void setMenu(string title, paramDelegate dg, int param)
    {
        this.createMenu().setInitDlg(title, dg, param);
    }

    public void setMenu(string title, paramStrDelegate dg, string param)
    {
        this.createMenu().setInitDlg(title, dg, param);
    }

    public void setMenu(string title, tgrDelegate dg, bool init)
    {
        this.createMenu().setInitDlg(title, dg, init);
    }

    public void setMenu(string title, paramtgrDelegate dg, int param, bool init)
    {
        this.createMenu().setInitDlg(title, dg, param, init);
    }

    public void setMenu(string title, paramDelegate dg, int param, int min, int max)
    {
        this.createMenu().setInitDlg(title, dg, param, min, max);
    }

    public delegate void menuDelegate();

    public delegate void paramDelegate(int param);

    public delegate void paramStrDelegate(string param);

    public delegate void paramtgrDelegate(int param, bool flg);

    public enum TAG
    {
        SVT,
        AI
    }

    public delegate void tgrDelegate(bool flg);
}

