using HutongGames.PlayMaker;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using WellFired;

public class BattleActorControl : BaseMonoBehaviour
{
    public int actindex;
    private BattleActionData actiondata;
    public GameObject actorObject;
    private string actorside;
    private float animetimescale = 1f;
    private float backupFov;
    private BattleLogData battleLog = new BattleLogData("act", 90);
    private BattleServantData battleSvtData;
    private Hashtable CompleteList = new Hashtable();
    private Vector3 criteriaPos;
    private DIR dir;
    private string endmotionevent;
    private GameObject enemyStage;
    private Hashtable EventList = new Hashtable();
    public BattleFBXComponent fbxcomponent;
    private Vector3 headupVec;
    private bool isEnemy;
    public PlayMakerFSM[] motionFSM;
    private GameObject myStage;
    private System.Action noblePhantasmCallback;
    private ParticleDisconnector particleDisconnectorObj;
    public BattlePerformance performance;
    protected List<SkinnedMeshRenderer> rendererArrayList;
    private float resumetimescale = 1f;
    private float scale = 1f;
    private Transform shadowObj;
    private GameObject shadowObject;
    private static Dictionary<int, Vector3[]> ShadowTransform;
    protected Vector3 ShakeRange;
    protected float ShakeTargetTime;
    protected float ShakeTime;
    protected bool Shaking;
    private bool stepFlg;
    private GameObject targetObject;
    public int uniqueID;

    public void AddChildNodesRenderer(GameObject obj)
    {
        foreach (SkinnedMeshRenderer renderer in obj.GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            if (!this.rendererArrayList.Contains(renderer))
            {
                this.rendererArrayList.Add(renderer);
            }
        }
    }

    public void ChangeShadowColor(Color col, float duration = 0.3f)
    {
        if (col == Color.clear)
        {
            col = this.performance.data.getGroundShadowColor();
        }
        else
        {
            col.r = 1f;
            col.g = 1f;
            col.b = 1f;
        }
        if (this.shadowObj != null)
        {
            this.shadowObj.GetComponent<Renderer>().material.color = col;
        }
    }

    public void ChangeShadowTexture(int shadowId)
    {
        if (this.shadowObj != null)
        {
            Texture2D shadowTexture = null;
            shadowTexture = this.performance.bgPerf.GetShadowTexture(shadowId);
            if (shadowTexture == null)
            {
                shadowTexture = Resources.Load("Battle/Textures/shadow_" + shadowId) as Texture2D;
            }
            if (shadowTexture == null)
            {
                shadowTexture = Resources.Load("Battle/Textures/shadow_1") as Texture2D;
            }
            if (shadowTexture != null)
            {
                this.shadowObj.GetComponent<Renderer>().material.mainTexture = shadowTexture;
            }
        }
    }

    public bool checkAnimation(string animname) => 
        this.fbxcomponent.checkAnimation(animname);

    public bool checkGEvent(string name)
    {
        foreach (FsmTransition transition in this.motionFSM[this.actindex].FsmGlobalTransitions)
        {
            if (transition.EventName == name)
            {
                return true;
            }
        }
        return false;
    }

    public bool checkID(int id) => 
        (this.uniqueID == id);

    public bool checkMotionEvent(string name)
    {
        foreach (FsmTransition transition in this.motionFSM[0].FsmGlobalTransitions)
        {
            if (transition.EventName == name)
            {
                return true;
            }
        }
        return false;
    }

    public bool checkNextAttackMe()
    {
        if (this.actiondata == null)
        {
            return false;
        }
        return this.actiondata.nextattackme;
    }

    public bool checkPrevAttackMe()
    {
        if (this.actiondata == null)
        {
            return false;
        }
        return this.actiondata.prevattackme;
    }

    public bool checkScriptValue(string key, int value) => 
        this.battleSvtData.checkScriptValue(key, value);

    public bool checkStepFlg() => 
        this.stepFlg;

    public bool checkStepIn()
    {
        if (this.isEnemy)
        {
            return this.checkScriptValue("summon", 1);
        }
        int num = this.battleSvtData.getStepRate();
        return (UnityEngine.Random.Range(0, 0x3e8) < num);
    }

    public bool checkVoice(Voice.BATTLE type) => 
        ServantAssetLoadManager.checkBattleVoice(this.getServantId(), this.getLimitCount(), type);

    [DebuggerHidden]
    private IEnumerator colloadTransformServant() => 
        new <colloadTransformServant>c__Iterator25 { <>f__this = this };

