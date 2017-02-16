using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

public class ScriptManager : SingletonMonoBehaviour<ScriptManager>
{
    [CompilerGenerated]
    private static Dictionary<string, int> <>f__switch$map1E;
    [CompilerGenerated]
    private static Dictionary<string, int> <>f__switch$map1F;
    [CompilerGenerated]
    private static Dictionary<string, int> <>f__switch$map20;
    [CompilerGenerated]
    private static Dictionary<string, int> <>f__switch$map21;
    [CompilerGenerated]
    private static Dictionary<string, int> <>f__switch$map22;
    [CompilerGenerated]
    private static Dictionary<string, int> <>f__switch$map23;
    [CompilerGenerated]
    private static Dictionary<string, int> <>f__switch$map24;
    [CompilerGenerated]
    private static Dictionary<string, int> <>f__switch$map25;
    [CompilerGenerated]
    private static Dictionary<string, int> <>f__switch$map26;
    [CompilerGenerated]
    private static Dictionary<string, int> <>f__switch$map27;
    [CompilerGenerated]
    private static Dictionary<string, int> <>f__switch$map28;
    [CompilerGenerated]
    private static Dictionary<string, int> <>f__switch$map29;
    [CompilerGenerated]
    private static Dictionary<string, int> <>f__switch$map2A;
    [CompilerGenerated]
    private static Dictionary<string, int> <>f__switch$map2B;
    [SerializeField]
    protected UIPanel actionPanel;
    protected AssetData assetData;
    protected string assetName;
    protected List<string> audioAssetList;
    protected int audioLoadIndex;
    [SerializeField]
    protected UIWidget backBase;
    [SerializeField]
    protected GameObject backEffectBase;
    [SerializeField]
    protected UICommonButton backLogButton;
    [SerializeField]
    protected ScriptBackLog backLogDialog;
    protected string backLogTalkEndString;
    protected string backLogTalkStartString;
    protected string backName;
    protected string[] backNameList;
    [SerializeField]
    protected ExUISpriteRenderer backSprite1;
    [SerializeField]
    protected ExUISpriteRenderer backSprite2;
    [SerializeField]
    protected GameObject backViewOperationBase;
    private bool bfinish;
    [SerializeField]
    protected UIPanel blockPanel;
    protected CallbackFunc callbackFunc;
    [SerializeField]
    protected GameObject cameraPosition;
    [SerializeField]
    protected GameObject cameraScale;
    protected RenderTexture captureTexture;
    public static readonly int CHARA_MAX = 8;
    protected ScriptCharaData[] charaList;
    protected string codeCommentString;
    protected string codeLabelString;
    protected string codeReturnString;
    protected string codeSceneString;
    protected string codeSwitchCaseSplitString;
    protected string codeSwitchCaseString;
    protected string codeSwitchEndString;
    protected string codeTalkString;
    protected string codeVoiceString;
    [SerializeField]
    protected GameObject communicationCharaEffectBase;
    public static readonly float DEFAULT_FADE_TIME = 0.5f;
    [SerializeField]
    protected float defaultKeyDelayTime = 0.2f;
    protected List<string> downloadAssetList;
    [SerializeField]
    protected GameObject effectBase;
    [SerializeField]
    protected ItemSeed equipSeed;
    protected static int eventId;
    protected string[] executeDataList;
    protected int executeIndex;
    protected int executeIndexMax;
    protected int[] executeLineList;
    protected int[] executeOrgLineList;
    protected int[] executeSwitchList;
    protected string[] executeTagList;
    protected int executeWaitIndex;
    protected Color fadeColor;
    protected string fadeName;
    [SerializeField]
    protected GameObject fastPlayMark;
    protected int figureFaceNum;
    protected string figureName;
    protected string[] figureNameList;
    [SerializeField]
    protected ItemSeed figureSeed;
    [SerializeField]
    protected GameObject figureViewOperationBase;
    protected List<ScriptFlagData> flagDataList;
    protected Color flashColor1;
    protected Color flashColor2;
    protected int flashCount;
    protected string flashName;
    protected float flashTime1;
    protected float flashTime2;
    protected float flashTime3;
    [SerializeField]
    protected GameObject fowardEffectBase;
    protected AssetData gameDemoAssetData;
    [SerializeField]
    protected ItemSeed imageSeed;
    protected int inputCommandIndex;
    protected InputTopMode inputTopMode;
    protected bool isBackLog;
    protected bool isBusyScene;
    protected bool isBusySceneLoad;
    protected bool isCancelInput;
    protected bool isCollection;
    protected bool isEndRequestFlash;
    protected bool isExecuteCamera;
    protected bool isExecuteFade;
    protected bool isExecuteFlash;
    protected bool isExecuteLoadAsset;
    protected bool isExecuteSkip;
    protected bool isExecuteStretch;
    protected bool isExecuteWipe;
    protected bool isLoadCommunicationChara;
    protected bool isLoadWipe;
    protected bool isMessageOffTalkMode;
    protected static bool isReadScriptSetting;
    protected bool isRequestEffect;
    protected bool isRequestSkip;
    protected bool isRequestVoiceCancel;
    protected bool isSkip;
    protected bool isSkipAudioStop;
    protected bool isSkipExit;
    protected bool isStartModeEnd;
    protected bool isSwithCase;
    protected bool isTalkMask;
    protected bool isWipeFilter;
    protected bool isWipeIn;
    [SerializeField]
    protected ScriptLogMessage logMessage;
    protected SePlayer loopSePlayer;
    protected string[] menuLabelList;
    protected string[] menuMessageList;
    [SerializeField]
    protected ExUIMeshRenderer meshCaptureBase;
    [SerializeField]
    protected UITweenRenderer meshFadeBase;
    [SerializeField]
    protected UITweenRenderer meshFlashBase;
    [SerializeField]
    protected ExUIMeshRenderer meshRenderBase;
    [SerializeField]
    protected ExUIMeshRenderer meshWipeBase;
    [SerializeField]
    protected ScriptMessageManager messageManager;
    [SerializeField]
    protected GameObject normalOperationBase;
    [SerializeField]
    protected ScriptNotificationDialog notificationDialog;
    protected string[] orgScriptList;
    protected PlayMode playMode;
    protected string[] playScriptDataList;
    protected static string playScriptDataName = string.Empty;
    protected PlayScriptDebugMode playScriptDebugMode;
    protected int playScriptJumpLine;
    protected PlaySpeed playSpeed;
    protected static int questId;
    protected static string questMessage = "Dummy";
    protected static string questTitle = "Dummy";
    protected static QuestEntity.enType questType = QuestEntity.enType.EVENT;
    [SerializeField]
    protected UIPanel renderPanel;
    [SerializeField]
    protected Camera renderTextureCamera;
    protected PlaySpeed requestPlaySpeed;
    protected float sceneCrossFadeTime;
    protected int sceneMainIndex;
    public const string SCRIPT_NAME_BATTLE_BASE = "{0:D8}{1:D1}";
    public const string SCRIPT_NAME_BATTLE_END = "{0:D8}{1:D1}1";
    public const string SCRIPT_NAME_BATTLE_START = "{0:D8}{1:D1}0";
    [SerializeField]
    protected UILabel scriptDataLabel;
    protected static int scriptGenderSettingIndex = 1;
    protected string scriptLabel;
    protected static string scriptNotificationMessage = "Dummy";
    protected static string scriptObjectSettingAddress = string.Empty;
    [SerializeField]
    protected GameObject scriptPlayBase;
    protected static string scriptPlayerObjectSettingAddress = string.Empty;
    protected static string scriptPlayerPathSettingAddress = string.Empty;
    [SerializeField]
    protected UILabel scriptPlayLabel;
    [SerializeField]
    protected UISprite scriptPlaySprite;
    [SerializeField]
    protected UIRootReScale scriptReScale;
    protected static string scriptServerSettingAddress = string.Empty;
    protected static string scriptStartModeSettingName = string.Empty;
    [SerializeField]
    protected ScriptSelectDialog selectDialog;
    protected SePlayer sePlayer;
    protected float shakeCycle;
    [SerializeField]
    protected Transform shakeRoot;
    protected float shakeTime;
    protected float shakeX;
    protected float shakeY;
    [SerializeField]
    protected UICommonButton skipButton;
    [SerializeField]
    protected ScriptSkipDialog skipConfirmDialog;
    [SerializeField]
    protected GameObject specialEffectBase;
    protected StartMode startMode;
    protected State state;
    [SerializeField]
    protected UIWidget stretchBase;
    protected Vector2 stretchBaseRange;
    protected string stretchName;
    [SerializeField]
    protected UIPanel systemPanel;
    [SerializeField]
    protected GameObject Talk;
    [SerializeField]
    protected UILabel TalkLab;
    protected string talkMaskName;
    [SerializeField]
    protected UISprite TalkSpr;
    [SerializeField]
    protected UISprite TalkSprText;
    protected static System.Action templateCallbackFunc;
    protected static int templateImageLimitCount;
    protected static bool templateIsFaceFirst;
    protected static int templateSvtId;
    protected static ServantVoiceData[] templateVoiceList;
    public static string textPath = "ScriptActionEncrypt";
    [SerializeField]
    protected GameObject touchFullScreen;
    [SerializeField]
    protected UILabel viewBackLabel;
    [SerializeField]
    protected UILabel viewFaceLabel;
    [SerializeField]
    protected UILabel viewFigureLabel;
    protected SePlayer voicePlayer;
    protected float waitCount;
    protected int waitIndex;
    protected string waitMessage;
    protected string waitName;
    protected string waitType;
    protected static int warId;
    protected AssetData wipeAssetData;
    protected float wipeDuration;
    protected float wipeLevel;
    protected string wipeName;

    private static void _playChapterStart(int warId, CallbackFunc callbackFunc, bool isCollection)
    {
        StartMode start = StartMode.WHITE_FULL;
        if (warId == ConstantMaster.getValue("FIRST_WAR_ID"))
        {
            start = StartMode.WHITE_FULL;
        }
        playScriptDataName = "PlayChapterStart " + warId;
        SingletonMonoBehaviour<ScriptManager>.Instance.PlayLocal(PlayMode.NORMAL, start, "Common", "ChapterStart" + warId, isCollection, false, false, callbackFunc, -1);
    }

    private static void _playGacha(int svtId, int imageLimitCount, bool isFaceFirst, ServantVoiceData[] voiceList, string message, System.Action callbackFunc)
    {
        object[] objArray1 = new object[] { "PlayGacha ", svtId, " ", imageLimitCount };
        playScriptDataName = string.Concat(objArray1);
        templateCallbackFunc = callbackFunc;
        templateSvtId = svtId;
        templateImageLimitCount = imageLimitCount;
        templateVoiceList = voiceList;
        templateIsFaceFirst = isFaceFirst;
        scriptNotificationMessage = message;
        SingletonMonoBehaviour<SoundManager>.Instance.LoadAudioAssetStorage("ChrVoice_" + templateSvtId, new System.Action(ScriptManager.EndLoadPlayGachaVoice), SoundManager.CueType.ALL);
    }

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

    protected void AnalysScript(string scriptData, string orgScriptData)
    {
        string[] strArray;
        List<string> list;
        List<string> list2;
        List<int> list3;
        List<int> list4;
        int num;
        int num2;
        int num3;
        int num4;
        int count;
        string str2;
        int num9;
        PlayMode playMode = this.playMode;
        if (playMode != PlayMode.BACK)
        {
            if (playMode == PlayMode.FIGURE)
            {
                this.figureName = scriptData;
                this.executeTagList = null;
                this.executeDataList = null;
                this.executeLineList = null;
                this.executeOrgLineList = null;
                this.executeIndexMax = 0;
                return;
            }
            strArray = scriptData.Replace("\r", string.Empty).Split(new char[] { '뮿', '﻿', '￾', '\n' }, StringSplitOptions.None);
            list = new List<string>();
            list2 = new List<string>();
            list3 = new List<int>();
            list4 = new List<int>();
            num = -1;
            num2 = 0;
            num3 = -1;
            num4 = -1;
            count = -1;
            str2 = string.Empty;
            if (this.downloadAssetList != null)
            {
                this.downloadAssetList.Clear();
            }
            else
            {
                this.downloadAssetList = new List<string>();
            }
            if (this.audioAssetList != null)
            {
                for (int i = 0; i < this.audioAssetList.Count; i++)
                {
                    SoundManager.releaseAudioAssetStorage(this.audioAssetList[i]);
                }
                this.audioAssetList.Clear();
            }
            else
            {
                this.audioAssetList = new List<string>();
            }
        }
        else
        {
            this.backName = scriptData;
            this.executeTagList = null;
            this.executeDataList = null;
            this.executeLineList = null;
            this.executeOrgLineList = null;
            this.executeIndexMax = 0;
            return;
        }
    Label_014B:
        if (num2 >= strArray.Length)
        {
            if (count >= 0)
            {
                if (str2 != string.Empty)
                {
                    list.Add("talkEnd");
                    list2.Add(string.Empty);
                    list3.Add(num2);
                    list4.Add(num);
                }
                list.Add(null);
                list2.Add("[r]");
                list3.Add(num2);
            }
            this.executeTagList = list.ToArray();
            this.executeDataList = list2.ToArray();
            this.executeLineList = list3.ToArray();
            this.executeOrgLineList = (num < 0) ? null : list4.ToArray();
            this.executeIndexMax = this.executeDataList.Length;
            if (orgScriptData == null)
            {
                this.orgScriptList = null;
                goto Label_0781;
            }
            this.orgScriptList = orgScriptData.Replace("\r", string.Empty).Split(new char[] { '뮿', '﻿', '￾', '\n' }, StringSplitOptions.None);
            if ((this.playScriptJumpLine <= 0) || (this.executeOrgLineList == null))
            {
                goto Label_0781;
            }
            int length = this.executeOrgLineList.Length;
            num9 = -1;
            for (int j = 0; j < length; j++)
            {
                if (this.executeOrgLineList[j] > this.playScriptJumpLine)
                {
                    break;
                }
                num9 = this.executeOrgLineList[j];
            }
        }
        else
        {
            string analysData = strArray[num2++];
            if (analysData.Length > 0)
            {
                string item = null;
                string str5 = string.Empty;
                if (analysData.StartsWith("[="))
                {
                    int index = analysData.IndexOf("]");
                    if (index < 0)
                    {
                        goto Label_014B;
                    }
                    if (index > 2)
                    {
                        num = int.Parse(analysData.Substring(2, index - 2));
                    }
                    if ((index + 1) >= analysData.Length)
                    {
                        goto Label_014B;
                    }
                    analysData = analysData.Substring(index + 1);
                }
                if (!analysData.StartsWith("//") && !analysData.StartsWith(this.codeCommentString))
                {
                    if (analysData.StartsWith(this.codeLabelString))
                    {
                        item = "label";
                        str5 = analysData.Substring(1);
                        Debug.Log("script label " + str5);
                    }
                    else if (analysData.StartsWith(this.codeSceneString))
                    {
                        item = "scene";
                        str5 = analysData.Substring(1);
                        Debug.Log("script scene " + str5);
                    }
                    else if (analysData.StartsWith(this.codeTalkString))
                    {
                        item = "talkName";
                        str5 = analysData.Substring(1);
                        Debug.Log("script talk name " + str5);
                    }
                    else
                    {
                        if (analysData.StartsWith(this.codeVoiceString))
                        {
                            Debug.Log("script voice " + analysData);
                            goto Label_014B;
                        }
                        if (analysData.StartsWith(this.codeSwitchEndString))
                        {
                            if (num3 >= 0)
                            {
                                item = "switchCaseEnd";
                                Debug.Log("switch case end");
                                num3 = -1;
                                num4 = -1;
                            }
                            else
                            {
                                Debug.LogError("switch case message error " + num2);
                            }
                        }
                        else if (analysData.StartsWith(this.codeSwitchCaseString))
                        {
                            item = "switchCase";
                            str5 = analysData.Substring(1);
                            Debug.Log("switch case message " + str5);
                            if (num3 >= 0)
                            {
                                num4++;
                            }
                            else
                            {
                                num3 = list.Count - 1;
                                num4 = 0;
                            }
                        }
                        else
                        {
                            this.AnalysText(ref list, ref list2, ref list3, ref list4, ref count, ref str2, analysData, num2, num);
                        }
                    }
                    switch (item)
                    {
                        case null:
                            if (str5 != string.Empty)
                            {
                                if ((str2 != string.Empty) && (count < 0))
                                {
                                    list.Add("talkStart");
                                    list2.Add(string.Empty);
                                    list3.Add(num2);
                                    list4.Add(num);
                                }
                                list.Add(null);
                                count = list2.Count;
                                list2.Add(str5.Replace(this.codeReturnString, "[r]"));
                                list3.Add(num2);
                                list4.Add(num);
                            }
                            goto Label_014B;

                        case "k":
                        case "q":
                            if (count >= 0)
                            {
                                if (str2 != string.Empty)
                                {
                                    list.Add("talkEnd");
                                    list2.Add(string.Empty);
                                    list3.Add(num2);
                                    list4.Add(num);
                                }
                                list.Add(null);
                                list2.Add("[r]");
                                list3.Add(num2);
                                list4.Add(num);
                                count = -1;
                            }
                            break;

                        case "talkName":
                        case "switchCase":
                        case "switchCaseEnd":
                        case "menu":
                        case "notification":
                        case "input":
                            str2 = (item != "talkName") ? string.Empty : str5;
                            break;

                        default:
                            if ((((item == "talkName") || (item == "switchCase")) || ((item == "switchCaseEnd") || (item == "menu"))) || ((item == "notification") || (item == "input")))
                            {
                                str2 = (item != "talkName") ? string.Empty : str5;
                            }
                            else if ((((item == "i") || (item == "image")) || ((item == "line") || item.StartsWith("%"))) || (item.StartsWith("&") || item.StartsWith("#")))
                            {
                                if ((str2 != string.Empty) && (count < 0))
                                {
                                    list.Add("talkStart");
                                    list2.Add(string.Empty);
                                    list3.Add(num2);
                                    list4.Add(num);
                                }
                                count = list2.Count;
                            }
                            break;
                    }
                    list.Add(item);
                    list2.Add(str5);
                    list3.Add(num2);
                    list4.Add(num);
                }
            }
            goto Label_014B;
        }
        this.playScriptJumpLine = num9;
    Label_0781:
        this.DebugProcessScript(-1, State.NONE);
        if (this.playMode == PlayMode.DEBUG)
        {
            this.playScriptDataList = strArray;
            if (this.playScriptJumpLine >= 0)
            {
                if ((this.executeLineList.Length < 1) || (this.playScriptJumpLine > this.executeLineList[this.executeLineList.Length - 1]))
                {
                    this.playScriptJumpLine = -1;
                }
                else
                {
                    this.playScriptDebugMode = PlayScriptDebugMode.STATE;
                    this.DebugProcessScript(0, this.state);
                }
            }
        }
        else
        {
            this.playScriptJumpLine = -1;
        }
    }

    protected void AnalysText(ref List<string> tagDataList, ref List<string> scriptDataList, ref List<int> lineDataList, ref List<int> orgLineDataList, ref int lastMessageIndex, ref string talkName, string analysData, int line, int orgLine)
    {
        int length = analysData.Length;
        string str = string.Empty;
        int num2 = 0;
        while (num2 < length)
        {
            string str3;
            Dictionary<string, int> dictionary;
            char ch = analysData[num2++];
            if (ch != '[')
            {
                goto Label_0DB6;
            }
            string item = null;
            int num3 = 0;
            if (str != string.Empty)
            {
                if ((talkName != string.Empty) && (lastMessageIndex < 0))
                {
                    tagDataList.Add("talkStart");
                    scriptDataList.Add(string.Empty);
                    lineDataList.Add(line);
                    orgLineDataList.Add(orgLine);
                }
                lastMessageIndex = scriptDataList.Count;
                tagDataList.Add(null);
                scriptDataList.Add(str.Replace(this.codeReturnString, "[r]"));
                lineDataList.Add(line);
                orgLineDataList.Add(orgLine);
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
                int num8;
                if (<>f__switch$map20 == null)
                {
                    dictionary = new Dictionary<string, int>(0x6d) {
                        { 
                            "flag",
                            0
                        },
                        { 
                            "jump",
                            0
                        },
                        { 
                            "branch",
                            0
                        },
                        { 
                            "end",
                            0
                        },
                        { 
                            "skip",
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
                            "shake",
                            0
                        },
                        { 
                            "shakeStop",
                            0
                        },
                        { 
                            "messageShake",
                            0
                        },
                        { 
                            "messageShakeStop",
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
                            "capture",
                            0
                        },
                        { 
                            "captureRelease",
                            0
                        },
                        { 
                            "scene",
                            0
                        },
                        { 
                            "maskin",
                            0
                        },
                        { 
                            "maskout",
                            0
                        },
                        { 
                            "maskOff",
                            0
                        },
                        { 
                            "fadein",
                            0
                        },
                        { 
                            "fadeout",
                            0
                        },
                        { 
                            "fadeMove",
                            0
                        },
                        { 
                            "fadeOff",
                            0
                        },
                        { 
                            "wipein",
                            0
                        },
                        { 
                            "wipeout",
                            0
                        },
                        { 
                            "wipeFilter",
                            0
                        },
                        { 
                            "wipeOff",
                            0
                        },
                        { 
                            "flashin",
                            0
                        },
                        { 
                            "flashout",
                            0
                        },
                        { 
                            "flashOff",
                            0
                        },
                        { 
                            "stretchin",
                            0
                        },
                        { 
                            "stretchout",
                            0
                        },
                        { 
                            "cameraMove",
                            0
                        },
                        { 
                            "cameraHome",
                            0
                        },
                        { 
                            "specialEffect",
                            0
                        },
                        { 
                            "specialEffectStop",
                            0
                        },
                        { 
                            "fowardEffect",
                            0
                        },
                        { 
                            "fowardEffectPause",
                            0
                        },
                        { 
                            "fowardEffectStart",
                            0
                        },
                        { 
                            "fowardEffectStop",
                            0
                        },
                        { 
                            "effect",
                            0
                        },
                        { 
                            "effectStop",
                            0
                        },
                        { 
                            "backEffect",
                            0
                        },
                        { 
                            "backEffectStop",
                            0
                        },
                        { 
                            "charaSet",
                            0
                        },
                        { 
                            "figureSet",
                            0
                        },
                        { 
                            "equipSet",
                            0
                        },
                        { 
                            "imageSet",
                            0
                        },
                        { 
                            "charaChange",
                            0
                        },
                        { 
                            "figureChange",
                            0
                        },
                        { 
                            "equipChange",
                            0
                        },
                        { 
                            "imageChange",
                            0
                        },
                        { 
                            "charaClear",
                            0
                        },
                        { 
                            "charaShadow",
                            0
                        },
                        { 
                            "charaFace",
                            0
                        },
                        { 
                            "charaFilter",
                            0
                        },
                        { 
                            "charaTalk",
                            0
                        },
                        { 
                            "charaFadein",
                            0
                        },
                        { 
                            "charaFadeout",
                            0
                        },
                        { 
                            "charaPut",
                            0
                        },
                        { 
                            "charaScale",
                            0
                        },
                        { 
                            "charaMove",
                            0
                        },
                        { 
                            "charaReturn",
                            0
                        },
                        { 
                            "charaMoveReturn",
                            0
                        },
                        { 
                            "charaAttack",
                            0
                        },
                        { 
                            "charaShake",
                            0
                        },
                        { 
                            "charaShakeStop",
                            0
                        },
                        { 
                            "charaDepth",
                            0
                        },
                        { 
                            "charaSpecialEffect",
                            0
                        },
                        { 
                            "charaSpecialEffectStop",
                            0
                        },
                        { 
                            "charaEffect",
                            0
                        },
                        { 
                            "charaEffectStop",
                            0
                        },
                        { 
                            "charaBackEffect",
                            0
                        },
                        { 
                            "charaBackEffectStop",
                            0
                        },
                        { 
                            "charaCutin",
                            0
                        },
                        { 
                            "charaCutout",
                            0
                        },
                        { 
                            "charaCutStop",
                            0
                        },
                        { 
                            "communicationChara",
                            0
                        },
                        { 
                            "communicationCharaLoop",
                            0
                        },
                        { 
                            "communicationCharaFace",
                            0
                        },
                        { 
                            "communicationCharaStop",
                            0
                        },
                        { 
                            "communicationCharaClear",
                            0
                        },
                        { 
                            "screen",
                            1
                        },
                        { 
                            "messageOn",
                            1
                        },
                        { 
                            "messageOff",
                            1
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
                            "menu",
                            1
                        },
                        { 
                            "notification",
                            1
                        },
                        { 
                            "input",
                            1
                        },
                        { 
                            "label",
                            2
                        },
                        { 
                            "wt",
                            3
                        },
                        { 
                            "k",
                            4
                        },
                        { 
                            "q",
                            4
                        },
                        { 
                            string.Empty,
                            5
                        }
                    };
                    <>f__switch$map20 = dictionary;
                }
                if (<>f__switch$map20.TryGetValue(key, out num8))
                {
                    switch (num8)
                    {
                        case 0:
                            goto Label_076B;

                        case 1:
                            goto Label_0B26;

                        case 2:
                            if (str.Length > (item.Length + 1))
                            {
                                tagDataList.Add(item);
                                scriptDataList.Add(this.ConvertNameString(str.Substring(item.Length + 1)));
                                lineDataList.Add(line);
                                orgLineDataList.Add(orgLine);
                            }
                            goto Label_0DAB;

                        case 3:
                            tagDataList.Add("wait");
                            scriptDataList.Add("time " + ((str.Length <= (item.Length + 1)) ? "0" : str.Substring(item.Length + 1)));
                            lineDataList.Add(line);
                            orgLineDataList.Add(orgLine);
                            goto Label_0DAB;

                        case 4:
                            goto Label_0C81;

                        case 5:
                            goto Label_0DAB;
                    }
                }
            }
            goto Label_0D24;
        Label_076B:
            str3 = (str.Length <= (item.Length + 1)) ? string.Empty : str.Substring(item.Length + 1);
            tagDataList.Add(item);
            scriptDataList.Add(str3);
            lineDataList.Add(line);
            orgLineDataList.Add(orgLine);
            string[] strArray = this.AnalysParam(str3);
            string[] assetNameList = null;
            string assetName = null;
            string str5 = null;
            string str7 = item;
            if (str7 != null)
            {
                int num9;
                if (<>f__switch$map1F == null)
                {
                    dictionary = new Dictionary<string, int>(0x12) {
                        { 
                            "scene",
                            0
                        },
                        { 
                            "charaSet",
                            1
                        },
                        { 
                            "figureSet",
                            1
                        },
                        { 
                            "charaChange",
                            1
                        },
                        { 
                            "figureChange",
                            1
                        },
                        { 
                            "equipSet",
                            2
                        },
                        { 
                            "equipChange",
                            2
                        },
                        { 
                            "imageSet",
                            3
                        },
                        { 
                            "imageChange",
                            3
                        },
                        { 
                            "fowardEffect",
                            4
                        },
                        { 
                            "effect",
                            4
                        },
                        { 
                            "backEffect",
                            4
                        },
                        { 
                            "charaEffect",
                            5
                        },
                        { 
                            "charaBackEffect",
                            5
                        },
                        { 
                            "se",
                            6
                        },
                        { 
                            "seLoop",
                            6
                        },
                        { 
                            "voice",
                            7
                        },
                        { 
                            "soundStopAll",
                            8
                        }
                    };
                    <>f__switch$map1F = dictionary;
                }
                if (<>f__switch$map1F.TryGetValue(str7, out num9))
                {
                    switch (num9)
                    {
                        case 0:
                            assetName = this.ConvertBackTextureName(strArray[0]);
                            break;

                        case 1:
                            assetNameList = UIStandFigureRender.GetAssetNameList(this.ConvertNameString(strArray[2]));
                            break;

                        case 2:
                            assetNameList = UIEquipGraphRender.GetAssetNameList(this.ConvertNameString(strArray[2]));
                            break;

                        case 3:
                            assetNameList = UIImageRender.GetAssetNameList(this.ConvertNameString(strArray[2]));
                            break;

                        case 4:
                            assetName = CommonEffectManager.GetAssetName(this.ConvertTalkEffectName(strArray[0]));
                            break;

                        case 5:
                            assetName = CommonEffectManager.GetAssetName(this.ConvertTalkEffectName(strArray[2]));
                            break;

                        case 6:
                            str5 = SoundManager.getAssetName(this.ConvertNameString(strArray[0]));
                            break;

                        case 7:
                            str5 = SoundManager.getCharaVoiceAssetName(this.ConvertNameString(strArray[0]));
                            break;

                        case 8:
                            this.isSkipAudioStop = true;
                            break;
                    }
                }
            }
            if (str5 != null)
            {
                for (int i = 0; i < this.audioAssetList.Count; i++)
                {
                    if (assetName == this.audioAssetList[i])
                    {
                        assetName = null;
                        break;
                    }
                }
                if (str5 != null)
                {
                    this.audioAssetList.Add(str5);
                    this.downloadAssetList.Add(SoundManager.getDownloadAssetName(str5));
                }
            }
            if (assetName != null)
            {
                for (int j = 0; j < this.downloadAssetList.Count; j++)
                {
                    if (assetName == this.downloadAssetList[j])
                    {
                        assetName = null;
                        break;
                    }
                }
                if (assetName != null)
                {
                    this.downloadAssetList.Add(assetName);
                }
            }
            if (assetNameList != null)
            {
                for (int k = 0; k < assetNameList.Length; k++)
                {
                    assetName = assetNameList[k];
                    for (int m = 0; m < this.downloadAssetList.Count; m++)
                    {
                        if (assetName == this.downloadAssetList[m])
                        {
                            assetName = null;
                            break;
                        }
                    }
                    if (assetName != null)
                    {
                        this.downloadAssetList.Add(assetName);
                    }
                }
            }
            goto Label_0DAB;
        Label_0B26:
            if ((item != "clear") && (item != "page"))
            {
                talkName = string.Empty;
            }
            if ((item == "input") && !this.isCollection)
            {
                this.inputCommandIndex = tagDataList.Count;
            }
            tagDataList.Add(item);
            scriptDataList.Add((str.Length <= (item.Length + 1)) ? string.Empty : str.Substring(item.Length + 1));
            lineDataList.Add(line);
            orgLineDataList.Add(orgLine);
            goto Label_0DAB;
        Label_0C81:
            if (lastMessageIndex >= 0)
            {
                if (talkName != string.Empty)
                {
                    tagDataList.Add("talkEnd");
                    scriptDataList.Add(string.Empty);
                    lineDataList.Add(line);
                    orgLineDataList.Add(orgLine);
                }
                lastMessageIndex = -1;
            }
            tagDataList.Add(null);
            scriptDataList.Add("[r]");
            lineDataList.Add(line);
            orgLineDataList.Add(orgLine);
            tagDataList.Add(item);
            scriptDataList.Add(string.Empty);
            lineDataList.Add(line);
            orgLineDataList.Add(orgLine);
            goto Label_0DAB;
        Label_0D24:
            if ((talkName != string.Empty) && (lastMessageIndex < 0))
            {
                tagDataList.Add("talkStart");
                scriptDataList.Add(string.Empty);
                lineDataList.Add(line);
                orgLineDataList.Add(orgLine);
            }
            lastMessageIndex = scriptDataList.Count;
            tagDataList.Add(null);
            scriptDataList.Add("[" + str + "]");
            lineDataList.Add(line);
            orgLineDataList.Add(orgLine);
        Label_0DAB:
            str = string.Empty;
            continue;
        Label_0DB6:
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
            if ((talkName != string.Empty) && (lastMessageIndex < 0))
            {
                tagDataList.Add("talkStart");
                scriptDataList.Add(string.Empty);
                lineDataList.Add(line);
                orgLineDataList.Add(orgLine);
            }
            lastMessageIndex = scriptDataList.Count;
            tagDataList.Add(null);
            scriptDataList.Add(str.Replace(this.codeReturnString, "[r]"));
            lineDataList.Add(line);
            orgLineDataList.Add(orgLine);
        }
    }

