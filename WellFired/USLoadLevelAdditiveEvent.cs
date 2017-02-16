namespace WellFired
{
    using System;
    using UnityEngine;

    [USequencerFriendlyName("Load Level Additively"), USequencerEvent("Application/Load Level Additive"), USequencerEventHideDuration]
    public class USLoadLevelAdditiveEvent : USEventBase
    {
        public bool fireInEditor;
        public int levelIndex = -1;
        public string levelName = string.Empty;

        public override void FireEvent()
        {
            if ((this.levelName.Length == 0) && (this.levelIndex < 0))
            {
                Debug.LogError("You have a Load Level event in your sequence, however, you didn't give it a level to load.");
            }
            else if (this.levelIndex >= Application.levelCount)
            {
                Debug.LogError("You tried to load a level that is invalid, the level index is out of range.");
            }
            else if (!Application.isPlaying && !this.fireInEditor)
            {
                Debug.Log("Load Level Fired, but it wasn't processed, since we are in the editor. Please set the fire In Editor flag in the inspector if you require this behaviour.");
            }
            else
            {
                if (this.levelName.Length != 0)
                {
                    Application.LoadLevelAdditive(this.levelName);
                }
                if (this.levelIndex != -1)
                {
                    Application.LoadLevelAdditive(this.levelIndex);
                }
            }
        }

        public override void ProcessEvent(float deltaTime)
        {
        }
    }
}

