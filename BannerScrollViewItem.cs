using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BannerScrollViewItem : BaseScrollViewItem
{
    [CompilerGenerated]
    private static System.Action <>f__am$cache1;
    private AnnouncementData data = new AnnouncementData();

    public void Click()
    {
        if (<>f__am$cache1 == null)
        {
            <>f__am$cache1 = delegate {
            };
        }
        WebViewManager.OpenUniWebView(string.Empty, this.data.url, <>f__am$cache1);
    }

    [DebuggerHidden]
    private IEnumerator InitImage() => 
        new <InitImage>c__Iterator2 { <>f__this = this };

    public override void SetItem(params object[] o)
    {
        this.data = (AnnouncementData) o[0];
        if (base.gameObject.activeInHierarchy)
        {
            base.StartCoroutine(this.InitImage());
        }
        Debug.LogError(this.data.imgurl);
    }

    private void Start()
    {
    }

    private void Update()
    {
    }

    [CompilerGenerated]
    private sealed class <InitImage>c__Iterator2 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal BannerScrollViewItem <>f__this;
        internal WWW <load>__0;

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
                    this.<load>__0 = new WWW(this.<>f__this.data.imgurl);
                    this.$current = this.<load>__0;
                    this.$PC = 1;
                    return true;

                case 1:
                    if (!string.IsNullOrEmpty(this.<load>__0.error))
                    {
                        Debug.Log(this.<load>__0.error);
                        break;
                    }
                    Debug.Log("aaaa   " + this.<load>__0.texture);
                    this.<>f__this.GetComponent<UITexture>().mainTexture = this.<load>__0.texture;
                    this.<load>__0.Dispose();
                    break;

                default:
                    goto Label_00C5;
            }
            this.$PC = -1;
        Label_00C5:
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