    private void Complete(string name)
    {
        this.printLog($"@OnComplete Fired!
 => <{name}>:");
        if (this.CompleteList.ContainsKey(name))
        {
            EventClass class2 = (EventClass) this.CompleteList[name];
            this.CompleteList.Remove(name);
            class2.Proc();
        }
        this.sendEventFSM(name);
    }

    public void endMotion(string call)
    {
        this.printLog($"@endMotion
 =>{call}");
        this.sendEventFSM(call);
    }

    public void finishMotion()
    {
        if (this.performance != null)
        {
            if (this.actiondata != null)
            {
                this.performance.endActionData();
            }
        }
        else
        {
            Debug.Log("err - finishMotion");
        }
    }

    public GameObject getActorEffect(string name) => 
        ServantAssetLoadManager.loadNoblePhantasmEffect(this.battleSvtData.TreasureDevice.id, name);

    public GameObject getActorEffectFromActor(string name)
    {
        int svtId = this.battleSvtData.getSvtId();
        int limitCount = this.battleSvtData.getDispLimitCount();
        int weapongroup = this.battleSvtData.getWeaponGroup();
        return ServantAssetLoadManager.loadActorEffectFromActor(svtId, limitCount, weapongroup, name);
    }

    public Color GetAddColor()
    {
        if ((this.rendererArrayList != null) && (this.rendererArrayList.Count > 0))
        {
            return this.rendererArrayList[0].material.GetColor("_AddColor");
        }
        return Color.clear;
    }

    public Transform getDropTransform() => 
        base.gameObject.transform;

    public int getEffectFolder() => 
        this.battleSvtData.getEffectFolder();

    public Transform getFieldRoot() => 
        this.performance.root_field;

    public GameObject getHeadUpObject() => 
        base.gameObject;

    public Vector3 getHeadUpY() => 
        new Vector3(this.headupVec.x, (this.headupVec.y + 0.5f) + this.battleSvtData.getheadUpY());

    public int getLimitCount() => 
        this.battleSvtData.getDispLimitCount();

    public string[] getlog()
    {
        List<string> list = new List<string> {
            $"UniqueID
 => {this.uniqueID}",
            $"weapon:{this.getWeaponGroup()}
 state:{this.motionFSM[0].ActiveStateName}",
            $"common
 state:{this.motionFSM[1].ActiveStateName}"
        };
        BattleServantData data = this.performance.data.getServantData(this.uniqueID);
        list.Add("passive");
        foreach (BattleBuffData.BuffData data2 in data.buffData.getPassiveList())
        {
            list.Add($"id[{data2.buffId}],param[{data2.param}]");
        }
        list.Add("active");
        foreach (BattleBuffData.BuffData data3 in data.buffData.getActiveList())
        {
            list.Add($"id[{data3.buffId}],param[{data3.param}]");
        }
        string[] strArray = this.battleLog.getStringList();
        for (int i = strArray.Length - 1; 0 <= i; i--)
        {
            list.Add(strArray[i]);
        }
        return list.ToArray();
    }

    public Color GetMainColor()
    {
        if ((this.rendererArrayList != null) && (this.rendererArrayList.Count > 0))
        {
            return this.rendererArrayList[0].material.GetColor("_Color");
        }
        return Color.white;
    }

    public int getNobleChainCount()
    {
        if (this.actiondata != null)
        {
            return this.actiondata.chainCount;
        }
        return 0;
    }

    public int getServantId() => 
        this.battleSvtData.getSvtId();

    public string getStrParam() => 
        this.battleSvtData.getStrParam();

    public Vector3 getTargetObjectVec(POS pos)
    {
        float distanceofactor = this.performance.distanceofactor;
        Vector3 zero = Vector3.zero;
        if (this.targetObject != null)
        {
            zero = this.targetObject.transform.position;
        }
        if (this.dir == DIR.RIGHT)
        {
            distanceofactor *= -1f;
        }
        if (pos == POS.FRONT)
        {
            zero = this.targetObject.transform.position + new Vector3(distanceofactor, 0f);
        }
        if (pos == POS.BACK)
        {
            zero = this.targetObject.transform.position + new Vector3(-distanceofactor, 0f);
        }
        if (pos == POS.CRITERIA)
        {
            zero = this.criteriaPos;
        }
        if (pos == POS.MYSTAGE)
        {
            zero = this.myStage.transform.position;
        }
        if (pos == POS.ENEMYSTAGE)
        {
            zero = this.enemyStage.transform.position;
        }
        return zero;
    }

    public Color getWeaponColor() => 
        this.battleSvtData.getWeaponColor();

    public int getWeaponGroup() => 
        this.battleSvtData.getWeaponGroup();

    public int getWeaponScale() => 
        this.battleSvtData.getWeaponScale();

    public bool isChocoServant() => 
        ((this.battleSvtData != null) && (this.battleSvtData.displayType == 3));

    public bool isFlash()
    {
        if (this.actiondata == null)
        {
            return false;
        }
        return ((this.actiondata.flash && !this.actiondata.three) && !this.actiondata.pair);
    }

    public bool isMonsterServant() => 
        (this.battleSvtData.svtType == 4);

    public bool isPairFlash()
    {
        if (this.actiondata == null)
        {
            return false;
        }
        return (this.actiondata.flash && this.actiondata.pair);
    }

    public bool isShadowServant() => 
        ((this.battleSvtData != null) && (this.battleSvtData.displayType == 2));

    public bool isThree()
    {
        if (this.actiondata == null)
        {
            return false;
        }
        return (!this.actiondata.flash && this.actiondata.three);
    }

    public bool isThreeFlash()
    {
        if (this.actiondata == null)
        {
            return false;
        }
        return (this.actiondata.flash && this.actiondata.three);
    }

    public void loadEvents()
    {
        Debug.Log("loadEvents_");
        if (this.battleSvtData.isLoad)
        {
        }
    }

    public void loadTransformServant()
    {
        base.StartCoroutine(this.colloadTransformServant());
    }

    public void motion_BackStep(GameObject target, float hight, float time, POS pos, string pmevent)
    {
        this.targetObject = target;
        this.endmotionevent = pmevent;
        this.playAnimation("step_back");
        EventClass ec = new EventClass {
            eventcall = new EndCallEvent(this.moveStep)
        };
        ec.Add("hight", hight);
        ec.Add("time", time);
        ec.Add("pos", pos);
        this.setAnimationEvent("STEP_START", ec);
    }

    public void motion_Jump(GameObject target, float hight, float time, POS pos, string pmevent)
    {
        this.targetObject = target;
        this.endmotionevent = pmevent;
        this.playAnimation("jump");
        EventClass ec = new EventClass {
            eventcall = new EndCallEvent(this.moveJump)
        };
        ec.Add("hight", hight);
        ec.Add("time", time);
        ec.Add("pos", pos);
        this.setAnimationEvent("JUMP_UP", ec);
    }

    public void motion_Step(GameObject target, float hight, float time, POS pos, string pmevent)
    {
        this.targetObject = target;
        this.endmotionevent = pmevent;
        this.playAnimation("step_front");
        EventClass ec = new EventClass {
            eventcall = new EndCallEvent(this.moveStep)
        };
        ec.Add("hight", hight);
        ec.Add("time", time);
        ec.Add("pos", pos);
        this.setAnimationEvent("STEP_START", ec);
    }

    public void motion_StepWait(GameObject target, float hight, float time, POS pos, string pmevent)
    {
        this.targetObject = target;
        this.endmotionevent = pmevent;
        this.playAnimation("step_front");
        EventClass ec = new EventClass {
            eventcall = new EndCallEvent(this.moveStep)
        };
        ec.Add("hight", hight);
        ec.Add("time", time);
        ec.Add("pos", pos);
        this.setAnimationEvent("STEP_START", ec);
        ec = new EventClass {
            eventcall = new EndCallEvent(this.playAnimation)
        };
        ec.Add("name", "wait");
        this.setAnimationEvent("STEP_END_02", ec);
    }

    public void motion_TreasureArms(GameObject target, float hight, float time, POS pos, string pmevent)
    {
        this.targetObject = target;
        this.endmotionevent = pmevent;
        this.playAnimation("treasure_arms");
        EventClass ec = new EventClass {
            eventcall = new EndCallEvent(this.moveStep)
        };
        ec.Add("hight", hight);
        ec.Add("time", time);
        ec.Add("pos", pos);
        this.setAnimationEvent("STEP_START", ec);
    }

    public void moveDown(float time)
    {
        Hashtable args = new Hashtable {
            { 
                "y",
                0
            },
            { 
                "islocal",
                true
            },
            { 
                "easetype",
                iTween.EaseType.easeInQuad
            },
            { 
                "time",
                time
            }
        };
        iTween.MoveTo(this.actorObject, args);
    }

    public void moveJump(Hashtable table)
    {
        this.moveJump((float) table["hight"], (float) table["time"], (POS) ((int) table["pos"]));
    }

    public void moveJump(float hight, float time, POS pos)
    {
        this.movePos(pos, time * 2f, this.endmotionevent);
        this.moveUp(hight, time);
    }

    public void movePos(POS pos, float time, string endcall = null)
    {
        Hashtable args = new Hashtable();
        Vector3 vector = this.getTargetObjectVec(pos);
        args.Add("position", vector);
        args.Add("easetype", iTween.EaseType.easeOutQuad);
        args.Add("time", time);
        if (endcall != null)
        {
            args.Add("oncompletetarget", base.gameObject);
            args.Add("oncomplete", "endMotion");
            args.Add("oncompleteparams", endcall);
        }
        iTween.MoveTo(base.gameObject, args);
    }

    public void moveStep(Hashtable table)
    {
        this.moveStep((float) table["hight"], (float) table["time"], (POS) ((int) table["pos"]));
    }

    public void moveStep(float hight, float time, POS pos)
    {
        this.movePos(pos, time * 2f, this.endmotionevent);
        this.moveUp(hight, time);
    }

    public void moveUp(Hashtable table)
    {
        this.moveUp((float) table["hight"], (float) table["time"]);
    }

    public void moveUp(float hight, float time)
    {
        Hashtable args = new Hashtable {
            { 
                "y",
                hight
            },
            { 
                "islocal",
                true
            },
            { 
                "easetype",
                iTween.EaseType.easeOutQuad
            },
            { 
                "oncompletetarget",
                base.gameObject
            },
            { 
                "oncomplete",
                "moveDown"
            },
            { 
                "oncompleteparams",
                time
            },
            { 
                "time",
                time
            }
        };
        iTween.MoveTo(this.actorObject, args);
    }

    public void offTouchEvent()
    {
        this.performance.statusPerf.setTouchOff();
    }

    public void OnDestroy()
    {
        this.rendererArrayList = null;
    }

    private void OnEvent(string name)
    {
        if (this.EventList.ContainsKey(name))
        {
            ((EventClass) this.EventList[name]).Proc();
            this.EventList.Remove(name);
        }
        this.sendEventFSM(name);
    }

    public void OnFinishDead()
    {
        if (this.particleDisconnectorObj != null)
        {
            UnityEngine.Object.Destroy(this.particleDisconnectorObj);
            this.particleDisconnectorObj = null;
        }
    }

    private void OnNoblePhantasmLoadComplete(GameObject obj)
    {
        SingletonMonoBehaviour<BattleSequenceManager>.Instance.setup(null, false);
        base.StartCoroutine(this.WaitToNoblePhantasmPlay());
    }

    private void OnNoblePhantasmPlayComplete(USSequencer seq)
    {
        Debug.LogError("OnNoblePhantasmPlayComplete");
        this.BattleSvtData.changeNp(this.BattleSvtData.np + this.BattleSvtData.tmpNp, false);
        this.BattleSvtData.updateNpGauge();
        if (this.noblePhantasmCallback != null)
        {
            BattleFBXComponent.EnableEvent = true;
            this.noblePhantasmCallback();
        }
        this.performance.setupCameraFov(this.backupFov);
        SingletonMonoBehaviour<ServantAssetLoadManager>.Instance.releaseNoblePhantasm();
        Resources.UnloadUnusedAssets();
        GC.Collect();
        this.performance.endNoblePhantasm();
        this.sendEventFSM("FINISHED");
    }

    public void onTouchEvent()
    {
        this.performance.statusPerf.setTouchOn(new BattlePerformanceStatus.TouchEventDelegate(this.skipVoice));
    }

    public void playAnimation(Hashtable table)
    {
        this.playAnimation((string) table["name"]);
    }

    public void playAnimation(string animname)
    {
        this.printLog($"@playAnim
 =>{animname}");
        this.fbxcomponent.playAnimation(animname);
        if (this.IsEnemy && (animname == "wait"))
        {
            Animation componentInChildren = this.actorObject.GetComponentInChildren<Animation>();
            if ((componentInChildren != null) && (componentInChildren[animname] != null))
            {
                this.fbxcomponent.setCurrentAnimTime(UnityEngine.Random.Range(0f, componentInChildren[animname].length));
            }
        }
    }

    public void playAnimationFtag(string animname, string tag)
    {
        this.printLog($"@playAnim
 =>{animname},{tag}");
        this.fbxcomponent.playAnimationFromTag(animname, tag);
    }

    public void playBattleActionData(BattleActionData badata)
    {
        this.actiondata = badata;
        this.setActionData(this.actiondata);
        if (this.actiondata.motionname != null)
        {
            this.playMotion(this.actiondata.motionname);
        }
        else
        {
            this.playMotion("MOTION_" + this.actiondata.motionId);
        }
        if (((this.actiondata != null) && this.actiondata.isDeadMotion()) && (this.battleSvtData != null))
        {
            this.battleSvtData.playDead();
        }
    }

    public void playCallAnimation(string animname, string endevent, string starttag = null)
    {
        this.printLog($"@playCallAnimation
 =>{animname}
 =>{endevent}
 =>{starttag}");
        EventClass ec = new EventClass {
            eventcall = new EndCallEvent(this.sendEventFSM)
        };
        ec.Add("event", endevent);
        this.setAnimationComplete(animname, ec);
        if (starttag != null)
        {
            this.playAnimationFtag(animname, starttag);
        }
        else
        {
            this.playAnimation(animname);
        }
    }

    public void playMotion(string name)
    {
        this.sendMotionEventFSM(name);
    }

    public void playNoActionDataMotion(string name)
    {
        this.actiondata = null;
        this.sendMotionEventFSM(name);
    }

    public GameObject playSideEffect(string effectname, Vector3 vec3, bool sideflip)
    {
        Debug.Log(" Do not Use");
        GameObject obj2 = ServantAssetLoadManager.loadEffect(effectname, this.battleSvtData.getWeaponGroup(), 0);
        if (obj2 == null)
        {
            return null;
        }
        obj2.transform.parent = base.gameObject.transform;
        obj2.transform.localPosition = Vector3.zero;
        obj2.transform.localScale = Vector3.one;
        SingletonMonoBehaviour<BattleSeManager>.Instance.PlaySeByEffect(effectname, null);
        obj2.transform.localPosition = new Vector3(vec3.x * -1f, vec3.y, vec3.z);
        if (sideflip)
        {
            Debug.Log("actorside:" + this.actorside);
            if (this.actorside.Equals("_ENEMY"))
            {
                obj2.transform.localPosition = new Vector3(vec3.x, vec3.y, vec3.z);
                obj2.transform.Rotate((float) 0f, 180f, (float) 0f);
            }
        }
        obj2.transform.parent = this.performance.root_field;
        return obj2;
    }

    public void playVoice(Voice.BATTLE type, float volume, System.Action callback = null)
    {
        ServantAssetLoadManager.playBattleVoice(this.getServantId(), this.getLimitCount(), type, volume, callback, this.uniqueID);
    }

    private void printLog(string str)
    {
        this.battleLog.addStr(str);
    }

    public void RemoveChildNodesRenderer(GameObject obj)
    {
        foreach (SkinnedMeshRenderer renderer in obj.GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            if (this.rendererArrayList.Contains(renderer))
            {
                this.rendererArrayList.Remove(renderer);
            }
        }
    }

    public void resumeAnimation()
    {
        this.animetimescale = this.resumetimescale;
        this.fbxcomponent.setTimeScale(this.animetimescale);
    }

    public void sendDamageEvent()
    {
        this.sendMotionEventFSM("MOTION_DAMAGE");
    }

    public void sendEventFSM(Hashtable table)
    {
        this.sendEventFSM((string) table["event"]);
    }

    public void sendEventFSM(string name)
    {
        string[] strArray = new string[] { "weapon", "common" };
        this.printLog($"@sendEvent[{strArray[this.actindex]}]
 =>{name}
 <={this.motionFSM[this.actindex].ActiveStateName}");
        if (((this.motionFSM != null) && (this.actindex < this.motionFSM.Length)) && (this.motionFSM[this.actindex] != null))
        {
            this.motionFSM[this.actindex].SendEvent(name);
        }
    }

    public void sendMotionEventFSM(string name)
    {
        this.actindex = !this.checkMotionEvent(name) ? 1 : 0;
        this.sendEventFSM(name);
    }

    public void setActionData(BattleActionData actiondata)
    {
        this.motionFSM[0].Fsm.Variables.FindFsmInt("Count_Action").Value = actiondata.actionIndex + 1;
        this.motionFSM[0].Fsm.Variables.FindFsmInt("Count_Hit").Value = actiondata.attackcount;
        if (this.motionFSM[1].Fsm.Variables.FindFsmInt("Count_Action") != null)
        {
            this.motionFSM[1].Fsm.Variables.FindFsmInt("Count_Action").Value = actiondata.actionIndex + 1;
        }
        if (this.motionFSM[1].Fsm.Variables.FindFsmInt("Count_Hit") != null)
        {
            this.motionFSM[1].Fsm.Variables.FindFsmInt("Count_Hit").Value = actiondata.attackcount;
        }
    }

    public void SetAddColor(Color col)
    {
        if (this.rendererArrayList != null)
        {
            foreach (SkinnedMeshRenderer renderer in this.rendererArrayList)
            {
                if (renderer != null)
                {
                    foreach (Material material in renderer.materials)
                    {
                        if (material != null)
                        {
                            material.SetColor("_AddColor", col);
                        }
                    }
                }
            }
        }
    }

    private void setAnimationComplete(string name, EventClass ec)
    {
        if (this.CompleteList.ContainsKey(name))
        {
            this.CompleteList.Remove(name);
        }
        this.CompleteList.Add(name, ec);
    }

    private void setAnimationEvent(string name, EventClass ec)
    {
        if (this.EventList.Contains(name))
        {
            this.EventList.Remove(name);
        }
        this.EventList.Add(name, ec);
    }

    public void setCamera(Camera camera)
    {
        base.gameObject.GetComponent<BillBoard>().setCamera(camera);
    }

    public void setCriteriaPos()
    {
        this.criteriaPos = new Vector3(base.gameObject.transform.position.x, base.gameObject.transform.position.y, base.gameObject.transform.position.z);
    }

    public void setDir()
    {
        if (this.dir == DIR.LEFT)
        {
            this.actorObject.transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
            this.actorObject.transform.localScale = new Vector3(this.scale, this.scale, this.scale);
        }
        else if (this.dir == DIR.RIGHT)
        {
            this.actorObject.transform.localRotation = Quaternion.Euler(0f, 270f, 0f);
            this.actorObject.transform.localScale = new Vector3(-this.scale, this.scale, this.scale);
        }
    }

    public void setDirLeft()
    {
        this.actorObject.transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
        this.actorObject.transform.localScale = new Vector3(this.scale, this.scale, this.scale);
        this.dir = DIR.LEFT;
    }

    public void setDirRight()
    {
        this.actorObject.transform.localRotation = Quaternion.Euler(0f, 270f, 0f);
        this.actorObject.transform.localScale = new Vector3(-this.scale, this.scale, this.scale);
        this.dir = DIR.RIGHT;
    }

    public void SetDispServant(bool isShadow, bool isChoco)
    {
        Color white = Color.white;
        if (this.shadowObject != null)
        {
            UnityEngine.Object.Destroy(this.shadowObject);
            this.shadowObject = null;
        }
        if (isShadow)
        {
            white = new Color(0.05f, 0.05f, 0.05f, 1f);
            Transform transform = this.actorObject.transform.getNodeFromName("en_waist", false);
            if (transform != null)
            {
                GameObject obj2 = SingletonMonoBehaviour<ServantAssetLoadManager>.Instance.createShadowEffect(0);
                obj2.transform.parent = transform;
                obj2.transform.localPosition = Vector3.zero;
                obj2.transform.localEulerAngles = Vector3.zero;
                obj2.transform.localScale = Vector3.one;
                this.shadowObject = obj2;
            }
        }
        if (isChoco)
        {
            ServantAssetLoadManager.changeChocoSahder(base.gameObject);
        }
        SkinnedMeshRenderer[] componentsInChildren = this.actorObject.transform.GetComponentsInChildren<SkinnedMeshRenderer>();
        if (componentsInChildren != null)
        {
            foreach (SkinnedMeshRenderer renderer in componentsInChildren)
            {
                renderer.material.SetColor("_Color", white);
            }
        }
        MeshRenderer[] rendererArray3 = this.actorObject.transform.GetComponentsInChildren<MeshRenderer>();
        if (rendererArray3 != null)
        {
            foreach (MeshRenderer renderer2 in rendererArray3)
            {
                renderer2.material.SetColor("_Color", white);
            }
        }
    }

    public void setEnemyStage(GameObject obj)
    {
        this.enemyStage = obj;
    }

    public void setID(int id)
    {
        this.uniqueID = id;
    }

    public void setInitActionBattle()
    {
        this.setCriteriaPos();
    }

    public void SetMainColor(Color col)
    {
        if (this.rendererArrayList != null)
        {
            foreach (SkinnedMeshRenderer renderer in this.rendererArrayList)
            {
                if (renderer != null)
                {
                    foreach (Material material in renderer.materials)
                    {
                        if (material != null)
                        {
                            material.SetColor("_Color", col);
                        }
                    }
                }
            }
        }
    }

    public void SetMaterialColor(Color fadeColour, float alpha)
    {
        this.SetDispServant(this.isShadowServant(), this.isChocoServant());
        SkinnedMeshRenderer[] componentsInChildren = base.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
        float num = 1f - alpha;
        int num2 = 0;
        foreach (SkinnedMeshRenderer renderer in componentsInChildren)
        {
            Color color = renderer.material.color;
            color = new Color(color.r * num, color.g * num, color.b * num, 1f);
            Color color2 = new Color(fadeColour.r * alpha, fadeColour.g * alpha, fadeColour.b * alpha, 0f);
            foreach (Material material in renderer.materials)
            {
                if (material != null)
                {
                    material.SetColor("_Color", color);
                    material.SetColor("_AddColor", color2);
                }
            }
            num2++;
        }
    }

    public void setMotionlist(string side, GameObject camera, GameObject camerafsm)
    {
        this.motionFSM = new PlayMakerFSM[2];
        GameObject obj2 = base.createObject(this.performance.commonMotionPrefab, base.gameObject.transform, null);
        GameObject obj3 = ServantAssetLoadManager.loadActorMotion(base.gameObject, this.getServantId(), this.getWeaponGroup());
        obj3.SetActive(true);
        this.motionFSM[0] = obj3.GetComponent<PlayMakerFSM>();
        this.motionFSM[1] = obj2.GetComponent<PlayMakerFSM>();
        for (int i = 0; i < 2; i++)
        {
            this.motionFSM[i].Fsm.Variables.FindFsmGameObject("ActorObject").Value = base.gameObject;
            this.motionFSM[i].Fsm.Variables.FindFsmString("ActorSide").Value = side;
            this.motionFSM[i].Fsm.Variables.FindFsmGameObject("Camera").Value = camera;
            this.motionFSM[i].Fsm.Variables.FindFsmGameObject("CameraFsm").Value = camerafsm;
            this.motionFSM[i].Fsm.Variables.FindFsmGameObject("Performance").Value = this.performance.gameObject;
            this.actorside = side;
            if (this.motionFSM[i].Fsm.Variables.FindFsmVector3("TargetFrontPos") != null)
            {
                if (!this.actorside.Equals("_ENEMY"))
                {
                    this.motionFSM[i].Fsm.Variables.FindFsmVector3("TargetFrontPos").Value = new Vector3(this.performance.distanceofactor, 0f);
                    this.motionFSM[i].Fsm.Variables.FindFsmVector3("TargetBackPos").Value = new Vector3(-this.performance.distanceofactor, 0f);
                }
                else
                {
                    this.motionFSM[i].Fsm.Variables.FindFsmVector3("TargetFrontPos").Value = new Vector3(-this.performance.distanceofactor, 0f);
                    this.motionFSM[i].Fsm.Variables.FindFsmVector3("TargetBackPos").Value = new Vector3(this.performance.distanceofactor, 0f);
                }
            }
        }
        foreach (KeyValuePair<string, object> pair in JsonManager.getDictionary(this.getStrParam()))
        {
            if (this.motionFSM[0].Fsm.Variables.FindFsmInt(pair.Key) != null)
            {
                FsmInt introduced6 = this.motionFSM[0].Fsm.Variables.FindFsmInt(pair.Key);
                introduced6.Value = (int) ((long) pair.Value);
            }
        }
    }

    public void setMotionListForDemo(string side, GameObject camera, GameObject camerafsm, GameObject commonMotionPrefab)
    {
        this.motionFSM = new PlayMakerFSM[2];
        GameObject obj2 = base.createObject(commonMotionPrefab, base.gameObject.transform, null);
        GameObject obj3 = ServantAssetLoadManager.loadActorMotion(base.gameObject, this.getServantId(), this.getWeaponGroup());
        obj3.SetActive(true);
        this.motionFSM[0] = obj3.GetComponent<PlayMakerFSM>();
        this.motionFSM[1] = obj2.GetComponent<PlayMakerFSM>();
        for (int i = 0; i < 2; i++)
        {
            this.motionFSM[i].Fsm.Variables.FindFsmGameObject("ActorObject").Value = base.gameObject;
            this.motionFSM[i].Fsm.Variables.FindFsmString("ActorSide").Value = side;
            this.motionFSM[i].Fsm.Variables.FindFsmGameObject("Camera").Value = camera;
            this.motionFSM[i].Fsm.Variables.FindFsmGameObject("CameraFsm").Value = camerafsm;
            this.motionFSM[i].Fsm.Variables.FindFsmGameObject("Performance").Value = null;
            this.actorside = side;
            if (this.motionFSM[i].Fsm.Variables.FindFsmVector3("TargetFrontPos") != null)
            {
                if (!this.actorside.Equals("_ENEMY"))
                {
                    this.motionFSM[i].Fsm.Variables.FindFsmVector3("TargetFrontPos").Value = new Vector3(this.performance.distanceofactor, 0f);
                    this.motionFSM[i].Fsm.Variables.FindFsmVector3("TargetBackPos").Value = new Vector3(-this.performance.distanceofactor, 0f);
                }
                else
                {
                    this.motionFSM[i].Fsm.Variables.FindFsmVector3("TargetFrontPos").Value = new Vector3(-this.performance.distanceofactor, 0f);
                    this.motionFSM[i].Fsm.Variables.FindFsmVector3("TargetBackPos").Value = new Vector3(this.performance.distanceofactor, 0f);
                }
            }
        }
        foreach (KeyValuePair<string, object> pair in JsonManager.getDictionary(this.getStrParam()))
        {
            if (this.motionFSM[0].Fsm.Variables.FindFsmInt(pair.Key) != null)
            {
                FsmInt introduced6 = this.motionFSM[0].Fsm.Variables.FindFsmInt(pair.Key);
                introduced6.Value = (int) ((long) pair.Value);
            }
        }
    }

    public void setMyStage(GameObject obj)
    {
        this.myStage = obj;
    }

    public void setNpDamageVoice(bool flg)
    {
        this.motionFSM[1].Fsm.Variables.FindFsmBool("Noble_Check").Value = flg;
    }

    public void setPerformance(BattlePerformance perf)
    {
        this.performance = perf;
    }

    public void setReloadWeaponMotion()
    {
        GameObject obj2 = ServantAssetLoadManager.loadActorMotion(base.gameObject, this.getServantId(), this.getWeaponGroup());
        obj2.SetActive(true);
        this.motionFSM[0] = obj2.GetComponent<PlayMakerFSM>();
        this.motionFSM[0].Fsm.Variables.FindFsmGameObject("ActorObject").Value = base.gameObject;
        this.motionFSM[0].Fsm.Variables.FindFsmString("ActorSide").Value = this.motionFSM[1].Fsm.Variables.FindFsmString("ActorSide").Value;
        this.motionFSM[0].Fsm.Variables.FindFsmGameObject("Camera").Value = this.motionFSM[1].Fsm.Variables.FindFsmGameObject("Camera").Value;
        this.motionFSM[0].Fsm.Variables.FindFsmGameObject("CameraFsm").Value = this.motionFSM[1].Fsm.Variables.FindFsmGameObject("CameraFsm").Value;
        this.motionFSM[0].Fsm.Variables.FindFsmGameObject("Performance").Value = this.performance.gameObject;
        if (this.motionFSM[0].Fsm.Variables.FindFsmVector3("TargetFrontPos") != null)
        {
            if (!this.actorside.Equals("_ENEMY"))
            {
                this.motionFSM[0].Fsm.Variables.FindFsmVector3("TargetFrontPos").Value = new Vector3(this.performance.distanceofactor, 0f);
                this.motionFSM[0].Fsm.Variables.FindFsmVector3("TargetBackPos").Value = new Vector3(-this.performance.distanceofactor, 0f);
            }
            else
            {
                this.motionFSM[0].Fsm.Variables.FindFsmVector3("TargetFrontPos").Value = new Vector3(-this.performance.distanceofactor, 0f);
                this.motionFSM[0].Fsm.Variables.FindFsmVector3("TargetBackPos").Value = new Vector3(this.performance.distanceofactor, 0f);
            }
        }
        foreach (KeyValuePair<string, object> pair in JsonManager.getDictionary(this.getStrParam()))
        {
            if (this.motionFSM[0].Fsm.Variables.FindFsmInt(pair.Key) != null)
            {
                FsmInt introduced4 = this.motionFSM[0].Fsm.Variables.FindFsmInt(pair.Key);
                introduced4.Value = (int) ((long) pair.Value);
            }
        }
    }

    public void setServantData(BattleServantData svtdata)
    {
        this.battleSvtData = svtdata;
        ServantMaster master = (ServantMaster) SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT);
        if (master.getEntityFromId<ServantEntity>(this.battleSvtData.svtId) == null)
        {
            Debug.LogError("setServantData:Servant not found:" + this.battleSvtData.svtId);
        }
        if (this.shadowObj == null)
        {
            this.shadowObj = base.transform.FindChild("Shadow");
        }
        this.actorObject = ServantAssetLoadManager.loadBattleActor(base.gameObject, this.battleSvtData.svtId, this.battleSvtData.getDispLimitCount());
        GameObject gameObject = this.actorObject.transform.getNodeFromLvName("en_Head", svtdata.getDispLimitCount()).gameObject;
        GameObject obj3 = new GameObject {
            transform = { 
                parent = gameObject.transform,
                localPosition = Vector3.zero,
                localEulerAngles = Vector3.up,
                localScale = Vector3.one,
                parent = base.gameObject.transform
            }
        };
        this.headupVec = obj3.transform.localPosition;
        UnityEngine.Object.Destroy(obj3);
        this.fbxcomponent.RootTransform = this.actorObject.transform;
        this.fbxcomponent.SetEvolutionLevel(this.battleSvtData.svtId, this.battleSvtData.getDispLimitCount());
        TextAsset data = ServantAssetLoadManager.loadAnimEvents(this.battleSvtData.svtId, svtdata.getDispLimitCount());
        this.fbxcomponent.loadAnimationEvents(data, this.battleSvtData.svtId, svtdata.getDispLimitCount());
        ServantAssetLoadManager.preloadWeaponEffect(this.battleSvtData.getWeaponGroup(), this.battleSvtData.getEffectFolder());
        this.SetDispServant(this.isShadowServant(), this.isChocoServant());
        this.UpdateShadow();
        this.stepFlg = false;
        this.rendererArrayList = new List<SkinnedMeshRenderer>(base.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>());
    }

    public void SetShadowColor(Color col)
    {
        if (col == Color.clear)
        {
            col = this.performance.data.getGroundShadowColor();
        }
        if (this.shadowObj != null)
        {
            this.shadowObj.GetComponent<Renderer>().material.color = col;
        }
    }

    public void SetShadowSize(int btlSize)
    {
        if (ShadowTransform == null)
        {
            Dictionary<int, Vector3[]> dictionary = new Dictionary<int, Vector3[]>();
            Vector3[] vectorArray1 = new Vector3[] { new Vector3(1.2f, 1.2f, 1f), new Vector3(0f, 0.03f, -0.2f) };
            dictionary.Add(1, vectorArray1);
            Vector3[] vectorArray2 = new Vector3[] { new Vector3(2f, 3f, 1f), new Vector3(0f, 0.03f, -0.2f) };
            dictionary.Add(2, vectorArray2);
            Vector3[] vectorArray3 = new Vector3[] { new Vector3(2.2f, 3.3f, 1f), new Vector3(0f, 0.03f, -0.2f) };
            dictionary.Add(3, vectorArray3);
            Vector3[] vectorArray4 = new Vector3[] { new Vector3(2.3f, 3.45f, 1f), new Vector3(0f, 0.03f, -0.2f) };
            dictionary.Add(4, vectorArray4);
            Vector3[] vectorArray5 = new Vector3[] { new Vector3(2.5f, 3.75f, 1f), new Vector3(0f, 0.03f, -0.2f) };
            dictionary.Add(5, vectorArray5);
            Vector3[] vectorArray6 = new Vector3[] { new Vector3(25f, 15f, 1f), new Vector3(3.76f, 0.13f, -2.48f) };
            dictionary.Add(6, vectorArray6);
            Vector3[] vectorArray7 = new Vector3[] { new Vector3(5f, 4f, 1f), new Vector3(0.85f, 0.19f, -0.12f) };
            dictionary.Add(7, vectorArray7);
            ShadowTransform = dictionary;
        }
        if (ShadowTransform.ContainsKey(btlSize))
        {
            Vector3[] vectorArray = ShadowTransform[btlSize];
            if (this.shadowObj == null)
            {
                this.shadowObj = base.transform.FindChild("Shadow");
            }
            if (this.shadowObj != null)
            {
                this.shadowObj.transform.localScale = vectorArray[0];
                this.shadowObj.transform.localPosition = vectorArray[1];
            }
        }
    }

    public void setStepFlg(bool flg)
    {
        this.stepFlg = flg;
    }

    public void setTargetObject(GameObject obj)
    {
        if (this.motionFSM != null)
        {
            this.motionFSM[0].Fsm.Variables.FindFsmGameObject("TargetObject").Value = obj;
            this.motionFSM[1].Fsm.Variables.FindFsmGameObject("TargetObject").Value = obj;
        }
    }

    public void setTimeScale(float time)
    {
        this.animetimescale = time;
        this.fbxcomponent.setTimeScale(this.animetimescale);
    }

    public void setTypeEnemy()
    {
        this.isEnemy = true;
    }

    public void setTypePlayer()
    {
        this.isEnemy = false;
    }

    public void ShakePosition(Vector3 range, float tm)
    {
        this.Shaking = true;
        this.ShakeRange = range;
        this.ShakeTargetTime = tm;
        this.ShakeTime = 0f;
    }

    public void skipVoice()
    {
        ServantAssetLoadManager.StopVoice(this.uniqueID);
        this.motionFSM[1].SendEvent("SKIP_VOICE");
    }

    public void Start()
    {
        this.fbxcomponent.OnEvent = (BattleFBXComponent.onEventDelegate) Delegate.Combine(this.fbxcomponent.OnEvent, new BattleFBXComponent.onEventDelegate(this.OnEvent));
        this.fbxcomponent.Complete = (BattleFBXComponent.onEventDelegate) Delegate.Combine(this.fbxcomponent.Complete, new BattleFBXComponent.onEventDelegate(this.Complete));
    }

    public void startBattleUIFade(float time, float targetAlpha)
    {
        this.performance.startBattleUIFade(time, targetAlpha);
    }

    public void startDeadEffect()
    {
        this.startDropItem();
        if (this.isChocoServant())
        {
            ServantAssetLoadManager.changeChocoDeadShader(base.gameObject);
        }
        else
        {
            ServantAssetLoadManager.changeDeadShader(base.gameObject);
        }
        this.particleDisconnectorObj = ParticleDisconnector.DisconnectParticles(this.performance.root_field, base.gameObject.transform);
        object[] args = new object[] { "from", 0f, "to", (this.battleSvtData.BattleSize != 6) ? 3.5f : 15f, "easetype", iTween.EaseType.easeInQuart, "onupdate", "updateDeadEffect", "oncomplete", "OnFinishDead", "time", 1.3f };
        iTween.ValueTo(base.gameObject, iTween.Hash(args));
    }

    public void startDropItem()
    {
        DropInfo[] infoArray = this.battleSvtData.getDropItem();
        if (infoArray != null)
        {
            int index = 0;
            foreach (DropInfo info in infoArray)
            {
                this.performance.dropGetItem(this.getDropTransform(), info, infoArray.Length, index);
                this.performance.data.addDropItems(info);
                index++;
            }
        }
        this.performance.ShowBuff(base.gameObject, -1);
        this.performance.showHeal(base.gameObject, -1);
    }

    public void startNoblePhantasm(System.Action callback, bool flg)
    {
        this.noblePhantasmCallback = callback;
        this.performance.startNoblePhantasm();
        this.performance.setDamageVoiceFlg(false);
        this.performance.fieldmotionfsm.Fsm.Variables.GetFsmGameObject("NPACTOR").Value = base.gameObject;
        BattleFBXComponent.EnableEvent = false;
        SingletonMonoBehaviour<BattleSequenceManager>.Instance.init(this.performance, base.gameObject, this.performance.PlayerActorList, this.performance.EnemyActorList, this.performance.actorcamera, this.performance.bgPerf.bgobject);
        SingletonMonoBehaviour<BattleSequenceManager>.Instance.setWhiteInFlg(flg);
        SingletonMonoBehaviour<BattleSequenceManager>.Instance.loadSequence(this.battleSvtData.svtId, this.battleSvtData.TreasureDevice.id, this.battleSvtData.getDispLimitCount(), new BattleSequenceManager.onGameObjectLoadComplete(this.OnNoblePhantasmLoadComplete));
    }

    public void stopAnimation()
    {
        this.resumetimescale = this.animetimescale;
        this.fbxcomponent.setTimeScale(0f);
    }

    private void Update()
    {
        if (this.Shaking)
        {
            float x = UnityEngine.Random.Range((float) (-this.ShakeRange.x / 2f), (float) (this.ShakeRange.x / 2f));
            float y = UnityEngine.Random.Range((float) (-this.ShakeRange.y / 2f), (float) (this.ShakeRange.y / 2f));
            float z = UnityEngine.Random.Range((float) (-this.ShakeRange.z / 2f), (float) (this.ShakeRange.z / 2f));
            this.actorObject.transform.localPosition = new Vector3(x, y, z);
            this.ShakeTime += Time.deltaTime;
            if (this.ShakeTime >= this.ShakeTargetTime)
            {
                this.actorObject.transform.localPosition = Vector3.zero;
                this.Shaking = false;
            }
        }
    }

    public void updateDeadEffect(float val)
    {
        GameObject gameObject = base.gameObject;
        if (gameObject != null)
        {
            foreach (MeshRenderer renderer in gameObject.GetComponentsInChildren<MeshRenderer>())
            {
                float num2 = val / 3.5f;
                Color color = renderer.material.color;
                renderer.material.color = new Color(color.r, color.g, color.b, 1f - num2);
            }
            foreach (SkinnedMeshRenderer renderer2 in gameObject.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                foreach (Material material in renderer2.materials)
                {
                    if (renderer2.gameObject.name.StartsWith("body"))
                    {
                        material.SetFloat("_ClipSharpnessY", val);
                    }
                    else
                    {
                        material.SetFloat("_ClipSharpnessY", (renderer2.localBounds.center.y - renderer2.localBounds.extents.y) + val);
                    }
                }
            }
        }
    }

    public void UpdateShadow()
    {
        BattleBgMaster master = (BattleBgMaster) SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.BATTLE_BG);
        int currentGroundNo = this.performance.CurrentGroundNo;
        int currentGroundType = this.performance.CurrentGroundType;
        int bgShadowImageId = master.GetBgShadowImageId(currentGroundNo, currentGroundType);
        this.ChangeShadowTexture(bgShadowImageId);
        if ((this.performance != null) && (this.performance.data != null))
        {
            Color col = this.performance.data.getGroundShadowColor();
            this.SetShadowColor(col);
        }
        int battleSize = this.battleSvtData.BattleSize;
        this.SetShadowSize(battleSize);
    }

    [DebuggerHidden]
    private IEnumerator WaitToNoblePhantasmPlay() => 
        new <WaitToNoblePhantasmPlay>c__Iterator24 { <>f__this = this };

    public BattleActionData ActionData =>
        this.actiondata;

    public BattleServantData BattleSvtData =>
        this.battleSvtData;

    public bool IsEnemy =>
        this.isEnemy;

    public int Level =>
        this.battleSvtData.level;

    public int LimitImageIndex =>
        ServantAssetLoadManager.GetLimitImageIndex(this.getServantId(), this.getLimitCount());

    public Transform ShadowObj =>
        this.shadowObj;

    [CompilerGenerated]
    private sealed class <colloadTransformServant>c__Iterator25 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal BattleActorControl <>f__this;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                    this.<>f__this.fbxcomponent.stopAnimation();
                    this.<>f__this.fbxcomponent.stopAnimationCheck();
                    UnityEngine.Object.Destroy(this.<>f__this.actorObject);
                    UnityEngine.Object.Destroy(this.<>f__this.motionFSM[0]);
                    this.$current = 0;
                    this.$PC = 1;
                    goto Label_0207;

                case 1:
                    ServantAssetLoadManager.unloadServant(this.<>f__this.battleSvtData.svtId, this.<>f__this.battleSvtData.getDispLimitCount());
                    ServantAssetLoadManager.unloadWeaponGroupEffect(this.<>f__this.battleSvtData.getWeaponGroup(), this.<>f__this.battleSvtData.getEffectFolder());
                    this.$current = 0;
                    this.$PC = 2;
                    goto Label_0207;

                case 2:
                    this.<>f__this.battleSvtData.changeTransformServant();
                    ServantAssetLoadManager.preloadServant(this.<>f__this.battleSvtData.transformSvtId, this.<>f__this.battleSvtData.getDispLimitCount());
                    ServantAssetLoadManager.preloadActorMotion(this.<>f__this.battleSvtData.getWeaponGroup());
                    break;

                case 3:
                    break;

                case 4:
                    this.<>f__this.sendEventFSM("END_LOAD");
                    this.$PC = -1;
                    goto Label_0205;

                default:
                    goto Label_0205;
            }
            if (ServantAssetLoadManager.checkLoad())
            {
                this.$current = 0;
                this.$PC = 3;
            }
            else
            {
                this.<>f__this.setServantData(this.<>f__this.battleSvtData);
                this.<>f__this.setDir();
                this.<>f__this.setReloadWeaponMotion();
                this.<>f__this.battleSvtData.fixUpdateStatus();
                this.<>f__this.performance.data.transformSvtCommand(this.<>f__this.battleSvtData);
                this.<>f__this.performance.commandPerf.transformSvtFace(this.<>f__this.battleSvtData);
                this.$current = 0;
                this.$PC = 4;
            }
            goto Label_0207;
        Label_0205:
            return false;
        Label_0207:
            return true;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current =>
            this.$current;

        object IEnumerator.Current =>
            this.$current;
    }

    [CompilerGenerated]
    private sealed class <WaitToNoblePhantasmPlay>c__Iterator24 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal BattleActorControl <>f__this;
        internal NGUIFader <fadeComp>__0;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                    this.$current = new WaitForEndOfFrame();
                    this.$PC = 1;
                    return true;

                case 1:
                    this.<>f__this.performance.setDamageVoiceFlg(true);
                    this.<>f__this.performance.FlipAll(this.<>f__this.IsEnemy);
                    this.<>f__this.backupFov = this.<>f__this.performance.currentFov;
                    this.<>f__this.performance.setupCameraFov(BattlePerformance.DefaultFov);
                    this.<>f__this.performance.actorcamera.transform.localEulerAngles = Vector3.zero;
                    this.<fadeComp>__0 = this.<>f__this.performance.fadeObject.GetComponent<NGUIFader>();
                    this.<fadeComp>__0.FadeStart(Color.white, 0.3f, true, null, false);
                    SingletonMonoBehaviour<BattleSequenceManager>.Instance.play(false, false, new Action<USSequencer>(this.<>f__this.OnNoblePhantasmPlayComplete));
                    this.$PC = -1;
                    break;
            }
            return false;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current =>
            this.$current;

        object IEnumerator.Current =>
            this.$current;
    }

    private enum DIR
    {
        RIGHT,
        LEFT
    }

    private delegate void EndCallEvent(Hashtable hash);

    private class EventClass
    {
        public BattleActorControl.EndCallEvent eventcall;
        public Hashtable table;

        public void Add(string key, object obj)
        {
            if (this.table == null)
            {
                this.table = new Hashtable();
            }
            this.table.Add(key, obj);
        }

        public void Proc()
        {
            this.eventcall(this.table);
        }
    }

    public enum POS
    {
        FRONT,
        BACK,
        CENTER,
        CRITERIA,
        MYSTAGE,
        ENEMYSTAGE
    }
}

