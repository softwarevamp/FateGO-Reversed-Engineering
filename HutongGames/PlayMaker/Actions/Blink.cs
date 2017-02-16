namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Effects), HutongGames.PlayMaker.Tooltip("Turns a Game Object on/off in a regular repeating pattern.")]
    public class Blink : ComponentAction<Renderer>
    {
        private bool blinkOn;
        [RequiredField, HutongGames.PlayMaker.Tooltip("The GameObject to blink on/off.")]
        public FsmOwnerDefault gameObject;
        [HutongGames.PlayMaker.Tooltip("Ignore TimeScale. Useful if the game is paused.")]
        public bool realTime;
        [HutongGames.PlayMaker.Tooltip("Only effect the renderer, keeping other components active.")]
        public bool rendererOnly;
        [HutongGames.PlayMaker.Tooltip("Should the object start in the active/visible state?")]
        public FsmBool startOn;
        private float startTime;
        [HutongGames.PlayMaker.Tooltip("Time to stay off in seconds."), HasFloatSlider(0f, 5f)]
        public FsmFloat timeOff;
        [HutongGames.PlayMaker.Tooltip("Time to stay on in seconds."), HasFloatSlider(0f, 5f)]
        public FsmFloat timeOn;
        private float timer;

        public override void OnEnter()
        {
            this.startTime = FsmTime.RealtimeSinceStartup;
            this.timer = 0f;
            this.UpdateBlinkState(this.startOn.Value);
        }

        public override void OnUpdate()
        {
            if (this.realTime)
            {
                this.timer = FsmTime.RealtimeSinceStartup - this.startTime;
            }
            else
            {
                this.timer += Time.deltaTime;
            }
            if (this.blinkOn && (this.timer > this.timeOn.Value))
            {
                this.UpdateBlinkState(false);
            }
            if (!this.blinkOn && (this.timer > this.timeOff.Value))
            {
                this.UpdateBlinkState(true);
            }
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.timeOff = 0.5f;
            this.timeOn = 0.5f;
            this.rendererOnly = true;
            this.startOn = 0;
            this.realTime = false;
        }

        private void UpdateBlinkState(bool state)
        {
            GameObject go = (this.gameObject.OwnerOption != OwnerDefaultOption.UseOwner) ? this.gameObject.GameObject.Value : base.Owner;
            if (go != null)
            {
                if (this.rendererOnly)
                {
                    if (base.UpdateCache(go))
                    {
                        base.renderer.enabled = state;
                    }
                }
                else
                {
                    go.SetActive(state);
                }
                this.blinkOn = state;
                this.startTime = FsmTime.RealtimeSinceStartup;
                this.timer = 0f;
            }
        }
    }
}

