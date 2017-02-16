namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("Gets system date and time info and stores it in a string variable. An optional format string gives you a lot of control over the formatting (see online docs for format syntax)."), ActionCategory(ActionCategory.Time)]
    public class GetSystemDateTime : FsmStateAction
    {
        [Tooltip("Repeat every frame.")]
        public bool everyFrame;
        [Tooltip("Optional format string. E.g., MM/dd/yyyy HH:mm")]
        public FsmString format;
        [UIHint(UIHint.Variable), Tooltip("Store System DateTime as a string.")]
        public FsmString storeString;

        public override void OnEnter()
        {
            this.storeString.Value = DateTime.Now.ToString(this.format.Value);
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.storeString.Value = DateTime.Now.ToString(this.format.Value);
        }

        public override void Reset()
        {
            this.storeString = null;
            this.format = "MM/dd/yyyy HH:mm";
        }
    }
}

