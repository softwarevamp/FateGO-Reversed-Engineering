namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Sets the Volume of the Audio Clip played by the AudioSource component on a Game Object."), ActionCategory(ActionCategory.Audio)]
    public class SetAudioVolume : ComponentAction<AudioSource>
    {
        public bool everyFrame;
        [CheckForComponent(typeof(AudioSource)), RequiredField]
        public FsmOwnerDefault gameObject;
        [HasFloatSlider(0f, 1f)]
        public FsmFloat volume;

        private void DoSetAudioVolume()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (base.UpdateCache(ownerDefaultTarget) && !this.volume.IsNone)
            {
                base.audio.volume = this.volume.Value;
            }
        }

        public override void OnEnter()
        {
            this.DoSetAudioVolume();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoSetAudioVolume();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.volume = 1f;
            this.everyFrame = false;
        }
    }
}

