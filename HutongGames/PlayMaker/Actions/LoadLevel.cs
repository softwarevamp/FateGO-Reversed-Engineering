namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Loads a Level by Name. NOTE: Before you can load a level, you have to add it to the list of levels defined in File->Build Settings..."), ActionCategory(ActionCategory.Level)]
    public class LoadLevel : FsmStateAction
    {
        [HutongGames.PlayMaker.Tooltip("Load the level additively, keeping the current scene.")]
        public bool additive;
        [HutongGames.PlayMaker.Tooltip("Load the level asynchronously in the background.")]
        public bool async;
        private AsyncOperation asyncOperation;
        [HutongGames.PlayMaker.Tooltip("Keep this GameObject in the new level. NOTE: The GameObject and components is disabled then enabled on load; uncheck Reset On Disable to keep the active state.")]
        public FsmBool dontDestroyOnLoad;
        [HutongGames.PlayMaker.Tooltip("The name of the level to load. NOTE: Must be in the list of levels defined in File->Build Settings... "), RequiredField]
        public FsmString levelName;
        [HutongGames.PlayMaker.Tooltip("Event to send when the level has loaded. NOTE: This only makes sense if the FSM is still in the scene!")]
        public FsmEvent loadedEvent;

        public override void OnEnter()
        {
            if (this.dontDestroyOnLoad.Value)
            {
                UnityEngine.Object.DontDestroyOnLoad(base.Owner.transform.root.gameObject);
            }
            if (this.additive)
            {
                if (this.async)
                {
                    this.asyncOperation = Application.LoadLevelAdditiveAsync(this.levelName.Value);
                    Debug.Log("LoadLevelAdditiveAsyc: " + this.levelName.Value);
                    return;
                }
                Application.LoadLevelAdditive(this.levelName.Value);
                Debug.Log("LoadLevelAdditive: " + this.levelName.Value);
            }
            else
            {
                if (this.async)
                {
                    this.asyncOperation = Application.LoadLevelAsync(this.levelName.Value);
                    Debug.Log("LoadLevelAsync: " + this.levelName.Value);
                    return;
                }
                Application.LoadLevel(this.levelName.Value);
                Debug.Log("LoadLevel: " + this.levelName.Value);
            }
            this.Log("LOAD COMPLETE");
            base.Fsm.Event(this.loadedEvent);
            base.Finish();
        }

        public override void OnUpdate()
        {
            if (this.asyncOperation.isDone)
            {
                base.Fsm.Event(this.loadedEvent);
                base.Finish();
            }
        }

        public override void Reset()
        {
            this.levelName = string.Empty;
            this.additive = false;
            this.async = false;
            this.loadedEvent = null;
            this.dontDestroyOnLoad = 0;
        }
    }
}

