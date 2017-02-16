using System;
using UnityEngine;

public class ScriptMessageGestureReciver : MonoBehaviour
{
    public MonoBehaviour swipCallback;

    protected void FingerGestures_OnGestureEvent(Gesture gesture)
    {
        if (gesture is SwipeGesture)
        {
            Debug.Log(string.Concat(new object[] { "ScriptMessageGestureReciver: Swipe [", gesture.StartPosition, "] ", (gesture.StartSelection == null) ? "null" : gesture.StartSelection.name }));
            if (((this.swipCallback != null) && (gesture.StartSelection != null)) && UICamera.Raycast(new Vector3(gesture.StartPosition.x, gesture.StartPosition.y)))
            {
                RaycastHit lastHit = UICamera.lastHit;
                Debug.Log("hit " + lastHit.transform.name + " " + gesture.StartSelection.name + " " + base.gameObject.name);
                if (gesture.StartSelection == base.gameObject)
                {
                    this.swipCallback.SendMessage("OnSwipe", gesture);
                }
            }
        }
    }

    private void Start()
    {
        FingerGestures.OnGestureEvent += new Gesture.EventHandler(this.FingerGestures_OnGestureEvent);
    }
}

