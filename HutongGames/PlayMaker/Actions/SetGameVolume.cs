namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Sets the global sound volume."), ActionCategory(ActionCategory.Audio)]
    public class SetGameVolume : FsmStateAction
    {
        public bool everyFrame;
        [RequiredField, HasFloatSlider(0f, 1f)]
        public FsmFloat volume;

        public override void OnEnter()
        {
            AudioListener.volume = this.volume.Value;
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            AudioListener.volume = this.volume.Value;
        }

        public override void Reset()
        {
            this.volume = 1f;
            this.everyFrame = false;
        }
    }
}

