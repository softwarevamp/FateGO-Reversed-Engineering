using System;
using System.Collections.Generic;

public class BattleLogData
{
    private List<string> list;
    private int max;
    private string tag;

    public BattleLogData()
    {
        this.list = new List<string>();
        this.tag = string.Empty;
        this.max = 10;
        this.tag = string.Empty;
    }

    public BattleLogData(string intag)
    {
        this.list = new List<string>();
        this.tag = string.Empty;
        this.max = 10;
        this.tag = intag;
    }

    public BattleLogData(string intag, int inmax)
    {
        this.list = new List<string>();
        this.tag = string.Empty;
        this.max = 10;
        this.tag = intag;
        this.max = inmax;
    }

    public void addStr(string str)
    {
        this.list.Add(this.tag + str);
        if (this.max < this.list.Count)
        {
            this.list.RemoveAt(0);
        }
    }

    public string[] getStringList() => 
        this.list.ToArray();
}

