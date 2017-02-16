namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory("ExAction")]
    public class ReleaseScene : FsmStateAction
    {
        [RequiredField]
        public SceneManager.FadeType fadeType = SceneManager.FadeType.BLACK;
        [RequiredField]
        public SceneList.Type SceneType;

        public override void OnEnter()
        {
            SingletonMonoBehaviour<SceneManager>.Instance.transitionSceneRefresh(this.SceneType, this.fadeType, null);
            base.Finish();
        }
    }
}

