namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Audio), HutongGames.PlayMaker.Tooltip("Stops playing the Audio Clip played by an Audio Source component on a Game Object.")]
    public class AudioStop : FsmStateAction
    {
        [CheckForComponent(typeof(AudioSource)), HutongGames.PlayMaker.Tooltip("The GameObject with an AudioSource component."), RequiredField]
        public FsmOwnerDefault gameObject;

        public override void OnEnter()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (ownerDefaultTarget != null)
            {
                AudioSource component = ownerDefaultTarget.GetComponent<AudioSource>();
                if (component != null)
                {
                    component.Stop();
                }
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
        }
    }
}

