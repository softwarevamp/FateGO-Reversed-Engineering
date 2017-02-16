using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

public class CommonMessageManager : ScriptMessageManager
{
    [CompilerGenerated]
    private static Dictionary<string, int> <>f__switch$map1A;
    [CompilerGenerated]
    private static Dictionary<string, int> <>f__switch$map1B;
    [CompilerGenerated]
    private static Dictionary<string, int> <>f__switch$map1C;
    protected System.Action callbackFunc;
    protected string codeCommentString;
    protected string codeReturnString;
    protected string codeTalkString;
    protected string codeVoiceString;
    protected string[] executeDataList;
    protected int executeIndex;
    protected int executeIndexMax;
    protected int[] executeLineList;
    protected string[] executeTagList;
    protected bool isCancelInput;
    protected PlaySpeed playSpeed = PlaySpeed.NORMAL;
    protected State state;
    protected string waitMessage;
    protected string waitTalkName;
    protected string waitType;

    protected string[] AnalysParam(string data)
    {
        int length = data.Length;
        List<string> list = new List<string>();
        string item = string.Empty;
        int num2 = 0;
        while (num2 < length)
        {
            char ch = data[num2++];
            if (ch == '"')
            {
                item = item + ch;
                while (num2 < length)
                {
                    ch = data[num2++];
                    if (ch == '"')
                    {
                        break;
                    }
                    item = item + ch;
                }
                item = item + '"';
                continue;
            }
            switch (ch)
            {
                case '=':
                case ',':
                {
                    if (item != string.Empty)
                    {
                        list.Add(item);
                        list.Add(string.Empty + ch);
                        item = string.Empty;
                    }
                    else if ((list.Count > 0) && (list[list.Count - 1] == " "))
                    {
                        list[list.Count - 1] = string.Empty + ch;
                    }
                    else
                    {
                        list.Add(string.Empty + ch);
                    }
                    continue;
                }
                case ' ':
                {
                    if (item != string.Empty)
                    {
                        list.Add(item);
                        list.Add(string.Empty + ch);
                        item = string.Empty;
                    }
                    else if ((list.Count <= 0) || ((list[list.Count - 1] != "=") && (list[list.Count - 1] != ",")))
                    {
                        list.Add(string.Empty + ch);
                    }
                    continue;
                }
            }
            item = item + ch;
        }
        if (item != string.Empty)
        {
            list.Add(item);
        }
        return list.ToArray();
    }

