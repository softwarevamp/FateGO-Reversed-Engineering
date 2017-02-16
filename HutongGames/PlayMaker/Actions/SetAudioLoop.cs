namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Audio), HutongGames.PlayMaker.Tooltip("Sets looping on the AudioSource component on a Game Object.")]
    public class SetAudioLoop : ComponentAction<AudioSource>
    {
        [CheckForComponent(typeof(AudioSource)), RequiredField]
        public FsmOwnerDefault gameObject;
        public FsmBool loop;

        public override void OnEnter()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (base.UpdateCache(ownerDefaultTarget))
            {
                base.audio.loop = this.loop.Value;
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.loop = 0;
        }
    }
}

