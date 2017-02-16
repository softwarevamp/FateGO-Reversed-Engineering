namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Mute/unmute the Audio Clip played by an Audio Source component on a Game Object."), ActionCategory(ActionCategory.Audio)]
    public class AudioMute : FsmStateAction
    {
        [HutongGames.PlayMaker.Tooltip("The GameObject with an Audio Source component."), RequiredField, CheckForComponent(typeof(AudioSource))]
        public FsmOwnerDefault gameObject;
        [RequiredField, HutongGames.PlayMaker.Tooltip("Check to mute, uncheck to unmute.")]
        public FsmBool mute;

        public override void OnEnter()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (ownerDefaultTarget != null)
            {
                AudioSource component = ownerDefaultTarget.GetComponent<AudioSource>();
                if (component != null)
                {
                    component.mute = this.mute.Value;
                }
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.mute = 0;
        }
    }
}

