using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

[AddComponentMenu("NGUI/UI/NGUI Event System (UICamera)"), RequireComponent(typeof(Camera)), ExecuteInEditMode]
public class UICamera : MonoBehaviour
{
    [CompilerGenerated]
    private static BetterList<DepthEntry>.CompareFunc <>f__am$cache50;
    [CompilerGenerated]
    private static BetterList<DepthEntry>.CompareFunc <>f__am$cache51;
    public bool allowMultiTouch = true;
    public KeyCode cancelKey0 = KeyCode.Escape;
    public KeyCode cancelKey1 = KeyCode.JoystickButton1;
    public bool commandClick = true;
    public static MouseOrTouch controller = new MouseOrTouch();
    public static UICamera current = null;
    public static Camera currentCamera = null;
    public static KeyCode currentKey = KeyCode.None;
    public static ControlScheme currentScheme = ControlScheme.Mouse;
    public static MouseOrTouch currentTouch = null;
    public static int currentTouchID = -100;
    public bool debug;
    public LayerMask eventReceiverMask = -1;
    public bool eventsGoToColliders;
    public EventType eventType = EventType.UI_3D;
    public static GameObject fallThrough;
    public static GetAxisFunc GetAxis = new GetAxisFunc(Input.GetAxis);
    public static GetTouchCallback GetInputTouch;
    public static GetTouchCountCallback GetInputTouchCount;
    public static GetKeyStateFunc GetKey = new GetKeyStateFunc(Input.GetKey);
    public static GetKeyStateFunc GetKeyDown = new GetKeyStateFunc(Input.GetKeyDown);
    public static GetKeyStateFunc GetKeyUp = new GetKeyStateFunc(Input.GetKeyUp);
    public string horizontalAxisName = "Horizontal";
    public static GameObject hoveredObject;
    public static bool inputHasFocus = false;
    public static bool isDragging = false;
    public static RaycastHit lastHit;
    public static Vector2 lastTouchPosition = Vector2.zero;
    public static Vector3 lastWorldPosition = Vector3.zero;
    public static BetterList<UICamera> list = new BetterList<UICamera>();
    private static Plane m2DPlane = new Plane(Vector3.back, 0f);
    private Camera mCam;
    private static GameObject mCurrentSelection = null;
    private static GameObject mGenericHandler;
    private static int mHeight = 0;
    private static DepthEntry mHit = new DepthEntry();
    private static BetterList<DepthEntry> mHits = new BetterList<DepthEntry>();
    private static GameObject mHover;
    private static MouseOrTouch[] mMouse = new MouseOrTouch[] { new MouseOrTouch(), new MouseOrTouch(), new MouseOrTouch() };
    private static float mNextEvent = 0f;
    private float mNextRaycast;
    private static int mNotifying = 0;
    public float mouseClickThreshold = 10f;
    public float mouseDragThreshold = 4f;
    private GameObject mTooltip;
    private float mTooltipTime;
    private static Dictionary<int, MouseOrTouch> mTouches = new Dictionary<int, MouseOrTouch>();
    private static bool mUsingTouchEvents = true;
    private static int mWidth = 0;
    public static VoidDelegate onClick;
    public static OnCustomInput onCustomInput;
    public static VoidDelegate onDoubleClick;
    public static VectorDelegate onDrag;
    public static VoidDelegate onDragEnd;
    public static ObjectDelegate onDragOut;
    public static ObjectDelegate onDragOver;
    public static VoidDelegate onDragStart;
    public static ObjectDelegate onDrop;
    public static BoolDelegate onHover;
    public static KeyCodeDelegate onKey;
    public static MoveDelegate onMouseMove;
    public static BoolDelegate onPress;
    public static OnScreenResize onScreenResize;
    public static FloatDelegate onScroll;
    public static BoolDelegate onSelect;
    public static BoolDelegate onTooltip;
    public float rangeDistance = -1f;
    public string scrollAxisName = "Mouse ScrollWheel";
    public static bool showTooltips = true;
    public bool stickyTooltip = true;
    public KeyCode submitKey0 = KeyCode.Return;
    public KeyCode submitKey1 = KeyCode.JoystickButton0;
    public float tooltipDelay = 1f;
    public float touchClickThreshold = 40f;
    public float touchDragThreshold = 40f;
    public bool useController = true;
    public bool useKeyboard = true;
    public bool useMouse = true;
    public bool useTouch = true;
    public string verticalAxisName = "Vertical";

    private void Awake()
    {
        mWidth = Screen.width;
        mHeight = Screen.height;
        if (((Application.platform == RuntimePlatform.Android) || (Application.platform == RuntimePlatform.IPhonePlayer)) || ((Application.platform == RuntimePlatform.WP8Player) || (Application.platform == RuntimePlatform.BlackBerryPlayer)))
        {
            this.useTouch = true;
            this.useMouse = false;
            this.useKeyboard = false;
            this.useController = false;
        }
        else if ((Application.platform == RuntimePlatform.PS3) || (Application.platform == RuntimePlatform.XBOX360))
        {
            this.useMouse = false;
            this.useTouch = false;
            this.useKeyboard = false;
            this.useController = true;
        }
        mMouse[0].pos = Input.mousePosition;
        for (int i = 1; i < 3; i++)
        {
            mMouse[i].pos = mMouse[0].pos;
            mMouse[i].lastPos = mMouse[0].pos;
        }
        lastTouchPosition = mMouse[0].pos;
    }

    private static int CompareFunc(UICamera a, UICamera b)
    {
        if (a.cachedCamera.depth < b.cachedCamera.depth)
        {
            return 1;
        }
        if (a.cachedCamera.depth > b.cachedCamera.depth)
        {
            return -1;
        }
        return 0;
    }

    public static UICamera FindCameraForLayer(int layer)
    {
        int num = ((int) 1) << layer;
        for (int i = 0; i < list.size; i++)
        {
            UICamera camera = list.buffer[i];
            Camera cachedCamera = camera.cachedCamera;
            if ((cachedCamera != null) && ((cachedCamera.cullingMask & num) != 0))
            {
                return camera;
            }
        }
        return null;
    }

