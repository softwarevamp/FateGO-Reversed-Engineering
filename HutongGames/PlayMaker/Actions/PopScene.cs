namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory("ExAction")]
    public class PopScene : FsmStateAction
    {
        public override void OnEnter()
        {
            SingletonMonoBehaviour<SceneManager>.Instance.popScene(SceneManager.FadeType.BLACK, null);
            base.Finish();
        }
    }
}

