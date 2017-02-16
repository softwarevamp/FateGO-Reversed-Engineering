namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("Open a web view on a handheld device."), ActionCategory(ActionCategory.Device)]
    public class DeviceWebView : FsmStateAction
    {
        [RequiredField]
        public FsmString webPath;

        protected void OnEndWebView()
        {
            base.Finish();
        }

        public override void OnEnter()
        {
            WebViewManager.OpenView(string.Empty, this.webPath.Value, new System.Action(this.OnEndWebView));
        }
    }
}

