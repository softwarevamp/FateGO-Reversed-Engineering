namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Set a named color property in a Substance material. NOTE: Use Rebuild Textures after setting Substance properties."), ActionCategory("Substance")]
    public class SetProceduralColor : FsmStateAction
    {
        [RequiredField]
        public FsmString colorProperty;
        [RequiredField]
        public FsmColor colorValue;
        [HutongGames.PlayMaker.Tooltip("NOTE: Updating procedural materials every frame can be very slow!")]
        public bool everyFrame;
        [RequiredField]
        public FsmMaterial substanceMaterial;

        private void DoSetProceduralFloat()
        {
        }

        public override void OnEnter()
        {
            this.DoSetProceduralFloat();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoSetProceduralFloat();
        }

        public override void Reset()
        {
            this.substanceMaterial = null;
            this.colorProperty = string.Empty;
            this.colorValue = Color.white;
            this.everyFrame = false;
        }
    }
}

