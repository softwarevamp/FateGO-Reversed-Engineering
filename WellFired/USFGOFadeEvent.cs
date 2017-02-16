namespace WellFired
{
    using System;
    using UnityEngine;

    [USequencerEvent("FGO/Fade Screen"), USequencerEventHideDuration, USequencerFriendlyName("FGO Fade Screen")]
    public class USFGOFadeEvent : USEventBase
    {
        private float currentCurveSampleTime;
        public Color fadeColour;
        public AnimationCurve fadeCurve;
        public GameObject faderObject;
        public UILayer uiLayer;

        public USFGOFadeEvent()
        {
            Keyframe[] keys = new Keyframe[] { new Keyframe(0f, 0f), new Keyframe(1f, 1f), new Keyframe(3f, 1f), new Keyframe(4f, 0f) };
            this.fadeCurve = new AnimationCurve(keys);
            this.fadeColour = Color.black;
        }

        public override void EndEvent()
        {
            float b = this.fadeCurve.Evaluate(this.fadeCurve.keys[this.fadeCurve.length - 1].time);
            b = Mathf.Min(Mathf.Max(0f, b), 1f);
            Color col = new Color(this.fadeColour.r, this.fadeColour.g, this.fadeColour.b, b);
            this.faderObject.GetComponent<NGUIFader>().setColor(col);
        }

        public override void FireEvent()
        {
            if (base.AffectedObject == null)
            {
                Debug.Log("Can not found FGOSequenceManager in USFGOFadeEvent.FireEvent");
            }
            else if (this.faderObject == null)
            {
                this.faderObject = base.AffectedObject.GetComponent<FGOSequenceManager>().faderObject;
            }
        }

        private void OnEnable()
        {
            if (this.faderObject != null)
            {
                Color col = new Color(this.fadeColour.r, this.fadeColour.g, this.fadeColour.b, 0f);
                this.faderObject.GetComponent<NGUIFader>().setColor(col);
            }
        }

        public override void ProcessEvent(float deltaTime)
        {
            this.currentCurveSampleTime = deltaTime;
            float b = this.fadeCurve.Evaluate(this.currentCurveSampleTime);
            b = Mathf.Min(Mathf.Max(0f, b), 1f);
            NGUIFader component = this.faderObject.GetComponent<NGUIFader>();
            Color col = new Color(this.fadeColour.r, this.fadeColour.g, this.fadeColour.b, b);
            float time = 0f;
            foreach (Keyframe keyframe in this.fadeCurve.keys)
            {
                if (keyframe.time > time)
                {
                    time = keyframe.time;
                }
            }
            base.Duration = time;
            component.setColor(col);
        }

        public override void StopEvent()
        {
            this.UndoEvent();
        }

        public override void UndoEvent()
        {
            this.currentCurveSampleTime = 0f;
            Color col = new Color(this.fadeColour.r, this.fadeColour.g, this.fadeColour.b, 0f);
            this.faderObject.GetComponent<NGUIFader>().setColor(col);
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

