namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("Selects a Random Game Object from an array of Game Objects."), ActionCategory(ActionCategory.GameObject)]
    public class SelectRandomGameObject : FsmStateAction
    {
        [CompoundArray("Game Objects", "Game Object", "Weight")]
        public FsmGameObject[] gameObjects;
        [UIHint(UIHint.Variable), RequiredField]
        public FsmGameObject storeGameObject;
        [HasFloatSlider(0f, 1f)]
        public FsmFloat[] weights;

        private void DoSelectRandomGameObject()
        {
            if (((this.gameObjects != null) && (this.gameObjects.Length != 0)) && (this.storeGameObject != null))
            {
                int randomWeightedIndex = ActionHelpers.GetRandomWeightedIndex(this.weights);
                if (randomWeightedIndex != -1)
                {
                    this.storeGameObject.Value = this.gameObjects[randomWeightedIndex].Value;
                }
            }
        }

        public override void OnEnter()
        {
            this.DoSelectRandomGameObject();
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObjects = new FsmGameObject[3];
            this.weights = new FsmFloat[] { 1f, 1f, 1f };
            this.storeGameObject = null;
        }
    }
}