    public static void BackViewClear()
    {
        SingletonMonoBehaviour<ScriptManager>.Instance.backName = null;
    }

    public static void BackViewPlay(string backName, string[] backNameList, CallbackFunc callbackFunc)
    {
        SingletonMonoBehaviour<ScriptManager>.Instance.PlayLocal(PlayMode.BACK, StartMode.NONE, backName, null, backNameList, 0, callbackFunc, -1);
    }

    public void closeTalk(Coroutine Talkdate = null)
    {
        this.TalkLab.text = null;
        if (Talkdate != null)
        {
            base.StopCoroutine(Talkdate);
        }
        else
        {
            base.StopCoroutine(this.settalk(0x3e7, null, 0f));
        }
        this.Talk.SetActive(false);
    }

    protected string ConvertBackTextureName(string s) => 
        $"Back/back{int.Parse(s):D5}";

    protected int ConvertCharaIndex(string s)
    {
        int num = 0;
        for (int i = 0; i < s.Length; i++)
        {
            int num3 = s[i];
            if ((num3 < 0x41) || (num3 > 90))
            {
                return num;
            }
            num = (num << 8) | (num3 - 0x41);
        }
        return num;
    }

    protected int ConvertCharaIndexTalk(string s)
    {
        string str = this.ConvertNameString(s);
        switch (str)
        {
            case "on":
                return -1;

            case "off":
                return -1;
        }
        for (int i = 0; i < this.charaList.Length; i++)
        {
            ScriptCharaData data = this.charaList[i];
            if ((data != null) && (data.TalkName == str))
            {
                return i;
            }
        }
        return this.ConvertCharaIndex(s);
    }

    protected Color ConvertColor(string s)
    {
        int num = Convert.ToInt32(s, 0x10);
        if (s.Length == 6)
        {
            return new Color(((float) ((num >> 0x10) & 0xff)) / 255f, ((float) ((num >> 8) & 0xff)) / 255f, ((float) (num & 0xff)) / 255f);
        }
        if (s.Length == 8)
        {
            return new Color(((float) ((num >> 0x18) & 0xff)) / 255f, ((float) ((num >> 0x10) & 0xff)) / 255f, ((float) ((num >> 8) & 0xff)) / 255f, ((float) (num & 0xff)) / 255f);
        }
        return Color.black;
    }

    protected string ConvertNameString(string s)
    {
        if (s == null)
        {
            return string.Empty;
        }
        if (((s.Length > 2) && (s[0] == '"')) && (s[s.Length - 1] == '"'))
        {
            return s.Substring(1, s.Length - 2);
        }
        return s;
    }

    protected string ConvertTalkEffectName(string s)
    {
        if (s == null)
        {
            return null;
        }
        if (((s.Length > 2) && (s[0] == '"')) && (s[s.Length - 1] == '"'))
        {
            return ("Talk/" + s.Substring(1, s.Length - 2));
        }
        return ("Talk/" + s);
    }

    public static void DebugPlay(string startModeName, string name, string label, int jumpLine, CallbackFunc callbackFunc)
    {
        playScriptDataName = "DebugPlay " + name + " " + label;
        scriptNotificationMessage = "Test";
        SingletonMonoBehaviour<ScriptManager>.Instance.PlayLocal(PlayMode.DEBUG, GetStartMode(startModeName), name, label, true, true, true, callbackFunc, jumpLine);
    }

    public static void DebugPlay(string startModeName, string data, string orgData, int genderIndex, int jumpLine, CallbackFunc callbackFunc)
    {
        playScriptDataName = "DebugPlay data";
        scriptNotificationMessage = "Test";
        SingletonMonoBehaviour<ScriptManager>.Instance.PlayLocal(PlayMode.DEBUG, GetStartMode(startModeName), data, orgData, null, genderIndex, callbackFunc, jumpLine);
    }

    protected void DebugProcessScript(int executeIndex = -1, State state = 0)
    {
        int num = -1;
        int num2 = -1;
        if (((this.playScriptDebugMode != PlayScriptDebugMode.NONE) && (this.playScriptDataList != null)) && (executeIndex >= 0))
        {
            this.scriptPlayBase.SetActive(true);
            if (this.executeOrgLineList != null)
            {
                num = this.executeLineList[executeIndex];
                num2 = this.executeOrgLineList[executeIndex];
                this.scriptPlayLabel.text = $"line {num2 + 1:D4}[{num + 1:D4}]";
            }
            else if (this.executeLineList != null)
            {
                num = this.executeLineList[executeIndex];
                this.scriptPlayLabel.text = $"line {num + 1:D4}";
            }
            else
            {
                this.scriptPlayLabel.text = "line ----";
            }
        }
        else
        {
            this.scriptPlayBase.SetActive(false);
            return;
        }
        if ((this.playScriptDebugMode != PlayScriptDebugMode.ALL) || (num < 0))
        {
            this.scriptDataLabel.gameObject.SetActive(false);
            this.scriptPlaySprite.gameObject.SetActive(false);
        }
        else
        {
            StringBuilder builder = new StringBuilder();
            int num3 = 10;
            if (this.executeOrgLineList != null)
            {
                num = this.executeOrgLineList[executeIndex];
                int length = this.executeOrgLineList.Length;
                int index = num - 10;
                if (num < 10)
                {
                    num3 = num;
                    index = 0;
                }
                else if ((num + 10) >= length)
                {
                    num3 = (num + 20) - length;
                    index = length - 20;
                }
                for (int i = 0; i < 20; i++)
                {
                    if (index >= length)
                    {
                        break;
                    }
                    if (i > 0)
                    {
                        builder.Append("\n");
                    }
                    builder.AppendFormat("{0:D4} ", index + 1);
                    if (this.orgScriptList[index].Length > 0x2a)
                    {
                        builder.Append(this.orgScriptList[index].Substring(0, 0x2a) + "…");
                    }
                    else
                    {
                        builder.Append(this.orgScriptList[index]);
                    }
                    index++;
                }
            }
            else
            {
                int num7 = this.playScriptDataList.Length;
                int num8 = num - 10;
                if (num < 10)
                {
                    num3 = num;
                    num8 = 0;
                }
                else if ((num + 10) >= num7)
                {
                    num3 = (num + 20) - num7;
                    num8 = num7 - 20;
                }
                for (int j = 0; j < 20; j++)
                {
                    if (num8 >= num7)
                    {
                        break;
                    }
                    if (j > 0)
                    {
                        builder.Append("\n");
                    }
                    builder.AppendFormat("{0:D4} ", num8 + 1);
                    if (this.playScriptDataList[num8].Length > 0x2a)
                    {
                        builder.Append(this.playScriptDataList[num8].Substring(0, 0x2a) + "…");
                    }
                    else
                    {
                        builder.Append(this.playScriptDataList[num8]);
                    }
                    num8++;
                }
            }
            this.scriptDataLabel.gameObject.SetActive(true);
            this.scriptDataLabel.text = builder.ToString();
            this.scriptPlaySprite.gameObject.SetActive(true);
            this.scriptPlaySprite.transform.localPosition = new Vector3(0f, (float) (0x1b - (num3 * 0x18)));
        }
    }

    protected void EndCloseNameInput()
    {
        string str;
        int num;
        int num2;
        int num3;
        SingletonMonoBehaviour<NetworkManager>.Instance.GetSignup(out str, out num, out num2, out num3);
        Debug.Log(string.Concat(new object[] { "userName ", str, " genderType ", num }));
        ScriptReplaceString.SetString(1, str);
        ScriptReplaceString.SetPlayerGenderIndex(num);
        this.backLogDialog.ClearLog();
        this.messageManager.ClearText();
        this.InitEffect();
        this.messageManager.Shake(0f, 0f, 0f, 0f);
        this.Shake(0f, 0f, 0f, 0f);
        this.InitCamera();
        this.InitStretch();
        this.InitFlash();
        this.InitWipe();
        this.InitFade();
        this.meshRenderBase.SetTweenColor(new Color(1f, 1f, 1f, 1f));
        this.ResumePlaySpeed();
        this.EndNameInput();
    }

    protected void EndCloseNotificationDialog()
    {
        this.ResumePlaySpeed();
        this.inputTopMode = InputTopMode.NORMAL;
        this.waitType = "notification3";
    }

    protected void EndDownloadAssetData()
    {
        this.isExecuteLoadAsset = true;
        this.audioLoadIndex = 0;
        this.LoadAudioAssetData();
    }

    protected void EndExecuteCamera()
    {
        this.isExecuteCamera = false;
    }

    protected void EndExecuteFade()
    {
        this.fadeName = null;
        this.isExecuteFade = false;
    }

    protected void EndExecuteFlash()
    {
        this.flashName = null;
        this.isExecuteFlash = false;
        this.isEndRequestFlash = false;
        this.meshFlashBase.SetTweenColor(Color.clear);
    }

    protected void EndExecuteScript()
    {
        Debug.Log("execute end");
        if (this.startMode == StartMode.BLACK_SCENE_STOP)
        {
            SingletonMonoBehaviour<SceneManager>.Instance.SetTargetRootActive(true);
        }
        switch (this.startMode)
        {
            case StartMode.CLEAR_BLACK:
            case StartMode.BLACK:
            case StartMode.BLACK_FULL:
            case StartMode.CLEAR_BLACK_FULL:
            case StartMode.BLACK_SCENE_STOP:
                if (SingletonMonoBehaviour<CommonUI>.Instance.maskFadGetFadeoutKind() != MaskFade.Kind.BLACK)
                {
                    if (this.GetFadeoutType() == MaskFade.Kind.BLACK)
                    {
                        SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, 0f, null);
                        this.state = State.WAIT_EXIT;
                    }
                    else
                    {
                        SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, DEFAULT_FADE_TIME, null);
                        this.state = State.WAIT_EXIT;
                    }
                    return;
                }
                this.state = State.WAIT_EXIT;
                return;

