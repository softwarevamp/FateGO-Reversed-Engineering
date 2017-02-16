using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BattleHitStopControl : BaseMonoBehaviour
{
    private List<GameObject> stopobjlist;

    public void actWait(float time)
    {
        base.StartCoroutine(this.stopwaitCor(time));
    }

    public void Clear()
    {
        this.stopobjlist.Clear();
    }

    public void setTargetObject(GameObject obj)
    {
        this.stopobjlist.Add(obj);
    }

    private void Start()
    {
        this.stopobjlist = new List<GameObject>();
    }

    [DebuggerHidden]
    private IEnumerator stopwaitCor(float time) => 
        new <stopwaitCor>c__Iterator18 { 
            time = time,
            <$>time = time,
            <>f__this = this
        };

    [CompilerGenerated]
    private sealed class <stopwaitCor>c__Iterator18 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal float <$>time;
        internal List<GameObject>.Enumerator <$s_834>__0;
        internal List<GameObject>.Enumerator <$s_835>__2;
        internal BattleHitStopControl <>f__this;
        internal GameObject <obj>__1;
        internal GameObject <obj>__3;
        internal float time;

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
                    this.$current = new WaitForSeconds(0.01f);
                    this.$PC = 1;
                    goto Label_0135;

                case 1:
                    this.<$s_834>__0 = this.<>f__this.stopobjlist.GetEnumerator();
                    try
                    {
                        while (this.<$s_834>__0.MoveNext())
                        {
                            this.<obj>__1 = this.<$s_834>__0.Current;
                            this.<obj>__1.SendMessage("stopAnimation");
                        }
                    }
                    finally
                    {
                        this.<$s_834>__0.Dispose();
                    }
                    Debug.Log("hitstop");
                    this.$current = new WaitForSeconds(this.time);
                    this.$PC = 2;
                    goto Label_0135;

                case 2:
                    this.<$s_835>__2 = this.<>f__this.stopobjlist.GetEnumerator();
                    try
                    {
                        while (this.<$s_835>__2.MoveNext())
                        {
                            this.<obj>__3 = this.<$s_835>__2.Current;
                            this.<obj>__3.SendMessage("resumeAnimation");
                        }
                    }
                    finally
                    {
                        this.<$s_835>__2.Dispose();
                    }
                    this.$PC = -1;
                    break;
            }
            return false;
        Label_0135:
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

