namespace WellFired
{
    using System;
    using UnityEngine;

    [USequencerFriendlyName("Fade Screen"), USequencerEvent("Fullscreen/Fade Screen"), USequencerEventHideDuration]
    public class USFadeScreenEvent : USEventBase
    {
        private float currentCurveSampleTime;
        public Color fadeColour;
        public AnimationCurve fadeCurve;
        public static Texture2D texture;
        public UILayer uiLayer;

        public USFadeScreenEvent()
        {
            Keyframe[] keys = new Keyframe[] { new Keyframe(0f, 0f), new Keyframe(1f, 1f), new Keyframe(3f, 1f), new Keyframe(4f, 0f) };
            this.fadeCurve = new AnimationCurve(keys);
            this.fadeColour = Color.black;
        }

        public override void EndEvent()
        {
            if (texture == null)
            {
                texture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            }
            float b = this.fadeCurve.Evaluate(this.fadeCurve.keys[this.fadeCurve.length - 1].time);
            b = Mathf.Min(Mathf.Max(0f, b), 1f);
            texture.SetPixel(0, 0, new Color(this.fadeColour.r, this.fadeColour.g, this.fadeColour.b, b));
            texture.Apply();
        }

        public override void FireEvent()
        {
        }

        private void OnEnable()
        {
            if (texture == null)
            {
                texture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            }
            texture.SetPixel(0, 0, new Color(this.fadeColour.r, this.fadeColour.g, this.fadeColour.b, 0f));
            texture.Apply();
        }

        private void OnGUI()
        {
            if (base.Sequence.IsPlaying)
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
                if (texture != null)
                {
                    int depth = GUI.depth;
                    GUI.depth = (int) this.uiLayer;
                    GUI.DrawTexture(new Rect(0f, 0f, (float) Screen.width, (float) Screen.height), texture);
                    GUI.depth = depth;
                }
            }
        }

        public override void ProcessEvent(float deltaTime)
        {
            this.currentCurveSampleTime = deltaTime;
            if (texture == null)
            {
                texture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            }
            float b = this.fadeCurve.Evaluate(this.currentCurveSampleTime);
            b = Mathf.Min(Mathf.Max(0f, b), 1f);
            texture.SetPixel(0, 0, new Color(this.fadeColour.r, this.fadeColour.g, this.fadeColour.b, b));
            texture.Apply();
        }

        public override void StopEvent()
        {
            this.UndoEvent();
        }

        public override void UndoEvent()
        {
            this.currentCurveSampleTime = 0f;
            if (texture == null)
            {
                texture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            }
            texture.SetPixel(0, 0, new Color(this.fadeColour.r, this.fadeColour.g, this.fadeColour.b, 0f));
            texture.Apply();
        }
    }
}

