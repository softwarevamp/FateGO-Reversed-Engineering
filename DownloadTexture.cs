using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(UITexture))]
public class DownloadTexture : MonoBehaviour
{
    private Texture2D mTex;
    public bool pixelPerfect = true;
    public string url = "http://www.yourwebsite.com/logo.png";

    private void OnDestroy()
    {
        if (this.mTex != null)
        {
            UnityEngine.Object.Destroy(this.mTex);
        }
    }

    [DebuggerHidden]
    private IEnumerator Start() => 
        new <Start>c__Iterator3C { <>f__this = this };

    [CompilerGenerated]
    private sealed class <Start>c__Iterator3C : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal DownloadTexture <>f__this;
        internal UITexture <ut>__1;
        internal WWW <www>__0;

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
                    this.<www>__0 = new WWW(this.<>f__this.url);
                    this.$current = this.<www>__0;
                    this.$PC = 1;
                    return true;

                case 1:
                    this.<>f__this.mTex = this.<www>__0.texture;
                    if (this.<>f__this.mTex != null)
                    {
                        this.<ut>__1 = this.<>f__this.GetComponent<UITexture>();
                        this.<ut>__1.mainTexture = this.<>f__this.mTex;
                        if (this.<>f__this.pixelPerfect)
                        {
                            this.<ut>__1.MakePixelPerfect();
                        }
                    }
                    this.<www>__0.Dispose();
                    this.$PC = -1;
                    break;
            }
            return false;
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

