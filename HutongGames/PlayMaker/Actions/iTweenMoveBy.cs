namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using System.Collections;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Adds the supplied vector to a GameObject's position."), ActionCategory("iTween")]
    public class iTweenMoveBy : iTweenFsmAction
    {
        [HutongGames.PlayMaker.Tooltip("Restricts rotation to the supplied axis only. Just put there strinc like 'x' or 'xz'")]
        public iTweenFsmAction.AxisRestriction axis;
        [HutongGames.PlayMaker.Tooltip("For the time in seconds the animation will wait before beginning.")]
        public FsmFloat delay;
        [HutongGames.PlayMaker.Tooltip("For the shape of the easing curve applied to the animation.")]
        public iTween.EaseType easeType = iTween.EaseType.linear;
        [RequiredField]
        public FsmOwnerDefault gameObject;
        [HutongGames.PlayMaker.Tooltip("iTween ID. If set you can use iTween Stop action to stop it by its id.")]
        public FsmString id;
        [HutongGames.PlayMaker.Tooltip("For a target the GameObject will look at.")]
        public FsmGameObject lookAtObject;
        [HutongGames.PlayMaker.Tooltip("For a target the GameObject will look at.")]
        public FsmVector3 lookAtVector;
        [HutongGames.PlayMaker.Tooltip("For the time in seconds the object will take to look at either the 'looktarget' or 'orienttopath'. 0 by default")]
        public FsmFloat lookTime;
        [HutongGames.PlayMaker.Tooltip("For the type of loop to apply once the animation has completed.")]
        public iTween.LoopType loopType;
        [ActionSection("LookAt"), HutongGames.PlayMaker.Tooltip("For whether or not the GameObject will orient to its direction of travel. False by default.")]
        public FsmBool orientToPath;
        public Space space;
        [HutongGames.PlayMaker.Tooltip("Can be used instead of time to allow animation based on speed. When you define speed the time variable is ignored.")]
        public FsmFloat speed;
        [HutongGames.PlayMaker.Tooltip("For the time in seconds the animation will take to complete.")]
        public FsmFloat time;
        [HutongGames.PlayMaker.Tooltip("The vector to add to the GameObject's position."), RequiredField]
        public FsmVector3 vector;

        private void DoiTween()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (ownerDefaultTarget != null)
            {
                Hashtable args = new Hashtable {
                    { 
                        "amount",
                        !this.vector.IsNone ? this.vector.Value : Vector3.zero
                    },
                    { 
                        !this.speed.IsNone ? "speed" : "time",
                        !this.speed.IsNone ? this.speed.Value : (!this.time.IsNone ? this.time.Value : 1f)
                    },
                    { 
                        "delay",
                        !this.delay.IsNone ? this.delay.Value : 0f
                    },
                    { 
                        "easetype",
                        this.easeType
                    },
                    { 
                        "looptype",
                        this.loopType
                    },
                    { 
                        "oncomplete",
                        "iTweenOnComplete"
                    },
                    { 
                        "oncompleteparams",
                        base.itweenID
                    },
                    { 
                        "onstart",
                        "iTweenOnStart"
                    },
                    { 
                        "onstartparams",
                        base.itweenID
                    },
                    { 
                        "ignoretimescale",
                        !base.realTime.IsNone ? ((object) base.realTime.Value) : ((object) 0)
                    },
                    { 
                        "space",
                        this.space
                    },
                    { 
                        "name",
                        !this.id.IsNone ? this.id.Value : string.Empty
                    },
                    { 
                        "axis",
                        (this.axis != iTweenFsmAction.AxisRestriction.none) ? Enum.GetName(typeof(iTweenFsmAction.AxisRestriction), this.axis) : string.Empty
                    }
                };
                if (!this.orientToPath.IsNone)
                {
                    args.Add("orienttopath", this.orientToPath.Value);
                }
                if (!this.lookAtObject.IsNone)
                {
                    args.Add("looktarget", !this.lookAtVector.IsNone ? (this.lookAtObject.Value.transform.position + this.lookAtVector.Value) : this.lookAtObject.Value.transform.position);
                }
                else if (!this.lookAtVector.IsNone)
                {
                    args.Add("looktarget", this.lookAtVector.Value);
                }
                if (!this.lookAtObject.IsNone || !this.lookAtVector.IsNone)
                {
                    args.Add("looktime", !this.lookTime.IsNone ? this.lookTime.Value : 0f);
                }
                base.itweenType = "move";
                iTween.MoveBy(ownerDefaultTarget, args);
            }
        }

        public override void OnEnter()
        {
            base.OnEnteriTween(this.gameObject);
            if (this.loopType != iTween.LoopType.none)
            {
                base.IsLoop(true);
            }
            this.DoiTween();
        }

        public override void OnExit()
        {
            base.OnExitiTween(this.gameObject);
        }

        public override void Reset()
        {
            base.Reset();
            FsmString str = new FsmString {
                UseVariable = true
            };
            this.id = str;
            this.time = 1f;
            this.delay = 0f;
            this.loopType = iTween.LoopType.none;
            FsmVector3 vector = new FsmVector3 {
                UseVariable = true
            };
            this.vector = vector;
            FsmFloat num = new FsmFloat {
                UseVariable = true
            };
            this.speed = num;
            this.space = Space.World;
            this.orientToPath = 0;
            FsmGameObject obj2 = new FsmGameObject {
                UseVariable = true
            };
            this.lookAtObject = obj2;
            vector = new FsmVector3 {
                UseVariable = true
            };
            this.lookAtVector = vector;
            this.lookTime = 0f;
            this.axis = iTweenFsmAction.AxisRestriction.none;
        }
    }
}

