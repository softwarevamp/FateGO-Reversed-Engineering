namespace WellFired
{
    using System;
    using UnityEngine;

    [USequencerEvent("FGO/Ambient Fade"), USequencerFriendlyName("FGO Ambient Fade"), USequencerEventHideDuration]
    public class USFGOAmbientFadeEvent : USEventBase
    {
        private float currentCurveSampleTime;
        public Color fadeColorFrom;
        public Color fadeColorTo;
        public AnimationCurve fadeCurve;
        public UILayer uiLayer;

        public USFGOAmbientFadeEvent()
        {
            Keyframe[] keys = new Keyframe[] { new Keyframe(0f, 0f), new Keyframe(1f, 1f), new Keyframe(3f, 1f), new Keyframe(4f, 0f) };
            this.fadeCurve = new AnimationCurve(keys);
            this.fadeColorFrom = Color.white;
            this.fadeColorTo = Color.black;
        }

        public override void EndEvent()
        {
            float b = this.fadeCurve.Evaluate(this.fadeCurve.keys[this.fadeCurve.length - 1].time);
            b = Mathf.Min(Mathf.Max(0f, b), 1f);
            float r = this.fadeColorFrom.r + ((this.fadeColorTo.r - this.fadeColorFrom.r) * b);
            float g = this.fadeColorFrom.g + ((this.fadeColorTo.g - this.fadeColorFrom.g) * b);
            float num4 = this.fadeColorFrom.b + ((this.fadeColorTo.b - this.fadeColorFrom.b) * b);
            Color color = new Color(r, g, num4, 1f);
            RenderSettings.ambientLight = color;
        }

        public override void FireEvent()
        {
        }

        private void OnEnable()
        {
        }

        public override void ProcessEvent(float deltaTime)
        {
            this.currentCurveSampleTime = deltaTime;
            float b = this.fadeCurve.Evaluate(this.currentCurveSampleTime);
            b = Mathf.Min(Mathf.Max(0f, b), 1f);
            float r = this.fadeColorFrom.r + ((this.fadeColorTo.r - this.fadeColorFrom.r) * b);
            float g = this.fadeColorFrom.g + ((this.fadeColorTo.g - this.fadeColorFrom.g) * b);
            float num4 = this.fadeColorFrom.b + ((this.fadeColorTo.b - this.fadeColorFrom.b) * b);
            Color color = new Color(r, g, num4, 1f);
            float time = 0f;
            foreach (Keyframe keyframe in this.fadeCurve.keys)
            {
                if (keyframe.time > time)
                {
                    time = keyframe.time;
                }
            }
            base.Duration = time;
            RenderSettings.ambientLight = color;
        }

        public override void StopEvent()
        {
            this.UndoEvent();
        }

        public override void UndoEvent()
        {
            this.currentCurveSampleTime = 0f;
        }

        private void Update()
        {
            float time = 0f;
            foreach (Keyframe keyframe in this.fadeCurve.keys)
            {
                if (keyframe.time > time)
                {
                    time = keyframe.time;
                }
            }
            base.Duration = time;
        }
    }
}

