namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory(ActionCategory.String), Tooltip("Select a Random String from an array of Strings.")]
    public class SelectRandomString : FsmStateAction
    {
        [UIHint(UIHint.Variable), RequiredField]
        public FsmString storeString;
        [CompoundArray("Strings", "String", "Weight")]
        public FsmString[] strings;
        [HasFloatSlider(0f, 1f)]
        public FsmFloat[] weights;

        private void DoSelectRandomString()
        {
            if (((this.strings != null) && (this.strings.Length != 0)) && (this.storeString != null))
            {
                int randomWeightedIndex = ActionHelpers.GetRandomWeightedIndex(this.weights);
                if (randomWeightedIndex != -1)
                {
                    this.storeString.Value = this.strings[randomWeightedIndex].Value;
                }
            }
        }

        public override void OnEnter()
        {
            this.DoSelectRandomString();
            base.Finish();
        }

        public override void Reset()
        {
            this.strings = new FsmString[3];
            this.weights = new FsmFloat[] { 1f, 1f, 1f };
            this.storeString = null;
        }
    }
}

