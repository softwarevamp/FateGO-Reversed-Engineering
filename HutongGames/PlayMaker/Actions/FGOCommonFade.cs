namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory("FGO Common Action")]
    public class FGOCommonFade : FsmStateAction
    {
        public FsmBool fadeIn = 0;
        public MaskFade.Kind fadeOutKind = MaskFade.Kind.BLACK;
        public FsmFloat fadeTime = SceneManager.DEFAULT_FADE_TIME;
        public FsmEvent finishEvent;
        public bool FinishOnFadeEnd;

        public override void OnEnter()
        {
            if (this.fadeIn.Value)
            {
                SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(this.fadeTime.Value, delegate {
                    if (this.finishEvent != null)
                    {
                        base.Fsm.Event(this.finishEvent);
                    }
                    if (this.FinishOnFadeEnd)
                    {
                        base.Finish();
                    }
                });
            }
            else
            {
                SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(this.fadeOutKind, this.fadeTime.Value, delegate {
                    if (this.finishEvent != null)
                    {
                        base.Fsm.Event(this.finishEvent);
                    }
                    if (this.FinishOnFadeEnd)
                    {
                        base.Finish();
                    }
                });
            }
            if (!this.FinishOnFadeEnd)
            {
                base.Finish();
            }
        }
    }
}

