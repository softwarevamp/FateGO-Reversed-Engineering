using System;

public class FinishBattleRootComponent : SceneRootComponent
{
    protected BattleData.FinishBattleInfo finishBattleInfo;

    public override void beginInitialize()
    {
        base.beginInitialize();
        SingletonMonoBehaviour<SceneManager>.Instance.endInitialize(this);
    }

    public override void beginStartUp(object data)
    {
        this.finishBattleInfo = (BattleData.FinishBattleInfo) data;
        base.beginStartUp();
    }

    public void endScript(bool isExit)
    {
        ScriptManager.LoadBattleEndGameDemo(this.finishBattleInfo.questId, this.finishBattleInfo.questPhase, false, delegate (string demoInfo) {
            if (demoInfo != null)
            {
                SingletonMonoBehaviour<CommonUI>.Instance.SetLoadMode(ConnectMark.Mode.LOAD);
                BattleSetupInfo data = new BattleSetupInfo {
                    questId = this.finishBattleInfo.questId,
                    questPhase = this.finishBattleInfo.questPhase,
                    battleBefore = false,
                    isBefore = false,
                    demoInfo = demoInfo
                };
                SingletonMonoBehaviour<SceneManager>.Instance.transitionSceneRefresh(SceneList.Type.BattleDemoScene, SceneManager.FadeType.BLACK, data);
            }
            else
            {
                BattleData.DeleteBattleAfterTalkResumeInfo();
                TerminalPramsManager.IsAutoResume = true;
                SingletonMonoBehaviour<SceneManager>.Instance.transitionScene(SceneList.Type.Terminal, SceneManager.FadeType.BLACK, null);
            }
        });
    }

    public void StartFinishBattleScript()
    {
        if (this.finishBattleInfo.winLoseInfo == 2)
        {
            BattleData.DeleteBattleAfterTalkResumeInfo();
            TerminalPramsManager.IsAutoResume = true;
            SingletonMonoBehaviour<SceneManager>.Instance.transitionScene(SceneList.Type.Terminal, SceneManager.FadeType.BLACK, null);
        }
        else if (this.finishBattleInfo.questId > 0)
        {
            ScriptManager.LoadBattleEndGameDemo(this.finishBattleInfo.questId, this.finishBattleInfo.questPhase, true, delegate (string demoInfo) {
                if (demoInfo != null)
                {
                    SingletonMonoBehaviour<CommonUI>.Instance.SetLoadMode(ConnectMark.Mode.LOAD);
                    BattleSetupInfo data = new BattleSetupInfo {
                        questId = this.finishBattleInfo.questId,
                        questPhase = this.finishBattleInfo.questPhase,
                        battleBefore = false,
                        isBefore = true,
                        demoInfo = demoInfo
                    };
                    SingletonMonoBehaviour<SceneManager>.Instance.transitionSceneRefresh(SceneList.Type.BattleDemoScene, SceneManager.FadeType.BLACK, data);
                }
                else
                {
                    ScriptManager.PlayBattleEnd(this.finishBattleInfo.questId, this.finishBattleInfo.questPhase, new ScriptManager.CallbackFunc(this.endScript), false);
                }
            });
        }
        else
        {
            this.endScript(false);
        }
    }
}