    private static Rigidbody FindRootRigidbody(Transform trans)
    {
        while (trans != null)
        {
            if (trans.GetComponent<UIPanel>() != null)
            {
                return null;
            }
            Rigidbody component = trans.GetComponent<Rigidbody>();
            if (component != null)
            {
                return component;
            }
            trans = trans.parent;
        }
        return null;
    }

    private static Rigidbody2D FindRootRigidbody2D(Transform trans)
    {
        while (trans != null)
        {
            if (trans.GetComponent<UIPanel>() != null)
            {
                return null;
            }
            Rigidbody2D component = trans.GetComponent<Rigidbody2D>();
            if (component != null)
            {
                return component;
            }
            trans = trans.parent;
        }
        return null;
    }

    private static int GetDirection(string axis)
    {
        float time = RealTime.time;
        if ((mNextEvent < time) && !string.IsNullOrEmpty(axis))
        {
            float num2 = GetAxis(axis);
            if (num2 > 0.75f)
            {
                mNextEvent = time + 0.25f;
                return 1;
            }
            if (num2 < -0.75f)
            {
                mNextEvent = time + 0.25f;
                return -1;
            }
        }
        return 0;
    }

    private static int GetDirection(KeyCode up, KeyCode down)
    {
        if (GetKeyDown(up))
        {
            return 1;
        }
        if (GetKeyDown(down))
        {
            return -1;
        }
        return 0;
    }

    private static int GetDirection(KeyCode up0, KeyCode up1, KeyCode down0, KeyCode down1)
    {
        if (GetKeyDown(up0) || GetKeyDown(up1))
        {
            return 1;
        }
        if (!GetKeyDown(down0) && !GetKeyDown(down1))
        {
            return 0;
        }
        return -1;
    }

    public static MouseOrTouch GetMouse(int button) => 
        mMouse[button];

    public static MouseOrTouch GetTouch(int id)
    {
        MouseOrTouch touch = null;
        if (id < 0)
        {
            return GetMouse(-id - 1);
        }
        if (!mTouches.TryGetValue(id, out touch))
        {
            touch = new MouseOrTouch {
                pressTime = RealTime.time,
                touchBegan = true
            };
            mTouches.Add(id, touch);
        }
        return touch;
    }

    public static bool IsHighlighted(GameObject go)
    {
        if (currentScheme == ControlScheme.Mouse)
        {
            return (hoveredObject == go);
        }
        return ((currentScheme == ControlScheme.Controller) && (selectedObject == go));
    }

    public static bool IsPressed(GameObject go)
    {
        for (int i = 0; i < 3; i++)
        {
            if (mMouse[i].pressed == go)
            {
                return true;
            }
        }
        foreach (KeyValuePair<int, MouseOrTouch> pair in mTouches)
        {
            if (pair.Value.pressed == go)
            {
                return true;
            }
        }
        return (controller.pressed == go);
    }

    private static bool IsVisible(ref DepthEntry de)
    {
        for (UIPanel panel = NGUITools.FindInParents<UIPanel>(de.go); panel != null; panel = panel.parentPanel)
        {
            if (!panel.IsVisible(de.point))
            {
                return false;
            }
        }
        return true;
    }

    private static bool IsVisible(Vector3 worldPoint, GameObject go)
    {
        for (UIPanel panel = NGUITools.FindInParents<UIPanel>(go); panel != null; panel = panel.parentPanel)
        {
            if (!panel.IsVisible(worldPoint))
            {
                return false;
            }
        }
        return true;
    }

    private void LateUpdate()
    {
        if (this.handlesEvents)
        {
            int width = Screen.width;
            int height = Screen.height;
            if ((width != mWidth) || (height != mHeight))
            {
                mWidth = width;
                mHeight = height;
                UIRoot.Broadcast("UpdateAnchors");
                if (onScreenResize != null)
                {
                    onScreenResize();
                }
            }
        }
    }

    public static void Notify(GameObject go, string funcName, object obj)
    {
        if ((mNotifying <= 10) && NGUITools.GetActive(go))
        {
            mNotifying++;
            go.SendMessage(funcName, obj, SendMessageOptions.DontRequireReceiver);
            if ((mGenericHandler != null) && (mGenericHandler != go))
            {
                mGenericHandler.SendMessage(funcName, obj, SendMessageOptions.DontRequireReceiver);
            }
            mNotifying--;
        }
    }

    private void OnApplicationPause()
    {
        MouseOrTouch currentTouch = UICamera.currentTouch;
        if (this.useTouch)
        {
            BetterList<int> list = new BetterList<int>();
            foreach (KeyValuePair<int, MouseOrTouch> pair in mTouches)
            {
                if ((pair.Value != null) && (pair.Value.pressed != null))
                {
                    UICamera.currentTouch = pair.Value;
                    currentTouchID = pair.Key;
                    currentScheme = ControlScheme.Touch;
                    UICamera.currentTouch.clickNotification = ClickNotification.None;
                    this.ProcessTouch(false, true);
                    list.Add(currentTouchID);
                }
            }
            for (int i = 0; i < list.size; i++)
            {
                RemoveTouch(list[i]);
            }
        }
        if (this.useMouse)
        {
            for (int j = 0; j < 3; j++)
            {
                if (mMouse[j].pressed != null)
                {
                    UICamera.currentTouch = mMouse[j];
                    currentTouchID = -1 - j;
                    currentKey = (KeyCode) (0x143 + j);
                    currentScheme = ControlScheme.Mouse;
                    UICamera.currentTouch.clickNotification = ClickNotification.None;
                    this.ProcessTouch(false, true);
                }
            }
        }
        if (this.useController && (controller.pressed != null))
        {
            UICamera.currentTouch = controller;
            currentTouchID = -100;
            currentScheme = ControlScheme.Controller;
            UICamera.currentTouch.last = UICamera.currentTouch.current;
            UICamera.currentTouch.current = mCurrentSelection;
            UICamera.currentTouch.clickNotification = ClickNotification.None;
            this.ProcessTouch(false, true);
            UICamera.currentTouch.last = null;
        }
        UICamera.currentTouch = currentTouch;
    }

