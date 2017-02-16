namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Plays the Game System BGM data."), ActionCategory(ActionCategory.Audio)]
    public class PlayBgm : FsmStateAction
    {
        [HutongGames.PlayMaker.Tooltip("Set the bgm name string."), ObjectType(typeof(AudioClip))]
        public FsmString bgmName;
        [HasFloatSlider(0f, 60f), HutongGames.PlayMaker.Tooltip("Set the fadein time.")]
        public FsmFloat fadeTime = 0f;
        [HasFloatSlider(0f, 1f), HutongGames.PlayMaker.Tooltip("Set the volume.")]
        public FsmFloat volume = 1f;

        public override void OnEnter()
        {
            SoundManager.playBgm(this.bgmName.Value, this.volume.Value, this.fadeTime.Value);
            base.Finish();
        }

        public override void Reset()
        {
        }
    }
}

