namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory("ExAction")]
    public class ChangeSceneString : FsmStateAction
    {
        [RequiredField]
        public string SceneName;

        public override void OnEnter()
        {
            SingletonMonoBehaviour<SceneManager>.Instance.changeScene(this.SceneName, SceneManager.FadeType.BLACK, null);
            base.Finish();
        }
    }
}