    private void OnDisable()
    {
        list.Remove(this);
    }

    private void OnEnable()
    {
        list.Add(this);
        list.Sort(new BetterList<UICamera>.CompareFunc(UICamera.CompareFunc));
    }

    private void ProcessFakeTouches()
    {
        bool mouseButtonDown = Input.GetMouseButtonDown(0);
        bool mouseButtonUp = Input.GetMouseButtonUp(0);
        bool mouseButton = Input.GetMouseButton(0);
        if ((mouseButtonDown || mouseButtonUp) || mouseButton)
        {
            currentTouchID = 1;
            currentTouch = mMouse[0];
            currentTouch.touchBegan = mouseButtonDown;
            if (mouseButtonDown)
            {
                currentTouch.pressTime = RealTime.time;
            }
            Vector2 mousePosition = Input.mousePosition;
            currentTouch.delta = !mouseButtonDown ? (mousePosition - currentTouch.pos) : Vector2.zero;
            currentTouch.pos = mousePosition;
            if (!Raycast((Vector3) currentTouch.pos))
            {
                hoveredObject = fallThrough;
            }
            if (hoveredObject == null)
            {
                hoveredObject = mGenericHandler;
            }
            currentTouch.last = currentTouch.current;
            currentTouch.current = hoveredObject;
            lastTouchPosition = currentTouch.pos;
            if (mouseButtonDown)
            {
                currentTouch.pressedCam = currentCamera;
            }
            else if (currentTouch.pressed != null)
            {
                currentCamera = currentTouch.pressedCam;
            }
            this.ProcessTouch(mouseButtonDown, mouseButtonUp);
            if (mouseButtonUp)
            {
                RemoveTouch(currentTouchID);
            }
            currentTouch.last = null;
            currentTouch = null;
        }
    }

    public void ProcessMouse()
    {
        lastTouchPosition = Input.mousePosition;
        mMouse[0].delta = lastTouchPosition - mMouse[0].pos;
        mMouse[0].pos = lastTouchPosition;
        bool flag = mMouse[0].delta.sqrMagnitude > 0.001f;
        for (int i = 1; i < 3; i++)
        {
            mMouse[i].pos = mMouse[0].pos;
            mMouse[i].delta = mMouse[0].delta;
        }
        bool flag2 = false;
        bool flag3 = false;
        for (int j = 0; j < 3; j++)
        {
            if (Input.GetMouseButtonDown(j))
            {
                currentScheme = ControlScheme.Mouse;
                flag3 = true;
                flag2 = true;
            }
            else if (Input.GetMouseButton(j))
            {
                currentScheme = ControlScheme.Mouse;
                flag2 = true;
            }
        }
        if ((flag2 || flag) || (this.mNextRaycast < RealTime.time))
        {
            this.mNextRaycast = RealTime.time + 0.02f;
            if (!Raycast(Input.mousePosition))
            {
                hoveredObject = fallThrough;
            }
            if (hoveredObject == null)
            {
                hoveredObject = mGenericHandler;
            }
            for (int n = 0; n < 3; n++)
            {
                mMouse[n].current = hoveredObject;
            }
        }
        bool flag4 = mMouse[0].last != mMouse[0].current;
        if (flag4)
        {
            currentScheme = ControlScheme.Mouse;
        }
        currentTouch = mMouse[0];
        currentTouchID = -1;
        if (flag2)
        {
            this.mTooltipTime = 0f;
        }
        else if (flag && (!this.stickyTooltip || flag4))
        {
            if (this.mTooltipTime != 0f)
            {
                this.mTooltipTime = RealTime.time + this.tooltipDelay;
            }
            else if (this.mTooltip != null)
            {
                this.ShowTooltip(false);
            }
        }
        if (flag && (onMouseMove != null))
        {
            onMouseMove(currentTouch.delta);
            currentTouch = null;
        }
        if ((flag3 || !flag2) && ((mHover != null) && flag4))
        {
            currentScheme = ControlScheme.Mouse;
            if (this.mTooltip != null)
            {
                this.ShowTooltip(false);
            }
            if (onHover != null)
            {
                onHover(mHover, false);
            }
            Notify(mHover, "OnHover", false);
            mHover = null;
        }
        for (int k = 0; k < 3; k++)
        {
            bool mouseButtonDown = Input.GetMouseButtonDown(k);
            bool mouseButtonUp = Input.GetMouseButtonUp(k);
            if (mouseButtonDown || mouseButtonUp)
            {
                currentScheme = ControlScheme.Mouse;
            }
            currentTouch = mMouse[k];
            currentTouchID = -1 - k;
            currentKey = (KeyCode) (0x143 + k);
            if (mouseButtonDown)
            {
                currentTouch.pressedCam = currentCamera;
            }
            else if (currentTouch.pressed != null)
            {
                currentCamera = currentTouch.pressedCam;
            }
            this.ProcessTouch(mouseButtonDown, mouseButtonUp);
            currentKey = KeyCode.None;
        }
        if (!flag2 && flag4)
        {
            currentScheme = ControlScheme.Mouse;
            this.mTooltipTime = RealTime.time + this.tooltipDelay;
            mHover = mMouse[0].current;
            currentTouch = mMouse[0];
            currentTouchID = -1;
            if (onHover != null)
            {
                onHover(mHover, true);
            }
            Notify(mHover, "OnHover", true);
        }
        currentTouch = null;
        mMouse[0].last = mMouse[0].current;
        for (int m = 1; m < 3; m++)
        {
            mMouse[m].last = mMouse[0].last;
        }
    }

