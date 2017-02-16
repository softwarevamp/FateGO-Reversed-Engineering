using System;
using UnityEngine;

public class SummonRootComponent : SceneRootComponent
{
    private GameObject bgGo;
    public GameObject bgRoot;
    protected static int FIGURE_ID = 0xc3564;
    private string path;
    private UIStandFigureR standFigure;
    public SummonControl summonCtr;
    public GameObject svtBase;

    public override void beginFinish()
    {
        ((this.summonCtr == null) ? base.gameObject.GetComponent<SummonControl>() : this.summonCtr).quit();
        this.destroyBgInfo();
        SingletonTemplate<MissionNotifyManager>.Instance.EndPause();
    }

    public override void beginInitialize()
    {
        base.beginInitialize();
        base.setMainMenuBar(MainMenuBar.Kind.SUMMON, 30);
        SingletonMonoBehaviour<SceneManager>.Instance.endInitialize(this);
        Debug.Log("Call beginInitialize");
    }

    public override void beginResume()
    {
        Debug.Log("Call beginResume");
        base.beginResume();
    }

    public override void beginStartUp()
    {
        SoundManager.playBgm("BGM_CHALDEA_1");
        this.SetBg();
    }

    private void createSvtStandFigure()
    {
        if (this.standFigure == null)
        {
            UserServantEntity entity = (SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_SERVANT) as UserServantMaster).getHeroineData(FIGURE_ID);
            if (entity != null)
            {
                this.standFigure = StandFigureManager.CreateRenderPrefab(this.svtBase, FIGURE_ID, entity.limitCount, entity.lv, Face.Type.NORMAL, 1, null);
            }
            else
            {
                this.standFigure = StandFigureManager.CreateRenderPrefab(this.svtBase, FIGURE_ID, 0, Face.Type.NORMAL, 1, null);
            }
        }
    }

    private void destroyBgInfo()
    {
        AssetManager.releaseAssetStorage(this.path);
        if (this.bgGo != null)
        {
            UnityEngine.Object.Destroy(this.bgGo);
            this.bgGo = null;
        }
    }

    private void destroySvtStandFigure()
    {
        if (this.standFigure != null)
        {
            UnityEngine.Object.Destroy(this.standFigure.gameObject);
            this.standFigure = null;
        }
    }

    private void EndLoadBg(AssetData data)
    {
        this.bgGo = UnityEngine.Object.Instantiate<GameObject>(data.GetObject<GameObject>("bg"));
        this.bgGo.transform.parent = this.bgRoot.transform;
        this.bgGo.transform.localPosition = Vector3.zero;
        this.bgGo.transform.localScale = Vector3.one;
        MainMenuBar.setMenuActive(true, null);
        base.sendMessageStartUp();
        Debug.Log("Call beginStartUp");
    }

    public void GoToSellServant()
    {
        SceneJumpInfo data = new SceneJumpInfo("SellServant", 0);
        SingletonMonoBehaviour<SceneManager>.Instance.transitionScene(SceneList.Type.Shop, SceneManager.FadeType.BLACK, data);
    }

    private void SetBg()
    {
        this.path = "Bg/10500";
        AssetManager.loadAssetStorage(this.path, new AssetLoader.LoadEndDataHandler(this.EndLoadBg));
    }

    public void SetBgActive(bool isDisp)
    {
        this.bgRoot.SetActive(isDisp);
    }
}

