using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class BattleCommandComponent : BaseMonoBehaviour
{
    private GameObject[] addObjectList = new GameObject[2];
    private int basedepth;
    public UISprite bg;
    public BattleServantBuffIconComponent[] buffIconList;
    public Transform buffRoot;
    public DrumRollLabel criticallabel;
    public GameObject criticalObject;
    private BattleCommandData data;
    public UISprite donotactSprite;
    public UISprite dontuseSprite;
    public GameObject effect_addcritical;
    public GameObject effect_cricomplete;
    public GameObject effect_fullcricomplete;
    private const int effectdepth = 7;
    private GameObject[] effectobj = new GameObject[6];
    public UITexture facetex;
    public UISprite friendIcon;
    public UISprite icon;
    private int index;
    public bool isDontAction;
    public bool isSealed;
    private bool newMatriarl;
    public UITexture nobletex;
    public UIWidget nomalwidget;
    public Transform objectRoot;
    private BattlePerformance perf;
    public UISprite sealedSprite;
    private bool selectflg;
    private Spawner spawner;
    private BattleServantData svtData;
    public UILabel svtId_label;
    private GameObject target;
    public UISprite text;
    public GameObject tr_criticaleffect;

    public void addCriticalBuff(GameObject obj)
    {
        if (this.spawner != null)
        {
            this.spawner.Despawn(obj, false);
        }
        else
        {
            UnityEngine.Object.Destroy(obj);
        }
        SoundManager.playSe("ba12");
        base.createObject(this.effect_addcritical, this.criticallabel.transform, null).addNguiDepth(this.basedepth + 7, false);
    }

    public void addFirstAura(GameObject effect)
    {
        this.effectobj[4] = base.createObject(effect, this.objectRoot, null);
    }

    public void addObject(string name, ADDOBJECT_TYPE type)
    {
        int index = (int) type;
        if (this.addObjectList[index] != null)
        {
            UnityEngine.Object.Destroy(this.addObjectList[index]);
        }
        GameObject obj2 = base.createObject("effect/" + name, this.objectRoot, null);
        obj2.transform.localRotation = Quaternion.identity;
        obj2.name = type.ToString();
        this.addObjectList[index] = obj2;
        this.addObjectList[index].SetActive(false);
    }

    public void attachEffect(string name, int index)
    {
        GameObject obj2 = base.createObject("effect/" + name, this.objectRoot, null);
        obj2.transform.localPosition = Vector3.zero;
        obj2.transform.localScale = Vector3.one;
        if (index < this.effectobj.Length)
        {
            this.effectobj[index] = obj2;
        }
    }

    public bool checkMark(int mark)
    {
        if (this.data == null)
        {
            return false;
        }
        return (mark == this.data.markindex);
    }

    public bool checkObject() => 
        (this.data != null);

    public void compCriticallabel()
    {
        if (this.data.checkCriticalMax())
        {
            base.createObject(this.effect_fullcricomplete, this.tr_criticaleffect.transform, null).addNguiDepth(this.basedepth + 7, true);
        }
        else
        {
            base.createObject(this.effect_cricomplete, this.tr_criticaleffect.transform, null).addNguiDepth(this.basedepth + 7, true);
        }
    }

    private void DestroyEffectObject1()
    {
        if (this.effectobj[1] != null)
        {
            if (this.newMatriarl)
            {
                UnityEngine.Object.Destroy(this.effectobj[1].GetComponentInChildren<UITexture>().material);
                this.newMatriarl = false;
            }
            UnityEngine.Object.Destroy(this.effectobj[1]);
            this.effectobj[1] = null;
        }
    }

    public void endFashSvt()
    {
        this.DestroyEffectObject1();
    }

    public void endFashTypeCard()
    {
        if (this.effectobj[0] != null)
        {
            UnityEngine.Object.Destroy(this.effectobj[0]);
        }
    }

    [DebuggerHidden]
    private IEnumerator fadeoutEffect(bool isTresure = false) => 
        new <fadeoutEffect>c__Iterator1A { 
            isTresure = isTresure,
            <$>isTresure = isTresure,
            <>f__this = this
        };

    public void flashComboSvt(int index, BattleComboData combo)
    {
        if (2 < index)
        {
            this.flashServant();
        }
        else if (combo.sameflg[index])
        {
            this.flashServant();
        }
    }

    public void flashComboType(int index, BattleComboData combo)
    {
        if (combo.flash)
        {
            this.flashTypeCard();
        }
    }

    public void flashServant()
    {
        this.DestroyEffectObject1();
        if (this.facetex != null)
        {
            this.effectobj[1] = base.createObject(this.facetex.gameObject, this.nomalwidget.gameObject.transform.parent, null);
            UITexture componentInChildren = this.effectobj[1].GetComponentInChildren<UITexture>();
            componentInChildren.gameObject.transform.localPosition = new Vector3(0f, 0f, -5f);
            this.newMatriarl = true;
            componentInChildren.material = new Material(componentInChildren.shader);
            componentInChildren.material.SetColor("_AddColor", new Color(1f, 1f, 1f, 0f));
            componentInChildren.depth = this.basedepth + 400;
            object[] args = new object[] { "scale", new Vector3(1.8f, 1.8f), "time", 1f, "oncompletetarget", base.gameObject, "oncomplete", "endFashSvt" };
            iTween.ScaleTo(this.effectobj[1], iTween.Hash(args));
            TweenColor.Begin(this.effectobj[1], 1f, new Color(0f, 0f, 0f, 0f));
        }
    }

    public void flashType(bool flg)
    {
        this.flashTypeCard();
        if (flg)
        {
            base.StartCoroutine(this.fadeoutEffect(false));
        }
    }

    public void flashTypeCard()
    {
        if (this.effectobj[0] != null)
        {
            UnityEngine.Object.Destroy(this.effectobj[0]);
        }
        this.effectobj[0] = base.createObject(this.icon.gameObject, this.nomalwidget.gameObject.transform.parent, this.icon.gameObject.transform);
        UISprite componentInChildren = this.effectobj[0].GetComponentInChildren<UISprite>();
        if (componentInChildren != null)
        {
            componentInChildren.depth = this.basedepth + 400;
            TweenColor.Begin(this.effectobj[0], 1f, new Color(0f, 0f, 0f, 0f));
        }
        object[] args = new object[] { "scale", new Vector3(2f, 2f), "time", 1f, "oncompletetarget", base.gameObject, "oncomplete", "endFashTypeCard" };
        iTween.ScaleTo(this.effectobj[0], iTween.Hash(args));
    }

    public BattleCommandData getcommandData() => 
        this.data;

    public int getCommandType() => 
        this.data.getCommandType();

    public int getCriticalCount()
    {
        int num = this.data.getCriticalPoint();
        if (0 < num)
        {
            num /= 10;
            if (num <= 0)
            {
                num = 1;
            }
        }
        return num;
    }

    public int getMarkIndex()
    {
        if (this.data == null)
        {
            return -1;
        }
        return this.data.markindex;
    }

    public int getSvtId() => 
        this.data.getServantId();

    public int getSvtLimitCount() => 
        this.data.getServantLimitCount();

    public int getUniqueID() => 
        this.data.getUniqueId();

    public void hideOutCard()
    {
        this.nomalwidget.alpha = 0f;
        this.DestroyEffectObject1();
        for (int i = 0; i < this.addObjectList.Length; i++)
        {
            if (this.addObjectList[i] != null)
            {
                this.addObjectList[i].SetActive(false);
            }
        }
    }

    public void initView()
    {
        this.sealedSprite.gameObject.SetActive(false);
        this.donotactSprite.gameObject.SetActive(false);
    }

    public bool isSelect() => 
        this.selectflg;

    public bool isTreasureDvc()
    {
        if (this.data == null)
        {
            return false;
        }
        return (0 < this.data.treasureDvc);
    }

    public void OnClickEvent()
    {
        if ((this.target != null) && (this.data != null))
        {
            this.target.SendMessage("touchCommandCard", this.data.markindex);
        }
    }

    public void OnLongPressEvent(UnityEngine.Object obj)
    {
        if ((this.target != null) && (this.data != null))
        {
            this.target.SendMessage("LongPress", this.data.markindex);
        }
    }

    public void openCard()
    {
        if (this.data.starcount <= 0)
        {
            this.setCriticalObject(false);
        }
    }

    public void playAddAttackEffect(bool flg)
    {
        this.flashServant();
        if (flg)
        {
            base.StartCoroutine(this.fadeoutEffect(false));
        }
    }

    public void playAttackEffect(bool flg)
    {
        this.flashTypeCard();
        this.flashServant();
        if (flg)
        {
            base.StartCoroutine(this.fadeoutEffect(false));
        }
    }

    public void playNpAttackEffect(float ftime)
    {
        object[] args = new object[] { "x", 2f, "y", 2f, "time", ftime + 0.1f };
        iTween.ScaleTo(base.gameObject, iTween.Hash(args));
        object[] objArray2 = new object[] { "z", 720f, "time", ftime };
        iTween.RotateTo(base.gameObject, iTween.Hash(objArray2));
        base.StartCoroutine(this.fadeoutEffect(true));
    }

    public void playOpenNobleCard()
    {
        this.effectobj[2] = base.createObject("effect/ef_noblecard", this.nomalwidget.transform, null);
    }

    public void resetAddObject()
    {
        ADDOBJECT_TYPE[] addobject_typeArray1 = new ADDOBJECT_TYPE[2];
        addobject_typeArray1[1] = ADDOBJECT_TYPE.ARROW_WEAK;
        ADDOBJECT_TYPE[] addobject_typeArray = addobject_typeArray1;
        foreach (ADDOBJECT_TYPE addobject_type in addobject_typeArray)
        {
            Transform transform = this.objectRoot.getNodeFromName(addobject_type.ToString(), true);
            if (transform != null)
            {
                this.addObjectList[(int) addobject_type] = transform.gameObject;
                this.addObjectList[(int) addobject_type].SetActive(false);
            }
        }
    }

    public void resetComboData()
    {
        this.DestroyEffectObject1();
        foreach (GameObject obj2 in this.effectobj)
        {
            if (obj2 != null)
            {
                UnityEngine.Object.Destroy(obj2);
            }
        }
    }

    public void resetSelect()
    {
        ServantAssetLoadManager.StopVoice(this.data.uniqueId);
        if (this.facetex != null)
        {
            this.facetex.color = Color.white;
        }
        if (this.bg != null)
        {
            this.bg.color = Color.white;
        }
        this.resetSelectStamp();
        if (this.effectobj[3] != null)
        {
            UnityEngine.Object.Destroy(this.effectobj[3]);
        }
    }

    public void resetSelectStamp()
    {
        if (this.effectobj[5] != null)
        {
            UnityEngine.Object.Destroy(this.effectobj[5]);
        }
    }

    public void selectCard(int targetIndex)
    {
        if (this.facetex != null)
        {
            this.facetex.color = Color.gray;
        }
        if (this.bg != null)
        {
            this.bg.color = Color.gray;
        }
        this.stopFirstAura();
        if (!this.data.isBlank() && !this.isDontAction)
        {
            Voice.BATTLE[] battleArray;
            bool flag = this.data.treasureDvc > 0;
            int index = UnityEngine.Random.Range(0, 3);
            if (!flag)
            {
                battleArray = new Voice.BATTLE[] { Voice.BATTLE.CARD1, Voice.BATTLE.CARD2, Voice.BATTLE.CARD3 };
            }
            else
            {
                battleArray = new Voice.BATTLE[] { Voice.BATTLE.HCARD1, Voice.BATTLE.HCARD2, Voice.BATTLE.HCARD3 };
            }
            if (ServantAssetLoadManager.GetLastVoiceType(this.data.uniqueId) == battleArray[index])
            {
                index++;
                index = index % 3;
            }
            for (int i = 0; i < 3; i++)
            {
                if (ServantAssetLoadManager.ExistsBattleVoice(this.data.getServantId(), this.data.svtlimit, battleArray[index]))
                {
                    int condValue = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<VoiceMaster>(DataNameKind.Kind.VOICE).getFlagRequestNumber(this.data.getServantId(), "_" + Voice.getFileName(battleArray[index]), false);
                    if ((condValue != 0) && (this.svtData.followerType == Follower.Type.NONE))
                    {
                        this.perf.data.AddServantVoicePlayed(this.data.getServantId(), condValue);
                    }
                    ServantAssetLoadManager.playBattleVoice(this.data.getServantId(), this.data.svtlimit, battleArray[index], 1f, null, this.data.uniqueId);
                    break;
                }
                index++;
                index = index % 3;
            }
            string format = "effect/ef_commandup_{0}{1:00}";
            if (this.data.isQuick())
            {
                format = string.Format(format, "q", targetIndex + 1);
            }
            else if (this.data.isArts())
            {
                format = string.Format(format, "a", targetIndex + 1);
            }
            else if (this.data.isBuster())
            {
                format = string.Format(format, "b", targetIndex + 1);
            }
            this.effectobj[3] = base.createObject(format, this.nomalwidget.transform, null);
        }
    }

    public void setAttackCommandData(BattleCommandComponent comp)
    {
        this.setData(comp.data, null);
        this.setPerf(comp.perf);
    }

    public void setBuffIconList(BattleBuffData buffData)
    {
        if ((buffData == null) || (this.data == null))
        {
            this.buffRoot.gameObject.SetActive(false);
        }
        else
        {
            this.buffRoot.gameObject.SetActive(true);
            BuffList.TYPE[] types = null;
            BattleBuffData.BuffData[] collection = null;
            List<BattleBuffData.BuffData> list = new List<BattleBuffData.BuffData>();
            types = new BuffList.TYPE[] { BuffList.TYPE.UP_COMMANDALL, BuffList.TYPE.UP_COMMANDATK, BuffList.TYPE.UP_COMMANDNP, BuffList.TYPE.UP_COMMANDSTAR };
            collection = buffData.getBuffList(types, BattleCommand.getIndividuality(this.data.getCommandType(), 1), buffData.getActiveList());
            list.AddRange(collection);
            collection = buffData.getBuffList(types, BattleCommand.getIndividuality(this.data.getCommandType(), 1), buffData.getShowBuffListPassive());
            list.AddRange(collection);
            if (this.data.isTreasureDvc())
            {
                types = new BuffList.TYPE[] { BuffList.TYPE.UP_NPDAMAGE, BuffList.TYPE.DOWN_NPDAMAGE };
                collection = buffData.getBuffList(types, null, buffData.getActiveList());
                list.AddRange(collection);
            }
            types = new BuffList.TYPE[] { BuffList.TYPE.UP_ATK, BuffList.TYPE.DOWN_ATK, BuffList.TYPE.UP_DAMAGE, BuffList.TYPE.DOWN_DAMAGE, BuffList.TYPE.ADD_DAMAGE, BuffList.TYPE.SUB_DAMAGE, BuffList.TYPE.UP_CRITICALDAMAGE, BuffList.TYPE.DOWN_CRITICALDAMAGE, BuffList.TYPE.BREAK_AVOIDANCE };
            collection = buffData.getBuffList(types, null, buffData.getShowBuffList());
            list.AddRange(collection);
            List<int> list2 = new List<int>();
            BuffMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<BuffMaster>(DataNameKind.Kind.BUFF);
            foreach (BattleBuffData.BuffData data in list)
            {
                BuffEntity entity = master.getEntityFromId<BuffEntity>(data.buffId);
                if (!list2.Contains(entity.iconId))
                {
                    list2.Add(entity.iconId);
                }
            }
            for (int i = 0; i < this.buffIconList.Length; i++)
            {
                if (i < list2.Count)
                {
                    this.buffIconList[i].setImageId(list2[i]);
                }
                else
                {
                    this.buffIconList[i].setImageId(0);
                }
            }
        }
    }

    public void setCriticalObject(bool flg)
    {
        this.criticalObject.SetActive(flg);
    }

    public void setData(BattleCommandData indata, BattleServantData insvtData = null)
    {
        this.data = indata;
        this.svtData = insvtData;
        this.selectflg = false;
        if (this.effectobj != null)
        {
            this.DestroyEffectObject1();
            foreach (GameObject obj2 in this.effectobj)
            {
                if (obj2 != null)
                {
                    UnityEngine.Object.Destroy(obj2);
                }
            }
        }
        this.effectobj = new GameObject[this.effectobj.Length];
        this.updateView(true);
    }

    public void setDepth(int basedepth)
    {
        this.bg.depth = basedepth + 1;
        this.facetex.depth = basedepth + 2;
        this.icon.depth = basedepth + 3;
        this.text.depth = basedepth + 4;
        this.nobletex.depth = basedepth + 5;
        this.sealedSprite.depth = basedepth + 9;
        this.donotactSprite.depth = basedepth + 9;
        this.criticalObject.addNguiDepth(basedepth + 7, true);
        this.friendIcon.depth = basedepth + 10;
        foreach (BattleServantBuffIconComponent component in this.buffIconList)
        {
            component.iconSprite.depth = basedepth + 11;
        }
        this.basedepth = basedepth;
    }

    public void setMoveMode()
    {
        this.stopFirstAura();
        this.resetSelectStamp();
    }

    public void setPerf(BattlePerformance inPerf)
    {
        this.perf = inPerf;
    }

    public void setSelect(bool flg)
    {
        this.selectflg = flg;
    }

    public void setSelectStamp(GameObject stamp)
    {
        this.effectobj[5] = stamp;
    }

    public void setShader(string shaderName)
    {
        if (this.facetex != null)
        {
            this.facetex.material = new Material(Shader.Find(shaderName));
        }
    }

    public void setTarget(GameObject target)
    {
        this.target = target;
    }

    public void setTouchFlg(bool flg)
    {
        Collider component = base.GetComponent<Collider>();
        if (component != null)
        {
            component.enabled = flg;
        }
    }

    private void Start()
    {
        this.spawner = SingletonMonoBehaviour<Spawner>.Instance;
        if (this.spawner != null)
        {
            this.spawner.Precache(this.effect_addcritical, 1);
            this.spawner.Precache(this.effect_cricomplete, 1);
            this.spawner.Precache(this.effect_fullcricomplete, 1);
        }
    }

    public void startComboCard()
    {
        if (this.facetex != null)
        {
            this.facetex.color = Color.white;
        }
        if (this.bg != null)
        {
            this.bg.color = Color.white;
        }
        this.resetSelectStamp();
    }

    public void startCountUp()
    {
        if (this.data.starcount <= 0)
        {
            this.setCriticalObject(false);
        }
        else
        {
            this.setCriticalObject(true);
            this.criticallabel.setParam(0);
            this.criticallabel.changeParam(this.data.getCriticalPoint(), new DrumRollLabel.CompleteEventHandler(this.compCriticallabel));
        }
    }

    public void startMoveFloat()
    {
        Animation component = base.GetComponent<Animation>();
        component.enabled = false;
        component.enabled = true;
        component["anim_commandfloat"].time = UnityEngine.Random.Range((float) 0f, (float) 0.2f);
        component.Play("anim_commandfloat");
    }

    public void stopAnimation()
    {
        base.GetComponent<Animation>().Stop();
        this.nomalwidget.gameObject.transform.localPosition = Vector3.zero;
        if (this.effectobj[2] != null)
        {
            UnityEngine.Object.Destroy(this.effectobj[2]);
        }
    }

    public void stopFirstAura()
    {
        if (this.effectobj[4] != null)
        {
            UnityEngine.Object.Destroy(this.effectobj[4]);
        }
    }

    public void transformSvtFace(BattleServantData svtData)
    {
        if ((this.data != null) && (svtData.getUniqueID() == this.data.getUniqueId()))
        {
            this.data.svtId = svtData.getSvtId();
            this.data.svtlimit = svtData.getLimitImageIndex();
            this.data.loadSvtLimit = svtData.getLimitImageIndex();
            if (this.data.isBlank())
            {
                this.facetex.enabled = false;
            }
            else
            {
                this.facetex = ServantAssetLoadManager.loadCommandCard(this.facetex, this.data.svtId, this.data.loadSvtLimit, this.data.svtlimit);
                if (this.facetex != null)
                {
                    this.facetex.enabled = true;
                    this.facetex.height = 0xbf;
                    this.facetex.width = 0xbf;
                }
            }
        }
    }

    public void updateClassMag(int targetClass)
    {
        if (((this.data != null) && (targetClass != -1)) && ((this.addObjectList[0] != null) && (this.addObjectList[1] != null)))
        {
            BattleServantData data = this.perf.data.getServantData(this.data.getUniqueId());
            this.addObjectList[0].SetActive(false);
            this.addObjectList[1].SetActive(false);
            if (data != null)
            {
                float num = SvtClassAttri.getMagnification(data.getClassId(), targetClass);
                if (1f < num)
                {
                    this.addObjectList[1].SetActive(true);
                }
                else if (num < 1f)
                {
                    this.addObjectList[0].SetActive(true);
                }
            }
        }
    }

    public void updateView(bool initFlg = true)
    {
        if (this.data == null)
        {
            base.gameObject.SetActive(false);
        }
        else
        {
            string format = "card_bg_{0}";
            string str2 = "none";
            base.transform.localScale = Vector3.one;
            if (initFlg)
            {
                this.nomalwidget.alpha = 1f;
            }
            base.gameObject.SetActive(true);
            this.setCriticalObject(false);
            this.setBuffIconList(null);
            if (this.data.isBlank())
            {
                this.facetex.enabled = false;
                this.icon.enabled = false;
                this.text.enabled = false;
                this.bg.spriteName = "card_bg_blank";
                this.bg.height = 170;
                this.bg.width = 0x85;
                this.friendIcon.enabled = false;
            }
            else
            {
                this.facetex = ServantAssetLoadManager.loadCommandCard(this.facetex, this.data.svtId, this.data.loadSvtLimit, this.data.svtlimit);
                this.icon.enabled = true;
                this.text.enabled = true;
                if (this.facetex != null)
                {
                    this.facetex.enabled = true;
                }
                if (0 < this.data.treasureDvc)
                {
                    if (this.facetex != null)
                    {
                        this.facetex.height = 0xbf;
                        this.facetex.width = 0xbf;
                    }
                    format = "card_bg_np{0}";
                }
                else if (this.facetex != null)
                {
                    this.facetex.height = 0xbf;
                    this.facetex.width = 0xbf;
                }
                if (this.data.isBuster())
                {
                    str2 = "buster";
                }
                else if (this.data.isQuick())
                {
                    str2 = "quick";
                }
                else if (this.data.isArts())
                {
                    str2 = "arts";
                }
                else if (this.data.isAddAttack())
                {
                    str2 = "extra";
                }
                this.friendIcon.enabled = false;
                if (this.data.getFollowerType() != Follower.Type.NONE)
                {
                    this.friendIcon.spriteName = FileName.friendIconName;
                    this.friendIcon.enabled = true;
                }
                if (this.data.flgEventJoin)
                {
                    this.friendIcon.spriteName = FileName.eventJoinIconName;
                    this.friendIcon.enabled = true;
                }
                this.bg.spriteName = string.Format(format, str2);
                this.icon.spriteName = $"card_icon_{str2}";
                this.text.spriteName = $"card_txt_{str2}";
                this.text.SetRect(0f, 0f, (float) this.text.GetAtlasSprite().width, (float) this.text.GetAtlasSprite().height);
                this.text.gameObject.transform.localPosition = Vector3.zero;
                this.isSealed = false;
                this.isDontAction = false;
                if (0 < this.data.treasureDvc)
                {
                    this.text.enabled = false;
                    if (this.nobletex != null)
                    {
                        this.nobletex.gameObject.SetActive(false);
                    }
                    this.nobletex = ServantAssetLoadManager.loadNobleName(this.nobletex, this.data.svtId, this.data.svtlimit, this.data.treasureDvc);
                    if (this.nobletex != null)
                    {
                        this.nobletex.gameObject.SetActive(true);
                    }
                    this.sealedSprite.gameObject.SetActive(false);
                    this.dontuseSprite.gameObject.SetActive(false);
                    if (this.svtData != null)
                    {
                        if (this.svtData.isTDSeraled())
                        {
                            this.isSealed = true;
                            if (this.svtData.isHeroine())
                            {
                                this.sealedSprite.gameObject.SetActive(true);
                            }
                            else
                            {
                                this.dontuseSprite.gameObject.SetActive(true);
                            }
                        }
                        else if (!this.svtData.isNobleAction())
                        {
                            this.isSealed = true;
                            this.sealedSprite.gameObject.SetActive(true);
                        }
                    }
                }
                if (this.svtData != null)
                {
                    if (this.svtData.isAction())
                    {
                        this.donotactSprite.gameObject.SetActive(false);
                    }
                    else
                    {
                        this.isDontAction = true;
                        this.sealedSprite.gameObject.SetActive(false);
                        this.dontuseSprite.gameObject.SetActive(false);
                        this.donotactSprite.gameObject.SetActive(true);
                    }
                }
                else
                {
                    this.sealedSprite.gameObject.SetActive(false);
                    this.donotactSprite.gameObject.SetActive(false);
                    this.dontuseSprite.gameObject.SetActive(false);
                }
            }
            this.setTouchFlg(true);
            if (this.svtId_label != null)
            {
                this.svtId_label.text = string.Empty + this.data.getServantId();
            }
        }
    }

    [CompilerGenerated]
    private sealed class <fadeoutEffect>c__Iterator1A : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal bool <$>isTresure;
        internal BattleCommandComponent <>f__this;
        internal bool isTresure;

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
                    this.$current = new WaitForSeconds(0.6f);
                    this.$PC = 1;
                    return true;

                case 1:
                    this.<>f__this.nomalwidget.alpha = 0f;
                    this.<>f__this.DestroyEffectObject1();
                    if (!this.isTresure)
                    {
                        this.<>f__this.effectobj[1] = this.<>f__this.createObject("effect/ef_commandburn_s01", this.<>f__this.gameObject.transform, null);
                        this.<>f__this.effectobj[1].transform.parent = this.<>f__this.gameObject.transform.parent;
                        this.<>f__this.effectobj[1].transform.localScale = Vector3.one;
                    }
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

    public enum ADDOBJECT_TYPE
    {
        ARROW_RESIST,
        ARROW_WEAK,
        MAX
    }
}