    public void ProcessOthers()
    {
        currentTouchID = -100;
        currentTouch = controller;
        bool pressed = false;
        bool released = false;
        if ((this.submitKey0 != KeyCode.None) && GetKeyDown(this.submitKey0))
        {
            currentKey = this.submitKey0;
            pressed = true;
        }
        if ((this.submitKey1 != KeyCode.None) && GetKeyDown(this.submitKey1))
        {
            currentKey = this.submitKey1;
            pressed = true;
        }
        if ((this.submitKey0 != KeyCode.None) && GetKeyUp(this.submitKey0))
        {
            currentKey = this.submitKey0;
            released = true;
        }
        if ((this.submitKey1 != KeyCode.None) && GetKeyUp(this.submitKey1))
        {
            currentKey = this.submitKey1;
            released = true;
        }
        if (pressed || released)
        {
            currentScheme = ControlScheme.Controller;
            currentTouch.last = currentTouch.current;
            currentTouch.current = mCurrentSelection;
            this.ProcessTouch(pressed, released);
            currentTouch.last = null;
        }
        int num = 0;
        int num2 = 0;
        if (this.useKeyboard)
        {
            if (inputHasFocus)
            {
                num += GetDirection(KeyCode.UpArrow, KeyCode.DownArrow);
                num2 += GetDirection(KeyCode.RightArrow, KeyCode.LeftArrow);
            }
            else
            {
                num += GetDirection(KeyCode.W, KeyCode.UpArrow, KeyCode.S, KeyCode.DownArrow);
                num2 += GetDirection(KeyCode.D, KeyCode.RightArrow, KeyCode.A, KeyCode.LeftArrow);
            }
        }
        if (this.useController)
        {
            if (!string.IsNullOrEmpty(this.verticalAxisName))
            {
                num += GetDirection(this.verticalAxisName);
            }
            if (!string.IsNullOrEmpty(this.horizontalAxisName))
            {
                num2 += GetDirection(this.horizontalAxisName);
            }
        }
        if (num != 0)
        {
            currentScheme = ControlScheme.Controller;
            KeyCode key = (num <= 0) ? KeyCode.DownArrow : KeyCode.UpArrow;
            if (onKey != null)
            {
                onKey(mCurrentSelection, key);
            }
            Notify(mCurrentSelection, "OnKey", key);
        }
        if (num2 != 0)
        {
            currentScheme = ControlScheme.Controller;
            KeyCode code2 = (num2 <= 0) ? KeyCode.LeftArrow : KeyCode.RightArrow;
            if (onKey != null)
            {
                onKey(mCurrentSelection, code2);
            }
            Notify(mCurrentSelection, "OnKey", code2);
        }
        if (this.useKeyboard && GetKeyDown(KeyCode.Tab))
        {
            currentKey = KeyCode.Tab;
            currentScheme = ControlScheme.Controller;
            if (onKey != null)
            {
                onKey(mCurrentSelection, KeyCode.Tab);
            }
            Notify(mCurrentSelection, "OnKey", KeyCode.Tab);
        }
        if ((this.cancelKey0 != KeyCode.None) && GetKeyDown(this.cancelKey0))
        {
            currentKey = this.cancelKey0;
            currentScheme = ControlScheme.Controller;
            if (onKey != null)
            {
                onKey(mCurrentSelection, KeyCode.Escape);
            }
            Notify(mCurrentSelection, "OnKey", KeyCode.Escape);
        }
        if ((this.cancelKey1 != KeyCode.None) && GetKeyDown(this.cancelKey1))
        {
            currentKey = this.cancelKey1;
            currentScheme = ControlScheme.Controller;
            if (onKey != null)
            {
                onKey(mCurrentSelection, KeyCode.Escape);
            }
            Notify(mCurrentSelection, "OnKey", KeyCode.Escape);
        }
        currentTouch = null;
        currentKey = KeyCode.None;
    }

    private void ProcessPress(bool pressed, float click, float drag)
    {
        if (pressed)
        {
            if (this.mTooltip != null)
            {
                this.ShowTooltip(false);
            }
            currentTouch.pressStarted = true;
            if ((onPress != null) && (currentTouch.pressed != null))
            {
                onPress(currentTouch.pressed, false);
            }
            Notify(currentTouch.pressed, "OnPress", false);
            currentTouch.pressed = currentTouch.current;
            currentTouch.dragged = currentTouch.current;
            currentTouch.clickNotification = ClickNotification.BasedOnDelta;
            currentTouch.totalDelta = Vector2.zero;
            currentTouch.dragStarted = false;
            if ((onPress != null) && (currentTouch.pressed != null))
            {
                onPress(currentTouch.pressed, true);
            }
            TouchEffectManager.Press(currentTouch.pos);
            Notify(currentTouch.pressed, "OnPress", true);
            if (this.mTooltip != null)
            {
                this.ShowTooltip(false);
            }
            selectedObject = currentTouch.pressed;
        }
        else if ((currentTouch.pressed != null) && ((currentTouch.delta.sqrMagnitude != 0f) || (currentTouch.current != currentTouch.last)))
        {
            currentTouch.totalDelta += currentTouch.delta;
            float sqrMagnitude = currentTouch.totalDelta.sqrMagnitude;
            bool flag = false;
            if (!currentTouch.dragStarted && (currentTouch.last != currentTouch.current))
            {
                currentTouch.dragStarted = true;
                currentTouch.delta = currentTouch.totalDelta;
                isDragging = true;
                if (onDragStart != null)
                {
                    onDragStart(currentTouch.dragged);
                }
                Notify(currentTouch.dragged, "OnDragStart", null);
                if (onDragOver != null)
                {
                    onDragOver(currentTouch.last, currentTouch.dragged);
                }
                Notify(currentTouch.last, "OnDragOver", currentTouch.dragged);
                isDragging = false;
            }
            else if (!currentTouch.dragStarted && (drag < sqrMagnitude))
            {
                flag = true;
                currentTouch.dragStarted = true;
                currentTouch.delta = currentTouch.totalDelta;
            }
            if (currentTouch.dragStarted)
            {
                if (this.mTooltip != null)
                {
                    this.ShowTooltip(false);
                }
                isDragging = true;
                bool flag2 = currentTouch.clickNotification == ClickNotification.None;
                if (currentTouch != null)
                {
                    TouchEffectManager.Drag(currentTouch.delta);
                }
                if (flag)
                {
                    if (onDragStart != null)
                    {
                        onDragStart(currentTouch.dragged);
                    }
                    Notify(currentTouch.dragged, "OnDragStart", null);
                    if (onDragOver != null)
                    {
                        onDragOver(currentTouch.last, currentTouch.dragged);
                    }
                    Notify(currentTouch.current, "OnDragOver", currentTouch.dragged);
                }
                else if (currentTouch.last != currentTouch.current)
                {
                    if (onDragStart != null)
                    {
                        onDragStart(currentTouch.dragged);
                    }
                    Notify(currentTouch.last, "OnDragOut", currentTouch.dragged);
                    if (onDragOver != null)
                    {
                        onDragOver(currentTouch.last, currentTouch.dragged);
                    }
                    Notify(currentTouch.current, "OnDragOver", currentTouch.dragged);
                }
                if (onDrag != null)
                {
                    onDrag(currentTouch.dragged, currentTouch.delta);
                }
                Notify(currentTouch.dragged, "OnDrag", currentTouch.delta);
                currentTouch.last = currentTouch.current;
                isDragging = false;
                if (flag2)
                {
                    currentTouch.clickNotification = ClickNotification.None;
                }
                else if ((currentTouch.clickNotification == ClickNotification.BasedOnDelta) && (click < sqrMagnitude))
                {
                    currentTouch.clickNotification = ClickNotification.None;
                }
            }
        }
    }

