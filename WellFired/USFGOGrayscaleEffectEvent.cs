namespace WellFired
{
    using System;
    using UnityEngine;

    [USequencerFriendlyName("FGO Grayscale Effect"), USequencerEvent("FGO/Effect/Grayscale Effect"), USequencerEventHideDuration]
    public class USFGOGrayscaleEffectEvent : USEventBase
    {
        private float currentCurveSampleTime;
        public AnimationCurve fadeCurve;

        public USFGOGrayscaleEffectEvent()
        {
            Keyframe[] keys = new Keyframe[] { new Keyframe(0f, 0f), new Keyframe(1f, 1f) };
            this.fadeCurve = new AnimationCurve(keys);
        }

        public override void EndEvent()
        {
            float b = this.fadeCurve.Evaluate(this.fadeCurve.keys[this.fadeCurve.length - 1].time);
            b = Mathf.Min(Mathf.Max(0f, b), 1f);
            this.SetSaturation(b);
        }

        public override void FireEvent()
        {
        }

        public override void ProcessEvent(float deltaTime)
        {
            this.currentCurveSampleTime = deltaTime;
            float b = this.fadeCurve.Evaluate(this.currentCurveSampleTime);
            b = Mathf.Min(Mathf.Max(0f, b), 1f);
            this.SetSaturation(b);
        }

        protected void SetSaturation(float saturation)
        {
            Camera effectCamera = SingletonMonoBehaviour<FGOSequenceManager>.Instance.effectCamera;
            if (effectCamera != null)
            {
                GrayscaleEffect component = effectCamera.GetComponent<GrayscaleEffect>();
                if (component != null)
                {
                    component.enabled = true;
                    component.saturation = saturation;
                    GrayscaleEffect effect2 = SingletonMonoBehaviour<FGOSequenceManager>.Instance.cutInCamera.GetComponent<GrayscaleEffect>();
                    if (effect2 != null)
                    {
                        effect2.enabled = true;
                        effect2.saturation = saturation;
                    }
                }
            }
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

