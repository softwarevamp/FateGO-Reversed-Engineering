using System;

public enum NGuiPlayMakerDelegates
{
    [PlayMakerUtils_FsmEvent("NGUI / ON CHANGE")]
    OnChangeEvent = 14,
    [PlayMakerUtils_FsmEvent("NGUI / ON CLICK")]
    OnClickEvent = 0,
    [PlayMakerUtils_FsmEvent("NGUI / ON DRAG")]
    OnDragEvent = 8,
    [PlayMakerUtils_FsmEvent("NGUI / ON DROP")]
    OnDropEvent = 9,
    [PlayMakerUtils_FsmEvent("NGUI / ON HOVER")]
    OnHoverEvent = 1,
    [PlayMakerUtils_FsmEvent("NGUI / ON HOVER ENTER")]
    OnHoverEventEnter = 2,
    [PlayMakerUtils_FsmEvent("NGUI / ON HOVER EXIT")]
    OnHoverEventExit = 3,
    [PlayMakerUtils_FsmEvent("NGUI / ON PRESS")]
    OnPressEvent = 4,
    [PlayMakerUtils_FsmEvent("NGUI / ON PRESS DOWN")]
    OnPressEventDown = 6,
    [PlayMakerUtils_FsmEvent("NGUI / ON PRESS UP")]
    OnPressEventUp = 5,
    [PlayMakerUtils_FsmEvent("NGUI / ON SELECT")]
    OnSelectEvent = 7,
    [PlayMakerUtils_FsmEvent("NGUI / ON SELECTION CHANGE")]
    OnSelectionChangeEvent = 12,
    [PlayMakerUtils_FsmEvent("NGUI / ON SLIDER CHANGE")]
    OnSliderChangeEvent = 11,
    [PlayMakerUtils_FsmEvent("NGUI / ON SUBMIT")]
    OnSubmitEvent = 10,
    [PlayMakerUtils_FsmEvent("NGUI / ON TOOLTIP")]
    OnTooltipEvent = 13
}

