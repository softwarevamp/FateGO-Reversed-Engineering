using System;
using UnityEngine;

public class TestTalkEffectRootComponent : SceneRootComponent
{
    public GameObject testBackEffectPrefab;
    public GameObject testCharaBackEffectPrefab;
    public GameObject testCharaEffectPrefab;
    public GameObject testEffectPrefab;

    public override void beginInitialize()
    {
        base.beginInitialize();
        SingletonMonoBehaviour<SceneManager>.Instance.endInitialize(this);
    }

    public override void beginStartUp()
    {
        base.beginStartUp();
    }

    public bool setupTestEffectPrefab()
    {
        CommonEffectManager.SetTestEffectPrefab(this.testEffectPrefab, this.testBackEffectPrefab, this.testCharaEffectPrefab, this.testCharaBackEffectPrefab);
        return true;
    }
}

