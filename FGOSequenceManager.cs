using System;
using UnityEngine;

public class FGOSequenceManager : SingletonMonoBehaviour<FGOSequenceManager>
{
    public Transform allEffectRoot;
    public Transform bgRoot;
    public GameObject cameraPos;
    public Camera cutInCamera;
    public Camera effectCamera;
    public GameObject faderObject;
    public GameObject fieldPos;
    public bool isEditorMode;
    protected bool isInitialized;
    public Camera mainCamera;
    private GameObject oldBg;
    public GameObject standardCutIn;

    private void Awake()
    {
        base.Awake();
    }

    public void ChangeBg(string name, string bgType, bool parentCamera, Vector3 pos, Vector3 rot, System.Action callback)
    {
        int num;
        if (!int.TryParse(bgType, out num))
        {
            num = 0;
        }
        if (!this.isEditorMode)
        {
            SingletonMonoBehaviour<BattleSequenceManager>.Instance.changeBg(int.Parse(name), num, pos, rot, parentCamera, callback);
        }
    }

    public Transform getCameraTransform(string name)
    {
        if (this.cameraPos == null)
        {
            Debug.Log("Can not found RootCameraPrefab.");
            this.setup();
        }
        Debug.Log("Search Camera Position:" + name);
        return this.cameraPos.transform.getNodeFromName(name, false);
    }

    public Transform getCharacterPosition(string name)
    {
        if (this.fieldPos == null)
        {
            Debug.Log("Can not found FieldMotionPrefab.");
            this.setup();
        }
        return this.fieldPos.transform.getNodeFromName(name, false);
    }

    public void InitNoblePhantasm()
    {
        Debug.Log("InitNoblePhantasm!");
        ForceDisableObject component = base.GetComponent<ForceDisableObject>();
        if (component != null)
        {
            component.DisableAllObjects();
        }
    }

    private void setup()
    {
        this.cameraPos = GameObject.Find("RootCameraPrefab");
        this.fieldPos = GameObject.Find("FieldMotionPrefab");
    }

    public void SetupSound()
    {
        if (this.isEditorMode && !this.isInitialized)
        {
            SoundManager.initialize();
            this.isInitialized = true;
        }
    }

    private void Start()
    {
        this.setup();
    }
}

