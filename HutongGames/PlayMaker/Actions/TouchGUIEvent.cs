namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Device), HutongGames.PlayMaker.Tooltip("Sends events when a GUI Texture or GUI Text is touched. Optionally filter by a fingerID.")]
    public class TouchGUIEvent : FsmStateAction
    {
        [HutongGames.PlayMaker.Tooltip("Repeate every frame."), ActionSection("")]
        public bool everyFrame;
        [HutongGames.PlayMaker.Tooltip("Only detect touches that match this fingerID, or set to None.")]
        public FsmInt fingerId;
        [RequiredField, CheckForComponent(typeof(GUIElement)), HutongGames.PlayMaker.Tooltip("The Game Object that owns the GUI Texture or GUI Text.")]
        public FsmOwnerDefault gameObject;
        private GUIElement guiElement;
        [HutongGames.PlayMaker.Tooltip("Normalize the hit point screen coordinates (0-1).")]
        public FsmBool normalizeHitPoint;
        [HutongGames.PlayMaker.Tooltip("Normalize the offset.")]
        public FsmBool normalizeOffset;
        [HutongGames.PlayMaker.Tooltip("Event to send if not touching (finger down but not over the GUI element)")]
        public FsmEvent notTouching;
        [HutongGames.PlayMaker.Tooltip("How to measure the offset.")]
        public OffsetOptions relativeTo;
        [ActionSection("Store Results"), UIHint(UIHint.Variable), HutongGames.PlayMaker.Tooltip("Store the fingerId of the touch.")]
        public FsmInt storeFingerId;
        [UIHint(UIHint.Variable), HutongGames.PlayMaker.Tooltip("Store the screen position where the GUI element was touched.")]
        public FsmVector3 storeHitPoint;
        [HutongGames.PlayMaker.Tooltip("Store the offset position of the hit."), UIHint(UIHint.Variable)]
        public FsmVector3 storeOffset;
        [ActionSection("Events"), HutongGames.PlayMaker.Tooltip("Event to send on touch began.")]
        public FsmEvent touchBegan;
        [HutongGames.PlayMaker.Tooltip("Event to send on touch cancel.")]
        public FsmEvent touchCanceled;
        [HutongGames.PlayMaker.Tooltip("Event to send on touch ended.")]
        public FsmEvent touchEnded;
        [HutongGames.PlayMaker.Tooltip("Event to send on touch moved.")]
        public FsmEvent touchMoved;
        private Vector3 touchStartPos;
        [HutongGames.PlayMaker.Tooltip("Event to send on stationary touch.")]
        public FsmEvent touchStationary;

        private void DoTouch(Touch touch)
        {
            if (this.fingerId.IsNone || (touch.fingerId == this.fingerId.Value))
            {
                Vector3 position = (Vector3) touch.position;
                if (this.guiElement.HitTest(position))
                {
                    if (touch.phase == TouchPhase.Began)
                    {
                        this.touchStartPos = position;
                    }
                    this.storeFingerId.Value = touch.fingerId;
                    if (this.normalizeHitPoint.Value)
                    {
                        position.x /= (float) Screen.width;
                        position.y /= (float) Screen.height;
                    }
                    this.storeHitPoint.Value = position;
                    this.DoTouchOffset(position);
                    switch (touch.phase)
                    {
                        case TouchPhase.Began:
                            base.Fsm.Event(this.touchBegan);
                            return;

                        case TouchPhase.Moved:
                            base.Fsm.Event(this.touchMoved);
                            return;

                        case TouchPhase.Stationary:
                            base.Fsm.Event(this.touchStationary);
                            return;

                        case TouchPhase.Ended:
                            base.Fsm.Event(this.touchEnded);
                            return;

                        case TouchPhase.Canceled:
                            base.Fsm.Event(this.touchCanceled);
                            return;
                    }
                }
                else
                {
                    base.Fsm.Event(this.notTouching);
                }
            }
        }

        private void DoTouchGUIEvent()
        {
            if (Input.touchCount > 0)
            {
                GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
                if (ownerDefaultTarget != null)
                {
                    GUITexture component = ownerDefaultTarget.GetComponent<GUITexture>();
                    if (component == null)
                    {
                    }
                    this.guiElement = ownerDefaultTarget.GetComponent<GUIText>();
                    if (this.guiElement != null)
                    {
                        foreach (Touch touch in Input.touches)
                        {
                            this.DoTouch(touch);
                        }
                    }
                }
            }
        }

        private void DoTouchOffset(Vector3 touchPos)
        {
            if (!this.storeOffset.IsNone)
            {
                Rect screenRect = this.guiElement.GetScreenRect();
                Vector3 vector = new Vector3();
                switch (this.relativeTo)
                {
                    case OffsetOptions.TopLeft:
                        vector.x = touchPos.x - screenRect.x;
                        vector.y = touchPos.y - screenRect.y;
                        break;

                    case OffsetOptions.Center:
                    {
                        Vector3 vector2 = new Vector3(screenRect.x + (screenRect.width * 0.5f), screenRect.y + (screenRect.height * 0.5f), 0f);
                        vector = touchPos - vector2;
                        break;
                    }
                    case OffsetOptions.TouchStart:
                        vector = touchPos - this.touchStartPos;
                        break;
                }
                if (this.normalizeOffset.Value)
                {
                    vector.x /= screenRect.width;
                    vector.y /= screenRect.height;
                }
                this.storeOffset.Value = vector;
            }
        }

        public override void OnEnter()
        {
            this.DoTouchGUIEvent();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoTouchGUIEvent();
        }

        public override void Reset()
        {
            this.gameObject = null;
            FsmInt num = new FsmInt {
                UseVariable = true
            };
            this.fingerId = num;
            this.touchBegan = null;
            this.touchMoved = null;
            this.touchStationary = null;
            this.touchEnded = null;
            this.touchCanceled = null;
            this.storeFingerId = null;
            this.storeHitPoint = null;
            this.normalizeHitPoint = 0;
            this.storeOffset = null;
            this.relativeTo = OffsetOptions.Center;
            this.normalizeOffset = 1;
            this.everyFrame = true;
        }

        public enum OffsetOptions
        {
            TopLeft,
            Center,
            TouchStart
        }
    }
}