    private void ProcessRelease(bool isMouse, float drag)
    {
        if (currentTouch != null)
        {
            TouchEffectManager.UnPress();
            currentTouch.pressStarted = false;
            if (this.mTooltip != null)
            {
                this.ShowTooltip(false);
            }
            if (currentTouch.pressed != null)
            {
                if (currentTouch.dragStarted)
                {
                    if (onDragOut != null)
                    {
                        onDragOut(currentTouch.last, currentTouch.dragged);
                    }
                    Notify(currentTouch.last, "OnDragOut", currentTouch.dragged);
                    if (onDragEnd != null)
                    {
                        onDragEnd(currentTouch.dragged);
                    }
                    Notify(currentTouch.dragged, "OnDragEnd", null);
                }
                if (onPress != null)
                {
                    onPress(currentTouch.pressed, false);
                }
                Notify(currentTouch.pressed, "OnPress", false);
                if (isMouse)
                {
                    if (onHover != null)
                    {
                        onHover(currentTouch.current, true);
                    }
                    Notify(currentTouch.current, "OnHover", true);
                }
                mHover = currentTouch.current;
                if ((currentTouch.dragged == currentTouch.current) || (((currentScheme != ControlScheme.Controller) && (currentTouch.clickNotification != ClickNotification.None)) && (currentTouch.totalDelta.sqrMagnitude < drag)))
                {
                    if ((currentTouch.clickNotification != ClickNotification.None) && (currentTouch.pressed == currentTouch.current))
                    {
                        float time = RealTime.time;
                        if (onClick != null)
                        {
                            onClick(currentTouch.pressed);
                        }
                        Notify(currentTouch.pressed, "OnClick", null);
                        if ((currentTouch.clickTime + 0.35f) > time)
                        {
                            if (onDoubleClick != null)
                            {
                                onDoubleClick(currentTouch.pressed);
                            }
                            Notify(currentTouch.pressed, "OnDoubleClick", null);
                        }
                        currentTouch.clickTime = time;
                    }
                }
                else if (currentTouch.dragStarted)
                {
                    if (onDrop != null)
                    {
                        onDrop(currentTouch.current, currentTouch.dragged);
                    }
                    Notify(currentTouch.current, "OnDrop", currentTouch.dragged);
                }
            }
            currentTouch.dragStarted = false;
            currentTouch.pressed = null;
            currentTouch.dragged = null;
        }
    }

    public void ProcessTouch(bool pressed, bool released)
    {
        bool isMouse = currentScheme == ControlScheme.Mouse;
        float drag = !isMouse ? this.touchDragThreshold : this.mouseDragThreshold;
        float click = !isMouse ? this.touchClickThreshold : this.mouseClickThreshold;
        drag *= drag;
        click *= click;
        if (currentTouch.pressed != null)
        {
            if (released)
            {
                this.ProcessRelease(isMouse, drag);
            }
            this.ProcessPress(pressed, click, drag);
        }
        else if ((isMouse || pressed) || released)
        {
            this.ProcessPress(pressed, click, drag);
            if (released)
            {
                this.ProcessRelease(isMouse, drag);
            }
        }
    }

    public void ProcessTouches()
    {
        currentScheme = ControlScheme.Touch;
        int num = (GetInputTouchCount != null) ? GetInputTouchCount() : Input.touchCount;
        for (int i = 0; i < num; i++)
        {
            int fingerId;
            TouchPhase phase;
            Vector2 position;
            int tapCount;
            if (GetInputTouch == null)
            {
                UnityEngine.Touch touch = Input.GetTouch(i);
                phase = touch.phase;
                fingerId = touch.fingerId;
                position = touch.position;
                tapCount = touch.tapCount;
            }
            else
            {
                Touch touch2 = GetInputTouch(i);
                phase = touch2.phase;
                fingerId = touch2.fingerId;
                position = touch2.position;
                tapCount = touch2.tapCount;
            }
            currentTouchID = !this.allowMultiTouch ? 1 : fingerId;
            currentTouch = GetTouch(currentTouchID);
            bool pressed = (phase == TouchPhase.Began) || currentTouch.touchBegan;
            bool released = (phase == TouchPhase.Canceled) || (phase == TouchPhase.Ended);
            currentTouch.touchBegan = false;
            currentTouch.delta = !pressed ? (position - currentTouch.pos) : Vector2.zero;
            currentTouch.pos = position;
            if (!Raycast((Vector3) currentTouch.pos))
            {
                hoveredObject = fallThrough;
            }
            if (hoveredObject == null)
            {
                hoveredObject = mGenericHandler;
            }
            currentTouch.last = currentTouch.current;
            currentTouch.current = hoveredObject;
            lastTouchPosition = currentTouch.pos;
            if (pressed)
            {
                currentTouch.pressedCam = currentCamera;
            }
            else if (currentTouch.pressed != null)
            {
                currentCamera = currentTouch.pressedCam;
            }
            if (tapCount > 1)
            {
                currentTouch.clickTime = RealTime.time;
            }
            this.ProcessTouch(pressed, released);
            if (released)
            {
                RemoveTouch(currentTouchID);
            }
            currentTouch.last = null;
            currentTouch = null;
            if (!this.allowMultiTouch)
            {
                break;
            }
        }
        if (num == 0)
        {
            if (mUsingTouchEvents)
            {
                mUsingTouchEvents = false;
            }
            else if (this.useMouse)
            {
                this.ProcessMouse();
            }
        }
        else
        {
            mUsingTouchEvents = true;
        }
    }

