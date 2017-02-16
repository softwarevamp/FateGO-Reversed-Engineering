namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory("ExAction")]
    public class PushScene : FsmStateAction
    {
        [RequiredField]
        public SceneList.Type SceneType;

        public override void OnEnter()
        {
            SingletonMonoBehaviour<SceneManager>.Instance.pushScene(this.SceneType, SceneManager.FadeType.BLACK, null);
            base.Finish();
        }
    }
}

