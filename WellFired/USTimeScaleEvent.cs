namespace WellFired
{
    using System;
    using UnityEngine;

    [USequencerEvent("Time/Time Scale"), USequencerFriendlyName("Time Scale"), USequencerEventHideDuration]
    public class USTimeScaleEvent : USEventBase
    {
        private float currentCurveSampleTime;
        private float prevTimeScale;
        public AnimationCurve scaleCurve;

        public USTimeScaleEvent()
        {
            Keyframe[] keys = new Keyframe[] { new Keyframe(0f, 1f), new Keyframe(0.1f, 0.2f), new Keyframe(2f, 1f) };
            this.scaleCurve = new AnimationCurve(keys);
            this.prevTimeScale = 1f;
        }

        public override void EndEvent()
        {
            float time = this.scaleCurve.keys[this.scaleCurve.length - 1].time;
            Time.timeScale = Mathf.Max(0f, this.scaleCurve.Evaluate(time));
        }

        public override void FireEvent()
        {
            this.prevTimeScale = Time.timeScale;
        }

        public override void ProcessEvent(float deltaTime)
        {
            this.currentCurveSampleTime = deltaTime;
            Time.timeScale = Mathf.Max(0f, this.scaleCurve.Evaluate(this.currentCurveSampleTime));
        }

        public override void StopEvent()
        {
            this.UndoEvent();
        }

        public override void UndoEvent()
        {
            this.currentCurveSampleTime = 0f;
            Time.timeScale = this.prevTimeScale;
        }
    }
}

