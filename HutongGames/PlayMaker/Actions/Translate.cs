namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Transform), HutongGames.PlayMaker.Tooltip("Translates a Game Object. Use a Vector3 variable and/or XYZ components. To leave any axis unchanged, set variable to 'None'.")]
    public class Translate : FsmStateAction
    {
        [HutongGames.PlayMaker.Tooltip("Repeat every frame.")]
        public bool everyFrame;
        [HutongGames.PlayMaker.Tooltip("Perform the translate in FixedUpdate. This is useful when working with rigid bodies and physics.")]
        public bool fixedUpdate;
        [HutongGames.PlayMaker.Tooltip("The game object to translate."), RequiredField]
        public FsmOwnerDefault gameObject;
        [HutongGames.PlayMaker.Tooltip("Perform the translate in LateUpdate. This is useful if you want to override the position of objects that are animated or otherwise positioned in Update.")]
        public bool lateUpdate;
        [HutongGames.PlayMaker.Tooltip("Translate over one second")]
        public bool perSecond;
        [HutongGames.PlayMaker.Tooltip("Translate in local or world space.")]
        public Space space;
        [HutongGames.PlayMaker.Tooltip("A translation vector. NOTE: You can override individual axis below."), UIHint(UIHint.Variable)]
        public FsmVector3 vector;
        [HutongGames.PlayMaker.Tooltip("Translation along x axis.")]
        public FsmFloat x;
        [HutongGames.PlayMaker.Tooltip("Translation along y axis.")]
        public FsmFloat y;
        [HutongGames.PlayMaker.Tooltip("Translation along z axis.")]
        public FsmFloat z;

        public override void Awake()
        {
            base.Fsm.HandleFixedUpdate = true;
        }

        private void DoTranslate()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (ownerDefaultTarget != null)
            {
                Vector3 translation = !this.vector.IsNone ? this.vector.Value : new Vector3(this.x.Value, this.y.Value, this.z.Value);
                if (!this.x.IsNone)
                {
                    translation.x = this.x.Value;
                }
                if (!this.y.IsNone)
                {
                    translation.y = this.y.Value;
                }
                if (!this.z.IsNone)
                {
                    translation.z = this.z.Value;
                }
                if (!this.perSecond)
                {
                    ownerDefaultTarget.transform.Translate(translation, this.space);
                }
                else
                {
                    ownerDefaultTarget.transform.Translate((Vector3) (translation * Time.deltaTime), this.space);
                }
            }
        }

        public override void OnEnter()
        {
            if ((!this.everyFrame && !this.lateUpdate) && !this.fixedUpdate)
            {
                this.DoTranslate();
                base.Finish();
            }
        }

        public override void OnFixedUpdate()
        {
            if (this.fixedUpdate)
            {
                this.DoTranslate();
            }
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnLateUpdate()
        {
            if (this.lateUpdate)
            {
                this.DoTranslate();
            }
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            if (!this.lateUpdate && !this.fixedUpdate)
            {
                this.DoTranslate();
            }
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.vector = null;
            FsmFloat num = new FsmFloat {
                UseVariable = true
            };
            this.x = num;
            num = new FsmFloat {
                UseVariable = true
            };
            this.y = num;
            num = new FsmFloat {
                UseVariable = true
            };
            this.z = num;
            this.space = Space.Self;
            this.perSecond = true;
            this.everyFrame = true;
            this.lateUpdate = false;
            this.fixedUpdate = false;
        }
    }
}

