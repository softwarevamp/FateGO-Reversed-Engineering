namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Sets the Audio Clip played by the AudioSource component on a Game Object."), ActionCategory(ActionCategory.Audio)]
    public class SetAudioClip : ComponentAction<AudioSource>
    {
        [ObjectType(typeof(AudioClip)), HutongGames.PlayMaker.Tooltip("The AudioClip to set.")]
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

