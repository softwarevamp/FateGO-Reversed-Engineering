namespace WellFired
{
    using System;
    using UnityEngine;

    [USequencerEvent("Fullscreen/Display Image"), USequencerEventHideDuration, USequencerFriendlyName("Display Image")]
    public class USDisplayImageEvent : USEventBase
    {
        public UIPosition anchorPosition;
        private float currentCurveSampleTime;
        public Texture2D displayImage;
        public UIPosition displayPosition;
        public AnimationCurve fadeCurve;
        public UILayer uiLayer;

        public USDisplayImageEvent()
        {
            Keyframe[] keys = new Keyframe[] { new Keyframe(0f, 0f), new Keyframe(1f, 1f), new Keyframe(3f, 1f), new Keyframe(4f, 0f) };
            this.fadeCurve = new AnimationCurve(keys);
        }

        public override void EndEvent()
        {
            float b = this.fadeCurve.Evaluate(this.fadeCurve.keys[this.fadeCurve.length - 1].time);
            b = Mathf.Min(Mathf.Max(0f, b), 1f);
        }

        public override void FireEvent()
        {
            if (this.displayImage == null)
            {
                Debug.LogWarning("Trying to use a DisplayImage Event, but you didn't give it an image to display", this);
            }
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
                float b = this.fadeCurve.Evaluate(this.currentCurveSampleTime);
                b = Mathf.Min(Mathf.Max(0f, b), 1f);
                if (this.displayImage != null)
                {
                    Rect position = new Rect(Screen.width * 0.5f, Screen.height * 0.5f, (float) this.displayImage.width, (float) this.displayImage.height);
                    switch (this.displayPosition)
                    {
                        case UIPosition.TopLeft:
                            position.x = 0f;
                            position.y = 0f;
                            break;

                        case UIPosition.TopRight:
                            position.x = Screen.width;
                            position.y = 0f;
                            break;

                        case UIPosition.BottomLeft:
                            position.x = 0f;
                            position.y = Screen.height;
                            break;

                        case UIPosition.BottomRight:
                            position.x = Screen.width;
                            position.y = Screen.height;
                            break;
                    }
                    switch (this.anchorPosition)
                    {
                        case UIPosition.Center:
                            position.x -= this.displayImage.width * 0.5f;
                            position.y -= this.displayImage.height * 0.5f;
                            break;

                        case UIPosition.TopRight:
                            position.x -= this.displayImage.width;
                            break;

                        case UIPosition.BottomLeft:
                            position.y -= this.displayImage.height;
                            break;

                        case UIPosition.BottomRight:
                            position.x -= this.displayImage.width;
                            position.y -= this.displayImage.height;
                            break;
                    }
                    GUI.depth = (int) this.uiLayer;
                    Color color = GUI.color;
                    GUI.color = new Color(1f, 1f, 1f, b);
                    GUI.DrawTexture(position, this.displayImage);
                    GUI.color = color;
                }
            }
        }

        public override void ProcessEvent(float deltaTime)
        {
            this.currentCurveSampleTime = deltaTime;
        }

        public override void StopEvent()
        {
            this.UndoEvent();
        }

        public override void UndoEvent()
        {
            this.currentCurveSampleTime = 0f;
        }
    }
}