    public static bool Raycast(Vector3 inPos)
    {
        for (int i = 0; i < list.size; i++)
        {
            UICamera camera = list.buffer[i];
            if (!camera.enabled || !NGUITools.GetActive(camera.gameObject))
            {
                continue;
            }
            currentCamera = camera.cachedCamera;
            Vector3 vector = currentCamera.ScreenToViewportPoint(inPos);
            if ((float.IsNaN(vector.x) || float.IsNaN(vector.y)) || ((((vector.x < 0f) || (vector.x > 1f)) || (vector.y < 0f)) || (vector.y > 1f)))
            {
                continue;
            }
            Ray ray = currentCamera.ScreenPointToRay(inPos);
            int layerMask = currentCamera.cullingMask & camera.eventReceiverMask;
            float maxDistance = (camera.rangeDistance <= 0f) ? (currentCamera.farClipPlane - currentCamera.nearClipPlane) : camera.rangeDistance;
            if (camera.eventType == EventType.World_3D)
            {
                if (Physics.Raycast(ray, out lastHit, maxDistance, layerMask))
                {
                    lastWorldPosition = lastHit.point;
                    hoveredObject = lastHit.collider.gameObject;
                    if (!list[0].eventsGoToColliders)
                    {
                        Rigidbody rigidbody = FindRootRigidbody(hoveredObject.transform);
                        if (rigidbody != null)
                        {
                            hoveredObject = rigidbody.gameObject;
                        }
                    }
                    return true;
                }
                continue;
            }
            if (camera.eventType != EventType.UI_3D)
            {
                goto Label_04C9;
            }
            RaycastHit[] hitArray = Physics.RaycastAll(ray, maxDistance, layerMask);
            if (hitArray.Length > 1)
            {
                for (int j = 0; j < hitArray.Length; j++)
                {
                    GameObject obj2 = hitArray[j].collider.gameObject;
                    UIWidget widget = obj2.GetComponent<UIWidget>();
                    if (widget != null)
                    {
                        if (widget.isVisible && ((widget.hitCheck == null) || widget.hitCheck(hitArray[j].point)))
                        {
                            goto Label_0260;
                        }
                        continue;
                    }
                    UIRect rect = NGUITools.FindInParents<UIRect>(obj2);
                    if ((rect != null) && (rect.finalAlpha < 0.001f))
                    {
                        continue;
                    }
                Label_0260:
                    mHit.depth = NGUITools.CalculateRaycastDepth(obj2);
                    if (mHit.depth != 0x7fffffff)
                    {
                        mHit.hit = hitArray[j];
                        mHit.point = hitArray[j].point;
                        mHit.go = hitArray[j].collider.gameObject;
                        mHits.Add(mHit);
                    }
                }
                if (<>f__am$cache50 == null)
                {
                    <>f__am$cache50 = (r1, r2) => r2.depth.CompareTo(r1.depth);
                }
                mHits.Sort(<>f__am$cache50);
                for (int k = 0; k < mHits.size; k++)
                {
                    if (IsVisible(ref mHits.buffer[k]))
                    {
                        DepthEntry entry = mHits[k];
                        lastHit = entry.hit;
                        DepthEntry entry2 = mHits[k];
                        hoveredObject = entry2.go;
                        DepthEntry entry3 = mHits[k];
                        lastWorldPosition = entry3.point;
                        mHits.Clear();
                        return true;
                    }
                }
                mHits.Clear();
                continue;
            }
            if (hitArray.Length != 1)
            {
                continue;
            }
            GameObject gameObject = hitArray[0].collider.gameObject;
            UIWidget component = gameObject.GetComponent<UIWidget>();
            if (component != null)
            {
                if (component.isVisible && ((component.hitCheck == null) || component.hitCheck(hitArray[0].point)))
                {
                    goto Label_0461;
                }
                continue;
            }
            UIRect rect2 = NGUITools.FindInParents<UIRect>(gameObject);
            if ((rect2 != null) && (rect2.finalAlpha < 0.001f))
            {
                continue;
            }
        Label_0461:
            if (!IsVisible(hitArray[0].point, hitArray[0].collider.gameObject))
            {
                continue;
            }
            lastHit = hitArray[0];
            lastWorldPosition = hitArray[0].point;
            hoveredObject = lastHit.collider.gameObject;
            return true;
        Label_04C9:
            if (camera.eventType == EventType.World_2D)
            {
                if (m2DPlane.Raycast(ray, out maxDistance))
                {
                    Vector3 point = ray.GetPoint(maxDistance);
                    Collider2D colliderd = Physics2D.OverlapPoint(point, layerMask);
                    if (colliderd != null)
                    {
                        lastWorldPosition = point;
                        hoveredObject = colliderd.gameObject;
                        if (!camera.eventsGoToColliders)
                        {
                            Rigidbody2D rigidbodyd = FindRootRigidbody2D(hoveredObject.transform);
                            if (rigidbodyd != null)
                            {
                                hoveredObject = rigidbodyd.gameObject;
                            }
                        }
                        return true;
                    }
                }
                continue;
            }
            if ((camera.eventType != EventType.UI_2D) || !m2DPlane.Raycast(ray, out maxDistance))
            {
                continue;
            }
            lastWorldPosition = ray.GetPoint(maxDistance);
            Collider2D[] colliderdArray = Physics2D.OverlapPointAll(lastWorldPosition, layerMask);
            if (colliderdArray.Length > 1)
            {
                for (int m = 0; m < colliderdArray.Length; m++)
                {
                    GameObject obj4 = colliderdArray[m].gameObject;
                    UIWidget widget3 = obj4.GetComponent<UIWidget>();
                    if (widget3 != null)
                    {
                        if (widget3.isVisible && ((widget3.hitCheck == null) || widget3.hitCheck(lastWorldPosition)))
                        {
                            goto Label_0639;
                        }
                        continue;
                    }
                    UIRect rect3 = NGUITools.FindInParents<UIRect>(obj4);
                    if ((rect3 != null) && (rect3.finalAlpha < 0.001f))
                    {
                        continue;
                    }
                Label_0639:
                    mHit.depth = NGUITools.CalculateRaycastDepth(obj4);
                    if (mHit.depth != 0x7fffffff)
                    {
                        mHit.go = obj4;
                        mHit.point = lastWorldPosition;
                        mHits.Add(mHit);
                    }
                }
                if (<>f__am$cache51 == null)
                {
                    <>f__am$cache51 = (r1, r2) => r2.depth.CompareTo(r1.depth);
                }
                mHits.Sort(<>f__am$cache51);
                for (int n = 0; n < mHits.size; n++)
                {
                    if (IsVisible(ref mHits.buffer[n]))
                    {
                        DepthEntry entry4 = mHits[n];
                        hoveredObject = entry4.go;
                        mHits.Clear();
                        return true;
                    }
                }
                mHits.Clear();
                continue;
            }
            if (colliderdArray.Length != 1)
            {
                continue;
            }
            GameObject go = colliderdArray[0].gameObject;
            UIWidget widget4 = go.GetComponent<UIWidget>();
            if (widget4 != null)
            {
                if (widget4.isVisible && ((widget4.hitCheck == null) || widget4.hitCheck(lastWorldPosition)))
                {
                    goto Label_07C3;
                }
                continue;
            }
            UIRect rect4 = NGUITools.FindInParents<UIRect>(go);
            if ((rect4 != null) && (rect4.finalAlpha < 0.001f))
            {
                continue;
            }
        Label_07C3:
            if (IsVisible(lastWorldPosition, go))
            {
                hoveredObject = go;
                return true;
            }
        }
        return false;
    }