            case StartMode.CLEAR_WHITE:
            case StartMode.WHITE:
            case StartMode.WHITE_FULL:
            case StartMode.CLEAR_WHITE_FULL:
                if (SingletonMonoBehaviour<CommonUI>.Instance.maskFadGetFadeoutKind() != MaskFade.Kind.WHITE)
                {
                    if (this.GetFadeoutType() == MaskFade.Kind.WHITE)
                    {
                        SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.WHITE, 0f, null);
                        this.state = State.WAIT_EXIT;
                    }
                    else
                    {
                        SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.WHITE, DEFAULT_FADE_TIME, null);
                        this.state = State.WAIT_EXIT;
                    }
                    return;
                }
                this.state = State.WAIT_EXIT;
                return;

            case StartMode.BLACK_CLEAR:
            case StartMode.WHITE_CLEAR:
            case StartMode.CLEAR:
            case StartMode.CLEAR_FULL:
                this.StartFade("clear", false, 1f);
                this.state = State.WAIT_EXIT;
                return;
        }
        this.state = State.WAIT_EXIT;
    }

    protected void EndExecuteStretch()
    {
        this.stretchName = null;
        this.isExecuteStretch = false;
    }

    protected void EndExecuteWipe()
    {
        this.wipeName = null;
        this.isExecuteWipe = false;
    }

    protected void EndFadeoutNameInput()
    {
        this.PausePlaySpeed();
        SoundManager.playBgm("BGM_EVENT_2", 1f, 0.1f);
        SingletonMonoBehaviour<CommonUI>.Instance.OpenUserNameEntry(new System.Action(this.EndCloseNameInput));
    }

    protected void EndFirstLoadAsset(AssetData loadData)
    {
        if (this.assetData != null)
        {
            AssetManager.releaseAsset(this.assetData);
        }
        this.assetData = loadData;
        this.FirstExecuteScript();
    }

    protected void EndFlash(float duration)
    {
        if (this.isExecuteFlash)
        {
            this.isEndRequestFlash = true;
            this.flashTime3 = duration;
        }
    }

    protected void EndLoadAsset(AssetData loadData)
    {
        this.state = State.IDLE;
        if (this.assetData != null)
        {
            AssetManager.releaseAsset(this.assetData);
        }
        this.assetData = loadData;
        this.ExecuteScript(this.scriptLabel, null, null);
    }

    protected static void EndLoadPlatGachaTemplate(AssetData data)
    {
        if (data != null)
        {
            string decryptObjectText = data.GetDecryptObjectText("GachaTemplate");
            int face = 0;
            string str2 = string.Empty;
            if (templateVoiceList != null)
            {
                if (templateIsFaceFirst && (templateVoiceList.Length > 0))
                {
                    face = templateVoiceList[0].face;
                }
                for (int i = 0; i < templateVoiceList.Length; i++)
                {
                    string str4;
                    ServantVoiceData data2 = templateVoiceList[i];
                    if (data2.delay > 0f)
                    {
                        str4 = str2;
                        object[] objArray1 = new object[] { str4, "[wait time ", data2.delay, "]\n" };
                        str2 = string.Concat(objArray1);
                    }
                    if (string.IsNullOrEmpty(data2.text))
                    {
                        str4 = str2;
                        object[] objArray2 = new object[] { str4, "[charaFace A ", data2.face, "]\n" };
                        str2 = string.Concat(objArray2);
                    }
                    else
                    {
                        str4 = str2;
                        object[] objArray3 = new object[] { str4, "[charaFace A ", data2.face, "]", data2.text, "\n" };
                        str2 = string.Concat(objArray3);
                    }
                    str4 = str2;
                    object[] objArray4 = new object[] { str4, "[voice ", templateSvtId, "_", data2.id, "]\n[wait voiceCancel]\n" };
                    str2 = string.Concat(objArray4);
                }
            }
            string str3 = string.Format(decryptObjectText, string.Empty + templateSvtId + templateImageLimitCount, string.Empty + face, str2);
            AssetManager.releaseAsset(data);
            SingletonMonoBehaviour<ScriptManager>.Instance.PlayLocal(PlayMode.NORMAL, StartMode.THROUGH, str3, false, false, false, new CallbackFunc(ScriptManager.EndPlayGachaPlayLocal), -1);
        }
        else
        {
            System.Action templateCallbackFunc = ScriptManager.templateCallbackFunc;
            ScriptManager.templateCallbackFunc = null;
            if (templateCallbackFunc != null)
            {
                templateCallbackFunc();
            }
        }
    }

    protected static void EndLoadPlayGachaVoice()
    {
        AssetManager.loadAssetStorage(textPath + "/Common", new AssetLoader.LoadEndDataHandler(ScriptManager.EndLoadPlatGachaTemplate));
    }

    protected void EndLoadWipe(AssetData data)
    {
        this.isLoadWipe = false;
        if (this.isExecuteWipe)
        {
            if (data == null)
            {
                Debug.LogError("wipe asset error [" + base.name + "]");
                this.EndExecuteWipe();
            }
            else
            {
                this.meshWipeBase.SetImage(data.GetObject<Texture2D>());
                if (this.wipeAssetData != null)
                {
                    AssetManager.releaseAsset(this.wipeAssetData);
                }
                this.wipeAssetData = data;
                this.meshWipeBase.SetTweenColor(Color.black);
                float num = !this.meshWipeBase.material.HasProperty("_Gradation") ? -1f : this.wipeLevel;
                float num2 = (num <= 0f) ? 0f : num;
                float num3 = num2 + 1.003922f;
                if (this.isWipeFilter)
                {
                    if (num >= 0f)
                    {
                        this.meshWipeBase.material.SetFloat("_Gradation", num2);
                    }
                    this.meshWipeBase.SetTweenVolume(num3 * this.wipeDuration);
                }
                else
                {
                    if (this.wipeDuration > 0f)
                    {
                        if (num >= 0f)
                        {
                            this.meshWipeBase.material.SetFloat("_Gradation", num2);
                        }
                        this.meshWipeBase.SetTweenVolume(!this.isWipeIn ? 0f : num3);
                        TweenRenderVolume volume = TweenRenderVolume.Begin(this.meshWipeBase.gameObject, this.wipeDuration, !this.isWipeIn ? num3 : 0f);
                        if (volume != null)
                        {
                            volume.eventReceiver = base.gameObject;
                            volume.callWhenFinished = "EndExecuteWipe";
                            return;
                        }
                    }
                    this.meshWipeBase.SetTweenVolume(!this.isWipeIn ? num3 : 0f);
                    this.EndExecuteWipe();
                }
            }
        }
    }

    protected void EndNameInput()
    {
        this.inputTopMode = InputTopMode.NORMAL;
        this.waitType = "inputname3";
    }

    protected void EndNotificationDialog()
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        this.notificationDialog.Close(new System.Action(this.EndCloseNotificationDialog));
    }

    protected static void EndPlayGachaPlayLocal(bool isExit)
    {
        SingletonMonoBehaviour<SoundManager>.Instance.ReleaseAudioAssetStorage("ChrVoice_" + templateSvtId);
        System.Action templateCallbackFunc = ScriptManager.templateCallbackFunc;
        ScriptManager.templateCallbackFunc = null;
        if (templateCallbackFunc != null)
        {
            templateCallbackFunc();
        }
    }

    protected void EndSelectSwitchCase(int index)
    {
        this.selectDialog.Close();
        this.menuMessageList = null;
        this.isCancelInput = true;
        this.JumpScriptSwitchCase(index);
        this.ResumePlaySpeed();
        this.state = State.WAIT;
        this.waitType = "time";
        this.waitCount = this.defaultKeyDelayTime;
    }

    protected void EndSkipConfirm(ScriptSkipDialog.ResultKind result)
    {
        this.inputTopMode = InputTopMode.NORMAL;
        if (result != ScriptSkipDialog.ResultKind.CANCEL)
        {
            this.isRequestSkip = true;
            this.isSkipExit = result == ScriptSkipDialog.ResultKind.EXIT;
        }
        this.skipConfirmDialog.Close();
        this.ResumePlaySpeed();
    }

    protected void ExecuteScript(string label, string scriptData = null, string orgScriptData = null)
    {
        if (scriptData == null)
        {
            scriptData = this.GetExecuteScriptText(label);
            if (scriptData == null)
            {
                Debug.LogError("execute script empty [" + this.assetName + "," + label + "]");
                return;
            }
            Debug.Log("execute script [" + this.assetName + "," + label + "]");
        }
        else
        {
            Debug.Log("execute script template [" + label + "]");
        }
        this.actionPanel.gameObject.SetActive(true);
        this.renderPanel.gameObject.SetActive(true);
        this.meshRenderBase.SetTweenColor(Color.clear);
        this.RenderCameraInit();
        int x = this.messageManager.InitScreen();
        this.logMessage.SetHomePosition(x);
        this.logMessage.ClearText();
        this.scriptLabel = label;
        this.AnalysScript(scriptData, orgScriptData);
        this.ModifyPlaySpeed(true);
        this.JumpScript((string) null);
    }

    protected void ExitExecuteScript()
    {
        this.state = State.NONE;
        this.skipConfirmDialog.Init();
        this.backLogDialog.Close();
        this.backLogDialog.ClearLog();
        this.logMessage.ClearText();
        this.ReleaseAsset();
        this.messageManager.QuitScreen();
        this.InitEffect();
        this.messageManager.Shake(0f, 0f, 0f, 0f);
        this.Shake(0f, 0f, 0f, 0f);
        this.InitCamera();
        this.InitStretch();
        this.InitFlash();
        this.InitWipe();
        this.InitFade();
        this.backNameList = null;
        this.figureNameList = null;
        this.figureFaceNum = 0;
        this.blockPanel.gameObject.SetActive(false);
        this.actionPanel.gameObject.SetActive(false);
        this.renderPanel.gameObject.SetActive(false);
        if (this.isSkipAudioStop)
        {
            this.SoundStopAll();
        }
        this.DebugProcessScript(-1, State.NONE);
        if (this.callbackFunc != null)
        {
            CallbackFunc callbackFunc = this.callbackFunc;
            this.callbackFunc = null;
            callbackFunc(this.isSkipExit);
        }
        SingletonTemplate<MissionNotifyManager>.Instance.EndPause();
    }

    public static bool Fade(string name, bool isIn, float duration) => 
        SingletonMonoBehaviour<ScriptManager>.Instance.StartFade(name, isIn, duration);

    public static void FigureViewClear()
    {
        SingletonMonoBehaviour<ScriptManager>.Instance.figureName = null;
    }

    public static void FigureViewPlay(string figureName, string[] figureNameList, CallbackFunc callbackFunc)
    {
        SingletonMonoBehaviour<ScriptManager>.Instance.PlayLocal(PlayMode.FIGURE, StartMode.NONE, figureName, null, figureNameList, 0, callbackFunc, -1);
    }

    protected void FirstExecuteScript()
    {
        string scriptData = null;
        if (playScriptDataName == "PlayQuestStart")
        {
            string label = this.scriptLabel + "Q" + questId;
            scriptData = this.GetExecuteScriptText(label);
            if (scriptData == null)
            {
                if (eventId > 0)
                {
                    label = this.scriptLabel + "E" + eventId;
                    scriptData = this.GetExecuteScriptText(label);
                }
                if ((scriptData == null) && (warId > 0))
                {
                    label = this.scriptLabel + "W" + warId;
                    scriptData = this.GetExecuteScriptText(label);
                }
            }
            if (scriptData != null)
            {
                this.scriptLabel = label;
            }
        }
        if (scriptData == null)
        {
            scriptData = this.GetExecuteScriptText(this.scriptLabel);
        }
        if (scriptData == null)
        {
            scriptData = "[end]";
            this.isSkip = false;
            switch (this.startMode)
            {
                case StartMode.CLEAR_BLACK:
                case StartMode.BLACK:
                case StartMode.BLACK_SCENE_STOP:
                    this.startMode = StartMode.BLACK_FULL;
                    break;

                case StartMode.CLEAR_WHITE:
                case StartMode.WHITE:
                    this.startMode = StartMode.WHITE_FULL;
                    break;

                case StartMode.BLACK_CLEAR:
                case StartMode.WHITE_CLEAR:
                case StartMode.CLEAR:
                    this.startMode = StartMode.CLEAR_FULL;
                    break;
            }
        }
        this.InitSetting();
        ScriptReplaceString.Init();
        if (NetworkManager.IsLogin)
        {
            UserGameEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_GAME).getSingleEntity<UserGameEntity>();
            if (entity != null)
            {
                ScriptReplaceString.SetString(1, entity.name);
                ScriptReplaceString.SetPlayerGenderIndex(entity.genderType);
            }
        }
        ScriptReplaceString.SetString(2, questTitle);
        ScriptReplaceString.SetString(3, questMessage);
        this.state = State.IDLE;
        this.ExecuteScript(this.scriptLabel, scriptData, null);
    }

    protected object GetEffectParameter(string effectName)
    {
        if (effectName.StartsWith("Talk/bit_queststart"))
        {
            return new CommonEffectParam { 
                title = questTitle,
                message = questMessage,
                type = (int) questType
            };
        }
        return null;
    }

    protected string GetExecuteScriptText(string label)
    {
        string decryptObjectText = this.assetData?.GetDecryptObjectText(label);
        if (decryptObjectText != null)
        {
            playScriptDataName = "AssetData " + label;
        }
        return decryptObjectText;
    }

    protected MaskFade.Kind GetFadeoutType()
    {
        if (!this.isExecuteFade)
        {
            Color fadeColor = this.fadeColor;
            if (fadeColor.a >= 1f)
            {
                fadeColor.a = 0f;
                if (this.fadeColor == Color.black)
                {
                    return MaskFade.Kind.BLACK;
                }
                if (this.fadeColor == Color.white)
                {
                    return MaskFade.Kind.WHITE;
                }
            }
        }
        return MaskFade.Kind.NONE;
    }

    protected float GetFloatParameter(string[] pd, int s, string name)
    {
        string str = this.GetStringParameter(pd, s, name);
        switch (str)
        {
            case null:
                return 0f;

            case "-":
                return -1f;
        }
        return float.Parse(str);
    }

    protected int GetIntegerParameter(string[] pd, int s, string name)
    {
        string str = this.GetStringParameter(pd, s, name);
        switch (str)
        {
            case null:
                return 0;

            case "-":
                return -1;
        }
        return int.Parse(str);
    }

    protected int[] GetPositionParameter(string[] pd, int s, string name)
    {
        int length = pd.Length;
        int index = s;
        while (index < length)
        {
            if ((pd[index] == name) && (pd[index + 1] == "="))
            {
                List<int> list = new List<int>();
                index += 2;
                while (index < length)
                {
                    string str = pd[index++];
                    if (str == " ")
                    {
                        break;
                    }
                    if (str != ",")
                    {
                        list.Add(int.Parse(str));
                    }
                }
                return list.ToArray();
            }
            index++;
            while (index < length)
            {
                if (pd[index++] == " ")
                {
                    continue;
                }
            }
        }
        return null;
    }

    public static int GetScriptGenderSetting() => 
        scriptGenderSettingIndex;

    public static string GetScriptName_BattleEnd(int questId, int questPhase) => 
        $"{questId:D8}{questPhase:D1}1";

    public static string GetScriptName_BattleStart(int questId, int questPhase) => 
        $"{questId:D8}{questPhase:D1}0";

    public static string GetScriptObjectSetting() => 
        scriptObjectSettingAddress;

    public static string GetScriptPlayerObjectSetting() => 
        scriptPlayerObjectSettingAddress;

    public static string GetScriptPlayerPathSetting() => 
        scriptPlayerPathSettingAddress;

    public static string GetScriptServerSetting() => 
        scriptServerSettingAddress;

    protected static string getScriptSettingFileName() => 
        (Application.persistentDataPath + "/scriptsave.dat");

    public static string GetScriptStartModeSetting() => 
        scriptStartModeSettingName;

    protected static StartMode GetStartMode(string startModeName)
    {
        string key = startModeName;
        if (key != null)
        {
            int num;
            if (<>f__switch$map1E == null)
            {
                Dictionary<string, int> dictionary = new Dictionary<string, int>(8) {
                    { 
                        "BATTLE_START",
                        0
                    },
                    { 
                        "BATTLE_END",
                        0
                    },
                    { 
                        "CHAPTER_START",
                        0
                    },
                    { 
                        "CHAPTER_CLEAR",
                        0
                    },
                    { 
                        "QUEST_START",
                        0
                    },
                    { 
                        "COMMON",
                        1
                    },
                    { 
                        "SCENARIO",
                        2
                    },
                    { 
                        "GACHA",
                        2
                    }
                };
                <>f__switch$map1E = dictionary;
            }
            if (<>f__switch$map1E.TryGetValue(key, out num))
            {
                switch (num)
                {
                    case 0:
                        return StartMode.BLACK_SCENE_STOP;

                    case 1:
                        return StartMode.FULL;
                }
            }
        }
        return StartMode.NONE;
    }

    public static string GetStartModeForAssetStorage(string path, string label)
    {
        string str = "DEFAULT";
        if (!path.StartsWith(textPath))
        {
            return str;
        }
        if (path.StartsWith(textPath + "/Ts"))
        {
            return str;
        }
        if (path.StartsWith(textPath + "/Effect"))
        {
            return str;
        }
        if (path.StartsWith(textPath + "/Gacha"))
        {
            return "GACHA";
        }
        if (label.StartsWith("ChapterStart"))
        {
            return "CHAPTER_START";
        }
        if (label.StartsWith("ChapterClear"))
        {
            return "CHAPTER_CLEAR";
        }
        if (label.StartsWith("QuestStart"))
        {
            return "QUEST_START";
        }
        if (path.StartsWith(textPath + "/Common"))
        {
            return "COMMON";
        }
        if (label.EndsWith("0"))
        {
            return "BATTLE_START";
        }
        if (label.EndsWith("1"))
        {
            return "BATTLE_END";
        }
        return "SCENARIO";
    }

    protected string GetStringParameter(string[] pd, int s, string name)
    {
        int num = pd.Length - 2;
        int index = s;
        while (index < num)
        {
            if ((pd[index] == name) && (pd[index + 1] == "="))
            {
                return this.ConvertNameString(pd[index + 2]);
            }
            index++;
            while (index < num)
            {
                if (pd[index++] == " ")
                {
                    continue;
                }
            }
        }
        return null;
    }

    [DebuggerHidden]
    private IEnumerator ImageCapture() => 
        new <ImageCapture>c__Iterator16();

    protected void InitCamera()
    {
        TweenScale component = this.cameraScale.GetComponent<TweenScale>();
        if (component != null)
        {
            component.enabled = false;
        }
        TweenPosition position = this.cameraPosition.GetComponent<TweenPosition>();
        if (position != null)
        {
            position.enabled = false;
        }
        this.cameraScale.transform.localScale = Vector3.one;
        this.cameraPosition.transform.localPosition = Vector3.zero;
        this.isExecuteCamera = false;
    }

    protected void InitEffect()
    {
        ProgramEffectManager.Destory(this.specialEffectBase);
        CommonEffectManager.Destroy(this.fowardEffectBase);
        CommonEffectManager.Destroy(this.effectBase);
        CommonEffectManager.Destroy(this.backEffectBase);
        CommonEffectManager.Destroy(this.communicationCharaEffectBase);
        this.isLoadCommunicationChara = false;
    }

    protected void InitFade()
    {
        UITweenRenderer meshFadeBase = this.meshFadeBase;
        UITweenRenderer meshRenderBase = this.meshRenderBase;
        meshFadeBase.SetTweenColor(new Color(0f, 0f, 0f, 0f));
        meshRenderBase.SetTweenColor(new Color(1f, 1f, 1f, 0f));
        if (this.isExecuteFade)
        {
            TweenRendererColor component = meshFadeBase.GetComponent<TweenRendererColor>();
            if (component != null)
            {
                component.enabled = false;
            }
            TweenRendererColor color2 = meshRenderBase.GetComponent<TweenRendererColor>();
            if (color2 != null)
            {
                color2.enabled = false;
            }
            this.isExecuteFade = false;
        }
        this.fadeName = null;
        this.fadeColor = new Color(0f, 0f, 0f, 0f);
    }

    protected void InitFlash()
    {
        UITweenRenderer meshFlashBase = this.meshFlashBase;
        meshFlashBase.SetTweenColor(new Color(1f, 1f, 1f, 0f));
        if (this.isExecuteFlash)
        {
            TweenRendererColor component = meshFlashBase.GetComponent<TweenRendererColor>();
            if (component != null)
            {
                component.enabled = false;
            }
            this.flashName = null;
            this.isExecuteFlash = false;
            this.isEndRequestFlash = false;
        }
    }

    public void Initialize()
    {
        this.state = State.NONE;
        this.RenderCameraQuit();
        this.charaList = new ScriptCharaData[CHARA_MAX];
        this.flagDataList = new List<ScriptFlagData>();
        this.stretchBaseRange = new Vector2((float) (this.stretchBase.width / 2), (float) (this.stretchBase.height / 2));
        this.codeCommentString = LocalizationManager.Get("SCRIPT_ACTION_CODE_COMMENT");
        this.codeLabelString = LocalizationManager.Get("SCRIPT_ACTION_CODE_LABEL");
        this.codeSceneString = LocalizationManager.Get("SCRIPT_ACTION_CODE_SCENE");
        this.codeTalkString = LocalizationManager.Get("SCRIPT_ACTION_CODE_TALK");
        this.codeSwitchCaseString = LocalizationManager.Get("SCRIPT_ACTION_CODE_SWITCH_CASE");
        this.codeSwitchCaseSplitString = LocalizationManager.Get("SCRIPT_ACTION_CODE_SWITCH_CASE_SPLIT");
        this.codeSwitchEndString = LocalizationManager.Get("SCRIPT_ACTION_CODE_SWITCH_END");
        this.codeVoiceString = LocalizationManager.Get("SCRIPT_ACTION_CODE_VOICE");
        this.codeReturnString = LocalizationManager.Get("SCRIPT_ACTION_CODE_RETURN");
        this.backLogTalkStartString = LocalizationManager.Get("SCRIPT_ACTION_BACK_LOG_TALK_START");
        this.backLogTalkEndString = LocalizationManager.Get("SCRIPT_ACTION_BACK_LOG_TALK_END");
        this.backLogDialog.ClearLog();
        this.logMessage.Quit();
        this.skipConfirmDialog.Init();
        this.notificationDialog.Init();
        this.ReleaseAsset();
        this.messageManager.ResetLongPress();
        this.messageManager.QuitScreen();
        this.InitEffect();
        this.messageManager.Shake(0f, 0f, 0f, 0f);
        this.Shake(0f, 0f, 0f, 0f);
        this.InitCamera();
        this.InitStretch();
        this.InitFlash();
        this.InitWipe();
        this.InitFade();
        this.blockPanel.gameObject.SetActive(false);
        this.actionPanel.gameObject.SetActive(false);
        this.renderPanel.gameObject.SetActive(false);
        this.DebugProcessScript(-1, State.NONE);
    }

    protected void InitMask()
    {
        SingletonMonoBehaviour<CommonUI>.Instance.maskFadeInit();
    }

    protected void InitSetting()
    {
        this.flagDataList.Clear();
        this.backLogDialog.ClearLog();
        this.logMessage.ClearText();
        this.blockPanel.gameObject.SetActive(true);
        this.systemPanel.gameObject.SetActive(true);
        this.normalOperationBase.SetActive(false);
        this.backViewOperationBase.SetActive(false);
        this.figureViewOperationBase.SetActive(false);
        this.viewBackLabel.text = string.Empty;
        this.viewFigureLabel.text = string.Empty;
        this.viewFaceLabel.text = string.Empty;
        PlayMode playMode = this.playMode;
        if (playMode == PlayMode.BACK)
        {
            this.backViewOperationBase.SetActive(true);
        }
        else if (playMode == PlayMode.FIGURE)
        {
            this.figureViewOperationBase.SetActive(true);
        }
        else
        {
            this.normalOperationBase.SetActive(true);
            this.backLogButton.gameObject.SetActive(this.isBackLog);
            this.skipButton.gameObject.SetActive(this.isSkip);
        }
        this.skipConfirmDialog.Init();
        this.notificationDialog.Init();
        this.fastPlayMark.SetActive(false);
        this.touchFullScreen.SetActive(false);
        this.messageManager.ResetLongPress();
        this.InitEffect();
        this.messageManager.Shake(0f, 0f, 0f, 0f);
        this.Shake(0f, 0f, 0f, 0f);
        this.InitCamera();
        this.InitStretch();
        this.InitFlash();
        this.InitWipe();
        this.InitFade();
        this.inputCommandIndex = -1;
        this.isRequestSkip = false;
        this.isExecuteSkip = false;
        this.isCancelInput = false;
        this.isTalkMask = true;
        this.talkMaskName = string.Empty;
        this.playSpeed = PlaySpeed.NORMAL;
        this.requestPlaySpeed = PlaySpeed.NONE;
        this.isStartModeEnd = false;
        this.isSkipAudioStop = false;
        this.inputTopMode = InputTopMode.NORMAL;
        this.isRequestEffect = false;
        this.playScriptDataList = null;
        this.playScriptDebugMode = PlayScriptDebugMode.NONE;
        this.DebugProcessScript(-1, State.NONE);
        if (this.downloadAssetList != null)
        {
            this.downloadAssetList = null;
        }
        if (this.audioAssetList != null)
        {
            for (int i = 0; i < this.audioAssetList.Count; i++)
            {
                SoundManager.releaseAudioAssetStorage(this.audioAssetList[i]);
            }
            this.audioAssetList = null;
        }
        SingletonTemplate<MissionNotifyManager>.Instance.StartPause();
    }

    protected void InitStretch()
    {
        Vector3 one = Vector3.one;
        Vector3 zero = Vector3.zero;
        if (this.isExecuteStretch)
        {
            TweenScale component = this.stretchBase.GetComponent<TweenScale>();
            if (component != null)
            {
                component.enabled = false;
            }
            TweenPosition position = this.stretchBase.GetComponent<TweenPosition>();
            if (position != null)
            {
                position.enabled = false;
            }
            TweenScale.Begin(this.stretchBase.gameObject, 0f, one);
            TweenPosition.Begin(this.stretchBase.gameObject, 0f, zero);
            this.stretchName = null;
            this.isExecuteStretch = false;
        }
        this.stretchBase.transform.localScale = one;
        this.stretchBase.transform.localPosition = zero;
    }

    protected void InitWipe()
    {
        this.meshWipeBase.SetTweenVolume(0f);
        if (this.isExecuteWipe)
        {
            TweenRenderVolume component = this.meshWipeBase.GetComponent<TweenRenderVolume>();
            if (component != null)
            {
                component.enabled = false;
            }
            if (this.wipeAssetData != null)
            {
                AssetManager.releaseAsset(this.wipeAssetData);
                this.wipeAssetData = null;
            }
            this.wipeName = null;
            this.isExecuteWipe = false;
            this.isLoadWipe = false;
        }
    }

    public static bool IsBusyFade() => 
        SingletonMonoBehaviour<ScriptManager>.Instance.isExecuteFade;

    protected bool IsFastPlaySpeed() => 
        ((this.playScriptJumpLine >= 0) || (this.playSpeed == PlaySpeed.FAST));

    protected bool IsJumpLine() => 
        (this.playScriptJumpLine >= 0);

    protected bool IsNormalPlaySpeed()
    {
        if (this.playScriptJumpLine >= 0)
        {
            return false;
        }
        return (this.playSpeed != PlaySpeed.FAST);
    }

    public bool IsShake() => 
        (this.shakeCycle > 0f);

    protected bool IsWaitTalk()
    {
        for (int i = 0; i < CHARA_MAX; i++)
        {
            ScriptCharaData data = this.charaList[i];
            if (data != null)
            {
                if (data.IsLoad())
                {
                    return true;
                }
                if (data.IsMoveAlpha())
                {
                    return true;
                }
            }
        }
        return false;
    }

    protected bool JumpScript(int index)
    {
        if ((index >= 0) && (index < this.executeIndexMax))
        {
            Debug.Log("jump index [" + index + "] ");
            this.state = State.EXECUTE;
            this.executeWaitIndex = this.executeIndex = index;
            this.isSwithCase = false;
            return true;
        }
        Debug.LogError("jump index error " + index);
        this.state = State.ERROR;
        return false;
    }

    protected bool JumpScript(string label)
    {
        object[] objArray1;
        PlayMode playMode = this.playMode;
        if (playMode != PlayMode.BACK)
        {
            if (playMode == PlayMode.FIGURE)
            {
                this.state = State.FIGURE_VIEW_INIT;
                return true;
            }
            if (label == null)
            {
                this.executeWaitIndex = this.executeIndex = 0;
                goto Label_00F4;
            }
            this.executeIndex = -1;
            for (int i = 0; i < this.executeIndexMax; i++)
            {
                if ((this.executeTagList[i] == "label") && label.Equals(this.executeDataList[i]))
                {
                    this.executeWaitIndex = this.executeIndex = i;
                    break;
                }
            }
        }
        else
        {
            this.state = State.BACK_VIEW_INIT;
            return true;
        }
        if (this.executeIndex < 0)
        {
            if (label != "skip")
            {
                if (label == "nameInput")
                {
                    return false;
                }
                Debug.LogError("execute tag error " + base.tag);
                this.state = State.ERROR;
            }
            return false;
        }
    Label_00F4:
        objArray1 = new object[] { "execute index [", this.executeIndex, "] ", base.tag };
        Debug.Log(string.Concat(objArray1));
        this.isSwithCase = false;
        if (this.isStartModeEnd)
        {
            this.state = State.WAIT;
            this.waitType = "load";
        }
        else
        {
            this.state = State.WAIT;
            this.waitType = "start";
        }
        return true;
    }

    protected bool JumpScriptCommand(string command)
    {
        if (command != null)
        {
            for (int i = this.executeIndex; i < this.executeIndexMax; i++)
            {
                if (command.Equals(this.executeTagList[i]))
                {
                    this.executeWaitIndex = this.executeIndex = i;
                    Debug.Log(string.Concat(new object[] { "jump command index [", this.executeIndex, "] ", command }));
                    this.state = State.EXECUTE;
                    if (command == "switchCaseEnd")
                    {
                        this.isSwithCase = false;
                    }
                    return true;
                }
            }
        }
        else
        {
            for (int j = this.executeIndex; j < this.executeIndexMax; j++)
            {
                if (this.executeTagList[j] == null)
                {
                    this.executeWaitIndex = this.executeIndex = j;
                    Debug.Log(string.Concat(new object[] { "jump command index [", this.executeIndex, "] ", command }));
                    this.state = State.EXECUTE;
                    return true;
                }
            }
        }
        Debug.LogError("jump command error " + command);
        this.state = State.ERROR;
        return false;
    }

    protected bool JumpScriptSwitchCase(int index)
    {
        if (index < 0)
        {
            return this.JumpScriptCommand("switchCaseEnd");
        }
        int num = 0;
        for (int i = this.executeIndex; i < this.executeIndexMax; i++)
        {
            if (this.executeTagList[i] == "switchCase")
            {
                if (index == num)
                {
                    this.executeWaitIndex = this.executeIndex = i + 1;
                    Debug.Log(string.Concat(new object[] { "jump switchcase index [", this.executeIndex, "] ", index }));
                    this.state = State.EXECUTE;
                    this.isSwithCase = true;
                    return true;
                }
                num++;
            }
        }
        Debug.LogError("jump switchcase index error " + index);
        this.state = State.ERROR;
        return false;
    }

    protected void LoadAudioAssetData()
    {
        if (this.audioLoadIndex < this.audioAssetList.Count)
        {
            SoundManager.loadAudioAssetStorage(this.audioAssetList[this.audioLoadIndex++], new System.Action(this.LoadAudioAssetData), SoundManager.CueType.ALL);
        }
        else
        {
            SingletonMonoBehaviour<CommonUI>.Instance.SetLoadMode(ConnectMark.Mode.DEFAULT);
            this.isExecuteLoadAsset = false;
        }
    }

    public static void LoadBattleEndGameDemo(int questId, int questPhase, bool isBefore, Action<string> callbackFunc)
    {
        object[] objArray1 = new object[] { "LoadBattleEndGameDemo ", questId, " ", questPhase, " ", isBefore };
        playScriptDataName = string.Concat(objArray1);
        SingletonMonoBehaviour<ScriptManager>.Instance.LoadBattleEndGameDemoLocal(string.Format("{0:D8}_{1:D1}_2_" + (!isBefore ? 2 : 1), questId, questPhase), callbackFunc);
    }

    protected void LoadBattleEndGameDemoLocal(string demoInfo, Action<string> callbackFunc)
    {
        <LoadBattleEndGameDemoLocal>c__AnonStorey72 storey = new <LoadBattleEndGameDemoLocal>c__AnonStorey72 {
            demoInfo = demoInfo,
            callbackFunc = callbackFunc,
            <>f__this = this
        };
        if (this.gameDemoAssetData == null)
        {
            AssetManager.loadAssetStorage("Demo", new AssetLoader.LoadEndDataHandler(storey.<>m__81));
        }
        else
        {
            string objectText = this.gameDemoAssetData.GetObjectText(storey.demoInfo);
            if (storey.callbackFunc != null)
            {
                storey.callbackFunc(objectText);
            }
        }
    }

    public static void LoadBattleStartGameDemo(int questId, int questPhase, bool isBefore, Action<string> callbackFunc)
    {
        object[] objArray1 = new object[] { "LoadBattleStartGameDemo ", questId, " ", questPhase, " ", isBefore };
        playScriptDataName = string.Concat(objArray1);
        SingletonMonoBehaviour<ScriptManager>.Instance.LoadBattleStartGameDemoLocal(string.Format("{0:D8}_{1:D1}_1_" + (!isBefore ? 2 : 1), questId, questPhase), callbackFunc);
    }

    protected void LoadBattleStartGameDemoLocal(string demoInfo, Action<string> callbackFunc)
    {
        <LoadBattleStartGameDemoLocal>c__AnonStorey71 storey = new <LoadBattleStartGameDemoLocal>c__AnonStorey71 {
            demoInfo = demoInfo,
            callbackFunc = callbackFunc,
            <>f__this = this
        };
        if (this.gameDemoAssetData == null)
        {
            AssetManager.loadAssetStorage("Demo", new AssetLoader.LoadEndDataHandler(storey.<>m__80));
        }
        else
        {
            TextAsset asset = this.gameDemoAssetData.GetObject<TextAsset>(storey.demoInfo);
            string text = null;
            if (asset != null)
            {
                text = asset.text;
            }
            if (storey.callbackFunc != null)
            {
                storey.callbackFunc(text);
            }
        }
    }

    protected void LoadExecuteScript(string name, string label)
    {
        this.assetName = name;
        this.scriptLabel = label;
        if ((this.assetData != null) && (this.assetData.Name == name))
        {
            this.state = State.IDLE;
            this.ExecuteScript(this.scriptLabel, null, null);
        }
        else
        {
            this.state = State.LOAD;
            if (!AssetManager.loadAssetStorage(textPath + "/" + name, new AssetLoader.LoadEndDataHandler(this.EndLoadAsset)))
            {
                this.EndLoadAsset(null);
            }
        }
    }

    protected void ModifyPlaySpeed(bool isRecover = true)
    {
        if (isRecover)
        {
            if ((this.playSpeed == PlaySpeed.PAUSE) || (this.requestPlaySpeed == PlaySpeed.PAUSE))
            {
                this.playSpeed = PlaySpeed.PAUSE;
                this.requestPlaySpeed = PlaySpeed.NONE;
            }
            else if (this.requestPlaySpeed != PlaySpeed.NONE)
            {
                this.playSpeed = this.requestPlaySpeed;
                this.requestPlaySpeed = PlaySpeed.NONE;
            }
            else
            {
                this.playSpeed = !this.messageManager.IsLongPress() ? PlaySpeed.NORMAL : PlaySpeed.FAST;
            }
        }
        else if (((this.playSpeed != PlaySpeed.PAUSE) && (this.requestPlaySpeed != PlaySpeed.PAUSE)) && ((this.requestPlaySpeed == PlaySpeed.NONE) && ((this.playSpeed == PlaySpeed.NORMAL) && this.messageManager.IsLongPress())))
        {
            this.playSpeed = PlaySpeed.FAST;
        }
        this.fastPlayMark.SetActive(this.playSpeed == PlaySpeed.FAST);
        if (this.IsFastPlaySpeed())
        {
            this.messageManager.RequestFastMessage();
        }
    }

    public void OnClickBackLog()
    {
        if ((this.isBackLog && !this.backLogDialog.IsEmptyLog()) && (this.inputTopMode == InputTopMode.NORMAL))
        {
            this.inputTopMode = InputTopMode.BACK_LOG;
            this.PausePlaySpeed();
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.backLogDialog.Open(new ScriptBackLog.ClickDelegate(this.OnEndBackLog));
        }
    }

    public void OnClickBackView()
    {
        switch (this.playMode)
        {
            case PlayMode.BACK:
                if (this.backNameList == null)
                {
                    return;
                }
                for (int i = 0; i < this.backNameList.Length; i++)
                {
                    if (this.backName.Equals(this.backNameList[i]))
                    {
                        string fileName = (i <= 0) ? this.backNameList[this.backNameList.Length - 1] : this.backNameList[i - 1];
                        if (this.SetSceneImage(fileName))
                        {
                            this.backName = fileName;
                            this.viewBackLabel.text = fileName;
                        }
                        break;
                    }
                }
                break;

            case PlayMode.FIGURE:
                if (this.figureNameList == null)
                {
                    return;
                }
                for (int j = 0; j < this.figureNameList.Length; j++)
                {
                    if (this.figureName.Equals(this.figureNameList[j]))
                    {
                        string charaName = (j <= 0) ? this.figureNameList[this.figureNameList.Length - 1] : this.figureNameList[j - 1];
                        if (this.SetCharaImage("View", charaName))
                        {
                            this.figureName = charaName;
                            this.viewFigureLabel.text = charaName;
                            this.viewFaceLabel.text = string.Empty + this.figureFaceNum;
                        }
                        break;
                    }
                }
                break;
        }
    }

    public void OnClickFaceBackView()
    {
        this.SetCharaFace(this.figureFaceNum - 1);
        this.viewFaceLabel.text = string.Empty + this.figureFaceNum;
    }

    public void OnClickFaceFowardView()
    {
        this.SetCharaFace(this.figureFaceNum + 1);
        this.viewFaceLabel.text = string.Empty + this.figureFaceNum;
    }

    public void OnClickFowardView()
    {
        switch (this.playMode)
        {
            case PlayMode.BACK:
                if (this.backNameList == null)
                {
                    return;
                }
                for (int i = 0; i < this.backNameList.Length; i++)
                {
                    if (this.backName.Equals(this.backNameList[i]))
                    {
                        string fileName = (i >= (this.backNameList.Length - 1)) ? this.backNameList[0] : this.backNameList[i + 1];
                        if (this.SetSceneImage(fileName))
                        {
                            this.backName = fileName;
                            this.viewBackLabel.text = fileName;
                        }
                        break;
                    }
                }
                break;

            case PlayMode.FIGURE:
                if (this.figureNameList == null)
                {
                    return;
                }
                for (int j = 0; j < this.figureNameList.Length; j++)
                {
                    if (this.figureName.Equals(this.figureNameList[j]))
                    {
                        string charaName = (j >= (this.figureNameList.Length - 1)) ? this.figureNameList[0] : this.figureNameList[j + 1];
                        if (this.SetCharaImage("View", charaName))
                        {
                            this.figureName = charaName;
                            this.viewFigureLabel.text = charaName;
                            this.viewFaceLabel.text = string.Empty + this.figureFaceNum;
                        }
                        break;
                    }
                }
                break;
        }
    }

    public void OnClickReturnViewMode()
    {
        switch (this.playMode)
        {
            case PlayMode.BACK:
            case PlayMode.FIGURE:
                this.ExitExecuteScript();
                break;
        }
    }

    public void OnClickSkip()
    {
        if (((this.inputTopMode == InputTopMode.NORMAL) && this.isSkip) && !this.isRequestSkip)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.SkipConfirm();
        }
    }

    protected void OnCrossFadeEndScene()
    {
        Debug.Log("OnCrossFadeEndScene:" + this.sceneMainIndex);
        this.isBusyScene = false;
        if (this.sceneMainIndex == 0)
        {
            this.sceneMainIndex = 1;
            this.backSprite1.SetTweenColor(new Color(1f, 1f, 1f, 1f));
            this.backSprite1.transform.localPosition = Vector3.zero;
            this.backSprite2.color = new Color(1f, 1f, 1f, 0f);
            this.backSprite2.ClearImage();
        }
        else
        {
            this.sceneMainIndex = 0;
            this.backSprite2.SetTweenColor(new Color(1f, 1f, 1f, 1f));
            this.backSprite2.transform.localPosition = Vector3.zero;
            this.backSprite1.color = new Color(1f, 1f, 1f, 0f);
            this.backSprite1.ClearImage();
        }
    }

    protected void OnEndBackLog()
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
        this.backLogDialog.Close();
        this.ResumePlaySpeed();
        this.inputTopMode = InputTopMode.NORMAL;
    }

    protected void OnFlashRequest()
    {
        if (this.isExecuteFlash)
        {
            TweenRendererColor color = null;
            if (this.isEndRequestFlash)
            {
                if (this.flashTime3 <= 0f)
                {
                    this.EndExecuteFlash();
                    return;
                }
                color = TweenRendererColor.Begin(this.meshFlashBase.gameObject, this.flashTime3, Color.clear);
            }
            else
            {
                switch (this.flashCount)
                {
                    case 0:
                        this.flashCount++;
                        if (this.flashTime1 > 0f)
                        {
                            color = TweenRendererColor.Begin(this.meshFlashBase.gameObject, this.flashTime1, this.flashColor1);
                        }
                        else
                        {
                            if ((this.flashName == "once") && (this.flashTime2 == 0f))
                            {
                                this.EndExecuteFlash();
                                return;
                            }
                            this.meshFlashBase.SetTweenColor(this.flashColor1);
                        }
                        break;

                    case 1:
                    {
                        string flashName = this.flashName;
                        if (flashName != null)
                        {
                            int num2;
                            if (<>f__switch$map28 == null)
                            {
                                Dictionary<string, int> dictionary = new Dictionary<string, int>(3) {
                                    { 
                                        "pingpong",
                                        0
                                    },
                                    { 
                                        "loop",
                                        1
                                    },
                                    { 
                                        "once",
                                        1
                                    }
                                };
                                <>f__switch$map28 = dictionary;
                            }
                            if (<>f__switch$map28.TryGetValue(flashName, out num2))
                            {
                                if (num2 == 0)
                                {
                                    if (this.flashTime2 > 0f)
                                    {
                                        this.meshFlashBase.SetTweenColor(this.flashColor2);
                                        color = TweenRendererColor.Begin(this.meshFlashBase.gameObject, this.flashTime2, this.flashColor1);
                                    }
                                    else
                                    {
                                        this.meshFlashBase.SetTweenColor(this.flashColor2);
                                    }
                                    break;
                                }
                                else if (num2 == 1)
                                {
                                }
                            }
                        }
                        this.flashCount = 0;
                        if (this.flashTime2 > 0f)
                        {
                            color = TweenRendererColor.Begin(this.meshFlashBase.gameObject, this.flashTime2, this.flashColor2);
                        }
                        else
                        {
                            this.meshFlashBase.SetTweenColor(this.flashColor2);
                        }
                        if (this.flashName == "once")
                        {
                            this.isEndRequestFlash = true;
                            this.flashTime3 = !(this.flashColor2 == Color.clear) ? this.flashTime1 : 0f;
                        }
                        break;
                    }
                }
            }
            if (color != null)
            {
                color.eventReceiver = base.gameObject;
                color.callWhenFinished = !this.isEndRequestFlash ? "OnFlashRequest" : "EndExecuteFlash";
            }
            else
            {
                base.Invoke(!this.isEndRequestFlash ? "OnFlashRequest" : "EndExecuteFlash", 0.001f);
            }
        }
    }

    protected void OnLoadEndScene()
    {
        Debug.Log(string.Concat(new object[] { "OnLoadEndScene:", this.sceneMainIndex, " ", this.sceneCrossFadeTime }));
        this.isBusySceneLoad = false;
        TweenRendererColor color = null;
        if (this.sceneCrossFadeTime > 0f)
        {
            if (this.sceneMainIndex == 0)
            {
                color = TweenRendererColor.Begin(this.backSprite1.gameObject, this.sceneCrossFadeTime, new Color(1f, 1f, 1f, 1f));
            }
            else
            {
                color = TweenRendererColor.Begin(this.backSprite2.gameObject, this.sceneCrossFadeTime, new Color(1f, 1f, 1f, 1f));
            }
            if (color != null)
            {
                color.eventReceiver = base.gameObject;
                color.callWhenFinished = "OnCrossFadeEndScene";
            }
            else
            {
                Debug.LogWarning(string.Concat(new object[] { "CrossFade tween error:", this.sceneMainIndex, " ", this.sceneCrossFadeTime }));
                Debug.Break();
                this.OnCrossFadeEndScene();
            }
        }
        else
        {
            this.OnCrossFadeEndScene();
        }
    }

    protected void OnSelectMenu(int index)
    {
        if (this.state == State.WAIT)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE2);
            this.logMessage.AddText("[FF4040]Select[r][FF4040]  " + this.menuMessageList[index] + "[r]");
            this.selectDialog.Close();
            string label = this.menuLabelList[index];
            this.menuMessageList = null;
            this.menuLabelList = null;
            this.isCancelInput = true;
            this.JumpScript(label);
            this.ResumePlaySpeed();
            this.waitType = "time";
            this.waitCount = this.defaultKeyDelayTime;
        }
    }

    protected void OnSelectSwitchCase(int index)
    {
        if (this.state == State.WAIT)
        {
            if (index >= 0)
            {
                SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE2);
                this.logMessage.AddText("[FF4040]Select[r][FF4040]  " + this.menuMessageList[index] + "[r]");
            }
            else
            {
                SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
            }
            this.selectDialog.SelectDecide(index, new ScriptSelectDialog.ClickDelegate(this.EndSelectSwitchCase));
        }
    }

    protected void OnShake()
    {
        if ((this.shakeCycle > 0f) && ((this.shakeTime == 0f) || (Time.time < this.shakeTime)))
        {
            this.shakeRoot.localPosition = new Vector3(UnityEngine.Random.Range(-this.shakeX, this.shakeX), UnityEngine.Random.Range(-this.shakeY, this.shakeY), 0f);
            base.Invoke("OnShake", this.shakeCycle);
        }
        else
        {
            base.CancelInvoke("OnShake");
            this.shakeRoot.localPosition = Vector3.zero;
            this.shakeCycle = 0f;
        }
    }

    protected void OnSwipe(SwipeGesture gesture)
    {
        if (this.inputTopMode == InputTopMode.NORMAL)
        {
            switch (gesture.Direction)
            {
                case FingerGestures.SwipeDirection.Right:
                    Debug.Log("RightSwipe");
                    break;

                case FingerGestures.SwipeDirection.Left:
                    Debug.Log("LeftSwipe");
                    break;

                case FingerGestures.SwipeDirection.Up:
                    Debug.Log("UpSwipe");
                    this.OnClickBackLog();
                    break;

                case FingerGestures.SwipeDirection.Down:
                    Debug.Log("DownSwipe");
                    break;
            }
        }
    }

    public void OnTouchFullScreen()
    {
        Debug.Log(string.Concat(new object[] { "OnTouchFullScreen ", this.inputTopMode, " ", this.isRequestVoiceCancel }));
        switch (this.inputTopMode)
        {
            case InputTopMode.SKIP_VOICE:
                if (!this.isRequestVoiceCancel)
                {
                    this.isRequestVoiceCancel = true;
                }
                break;

            case InputTopMode.SHOW_BACK:
                this.inputTopMode = InputTopMode.NORMAL;
                this.messageManager.SetMessageOffMode(false);
                this.touchFullScreen.SetActive(false);
                this.selectDialog.SetActive(true);
                if (this.isMessageOffTalkMode)
                {
                    this.SetTalkName("on");
                }
                this.ResumePlaySpeed();
                break;

            case InputTopMode.AUTO:
                this.inputTopMode = InputTopMode.NORMAL;
                this.touchFullScreen.SetActive(false);
                break;
        }
    }

    protected void OpenNameInput()
    {
        this.inputCommandIndex = -1;
        if (this.isCollection)
        {
            if (!this.JumpScript("inputName"))
            {
                this.EndNameInput();
            }
        }
        else
        {
            this.inputTopMode = InputTopMode.INPUT;
            SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, 1f, new System.Action(this.EndFadeoutNameInput));
        }
    }

    protected void OpenNotificationDialog(string typeName, string message)
    {
        this.inputTopMode = InputTopMode.NOTIFICATION;
        this.PausePlaySpeed();
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        this.notificationDialog.Open(message, new ScriptNotificationDialog.CallbackFunc(this.EndNotificationDialog));
    }

    protected void PausePlaySpeed()
    {
        this.requestPlaySpeed = PlaySpeed.PAUSE;
        this.messageManager.ResetLongPress();
        this.fastPlayMark.SetActive(false);
    }

    public static void Play(string name, string label, bool isCollection, bool isSkip, CallbackFunc callbackFunc)
    {
        playScriptDataName = "Play " + name + " " + label;
        SingletonMonoBehaviour<ScriptManager>.Instance.PlayLocal(PlayMode.NORMAL, StartMode.NONE, name, label, isCollection, isSkip, true, callbackFunc, -1);
    }

    public static void PlayBattleEnd(int questId, int questPhase, CallbackFunc callbackFunc, bool isCollection = false)
    {
        int num = questId / 0xf4240;
        object[] objArray1 = new object[] { "PlayBattleEnd ", questId, " ", questPhase };
        playScriptDataName = string.Concat(objArray1);
        SingletonMonoBehaviour<ScriptManager>.Instance.PlayLocal(PlayMode.NORMAL, StartMode.BLACK_SCENE_STOP, $"{num:D2}", GetScriptName_BattleEnd(questId, questPhase), isCollection, true, true, callbackFunc, -1);
    }

    public static void PlayBattleStart(int questId, int questPhase, CallbackFunc callbackFunc, bool isCollection = false)
    {
        int num = questId / 0xf4240;
        object[] objArray1 = new object[] { "PlayBattleStart ", questId, " ", questPhase };
        playScriptDataName = string.Concat(objArray1);
        SingletonMonoBehaviour<ScriptManager>.Instance.PlayLocal(PlayMode.NORMAL, StartMode.BLACK_SCENE_STOP, $"{num:D2}", GetScriptName_BattleStart(questId, questPhase), isCollection, true, true, callbackFunc, -1);
    }

    public static void PlayChapterClear(int warId, CallbackFunc callbackFunc, bool isCollection = false)
    {
        playScriptDataName = "PlayChapterClear " + warId;
        SingletonMonoBehaviour<ScriptManager>.Instance.PlayLocal(PlayMode.NORMAL, StartMode.BLACK_FULL, "Common", "ChapterClear" + warId, isCollection, false, false, callbackFunc, -1);
    }

    public static void PlayChapterStart(int warId, CallbackFunc callbackFunc, bool isCollection = false)
    {
        <PlayChapterStart>c__AnonStorey6F storeyf = new <PlayChapterStart>c__AnonStorey6F {
            callbackFunc = callbackFunc,
            warId = warId,
            isCollection = isCollection
        };
        string scriptId = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.WAR).getEntityFromId<WarEntity>(storeyf.warId).scriptId;
        if (scriptId != "NONE")
        {
            playScriptDataName = "PlayChapter " + scriptId;
            SingletonMonoBehaviour<ScriptManager>.Instance.PlayLocal(PlayMode.NORMAL, StartMode.WHITE, scriptId.Substring(0, 2), scriptId, storeyf.isCollection, true, true, new CallbackFunc(storeyf.<>m__7D), -1);
        }
        else
        {
            _playChapterStart(storeyf.warId, storeyf.callbackFunc, storeyf.isCollection);
        }
    }

    public static void PlayGacha(int svtId, int limitCount, bool isFaceFirst, string message, System.Action callbackFunc)
    {
        PlayGacha(0L, svtId, limitCount, isFaceFirst, message, callbackFunc);
    }

    public static void PlayGacha(long usrSvtId, int svtId, int limitCount, bool isFaceFirst, string message, System.Action callbackFunc)
    {
        if (message == string.Empty)
        {
            message = null;
        }
        int imageLimitCount = ImageLimitCount.GetImageLimitCount(svtId, limitCount);
        ServantLimitAddMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantLimitAddMaster>(DataNameKind.Kind.SERVANT_LIMIT_ADD);
        int num2 = master.getVoiceId(svtId, limitCount);
        int num3 = master.getVoicePrefix(svtId, limitCount);
        ServantVoiceMaster master2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantVoiceMaster>(DataNameKind.Kind.SERVANT_VOICE);
        ServantVoiceEntity entity = master2.getEntityFromId<ServantVoiceEntity>(num2, num3, 3);
        ServantVoiceData[] voiceList = null;
        if (entity != null)
        {
            voiceList = entity.getFirstGetVoiceList()[0];
        }
        if (usrSvtId > 0L)
        {
            UserServantEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantMaster>(DataNameKind.Kind.USER_SERVANT).getEntityFromId<UserServantEntity>(usrSvtId);
            if ((entity2 != null) && entity2.IsEventJoin())
            {
                entity = master2.getEntityFromId<ServantVoiceEntity>(num2, num3, 4);
                if (entity != null)
                {
                    voiceList = entity.getEventJoinVoiceList()[0];
                }
            }
        }
        _playGacha(svtId, imageLimitCount, isFaceFirst, voiceList, message, callbackFunc);
    }

    protected void PlayLocal(PlayMode mode, StartMode start, string data, bool isCollection, bool isSkip, bool isBackLog, CallbackFunc callbackFunc, int jumpLine = -1)
    {
        this.playMode = mode;
        this.startMode = start;
        this.assetName = "template";
        this.scriptLabel = null;
        this.isCollection = (mode == PlayMode.NORMAL) ? isCollection : false;
        this.isSkip = isSkip;
        this.isBackLog = isBackLog;
        this.callbackFunc = callbackFunc;
        this.isSkipExit = false;
        this.playScriptJumpLine = jumpLine;
        this.InitSetting();
        ScriptReplaceString.Init();
        if (NetworkManager.IsLogin)
        {
            UserGameEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_GAME).getSingleEntity<UserGameEntity>();
            if (entity != null)
            {
                ScriptReplaceString.SetString(1, entity.name);
                ScriptReplaceString.SetPlayerGenderIndex(entity.genderType);
            }
        }
        ScriptReplaceString.SetString(2, questTitle);
        ScriptReplaceString.SetString(3, questMessage);
        this.state = State.IDLE;
        this.ExecuteScript(null, data, null);
    }

    protected void PlayLocal(PlayMode mode, StartMode start, string data, string orgData, string[] list, int genderIndex, CallbackFunc callbackFunc, int jumpLine = -1)
    {
        this.playMode = mode;
        this.startMode = start;
        this.isCollection = false;
        this.isSkip = true;
        this.isBackLog = true;
        this.callbackFunc = callbackFunc;
        this.isSkipExit = false;
        this.playScriptJumpLine = jumpLine;
        switch (this.playMode)
        {
            case PlayMode.BACK:
                this.backNameList = list;
                break;

            case PlayMode.FIGURE:
                this.figureNameList = list;
                this.figureFaceNum = 0;
                break;
        }
        this.InitSetting();
        ScriptReplaceString.Init();
        ScriptReplaceString.SetPlayerGenderIndex(genderIndex);
        this.scriptLabel = null;
        this.assetName = null;
        this.state = State.IDLE;
        this.ExecuteScript(null, data, orgData);
    }

    protected void PlayLocal(PlayMode mode, StartMode start, string name, string label, bool isCollection, bool isSkip, bool isBackLog, CallbackFunc callbackFunc, int jumpLine = -1)
    {
        this.playMode = mode;
        this.startMode = start;
        this.assetName = name;
        this.scriptLabel = label;
        this.isCollection = (mode == PlayMode.NORMAL) ? isCollection : false;
        this.isSkip = isSkip | this.isCollection;
        this.isBackLog = isBackLog;
        this.callbackFunc = callbackFunc;
        this.isSkipExit = false;
        this.playScriptJumpLine = jumpLine;
        if ((this.assetData != null) && (this.assetData.Name == name))
        {
            this.FirstExecuteScript();
        }
        else
        {
            this.state = State.LOAD;
            string str = textPath + "/" + name;
            if (!AssetManager.isExistAssetStorage(str) || !AssetManager.loadAssetStorage(str, new AssetLoader.LoadEndDataHandler(this.EndFirstLoadAsset)))
            {
                if (this.assetData != null)
                {
                    AssetManager.releaseAsset(this.assetData);
                    this.assetData = null;
                }
                this.FirstExecuteScript();
            }
        }
    }

    public static void PlayQuestStart(int warId, int questId, System.Action callbackFunc)
    {
        object[] objArray1 = new object[] { "PlayQuestStart ", warId, " ", questId };
        playScriptDataName = string.Concat(objArray1);
        QuestEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.QUEST).getEntityFromId<QuestEntity>(questId);
        if (entity.hasStartAction == 0)
        {
            if (callbackFunc != null)
            {
                callbackFunc();
            }
        }
        else
        {
            string questTitle = string.Empty;
            switch (entity.type)
            {
                case 1:
                    questTitle = string.Format(LocalizationManager.Get("QUEST_START_ACTION_TITLE_MAIN"), entity.chapterSubId);
                    break;

                case 2:
                    questTitle = LocalizationManager.Get("QUEST_START_ACTION_TITLE_FREE");
                    break;

                case 3:
                    questTitle = LocalizationManager.Get("QUEST_START_ACTION_TITLE_STORY");
                    break;

                case 5:
                    questTitle = LocalizationManager.Get("QUEST_START_ACTION_TITLE_EVENT");
                    break;

                case 6:
                    questTitle = LocalizationManager.Get("QUEST_START_ACTION_TITLE_HERO");
                    break;
            }
            PlayQuestStart((QuestEntity.enType) entity.type, warId, questId, questTitle, entity.getQuestName(), callbackFunc);
        }
    }

    public static void PlayQuestStart(QuestEntity.enType questType, int warId, int questId, string questTitle, string questMessage, System.Action callbackFunc)
    {
        <PlayQuestStart>c__AnonStorey70 storey = new <PlayQuestStart>c__AnonStorey70 {
            callbackFunc = callbackFunc
        };
        playScriptDataName = "PlayQuestStart";
        ScriptManager.questType = questType;
        ScriptManager.warId = warId;
        eventId = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<QuestGroupMaster>(DataNameKind.Kind.QUEST_GROUP).GetEventId(questId);
        ScriptManager.questId = questId;
        ScriptManager.questTitle = questTitle;
        ScriptManager.questMessage = questMessage;
        if (SingletonMonoBehaviour<CommonUI>.Instance.maskFadGetFadeoutKind() == MaskFade.Kind.BLACK)
        {
            SingletonMonoBehaviour<ScriptManager>.Instance.PlayLocal(PlayMode.NORMAL, StartMode.BLACK_FULL, "Common", "QuestStartBlack", false, false, false, new CallbackFunc(storey.<>m__7E), -1);
        }
        else
        {
            SingletonMonoBehaviour<ScriptManager>.Instance.PlayLocal(PlayMode.NORMAL, StartMode.CLEAR_BLACK_FULL, "Common", "QuestStart", false, false, false, new CallbackFunc(storey.<>m__7F), -1);
        }
    }

    public static void PlayScenario(string label, CallbackFunc callbackFunc, bool isCollection = false)
    {
        playScriptDataName = "PlayScenario " + label;
        SingletonMonoBehaviour<ScriptManager>.Instance.PlayLocal(PlayMode.NORMAL, StartMode.BLACK, label.Substring(0, 2), label, isCollection, true, true, callbackFunc, -1);
    }

    public static void PlayScenario(string label, bool isSkip, CallbackFunc callbackFunc, bool isCollection = false)
    {
        playScriptDataName = "PlayScenario " + label;
        SingletonMonoBehaviour<ScriptManager>.Instance.PlayLocal(PlayMode.NORMAL, StartMode.BLACK, label.Substring(0, 2), label, isCollection, isSkip, true, callbackFunc, -1);
    }

    public static void PlayShop(string label, CallbackFunc callbackFunc, bool isCollection = false)
    {
        playScriptDataName = "PlayShop " + label;
        SingletonMonoBehaviour<ScriptManager>.Instance.PlayLocal(PlayMode.NORMAL, StartMode.BLACK, label.Substring(0, 2), label, isCollection, true, true, callbackFunc, -1);
    }

    protected void ProcessScript(float delta)
    {
        int num31;
        ScriptCharaData data3;
        string str19;
        bool isBusy;
        bool flag11;
        string waitType;
        Dictionary<string, int> dictionary;
        int num87;
        string str31;
        int num88;
        if (this.state == State.NONE)
        {
            return;
        }
        if (ManagerConfig.UseDebugCommand)
        {
            if (Input.GetKeyDown("e"))
            {
                this.playScriptDebugMode += 1;
                if (this.playScriptDebugMode >= PlayScriptDebugMode.END)
                {
                    this.playScriptDebugMode = PlayScriptDebugMode.NONE;
                }
                this.DebugProcessScript(this.executeWaitIndex, this.state);
            }
            if (Input.GetKeyDown("p"))
            {
                base.StartCoroutine(this.ImageCapture());
            }
        }
    Label_0074:
        if (this.state != State.EXECUTE)
        {
            if (this.state != State.WAIT)
            {
                if (this.state == State.WAIT_EXIT)
                {
                    if (this.isExecuteFade || SingletonMonoBehaviour<CommonUI>.Instance.maskFadeIsBusy())
                    {
                        goto Label_65F5;
                    }
                    this.state = State.EXIT;
                    goto Label_0074;
                }
                if (this.state == State.EXIT)
                {
                    this.ExitExecuteScript();
                    goto Label_0074;
                }
                if ((this.state == State.BACK_VIEW_INIT) || (this.state == State.FIGURE_VIEW_INIT))
                {
                    if (!string.IsNullOrEmpty(this.backName))
                    {
                        this.SetSceneImage(this.backName);
                    }
                    if (!string.IsNullOrEmpty(this.figureName))
                    {
                        this.SetCharaImage("View", this.figureName);
                    }
                    this.StartMask("clear", true, 0f);
                    this.StartFade("clear", true, 0f);
                    switch (this.state)
                    {
                        case State.BACK_VIEW_INIT:
                            this.viewBackLabel.text = this.backName;
                            this.state = State.BACK_VIEW;
                            break;

                        case State.FIGURE_VIEW_INIT:
                            this.viewFigureLabel.text = this.figureName;
                            this.viewFaceLabel.text = string.Empty + this.figureFaceNum;
                            this.state = State.FIGURE_VIEW;
                            break;
                    }
                    goto Label_0074;
                }
                goto Label_65F5;
            }
            if (this.isRequestSkip && !this.isExecuteSkip)
            {
                waitType = this.waitType;
                if (waitType != null)
                {
                    if (<>f__switch$map25 == null)
                    {
                        dictionary = new Dictionary<string, int>(4) {
                            { 
                                "switchCase",
                                0
                            },
                            { 
                                "switchCase2",
                                0
                            },
                            { 
                                "menu",
                                0
                            },
                            { 
                                "menu2",
                                0
                            }
                        };
                        <>f__switch$map25 = dictionary;
                    }
                    if (<>f__switch$map25.TryGetValue(waitType, out num87) && (num87 == 0))
                    {
                        this.selectDialog.Close();
                        this.menuMessageList = null;
                        this.menuLabelList = null;
                    }
                }
                this.state = State.EXECUTE;
                goto Label_0074;
            }
            isBusy = true;
            flag11 = false;
            waitType = this.waitType;
            if (waitType != null)
            {
                if (<>f__switch$map26 == null)
                {
                    dictionary = new Dictionary<string, int>(0x4a) {
                        { 
                            "time",
                            0
                        },
                        { 
                            "frame",
                            1
                        },
                        { 
                            "talkStart",
                            2
                        },
                        { 
                            "talkEnd",
                            3
                        },
                        { 
                            "message",
                            4
                        },
                        { 
                            "message2",
                            5
                        },
                        { 
                            "message3",
                            6
                        },
                        { 
                            "talkName",
                            7
                        },
                        { 
                            "talkName2",
                            8
                        },
                        { 
                            "talkName3",
                            9
                        },
                        { 
                            "clear",
                            10
                        },
                        { 
                            "clear2",
                            11
                        },
                        { 
                            "page",
                            12
                        },
                        { 
                            "page2",
                            13
                        },
                        { 
                            "page3",
                            14
                        },
                        { 
                            "touch",
                            15
                        },
                        { 
                            "touch2",
                            0x10
                        },
                        { 
                            "switchCase",
                            0x11
                        },
                        { 
                            "switchCase2",
                            0x12
                        },
                        { 
                            "menu",
                            0x13
                        },
                        { 
                            "menu2",
                            20
                        },
                        { 
                            "notification",
                            0x15
                        },
                        { 
                            "notification2",
                            0x16
                        },
                        { 
                            "notification3",
                            0x17
                        },
                        { 
                            "inputname",
                            0x18
                        },
                        { 
                            "inputname2",
                            0x19
                        },
                        { 
                            "inputname3",
                            0x1a
                        },
                        { 
                            "bgm",
                            0x1b
                        },
                        { 
                            "jingle",
                            0x1c
                        },
                        { 
                            "se",
                            0x1d
                        },
                        { 
                            "seLoop",
                            30
                        },
                        { 
                            "voice",
                            0x1f
                        },
                        { 
                            "voiceCancel",
                            0x20
                        },
                        { 
                            "capture",
                            0x21
                        },
                        { 
                            "mask",
                            0x22
                        },
                        { 
                            "fade",
                            0x23
                        },
                        { 
                            "wipe",
                            0x24
                        },
                        { 
                            "wipeLoad",
                            0x25
                        },
                        { 
                            "flash",
                            0x26
                        },
                        { 
                            "stretch",
                            0x27
                        },
                        { 
                            "camera",
                            40
                        },
                        { 
                            "scene",
                            0x29
                        },
                        { 
                            "sceneLoad",
                            0x2a
                        },
                        { 
                            "specialEffect",
                            0x2b
                        },
                        { 
                            "fowardEffect",
                            0x2c
                        },
                        { 
                            "fowardEffectStart",
                            0x2d
                        },
                        { 
                            "effect",
                            0x2e
                        },
                        { 
                            "backEffect",
                            0x2f
                        },
                        { 
                            "charaSet",
                            0x30
                        },
                        { 
                            "figureSet",
                            0x30
                        },
                        { 
                            "equipSet",
                            0x30
                        },
                        { 
                            "imageSet",
                            0x30
                        },
                        { 
                            "charaChange",
                            0x31
                        },
                        { 
                            "figureChange",
                            0x31
                        },
                        { 
                            "equipChange",
                            0x31
                        },
                        { 
                            "imageChange",
                            0x31
                        },
                        { 
                            "charaCut",
                            50
                        },
                        { 
                            "charaFade",
                            0x33
                        },
                        { 
                            "charaMove",
                            0x34
                        },
                        { 
                            "charaReturn",
                            0x34
                        },
                        { 
                            "charaMoveReturn",
                            0x34
                        },
                        { 
                            "charaAttack",
                            0x34
                        },
                        { 
                            "charaTalk",
                            0x35
                        },
                        { 
                            "charaSpecialEffect",
                            0x36
                        },
                        { 
                            "charaEffect",
                            0x37
                        },
                        { 
                            "charaBackEffect",
                            0x38
                        },
                        { 
                            "communicationCharaLoad",
                            0x39
                        },
                        { 
                            "communicationChara",
                            0x3a
                        },
                        { 
                            "start",
                            0x3b
                        },
                        { 
                            "startEnd",
                            60
                        },
                        { 
                            "startFade",
                            0x3d
                        },
                        { 
                            "startFade2",
                            0x3e
                        },
                        { 
                            "load",
                            0x3f
                        },
                        { 
                            "load2",
                            0x40
                        }
                    };
                    <>f__switch$map26 = dictionary;
                }
                if (<>f__switch$map26.TryGetValue(waitType, out num87))
                {
                    switch (num87)
                    {
                        case 0:
                            this.waitCount -= delta;
                            delta = 0f;
                            isBusy = this.waitCount > 0f;
                            goto Label_6476;

                        case 1:
                            isBusy = this.waitCount > 0f;
                            this.waitCount--;
                            goto Label_6476;

                        case 2:
                            this.logMessage.FowardText(this.waitMessage);
                            this.waitMessage = null;
                            isBusy = false;
                            goto Label_6476;

                        case 3:
                            this.logMessage.AddText(this.waitMessage);
                            this.waitMessage = null;
                            isBusy = false;
                            goto Label_6476;

                        case 4:
                            if (!this.messageManager.IsReturnScroll())
                            {
                                if (this.waitMessage != null)
                                {
                                    this.messageManager.AddText(this.waitMessage);
                                    this.logMessage.AddText(this.waitMessage);
                                    this.waitMessage = null;
                                }
                                else
                                {
                                    this.messageManager.AddText(string.Empty);
                                }
                                this.isCancelInput = false;
                                this.ModifyPlaySpeed(true);
                                if (this.IsFastPlaySpeed())
                                {
                                    this.messageManager.MessageUpdate();
                                }
                                this.DebugProcessScript(this.executeWaitIndex, this.state);
                                isBusy = false;
                            }
                            else if (!this.messageManager.IsBusy)
                            {
                                if (this.isCancelInput)
                                {
                                    this.isCancelInput = false;
                                }
                                this.waitType = "message2";
                                flag11 = true;
                            }
                            goto Label_6476;

                        case 5:
                            if (!this.messageManager.IsWaitTouch())
                            {
                                this.messageManager.ReturnScroll(this.IsFastPlaySpeed());
                                this.waitType = "message3";
                                flag11 = true;
                            }
                            goto Label_6476;

                        case 6:
                            if (!this.messageManager.IsScroll())
                            {
                                if (this.waitMessage != null)
                                {
                                    this.messageManager.AddText(this.waitMessage);
                                    this.logMessage.AddText(this.waitMessage);
                                    this.waitMessage = null;
                                }
                                else
                                {
                                    this.messageManager.AddText(string.Empty);
                                }
                                this.ModifyPlaySpeed(true);
                                if (this.IsFastPlaySpeed())
                                {
                                    this.messageManager.MessageUpdate();
                                }
                                this.DebugProcessScript(this.executeWaitIndex, this.state);
                                isBusy = false;
                            }
                            goto Label_6476;

                        case 7:
                            if (!this.messageManager.IsBusy)
                            {
                                if (this.isCancelInput)
                                {
                                    this.isCancelInput = false;
                                }
                                else if (this.messageManager.IsWindowMode)
                                {
                                    if ((this.messageManager.IsPageScroll() && !this.IsJumpLine()) && (this.inputTopMode != InputTopMode.AUTO))
                                    {
                                        this.messageManager.WaitNextTouch();
                                    }
                                }
                                else if ((this.messageManager.IsReturnScroll2() && !this.IsJumpLine()) && (this.inputTopMode != InputTopMode.AUTO))
                                {
                                    this.messageManager.WaitNextTouch();
                                }
                                this.waitType = "talkName2";
                                flag11 = true;
                            }
                            goto Label_6476;

                        case 8:
                            if (!this.messageManager.IsWaitTouch())
                            {
                                if (this.messageManager.IsWindowMode)
                                {
                                    if (!this.messageManager.IsChangeTalkName(this.waitMessage))
                                    {
                                        this.messageManager.PageScroll(this.IsFastPlaySpeed());
                                    }
                                }
                                else if (this.messageManager.IsReturnScroll2())
                                {
                                    this.messageManager.ReturnScroll2(this.IsFastPlaySpeed());
                                }
                                this.waitType = "talkName3";
                                flag11 = true;
                            }
                            goto Label_6476;

                        case 9:
                            if (!this.messageManager.IsScroll() && !this.IsWaitTalk())
                            {
                                string str27;
                                string str28;
                                string str29;
                                int num78;
                                if (this.messageManager.IsWindowMode)
                                {
                                    this.messageManager.ClearText();
                                }
                                ScriptMessageLabel.GetTalkName(out str27, out str28, out str29, out num78, this.waitMessage);
                                this.messageManager.SetTalkName(null, str28 + str29);
                                this.logMessage.SetTalkName(str28 + str29);
                                if (num78 >= 0)
                                {
                                    this.SetTalkIndex(num78);
                                }
                                else
                                {
                                    this.SetTalkName(str29);
                                }
                                this.waitMessage = null;
                                this.DebugProcessScript(this.executeWaitIndex, this.state);
                                isBusy = false;
                            }
                            goto Label_6476;

                        case 10:
                            if (!this.messageManager.IsBusy)
                            {
                                if (this.isCancelInput)
                                {
                                    this.isCancelInput = false;
                                }
                                else if (!this.IsJumpLine() && (this.inputTopMode != InputTopMode.AUTO))
                                {
                                    this.messageManager.WaitNextTouch();
                                }
                                this.waitType = "clear2";
                                flag11 = true;
                            }
                            goto Label_6476;

                        case 11:
                            if (!this.messageManager.IsWaitTouch())
                            {
                                this.messageManager.ClearText();
                                this.logMessage.ReturnText();
                                isBusy = false;
                            }
                            goto Label_6476;

                        case 12:
                            if (!this.messageManager.IsBusy)
                            {
                                if (this.isCancelInput)
                                {
                                    this.isCancelInput = false;
                                }
                                else if (!this.IsJumpLine() && (this.inputTopMode != InputTopMode.AUTO))
                                {
                                    this.messageManager.WaitNextTouch();
                                }
                                this.waitType = "page2";
                                flag11 = true;
                            }
                            goto Label_6476;

                        case 13:
                            if (!this.messageManager.IsWaitTouch())
                            {
                                this.messageManager.PageScroll(this.IsFastPlaySpeed());
                                this.waitType = "page3";
                            }
                            goto Label_6476;

                        case 14:
                            if (!this.messageManager.IsScroll())
                            {
                                this.messageManager.ClearText();
                                this.logMessage.ReturnText();
                                isBusy = false;
                            }
                            goto Label_6476;

                        case 15:
                            if (!this.messageManager.IsBusy)
                            {
                                if (!this.IsJumpLine() && (this.inputTopMode != InputTopMode.AUTO))
                                {
                                    this.messageManager.WaitNextTouch();
                                }
                                this.waitType = "touch2";
                            }
                            goto Label_6476;

                        case 0x10:
                            if (!this.messageManager.IsWaitTouch())
                            {
                                this.isCancelInput = true;
                                isBusy = false;
                            }
                            goto Label_6476;

                        case 0x11:
                            if (!this.messageManager.IsBusy)
                            {
                                if (this.isRequestSkip || this.isExecuteSkip)
                                {
                                    isBusy = false;
                                }
                                else
                                {
                                    this.DebugProcessScript(this.executeWaitIndex, this.state);
                                    this.PausePlaySpeed();
                                    this.selectDialog.Open(this.menuMessageList, new ScriptSelectDialog.ClickDelegate(this.OnSelectSwitchCase));
                                    this.waitType = "switchCase2";
                                }
                            }
                            goto Label_6476;

                        case 0x12:
                        case 20:
                        case 0x16:
                        case 0x19:
                            goto Label_6476;

                        case 0x13:
                            if (!this.messageManager.IsBusy)
                            {
                                if (this.isRequestSkip || this.isExecuteSkip)
                                {
                                    isBusy = false;
                                }
                                else
                                {
                                    this.DebugProcessScript(this.executeWaitIndex, this.state);
                                    this.PausePlaySpeed();
                                    this.selectDialog.Open(this.menuMessageList, new ScriptSelectDialog.ClickDelegate(this.OnSelectMenu));
                                    this.waitType = "menu2";
                                }
                            }
                            goto Label_6476;

                        case 0x15:
                            if (!this.messageManager.IsBusy && (this.inputTopMode == InputTopMode.NORMAL))
                            {
                                if (this.isRequestSkip || this.isExecuteSkip)
                                {
                                    isBusy = false;
                                }
                                else
                                {
                                    this.waitType = "notification2";
                                    this.OpenNotificationDialog(this.waitName, this.waitMessage);
                                }
                            }
                            goto Label_6476;

                        case 0x17:
                            isBusy = false;
                            goto Label_6476;

                        case 0x18:
                            if (!this.messageManager.IsBusy && (this.inputTopMode == InputTopMode.NORMAL))
                            {
                                if (this.isRequestSkip || this.isExecuteSkip)
                                {
                                    isBusy = false;
                                }
                                else
                                {
                                    this.waitType = "inputname2";
                                    this.OpenNameInput();
                                }
                            }
                            goto Label_6476;

                        case 0x1a:
                            isBusy = false;
                            goto Label_6476;

                        case 0x1b:
                            isBusy = SoundManager.isPlayBgm(this.waitName);
                            goto Label_6476;

                        case 0x1c:
                            isBusy = SoundManager.isPlayJingle(this.waitName);
                            goto Label_6476;

                        case 0x1d:
                            if (string.IsNullOrEmpty(this.waitName))
                            {
                                isBusy = (this.sePlayer != null) && this.sePlayer.IsBusy;
                            }
                            else
                            {
                                isBusy = SoundManager.isBusySe(this.waitName);
                            }
                            goto Label_6476;

                        case 30:
                            if (!string.IsNullOrEmpty(this.waitName))
                            {
                                isBusy = SoundManager.isBusySe(this.waitName);
                            }
                            else
                            {
                                isBusy = (this.loopSePlayer != null) && this.loopSePlayer.IsBusy;
                            }
                            goto Label_6476;

                        case 0x1f:
                            isBusy = false;
                            if (this.voicePlayer != null)
                            {
                                if (!string.IsNullOrEmpty(this.waitName))
                                {
                                    isBusy = SoundManager.isBusyVoice(this.waitName);
                                }
                                else
                                {
                                    isBusy = (this.voicePlayer != null) && this.voicePlayer.IsBusy;
                                }
                            }
                            goto Label_6476;

                        case 0x20:
                            isBusy = false;
                            if (this.voicePlayer != null)
                            {
                                if (this.inputTopMode != InputTopMode.SKIP_VOICE)
                                {
                                    if (this.inputTopMode == InputTopMode.NORMAL)
                                    {
                                        this.inputTopMode = InputTopMode.SKIP_VOICE;
                                        this.isRequestVoiceCancel = false;
                                        this.touchFullScreen.SetActive(true);
                                    }
                                }
                                else if (this.isRequestVoiceCancel)
                                {
                                    if (string.IsNullOrEmpty(this.waitName))
                                    {
                                        this.voicePlayer.StopSe(0f);
                                    }
                                    else
                                    {
                                        this.voicePlayer.StopSe(0f);
                                    }
                                }
                                if (string.IsNullOrEmpty(this.waitName))
                                {
                                    isBusy = this.voicePlayer.IsBusy;
                                }
                                else
                                {
                                    isBusy = this.voicePlayer.IsBusy;
                                }
                                if (!isBusy && (this.inputTopMode == InputTopMode.SKIP_VOICE))
                                {
                                    this.inputTopMode = InputTopMode.NORMAL;
                                    this.touchFullScreen.SetActive(false);
                                }
                            }
                            goto Label_6476;

                        case 0x21:
                        {
                            RenderTexture targetTexture = this.renderTextureCamera.targetTexture;
                            if (targetTexture == null)
                            {
                                isBusy = false;
                            }
                            else
                            {
                                isBusy = !targetTexture.IsCreated();
                            }
                            goto Label_6476;
                        }
                        case 0x22:
                            isBusy = SingletonMonoBehaviour<CommonUI>.Instance.maskFadeIsBusy();
                            goto Label_6476;

                        case 0x23:
                            isBusy = this.isExecuteFade;
                            goto Label_6476;

                        case 0x24:
                            isBusy = this.isExecuteWipe && !this.isWipeFilter;
                            goto Label_6476;

                        case 0x25:
                            isBusy = this.isLoadWipe;
                            goto Label_6476;

                        case 0x26:
                            isBusy = this.isExecuteFlash;
                            goto Label_6476;

                        case 0x27:
                            isBusy = this.isExecuteStretch;
                            goto Label_6476;

                        case 40:
                            isBusy = this.isExecuteCamera;
                            goto Label_6476;

                        case 0x29:
                            isBusy = this.isBusyScene;
                            goto Label_6476;

                        case 0x2a:
                            isBusy = this.isBusySceneLoad;
                            goto Label_6476;

                        case 0x2b:
                            isBusy = ProgramEffectManager.IsBusy(this.specialEffectBase, this.waitName);
                            goto Label_6476;

                        case 0x2c:
                            isBusy = CommonEffectManager.IsBusy(this.fowardEffectBase, this.waitName);
                            goto Label_6476;

                        case 0x2d:
                            isBusy = !CommonEffectManager.IsStart(this.fowardEffectBase, this.waitName);
                            goto Label_6476;

                        case 0x2e:
                            isBusy = CommonEffectManager.IsBusy(this.effectBase, this.waitName);
                            goto Label_6476;

                        case 0x2f:
                            isBusy = CommonEffectManager.IsBusy(this.backEffectBase, this.waitName);
                            goto Label_6476;

                        case 0x30:
                            if (this.waitIndex < 0)
                            {
                                isBusy = false;
                                for (int i = 0; i < CHARA_MAX; i++)
                                {
                                    ScriptCharaData data30 = this.charaList[i];
                                    if ((data30 != null) && data30.IsLoad())
                                    {
                                        isBusy = true;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                ScriptCharaData data29 = this.charaList[this.waitIndex];
                                if (data29 == null)
                                {
                                    isBusy = false;
                                }
                                else
                                {
                                    isBusy = data29.IsLoad();
                                }
                            }
                            goto Label_6476;

                        case 0x31:
                            if (this.waitIndex < 0)
                            {
                                isBusy = false;
                                for (int j = 0; j < CHARA_MAX; j++)
                                {
                                    ScriptCharaData data32 = this.charaList[j];
                                    if ((data32 != null) && data32.IsChange())
                                    {
                                        isBusy = true;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                ScriptCharaData data31 = this.charaList[this.waitIndex];
                                if (data31 == null)
                                {
                                    isBusy = false;
                                }
                                else
                                {
                                    isBusy = data31.IsChange();
                                }
                            }
                            goto Label_6476;

                        case 50:
                            if (this.waitIndex < 0)
                            {
                                isBusy = false;
                                for (int k = 0; k < CHARA_MAX; k++)
                                {
                                    ScriptCharaData data34 = this.charaList[k];
                                    if ((data34 != null) && data34.IsCut())
                                    {
                                        isBusy = true;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                ScriptCharaData data33 = this.charaList[this.waitIndex];
                                if (data33 == null)
                                {
                                    isBusy = false;
                                }
                                else
                                {
                                    isBusy = data33.IsCut();
                                }
                            }
                            goto Label_6476;

                        case 0x33:
                            if (this.waitIndex < 0)
                            {
                                isBusy = false;
                                for (int m = 0; m < CHARA_MAX; m++)
                                {
                                    ScriptCharaData data36 = this.charaList[m];
                                    if ((data36 != null) && data36.IsMoveAlpha())
                                    {
                                        isBusy = true;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                ScriptCharaData data35 = this.charaList[this.waitIndex];
                                if (data35 == null)
                                {
                                    isBusy = false;
                                }
                                else
                                {
                                    isBusy = data35.IsMoveAlpha();
                                }
                            }
                            goto Label_6476;

                        case 0x34:
                            if (this.waitIndex < 0)
                            {
                                isBusy = false;
                                for (int n = 0; n < CHARA_MAX; n++)
                                {
                                    ScriptCharaData data38 = this.charaList[n];
                                    if ((data38 != null) && data38.IsMove())
                                    {
                                        isBusy = true;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                ScriptCharaData data37 = this.charaList[this.waitIndex];
                                if (data37 == null)
                                {
                                    isBusy = false;
                                }
                                else
                                {
                                    isBusy = data37.IsMove();
                                }
                            }
                            goto Label_6476;

                        case 0x35:
                            isBusy = this.IsWaitTalk();
                            goto Label_6476;

                        case 0x36:
                            if (this.waitIndex < 0)
                            {
                                isBusy = false;
                                for (int num84 = 0; num84 < CHARA_MAX; num84++)
                                {
                                    ScriptCharaData data40 = this.charaList[num84];
                                    if ((data40 != null) && data40.IsSpecialEffect(this.waitName))
                                    {
                                        isBusy = true;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                ScriptCharaData data39 = this.charaList[this.waitIndex];
                                if (data39 == null)
                                {
                                    isBusy = false;
                                }
                                else
                                {
                                    isBusy = data39.IsSpecialEffect(this.waitName);
                                }
                            }
                            goto Label_6476;

                        case 0x37:
                            if (this.waitIndex < 0)
                            {
                                isBusy = false;
                                for (int num85 = 0; num85 < CHARA_MAX; num85++)
                                {
                                    ScriptCharaData data42 = this.charaList[num85];
                                    if ((data42 != null) && data42.IsEffect(this.waitName))
                                    {
                                        isBusy = true;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                ScriptCharaData data41 = this.charaList[this.waitIndex];
                                if (data41 == null)
                                {
                                    isBusy = false;
                                }
                                else
                                {
                                    isBusy = data41.IsEffect(this.waitName);
                                }
                            }
                            goto Label_6476;

                        case 0x38:
                            if (this.waitIndex < 0)
                            {
                                isBusy = false;
                                for (int num86 = 0; num86 < CHARA_MAX; num86++)
                                {
                                    ScriptCharaData data44 = this.charaList[num86];
                                    if ((data44 != null) && data44.IsBackEffect(this.waitName))
                                    {
                                        isBusy = true;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                ScriptCharaData data43 = this.charaList[this.waitIndex];
                                if (data43 == null)
                                {
                                    isBusy = false;
                                }
                                else
                                {
                                    isBusy = data43.IsBackEffect(this.waitName);
                                }
                            }
                            goto Label_6476;

                        case 0x39:
                            isBusy = this.isLoadCommunicationChara;
                            goto Label_6476;

                        case 0x3a:
                            isBusy = CommonEffectManager.IsBusy(this.communicationCharaEffectBase);
                            goto Label_6476;

                        case 0x3b:
                            if (!SingletonMonoBehaviour<CommonUI>.Instance.maskFadeIsBusy())
                            {
                                this.waitType = "startEnd";
                            }
                            goto Label_6476;

                        case 60:
                            switch (this.startMode)
                            {
                                case StartMode.CLEAR_BLACK:
                                case StartMode.CLEAR_WHITE:
                                case StartMode.CLEAR:
                                case StartMode.CLEAR_FULL:
                                case StartMode.CLEAR_BLACK_FULL:
                                case StartMode.CLEAR_WHITE_FULL:
                                    if (SingletonMonoBehaviour<CommonUI>.Instance.maskFadGetFadeoutKind() != MaskFade.Kind.NONE)
                                    {
                                        SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(DEFAULT_FADE_TIME, null);
                                    }
                                    this.waitType = "startFade";
                                    goto Label_6476;

                                case StartMode.BLACK_CLEAR:
                                case StartMode.BLACK:
                                case StartMode.BLACK_FULL:
                                case StartMode.BLACK_SCENE_STOP:
                                    if (SingletonMonoBehaviour<CommonUI>.Instance.maskFadGetFadeoutKind() == MaskFade.Kind.BLACK)
                                    {
                                        this.StartFade("black", false, 0f);
                                    }
                                    else
                                    {
                                        this.StartFade("black", false, DEFAULT_FADE_TIME);
                                    }
                                    this.waitType = "startFade";
                                    goto Label_6476;

                                case StartMode.WHITE_CLEAR:
                                case StartMode.WHITE:
                                case StartMode.WHITE_FULL:
                                    if (SingletonMonoBehaviour<CommonUI>.Instance.maskFadGetFadeoutKind() == MaskFade.Kind.WHITE)
                                    {
                                        this.StartFade("white", false, 0f);
                                    }
                                    else
                                    {
                                        this.StartFade("white", false, DEFAULT_FADE_TIME);
                                    }
                                    this.waitType = "startFade";
                                    goto Label_6476;
                            }
                            this.waitType = "load";
                            goto Label_6476;

                        case 0x3d:
                            if (!SingletonMonoBehaviour<CommonUI>.Instance.maskFadeIsBusy() && !this.isExecuteFade)
                            {
                                SingletonMonoBehaviour<CommonUI>.Instance.maskFadeInit();
                                switch (this.startMode)
                                {
                                    case StartMode.BLACK_CLEAR:
                                    case StartMode.WHITE_CLEAR:
                                    case StartMode.BLACK:
                                    case StartMode.WHITE:
                                    case StartMode.BLACK_FULL:
                                    case StartMode.WHITE_FULL:
                                    case StartMode.BLACK_SCENE_STOP:
                                        this.StartFade("clear", true, 0f);
                                        break;
                                }
                                this.waitType = "startFade2";
                            }
                            goto Label_6476;

                        case 0x3e:
                            if (!this.isExecuteFade)
                            {
                                if (this.startMode == StartMode.BLACK_SCENE_STOP)
                                {
                                    SingletonMonoBehaviour<SceneManager>.Instance.SetTargetRootActive(false);
                                }
                                this.waitType = "load";
                            }
                            goto Label_6476;

                        case 0x3f:
                            if ((this.downloadAssetList == null) || (this.downloadAssetList.Count <= 0))
                            {
                                if ((this.audioAssetList != null) && (this.audioAssetList.Count > 0))
                                {
                                    SingletonMonoBehaviour<CommonUI>.Instance.SetLoadMode(ConnectMark.Mode.LOAD);
                                    this.EndDownloadAssetData();
                                    this.waitType = "load2";
                                }
                                else
                                {
                                    isBusy = false;
                                }
                            }
                            else
                            {
                                SingletonMonoBehaviour<CommonUI>.Instance.SetLoadMode(ConnectMark.Mode.LOAD);
                                AssetManager.downloadAssetStorage(this.downloadAssetList.ToArray(), new System.Action(this.EndDownloadAssetData));
                                this.waitType = "load2";
                            }
                            goto Label_6476;

                        case 0x40:
                            if (!this.isExecuteLoadAsset)
                            {
                                if (!this.isStartModeEnd)
                                {
                                    this.isStartModeEnd = true;
                                }
                                isBusy = false;
                            }
                            goto Label_6476;
                    }
                }
            }
            Debug.LogError(string.Concat(new object[] { "ScriptManager wait type error ", this.waitType, " [", this.assetName, ",", this.scriptLabel, ",", this.executeLineList[this.executeIndex - 1], "] " }));
            this.state = State.ERROR;
            goto Label_6476;
        }
        if (this.isRequestSkip && !this.isExecuteSkip)
        {
            Debug.Log("RequestSkip " + this.inputCommandIndex);
            if (this.inputCommandIndex >= 0)
            {
                if (!this.JumpScript(this.inputCommandIndex))
                {
                    this.isExecuteSkip = true;
                    if (this.JumpScript("skip"))
                    {
                        goto Label_0129;
                    }
                    this.EndExecuteScript();
                    goto Label_65F5;
                }
                this.inputCommandIndex = -1;
                this.isRequestSkip = false;
            }
            else
            {
                this.isExecuteSkip = true;
                if (!this.JumpScript("skip"))
                {
                    this.EndExecuteScript();
                    goto Label_65F5;
                }
            }
        }
    Label_0129:
        if (this.executeIndex >= this.executeIndexMax)
        {
            this.EndExecuteScript();
            goto Label_65F5;
        }
        if (this.playSpeed == PlaySpeed.PAUSE)
        {
            goto Label_65F5;
        }
        if ((this.playScriptJumpLine >= 0) && (this.executeLineList[this.executeIndex] >= this.playScriptJumpLine))
        {
            this.playScriptJumpLine = -1;
        }
        string message = this.executeTagList[this.executeIndex];
        if ((message == null) && this.messageManager.IsBusy)
        {
            goto Label_65F5;
        }
        string data = this.executeDataList[this.executeIndex];
        int num = this.executeLineList[this.executeIndex];
        string[] strArray = this.AnalysParam(data);
        string str3 = null;
        bool flag = false;
        this.executeWaitIndex = this.executeIndex;
        waitType = message;
        if (waitType == null)
        {
            Debug.Log(string.Concat(new object[] { "execute message [", num, "] ", data }));
            this.state = State.WAIT;
            this.waitType = "message";
            this.waitMessage = data;
        }
        else
        {
            if (<>f__switch$map24 == null)
            {
                dictionary = new Dictionary<string, int>(0x70) {
                    { 
                        "label",
                        0
                    },
                    { 
                        "end",
                        1
                    },
                    { 
                        "skip",
                        2
                    },
                    { 
                        "flag",
                        3
                    },
                    { 
                        "jump",
                        4
                    },
                    { 
                        "branch",
                        5
                    },
                    { 
                        "wait",
                        6
                    },
                    { 
                        "screen",
                        7
                    },
                    { 
                        "messageOn",
                        8
                    },
                    { 
                        "messageOff",
                        9
                    },
                    { 
                        "clear",
                        10
                    },
                    { 
                        "page",
                        11
                    },
                    { 
                        "k",
                        12
                    },
                    { 
                        "q",
                        13
                    },
                    { 
                        "switchCase",
                        14
                    },
                    { 
                        "switchCaseEnd",
                        15
                    },
                    { 
                        "menu",
                        0x10
                    },
                    { 
                        "notification",
                        0x11
                    },
                    { 
                        "input",
                        0x12
                    },
                    { 
                        "fontSize",
                        0x13
                    },
                    { 
                        "font",
                        0x13
                    },
                    { 
                        "f",
                        0x13
                    },
                    { 
                        "speed",
                        20
                    },
                    { 
                        "s",
                        20
                    },
                    { 
                        "betweenHeight",
                        0x15
                    },
                    { 
                        "l",
                        0x15
                    },
                    { 
                        "shake",
                        0x16
                    },
                    { 
                        "shakeStop",
                        0x17
                    },
                    { 
                        "messageShake",
                        0x18
                    },
                    { 
                        "messageShakeStop",
                        0x19
                    },
                    { 
                        "soundStopAll",
                        0x1a
                    },
                    { 
                        "bgm",
                        0x1b
                    },
                    { 
                        "bgmStop",
                        0x1c
                    },
                    { 
                        "jingle",
                        0x1d
                    },
                    { 
                        "jingleStop",
                        30
                    },
                    { 
                        "se",
                        0x1f
                    },
                    { 
                        "seLoop",
                        0x20
                    },
                    { 
                        "seStop",
                        0x21
                    },
                    { 
                        "voice",
                        0x22
                    },
                    { 
                        "voiceStop",
                        0x23
                    },
                    { 
                        "capture",
                        0x24
                    },
                    { 
                        "captureRelease",
                        0x25
                    },
                    { 
                        "scene",
                        0x26
                    },
                    { 
                        "maskin",
                        0x27
                    },
                    { 
                        "maskout",
                        0x27
                    },
                    { 
                        "maskOff",
                        40
                    },
                    { 
                        "fadein",
                        0x29
                    },
                    { 
                        "fadeout",
                        0x29
                    },
                    { 
                        "fadeMove",
                        0x2a
                    },
                    { 
                        "fadeOff",
                        0x2b
                    },
                    { 
                        "wipein",
                        0x2c
                    },
                    { 
                        "wipeout",
                        0x2c
                    },
                    { 
                        "wipeFilter",
                        0x2d
                    },
                    { 
                        "wipeOff",
                        0x2e
                    },
                    { 
                        "flashin",
                        0x2f
                    },
                    { 
                        "flashout",
                        0x30
                    },
                    { 
                        "flashOff",
                        0x31
                    },
                    { 
                        "stretchin",
                        50
                    },
                    { 
                        "stretchout",
                        50
                    },
                    { 
                        "stretchOff",
                        0x33
                    },
                    { 
                        "cameraMove",
                        0x34
                    },
                    { 
                        "cameraHome",
                        0x35
                    },
                    { 
                        "specialEffect",
                        0x36
                    },
                    { 
                        "specialEffectStop",
                        0x37
                    },
                    { 
                        "fowardEffect",
                        0x38
                    },
                    { 
                        "fowardEffectStart",
                        0x39
                    },
                    { 
                        "fowardEffectStop",
                        0x3a
                    },
                    { 
                        "effect",
                        0x3b
                    },
                    { 
                        "effectStop",
                        60
                    },
                    { 
                        "backEffect",
                        0x3d
                    },
                    { 
                        "backEffectStop",
                        0x3e
                    },
                    { 
                        "charaSet",
                        0x3f
                    },
                    { 
                        "figureSet",
                        0x3f
                    },
                    { 
                        "equipSet",
                        0x3f
                    },
                    { 
                        "imageSet",
                        0x3f
                    },
                    { 
                        "charaChange",
                        0x40
                    },
                    { 
                        "figureChange",
                        0x40
                    },
                    { 
                        "equipChange",
                        0x41
                    },
                    { 
                        "imageChange",
                        0x41
                    },
                    { 
                        "charaClear",
                        0x42
                    },
                    { 
                        "charaFace",
                        0x43
                    },
                    { 
                        "charaShadow",
                        0x44
                    },
                    { 
                        "charaFilter",
                        0x45
                    },
                    { 
                        "charaPut",
                        70
                    },
                    { 
                        "charaScale",
                        0x47
                    },
                    { 
                        "charaTalk",
                        0x48
                    },
                    { 
                        "charaDepth",
                        0x49
                    },
                    { 
                        "charaSpecialEffect",
                        0x4a
                    },
                    { 
                        "charaSpecialEffectStop",
                        0x4b
                    },
                    { 
                        "charaEffect",
                        0x4c
                    },
                    { 
                        "charaEffectStop",
                        0x4d
                    },
                    { 
                        "charaBackEffect",
                        0x4e
                    },
                    { 
                        "charaBackEffectStop",
                        0x4f
                    },
                    { 
                        "charaCutin",
                        80
                    },
                    { 
                        "charaCutout",
                        0x51
                    },
                    { 
                        "charaCutStop",
                        0x52
                    },
                    { 
                        "charaFadein",
                        0x53
                    },
                    { 
                        "charaFadeout",
                        0x54
                    },
                    { 
                        "charaMove",
                        0x55
                    },
                    { 
                        "charaReturn",
                        0x56
                    },
                    { 
                        "charaMoveReturn",
                        0x57
                    },
                    { 
                        "charaAttack",
                        0x58
                    },
                    { 
                        "charaShake",
                        0x59
                    },
                    { 
                        "charaShakeStop",
                        90
                    },
                    { 
                        "communicationChara",
                        0x5b
                    },
                    { 
                        "communicationCharaLoop",
                        0x5b
                    },
                    { 
                        "communicationCharaFace",
                        0x5c
                    },
                    { 
                        "communicationCharaStop",
                        0x5d
                    },
                    { 
                        "communicationCharaClear",
                        0x5e
                    },
                    { 
                        "talkName",
                        0x5f
                    },
                    { 
                        "talkStart",
                        0x60
                    },
                    { 
                        "talkEnd",
                        0x61
                    }
                };
                <>f__switch$map24 = dictionary;
            }
            if (<>f__switch$map24.TryGetValue(waitType, out num87))
            {
                switch (num87)
                {
                    case 0:
                        if (strArray.Length != 1)
                        {
                            str3 = "parameter be unnecessary";
                        }
                        goto Label_4AE8;

                    case 1:
                        if (strArray.Length != 0)
                        {
                            str3 = "parameter be unnecessary";
                        }
                        else
                        {
                            this.EndExecuteScript();
                        }
                        goto Label_4AE8;

                    case 2:
                        if (strArray.Length != 1)
                        {
                            str3 = "parameter error";
                        }
                        else
                        {
                            bool flag2 = strArray[0] == "true";
                            this.isSkip = flag2;
                            this.skipButton.gameObject.SetActive(flag2);
                            this.isRequestSkip = false;
                        }
                        goto Label_4AE8;

                    case 3:
                        if ((strArray.Length != 3) || (strArray[1] != " "))
                        {
                            str3 = "parameter error";
                        }
                        else
                        {
                            this.SearchFlag(strArray[0]).Set(strArray[2]);
                        }
                        goto Label_4AE8;

                    case 4:
                        if ((strArray.Length != 3) || (strArray[1] != " "))
                        {
                            if (strArray.Length == 1)
                            {
                                this.ExecuteScript(this.ConvertNameString(strArray[0]), null, null);
                                flag = true;
                            }
                            else
                            {
                                str3 = "parameter error";
                            }
                            goto Label_4AE8;
                        }
                        this.LoadExecuteScript(this.ConvertNameString(strArray[0]), this.ConvertNameString(strArray[2]));
                        return;

                    case 5:
                        if ((strArray.Length != 3) || (strArray[1] != " "))
                        {
                            if (strArray.Length == 1)
                            {
                                this.JumpScript(this.ConvertNameString(strArray[0]));
                                flag = true;
                            }
                            else
                            {
                                str3 = "parameter error";
                            }
                        }
                        else if (this.SearchFlag(strArray[2]).Get())
                        {
                            this.JumpScript(this.ConvertNameString(strArray[0]));
                            flag = true;
                        }
                        goto Label_4AE8;

                    case 6:
                        Debug.Log("execute wait " + data);
                        this.state = State.WAIT;
                        delta = 0f;
                        if (strArray.Length >= 1)
                        {
                            this.waitType = strArray[0];
                            str31 = this.waitType;
                            if (str31 != null)
                            {
                                if (<>f__switch$map21 == null)
                                {
                                    dictionary = new Dictionary<string, int>(0x17) {
                                        { 
                                            "charaSet",
                                            0
                                        },
                                        { 
                                            "figureSet",
                                            0
                                        },
                                        { 
                                            "equipSet",
                                            0
                                        },
                                        { 
                                            "imageSet",
                                            0
                                        },
                                        { 
                                            "charaChange",
                                            0
                                        },
                                        { 
                                            "figureChange",
                                            0
                                        },
                                        { 
                                            "equipChange",
                                            0
                                        },
                                        { 
                                            "imageChange",
                                            0
                                        },
                                        { 
                                            "charaFade",
                                            0
                                        },
                                        { 
                                            "charaMove",
                                            0
                                        },
                                        { 
                                            "charaReturn",
                                            0
                                        },
                                        { 
                                            "charaMoveReturn",
                                            0
                                        },
                                        { 
                                            "charaAttack",
                                            0
                                        },
                                        { 
                                            "charaSpecialEffect",
                                            1
                                        },
                                        { 
                                            "charaEffect",
                                            2
                                        },
                                        { 
                                            "charaBackEffect",
                                            2
                                        },
                                        { 
                                            "charaCut",
                                            3
                                        },
                                        { 
                                            "communicatioChara",
                                            4
                                        },
                                        { 
                                            "specialEffect",
                                            5
                                        },
                                        { 
                                            "fowardEffect",
                                            6
                                        },
                                        { 
                                            "effect",
                                            6
                                        },
                                        { 
                                            "backEffect",
                                            6
                                        },
                                        { 
                                            "time",
                                            7
                                        }
                                    };
                                    <>f__switch$map21 = dictionary;
                                }
                                if (<>f__switch$map21.TryGetValue(str31, out num88))
                                {
                                    switch (num88)
                                    {
                                        case 0:
                                            if ((strArray.Length < 3) || (strArray[1] != " "))
                                            {
                                                this.waitIndex = -1;
                                            }
                                            else
                                            {
                                                this.waitIndex = this.ConvertCharaIndex(strArray[2]);
                                            }
                                            goto Label_4AE8;

                                        case 1:
                                            if (((strArray.Length < 5) || (strArray[1] != " ")) || (strArray[3] != " "))
                                            {
                                                if ((strArray.Length == 3) && (strArray[1] == " "))
                                                {
                                                    this.waitIndex = this.ConvertCharaIndex(strArray[2]);
                                                    this.waitName = null;
                                                }
                                                else
                                                {
                                                    this.waitIndex = -1;
                                                    this.waitName = null;
                                                }
                                            }
                                            else
                                            {
                                                this.waitIndex = this.ConvertCharaIndex(strArray[2]);
                                                this.waitName = this.ConvertNameString(strArray[4]);
                                                this.waitCount = 0f;
                                            }
                                            this.isRequestEffect = false;
                                            goto Label_4AE8;

                                        case 2:
                                            if (((strArray.Length < 5) || (strArray[1] != " ")) || (strArray[3] != " "))
                                            {
                                                if ((strArray.Length == 3) && (strArray[1] == " "))
                                                {
                                                    this.waitIndex = this.ConvertCharaIndex(strArray[2]);
                                                    this.waitName = null;
                                                }
                                                else
                                                {
                                                    this.waitIndex = -1;
                                                    this.waitName = null;
                                                }
                                            }
                                            else
                                            {
                                                this.waitIndex = this.ConvertCharaIndex(strArray[2]);
                                                this.waitName = this.ConvertTalkEffectName(strArray[4]);
                                                this.waitCount = 0f;
                                            }
                                            this.isRequestEffect = false;
                                            goto Label_4AE8;

                                        case 3:
                                            if ((strArray.Length != 3) || (strArray[1] != " "))
                                            {
                                                this.waitIndex = -1;
                                                this.waitName = null;
                                            }
                                            else
                                            {
                                                this.waitIndex = this.ConvertCharaIndex(strArray[2]);
                                                this.waitName = null;
                                            }
                                            this.isRequestEffect = false;
                                            goto Label_4AE8;

                                        case 4:
                                            this.waitName = null;
                                            goto Label_4AE8;

                                        case 5:
                                            if ((strArray.Length < 3) || (strArray[1] != " "))
                                            {
                                                this.waitName = null;
                                            }
                                            else
                                            {
                                                this.waitName = this.ConvertNameString(strArray[2]);
                                            }
                                            this.isRequestEffect = false;
                                            goto Label_4AE8;

                                        case 6:
                                            if ((strArray.Length < 3) || (strArray[1] != " "))
                                            {
                                                this.waitName = null;
                                            }
                                            else
                                            {
                                                this.waitName = this.ConvertTalkEffectName(strArray[2]);
                                            }
                                            this.isRequestEffect = false;
                                            goto Label_4AE8;

                                        case 7:
                                            if ((strArray.Length < 3) || (strArray[1] != " "))
                                            {
                                                str3 = "parameter error";
                                            }
                                            else
                                            {
                                                this.waitCount = float.Parse(strArray[2]);
                                                if (this.waitCount != 0f)
                                                {
                                                    if (this.IsFastPlaySpeed())
                                                    {
                                                        if (this.isRequestEffect)
                                                        {
                                                            this.isRequestEffect = false;
                                                            this.waitCount = 0.01f;
                                                        }
                                                        else
                                                        {
                                                            this.waitCount = 0f;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    this.waitName = this.ConvertNameString(strArray[2]);
                                                }
                                            }
                                            goto Label_4AE8;
                                    }
                                }
                            }
                            if (((strArray.Length == 5) && (strArray[1] == " ")) && (strArray[3] == " "))
                            {
                                this.waitName = this.ConvertNameString(strArray[2]);
                                this.waitIndex = int.Parse(strArray[4]);
                            }
                            else if ((strArray.Length == 3) && (strArray[1] == " "))
                            {
                                this.waitName = this.ConvertNameString(strArray[2]);
                                this.waitIndex = -1;
                                this.waitCount = 0f;
                            }
                            else if (strArray.Length == 1)
                            {
                                this.waitName = null;
                                this.waitIndex = -1;
                                this.waitCount = 0f;
                            }
                            else
                            {
                                str3 = "parameter error";
                            }
                        }
                        else
                        {
                            str3 = "parameter error";
                        }
                        goto Label_4AE8;

                    case 7:
                        if ((((strArray.Length != 9) || (strArray[1] != " ")) || ((strArray[3] != " ") || (strArray[5] != " "))) || (strArray[7] != " "))
                        {
                            if (strArray.Length == 1)
                            {
                                string str5 = strArray[0];
                                int x = this.messageManager.SetScreen(0, 0, 0, 0, str5 == "true");
                                this.logMessage.SetHomePosition(x);
                                this.isCancelInput = true;
                                this.state = State.WAIT;
                                this.waitType = "clear";
                            }
                            else
                            {
                                str3 = "parameter error";
                            }
                        }
                        else
                        {
                            string str4 = strArray[0];
                            int num2 = int.Parse(strArray[2]);
                            int y = int.Parse(strArray[4]);
                            int w = int.Parse(strArray[6]);
                            int h = int.Parse(strArray[8]);
                            int num6 = this.messageManager.SetScreen(num2, y, w, h, str4 == "true");
                            this.logMessage.SetHomePosition(num6);
                            this.isCancelInput = true;
                            this.state = State.WAIT;
                            this.waitType = "clear";
                        }
                        goto Label_4AE8;

                    case 8:
                        if (strArray.Length != 0)
                        {
                            str3 = "parameter error";
                        }
                        else
                        {
                            this.messageManager.OffScreen();
                            this.messageManager.ClearText();
                            this.logMessage.ReturnText();
                            this.state = State.WAIT;
                            this.waitType = "message";
                            this.waitMessage = null;
                        }
                        goto Label_4AE8;

                    case 9:
                        if (strArray.Length != 0)
                        {
                            str3 = "parameter error";
                        }
                        else
                        {
                            this.messageManager.OffScreen();
                            this.messageManager.ClearText();
                            this.logMessage.ReturnText();
                        }
                        goto Label_4AE8;

                    case 10:
                        if (strArray.Length != 0)
                        {
                            str3 = "parameter be unnecessary";
                        }
                        else
                        {
                            this.state = State.WAIT;
                            this.waitType = "clear";
                        }
                        goto Label_4AE8;

                    case 11:
                        if (strArray.Length != 0)
                        {
                            str3 = "parameter be unnecessary";
                        }
                        else
                        {
                            this.state = State.WAIT;
                            this.waitType = "page";
                        }
                        goto Label_4AE8;

                    case 12:
                        if (strArray.Length != 0)
                        {
                            str3 = "parameter be unnecessary";
                        }
                        else
                        {
                            this.state = State.WAIT;
                            this.waitType = "touch";
                        }
                        goto Label_4AE8;

                    case 13:
                        goto Label_4AE8;

                    case 14:
                        if (this.isSwithCase)
                        {
                            this.JumpScriptCommand("switchCaseEnd");
                        }
                        else
                        {
                            int executeIndex = this.executeIndex;
                            List<string> list = new List<string>();
                            int index = 0;
                            while (executeIndex < this.executeIndexMax)
                            {
                                string str6 = this.executeTagList[executeIndex];
                                string item = this.executeDataList[executeIndex];
                                if (str6 == "switchCaseEnd")
                                {
                                    this.state = State.WAIT;
                                    this.waitType = message;
                                    flag = true;
                                    break;
                                }
                                if (str6 == "switchCase")
                                {
                                    int length = item.IndexOf(this.codeSwitchCaseSplitString);
                                    if (length > 0)
                                    {
                                        int num11 = int.Parse(item.Substring(0, length));
                                        if (num11 > 0)
                                        {
                                            if (num11 > list.Count)
                                            {
                                                for (int num12 = list.Count; num12 < num11; num12++)
                                                {
                                                    list.Add(string.Empty);
                                                }
                                            }
                                            list[num11 - 1] = item.Substring(length + 1);
                                        }
                                    }
                                    else
                                    {
                                        list.Add(item);
                                    }
                                }
                                if (((this.playScriptJumpLine >= 0) && (this.executeLineList[executeIndex] == this.playScriptJumpLine)) && (list.Count > 0))
                                {
                                    index = list.Count - 1;
                                }
                                executeIndex++;
                            }
                            if (this.state == State.WAIT)
                            {
                                if (this.playScriptJumpLine >= 0)
                                {
                                    this.logMessage.AddText("[FF4040]Select[r][FF4040]  " + list[index] + "[r]");
                                    this.isCancelInput = true;
                                    if (!this.JumpScriptSwitchCase(index))
                                    {
                                        str3 = "structure error";
                                    }
                                }
                                else
                                {
                                    this.menuMessageList = list.ToArray();
                                }
                            }
                            else
                            {
                                str3 = "structure error";
                            }
                        }
                        goto Label_4AE8;

                    case 15:
                        if (!this.isSwithCase)
                        {
                            str3 = "structure error";
                        }
                        else
                        {
                            this.isSwithCase = false;
                        }
                        goto Label_4AE8;

                    case 0x10:
                    {
                        List<string> list2 = new List<string>();
                        List<string> list3 = new List<string>();
                        int num13 = 0;
                        while (num13 < strArray.Length)
                        {
                            if (strArray[num13] == " ")
                            {
                                num13++;
                            }
                            else if (((num13 + 2) < strArray.Length) && (strArray[num13 + 1] == "="))
                            {
                                list2.Add(this.ConvertNameString(strArray[num13]));
                                list3.Add(this.ConvertNameString(strArray[num13 + 2]));
                                num13 += 3;
                            }
                            else
                            {
                                str3 = "parameter error";
                                break;
                            }
                        }
                        if (list2.Count > 0)
                        {
                            this.state = State.WAIT;
                            this.waitType = message;
                            this.menuMessageList = list3.ToArray();
                            this.menuLabelList = list2.ToArray();
                        }
                        goto Label_4AE8;
                    }
                    case 0x11:
                    {
                        string str8 = ((strArray.Length <= 2) || (strArray[1] != " ")) ? scriptNotificationMessage : this.ConvertNameString(strArray[2]);
                        if (!string.IsNullOrEmpty(str8))
                        {
                            this.state = State.WAIT;
                            this.waitType = message;
                            this.waitName = (strArray.Length <= 0) ? "normal" : this.ConvertNameString(strArray[0]);
                            this.waitMessage = str8;
                        }
                        goto Label_4AE8;
                    }
                    case 0x12:
                        if (strArray.Length != 1)
                        {
                            str3 = "parameter error";
                        }
                        else
                        {
                            this.state = State.WAIT;
                            this.waitType = "input" + strArray[0];
                        }
                        goto Label_4AE8;

                    case 0x13:
                        if (strArray.Length != 1)
                        {
                            if (strArray.Length == 0)
                            {
                                this.messageManager.SetFontSize(null);
                                this.logMessage.SetFontSize(null);
                            }
                            else
                            {
                                str3 = "parameter error";
                            }
                        }
                        else
                        {
                            this.messageManager.SetFontSize(strArray[0]);
                            this.logMessage.SetFontSize(strArray[0]);
                        }
                        goto Label_4AE8;

                    case 20:
                        if (strArray.Length != 1)
                        {
                            if (strArray.Length == 0)
                            {
                                this.messageManager.SetSpeed(-1f);
                            }
                            else
                            {
                                str3 = "parameter error";
                            }
                        }
                        else
                        {
                            float num14 = (strArray[0] != "-") ? float.Parse(strArray[0]) : -1f;
                            this.messageManager.SetSpeed(num14);
                        }
                        goto Label_4AE8;

                    case 0x15:
                        if (strArray.Length != 1)
                        {
                            if (strArray.Length == 0)
                            {
                                this.messageManager.SetBetweenLineHeight(-1f);
                                this.logMessage.SetBetweenLineHeight(-1f);
                            }
                            else
                            {
                                str3 = "parameter error";
                            }
                        }
                        else
                        {
                            float height = (strArray[0] != "-") ? float.Parse(strArray[0]) : -1f;
                            this.messageManager.SetBetweenLineHeight(height);
                            this.logMessage.SetBetweenLineHeight(height);
                        }
                        goto Label_4AE8;

                    case 0x16:
                        if (((strArray.Length >= 7) && (strArray[1] == " ")) && ((strArray[3] == " ") && (strArray[5] == " ")))
                        {
                            float cycle = float.Parse(strArray[0]);
                            if (cycle == 0f)
                            {
                                cycle = 0.01666667f;
                            }
                            float duration = float.Parse(strArray[6]);
                            if (this.IsFastPlaySpeed() && (duration > 0f))
                            {
                                cycle = 0f;
                            }
                            this.Shake(duration, cycle, float.Parse(strArray[2]), float.Parse(strArray[4]));
                        }
                        else
                        {
                            str3 = "parameter error";
                        }
                        goto Label_4AE8;

                    case 0x17:
                        this.Shake(0f, 0f, 0f, 0f);
                        goto Label_4AE8;

                    case 0x18:
                        if (((strArray.Length >= 7) && (strArray[1] == " ")) && ((strArray[3] == " ") && (strArray[5] == " ")))
                        {
                            float num18 = float.Parse(strArray[0]);
                            if (num18 == 0f)
                            {
                                num18 = 0.01666667f;
                            }
                            float num19 = float.Parse(strArray[6]);
                            if (this.IsFastPlaySpeed() && (num19 > 0f))
                            {
                                num18 = 0f;
                            }
                            this.messageManager.Shake(num19, num18, float.Parse(strArray[2]), float.Parse(strArray[4]));
                        }
                        else
                        {
                            str3 = "parameter error";
                        }
                        goto Label_4AE8;

                    case 0x19:
                        this.messageManager.Shake(0f, 0f, 0f, 0f);
                        goto Label_4AE8;

                    case 0x1a:
                        this.SoundStopAll();
                        goto Label_4AE8;

                    case 0x1b:
                        if (strArray.Length != 1)
                        {
                            if ((strArray.Length == 3) && (strArray[1] == " "))
                            {
                                Debug.Log("bgm " + strArray[0] + " " + strArray[2]);
                                SoundManager.playBgm(this.ConvertNameString(strArray[0]), 1f, float.Parse(strArray[2]));
                            }
                            else
                            {
                                str3 = "parameter error";
                            }
                        }
                        else
                        {
                            Debug.Log("bgm " + strArray[0]);
                            SoundManager.playBgm(this.ConvertNameString(strArray[0]));
                        }
                        goto Label_4AE8;

                    case 0x1c:
                        if (strArray.Length != 0)
                        {
                            if (strArray.Length == 1)
                            {
                                Debug.Log("bgmStop " + strArray[0]);
                                if (SoundManager.isPlayBgm(strArray[0]))
                                {
                                    SoundManager.stopBgm();
                                }
                            }
                            else if ((strArray.Length == 3) && (strArray[1] == " "))
                            {
                                Debug.Log("bgmStop " + strArray[0] + " " + strArray[2]);
                                if (SoundManager.isPlayBgm(strArray[0]))
                                {
                                    SoundManager.fadeoutBgm(float.Parse(strArray[2]));
                                }
                            }
                            else
                            {
                                str3 = "parameter error";
                            }
                        }
                        else
                        {
                            Debug.Log("bgmStop");
                            SoundManager.stopBgm();
                        }
                        goto Label_4AE8;

                    case 0x1d:
                        if (strArray.Length != 1)
                        {
                            str3 = "parameter error";
                        }
                        else
                        {
                            Debug.Log("jingle " + strArray[0]);
                            if (this.IsNormalPlaySpeed())
                            {
                                SoundManager.playJingle(this.ConvertNameString(strArray[0]));
                            }
                        }
                        goto Label_4AE8;

                    case 30:
                        if (strArray.Length != 0)
                        {
                            if (strArray.Length == 1)
                            {
                                Debug.Log("jingleStop " + strArray[0]);
                                if (SoundManager.isPlayJingle(strArray[0]))
                                {
                                    SoundManager.stopJingle();
                                }
                            }
                            else
                            {
                                str3 = "parameter error";
                            }
                        }
                        else
                        {
                            Debug.Log("jingleStop");
                            SoundManager.stopJingle();
                        }
                        goto Label_4AE8;

                    case 0x1f:
                        if (strArray.Length != 1)
                        {
                            str3 = "parameter error";
                        }
                        else
                        {
                            Debug.Log("se " + strArray[0]);
                            if (this.IsNormalPlaySpeed())
                            {
                                this.sePlayer = SoundManager.playSe(this.ConvertNameString(strArray[0]));
                            }
                        }
                        goto Label_4AE8;

                    case 0x20:
                        if (strArray.Length != 1)
                        {
                            str3 = "parameter error";
                        }
                        else
                        {
                            Debug.Log("seLoop " + strArray[0]);
                            this.loopSePlayer = SoundManager.playSeLoop(this.ConvertNameString(strArray[0]));
                        }
                        goto Label_4AE8;

                    case 0x21:
                        if (strArray.Length != 0)
                        {
                            if (strArray.Length >= 1)
                            {
                                Debug.Log("seStop " + strArray[0]);
                                if ((strArray.Length == 3) && (strArray[1] == " "))
                                {
                                    SoundManager.stopSe(strArray[0], float.Parse(strArray[2]));
                                }
                                else
                                {
                                    SoundManager.stopSe(strArray[0], 0f);
                                }
                            }
                            else
                            {
                                str3 = "parameter error";
                            }
                        }
                        else
                        {
                            Debug.Log("seStop");
                            SoundManager.stopSe(0f);
                        }
                        goto Label_4AE8;

                    case 0x22:
                        if (strArray.Length != 1)
                        {
                            str3 = "parameter error";
                        }
                        else
                        {
                            Debug.Log("voice " + strArray[0]);
                            if (this.IsNormalPlaySpeed() && (templateVoiceList != null))
                            {
                                this.voicePlayer = SoundManager.playCharaVoice(this.ConvertNameString(strArray[0]));
                            }
                        }
                        goto Label_4AE8;

                    case 0x23:
                        if (strArray.Length != 0)
                        {
                            if (strArray.Length > 1)
                            {
                                Debug.Log("voiceStop " + strArray[0]);
                                if ((strArray.Length == 3) && (strArray[1] == " "))
                                {
                                    this.voicePlayer.StopSe(float.Parse(strArray[2]));
                                }
                                else
                                {
                                    this.voicePlayer.StopSe(0f);
                                }
                            }
                            else
                            {
                                str3 = "parameter error";
                            }
                        }
                        else
                        {
                            Debug.Log("voiceStop");
                            SoundManager.stopSe(0f);
                            if (this.voicePlayer != null)
                            {
                                this.voicePlayer.StopSe(0f);
                            }
                        }
                        goto Label_4AE8;

                    case 0x24:
                    {
                        RenderTexture texture = this.renderTextureCamera.targetTexture;
                        if (texture != null)
                        {
                            if (!texture.IsCreated())
                            {
                                this.state = State.WAIT;
                                this.waitType = "capture";
                                flag = true;
                            }
                            else
                            {
                                this.meshCaptureBase.ClearImage();
                                this.captureTexture = this.RenderCameraSwap(this.captureTexture);
                                if (this.captureTexture != null)
                                {
                                    this.meshCaptureBase.SetImage(this.captureTexture);
                                    this.meshCaptureBase.SetTweenColor(new Color(1f, 1f, 1f, 0f));
                                }
                            }
                        }
                        else
                        {
                            str3 = "capture error";
                        }
                        goto Label_4AE8;
                    }
                    case 0x25:
                        if (this.captureTexture != null)
                        {
                            this.meshCaptureBase.SetTweenColor(Color.clear);
                            this.meshCaptureBase.ClearImage();
                            this.captureTexture.Release();
                            UnityEngine.Object.Destroy(this.captureTexture);
                            this.captureTexture = null;
                        }
                        goto Label_4AE8;

                    case 0x26:
                        if (!this.isBusySceneLoad)
                        {
                            if (this.isBusyScene)
                            {
                                this.state = State.WAIT;
                                this.waitType = "scene";
                                flag = true;
                            }
                            else if (strArray.Length >= 1)
                            {
                                Debug.Log("scene " + strArray[0]);
                                this.sceneCrossFadeTime = ((!this.IsNormalPlaySpeed() || (strArray.Length < 3)) || (strArray[1] != " ")) ? 0f : float.Parse(strArray[2]);
                                this.SetSceneImage(this.ConvertBackTextureName(strArray[0]));
                                this.state = State.WAIT;
                                this.waitType = "sceneLoad";
                            }
                            else
                            {
                                str3 = "parameter error";
                            }
                        }
                        else
                        {
                            this.state = State.WAIT;
                            this.waitType = "sceneLoad";
                            flag = true;
                        }
                        goto Label_4AE8;

                    case 0x27:
                        if (!this.isExecuteFade)
                        {
                            if (strArray.Length >= 1)
                            {
                                Debug.Log(message + " " + data);
                                float num20 = ((!this.IsNormalPlaySpeed() || (strArray.Length != 3)) || (strArray[1] != " ")) ? 0f : float.Parse(strArray[2]);
                                this.StartMask(this.ConvertNameString(strArray[0]), message == "maskin", num20);
                            }
                            else
                            {
                                str3 = "parameter error";
                            }
                        }
                        else
                        {
                            this.state = State.WAIT;
                            this.waitType = "mask";
                            flag = true;
                        }
                        goto Label_4AE8;

                    case 40:
                        this.InitMask();
                        goto Label_4AE8;

                    case 0x29:
                        if (!this.isExecuteFade)
                        {
                            if (strArray.Length >= 1)
                            {
                                Debug.Log(message + " " + data);
                                float num21 = ((!this.IsNormalPlaySpeed() || (strArray.Length != 3)) || (strArray[1] != " ")) ? 0f : float.Parse(strArray[2]);
                                this.StartFade(this.ConvertNameString(strArray[0]), message == "fadein", num21);
                            }
                            else
                            {
                                str3 = "parameter error";
                            }
                        }
                        else
                        {
                            this.state = State.WAIT;
                            this.waitType = "fade";
                            flag = true;
                        }
                        goto Label_4AE8;

                    case 0x2a:
                        if (!this.isExecuteFade)
                        {
                            if (((strArray.Length == 5) && (strArray[1] == " ")) && (strArray[3] == " "))
                            {
                                Debug.Log(message + " " + data);
                                float num22 = !this.IsNormalPlaySpeed() ? 0f : float.Parse(strArray[2]);
                                this.StartFadeMove(this.ConvertNameString(strArray[0]), num22, float.Parse(strArray[4]));
                            }
                            else
                            {
                                str3 = "parameter error";
                            }
                        }
                        else
                        {
                            this.state = State.WAIT;
                            this.waitType = "fade";
                            flag = true;
                        }
                        goto Label_4AE8;

                    case 0x2b:
                        this.InitFade();
                        goto Label_4AE8;

                    case 0x2c:
                        if (!this.isExecuteWipe)
                        {
                            if (strArray.Length >= 1)
                            {
                                Debug.Log(message + " " + data);
                                float num23 = ((!this.IsNormalPlaySpeed() || (strArray.Length < 3)) || (strArray[1] != " ")) ? 0f : float.Parse(strArray[2]);
                                this.StartWipe(this.ConvertNameString(strArray[0]), message == "wipein", num23, ((strArray.Length < 5) || (strArray[3] != " ")) ? 0f : float.Parse(strArray[4]));
                                this.state = State.WAIT;
                                this.waitType = "wipeLoad";
                            }
                            else
                            {
                                str3 = "parameter error";
                            }
                        }
                        else
                        {
                            this.state = State.WAIT;
                            this.waitType = "wipe";
                            flag = true;
                        }
                        goto Label_4AE8;

                    case 0x2d:
                        if (!this.isExecuteWipe || this.isWipeFilter)
                        {
                            if (strArray.Length >= 1)
                            {
                                Debug.Log(message + " " + data);
                                float num24 = ((!this.IsNormalPlaySpeed() || (strArray.Length < 3)) || (strArray[1] != " ")) ? 0f : float.Parse(strArray[2]);
                                this.SetWipeFilter(this.ConvertNameString(strArray[0]), num24, ((strArray.Length < 5) || (strArray[3] != " ")) ? 0f : float.Parse(strArray[4]));
                                this.state = State.WAIT;
                                this.waitType = "wipeLoad";
                            }
                            else
                            {
                                this.ResetWipeFilter();
                            }
                        }
                        else
                        {
                            this.state = State.WAIT;
                            this.waitType = "wipe";
                            flag = true;
                        }
                        goto Label_4AE8;

                    case 0x2e:
                        this.InitWipe();
                        goto Label_4AE8;

                    case 0x2f:
                        if (!this.isExecuteFlash)
                        {
                            if ((((strArray.Length == 9) && (strArray[1] == " ")) && ((strArray[3] == " ") && (strArray[5] == " "))) && (strArray[7] == " "))
                            {
                                Debug.Log(message + " " + data);
                                string name = this.ConvertNameString(strArray[0]);
                                if (this.IsFastPlaySpeed() && (name == "once"))
                                {
                                    this.StartFlash(name, 0f, 0f, this.ConvertColor(strArray[6]), this.ConvertColor(strArray[8]));
                                }
                                else
                                {
                                    this.StartFlash(name, float.Parse(strArray[2]), float.Parse(strArray[4]), this.ConvertColor(strArray[6]), this.ConvertColor(strArray[8]));
                                }
                            }
                            else
                            {
                                str3 = "parameter error";
                            }
                        }
                        else
                        {
                            this.EndFlash(0f);
                            this.state = State.WAIT;
                            this.waitType = "flash";
                            flag = true;
                        }
                        goto Label_4AE8;

                    case 0x30:
                        if (strArray.Length > 1)
                        {
                            str3 = "parameter error";
                        }
                        else
                        {
                            Debug.Log(message);
                            this.EndFlash((!this.IsNormalPlaySpeed() || (strArray.Length < 1)) ? 0f : float.Parse(strArray[0]));
                        }
                        goto Label_4AE8;

                    case 0x31:
                        this.InitFlash();
                        goto Label_4AE8;

                    case 50:
                        if (!this.isExecuteStretch)
                        {
                            if (strArray.Length >= 1)
                            {
                                Debug.Log(message + " " + data);
                                float num25 = ((!this.IsNormalPlaySpeed() || (strArray.Length < 3)) || (strArray[1] != " ")) ? 0f : float.Parse(strArray[2]);
                                this.StartStretch(this.ConvertNameString(strArray[0]), message == "stretchin", num25, ((strArray.Length < 5) || (strArray[3] != " ")) ? 1f : float.Parse(strArray[4]));
                            }
                            else
                            {
                                str3 = "parameter error";
                            }
                        }
                        else
                        {
                            this.state = State.WAIT;
                            this.waitType = "stretch";
                            flag = true;
                        }
                        goto Label_4AE8;

                    case 0x33:
                        this.InitStretch();
                        goto Label_4AE8;

                    case 0x34:
                        if ((strArray.Length < 3) || (strArray[1] != " "))
                        {
                            str3 = "parameter error";
                        }
                        else
                        {
                            float num26 = !this.IsNormalPlaySpeed() ? 0f : float.Parse(strArray[0]);
                            Vector3 zero = Vector3.zero;
                            float scale = 1f;
                            if ((strArray.Length < 5) || (strArray[3][0] != ','))
                            {
                                zero = ScriptPosition.GetPosition(num26, (float) int.Parse(strArray[2]));
                                if ((strArray.Length >= 5) && (strArray[3][0] == ' '))
                                {
                                    scale = float.Parse(strArray[4]);
                                }
                            }
                            else
                            {
                                zero = ScriptPosition.GetPosition((float) int.Parse(strArray[2]), (float) int.Parse(strArray[4]));
                                if ((strArray.Length >= 7) && (strArray[5][0] == ' '))
                                {
                                    scale = float.Parse(strArray[6]);
                                }
                            }
                            this.StartCamera(num26, zero, scale);
                        }
                        goto Label_4AE8;

                    case 0x35:
                        if (strArray.Length < 1)
                        {
                            this.InitCamera();
                        }
                        else
                        {
                            float num28 = !this.IsNormalPlaySpeed() ? 0f : float.Parse(strArray[0]);
                            Vector3 position = Vector3.zero;
                            float num29 = 1f;
                            this.StartCamera(num28, position, num29);
                        }
                        goto Label_4AE8;

                    case 0x36:
                        if (strArray.Length < 1)
                        {
                            str3 = "parameter error";
                        }
                        else
                        {
                            string effectName = this.ConvertNameString(strArray[0]);
                            Debug.Log("create specialEffect " + effectName);
                            Vector3 pos = (Vector3) Vector2.zero;
                            float time = 0f;
                            Color clear = Color.clear;
                            if ((strArray.Length >= 3) && (strArray[1] == " "))
                            {
                                if ((strArray.Length < 5) || (strArray[3] != ","))
                                {
                                    pos = ScriptPosition.GetPosition(int.Parse(strArray[2]));
                                    if ((strArray.Length >= 5) && (strArray[3] == " "))
                                    {
                                        time = float.Parse(strArray[4]);
                                        if ((strArray.Length >= 7) && (strArray[5] == " "))
                                        {
                                            clear = this.ConvertColor(strArray[6]);
                                        }
                                    }
                                }
                                else
                                {
                                    pos = ScriptPosition.GetPosition((float) int.Parse(strArray[2]), (float) int.Parse(strArray[4]));
                                    if ((strArray.Length >= 7) && (strArray[5] == " "))
                                    {
                                        time = float.Parse(strArray[6]);
                                        if ((strArray.Length >= 9) && (strArray[7] == " "))
                                        {
                                            clear = this.ConvertColor(strArray[8]);
                                        }
                                    }
                                }
                            }
                            GameObject obj2 = ProgramEffectManager.CreateMainEffect(this.specialEffectBase, effectName, pos, time, clear, this.IsFastPlaySpeed());
                            if (obj2 != null)
                            {
                                this.isRequestEffect = true;
                                str31 = effectName;
                                if (str31 != null)
                                {
                                    if (<>f__switch$map22 == null)
                                    {
                                        dictionary = new Dictionary<string, int>(2) {
                                            { 
                                                "cutting",
                                                0
                                            },
                                            { 
                                                "flash",
                                                1
                                            }
                                        };
                                        <>f__switch$map22 = dictionary;
                                    }
                                    if (<>f__switch$map22.TryGetValue(str31, out num88))
                                    {
                                        if (num88 == 0)
                                        {
                                            obj2.GetComponent<CuttingEffectComponent>().CuttingStart(this.captureTexture);
                                        }
                                        else if (num88 == 1)
                                        {
                                            obj2.GetComponent<FlashEffectComponent>().FlashStart();
                                        }
                                    }
                                }
                            }
                        }
                        goto Label_4AE8;

                    case 0x37:
                        if (strArray.Length < 1)
                        {
                            Debug.Log("stop specialEffect");
                            ProgramEffectManager.Stop(this.specialEffectBase);
                        }
                        else
                        {
                            string str11 = this.ConvertNameString(strArray[0]);
                            Debug.Log("stop specialEffect " + str11);
                            ProgramEffectManager.Stop(this.specialEffectBase, str11);
                        }
                        goto Label_4AE8;

                    case 0x38:
                        if (strArray.Length < 1)
                        {
                            str3 = "parameter error";
                        }
                        else
                        {
                            string str12 = this.ConvertTalkEffectName(strArray[0]);
                            Debug.Log("create fowardEffect " + str12);
                            object effectParameter = this.GetEffectParameter(str12);
                            bool isSkip = this.IsFastPlaySpeed();
                            this.isRequestEffect = true;
                            if (((strArray.Length < 5) || (strArray[1] != " ")) || (strArray[3] != ","))
                            {
                                if ((strArray.Length >= 3) && (strArray[1] == " "))
                                {
                                    CommonEffectManager.CreateParam(this.fowardEffectBase, str12, ScriptPosition.GetPosition(int.Parse(strArray[2])), effectParameter, isSkip);
                                }
                                else
                                {
                                    CommonEffectManager.CreateParam(this.fowardEffectBase, str12, effectParameter, isSkip);
                                }
                            }
                            else
                            {
                                CommonEffectManager.CreateParam(this.fowardEffectBase, str12, ScriptPosition.GetPosition((float) int.Parse(strArray[2]), (float) int.Parse(strArray[4])), effectParameter, isSkip);
                            }
                        }
                        goto Label_4AE8;

                    case 0x39:
                        if (strArray.Length < 1)
                        {
                            Debug.Log("start fowardEffect");
                            CommonEffectManager.Resume(this.fowardEffectBase, this.IsFastPlaySpeed());
                        }
                        else
                        {
                            string str13 = this.ConvertTalkEffectName(strArray[0]);
                            Debug.Log("start fowardEffect " + str13);
                            CommonEffectManager.Resume(this.fowardEffectBase, str13, this.IsFastPlaySpeed());
                        }
                        goto Label_4AE8;

                    case 0x3a:
                        if (strArray.Length < 1)
                        {
                            Debug.Log("stop fowardEffect");
                            if (!CommonEffectManager.Stop(this.fowardEffectBase, this.IsFastPlaySpeed(), false))
                            {
                                this.state = State.WAIT;
                                this.waitType = "frame";
                                this.waitCount = 1f;
                                flag = true;
                            }
                        }
                        else
                        {
                            string str14 = this.ConvertTalkEffectName(strArray[0]);
                            Debug.Log("stop fowardEffect " + str14);
                            if (!CommonEffectManager.Stop(this.fowardEffectBase, str14, this.IsFastPlaySpeed(), false))
                            {
                                this.state = State.WAIT;
                                this.waitType = "frame";
                                this.waitCount = 1f;
                                flag = true;
                            }
                        }
                        goto Label_4AE8;

                    case 0x3b:
                        if (strArray.Length < 1)
                        {
                            str3 = "parameter error";
                        }
                        else
                        {
                            string str15 = this.ConvertTalkEffectName(strArray[0]);
                            Debug.Log("create effect " + str15);
                            object obj4 = this.GetEffectParameter(str15);
                            bool flag4 = this.IsFastPlaySpeed();
                            this.isRequestEffect = true;
                            if (((strArray.Length < 5) || (strArray[1] != " ")) || (strArray[3] != ","))
                            {
                                if ((strArray.Length >= 3) && (strArray[1] == " "))
                                {
                                    CommonEffectManager.CreateParam(this.effectBase, str15, ScriptPosition.GetPosition(int.Parse(strArray[2])), obj4, flag4);
                                }
                                else
                                {
                                    CommonEffectManager.CreateParam(this.effectBase, str15, obj4, flag4);
                                }
                            }
                            else
                            {
                                CommonEffectManager.CreateParam(this.effectBase, str15, ScriptPosition.GetPosition((float) int.Parse(strArray[2]), (float) int.Parse(strArray[4])), obj4, flag4);
                            }
                        }
                        goto Label_4AE8;

                    case 60:
                        if (strArray.Length >= 1)
                        {
                            string str16 = this.ConvertTalkEffectName(strArray[0]);
                            Debug.Log("stop effect " + str16);
                            if (str16 == "bit_talk_06")
                            {
                                Debug.Log("stop test");
                            }
                            if (!CommonEffectManager.Stop(this.effectBase, str16, this.IsFastPlaySpeed(), false))
                            {
                                this.state = State.WAIT;
                                this.waitType = "frame";
                                this.waitCount = 1f;
                                flag = true;
                            }
                        }
                        else
                        {
                            Debug.Log("stop effect");
                            if (!CommonEffectManager.Stop(this.effectBase, this.IsFastPlaySpeed(), false))
                            {
                                this.state = State.WAIT;
                                this.waitType = "frame";
                                this.waitCount = 1f;
                                flag = true;
                            }
                        }
                        goto Label_4AE8;

                    case 0x3d:
                        if (strArray.Length < 1)
                        {
                            str3 = "parameter error";
                        }
                        else
                        {
                            string str17 = this.ConvertTalkEffectName(strArray[0]);
                            Debug.Log("create backEffect " + str17);
                            object obj5 = this.GetEffectParameter(str17);
                            bool flag5 = this.IsFastPlaySpeed();
                            this.isRequestEffect = true;
                            if (((strArray.Length < 5) || (strArray[1] != " ")) || (strArray[3] != ","))
                            {
                                if ((strArray.Length >= 3) && (strArray[1] == " "))
                                {
                                    CommonEffectManager.CreateParam(this.backEffectBase, str17, ScriptPosition.GetPosition(int.Parse(strArray[2])), obj5, flag5);
                                }
                                else
                                {
                                    CommonEffectManager.CreateParam(this.backEffectBase, str17, obj5, flag5);
                                }
                            }
                            else
                            {
                                CommonEffectManager.CreateParam(this.backEffectBase, str17, ScriptPosition.GetPosition((float) int.Parse(strArray[2]), (float) int.Parse(strArray[4])), obj5, flag5);
                            }
                        }
                        goto Label_4AE8;

                    case 0x3e:
                        if (strArray.Length < 1)
                        {
                            Debug.Log("stop backEffect");
                            if (!CommonEffectManager.Stop(this.backEffectBase, this.IsFastPlaySpeed(), false))
                            {
                                this.state = State.WAIT;
                                this.waitType = "frame";
                                this.waitCount = 1f;
                                flag = true;
                            }
                        }
                        else
                        {
                            string str18 = this.ConvertTalkEffectName(strArray[0]);
                            Debug.Log("stop backEffect " + str18);
                            if (!CommonEffectManager.Stop(this.backEffectBase, str18, this.IsFastPlaySpeed(), false))
                            {
                                this.state = State.WAIT;
                                this.waitType = "frame";
                                this.waitCount = 1f;
                                flag = true;
                            }
                        }
                        goto Label_4AE8;

                    case 0x3f:
                        if ((strArray.Length < 3) || (strArray[1] != " "))
                        {
                            str3 = "parameter error";
                        }
                        else
                        {
                            num31 = this.ConvertCharaIndex(strArray[0]);
                            data3 = this.charaList[num31];
                            if (data3 == null)
                            {
                                goto Label_2F24;
                            }
                            if (!data3.IsLoad())
                            {
                                data3.Destroy();
                                this.charaList[num31] = null;
                                goto Label_2F24;
                            }
                            this.state = State.WAIT;
                            this.waitType = message;
                            this.waitIndex = num31;
                            flag = true;
                        }
                        goto Label_4AE8;

                    case 0x40:
                        if (strArray.Length < 1)
                        {
                            str3 = "parameter error";
                        }
                        else
                        {
                            int num32 = this.ConvertCharaIndex(strArray[0]);
                            ScriptCharaData data4 = this.charaList[num32];
                            if (data4 == null)
                            {
                                Debug.LogWarning("ScriptCharData " + strArray[0] + " is null");
                            }
                            else if (!data4.IsLoad())
                            {
                                if ((((strArray.Length == 9) && (strArray[1] == " ")) && ((strArray[3] == " ") && (strArray[5] == " "))) && (strArray[7] == " "))
                                {
                                    if (this.IsNormalPlaySpeed())
                                    {
                                        data4.ChangeCharacter(strArray[6], float.Parse(strArray[8]), this.ConvertNameString(strArray[2]), int.Parse(strArray[4]));
                                    }
                                    else
                                    {
                                        data4.SetCharacter(this.ConvertNameString(strArray[2]));
                                        data4.SetFace(int.Parse(strArray[4]));
                                    }
                                    this.state = State.WAIT;
                                    this.waitIndex = num32;
                                    this.waitType = "charaSet";
                                }
                                else
                                {
                                    str3 = "parameter error";
                                }
                            }
                            else
                            {
                                this.state = State.WAIT;
                                this.waitType = "charaChange";
                                this.waitIndex = num32;
                                flag = true;
                            }
                        }
                        goto Label_4AE8;

                    case 0x41:
                        if (strArray.Length < 1)
                        {
                            str3 = "parameter error";
                        }
                        else
                        {
                            int num33 = this.ConvertCharaIndex(strArray[0]);
                            ScriptCharaData data5 = this.charaList[num33];
                            if (data5 == null)
                            {
                                Debug.LogWarning("ScriptCharData " + strArray[0] + " is null");
                            }
                            else if (!data5.IsLoad())
                            {
                                if (((strArray.Length == 7) && (strArray[1] == " ")) && ((strArray[3] == " ") && (strArray[5] == " ")))
                                {
                                    if (this.IsNormalPlaySpeed())
                                    {
                                        data5.ChangeCharacter(strArray[4], float.Parse(strArray[6]), this.ConvertNameString(strArray[2]), 0);
                                    }
                                    else
                                    {
                                        data5.SetCharacter(this.ConvertNameString(strArray[2]));
                                    }
                                    this.state = State.WAIT;
                                    this.waitIndex = num33;
                                    this.waitType = "charaSet";
                                }
                                else
                                {
                                    str3 = "parameter error";
                                }
                            }
                            else
                            {
                                this.state = State.WAIT;
                                this.waitType = "charaChange";
                                this.waitIndex = num33;
                                flag = true;
                            }
                        }
                        goto Label_4AE8;

                    case 0x42:
                        if (strArray.Length != 2)
                        {
                            str3 = "parameter error";
                        }
                        else
                        {
                            int num34 = this.ConvertCharaIndex(strArray[0]);
                            if (this.charaList[num34] != null)
                            {
                                this.charaList[num34].Destroy();
                                this.charaList[num34] = null;
                            }
                        }
                        goto Label_4AE8;

                    case 0x43:
                        if ((strArray.Length != 3) || (strArray[1] != " "))
                        {
                            str3 = "parameter error";
                        }
                        else
                        {
                            int num35 = this.ConvertCharaIndex(strArray[0]);
                            ScriptCharaData data6 = this.charaList[num35];
                            if (data6 == null)
                            {
                                Debug.LogWarning("ScriptCharData " + strArray[0] + " is null");
                            }
                            else
                            {
                                data6.SetFace(int.Parse(strArray[2]));
                            }
                        }
                        goto Label_4AE8;

                    case 0x44:
                        if ((strArray.Length < 3) || (strArray[1] != " "))
                        {
                            str3 = "parameter error";
                        }
                        else
                        {
                            int num36 = this.ConvertCharaIndex(strArray[0]);
                            ScriptCharaData data7 = this.charaList[num36];
                            if (data7 == null)
                            {
                                Debug.LogWarning("ScriptCharData " + strArray[0] + " is null");
                            }
                            else
                            {
                                data7.SetShadow(strArray[2] == "true");
                            }
                        }
                        goto Label_4AE8;

                    case 0x45:
                        if ((strArray.Length < 3) || (strArray[1] != " "))
                        {
                            str3 = "parameter error";
                        }
                        else
                        {
                            int num37 = this.ConvertCharaIndex(strArray[0]);
                            ScriptCharaData data8 = this.charaList[num37];
                            if (data8 == null)
                            {
                                Debug.LogWarning("ScriptCharData " + strArray[0] + " is null");
                            }
                            else if ((strArray.Length < 5) || (strArray[3] != " "))
                            {
                                data8.SetFilter(strArray[2], Color.white);
                            }
                            else
                            {
                                data8.SetFilter(strArray[2], this.ConvertColor(strArray[4]));
                            }
                        }
                        goto Label_4AE8;

                    case 70:
                        if ((strArray.Length < 3) || (strArray[1] != " "))
                        {
                            str3 = "parameter error";
                        }
                        else
                        {
                            int num38 = this.ConvertCharaIndex(strArray[0]);
                            ScriptCharaData data9 = this.charaList[num38];
                            if (data9 == null)
                            {
                                Debug.LogWarning("ScriptCharData " + strArray[0] + " is null");
                            }
                            else if ((strArray.Length < 5) || (strArray[3][0] != ','))
                            {
                                data9.SetPosition(int.Parse(strArray[2]));
                            }
                            else
                            {
                                data9.SetPosition((float) int.Parse(strArray[2]), (float) int.Parse(strArray[4]));
                            }
                        }
                        goto Label_4AE8;

                    case 0x47:
                        if ((strArray.Length != 3) || (strArray[1] != " "))
                        {
                            str3 = "parameter error";
                        }
                        else
                        {
                            int num39 = this.ConvertCharaIndex(strArray[0]);
                            ScriptCharaData data10 = this.charaList[num39];
                            if (data10 == null)
                            {
                                Debug.LogWarning("ScriptCharData " + strArray[0] + " is null");
                            }
                            else
                            {
                                data10.SetScale(float.Parse(strArray[2]));
                            }
                        }
                        goto Label_4AE8;

                    case 0x48:
                        if (!this.IsWaitTalk())
                        {
                            if (strArray.Length == 1)
                            {
                                int num40 = this.ConvertCharaIndexTalk(strArray[0]);
                                if (num40 >= 0)
                                {
                                    this.SetTalkIndex(num40);
                                }
                                else
                                {
                                    this.SetTalkName(strArray[0]);
                                }
                            }
                            else
                            {
                                str3 = "parameter error";
                            }
                        }
                        else
                        {
                            this.state = State.WAIT;
                            this.waitType = "charaTalk";
                            this.waitIndex = -1;
                            flag = true;
                        }
                        goto Label_4AE8;

                    case 0x49:
                        if ((strArray.Length != 3) || (strArray[1] != " "))
                        {
                            str3 = "parameter error";
                        }
                        else
                        {
                            int num41 = this.ConvertCharaIndex(strArray[0]);
                            ScriptCharaData data11 = this.charaList[num41];
                            if (data11 == null)
                            {
                                Debug.LogWarning("ScriptCharData " + strArray[0] + " is null");
                            }
                            else
                            {
                                data11.SetDepth(int.Parse(strArray[2]));
                            }
                        }
                        goto Label_4AE8;

                    case 0x4a:
                        if ((strArray.Length < 3) || (strArray[1] != " "))
                        {
                            str3 = "parameter error";
                        }
                        else
                        {
                            int num42 = this.ConvertCharaIndex(strArray[0]);
                            ScriptCharaData data12 = this.charaList[num42];
                            if (data12 == null)
                            {
                                Debug.LogWarning("ScriptCharData " + strArray[0] + " is null");
                            }
                            else
                            {
                                string str20 = this.ConvertNameString(strArray[2]);
                                Debug.Log("create specialEffect " + str20);
                                Vector3 charaOffset = (Vector3) Vector2.zero;
                                float num43 = 0f;
                                Color color = Color.clear;
                                if ((strArray.Length >= 5) && (strArray[3] == " "))
                                {
                                    if ((strArray.Length < 7) || (strArray[5] != ","))
                                    {
                                        charaOffset = ScriptPosition.GetCharaOffset(int.Parse(strArray[4]));
                                        if ((strArray.Length >= 7) && (strArray[5] == " "))
                                        {
                                            num43 = float.Parse(strArray[6]);
                                            if ((strArray.Length >= 9) && (strArray[7] == " "))
                                            {
                                                color = this.ConvertColor(strArray[8]);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        charaOffset = ScriptPosition.GetCharaOffset((float) int.Parse(strArray[4]), (float) int.Parse(strArray[6]));
                                        if ((strArray.Length >= 9) && (strArray[7] == " "))
                                        {
                                            num43 = float.Parse(strArray[8]);
                                            if ((strArray.Length >= 11) && (strArray[9] == " "))
                                            {
                                                color = this.ConvertColor(strArray[10]);
                                            }
                                        }
                                    }
                                }
                                this.isRequestEffect = true;
                                data12.SetSpecialEffect(str20, charaOffset, num43, color, this.IsFastPlaySpeed());
                            }
                        }
                        goto Label_4AE8;

                    case 0x4b:
                        if (strArray.Length < 1)
                        {
                            str3 = "parameter error";
                        }
                        else
                        {
                            int num44 = this.ConvertCharaIndex(strArray[0]);
                            ScriptCharaData data13 = this.charaList[num44];
                            if (data13 == null)
                            {
                                Debug.LogWarning("ScriptCharData " + strArray[0] + " is null");
                            }
                            else if ((strArray.Length < 3) || (strArray[1] != " "))
                            {
                                Debug.Log("stop charaSpecialEffect " + strArray[0]);
                                data13.StopSpecialEffect();
                            }
                            else
                            {
                                string str21 = this.ConvertNameString(strArray[2]);
                                Debug.Log("stop charaSpecialEffect " + strArray[0] + " " + str21);
                                data13.StopSpecialEffect(str21);
                            }
                        }
                        goto Label_4AE8;

                    case 0x4c:
                        if ((strArray.Length < 3) || (strArray[1] != " "))
                        {
                            str3 = "parameter error";
                        }
                        else
                        {
                            int num45 = this.ConvertCharaIndex(strArray[0]);
                            ScriptCharaData data14 = this.charaList[num45];
                            if (data14 == null)
                            {
                                Debug.LogWarning("ScriptCharData " + strArray[0] + " is null");
                            }
                            else
                            {
                                string str22 = this.ConvertTalkEffectName(strArray[2]);
                                Debug.Log("create charaEffect " + str22);
                                bool flag6 = this.IsFastPlaySpeed();
                                this.isRequestEffect = true;
                                if (((strArray.Length < 7) || (strArray[3] != " ")) || (strArray[5] != ","))
                                {
                                    if ((strArray.Length >= 5) && (strArray[3] == " "))
                                    {
                                        data14.SetEffect(str22, ScriptPosition.GetCharaOffset(int.Parse(strArray[4])), flag6);
                                    }
                                    else
                                    {
                                        data14.SetEffect(str22, flag6);
                                    }
                                }
                                else
                                {
                                    data14.SetEffect(str22, ScriptPosition.GetCharaOffset((float) int.Parse(strArray[4]), (float) int.Parse(strArray[6])), flag6);
                                }
                            }
                        }
                        goto Label_4AE8;

                    case 0x4d:
                        if (strArray.Length < 1)
                        {
                            str3 = "parameter error";
                        }
                        else
                        {
                            int num46 = this.ConvertCharaIndex(strArray[0]);
                            ScriptCharaData data15 = this.charaList[num46];
                            if (data15 == null)
                            {
                                Debug.LogWarning("ScriptCharData " + strArray[0] + " is null");
                            }
                            else
                            {
                                bool flag7 = this.IsFastPlaySpeed();
                                if ((strArray.Length < 3) || (strArray[1] != " "))
                                {
                                    Debug.Log("stop charaEffect " + strArray[0]);
                                    data15.StopEffect(flag7);
                                }
                                else
                                {
                                    string str23 = this.ConvertTalkEffectName(strArray[2]);
                                    Debug.Log("stop charaEffect " + strArray[0] + " " + str23);
                                    data15.StopEffect(str23, flag7);
                                }
                            }
                        }
                        goto Label_4AE8;

                    case 0x4e:
                        if ((strArray.Length < 3) || (strArray[1] != " "))
                        {
                            str3 = "parameter error";
                        }
                        else
                        {
                            int num47 = this.ConvertCharaIndex(strArray[0]);
                            ScriptCharaData data16 = this.charaList[num47];
                            if (data16 == null)
                            {
                                Debug.LogWarning("ScriptCharData " + strArray[0] + " is null");
                            }
                            else
                            {
                                string str24 = this.ConvertTalkEffectName(strArray[2]);
                                Debug.Log("create charaBackEffect " + str24);
                                bool flag8 = this.IsFastPlaySpeed();
                                this.isRequestEffect = true;
                                if (((strArray.Length < 7) || (strArray[3] != " ")) || (strArray[5] != ","))
                                {
                                    if ((strArray.Length >= 5) && (strArray[3] == " "))
                                    {
                                        data16.SetBackEffect(str24, ScriptPosition.GetCharaOffset(int.Parse(strArray[4])), flag8);
                                    }
                                    else
                                    {
                                        data16.SetBackEffect(str24, flag8);
                                    }
                                }
                                else
                                {
                                    data16.SetBackEffect(str24, ScriptPosition.GetCharaOffset((float) int.Parse(strArray[4]), (float) int.Parse(strArray[6])), flag8);
                                }
                            }
                        }
                        goto Label_4AE8;

                    case 0x4f:
                        if (strArray.Length < 1)
                        {
                            str3 = "parameter error";
                        }
                        else
                        {
                            int num48 = this.ConvertCharaIndex(strArray[0]);
                            ScriptCharaData data17 = this.charaList[num48];
                            if (data17 == null)
                            {
                                Debug.LogWarning("ScriptCharData " + strArray[0] + " is null");
                            }
                            else
                            {
                                bool flag9 = this.IsFastPlaySpeed();
                                if ((strArray.Length < 3) || (strArray[1] != " "))
                                {
                                    Debug.Log("stop charaBackEffect " + strArray[0]);
                                    data17.StopBackEffect(flag9);
                                }
                                else
                                {
                                    string str25 = this.ConvertTalkEffectName(strArray[2]);
                                    Debug.Log("stop charaBackEffect " + strArray[0] + " " + str25);
                                    data17.StopBackEffect(str25, flag9);
                                }
                            }
                        }
                        goto Label_4AE8;

                    case 80:
                        if (((strArray.Length >= 5) && (strArray[1] == " ")) && (strArray[3] == " "))
                        {
                            int num49 = this.ConvertCharaIndex(strArray[0]);
                            ScriptCharaData data18 = this.charaList[num49];
                            if (data18 != null)
                            {
                                string str26 = this.ConvertNameString(strArray[2]);
                                float num50 = float.Parse(strArray[4]);
                                float mgd = 0f;
                                Debug.Log("create cutin " + str26);
                                if (strArray.Length >= 7)
                                {
                                    mgd = float.Parse(strArray[6]);
                                }
                                this.isRequestEffect = true;
                                data18.SetCutin(str26, num50, mgd, this.IsFastPlaySpeed());
                            }
                            else
                            {
                                Debug.LogWarning("ScriptCharData " + strArray[0] + " is null");
                            }
                        }
                        else
                        {
                            str3 = "parameter error";
                        }
                        goto Label_4AE8;

                    case 0x51:
                        if ((strArray.Length != 3) || (strArray[1] != " "))
                        {
                            str3 = "parameter error";
                        }
                        else
                        {
                            int num52 = this.ConvertCharaIndex(strArray[0]);
                            ScriptCharaData data19 = this.charaList[num52];
                            if (data19 == null)
                            {
                                Debug.LogWarning("ScriptCharData " + strArray[0] + " is null");
                            }
                            else
                            {
                                float num53 = float.Parse(strArray[2]);
                                Debug.Log("create cutout");
                                this.isRequestEffect = true;
                                data19.SetCutout(num53, this.IsFastPlaySpeed());
                            }
                        }
                        goto Label_4AE8;

                    case 0x52:
                        if (strArray.Length < 1)
                        {
                            str3 = "parameter error";
                        }
                        else
                        {
                            int num54 = this.ConvertCharaIndex(strArray[0]);
                            ScriptCharaData data20 = this.charaList[num54];
                            if (data20 == null)
                            {
                                Debug.LogWarning("ScriptCharData " + strArray[0] + " is null");
                            }
                            else
                            {
                                Debug.Log("stop charaCut " + strArray[0]);
                                data20.StopCut();
                            }
                        }
                        goto Label_4AE8;

                    case 0x53:
                        if ((strArray.Length < 3) || (strArray[1] != " "))
                        {
                            str3 = "parameter error";
                        }
                        else
                        {
                            int num55 = this.ConvertCharaIndex(strArray[0]);
                            ScriptCharaData data21 = this.charaList[num55];
                            if (data21 == null)
                            {
                                Debug.LogWarning("ScriptCharData " + strArray[0] + " is null");
                            }
                            else if (!data21.IsMoveAlpha())
                            {
                                if ((strArray.Length >= 5) && (strArray[3] == " "))
                                {
                                    if ((strArray.Length >= 7) && (strArray[5][0] == ','))
                                    {
                                        data21.SetPosition((float) int.Parse(strArray[4]), (float) int.Parse(strArray[6]));
                                    }
                                    else
                                    {
                                        data21.SetPosition(int.Parse(strArray[4]));
                                    }
                                }
                                float num56 = !this.IsNormalPlaySpeed() ? 0f : float.Parse(strArray[2]);
                                data21.MoveAlpha(num56, 1f);
                            }
                            else
                            {
                                this.state = State.WAIT;
                                this.waitType = "charaFade";
                                this.waitIndex = num55;
                                flag = true;
                            }
                        }
                        goto Label_4AE8;

                    case 0x54:
                        if ((strArray.Length != 3) || (strArray[1] != " "))
                        {
                            str3 = "parameter error";
                        }
                        else
                        {
                            int num57 = this.ConvertCharaIndex(strArray[0]);
                            ScriptCharaData data22 = this.charaList[num57];
                            if (data22 == null)
                            {
                                Debug.LogWarning("ScriptCharData " + strArray[0] + " is null");
                            }
                            else if (!data22.IsMoveAlpha())
                            {
                                float num58 = !this.IsNormalPlaySpeed() ? 0f : float.Parse(strArray[2]);
                                data22.MoveAlpha(num58, 0f);
                            }
                            else
                            {
                                this.state = State.WAIT;
                                this.waitType = "charaFade";
                                this.waitIndex = num57;
                                flag = true;
                            }
                        }
                        goto Label_4AE8;

                    case 0x55:
                        if ((strArray.Length < 5) || (strArray[1] != " "))
                        {
                            str3 = "parameter error";
                        }
                        else
                        {
                            int num59 = this.ConvertCharaIndex(strArray[0]);
                            ScriptCharaData data23 = this.charaList[num59];
                            if (data23 == null)
                            {
                                Debug.LogWarning("ScriptCharData " + strArray[0] + " is null");
                            }
                            else if (((strArray.Length < 7) || (strArray[3][0] != ',')) || (strArray[5][0] != ' '))
                            {
                                if (strArray[3][0] == ' ')
                                {
                                    float num61 = !this.IsNormalPlaySpeed() ? 0f : float.Parse(strArray[4]);
                                    data23.MovePosition(num61, int.Parse(strArray[2]));
                                }
                                else
                                {
                                    Debug.LogWarning("ScriptCharData parameter errror " + data);
                                }
                            }
                            else
                            {
                                float num60 = !this.IsNormalPlaySpeed() ? 0f : float.Parse(strArray[6]);
                                data23.MovePosition(num60, (float) int.Parse(strArray[2]), (float) int.Parse(strArray[4]));
                            }
                        }
                        goto Label_4AE8;

                    case 0x56:
                        if (strArray.Length < 1)
                        {
                            str3 = "parameter error";
                        }
                        else
                        {
                            int num62 = this.ConvertCharaIndex(strArray[0]);
                            ScriptCharaData data24 = this.charaList[num62];
                            if (data24 == null)
                            {
                                Debug.LogWarning("ScriptCharData " + strArray[0] + " is null");
                            }
                            else if ((strArray.Length < 3) || (strArray[1][0] != ' '))
                            {
                                data24.MoveReturnPosition(0f);
                            }
                            else
                            {
                                float num63 = !this.IsNormalPlaySpeed() ? 0f : float.Parse(strArray[2]);
                                data24.MoveReturnPosition(num63);
                            }
                        }
                        goto Label_4AE8;

                    case 0x57:
                        if ((strArray.Length < 5) || (strArray[1] != " "))
                        {
                            str3 = "parameter error";
                        }
                        else
                        {
                            int num64 = this.ConvertCharaIndex(strArray[0]);
                            ScriptCharaData data25 = this.charaList[num64];
                            if (data25 == null)
                            {
                                Debug.LogWarning("ScriptCharData " + strArray[0] + " is null");
                            }
                            else if (((strArray.Length < 4) || (strArray[3][0] != ',')) || (strArray[5][0] != ' '))
                            {
                                if (strArray[3][0] == ' ')
                                {
                                    float num66 = !this.IsNormalPlaySpeed() ? 0f : float.Parse(strArray[4]);
                                    data25.MoveReturnPosition(num66, int.Parse(strArray[2]));
                                }
                                else
                                {
                                    Debug.LogWarning("ScriptCharData parameter errror " + data);
                                }
                            }
                            else
                            {
                                float num65 = !this.IsNormalPlaySpeed() ? 0f : float.Parse(strArray[6]);
                                data25.MoveReturnPosition(num65, (float) int.Parse(strArray[2]), (float) int.Parse(strArray[4]));
                            }
                        }
                        goto Label_4AE8;

                    case 0x58:
                        if (((strArray.Length < 5) || (strArray[1] != " ")) || (strArray[3] != " "))
                        {
                            str3 = "parameter error";
                        }
                        else
                        {
                            int num67 = this.ConvertCharaIndex(strArray[0]);
                            ScriptCharaData data26 = this.charaList[num67];
                            if (data26 == null)
                            {
                                Debug.LogWarning("ScriptCharData " + strArray[0] + " is null");
                            }
                            else if (((strArray.Length != 9) || (strArray[5][0] != ',')) || (strArray[7][0] != ' '))
                            {
                                if ((strArray.Length == 7) && (strArray[5][0] == ','))
                                {
                                    data26.MoveAttack(strArray[2], 0f, (float) int.Parse(strArray[4]), (float) int.Parse(strArray[6]));
                                }
                                else if ((strArray.Length == 7) && (strArray[5][0] == ' '))
                                {
                                    float num69 = !this.IsNormalPlaySpeed() ? 0f : float.Parse(strArray[6]);
                                    data26.MoveAttack(strArray[2], num69, int.Parse(strArray[4]));
                                }
                                else if (strArray.Length == 5)
                                {
                                    data26.MoveAttack(strArray[2], 0f, int.Parse(strArray[4]));
                                }
                                else
                                {
                                    Debug.LogWarning("ScriptCharData parameter errror " + data);
                                }
                            }
                            else
                            {
                                float num68 = !this.IsNormalPlaySpeed() ? 0f : float.Parse(strArray[8]);
                                data26.MoveAttack(strArray[2], num68, (float) int.Parse(strArray[4]), (float) int.Parse(strArray[6]));
                            }
                        }
                        goto Label_4AE8;

                    case 0x59:
                        if ((((strArray.Length == 9) && (strArray[1] == " ")) && ((strArray[3] == " ") && (strArray[5] == " "))) && (strArray[7] == " "))
                        {
                            int num70 = this.ConvertCharaIndex(strArray[0]);
                            ScriptCharaData data27 = this.charaList[num70];
                            if (data27 != null)
                            {
                                float num71 = float.Parse(strArray[2]);
                                if (num71 == 0f)
                                {
                                    num71 = 0.01666667f;
                                }
                                float num72 = float.Parse(strArray[8]);
                                if (this.IsFastPlaySpeed() && (num72 > 0f))
                                {
                                    num71 = 0f;
                                }
                                data27.Shake(num72, num71, (float) int.Parse(strArray[4]), (float) int.Parse(strArray[6]));
                            }
                            else
                            {
                                Debug.LogWarning("ScriptShake " + strArray[0] + " is null");
                            }
                        }
                        else
                        {
                            str3 = "parameter error";
                        }
                        goto Label_4AE8;

                    case 90:
                        if (strArray.Length != 1)
                        {
                            str3 = "parameter error";
                        }
                        else
                        {
                            int num73 = this.ConvertCharaIndex(strArray[0]);
                            ScriptCharaData data28 = this.charaList[num73];
                            if (data28 == null)
                            {
                                Debug.LogWarning("ScriptShakeOff " + strArray[0] + " is null");
                            }
                            else
                            {
                                data28.ShakeStop();
                            }
                        }
                        goto Label_4AE8;

                    case 0x5b:
                        if (strArray.Length < 1)
                        {
                            str3 = "parameter error";
                        }
                        else
                        {
                            int num74 = int.Parse(strArray[0]);
                            Vector3 vector5 = Vector3.zero;
                            int num75 = 0;
                            int num76 = 0;
                            int num77 = 0;
                            Debug.Log("create communication " + strArray[0]);
                            if ((strArray.Length >= 3) && (strArray[1] == " "))
                            {
                                if ((strArray.Length < 5) || (strArray[3][0] != ','))
                                {
                                    vector5 = ScriptPosition.GetPosition(int.Parse(strArray[2]));
                                    if ((strArray.Length >= 5) && (strArray[3] == " "))
                                    {
                                        num75 = int.Parse(strArray[4]);
                                        if ((strArray.Length >= 7) && (strArray[5] == " "))
                                        {
                                            num76 = int.Parse(strArray[6]);
                                            if ((strArray.Length >= 9) && (strArray[7] == " "))
                                            {
                                                num77 = int.Parse(strArray[8]);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    vector5 = ScriptPosition.GetPosition((float) int.Parse(strArray[2]), (float) int.Parse(strArray[4]));
                                    if ((strArray.Length >= 7) && (strArray[5] == " "))
                                    {
                                        num75 = int.Parse(strArray[6]);
                                        if ((strArray.Length >= 9) && (strArray[7] == " "))
                                        {
                                            num76 = int.Parse(strArray[8]);
                                            if ((strArray.Length >= 11) && (strArray[9] == " "))
                                            {
                                                num77 = int.Parse(strArray[10]);
                                            }
                                        }
                                    }
                                }
                            }
                            this.isLoadCommunicationChara = true;
                            CommunicationCharaEffectParam param = new CommunicationCharaEffectParam {
                                noiseKind = num76,
                                svtId = num74 / 10,
                                limitCount = num74 % 10,
                                faceType = (Face.Type) num77,
                                isStartLoop = (message == "communicationCharaLoop") || this.IsFastPlaySpeed(),
                                callback = new System.Action(this.ReadyCommunicationChara)
                            };
                            vector5.z = -num75 * 10f;
                            CommonEffectManager.CreateParam(this.communicationCharaEffectBase, "Talk/communicationCharaEffect", vector5, param);
                            this.state = State.WAIT;
                            this.waitIndex = 0;
                            this.waitType = "communicationCharaLoad";
                        }
                        goto Label_4AE8;

                    case 0x5c:
                        if (strArray.Length < 1)
                        {
                            str3 = "parameter error";
                        }
                        else
                        {
                            CommonEffectComponent[] componentArray = CommonEffectManager.Get(this.communicationCharaEffectBase);
                            if (componentArray.Length > 0)
                            {
                                (componentArray[0] as CommunicationCharaEffectComponent).SetFace((Face.Type) int.Parse(strArray[0]));
                            }
                        }
                        goto Label_4AE8;

                    case 0x5d:
                        CommonEffectManager.Stop(this.communicationCharaEffectBase, false, false);
                        goto Label_4AE8;

                    case 0x5e:
                        CommonEffectManager.Destroy(this.communicationCharaEffectBase);
                        goto Label_4AE8;

                    case 0x5f:
                        Debug.Log(string.Concat(new object[] { "talk name message [", num, "] ", data }));
                        this.state = State.WAIT;
                        this.waitType = "talkName";
                        this.waitMessage = data;
                        goto Label_4AE8;

                    case 0x60:
                        this.state = State.WAIT;
                        this.waitType = "talkStart";
                        this.waitMessage = this.backLogTalkStartString;
                        goto Label_4AE8;

                    case 0x61:
                        this.state = State.WAIT;
                        this.waitType = "talkEnd";
                        this.waitMessage = this.backLogTalkEndString;
                        goto Label_4AE8;
                }
            }
            Debug.LogWarning(string.Concat(new object[] { "ScriptManager execute not support tag [", this.assetName, ",", this.scriptLabel, ",", num, "] ", message, " ", data }));
        }
        goto Label_4AE8;
    Label_2F24:
        str19 = this.ConvertNameString(strArray[2]);
        Debug.Log("create figure " + strArray[0] + " " + strArray[2]);
        str31 = message;
        if (str31 != null)
        {
            if (<>f__switch$map23 == null)
            {
                dictionary = new Dictionary<string, int>(2) {
                    { 
                        "equipSet",
                        0
                    },
                    { 
                        "imageSet",
                        1
                    }
                };
                <>f__switch$map23 = dictionary;
            }
            if (<>f__switch$map23.TryGetValue(str31, out num88))
            {
                if (num88 == 0)
                {
                    data3 = new ScriptCharaData(ScriptCharaData.Kind.EQUIP, strArray[0], str19, this.equipSeed);
                    goto Label_2FF5;
                }
                if (num88 == 1)
                {
                    data3 = new ScriptCharaData(ScriptCharaData.Kind.IMAGE, strArray[0], str19, this.imageSeed);
                    goto Label_2FF5;
                }
            }
        }
        data3 = new ScriptCharaData(ScriptCharaData.Kind.FIGURE, strArray[0], str19, this.figureSeed);
    Label_2FF5:
        data3.SetAlpha(0f);
        this.charaList[num31] = data3;
        if ((strArray.Length >= 5) && (strArray[3] == " "))
        {
            if ((strArray.Length >= 7) && (strArray[5][0] == ','))
            {
                data3.SetPosition((float) int.Parse(strArray[4]), (float) int.Parse(strArray[6]));
                if ((strArray.Length >= 9) && (strArray[7] == " "))
                {
                    data3.SetTalkName(strArray[8]);
                }
            }
            else
            {
                data3.SetPosition(int.Parse(strArray[4]));
                if ((strArray.Length >= 7) && (strArray[5] == " "))
                {
                    data3.SetTalkName(strArray[6]);
                }
            }
        }
        this.state = State.WAIT;
        this.waitIndex = num31;
        this.waitType = message;
    Label_4AE8:
        if (str3 != null)
        {
            Debug.LogError(string.Concat(new object[] { "ScriptManager ", str3, " [", this.assetName, ",", this.scriptLabel, ",", num, "] ", message, " ", data }));
            this.state = State.ERROR;
            goto Label_65F5;
        }
        if (!flag)
        {
            if ((this.state != State.WAIT) || (this.waitMessage == null))
            {
                this.DebugProcessScript(this.executeWaitIndex, this.state);
            }
            this.executeIndex++;
        }
        goto Label_0074;
    Label_6476:
        if (!flag11)
        {
            if (isBusy)
            {
                goto Label_65F5;
            }
            this.state = State.EXECUTE;
        }
        goto Label_0074;
    Label_65F5:
        if (this.messageManager.IsBusy)
        {
            this.ModifyPlaySpeed(false);
        }
        this.messageManager.MessageUpdate();
    }

    public bool ReadSetting()
    {
        if (isReadScriptSetting)
        {
            return true;
        }
        isReadScriptSetting = true;
        string path = getScriptSettingFileName();
        Debug.Log("NetworkManager::read " + path);
        if (File.Exists(path))
        {
            BinaryReader reader = new BinaryReader(File.OpenRead(path));
            try
            {
                scriptServerSettingAddress = reader.ReadString();
                scriptObjectSettingAddress = reader.ReadString();
                scriptGenderSettingIndex = reader.ReadInt32();
                scriptStartModeSettingName = reader.ReadString();
                scriptPlayerPathSettingAddress = reader.ReadString();
                scriptPlayerObjectSettingAddress = reader.ReadString();
                return true;
            }
            catch (Exception exception)
            {
                Debug.LogWarning(exception.Message);
            }
            finally
            {
                if (reader != null)
                {
                    ((IDisposable) reader).Dispose();
                }
            }
        }
        SetScriptServerSetting(string.Empty, string.Empty, 1, "DEFAULT");
        SetScriptPlayerSetting(string.Empty, string.Empty, 1, "DEFAULT");
        return false;
    }

    protected void ReadyCommunicationChara()
    {
        this.isLoadCommunicationChara = false;
    }

    public void reboot()
    {
        this.assetData = null;
        this.wipeAssetData = null;
        this.gameDemoAssetData = null;
        this.ReleaseAsset();
        this.meshRenderBase.SetTweenColor(Color.clear);
        this.RenderCameraQuit();
    }

    protected void ReleaseAsset()
    {
        if (this.charaList != null)
        {
            for (int i = 0; i < CHARA_MAX; i++)
            {
                ScriptCharaData data = this.charaList[i];
                if (data != null)
                {
                    data.Destroy();
                    this.charaList[i] = null;
                }
            }
            this.backSprite1.ClearImage();
            this.backSprite2.ClearImage();
            this.backSprite1.transform.localPosition = Vector3.zero;
            this.backSprite2.transform.localPosition = Vector3.zero;
            this.backSprite1.SetTweenColor(new Color(1f, 1f, 1f, 0f));
            this.backSprite2.SetTweenColor(new Color(1f, 1f, 1f, 0f));
            this.sceneMainIndex = 0;
            this.isBusyScene = false;
            this.isBusySceneLoad = false;
            this.meshCaptureBase.SetTweenColor(Color.clear);
            this.meshCaptureBase.ClearImage();
            if (this.captureTexture != null)
            {
                this.captureTexture.Release();
                UnityEngine.Object.Destroy(this.captureTexture);
                this.captureTexture = null;
            }
            if (this.assetData != null)
            {
                AssetManager.releaseAsset(this.assetData);
                this.assetData = null;
            }
            this.flagDataList.Clear();
            this.systemPanel.gameObject.SetActive(false);
            this.normalOperationBase.SetActive(false);
            this.backViewOperationBase.SetActive(false);
            this.figureViewOperationBase.SetActive(false);
        }
    }

    protected void RenderCameraInit()
    {
        this.renderTextureCamera.gameObject.SetActive(true);
        this.scriptReScale.ReScale();
        if (this.renderTextureCamera.targetTexture == null)
        {
            RenderTexture tex = RenderTexture.GetTemporary(0x400, 0x400, 0, RenderTextureFormat.ARGB32);
            tex.name = "TempScriptActionRednerTexture";
            this.renderTextureCamera.targetTexture = tex;
            this.meshRenderBase.SetImage(tex);
        }
        Material material = this.meshRenderBase.material;
        if ((material != null) && material.HasProperty("_Tb"))
        {
            material.SetFloat("_Tb", (this.startMode != StartMode.THROUGH) ? 1f : 0f);
        }
        this.renderTextureCamera.enabled = false;
        this.renderTextureCamera.enabled = true;
    }

    protected void RenderCameraQuit()
    {
        this.renderTextureCamera.gameObject.SetActive(false);
        RenderTexture targetTexture = this.renderTextureCamera.targetTexture;
        this.renderTextureCamera.targetTexture = null;
        if (targetTexture != null)
        {
            this.meshRenderBase.ClearImage();
            RenderTexture.ReleaseTemporary(targetTexture);
        }
    }

    protected RenderTexture RenderCameraSwap(RenderTexture swapTexture)
    {
        RenderTexture targetTexture = this.renderTextureCamera.targetTexture;
        if (targetTexture != null)
        {
            targetTexture.name = "captureScriptActionRednerTexture";
        }
        if (swapTexture == null)
        {
            swapTexture = RenderTexture.GetTemporary(0x400, 0x400, 0, RenderTextureFormat.ARGB32);
        }
        swapTexture.name = "TempScriptActionRednerTexture";
        this.renderTextureCamera.targetTexture = swapTexture;
        this.meshRenderBase.SetImage(swapTexture);
        this.renderTextureCamera.enabled = false;
        this.renderTextureCamera.enabled = true;
        return targetTexture;
    }

    protected void ResetWipeFilter()
    {
        this.InitWipe();
    }

    protected void ResumePlaySpeed()
    {
        if (this.playSpeed == PlaySpeed.PAUSE)
        {
            this.playSpeed = PlaySpeed.NORMAL;
        }
        if (this.requestPlaySpeed == PlaySpeed.PAUSE)
        {
            this.requestPlaySpeed = PlaySpeed.NONE;
            this.playSpeed = PlaySpeed.NORMAL;
        }
        this.fastPlayMark.SetActive(this.playSpeed == PlaySpeed.FAST);
    }

    protected ScriptFlagData SearchFlag(string name)
    {
        ScriptFlagData data;
        for (int i = 0; i < this.flagDataList.Count; i++)
        {
            data = this.flagDataList[i];
            if (data.Name.Equals(name))
            {
                return data;
            }
        }
        this.flagDataList.Add(data = new ScriptFlagData(name));
        return data;
    }

    protected void SetCharaFace(int n)
    {
        if (n < 0)
        {
            n = 20;
        }
        else if (n > 20)
        {
            n = 0;
        }
        this.figureFaceNum = n;
        ScriptCharaData data = this.charaList[0];
        if (data != null)
        {
            data.SetFace(n);
        }
    }

    protected bool SetCharaImage(string name, string charaName)
    {
        ScriptCharaData data = this.charaList[0];
        if (data != null)
        {
            if (data.IsLoad())
            {
                return false;
            }
            data.Destroy();
            this.charaList[0] = null;
        }
        data = new ScriptCharaData(ScriptCharaData.Kind.FIGURE, name, charaName, this.figureSeed);
        this.figureFaceNum = 0;
        this.charaList[0] = data;
        return true;
    }

    protected bool SetSceneImage(string fileName)
    {
        if (this.isBusyScene)
        {
            return false;
        }
        if (this.isBusySceneLoad)
        {
            return false;
        }
        Debug.Log(string.Concat(new object[] { "SetSceneImage:", this.sceneMainIndex, " ", this.sceneCrossFadeTime, " ", fileName }));
        this.isBusyScene = true;
        this.isBusySceneLoad = true;
        if (!AssetManager.isExistAssetStorage(fileName))
        {
            fileName = this.ConvertBackTextureName("0");
        }
        if (this.sceneMainIndex == 0)
        {
            this.backSprite1.SetTweenColor(new Color(1f, 1f, 1f, 0f));
            this.backSprite1.transform.localPosition = new Vector3(0f, 0f, -1f);
            this.backSprite1.SetAssetImage(fileName, new System.Action(this.OnLoadEndScene));
        }
        else
        {
            this.backSprite2.SetTweenColor(new Color(1f, 1f, 1f, 0f));
            this.backSprite2.transform.localPosition = new Vector3(0f, 0f, -1f);
            this.backSprite2.SetAssetImage(fileName, new System.Action(this.OnLoadEndScene));
        }
        return true;
    }

    public static bool SetScriptPlayerSetting(string scriptPath, string scriptObject, int genderIndex, string startModeName)
    {
        bool flag = false;
        if (scriptPlayerPathSettingAddress != scriptPath)
        {
            scriptPlayerPathSettingAddress = scriptPath;
            flag = true;
        }
        if (scriptPlayerObjectSettingAddress != scriptObject)
        {
            scriptPlayerObjectSettingAddress = scriptObject;
            flag = true;
        }
        if (scriptGenderSettingIndex != genderIndex)
        {
            scriptGenderSettingIndex = genderIndex;
            flag = true;
        }
        if (scriptStartModeSettingName != startModeName)
        {
            scriptStartModeSettingName = startModeName;
            flag = true;
        }
        return flag;
    }

    public static bool SetScriptServerSetting(string scriptServer, string scriptObject, int genderIndex, string startModeName)
    {
        bool flag = false;
        if (scriptServerSettingAddress != scriptServer)
        {
            scriptServerSettingAddress = scriptServer;
            flag = true;
        }
        if (scriptObjectSettingAddress != scriptObject)
        {
            scriptObjectSettingAddress = scriptObject;
            flag = true;
        }
        if (scriptGenderSettingIndex != genderIndex)
        {
            scriptGenderSettingIndex = genderIndex;
            flag = true;
        }
        if (scriptStartModeSettingName != startModeName)
        {
            scriptStartModeSettingName = startModeName;
            flag = true;
        }
        return flag;
    }

    [DebuggerHidden]
    public IEnumerator settalk(int type = 0x3e7, string text = null, float delay = 0f) => 
        new <settalk>c__Iterator15 { 
            delay = delay,
            text = text,
            type = type,
            <$>delay = delay,
            <$>text = text,
            <$>type = type,
            <>f__this = this
        };

    protected void SetTalkIndex(int index)
    {
        if (index >= 0)
        {
            for (int i = 0; i < this.charaList.Length; i++)
            {
                ScriptCharaData data = this.charaList[i];
                if (data != null)
                {
                    if (i == index)
                    {
                        data.SetTalkMask(false);
                        data.SetTalkDepth();
                    }
                    else
                    {
                        data.SetTalkMask(true);
                        data.RecoverDepth();
                    }
                }
            }
        }
        else
        {
            for (int j = 0; j < this.charaList.Length; j++)
            {
                ScriptCharaData data2 = this.charaList[j];
                if (data2 != null)
                {
                    data2.SetTalkMask(false);
                    data2.RecoverDepth();
                }
            }
        }
    }

    protected void SetTalkName(string name)
    {
        name = this.ConvertNameString(name);
        if (name == "on")
        {
            this.isTalkMask = true;
            name = this.talkMaskName;
        }
        else if (name == "off")
        {
            this.isTalkMask = false;
        }
        else
        {
            this.talkMaskName = name;
        }
        int index = -1;
        if (this.isTalkMask)
        {
            for (int i = 0; i < this.charaList.Length; i++)
            {
                ScriptCharaData data = this.charaList[i];
                if ((data != null) && (data.TalkName == name))
                {
                    index = i;
                    break;
                }
            }
        }
        this.SetTalkIndex(index);
    }

    protected bool SetWipeFilter(string name, float duration, float level)
    {
        if (!this.isLoadWipe)
        {
            if (this.isExecuteWipe && !this.isWipeFilter)
            {
                return false;
            }
            this.isExecuteWipe = true;
            this.isLoadWipe = true;
            this.wipeName = name;
            this.isWipeFilter = true;
            this.isWipeIn = false;
            this.wipeDuration = duration;
            this.wipeLevel = level;
            if (AssetManager.loadAssetStorage("Wipe/" + name, new AssetLoader.LoadEndDataHandler(this.EndLoadWipe)))
            {
                return true;
            }
            this.isLoadWipe = false;
            this.EndExecuteWipe();
        }
        return false;
    }

    public void Shake(float duration, float cycle, float x, float y)
    {
        this.shakeTime = (duration <= 0f) ? 0f : (Time.time + duration);
        this.shakeCycle = cycle;
        this.shakeX = x;
        this.shakeY = y;
        this.OnShake();
    }

    protected void SkipConfirm()
    {
        this.inputTopMode = InputTopMode.SKIP_CONFIRM;
        this.PausePlaySpeed();
        this.skipConfirmDialog.Open(this.isCollection, new ScriptSkipDialog.ClickDelegate(this.EndSkipConfirm));
    }

    protected void SoundStopAll()
    {
        SoundManager.stopBgm();
        SoundManager.stopJingle();
        SoundManager.stopSe(0f);
    }

    protected bool StartCamera(float duration, Vector3 position, float scale)
    {
        if (this.isExecuteCamera)
        {
            return false;
        }
        Vector3 vector = new Vector3(scale, scale, 1f);
        if (duration > 0f)
        {
            TweenScale component = this.cameraScale.GetComponent<TweenScale>();
            if (component != null)
            {
                component.enabled = false;
            }
            TweenPosition position2 = this.cameraPosition.GetComponent<TweenPosition>();
            if (position2 != null)
            {
                position2.enabled = false;
            }
            component = TweenScale.Begin(this.cameraScale, duration, vector);
            position2 = TweenPosition.Begin(this.cameraPosition, duration, position);
            if ((component != null) && component.enabled)
            {
                component.eventReceiver = base.gameObject;
                component.callWhenFinished = "EndExecuteCamera";
                this.isExecuteCamera = true;
                return true;
            }
            if ((position2 != null) && position2.enabled)
            {
                position2.eventReceiver = base.gameObject;
                position2.callWhenFinished = "EndExecuteCamera";
                this.isExecuteCamera = true;
                return true;
            }
        }
        this.cameraScale.transform.localScale = vector;
        this.cameraPosition.transform.localPosition = position;
        return true;
    }

    protected bool StartFade(string name, bool isIn, float duration)
    {
        TweenRendererColor color;
        if (this.isExecuteFade)
        {
            return false;
        }
        string key = name;
        if (key != null)
        {
            int num;
            if (<>f__switch$map29 == null)
            {
                Dictionary<string, int> dictionary = new Dictionary<string, int>(3) {
                    { 
                        "clear",
                        0
                    },
                    { 
                        "black",
                        1
                    },
                    { 
                        "white",
                        2
                    }
                };
                <>f__switch$map29 = dictionary;
            }
            if (<>f__switch$map29.TryGetValue(key, out num))
            {
                switch (num)
                {
                    case 0:
                        goto Label_00B9;

                    case 1:
                        this.fadeColor = Color.black;
                        goto Label_00B9;

                    case 2:
                        this.fadeColor = Color.white;
                        goto Label_00B9;
                }
            }
        }
        this.fadeColor = this.ConvertColor(name);
    Label_00B9:
        color = null;
        bool flag = false;
        if (name == "clear")
        {
            UITweenRenderer meshRenderBase = this.meshRenderBase;
            if ((this.fadeColor.a > 0f) && !isIn)
            {
                this.messageManager.OffScreen();
                this.messageManager.ClearText();
                this.logMessage.ReturnText();
                this.InitEffect();
                this.InitFlash();
                this.InitWipe();
                this.meshCaptureBase.SetTweenColor(Color.clear);
                this.meshCaptureBase.ClearImage();
                meshRenderBase.SetTweenColor(new Color(1f, 1f, 1f, 0f));
                isIn = true;
                flag = true;
            }
            else if (duration > 0f)
            {
                meshRenderBase.SetTweenColor(!isIn ? new Color(1f, 1f, 1f, 1f) : new Color(1f, 1f, 1f, 0f));
                color = TweenRendererColor.Begin(meshRenderBase.gameObject, duration, !isIn ? new Color(1f, 1f, 1f, 0f) : new Color(1f, 1f, 1f, 1f));
                if (color == null)
                {
                    return false;
                }
            }
            else
            {
                meshRenderBase.SetTweenColor(!isIn ? new Color(1f, 1f, 1f, 0f) : new Color(1f, 1f, 1f, 1f));
            }
        }
        else
        {
            flag = true;
        }
        if (flag)
        {
            UITweenRenderer meshFadeBase = this.meshFadeBase;
            if (duration > 0f)
            {
                this.fadeColor.a = !isIn ? ((float) 0) : ((float) 1);
                meshFadeBase.SetTweenColor(this.fadeColor);
                this.fadeColor.a = !isIn ? ((float) 1) : ((float) 0);
                color = TweenRendererColor.Begin(meshFadeBase.gameObject, duration, this.fadeColor);
                if (color == null)
                {
                    return false;
                }
            }
            else
            {
                this.fadeColor.a = !isIn ? ((float) 1) : ((float) 0);
                meshFadeBase.SetTweenColor(this.fadeColor);
            }
        }
        if (color != null)
        {
            color.eventReceiver = base.gameObject;
            color.callWhenFinished = "EndExecuteFade";
            this.fadeName = name;
            this.isExecuteFade = true;
        }
        return true;
    }

    protected bool StartFadeMove(string name, float duration, float level)
    {
        UITweenRenderer renderer;
        if (this.isExecuteFade)
        {
            return false;
        }
        string key = name;
        if (key != null)
        {
            int num;
            if (<>f__switch$map2A == null)
            {
                Dictionary<string, int> dictionary = new Dictionary<string, int>(3) {
                    { 
                        "clear",
                        0
                    },
                    { 
                        "black",
                        1
                    },
                    { 
                        "white",
                        2
                    }
                };
                <>f__switch$map2A = dictionary;
            }
            if (<>f__switch$map2A.TryGetValue(key, out num))
            {
                switch (num)
                {
                    case 0:
                        return false;

                    case 1:
                        this.fadeColor = Color.black;
                        goto Label_00AE;

                    case 2:
                        this.fadeColor = Color.white;
                        goto Label_00AE;
                }
            }
        }
        this.fadeColor = this.ConvertColor(name);
    Label_00AE:
        renderer = this.meshFadeBase;
        this.fadeColor.a = level;
        if (duration > 0f)
        {
            TweenRendererColor color = TweenRendererColor.Begin(renderer.gameObject, duration, this.fadeColor);
            if (color == null)
            {
                return false;
            }
            color.eventReceiver = base.gameObject;
            color.callWhenFinished = "EndExecuteFade";
            this.fadeName = name;
            this.isExecuteFade = true;
        }
        else
        {
            renderer.SetTweenColor(this.fadeColor);
        }
        return true;
    }

    protected bool StartFlash(string name, float duration1, float duration2, Color color1, Color color2)
    {
        if (this.isExecuteFlash)
        {
            return false;
        }
        this.flashName = name;
        this.isExecuteFlash = true;
        this.isEndRequestFlash = false;
        this.flashCount = 0;
        this.flashTime1 = duration1;
        this.flashTime2 = duration2;
        this.flashColor1 = color1;
        this.flashColor2 = color2;
        this.OnFlashRequest();
        return true;
    }

    protected bool StartMask(string name, bool isIn, float duration)
    {
        if (isIn)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(duration, null);
        }
        else
        {
            string key = name;
            if (key != null)
            {
                int num;
                if (<>f__switch$map2B == null)
                {
                    Dictionary<string, int> dictionary = new Dictionary<string, int>(3) {
                        { 
                            "clear",
                            0
                        },
                        { 
                            "black",
                            1
                        },
                        { 
                            "white",
                            2
                        }
                    };
                    <>f__switch$map2B = dictionary;
                }
                if (<>f__switch$map2B.TryGetValue(key, out num))
                {
                    switch (num)
                    {
                        case 0:
                            this.messageManager.OffScreen();
                            this.messageManager.ClearText();
                            this.logMessage.ReturnText();
                            this.InitEffect();
                            this.InitFlash();
                            this.InitWipe();
                            this.InitFade();
                            SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(duration, null);
                            break;

                        case 1:
                            SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, duration, null);
                            break;

                        case 2:
                            SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.WHITE, duration, null);
                            break;
                    }
                }
            }
        }
        return true;
    }

    protected bool StartStretch(string name, bool isIn, float duration, float scale)
    {
        if (this.isExecuteStretch)
        {
            return false;
        }
        Vector3 one = Vector3.one;
        Vector3 zero = Vector3.zero;
        if (!isIn)
        {
            string key = name;
            if (key != null)
            {
                int num;
                if (<>f__switch$map27 == null)
                {
                    Dictionary<string, int> dictionary = new Dictionary<string, int>(7) {
                        { 
                            "full",
                            0
                        },
                        { 
                            "vertical",
                            1
                        },
                        { 
                            "verticalUp",
                            2
                        },
                        { 
                            "verticalDown",
                            3
                        },
                        { 
                            "horizontally",
                            4
                        },
                        { 
                            "horizontallyLeft",
                            5
                        },
                        { 
                            "horizontallyRight",
                            6
                        }
                    };
                    <>f__switch$map27 = dictionary;
                }
                if (<>f__switch$map27.TryGetValue(key, out num))
                {
                    switch (num)
                    {
                        case 0:
                            one.Set(scale, scale, 1f);
                            goto Label_0268;

                        case 1:
                            one.Set(1f, scale, 1f);
                            goto Label_0268;

                        case 2:
                            one.Set(1f, scale, 1f);
                            zero.Set(0f, this.stretchBaseRange.y * scale, 0f);
                            goto Label_0268;

                        case 3:
                            one.Set(1f, scale, 1f);
                            zero.Set(0f, -this.stretchBaseRange.y * scale, 0f);
                            goto Label_0268;

                        case 4:
                            one.Set(scale, 1f, 1f);
                            goto Label_0268;

                        case 5:
                            one.Set(scale, 1f, 1f);
                            zero.Set(this.stretchBaseRange.x * scale, 0f, 0f);
                            goto Label_0268;

                        case 6:
                            one.Set(scale, 1f, 1f);
                            zero.Set(-this.stretchBaseRange.x * scale, 0f, 0f);
                            goto Label_0268;
                    }
                }
            }
            Debug.LogError(string.Concat(new object[] { "ScriptManager strach error ", name, " [", this.assetName, ",", this.scriptLabel, ",", this.executeLineList[this.executeIndex - 1], "] " }));
            this.state = State.ERROR;
            return false;
        }
    Label_0268:
        if (duration > 0f)
        {
            TweenScale scale2 = TweenScale.Begin(this.stretchBase.gameObject, duration, one);
            if (scale2 == null)
            {
                return false;
            }
            TweenPosition.Begin(this.stretchBase.gameObject, duration, zero);
            scale2.eventReceiver = base.gameObject;
            scale2.callWhenFinished = "EndExecuteStretch";
            this.stretchName = name;
            this.isExecuteStretch = true;
        }
        else
        {
            this.stretchBase.transform.localScale = one;
            this.stretchBase.transform.localPosition = zero;
        }
        return true;
    }

    protected bool StartWipe(string name, bool isIn, float duration, float level)
    {
        if (!this.isLoadWipe)
        {
            if (this.isExecuteWipe && !this.isWipeFilter)
            {
                return false;
            }
            this.isExecuteWipe = true;
            this.isLoadWipe = true;
            this.wipeName = name;
            this.isWipeFilter = false;
            this.isWipeIn = isIn;
            this.wipeDuration = duration;
            this.wipeLevel = level;
            if (AssetManager.loadAssetStorage("Wipe/" + name, new AssetLoader.LoadEndDataHandler(this.EndLoadWipe)))
            {
                return true;
            }
            this.isLoadWipe = false;
            this.EndExecuteWipe();
        }
        return false;
    }

    protected void Update()
    {
        float deltaTime = RealTime.deltaTime;
        this.ProcessScript(deltaTime);
    }

    public void WriteSetting()
    {
        if ((((scriptServerSettingAddress != string.Empty) || (scriptObjectSettingAddress != string.Empty)) || ((scriptStartModeSettingName != string.Empty) || (scriptPlayerPathSettingAddress != string.Empty))) || (scriptPlayerObjectSettingAddress != string.Empty))
        {
            using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(getScriptSettingFileName())))
            {
                writer.Write(scriptServerSettingAddress);
                writer.Write(scriptObjectSettingAddress);
                writer.Write(scriptGenderSettingIndex);
                writer.Write(scriptStartModeSettingName);
                writer.Write(scriptPlayerPathSettingAddress);
                writer.Write(scriptPlayerObjectSettingAddress);
            }
        }
    }

    [CompilerGenerated]
    private sealed class <ImageCapture>c__Iterator16 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal string <captureName>__3;
        internal string <dateString>__1;
        internal string <imageFullPath>__4;
        internal string <imagePath>__2;
        internal int <index>__5;
        internal DateTime <now>__0;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                {
                    this.<now>__0 = DateTime.Now;
                    this.<dateString>__1 = this.<now>__0.ToString().Replace("/", "-");
                    this.<dateString>__1 = this.<dateString>__1.Replace(":", "-");
                    this.<dateString>__1 = this.<dateString>__1.Replace(" ", "_");
                    this.<imagePath>__2 = "C:/FGO/ScreenshotFolder";
                    this.<captureName>__3 = "capture_" + this.<dateString>__1 + ".png";
                    this.<imageFullPath>__4 = this.<imagePath>__2 + "/" + this.<captureName>__3;
                    if (!Directory.Exists(this.<imagePath>__2))
                    {
                        Directory.CreateDirectory(this.<imagePath>__2);
                    }
                    this.<index>__5 = 1;
                    while (true)
                    {
                        object[] objArray1 = new object[] { "capture_", this.<dateString>__1, "_", this.<index>__5, ".png" };
                        this.<captureName>__3 = string.Concat(objArray1);
                        this.<imageFullPath>__4 = this.<imagePath>__2 + "/" + this.<captureName>__3;
                        if (!File.Exists(this.<imageFullPath>__4))
                        {
                            break;
                        }
                        this.<index>__5++;
                    }
                }
                case 1:
                case 2:
                    if (File.Exists(this.<imageFullPath>__4))
                    {
                        this.$PC = -1;
                        goto Label_01C1;
                    }
                    this.$current = null;
                    this.$PC = 2;
                    goto Label_01C3;

                default:
                    goto Label_01C1;
            }
            Application.CaptureScreenshot(this.<imageFullPath>__4);
            this.$current = null;
            this.$PC = 1;
            goto Label_01C3;
        Label_01C1:
            return false;
        Label_01C3:
            return true;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current =>
            this.$current;

        object IEnumerator.Current =>
            this.$current;
    }

    [CompilerGenerated]
    private sealed class <LoadBattleEndGameDemoLocal>c__AnonStorey72
    {
        internal ScriptManager <>f__this;
        internal Action<string> callbackFunc;
        internal string demoInfo;

        internal void <>m__81(AssetData data)
        {
            this.<>f__this.gameDemoAssetData = data;
            this.<>f__this.LoadBattleStartGameDemoLocal(this.demoInfo, this.callbackFunc);
        }
    }

    [CompilerGenerated]
    private sealed class <LoadBattleStartGameDemoLocal>c__AnonStorey71
    {
        internal ScriptManager <>f__this;
        internal Action<string> callbackFunc;
        internal string demoInfo;

        internal void <>m__80(AssetData data)
        {
            this.<>f__this.gameDemoAssetData = data;
            this.<>f__this.LoadBattleStartGameDemoLocal(this.demoInfo, this.callbackFunc);
        }
    }

    [CompilerGenerated]
    private sealed class <PlayChapterStart>c__AnonStorey6F
    {
        internal ScriptManager.CallbackFunc callbackFunc;
        internal bool isCollection;
        internal int warId;

        internal void <>m__7D(bool isExit)
        {
            if (isExit)
            {
                if (this.callbackFunc != null)
                {
                    this.callbackFunc(true);
                }
            }
            else
            {
                ScriptManager._playChapterStart(this.warId, this.callbackFunc, this.isCollection);
            }
        }
    }

    [CompilerGenerated]
    private sealed class <PlayQuestStart>c__AnonStorey70
    {
        internal System.Action callbackFunc;

        internal void <>m__7E(bool isExit)
        {
            if (this.callbackFunc != null)
            {
                this.callbackFunc.Call();
            }
        }

        internal void <>m__7F(bool isExit)
        {
            if (this.callbackFunc != null)
            {
                this.callbackFunc.Call();
            }
        }
    }

    [CompilerGenerated]
    private sealed class <settalk>c__Iterator15 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal float <$>delay;
        internal string <$>text;
        internal int <$>type;
        internal ScriptManager <>f__this;
        internal int <i>__3;
        internal string <str>__1;
        internal float <t>__2;
        internal string <te>__0;
        internal float delay;
        internal string text;
        internal int type;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                    if (this.delay <= 0f)
                    {
                        break;
                    }
                    this.$current = new WaitForSeconds(this.delay);
                    this.$PC = 1;
                    goto Label_0411;

                case 1:
                    break;

                case 2:
                    goto Label_02A1;

                case 3:
                    goto Label_0389;

                case 4:
                    goto Label_03C5;

                case 5:
                    this.$PC = -1;
                    goto Label_040F;

                default:
                    goto Label_040F;
            }
            if (!this.<>f__this.Talk.activeSelf && !string.IsNullOrEmpty(this.text))
            {
                if (this.text.StartsWith("[image"))
                {
                    this.<te>__0 = this.text.Replace("[image ", string.Empty);
                    this.<te>__0 = this.<te>__0.Replace("]", string.Empty);
                    this.<>f__this.TalkSprText.spriteName = this.<te>__0;
                    this.<>f__this.TalkLab.gameObject.SetActive(false);
                    this.<>f__this.TalkSprText.gameObject.SetActive(true);
                }
                else
                {
                    this.<>f__this.TalkSprText.gameObject.SetActive(false);
                    this.<>f__this.TalkLab.gameObject.SetActive(true);
                }
                if (this.type == 0)
                {
                    this.<>f__this.TalkSpr.width = 0x200;
                    this.<>f__this.TalkLab.width = 400;
                }
                else
                {
                    this.<>f__this.TalkSpr.width = 0x400;
                    this.<>f__this.TalkLab.width = 850;
                }
                this.<>f__this.Talk.SetActive(true);
                this.<str>__1 = string.Empty;
                this.<>f__this.TalkLab.text = string.Empty;
                if (this.text == null)
                {
                    this.<str>__1 = "暂无对话\n暂无对话";
                }
                else
                {
                    this.text = this.text.Replace("[r]", string.Empty);
                    this.<str>__1 = this.<str>__1 + this.text;
                }
                this.<>f__this.bfinish = false;
                this.<t>__2 = 0f;
                this.<i>__3 = 0;
                while (this.<i>__3 < this.<str>__1.Length)
                {
                    this.<t>__2++;
                    if (((this.<t>__2 * 38f) / ((float) this.<>f__this.TalkLab.width)) <= 3.85f)
                    {
                        goto Label_02C1;
                    }
                    this.$current = new WaitForSeconds(3f);
                    this.$PC = 2;
                    goto Label_0411;
                Label_02A1:
                    this.<>f__this.TalkLab.text = string.Empty;
                    this.<t>__2 = 0f;
                Label_02C1:
                    this.<>f__this.TalkLab.text = this.<>f__this.TalkLab.text + this.<str>__1[this.<i>__3];
                    if (this.<>f__this.TalkLab.height <= 0x72)
                    {
                        goto Label_03A9;
                    }
                    char ch = this.<str>__1[this.<i>__3];
                    if (string.Compare(ch.ToString(), "\n") != 0)
                    {
                        this.<>f__this.TalkLab.text.Substring(0, this.<>f__this.TalkLab.text.Length - 1);
                        this.<i>__3--;
                    }
                    this.$current = new WaitForSeconds(3f);
                    this.$PC = 3;
                    goto Label_0411;
                Label_0389:
                    this.<>f__this.TalkLab.text = string.Empty;
                    this.<t>__2 = 0f;
                Label_03A9:
                    this.$current = 0.2f;
                    this.$PC = 4;
                    goto Label_0411;
                Label_03C5:
                    this.<i>__3++;
                }
                this.<>f__this.bfinish = true;
                this.$current = null;
                this.$PC = 5;
                goto Label_0411;
            }
        Label_040F:
            return false;
        Label_0411:
            return true;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current =>
            this.$current;

        object IEnumerator.Current =>
            this.$current;
    }

    public delegate void CallbackFunc(bool isExit);

    protected enum InputTopMode
    {
        NORMAL,
        MENU,
        SKIP_CONFIRM,
        NOTIFICATION,
        INPUT,
        BACK_LOG,
        SKIP_VOICE,
        SHOW_BACK,
        AUTO,
        EXIT
    }

    protected enum PlayMode
    {
        NORMAL,
        DEBUG,
        BACK,
        FIGURE
    }

    protected enum PlayScriptDebugMode
    {
        NONE,
        STATE,
        ALL,
        END
    }

    protected enum PlaySpeed
    {
        NONE,
        PAUSE,
        NORMAL,
        FAST
    }

    protected enum StartMode
    {
        NONE,
        CLEAR_BLACK,
        CLEAR_WHITE,
        BLACK_CLEAR,
        WHITE_CLEAR,
        CLEAR,
        BLACK,
        WHITE,
        FULL,
        CLEAR_FULL,
        BLACK_FULL,
        WHITE_FULL,
        CLEAR_BLACK_FULL,
        CLEAR_WHITE_FULL,
        THROUGH,
        BLACK_SCENE_STOP
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

