namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Plays the Game System SE data."), ActionCategory(ActionCategory.Audio)]
    public class PlaySystemSe : FsmStateAction
    {
        [HutongGames.PlayMaker.Tooltip("Set the system se kind string."), ObjectType(typeof(AudioClip))]
        public SeManager.SystemSeKind kind;
        [HasFloatSlider(0f, 1f), HutongGames.PlayMaker.Tooltip("Set the volume.")]
        public FsmFloat volume = 1f;

        public override void OnEnter()
        {
            SoundManager.playSystemSe(this.kind);
            base.Finish();
        }

        public override void Reset()
        {
        }
    }
}

