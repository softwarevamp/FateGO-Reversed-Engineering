namespace HutongGames.PlayMaker.Actions
{
    using AnimationOrTween;
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Play a NGUI Tween. It is the same as the NGUI component 'Button Tween', only without the need for a NGUI button"), ActionCategory("NGUI")]
    public class NguiPlayTween : FsmStateAction
    {
        private GameObject _tweenTarget;
        [HutongGames.PlayMaker.Tooltip("What to do with the tweenTarget after the tween finishes.")]
        public DisableCondition disableWhenFinished;
        [HutongGames.PlayMaker.Tooltip("What to do if the tweenTarget game object is currently disabled.")]
        public EnableCondition ifDisabledOnPlay;
        [HutongGames.PlayMaker.Tooltip("Whether the tweens on the child game objects will be considered.")]
        public FsmBool includeChildren;
        private int mActive;
        private UITweener[] mTweens;
        [HutongGames.PlayMaker.Tooltip("Direction to tween in.")]
        public AnimationOrTween.Direction playDirection;
        [HutongGames.PlayMaker.Tooltip("Whether the tween will be reset to the start if it's disabled when activated.")]
        public FsmBool resetIfDisabled;
        [HutongGames.PlayMaker.Tooltip("Whether the tween will be reset to the start or end when activated. If not, it will continue from where it currently is.")]
        public FsmBool resetOnPlay;
        [HutongGames.PlayMaker.Tooltip("If there are multiple tweens, you can choose which ones get activated by changing their group. Default is 0")]
        public FsmInt tweenGroup;
        [HutongGames.PlayMaker.Tooltip("Event to trigger when the tween finishes.")]
        public FsmEvent tweeningFinishedEvent;
        [RequiredField, HutongGames.PlayMaker.Tooltip("The GameObject on which there is one or more NGUI tween."), CheckForComponent(typeof(UITweener))]
        public FsmOwnerDefault tweenTarget;

        public override void OnEnter()
        {
            this._tweenTarget = base.Fsm.GetOwnerDefaultTarget(this.tweenTarget);
            if (this._tweenTarget == null)
            {
                this.LogWarning("no gameObject target to tween");
            }
            else
            {
                this.PlayTweeners();
                if ((this.disableWhenFinished == DisableCondition.DoNotDisable) && (this.tweeningFinishedEvent == null))
                {
                    base.Finish();
                }
            }
        }

        private void OnFinished()
        {
            if (--this.mActive == 0)
            {
                base.Fsm.Event(this.tweeningFinishedEvent);
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            if ((this.disableWhenFinished != DisableCondition.DoNotDisable) && (this.mTweens != null))
            {
                bool flag = true;
                bool flag2 = true;
                int index = 0;
                int length = this.mTweens.Length;
                while (index < length)
                {
                    UITweener tweener = this.mTweens[index];
                    if (tweener.tweenGroup == this.tweenGroup.Value)
                    {
                        if (tweener.enabled)
                        {
                            flag = false;
                            break;
                        }
                        if (tweener.direction != ((AnimationOrTween.Direction) ((int) this.disableWhenFinished)))
                        {
                            flag2 = false;
                        }
                    }
                    index++;
                }
                if (flag)
                {
                    if (flag2)
                    {
                        NGUITools.SetActive(this._tweenTarget, false);
                    }
                    this.mTweens = null;
                }
            }
        }

        public void PlayTweeners()
        {
            if (!NGUITools.GetActive(this._tweenTarget))
            {
                if (this.ifDisabledOnPlay != EnableCondition.EnableThenPlay)
                {
                    return;
                }
                NGUITools.SetActive(this._tweenTarget, true);
            }
            this.mTweens = !this.includeChildren.Value ? this._tweenTarget.GetComponents<UITweener>() : this._tweenTarget.GetComponentsInChildren<UITweener>();
            if (this.mTweens.Length == 0)
            {
                if (this.disableWhenFinished != DisableCondition.DoNotDisable)
                {
                    NGUITools.SetActive(this._tweenTarget, false);
                }
            }
            else
            {
                bool flag = false;
                bool forward = this.playDirection == AnimationOrTween.Direction.Forward;
                int index = 0;
                int length = this.mTweens.Length;
                while (index < length)
                {
                    UITweener tweener = this.mTweens[index];
                    if (tweener.tweenGroup == this.tweenGroup.Value)
                    {
                        if (!flag && !NGUITools.GetActive(this._tweenTarget))
                        {
                            flag = true;
                            NGUITools.SetActive(this._tweenTarget, true);
                        }
                        this.mActive++;
                        if (this.playDirection == AnimationOrTween.Direction.Toggle)
                        {
                            tweener.Toggle();
                        }
                        else
                        {
                            if (this.resetOnPlay.Value || (this.resetIfDisabled.Value && !tweener.enabled))
                            {
                                tweener.ResetToBeginning();
                            }
                            tweener.Play(forward);
                        }
                        EventDelegate.Add(tweener.onFinished, new EventDelegate.Callback(this.OnFinished), true);
                    }
                    index++;
                }
            }
        }

        public override void Reset()
        {
            this.tweenTarget = null;
            this.tweenGroup = null;
            this.resetOnPlay = 0;
            this.resetIfDisabled = 0;
            this.ifDisabledOnPlay = EnableCondition.DoNothing;
            this.disableWhenFinished = DisableCondition.DoNotDisable;
            this.includeChildren = 0;
            this.tweeningFinishedEvent = null;
        }
    }
}