    protected void AnalysScript(string scriptData)
    {
        string[] strArray = scriptData.Split(new char[] { '뮿', '﻿', '￾', '\r', '\n', '\0' }, StringSplitOptions.RemoveEmptyEntries);
        List<string> tagDataList = new List<string>();
        List<string> scriptDataList = new List<string>();
        List<int> lineDataList = new List<int>();
        int line = 0;
        int lastMessageIndex = -1;
        string talkName = string.Empty;
    Label_0038:
        if (line < strArray.Length)
        {
            string analysData = strArray[line++];
            if (analysData.Length > 0)
            {
                string item = null;
                string str4 = string.Empty;
                if (!analysData.StartsWith("//") && !analysData.StartsWith(this.codeCommentString))
                {
                    if (analysData.StartsWith(this.codeTalkString))
                    {
                        item = "talkName";
                        str4 = analysData.Substring(1);
                        Debug.Log("script talk name " + str4);
                    }
                    else
                    {
                        if (analysData.StartsWith(this.codeVoiceString))
                        {
                            Debug.Log("script voice " + analysData);
                            goto Label_0038;
                        }
                        this.AnalysText(ref tagDataList, ref scriptDataList, ref lineDataList, ref lastMessageIndex, ref talkName, analysData, line);
                    }
                    if (item != null)
                    {
                        if ((item == "k") || (item == "q"))
                        {
                            if (lastMessageIndex >= 0)
                            {
                                tagDataList.Add(null);
                                scriptDataList.Add("[r]");
                                lineDataList.Add(line);
                                lastMessageIndex = -1;
                            }
                        }
                        else if (item == "talkName")
                        {
                            talkName = (item != "talkName") ? string.Empty : str4;
                        }
                        else if ((((item == "i") || (item == "image")) || ((item == "line") || item.StartsWith("%"))) || (item.StartsWith("&") || item.StartsWith("#")))
                        {
                            if ((talkName != string.Empty) && (lastMessageIndex < 0))
                            {
                                tagDataList.Add("talkStart");
                                scriptDataList.Add(string.Empty);
                                lineDataList.Add(line);
                            }
                            lastMessageIndex = scriptDataList.Count;
                        }
                        tagDataList.Add(item);
                        scriptDataList.Add(str4);
                        lineDataList.Add(line);
                    }
                    else if (str4 != string.Empty)
                    {
                        if ((talkName != string.Empty) && (lastMessageIndex < 0))
                        {
                            tagDataList.Add("talkStart");
                            scriptDataList.Add(string.Empty);
                            lineDataList.Add(line);
                        }
                        tagDataList.Add(null);
                        lastMessageIndex = scriptDataList.Count;
                        scriptDataList.Add(str4.Replace(this.codeReturnString, "[r]"));
                        lineDataList.Add(line);
                    }
                }
            }
            goto Label_0038;
        }
        if (lastMessageIndex >= 0)
        {
            tagDataList.Add(null);
            scriptDataList.Add("[r]");
            lineDataList.Add(line);
        }
        this.executeTagList = tagDataList.ToArray();
        this.executeDataList = scriptDataList.ToArray();
        this.executeLineList = lineDataList.ToArray();
        this.executeIndexMax = this.executeDataList.Length;
    }

