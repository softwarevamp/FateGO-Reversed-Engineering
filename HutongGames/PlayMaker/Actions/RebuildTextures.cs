namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory("Substance"), Tooltip("Rebuilds all dirty textures. By default the rebuild is spread over multiple frames so it won't halt the game. Check Immediately to rebuild all textures in a single frame.")]
    public class RebuildTextures : FsmStateAction
    {
        public bool everyFrame;
        [RequiredField]
        public FsmBool immediately;
        [RequiredField]
        public FsmMaterial substanceMaterial;

        private void DoRebuildTextures()
        {
        }

        public override void OnEnter()
        {
            this.DoRebuildTextures();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoRebuildTextures();
        }

        public override void Reset()
        {
            this.substanceMaterial = null;
            this.immediately = 0;
            this.everyFrame = false;
        }
    }
}

