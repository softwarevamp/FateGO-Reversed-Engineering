namespace WellFired
{
    using System;
    using UnityEngine;

    [USequencerEvent("FGO/Set Animation Camera"), USequencerFriendlyName("FGO Set Animation Camera")]
    public class USFGOAnimationCameraEvent : USEventBase
    {
        private AnimationClip animationClip;
        public GameObject animCamLoc;
        public Camera camera;
        private Transform cameraAim;
        private Transform cameraRoot;
        public string cutNumName = "treasureArms1_a";
        public bool isOpponent;
        private Transform originalParent;
        public float playbackSpeed = 1f;
        public WrapMode wrapMode;

        public override void EndEvent()
        {
            this.StopEvent();
        }

        public override void FireEvent()
        {
            if (this.animCamLoc == null)
            {
                Debug.Log("animCamLoc is null from USFGOAnimationCameraEvent::FireEvent");
            }
            else
            {
                Animation component = this.animCamLoc.GetComponent<Animation>();
                if (component == null)
                {
                    Debug.Log("animation component not found from USFGOAnimationCamera.FireEvent");
                }
                string cutNumName = this.cutNumName;
                if (component[cutNumName] == null)
                {
                    Debug.Log("Animation Clip '" + cutNumName + "' not found from USFGOAnimationCamera.FireEvent");
                }
                else
                {
                    this.animationClip = component[cutNumName].clip;
                    if (this.camera == null)
                    {
                        Debug.Log("Animation Camera not found from USFGOAnimationCamera.FireEvent");
                    }
                    else
                    {
                        this.cameraRoot = this.animCamLoc.transform.FindChild("animCam_root");
                        this.cameraAim = this.animCamLoc.transform.FindChild("animCam_aim");
                        if (this.cameraAim == null)
                        {
                            this.cameraAim = this.cameraRoot.transform.FindChild("animCam_aim");
                        }
                        this.originalParent = this.camera.transform.parent;
                        this.camera.transform.parent = this.cameraRoot.transform;
                        this.animCamLoc.transform.localRotation = Quaternion.Euler(0f, 270f, 0f);
                        this.camera.transform.localPosition = Vector3.zero;
                        this.camera.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
                        this.camera.transform.localScale = Vector3.one;
                        base.Duration = this.animationClip.length;
                        component.wrapMode = this.wrapMode;
                        component.Play(this.animationClip.name);
                        AnimationState state = component[this.animationClip.name];
                        if (state != null)
                        {
                            state.speed = this.playbackSpeed;
                        }
                    }
                }
            }
        }

        public override void ProcessEvent(float deltaTime)
        {
            Animation component = this.animCamLoc.GetComponent<Animation>();
            if (this.animationClip != null)
            {
                if (component == null)
                {
                    Debug.LogError(string.Concat(new object[] { "Trying to play an animation : ", this.animationClip.name, " but : ", this.animCamLoc, " doesn't have an animation component, we will add one, this time, though you should add it manually" }));
                    component = this.animCamLoc.AddComponent<Animation>();
                }
                if (component[this.animationClip.name] == null)
                {
                    Debug.LogError("Trying to play an animation : " + this.animationClip.name + " but it isn't in the animation list. I will add it, this time, though you should add it manually.");
                    component.AddClip(this.animationClip, this.animationClip.name);
                }
                AnimationState state = component[this.animationClip.name];
                if (!component.IsPlaying(this.animationClip.name))
                {
                    component.wrapMode = this.wrapMode;
                    component.Play(this.animationClip.name);
                }
                state.speed = this.playbackSpeed;
                state.time = deltaTime * this.playbackSpeed;
                state.enabled = true;
                component.Sample();
                state.enabled = false;
                Vector3 position = this.camera.transform.position;
                Vector3 forward = this.cameraAim.position - position;
                Quaternion quaternion = Quaternion.LookRotation(forward);
                this.camera.transform.rotation = quaternion;
            }
        }

        public override void StopEvent()
        {
            if (this.animCamLoc != null)
            {
                BattleFBXComponent component = this.animCamLoc.GetComponent<BattleFBXComponent>();
                if (component != null)
                {
                    component.stopAnimation();
                }
                this.UndoEvent();
            }
        }

        public override void UndoEvent()
        {
            if (this.camera != null)
            {
                this.camera.transform.parent = this.originalParent;
            }
        }

        public void Update()
        {
            if (this.animationClip != null)
            {
                base.Duration = this.animationClip.length / this.playbackSpeed;
            }
        }
    }
}