    protected void AnalysText(ref List<string> tagDataList, ref List<string> scriptDataList, ref List<int> lineDataList, ref int lastMessageIndex, ref string talkName, string analysData, int line)
    {
        int length = analysData.Length;
        string str = string.Empty;
        int num2 = 0;
        while (num2 < length)
        {
            char ch = analysData[num2++];
            if (ch != '[')
            {
                goto Label_0459;
            }
            string item = null;
            int num3 = 0;
            if (str != string.Empty)
            {
                lastMessageIndex = scriptDataList.Count;
                tagDataList.Add(null);
                scriptDataList.Add(str.Replace(this.codeReturnString, "[r]"));
                lineDataList.Add(line);
            }
            str = string.Empty;
            while (num2 < length)
            {
                ch = analysData[num2++];
                if (ch == ']')
                {
                    if (--num3 < 0)
                    {
                        break;
                    }
                }
                else if (ch == '[')
                {
                    num3++;
                }
                else if ((item == null) && (ch == ' '))
                {
                    item = str;
                }
                str = str + ch;
                if (ch == '"')
                {
                    while (num2 < length)
                    {
                        ch = analysData[num2++];
                        if (ch == '"')
                        {
                            break;
                        }
                        str = str + ch;
                    }
                    str = str + '"';
                }
            }
            if (item == null)
            {
                item = str;
            }
            string key = item;
            if (key != null)
            {
                int num4;
                if (<>f__switch$map1A == null)
                {
                    Dictionary<string, int> dictionary = new Dictionary<string, int>(0x19) {
                        { 
                            "end",
                            0
                        },
                        { 
                            "wait",
                            0
                        },
                        { 
                            "fontSize",
                            0
                        },
                        { 
                            "font",
                            0
                        },
                        { 
                            "f",
                            0
                        },
                        { 
                            "speed",
                            0
                        },
                        { 
                            "s",
                            0
                        },
                        { 
                            "betweenHeight",
                            0
                        },
                        { 
                            "l",
                            0
                        },
                        { 
                            "soundStopAll",
                            0
                        },
                        { 
                            "bgm",
                            0
                        },
                        { 
                            "bgmStop",
                            0
                        },
                        { 
                            "jingle",
                            0
                        },
                        { 
                            "jingleStop",
                            0
                        },
                        { 
                            "se",
                            0
                        },
                        { 
                            "seLoop",
                            0
                        },
                        { 
                            "seStop",
                            0
                        },
                        { 
                            "voice",
                            0
                        },
                        { 
                            "voiceStop",
                            0
                        },
                        { 
                            "clear",
                            1
                        },
                        { 
                            "page",
                            1
                        },
                        { 
                            "wt",
                            2
                        },
                        { 
                            "k",
                            3
                        },
                        { 
                            "q",
                            3
                        },
                        { 
                            string.Empty,
                            4
                        }
                    };
                    <>f__switch$map1A = dictionary;
                }
                if (<>f__switch$map1A.TryGetValue(key, out num4))
                {
                    switch (num4)
                    {
                        case 0:
                        {
                            string str3 = (str.Length <= (item.Length + 1)) ? string.Empty : str.Substring(item.Length + 1);
                            tagDataList.Add(item);
                            scriptDataList.Add(str3);
                            lineDataList.Add(line);
                            goto Label_044E;
                        }
                        case 1:
                            tagDataList.Add(item);
                            scriptDataList.Add((str.Length <= (item.Length + 1)) ? string.Empty : str.Substring(item.Length + 1));
                            lineDataList.Add(line);
                            goto Label_044E;

                        case 2:
                            tagDataList.Add("wait");
                            scriptDataList.Add("time " + ((str.Length <= (item.Length + 1)) ? "0" : str.Substring(item.Length + 1)));
                            lineDataList.Add(line);
                            goto Label_044E;

                        case 3:
                            goto Label_03C5;

                        case 4:
                            goto Label_044E;
                    }
                }
            }
            goto Label_0417;
        Label_03C5:
            if (lastMessageIndex >= 0)
            {
                lastMessageIndex = -1;
            }
            tagDataList.Add(null);
            scriptDataList.Add("[r]");
            lineDataList.Add(line);
            tagDataList.Add(item);
            scriptDataList.Add(string.Empty);
            lineDataList.Add(line);
            goto Label_044E;
        Label_0417:
            lastMessageIndex = scriptDataList.Count;
            tagDataList.Add(null);
            scriptDataList.Add("[" + str + "]");
            lineDataList.Add(line);
        Label_044E:
            str = string.Empty;
            continue;
        Label_0459:
            if (ch == '"')
            {
                str = str + ch;
                while (num2 < length)
                {
                    ch = analysData[num2++];
                    if (ch == '"')
                    {
                        break;
                    }
                    str = str + ch;
                }
                str = str + '"';
                continue;
            }
            str = str + ch;
        }
        if (str != string.Empty)
        {
            lastMessageIndex = scriptDataList.Count;
            tagDataList.Add(null);
            scriptDataList.Add(str.Replace(this.codeReturnString, "[r]"));
            lineDataList.Add(line);
        }
    }

    protected void EndExecuteScript()
    {
        this.state = State.NONE;
        if (this.callbackFunc != null)
        {
            System.Action callbackFunc = this.callbackFunc;
            this.callbackFunc = null;
            callbackFunc();
        }
    }

    public void Init()
    {
        this.codeCommentString = LocalizationManager.Get("SCRIPT_ACTION_CODE_COMMENT");
        this.codeTalkString = LocalizationManager.Get("SCRIPT_ACTION_CODE_TALK");
        this.codeVoiceString = LocalizationManager.Get("SCRIPT_ACTION_CODE_VOICE");
        this.codeReturnString = LocalizationManager.Get("SCRIPT_ACTION_CODE_RETURN");
        base.ResetLongPress();
        base.QuitScreen();
        base.Shake(0f, 0f, 0f, 0f);
        base.OffScreen();
        base.ClearText();
        base.InitScreen();
    }

