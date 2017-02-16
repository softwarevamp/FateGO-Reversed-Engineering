namespace WellFired
{
    using System;
    using UnityEngine;

    [USequencerEventHideDuration, USequencerEvent("FGO/Camera/Toggle Camera Effect"), USequencerFriendlyName("FGO Toggle Camera Effect")]
    public class USFGOCameraEffectEvent : USEventBase
    {
        public CameraEffectType effectType;
        public float effectValue = 0.1f;
        public bool isEnabled;
        public bool oldEnabled;

        public override void FireEvent()
        {
            if (base.AffectedObject != null)
            {
                this.setComponentEnabled(this.isEnabled, true);
            }
        }

        public override void ProcessEvent(float runningTime)
        {
        }

        private void setComponentEnabled(bool flg, bool saveOld)
        {
            if (this.effectType == CameraEffectType.Grayscale)
            {
                Camera effectCamera = SingletonMonoBehaviour<FGOSequenceManager>.Instance.effectCamera;
                if (effectCamera != null)
                {
                    switch (this.effectType)
                    {
                        case CameraEffectType.Bloom:
                        {
                            Bloom component = effectCamera.GetComponent<Bloom>();
                            if (component != null)
                            {
                                if (saveOld)
                                {
                                    this.oldEnabled = component.enabled;
                                }
                                component.enabled = this.isEnabled;
                                if (this.isEnabled)
                                {
                                    component.bloomIntensity = this.effectValue;
                                }
                            }
                            break;
                        }
                        case CameraEffectType.MotionBlur:
                        {
                            MotionBlur blur = effectCamera.GetComponent<MotionBlur>();
                            if (blur != null)
                            {
                                if (saveOld)
                                {
                                    this.oldEnabled = blur.enabled;
                                }
                                blur.enabled = this.isEnabled;
                            }
                            if (this.isEnabled)
                            {
                                blur.blurAmount = this.effectValue;
                            }
                            break;
                        }
                        case CameraEffectType.Vignet:
                        {
                            Vignetting vignetting = effectCamera.GetComponent<Vignetting>();
                            if (vignetting != null)
                            {
                                if (saveOld)
                                {
                                    this.oldEnabled = vignetting.enabled;
                                }
                                vignetting.enabled = this.isEnabled;
                                if (this.isEnabled)
                                {
                                    vignetting.intensity = this.effectValue;
                                }
                            }
                            break;
                        }
                        case CameraEffectType.Grayscale:
                        {
                            GrayscaleEffect effect = effectCamera.GetComponent<GrayscaleEffect>();
                            if (effect != null)
                            {
                                if (saveOld)
                                {
                                    this.oldEnabled = effect.enabled;
                                }
                                effect.enabled = this.isEnabled;
                                effect.saturation = this.effectValue;
                                GrayscaleEffect effect2 = SingletonMonoBehaviour<FGOSequenceManager>.Instance.cutInCamera.GetComponent<GrayscaleEffect>();
                                if (effect2 != null)
                                {
                                    effect2.enabled = this.isEnabled;
                                    effect2.saturation = this.effectValue;
                                }
                            }
                            break;
                        }
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
            this.setComponentEnabled(this.oldEnabled, false);
        }

        public enum CameraEffectType
        {
            Bloom,
            MotionBlur,
            Vignet,
            Grayscale
        }
    }
}

