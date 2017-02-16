namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory(ActionCategory.Audio), Tooltip("AssetManager - load or download AssetStorage.")]
    public class AssetManagerLoader : FsmStateAction
    {
        [Tooltip("Store the result in an Object variable of type AssetData.")]
        public FsmObject assetData;
        [Tooltip("Set the Asset Storage name string.")]
        public FsmString assetName;
        [Tooltip("Optionally send an Event when the load finishes.")]
        public FsmEvent finishEvent;
        [Tooltip("If checked, only download. ( usually, download and load to memory.")]
        public FsmBool isDownloadOnly;

        protected void LoadEndAsset(AssetData data)
        {
            Debug.Log("L:AssetManagerLoader LoadEndAsset");
            if (this.assetData != null)
            {
            }
            if (this.finishEvent != null)
            {
                Debug.Log("L:AssetManagerLoader Finished!");
                base.Fsm.Event(this.finishEvent);
            }
            Debug.Log("L:AssetManagerLoader Finished2!");
            base.Finish();
        }

        public override void OnEnter()
        {
            Debug.Log("L:AssetManagerLoader onEnter");
            if (this.isDownloadOnly.Value)
            {
                AssetManager.downloadAssetStorage(this.assetName.Value, new AssetLoader.LoadEndDataHandler(this.LoadEndAsset));
            }
            else
            {
                Debug.Log("L:AssetManagerLoader load");
                AssetManager.loadAssetStorage(this.assetName.Value, new AssetLoader.LoadEndDataHandler(this.LoadEndAsset));
            }
        }

        public override void Reset()
        {
            this.assetName = string.Empty;
            this.isDownloadOnly = 0;
            this.finishEvent = null;
        }
    }
}

