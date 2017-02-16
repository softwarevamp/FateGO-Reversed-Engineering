namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory("ExAction")]
    public class TransitionScene : FsmStateAction
    {
        [RequiredField]
        public SceneList.Type SceneType;

        public override void OnEnter()
        {
            SingletonMonoBehaviour<SceneManager>.Instance.transitionScene(this.SceneType, SceneManager.FadeType.BLACK, null);
            base.Finish();
        }
    }
}