    public static void RemoveTouch(int id)
    {
        mTouches.Remove(id);
    }

    public void ShowTooltip(bool val)
    {
        this.mTooltipTime = 0f;
        if (onTooltip != null)
        {
            onTooltip(this.mTooltip, val);
        }
        Notify(this.mTooltip, "OnTooltip", val);
        if (!val)
        {
            this.mTooltip = null;
        }
    }

    private void Start()
    {
        if ((this.eventType != EventType.World_3D) && (this.cachedCamera.transparencySortMode != TransparencySortMode.Orthographic))
        {
            this.cachedCamera.transparencySortMode = TransparencySortMode.Orthographic;
        }
        if (Application.isPlaying)
        {
            if (fallThrough == null)
            {
                UIRoot root = NGUITools.FindInParents<UIRoot>(base.gameObject);
                if (root != null)
                {
                    fallThrough = root.gameObject;
                }
                else
                {
                    Transform transform = base.transform;
                    fallThrough = (transform.parent == null) ? base.gameObject : transform.parent.gameObject;
                }
            }
            this.cachedCamera.eventMask = 0;
        }
        if (this.handlesEvents)
        {
            NGUIDebug.debugRaycast = this.debug;
        }
    }

    private void Update()
    {
        if (this.handlesEvents)
        {
            current = this;
            if (this.useTouch)
            {
                this.ProcessTouches();
            }
            else if (this.useMouse)
            {
                this.ProcessMouse();
            }
            if (onCustomInput != null)
            {
                onCustomInput();
            }
            if (this.useMouse && (mCurrentSelection != null))
            {
                if ((this.cancelKey0 != KeyCode.None) && GetKeyDown(this.cancelKey0))
                {
                    currentScheme = ControlScheme.Controller;
                    currentKey = this.cancelKey0;
                    selectedObject = null;
                }
                else if ((this.cancelKey1 != KeyCode.None) && GetKeyDown(this.cancelKey1))
                {
                    currentScheme = ControlScheme.Controller;
                    currentKey = this.cancelKey1;
                    selectedObject = null;
                }
            }
            if (mCurrentSelection == null)
            {
                inputHasFocus = false;
            }
            else if ((mCurrentSelection == null) || !mCurrentSelection.activeInHierarchy)
            {
                inputHasFocus = false;
                mCurrentSelection = null;
            }
            if ((this.useKeyboard || this.useController) && (mCurrentSelection != null))
            {
                this.ProcessOthers();
            }
            if (this.useMouse && (mHover != null))
            {
                float delta = string.IsNullOrEmpty(this.scrollAxisName) ? 0f : GetAxis(this.scrollAxisName);
                if (delta != 0f)
                {
                    if (onScroll != null)
                    {
                        onScroll(mHover, delta);
                    }
                    Notify(mHover, "OnScroll", delta);
                }
                if ((showTooltips && (this.mTooltipTime != 0f)) && (((this.mTooltipTime < RealTime.time) || GetKey(KeyCode.LeftShift)) || GetKey(KeyCode.RightShift)))
                {
                    this.mTooltip = mHover;
                    currentTouch = mMouse[0];
                    currentTouchID = -1;
                    this.ShowTooltip(true);
                }
            }
            current = null;
            currentTouchID = -100;
        }
    }

    public Camera cachedCamera
    {
        get
        {
            if (this.mCam == null)
            {
                this.mCam = base.GetComponent<Camera>();
            }
            return this.mCam;
        }
    }

