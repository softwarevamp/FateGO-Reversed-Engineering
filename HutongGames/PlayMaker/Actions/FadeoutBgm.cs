namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("Fadeout the Game System BGM data."), ActionCategory(ActionCategory.Audio)]
    public class FadeoutBgm : FsmStateAction
    {
        [HasFloatSlider(0f, 60f), Tooltip("Set the fadeout time.")]
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

