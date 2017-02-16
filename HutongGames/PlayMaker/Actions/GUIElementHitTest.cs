namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.GUIElement), HutongGames.PlayMaker.Tooltip("Performs a Hit Test on a Game Object with a GUITexture or GUIText component.")]
    public class GUIElementHitTest : FsmStateAction
    {
        [HutongGames.PlayMaker.Tooltip("Specify camera or use MainCamera as default.")]
        public Camera camera;
        [HutongGames.PlayMaker.Tooltip("Repeat every frame. Useful if you want to wait for the hit test to return true.")]
        public FsmBool everyFrame;
        [HutongGames.PlayMaker.Tooltip("The GameObject that has a GUITexture or GUIText component."), RequiredField, CheckForComponent(typeof(GUIElement))]
        public FsmOwnerDefault gameObject;
        private GameObject gameObjectCached;
        private GUIElement guiElement;
        [HutongGames.PlayMaker.Tooltip("Event to send if the Hit Test is true.")]
        public FsmEvent hitEvent;
        [HutongGames.PlayMaker.Tooltip("Whether the specified screen coordinates are normalized (0-1).")]
        public FsmBool normalized;
        [HutongGames.PlayMaker.Tooltip("A vector position on screen. Usually stored by actions like GetTouchInfo, or World To Screen Point.")]
        public FsmVector3 screenPoint;
        [HutongGames.PlayMaker.Tooltip("Specify screen X coordinate.")]
        public FsmFloat screenX;
        [HutongGames.PlayMaker.Tooltip("Specify screen Y coordinate.")]
        public FsmFloat screenY;
        [HutongGames.PlayMaker.Tooltip("Store the result of the Hit Test in a bool variable (true/false)."), UIHint(UIHint.Variable)]
        public FsmBool storeResult;

        private void DoHitTest()
        {
            GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
            if (ownerDefaultTarget != null)
            {
                if (ownerDefaultTarget != this.gameObjectCached)
                {
                    GUITexture component = ownerDefaultTarget.GetComponent<GUITexture>();
                    if (component == null)
                    {
                    }
                    this.guiElement = ownerDefaultTarget.GetComponent<GUIText>();
                    this.gameObjectCached = ownerDefaultTarget;
                }
                if (this.guiElement == null)
                {
                    base.Finish();
                }
                else
                {
                    Vector3 screenPosition = !this.screenPoint.IsNone ? this.screenPoint.Value : new Vector3(0f, 0f);
                    if (!this.screenX.IsNone)
                    {
                        screenPosition.x = this.screenX.Value;
                    }
                    if (!this.screenY.IsNone)
                    {
                        screenPosition.y = this.screenY.Value;
                    }
                    if (this.normalized.Value)
                    {
                        screenPosition.x *= Screen.width;
                        screenPosition.y *= Screen.height;
                    }
                    if (this.guiElement.HitTest(screenPosition, this.camera))
                    {
                        this.storeResult.Value = true;
                        base.Fsm.Event(this.hitEvent);
                    }
                    else
                    {
                        this.storeResult.Value = false;
                    }
                }
            }
        }

        public override void OnEnter()
        {
            this.DoHitTest();
            if (!this.everyFrame.Value)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoHitTest();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.camera = null;
            FsmVector3 vector = new FsmVector3 {
                UseVariable = true
            };
            this.screenPoint = vector;
            FsmFloat num = new FsmFloat {
                UseVariable = true
            };
            this.screenX = num;
            num = new FsmFloat {
                UseVariable = true
            };
            this.screenY = num;
            this.normalized = 1;
            this.hitEvent = null;
            this.everyFrame = 1;
        }
    }
}