    public static Ray currentRay =>
        (((currentCamera == null) || (currentTouch == null)) ? new Ray() : currentCamera.ScreenPointToRay((Vector3) currentTouch.pos));

    public static int dragCount
    {
        get
        {
            int num = 0;
            foreach (KeyValuePair<int, MouseOrTouch> pair in mTouches)
            {
                if (pair.Value.dragged != null)
                {
                    num++;
                }
            }
            for (int i = 0; i < mMouse.Length; i++)
            {
                if (mMouse[i].dragged != null)
                {
                    num++;
                }
            }
            if (controller.dragged != null)
            {
                num++;
            }
            return num;
        }
    }

    public static UICamera eventHandler
    {
        get
        {
            for (int i = 0; i < list.size; i++)
            {
                UICamera camera = list.buffer[i];
                if (((camera != null) && camera.enabled) && NGUITools.GetActive(camera.gameObject))
                {
                    return camera;
                }
            }
            return null;
        }
    }

    [Obsolete("Use delegates instead such as UICamera.onClick, UICamera.onHover, etc.")]
    public static GameObject genericEventHandler
    {
        get => 
            mGenericHandler;
        set
        {
            mGenericHandler = value;
        }
    }

    private bool handlesEvents =>
        (eventHandler == this);

    public static bool isOverUI
    {
        get
        {
            if (currentTouch != null)
            {
                return currentTouch.isOverUI;
            }
            if (hoveredObject == null)
            {
                return false;
            }
            if (hoveredObject == fallThrough)
            {
                return false;
            }
            return (NGUITools.FindInParents<UIRoot>(hoveredObject) != null);
        }
    }

    public static Camera mainCamera
    {
        get
        {
            UICamera eventHandler = UICamera.eventHandler;
            return eventHandler?.cachedCamera;
        }
    }

    public static GameObject selectedObject
    {
        get
        {
            if (mCurrentSelection != null)
            {
                return mCurrentSelection;
            }
            return null;
        }
        set
        {
            if (mCurrentSelection != value)
            {
                bool flag = false;
                if (currentTouch == null)
                {
                    flag = true;
                    currentTouchID = -1;
                    currentTouch = mMouse[0];
                    currentScheme = ControlScheme.Mouse;
                }
                inputHasFocus = false;
                if (onSelect != null)
                {
                    onSelect(selectedObject, false);
                }
                Notify(mCurrentSelection, "OnSelect", false);
                mCurrentSelection = value;
                if (mCurrentSelection != null)
                {
                    if (flag)
                    {
                        UICamera camera = (mCurrentSelection == null) ? list[0] : FindCameraForLayer(mCurrentSelection.layer);
                        if (camera != null)
                        {
                            current = camera;
                            currentCamera = camera.cachedCamera;
                        }
                    }
                    inputHasFocus = mCurrentSelection.activeInHierarchy && (mCurrentSelection.GetComponent<UIInput>() != null);
                    if (onSelect != null)
                    {
                        onSelect(mCurrentSelection, true);
                    }
                    Notify(mCurrentSelection, "OnSelect", true);
                }
                if (flag)
                {
                    current = null;
                    currentCamera = null;
                    currentTouch = null;
                    currentTouchID = -100;
                }
            }
        }
    }

    [Obsolete("Use new OnDragStart / OnDragOver / OnDragOut / OnDragEnd events instead")]
    public bool stickyPress =>
        true;

    public static int touchCount
    {
        get
        {
            int num = 0;
            foreach (KeyValuePair<int, MouseOrTouch> pair in mTouches)
            {
                if (pair.Value.pressed != null)
                {
                    num++;
                }
            }
            for (int i = 0; i < mMouse.Length; i++)
            {
                if (mMouse[i].pressed != null)
                {
                    num++;
                }
            }
            if (controller.pressed != null)
            {
                num++;
            }
            return num;
        }
    }

    public delegate void BoolDelegate(GameObject go, bool state);

    public enum ClickNotification
    {
        None,
        Always,
        BasedOnDelta
    }

    public enum ControlScheme
    {
        Mouse,
        Touch,
        Controller
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct DepthEntry
    {
        public int depth;
        public RaycastHit hit;
        public Vector3 point;
        public GameObject go;
    }

    public enum EventType
    {
        World_3D,
        UI_3D,
        World_2D,
        UI_2D
    }

    public delegate void FloatDelegate(GameObject go, float delta);

    public delegate float GetAxisFunc(string name);

    public delegate bool GetKeyStateFunc(KeyCode key);

    public delegate UICamera.Touch GetTouchCallback(int index);

    public delegate int GetTouchCountCallback();

    public delegate void KeyCodeDelegate(GameObject go, KeyCode key);

    public class MouseOrTouch
    {
        public UICamera.ClickNotification clickNotification = UICamera.ClickNotification.Always;
        public float clickTime;
        public GameObject current;
        public Vector2 delta;
        public GameObject dragged;
        public bool dragStarted;
        public GameObject last;
        public Vector2 lastPos;
        public Vector2 pos;
        public GameObject pressed;
        public Camera pressedCam;
        public bool pressStarted;
        public float pressTime;
        public Vector2 totalDelta;
        public bool touchBegan = true;

        public float deltaTime =>
            (!this.touchBegan ? 0f : (RealTime.time - this.pressTime));

        public bool isOverUI =>
            (((this.current != null) && (this.current != UICamera.fallThrough)) && (NGUITools.FindInParents<UIRoot>(this.current) != null));
    }

    public delegate void MoveDelegate(Vector2 delta);

    public delegate void ObjectDelegate(GameObject go, GameObject obj);

    public delegate void OnCustomInput();

    public delegate void OnScreenResize();

    public class Touch
    {
        public int fingerId;
        public TouchPhase phase;
        public Vector2 position;
        public int tapCount;
    }

    public delegate void VectorDelegate(GameObject go, Vector2 delta);

    public delegate void VoidDelegate(GameObject go);
}

