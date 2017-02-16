namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Audio), HutongGames.PlayMaker.Tooltip("Sets the Audio Clip played by the AudioSource component on a Game Object.")]
    public class SetAudioClip : ComponentAction<AudioSource>
    {
        [HutongGames.PlayMaker.Tooltip("The AudioClip to set."), ObjectType(typeof(AudioClip))]
        public FsmObject audioClip;
        [CheckForComponent(typeof(AudioSource)), HutongGames.PlayMaker.Tooltip("The GameObject with the AudioSource component."), RequiredField]
        public FsmOwnerDefault gameObject;

        public override void OnEnter()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (base.UpdateCache(ownerDefaultTarget))
            {
                base.audio.clip = this.audioClip.Value as AudioClip;
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.audioClip = null;
        }
    }
}