    protected void ProcessScript(float delta)
    {
        string waitType;
        Dictionary<string, int> dictionary;
        int num4;
        if (this.state == State.NONE)
        {
            return;
        }
    Label_000C:
        if (this.state != State.EXECUTE)
        {
            if (this.state == State.WAIT)
            {
                bool flag2 = true;
                bool flag3 = false;
                waitType = this.waitType;
                if (waitType != null)
                {
                    if (<>f__switch$map1C == null)
                    {
                        dictionary = new Dictionary<string, int>(10) {
                            { 
                                "message",
                                0
                            },
                            { 
                                "message2",
                                1
                            },
                            { 
                                "message3",
                                2
                            },
                            { 
                                "clear",
                                3
                            },
                            { 
                                "clear2",
                                4
                            },
                            { 
                                "page",
                                5
                            },
                            { 
                                "page2",
                                6
                            },
                            { 
                                "page3",
                                7
                            },
                            { 
                                "touch",
                                8
                            },
                            { 
                                "touch2",
                                9
                            }
                        };
                        <>f__switch$map1C = dictionary;
                    }
                    if (<>f__switch$map1C.TryGetValue(waitType, out num4))
                    {
                        switch (num4)
                        {
                            case 0:
                                if (!base.IsReturnScroll())
                                {
                                    if (this.waitTalkName != null)
                                    {
                                        this.SetTalkName(this.waitTalkName);
                                    }
                                    if (this.waitMessage != null)
                                    {
                                        base.AddText(this.waitMessage);
                                    }
                                    else
                                    {
                                        base.AddText(string.Empty);
                                    }
                                    this.isCancelInput = false;
                                    flag2 = false;
                                    if (this.playSpeed == PlaySpeed.FAST)
                                    {
                                        base.MessageUpdate();
                                    }
                                    break;
                                }
                                if (!base.IsBusy)
                                {
                                    if (this.isCancelInput)
                                    {
                                        this.isCancelInput = false;
                                    }
                                    this.waitType = "message2";
                                    flag3 = true;
                                    break;
                                }
                                break;

                            case 1:
                                if (!base.IsWaitTouch())
                                {
                                    base.ReturnScroll(this.playSpeed == PlaySpeed.FAST);
                                    this.waitType = "message3";
                                    flag3 = true;
                                    break;
                                }
                                break;

                            case 2:
                                if (!base.IsScroll())
                                {
                                    if (this.waitMessage != null)
                                    {
                                        base.AddText(this.waitMessage);
                                    }
                                    else
                                    {
                                        base.AddText(string.Empty);
                                    }
                                    flag2 = false;
                                    if (this.playSpeed == PlaySpeed.FAST)
                                    {
                                        base.MessageUpdate();
                                    }
                                    break;
                                }
                                break;

                            case 3:
                                if (!base.IsBusy)
                                {
                                    if (this.isCancelInput)
                                    {
                                        this.isCancelInput = false;
                                    }
                                    else
                                    {
                                        base.WaitNextTouch();
                                    }
                                    this.waitType = "clear2";
                                    flag3 = true;
                                    break;
                                }
                                break;

                            case 4:
                                if (!base.IsWaitTouch())
                                {
                                    base.ClearText();
                                    flag2 = false;
                                    break;
                                }
                                break;

                            case 5:
                                if (!base.IsBusy)
                                {
                                    if (this.isCancelInput)
                                    {
                                        this.isCancelInput = false;
                                    }
                                    else
                                    {
                                        base.WaitNextTouch();
                                    }
                                    this.waitType = "page2";
                                    flag3 = true;
                                    break;
                                }
                                break;

                            case 6:
                                if (!base.IsWaitTouch())
                                {
                                    base.PageScroll(this.playSpeed == PlaySpeed.FAST);
                                    this.waitType = "page3";
                                }
                                break;

                            case 7:
                                if (!base.IsScroll())
                                {
                                    base.ClearText();
                                    flag2 = false;
                                    break;
                                }
                                break;

                            case 8:
                                if (!base.IsBusy)
                                {
                                    base.WaitNextTouch();
                                    this.waitType = "touch2";
                                }
                                break;

                            case 9:
                                if (!base.IsWaitTouch())
                                {
                                    this.isCancelInput = true;
                                    flag2 = false;
                                    break;
                                }
                                break;
                        }
                    }
                }
                if (flag3)
                {
                    goto Label_000C;
                }
                if (!flag2)
                {
                    this.state = State.EXECUTE;
                    goto Label_000C;
                }
            }
            goto Label_08CF;
        }
        if (this.executeIndex >= this.executeIndexMax)
        {
            this.EndExecuteScript();
            goto Label_08CF;
        }
        if (this.playSpeed == PlaySpeed.PAUSE)
        {
            goto Label_08CF;
        }
        string str = this.executeTagList[this.executeIndex];
        string data = this.executeDataList[this.executeIndex];
        int num = this.executeLineList[this.executeIndex];
        string[] strArray = this.AnalysParam(data);
        string str3 = null;
        bool flag = false;
        if ((str == null) && base.IsBusy)
        {
            goto Label_08CF;
        }
        waitType = str;
        if (waitType == null)
        {
            Debug.Log(string.Concat(new object[] { "execute message [", num, "] ", data }));
            this.state = State.WAIT;
            this.waitType = "message";
            this.waitMessage = data;
        }
        else
        {
            if (<>f__switch$map1B == null)
            {
                dictionary = new Dictionary<string, int>(0x10) {
                    { 
                        "end",
                        0
                    },
                    { 
                        "wait",
                        1
                    },
                    { 
                        "messageOn",
                        2
                    },
                    { 
                        "messageOff",
                        3
                    },
                    { 
                        "clear",
                        4
                    },
                    { 
                        "page",
                        5
                    },
                    { 
                        "k",
                        6
                    },
                    { 
                        "q",
                        7
                    },
                    { 
                        "fontSize",
                        8
                    },
                    { 
                        "font",
                        8
                    },
                    { 
                        "f",
                        8
                    },
                    { 
                        "speed",
                        9
                    },
                    { 
                        "s",
                        9
                    },
                    { 
                        "betweenHeight",
                        10
                    },
                    { 
                        "l",
                        10
                    },
                    { 
                        "talkName",
                        11
                    }
                };
                <>f__switch$map1B = dictionary;
            }
            if (<>f__switch$map1B.TryGetValue(waitType, out num4))
            {
                switch (num4)
                {
                    case 0:
                        if (strArray.Length != 0)
                        {
                            str3 = "parameter be unnecessary";
                        }
                        else
                        {
                            this.EndExecuteScript();
                        }
                        goto Label_04DD;

                    case 1:
                        Debug.Log("execute wait " + data);
                        this.state = State.WAIT;
                        delta = 0f;
                        if (strArray.Length >= 1)
                        {
                            this.waitType = strArray[0];
                        }
                        else
                        {
                            str3 = "parameter error";
                        }
                        goto Label_04DD;

                    case 2:
                        if (strArray.Length != 0)
                        {
                            str3 = "parameter error";
                        }
                        else
                        {
                            base.OffScreen();
                            base.ClearText();
                            this.state = State.WAIT;
                            this.waitType = "message";
                            this.waitMessage = null;
                        }
                        goto Label_04DD;

                    case 3:
                        if (strArray.Length != 0)
                        {
                            str3 = "parameter error";
                        }
                        else
                        {
                            base.OffScreen();
                            base.ClearText();
                        }
                        goto Label_04DD;

                    case 4:
                        if (strArray.Length != 0)
                        {
                            str3 = "parameter be unnecessary";
                        }
                        else
                        {
                            this.state = State.WAIT;
                            this.waitType = "clear";
                        }
                        goto Label_04DD;

                    case 5:
                        if (strArray.Length != 0)
                        {
                            str3 = "parameter be unnecessary";
                        }
                        else
                        {
                            this.state = State.WAIT;
                            this.waitType = "page";
                        }
                        goto Label_04DD;

                    case 6:
                        if (strArray.Length != 0)
                        {
                            str3 = "parameter be unnecessary";
                        }
                        else
                        {
                            this.state = State.WAIT;
                            this.waitType = "touch";
                        }
                        goto Label_04DD;

                    case 7:
                        goto Label_04DD;

                    case 8:
                        if (strArray.Length == 1)
                        {
                            base.SetFontSize(strArray[0]);
                        }
                        else if (strArray.Length == 0)
                        {
                            base.SetFontSize(null);
                        }
                        else
                        {
                            str3 = "parameter error";
                        }
                        goto Label_04DD;

                    case 9:
                        if (strArray.Length != 1)
                        {
                            if (strArray.Length == 0)
                            {
                                base.SetSpeed(-1f);
                            }
                            else
                            {
                                str3 = "parameter error";
                            }
                        }
                        else
                        {
                            float n = (strArray[0] != "-") ? float.Parse(strArray[0]) : -1f;
                            base.SetSpeed(n);
                        }
                        goto Label_04DD;

                    case 10:
                        if (strArray.Length != 1)
                        {
                            if (strArray.Length == 0)
                            {
                                base.SetBetweenLineHeight(-1f);
                            }
                            else
                            {
                                str3 = "parameter error";
                            }
                        }
                        else
                        {
                            float height = (strArray[0] != "-") ? float.Parse(strArray[0]) : -1f;
                            base.SetBetweenLineHeight(height);
                        }
                        goto Label_04DD;

                    case 11:
                        Debug.Log(string.Concat(new object[] { "talk name message [", num, "] ", data }));
                        this.waitTalkName = data;
                        goto Label_04DD;
                }
            }
            Debug.LogWarning(string.Concat(new object[] { "ScriptManager execute not support tag [", num, "] ", str, " ", data }));
        }
    Label_04DD:
        if (str3 != null)
        {
            Debug.LogError(string.Concat(new object[] { "ScriptManager ", str3, " [", num, "] ", str, " ", data }));
            this.state = State.ERROR;
        }
        else
        {
            if (!flag)
            {
                this.executeIndex++;
            }
            goto Label_000C;
        }
    Label_08CF:
        if (base.IsBusy)
        {
        }
        base.MessageUpdate();
    }

