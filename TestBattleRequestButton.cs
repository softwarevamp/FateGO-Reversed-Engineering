using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TestBattleRequestButton : SceneRootComponent
{
    private bool isRequest;

    [DebuggerHidden]
    private IEnumerator battleRequest(int questNo, int questPhase) => 
        new <battleRequest>c__Iterator3B { 
            questNo = questNo,
            questPhase = questPhase,
            <$>questNo = questNo,
            <$>questPhase = questPhase,
            <>f__this = this
        };

    public override void beginInitialize()
    {
        base.beginInitialize();
        SingletonMonoBehaviour<SceneManager>.Instance.endInitialize(this);
    }

    public override void beginStartUp()
    {
        base.beginStartUp();
    }

    private void callbackRequest(string result)
    {
        this.isRequest = true;
    }

    public void startRequestA()
    {
        base.StartCoroutine(this.battleRequest(0x3f2, 1));
    }

    [CompilerGenerated]
    private sealed class <battleRequest>c__Iterator3B : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal int <$>questNo;
        internal int <$>questPhase;
        internal TestBattleRequestButton <>f__this;
        internal BattleSetupRequest <request>__1;
        internal UserGameEntity <userGameEntity>__0;
        internal int questNo;
        internal int questPhase;

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
                    this.<>f__this.isRequest = false;
                    this.<userGameEntity>__0 = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
                    this.<request>__1 = NetworkManager.getRequest<BattleSetupRequest>(new NetworkManager.ResultCallbackFunc(this.<>f__this.callbackRequest));
                    this.<request>__1.beginRequest(this.questNo, this.questPhase, (this.<userGameEntity>__0 == null) ? 0L : this.<userGameEntity>__0.activeDeckId, 0x3e8L, 0, 1);
                    break;

                case 1:
                    if (!this.<>f__this.isRequest)
                    {
                        break;
                    }
                    SingletonMonoBehaviour<SceneManager>.Instance.changeScene(SceneList.Type.Battle, SceneManager.FadeType.BLACK, null);
                    this.$PC = -1;
                    goto Label_00DE;

                default:
                    goto Label_00DE;
            }
            this.$current = new WaitForEndOfFrame();
            this.$PC = 1;
            return true;
        Label_00DE:
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

