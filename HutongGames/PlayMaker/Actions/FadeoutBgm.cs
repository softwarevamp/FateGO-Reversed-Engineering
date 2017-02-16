namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory(ActionCategory.Audio), Tooltip("Fadeout the Game System BGM data.")]
    public class FadeoutBgm : FsmStateAction
    {
        [Tooltip("Set the fadeout time."), HasFloatSlider(0f, 60f)]
        public FsmFloat fadeTime = 0f;

        public override void OnEnter()
        {
            SoundManager.fadeoutBgm(this.fadeTime.Value);
            base.Finish();
        }

        public override void Reset()
        {
        }
    }
}