    public void Quit()
    {
        base.QuitScreen();
        base.ResetLongPress();
        base.QuitScreen();
        base.Shake(0f, 0f, 0f, 0f);
        base.OffScreen();
        base.ClearText();
    }

    public void SetMessageBlock(string messageBlock, System.Action callbackFunc = null)
    {
        this.callbackFunc = callbackFunc;
        this.AnalysScript(messageBlock);
        this.state = State.EXECUTE;
        this.executeIndex = 0;
        this.isCancelInput = false;
    }

    public void SetMessageSpeed(PlaySpeed playSpeed)
    {
        this.playSpeed = playSpeed;
    }

    protected void SetTalkName(string name)
    {
        string str;
        string str2;
        string str3;
        int num;
        ScriptMessageLabel.GetTalkName(out str, out str2, out str3, out num, name);
        base.SetTalkName(null, str2 + str3);
    }

    protected void Update()
    {
        float deltaTime = RealTime.deltaTime;
        this.ProcessScript(deltaTime);
    }

    public enum PlaySpeed
    {
        NONE,
        PAUSE,
        NORMAL,
        FAST
    }

    protected enum State
    {
        NONE,
        IDLE,
        LOAD,
        EXECUTE,
        WAIT,
        WAIT_EXIT,
        EXIT,
        BACK_VIEW_INIT,
        BACK_VIEW,
        FIGURE_VIEW_INIT,
        FIGURE_VIEW,
        ERROR
    }
}

