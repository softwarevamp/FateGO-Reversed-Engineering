using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SoundTest : MonoBehaviour
{
    public string CueName = "0_B150";
    public string CueSheetName = "Servants_100100";
    public CriAtomSource player;
    private Dictionary<string, CriFsBinder> SoundBinders;

    [DebuggerHidden]
    private IEnumerator SoundPlay() => 
        new <SoundPlay>c__Iterator41 { <>f__this = this };

    private void Start()
    {
        this.SoundBinders = new Dictionary<string, CriFsBinder>();
        base.StartCoroutine(this.SoundPlay());
    }

    private void Update()
    {
    }

    [CompilerGenerated]
    private sealed class <SoundPlay>c__Iterator41 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal CriAtomEx.CueInfo[] <$s_1738>__5;
        internal int <$s_1739>__6;
        internal SoundTest <>f__this;
        internal CriAtomExAcb <acb>__3;
        internal CriFsBindRequest <bind_request>__2;
        internal CriFsBinder <binder>__0;
        internal string <cpkAssetPath>__1;
        internal CriAtomEx.CueInfo[] <cueInfos>__4;
        internal CriAtomEx.CueInfo <info>__7;

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
                    this.<binder>__0 = new CriFsBinder();
                    this.<>f__this.SoundBinders[this.<>f__this.CueSheetName] = this.<binder>__0;
                    this.<cpkAssetPath>__1 = Application.dataPath + "/AssetStorages/Audio/" + this.<>f__this.CueSheetName + ".cpk.bytes";
                    if (File.Exists(this.<cpkAssetPath>__1))
                    {
                        this.<bind_request>__2 = CriFsUtility.BindCpk(this.<binder>__0, this.<cpkAssetPath>__1);
                        this.$current = this.<bind_request>__2.WaitForDone(this.<>f__this);
                        this.$PC = 1;
                        goto Label_02D5;
                    }
                    Debug.LogError("Can not Found Audio :" + this.<cpkAssetPath>__1);
                    goto Label_02D3;

                case 1:
                    if (this.<bind_request>__2.error == null)
                    {
                        this.$current = new WaitForSeconds(0.5f);
                        this.$PC = 2;
                        goto Label_02D5;
                    }
                    Debug.LogError("Binder Error");
                    goto Label_02D3;

                case 2:
                    if (this.<binder>__0.GetFileSize(this.<>f__this.CueSheetName + ".awb") < 0L)
                    {
                        CriAtom.AddCueSheet(this.<>f__this.CueSheetName, this.<>f__this.CueSheetName + ".acb", string.Empty, this.<binder>__0);
                        break;
                    }
                    CriAtom.AddCueSheet(this.<>f__this.CueSheetName, this.<>f__this.CueSheetName + ".acb", this.<>f__this.CueSheetName + ".awb", this.<binder>__0);
                    break;

                case 3:
                    this.<acb>__3 = CriAtom.GetAcb(this.<>f__this.CueSheetName);
                    this.<cueInfos>__4 = this.<acb>__3.GetCueInfoList();
                    this.<$s_1738>__5 = this.<cueInfos>__4;
                    this.<$s_1739>__6 = 0;
                    while (this.<$s_1739>__6 < this.<$s_1738>__5.Length)
                    {
                        this.<info>__7 = this.<$s_1738>__5[this.<$s_1739>__6];
                        Debug.Log("name:" + this.<info>__7.name);
                        this.<$s_1739>__6++;
                    }
                    this.<>f__this.player.cueSheet = this.<>f__this.CueSheetName;
                    this.<>f__this.player.cueName = this.<>f__this.CueName;
                    this.<>f__this.player.Play();
                    this.$current = 0;
                    this.$PC = 4;
                    goto Label_02D5;

                case 4:
                    this.$PC = -1;
                    goto Label_02D3;

                default:
                    goto Label_02D3;
            }
            this.$current = new WaitForSeconds(0.5f);
            this.$PC = 3;
            goto Label_02D5;
        Label_02D3:
            return false;
        Label_02D5:
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
}

