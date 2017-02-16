using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class SceneManager : SingletonMonoBehaviour<SceneManager>
{
    public GameObject activerootobject;
    public static readonly float DEFAULT_FADE_TIME = 0.2f;
    protected FadeType fadeType;
    protected string firstSceneName;
    protected bool isBusySceneChange;
    protected PlayMakerGUI playMakerGUI;
    protected SceneTemp prevscenetemp;
    protected Stack<SceneTemp> scenestack = new Stack<SceneTemp>();
    protected object sendData;
    public GameObject stackrootobject;
    public SceneRootComponent targetRoot;

    private bool _popScene(FadeType fade, object data, bool is_refresh)
    {
        if (this.scenestack.Count <= 0)
        {
            return false;
        }
        SceneTemp temp = this.scenestack.Pop();
        ChangeType changeType = !is_refresh ? ChangeType.POP : ChangeType.POP_RELEASE;
        this.changeScene(changeType, temp.getSceneName(), fade, data);
        return true;
    }

    public bool changeScene(SceneList.Type type, FadeType fade = 1, object data = null) => 
        this.changeScene(ChangeType.TRANS, SceneList.getSceneName(type), fade, data);

    public bool changeScene(string scenename, FadeType fade = 1, object data = null) => 
        this.changeScene(ChangeType.TRANS, scenename, fade, data);

    protected bool changeScene(ChangeType changeType, string scenename, FadeType fade, object data)
    {
        if (scenename == null)
        {
            Debug.LogError("SceneManager:changeScene null error");
            return false;
        }
        if (this.isBusySceneChange)
        {
            Debug.LogError("SceneManager:changeScene is busy");
            return false;
        }
        this.isBusySceneChange = true;
        this.fadeType = fade;
        this.sendData = data;
        Debug.Log("changeScene");
        base.StartCoroutine(this.changeSceneCoroutine(changeType, scenename));
        return true;
    }

    [DebuggerHidden]
    private IEnumerator changeSceneCoroutine(ChangeType changeType, string scenename) => 
        new <changeSceneCoroutine>c__IteratorF { 
            changeType = changeType,
            scenename = scenename,
            <$>changeType = changeType,
            <$>scenename = scenename,
            <>f__this = this
        };

    protected void destroySceneObject()
    {
        this.prevscenetemp = null;
        this.scenestack.Clear();
        if (this.targetRoot != null)
        {
            UnityEngine.Object.Destroy(this.targetRoot.gameObject);
            this.targetRoot = null;
        }
        List<GameObject> list = new List<GameObject>();
        for (int i = 0; i < this.stackrootobject.transform.childCount; i++)
        {
            SceneRootComponent component = this.stackrootobject.transform.GetChild(i).GetComponent<SceneRootComponent>();
            if (component != null)
            {
                list.Add(component.gameObject);
            }
        }
        foreach (GameObject obj2 in list)
        {
            UnityEngine.Object.Destroy(obj2);
        }
    }

    public void endInitialize(SceneRootComponent comp)
    {
        Debug.Log("SceneManager::endInitialize");
        this.playMakerGUI = GameObject.Find("PlayMakerGUI").GetComponent<PlayMakerGUI>();
        object sendData = this.sendData;
        this.sendData = null;
        comp.beginStartUp(sendData);
    }

    public string GetNowSceneName()
    {
        if (this.prevscenetemp != null)
        {
            return this.prevscenetemp.getSceneName();
        }
        return null;
    }

    public bool IsStackScene() => 
        (this.scenestack.Count > 0);

    public bool popScene(FadeType fade = 1, object data = null) => 
        this._popScene(fade, data, false);

    public bool popSceneRefresh(FadeType fade = 1, object data = null) => 
        this._popScene(fade, data, true);

    public bool pushScene(SceneList.Type type, FadeType fade = 1, object data = null)
    {
        SceneTemp t = null;
        if ((this.targetRoot != null) && (this.prevscenetemp != null))
        {
            t = this.prevscenetemp;
            this.scenestack.Push(t);
        }
        if (!this.changeScene(ChangeType.PUSH, SceneList.getSceneName(type), fade, data))
        {
            if (t != null)
            {
                this.scenestack.Pop();
                this.changeScene(ChangeType.POP, t.getSceneName(), fade, data);
            }
            return false;
        }
        Debug.Log("scenestack.Count:" + this.scenestack.Count);
        return true;
    }

    public void reboot()
    {
        this.destroySceneObject();
    }

    public bool SetTargetRootActive(bool flag) => 
        ((this.targetRoot != null) && this.targetRoot.SetSceneActive(flag));

    public bool transitionScene(SceneList.Type type, FadeType fade = 1, object data = null) => 
        this.changeScene(ChangeType.CLEAR, SceneList.getSceneName(type), fade, data);

    public bool transitionScene(string scenename, FadeType fade = 1, object data = null) => 
        this.changeScene(ChangeType.CLEAR, scenename, fade, data);

    public bool transitionSceneRefresh(SceneList.Type type, FadeType fade = 1, object data = null) => 
        this.changeScene(ChangeType.RELEASE, SceneList.getSceneName(type), fade, data);

    [CompilerGenerated]
    private sealed class <changeSceneCoroutine>c__IteratorF : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal SceneManager.ChangeType <$>changeType;
        internal string <$>scenename;
        internal UnityEngine.Object[] <$s_265>__10;
        internal int <$s_266>__11;
        internal SceneManager <>f__this;
        internal AudioListener <a>__15;
        internal Behaviour <behavior>__13;
        internal Camera <c>__14;
        internal SceneRootComponent <comp>__3;
        internal SceneRootComponent <comp>__7;
        internal UnityEngine.Object <component>__12;
        internal UnityEngine.Object[] <components>__9;
        internal object <data>__16;
        internal int <i>__1;
        internal int <i>__5;
        internal SceneRootComponent <newTargetRoot>__4;
        internal GameObject <nowscene>__8;
        internal SceneTemp <st>__0;
        internal Transform <t>__2;
        internal Transform <t>__6;
        internal SceneManager.ChangeType changeType;
        internal string scenename;

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
                    if (this.<>f__this.fadeType == SceneManager.FadeType.NONE)
                    {
                        goto Label_00DA;
                    }
                    this.$current = 0;
                    this.$PC = 1;
                    goto Label_07D1;

                case 1:
                    switch (this.<>f__this.fadeType)
                    {
                        case SceneManager.FadeType.BLACK:
                            SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, null);
                            goto Label_00CB;

                        case SceneManager.FadeType.WHITE:
                            SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.WHITE, SceneManager.DEFAULT_FADE_TIME, null);
                            goto Label_00CB;
                    }
                    break;

                case 2:
                    break;

                case 3:
                    this.<>f__this.destroySceneObject();
                    this.$current = 0;
                    this.$PC = 4;
                    goto Label_07D1;

                case 4:
                    SingletonMonoBehaviour<AtlasManager>.Instance.ReleaseNoneResidentAtlas();
                    Resources.UnloadUnusedAssets();
                    GC.Collect();
                    goto Label_0288;

                case 5:
                    this.<nowscene>__8 = GameObject.Find(this.scenename);
                    this.$current = 0;
                    this.$PC = 6;
                    goto Label_07D1;

                case 6:
                    if (this.<nowscene>__8 != null)
                    {
                        this.<newTargetRoot>__4 = this.<nowscene>__8.GetComponent<SceneRootComponent>();
                        goto Label_0477;
                    }
                    Debug.LogError("SceneManager:changeScene error [" + this.scenename + "] is null");
                    goto Label_07CF;

                case 7:
                    if ((this.changeType != SceneManager.ChangeType.POP_RELEASE) && (this.<>f__this.targetRoot != this.<newTargetRoot>__4))
                    {
                        this.<>f__this.targetRoot.gameObject.SetActive(false);
                    }
                    goto Label_0692;

                default:
                    goto Label_07CF;
            }
        Label_00CB:
            while (SingletonMonoBehaviour<CommonUI>.Instance.maskFadeIsBusy())
            {
                this.$current = 0;
                this.$PC = 2;
                goto Label_07D1;
            }
        Label_00DA:
            MainMenuBar.setButtonActive(false);
            SingletonMonoBehaviour<CommonUI>.Instance.InitTurotialArrowMark();
            if ((this.changeType == SceneManager.ChangeType.CLEAR) || (this.changeType == SceneManager.ChangeType.RELEASE))
            {
                if (this.<>f__this.prevscenetemp != null)
                {
                    if (this.<>f__this.targetRoot != null)
                    {
                        this.<>f__this.targetRoot.beginFinish();
                    }
                    this.<>f__this.prevscenetemp = null;
                }
                while (this.<>f__this.scenestack.Count > 0)
                {
                    this.<st>__0 = this.<>f__this.scenestack.Pop();
                    this.<i>__1 = 0;
                    while (this.<i>__1 < this.<>f__this.stackrootobject.transform.childCount)
                    {
                        this.<t>__2 = this.<>f__this.stackrootobject.transform.GetChild(this.<i>__1);
                        this.<comp>__3 = this.<t>__2.GetComponent<SceneRootComponent>();
                        if (this.<comp>__3.checkSceneName(this.<st>__0.getSceneName()))
                        {
                            this.<comp>__3.gameObject.SetActive(true);
                            this.<comp>__3.beginFinish();
                            this.<comp>__3.gameObject.SetActive(false);
                            continue;
                        }
                        this.<i>__1++;
                    }
                }
                if (this.changeType == SceneManager.ChangeType.RELEASE)
                {
                    this.$current = 0;
                    this.$PC = 3;
                    goto Label_07D1;
                }
            }
        Label_0288:
            this.<newTargetRoot>__4 = null;
            if (this.<>f__this.targetRoot != null)
            {
                if ((this.scenename == string.Empty) || this.<>f__this.targetRoot.checkSceneName(this.scenename))
                {
                    this.<newTargetRoot>__4 = this.<>f__this.targetRoot;
                }
            }
            else if (this.scenename == string.Empty)
            {
                Debug.LogError("SceneManager:changeScene root null error");
                this.<newTargetRoot>__4 = this.<>f__this.targetRoot;
            }
            if (this.<newTargetRoot>__4 == null)
            {
                this.<i>__5 = 0;
                while (this.<i>__5 < this.<>f__this.stackrootobject.transform.childCount)
                {
                    this.<t>__6 = this.<>f__this.stackrootobject.transform.GetChild(this.<i>__5);
                    this.<comp>__7 = this.<t>__6.GetComponent<SceneRootComponent>();
                    if (this.<comp>__7.checkSceneName(this.scenename))
                    {
                        this.<newTargetRoot>__4 = this.<comp>__7;
                        break;
                    }
                    this.<i>__5++;
                }
                if (this.<newTargetRoot>__4 == null)
                {
                    Debug.Log("SceneManager:changeScene load [" + this.scenename + "]");
                    Application.LoadLevelAdditive(this.scenename);
                    this.$current = 0;
                    this.$PC = 5;
                    goto Label_07D1;
                }
            }
        Label_0477:
            if (this.<>f__this.playMakerGUI != null)
            {
                this.<components>__9 = UnityEngine.Object.FindObjectsOfType(typeof(PlayMakerGUI));
                if (this.<components>__9.Length > 1)
                {
                    this.<$s_265>__10 = this.<components>__9;
                    this.<$s_266>__11 = 0;
                    while (this.<$s_266>__11 < this.<$s_265>__10.Length)
                    {
                        this.<component>__12 = this.<$s_265>__10[this.<$s_266>__11];
                        if (this.<component>__12 != this.<>f__this.playMakerGUI)
                        {
                            this.<behavior>__13 = (PlayMakerGUI) this.<component>__12;
                            if (this.<behavior>__13.gameObject.GetComponents(typeof(Component)).Length == 2)
                            {
                                UnityEngine.Object.DestroyImmediate(this.<behavior>__13.gameObject);
                            }
                            else
                            {
                                UnityEngine.Object.DestroyImmediate(this.<component>__12);
                            }
                        }
                        this.<$s_266>__11++;
                    }
                }
            }
            if (this.<newTargetRoot>__4 == null)
            {
                Debug.LogError("SceneManager:changeScene targetRoot error");
                goto Label_07C8;
            }
            if (this.<>f__this.targetRoot != null)
            {
                if (this.<>f__this.targetRoot != this.<newTargetRoot>__4)
                {
                    if (this.changeType == SceneManager.ChangeType.POP_RELEASE)
                    {
                        UnityEngine.Object.Destroy(this.<>f__this.targetRoot.gameObject);
                    }
                    else
                    {
                        this.<>f__this.targetRoot.gameObject.transform.parent = this.<>f__this.stackrootobject.transform;
                    }
                }
                if (this.<>f__this.prevscenetemp != null)
                {
                    if (this.changeType == SceneManager.ChangeType.PUSH)
                    {
                        this.<>f__this.targetRoot.beginPause();
                    }
                    else
                    {
                        this.<>f__this.targetRoot.beginFinish();
                    }
                }
                this.$current = 0;
                this.$PC = 7;
                goto Label_07D1;
            }
        Label_0692:
            this.<newTargetRoot>__4.gameObject.transform.parent = this.<>f__this.activerootobject.transform;
            this.<>f__this.targetRoot = this.<newTargetRoot>__4;
            this.<>f__this.targetRoot.gameObject.SetActive(true);
            this.<>f__this.prevscenetemp = new SceneTemp(this.<>f__this.targetRoot);
            this.<c>__14 = this.<>f__this.targetRoot.GetComponentInChildren<Camera>();
            if (this.<c>__14 != null)
            {
                this.<a>__15 = this.<c>__14.GetComponent<AudioListener>();
                if (this.<a>__15 != null)
                {
                    UnityEngine.Object.Destroy(this.<a>__15);
                }
            }
            this.<>f__this.isBusySceneChange = false;
            if ((this.changeType == SceneManager.ChangeType.POP) || (this.changeType == SceneManager.ChangeType.POP_RELEASE))
            {
                this.<data>__16 = this.<>f__this.sendData;
                this.<>f__this.sendData = null;
                this.<>f__this.targetRoot.beginResume(this.<data>__16);
            }
            else
            {
                this.<>f__this.targetRoot.beginInitialize();
            }
        Label_07C8:
            this.$PC = -1;
        Label_07CF:
            return false;
        Label_07D1:
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

    protected enum ChangeType
    {
        RELEASE,
        CLEAR,
        TRANS,
        PUSH,
        POP,
        POP_RELEASE
    }

    public enum FadeType
    {
        BLACK = 1,
        DEFAULT = 1,
        NONE = 0,
        WHITE = 2
    }
}

