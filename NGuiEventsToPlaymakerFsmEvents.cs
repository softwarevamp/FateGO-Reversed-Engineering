using HutongGames.PlayMaker;
using System;
using System.Collections.Generic;
using UnityEngine;

public class NGuiEventsToPlaymakerFsmEvents : MonoBehaviour
{
    private UIInput _input;
    private UIProgressBar _pBar;
    private UIToggle _toggle;
    private int[] _usage;
    public static UICamera.MouseOrTouch currentTouch;
    public List<NGuiPlayMakerDelegates> customEventsKeys;
    public List<string> customEventsValues;
    public bool debug;
    public bool OnlyShowImplemented;
    public PlayMakerFSM targetFSM;

    public bool DoesTargetImplementsEvent(PlayMakerFSM fsm, NGuiPlayMakerDelegates fsmEventDelegate) => 
        this.DoesTargetImplementsEvent(fsm, NGuiPlayMakerProxy.GetFsmEventEnumValue(fsmEventDelegate));

    public bool DoesTargetImplementsEvent(PlayMakerFSM fsm, string fsmEvent)
    {
        foreach (FsmTransition transition in fsm.FsmGlobalTransitions)
        {
            if (transition.EventName.Equals(fsmEvent))
            {
                return true;
            }
        }
        foreach (FsmState state in fsm.FsmStates)
        {
            foreach (FsmTransition transition2 in state.Transitions)
            {
                if (transition2.EventName.Equals(fsmEvent))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool DoesTargetMissEventImplementation(PlayMakerFSM fsm, NGuiPlayMakerDelegates fsmEventDelegate) => 
        this.DoesTargetMissEventImplementation(fsm, NGuiPlayMakerProxy.GetFsmEventEnumValue(fsmEventDelegate));

    public bool DoesTargetMissEventImplementation(PlayMakerFSM fsm, string fsmEvent)
    {
        if (!this.DoesTargetImplementsEvent(fsm, fsmEvent))
        {
            foreach (FsmEvent event2 in fsm.FsmEvents)
            {
                if (event2.Name.Equals(fsmEvent))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void FireNGUIPlayMakerEvent(NGuiPlayMakerDelegates _event)
    {
        if (this.customEventsKeys.Contains(_event))
        {
            this.targetFSM.SendEvent(this.customEventsValues[this.customEventsKeys.IndexOf(_event)]);
        }
        else
        {
            if (this.debug)
            {
                Debug.Log("Sending event" + NGuiPlayMakerProxy.GetFsmEventEnumValue(_event));
            }
            this.targetFSM.SendEvent(NGuiPlayMakerProxy.GetFsmEventEnumValue(_event));
        }
    }

    public int getUsage(NGuiPlayMakerDelegates fsmEventDelegate)
    {
        if (this._usage == null)
        {
            return 0;
        }
        int index = (int) fsmEventDelegate;
        if (index >= this._usage.Length)
        {
            return -1;
        }
        return this._usage[index];
    }

    private void OnChange()
    {
        if (base.enabled && (this.targetFSM != null))
        {
            this._usage[14]++;
            if (this._input != null)
            {
                Fsm.EventData.StringData = this._input.value;
                if (this.debug)
                {
                    Debug.Log("NGuiEventsToPlaymakerFsmEvents UIInput OnChange(" + this._input.value + ") to " + this.targetFSM.gameObject.name + "/" + this.targetFSM.FsmName);
                }
            }
            if (this._toggle != null)
            {
                Fsm.EventData.BoolData = this._toggle.value;
                if (this.debug)
                {
                    Debug.Log(string.Concat(new object[] { "NGuiEventsToPlaymakerFsmEvents UIToggle OnChange(", this._toggle.value, ") to ", this.targetFSM.gameObject.name, "/", this.targetFSM.FsmName }));
                }
            }
            this.FireNGUIPlayMakerEvent(NGuiPlayMakerDelegates.OnChangeEvent);
        }
    }

    private void OnClick()
    {
        if (base.enabled && (this.targetFSM != null))
        {
            this._usage[0]++;
            if (this.debug)
            {
                Debug.Log(string.Concat(new object[] { "NGuiEventsToPlaymakerFsmEvents OnClick() ", this._usage[0], " to ", this.targetFSM.gameObject.name, "/", this.targetFSM.FsmName }));
            }
            currentTouch = UICamera.currentTouch;
            this.FireNGUIPlayMakerEvent(NGuiPlayMakerDelegates.OnClickEvent);
        }
    }

    private void OnDrag(Vector2 delta)
    {
        if (base.enabled && (this.targetFSM != null))
        {
            if (this.debug)
            {
                Debug.Log(string.Concat(new object[] { "NGuiEventsToPlaymakerFsmEvents OnDrag(", delta, ") to ", this.targetFSM.gameObject.name, "/", this.targetFSM.FsmName }));
            }
            Fsm.EventData.Vector3Data = new Vector3(delta.x, delta.y);
            currentTouch = UICamera.currentTouch;
            this.FireNGUIPlayMakerEvent(NGuiPlayMakerDelegates.OnDragEvent);
        }
    }

    private void OnDrop(GameObject go)
    {
        if (base.enabled && (this.targetFSM != null))
        {
            if (this.debug)
            {
                Debug.Log("NGuiEventsToPlaymakerFsmEvents OnDrop(" + go.name + ") to " + this.targetFSM.gameObject.name + "/" + this.targetFSM.FsmName);
            }
            Fsm.EventData.GameObjectData = go;
            currentTouch = UICamera.currentTouch;
            this.FireNGUIPlayMakerEvent(NGuiPlayMakerDelegates.OnDropEvent);
        }
    }

    private void OnEnable()
    {
        if ((this._usage == null) || (this._usage.Length == 0))
        {
            this._usage = new int[Enum.GetNames(typeof(NGuiPlayMakerDelegates)).Length];
        }
        if (this.targetFSM == null)
        {
            this.targetFSM = base.GetComponent<PlayMakerFSM>();
        }
        if (this.targetFSM == null)
        {
            base.enabled = false;
            Debug.LogWarning("No Fsm Target assigned");
        }
        if (this.DoesTargetImplementsEvent(this.targetFSM, NGuiPlayMakerDelegates.OnSubmitEvent))
        {
            this._input = base.GetComponent<UIInput>();
            if (this._input != null)
            {
                EventDelegate item = new EventDelegate {
                    target = this,
                    methodName = "OnSubmitChange"
                };
                this._input.onSubmit.Add(item);
            }
        }
        if (this.DoesTargetImplementsEvent(this.targetFSM, NGuiPlayMakerDelegates.OnSliderChangeEvent))
        {
            this._pBar = base.GetComponent<UIProgressBar>();
            if (this._pBar != null)
            {
                EventDelegate delegate3 = new EventDelegate {
                    target = this,
                    methodName = "OnSliderChange"
                };
                this._pBar.onChange.Add(delegate3);
            }
        }
        if (this.DoesTargetImplementsEvent(this.targetFSM, NGuiPlayMakerDelegates.OnChangeEvent))
        {
            this._input = base.GetComponent<UIInput>();
            if (this._input != null)
            {
                EventDelegate delegate4 = new EventDelegate {
                    target = this,
                    methodName = "OnChange"
                };
                this._input.onChange.Add(delegate4);
            }
            this._toggle = base.GetComponent<UIToggle>();
            if (this._toggle != null)
            {
                EventDelegate delegate5 = new EventDelegate {
                    target = this,
                    methodName = "OnChange"
                };
                this._toggle.onChange.Add(delegate5);
            }
        }
    }

    private void OnHover(bool isOver)
    {
        if (base.enabled && (this.targetFSM != null))
        {
            this._usage[1]++;
            if (this.debug)
            {
                Debug.Log(string.Concat(new object[] { "NGuiEventsToPlaymakerFsmEvents OnHover(", isOver, ") ", this._usage[0], " to ", this.targetFSM.gameObject.name, "/", this.targetFSM.FsmName }));
            }
            Fsm.EventData.BoolData = isOver;
            currentTouch = UICamera.currentTouch;
            this.FireNGUIPlayMakerEvent(NGuiPlayMakerDelegates.OnHoverEvent);
            if (isOver)
            {
                this.FireNGUIPlayMakerEvent(NGuiPlayMakerDelegates.OnHoverEventEnter);
                this._usage[2]++;
            }
            else
            {
                this.FireNGUIPlayMakerEvent(NGuiPlayMakerDelegates.OnHoverEventExit);
                this._usage[3]++;
            }
        }
    }

    private void OnPress(bool pressed)
    {
        if (base.enabled && (this.targetFSM != null))
        {
            this._usage[4]++;
            if (this.debug)
            {
                Debug.Log(string.Concat(new object[] { "NGuiEventsToPlaymakerFsmEvents OnPress(", pressed, ") ", this._usage[4], " to ", this.targetFSM.gameObject.name, "/", this.targetFSM.FsmName }));
            }
            Fsm.EventData.BoolData = pressed;
            currentTouch = UICamera.currentTouch;
            this.FireNGUIPlayMakerEvent(NGuiPlayMakerDelegates.OnPressEvent);
            if (pressed)
            {
                this.FireNGUIPlayMakerEvent(NGuiPlayMakerDelegates.OnPressEventDown);
                this._usage[6]++;
            }
            else
            {
                this.FireNGUIPlayMakerEvent(NGuiPlayMakerDelegates.OnPressEventUp);
                this._usage[5]++;
            }
        }
    }

    private void OnSelect(bool selected)
    {
        if (base.enabled && (this.targetFSM != null))
        {
            if (this.debug)
            {
                Debug.Log(string.Concat(new object[] { "NGuiEventsToPlaymakerFsmEvents OnSelect(", selected, ") to ", this.targetFSM.gameObject.name, "/", this.targetFSM.FsmName }));
            }
            Fsm.EventData.BoolData = selected;
            currentTouch = UICamera.currentTouch;
            this.FireNGUIPlayMakerEvent(NGuiPlayMakerDelegates.OnSelectEvent);
        }
    }

    private void OnSelectionChange(string item)
    {
        if (base.enabled && (this.targetFSM != null))
        {
            this._usage[12]++;
            if (this.debug)
            {
                Debug.Log(string.Concat(new object[] { "NGuiEventsToPlaymakerFsmEvents OnSelectionChange(", item, ") ", this._usage[12], " to ", this.targetFSM.gameObject.name, "/", this.targetFSM.FsmName }));
            }
            Fsm.EventData.StringData = item;
            this.FireNGUIPlayMakerEvent(NGuiPlayMakerDelegates.OnSelectionChangeEvent);
        }
    }

    private void OnSliderChange(float value)
    {
        if (base.enabled && (this.targetFSM != null))
        {
            this._usage[11]++;
            Fsm.EventData.FloatData = value;
            float num = 0f;
            if (this._pBar != null)
            {
                num = this._pBar.value;
                Fsm.EventData.FloatData = num;
            }
            if (this.debug)
            {
                Debug.Log(string.Concat(new object[] { "NGuiEventsToPlaymakerFsmEvents OnSliderChange(", num, ") to ", this.targetFSM.gameObject.name, "/", this.targetFSM.FsmName }));
            }
            this.FireNGUIPlayMakerEvent(NGuiPlayMakerDelegates.OnSliderChangeEvent);
        }
    }

    private void OnSubmitChange()
    {
        if (base.enabled && (this.targetFSM != null))
        {
            this._usage[10]++;
            string str = string.Empty;
            if (this._input != null)
            {
                str = this._input.value;
                Fsm.EventData.StringData = str;
            }
            if (this.debug)
            {
                Debug.Log("NGuiEventsToPlaymakerFsmEvents OnSubmitChange (" + str + ") to " + this.targetFSM.gameObject.name + "/" + this.targetFSM.FsmName);
            }
            this.FireNGUIPlayMakerEvent(NGuiPlayMakerDelegates.OnSubmitEvent);
        }
    }

    private void OnTooltip(bool show)
    {
        if (base.enabled && (this.targetFSM != null))
        {
            if (this.debug)
            {
                Debug.Log(string.Concat(new object[] { "NGuiEventsToPlaymakerFsmEvents OnTooltip(", show, ") to ", this.targetFSM.gameObject.name, "/", this.targetFSM.FsmName }));
            }
            Fsm.EventData.BoolData = show;
            currentTouch = UICamera.currentTouch;
            this.FireNGUIPlayMakerEvent(NGuiPlayMakerDelegates.OnTooltipEvent);
        }
    }

    public void SetCurrentSelection()
    {
        if ((base.enabled && (this.targetFSM != null)) && (UIPopupList.current != null))
        {
            this._usage[12]++;
            string str = !UIPopupList.current.isLocalized ? UIPopupList.current.value : Localization.Get(UIPopupList.current.value);
            if (this.debug)
            {
                Debug.Log(string.Concat(new object[] { "NGuiEventsToPlaymakerFsmEvents OnSelectionChange(", str, ") ", this._usage[12], " to ", this.targetFSM.gameObject.name, "/", this.targetFSM.FsmName }));
            }
            Fsm.EventData.StringData = str;
            this.FireNGUIPlayMakerEvent(NGuiPlayMakerDelegates.OnSelectionChangeEvent);
        }
    }
}

