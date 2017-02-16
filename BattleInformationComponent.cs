using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class BattleInformationComponent : BaseMonoBehaviour
{
    private string[] attackCount = new string[] { "1st Attack", "2nd Attack", "3rd Attack", "{0}th Attack" };
    public GameObject commonLabelPrefab;
    private GameObject commonMessageObject;
    public Transform commonMessageRoot;
    private BattleData data;
    public GameObject enemyAttackInfoPrefab;
    private bool isAlreadyOverKill;
    protected static readonly string NobleInfoAnimPrefix = "NobleLevel";
    public GameObject nobleInfoPrefab;
    public Transform nobleInfoRoot;
    public GameObject overKillMessagePrefab;
    private BattlePerformance perf;
    public GameObject playerAttackInfoPrefab;
    public GameObject skillInfoEnemyPrefab;
    public GameObject skillInfoPrefab;
    public Transform skillInfoRoot;
    private float timeStartOverKill;
    public GameObject totalLabelPrefab;
    public Transform totalMessageRoot;

    public void hideOverKillMessage()
    {
        if (this.isAlreadyOverKill)
        {
            Animation component = this.overKillMessagePrefab.GetComponent<Animation>();
            component.enabled = false;
            component.enabled = true;
            component.Play("overkill_out");
            this.isAlreadyOverKill = false;
        }
    }

    public void Initialize(BattlePerformance inperf, BattleData indata, BattleLogic inlogic)
    {
        this.perf = inperf;
        this.data = indata;
        if (this.playerAttackInfoPrefab != null)
        {
            this.playerAttackInfoPrefab.SetActive(false);
        }
        Vector3 zero = Vector3.zero;
        Vector3 up = Vector3.up;
        GameObject original = (GameObject) Resources.Load("effect/ef_overkill01");
        GameObject obj3 = (GameObject) UnityEngine.Object.Instantiate(original, zero, Quaternion.Euler(up));
        obj3.transform.parent = this.commonMessageRoot;
        obj3.transform.eulerAngles = up;
        obj3.transform.localScale = new Vector3(1f, 1f, 1f);
        obj3.SetActive(false);
        this.overKillMessagePrefab = obj3;
        this.isAlreadyOverKill = false;
        this.timeStartOverKill = 0f;
    }

    public void showCommonMessage(BattleActionData actionData)
    {
        if (this.commonMessageObject != null)
        {
            UnityEngine.Object.Destroy(this.commonMessageObject);
        }
        GameObject obj2 = null;
        if (!BattleCommand.isShowCommandAction(actionData.type) || !this.data.isPlayerID(actionData.actorId))
        {
            if (actionData.motionMessage != null)
            {
                if (0 < actionData.motionMessage.Length)
                {
                    obj2 = base.createObject(this.commonLabelPrefab, this.commonMessageRoot, null);
                    obj2.GetComponent<BattleInfoMessageComponent>().setText(actionData.motionMessage);
                    obj2.SetActive(true);
                }
            }
            else if (actionData.isSkill())
            {
                obj2 = this.showSkillName(this.data.isPlayerID(actionData.actorId), actionData.skillMessage);
            }
        }
        else
        {
            BattleCommand.TYPE type = BattleCommand.getType(actionData.type);
            if (this.data.isPlayerID(actionData.actorId))
            {
                obj2 = base.createObject(this.playerAttackInfoPrefab, this.commonMessageRoot, null);
            }
            BattleInfoMessageComponent component = obj2.GetComponent<BattleInfoMessageComponent>();
            if (this.data.isPlayerID(actionData.actorId))
            {
                string str = (actionData.actionIndex >= 3) ? string.Format(this.attackCount[3], actionData.actionIndex + 1) : this.attackCount[actionData.actionIndex];
                string str2 = string.Empty;
                switch (type)
                {
                    case BattleCommand.TYPE.QUICK:
                        str2 = "Quick!";
                        break;

                    case BattleCommand.TYPE.ARTS:
                        str2 = "Arts!";
                        break;

                    case BattleCommand.TYPE.BUSTER:
                        str2 = "Buster!";
                        break;

                    case BattleCommand.TYPE.ADDATTACK:
                        str2 = "Extra!";
                        break;
                }
                component.setText(str, str2);
                GameObject command = this.perf.commandPerf.getBattleCommandCardObject(actionData.actionIndex);
                if (command != null)
                {
                    component.setCommandObject(command, this.data.getServantData(actionData.targetId));
                    command.GetComponent<BattleCommandComponent>().hideOutCard();
                }
            }
            obj2.SetActive(true);
        }
        if (obj2 != null)
        {
            this.commonMessageObject = obj2;
        }
    }

    public void showCommonMessage(string message)
    {
    }

    public void showNoblePhantasmInfo(int tresureDeviceId, int treasureDeviceLevel, int treasureDevicePer = 500, bool isEnemy = false)
    {
        TreasureDvcEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<TreasureDvcMaster>(DataNameKind.Kind.TREASUREDEVICE).getEntityFromId<TreasureDvcEntity>(tresureDeviceId);
        if (((entity != null) && (entity.name != null)) && (entity.name.Length > 0))
        {
            GameObject obj2 = base.createObject(this.nobleInfoPrefab, this.nobleInfoRoot, null);
            UILabel component = obj2.transform.getNodeFromName("mainLabel", false).GetComponent<UILabel>();
            UILabel label2 = obj2.transform.getNodeFromName("rubyLabel", false).GetComponent<UILabel>();
            UILabel label3 = obj2.transform.getNodeFromName("LevelLabel", false).GetComponent<UILabel>();
            UILabel label4 = obj2.transform.getNodeFromName("PerLabel", false).GetComponent<UILabel>();
            GameObject gameObject = obj2.transform.getNodeFromName("NpLevelBase", false).gameObject;
            component.text = entity.name;
            label2.text = entity.ruby;
            label3.text = string.Empty + treasureDeviceLevel;
            label4.text = string.Format(LocalizationManager.Get("NOBLEINFO_NP_PER"), (treasureDevicePer / 100) * 100);
            gameObject.SetActive(!isEnemy);
            Animation animation = obj2.GetComponent<Animation>();
            int num = treasureDevicePer / 100;
            animation.Play(NobleInfoAnimPrefix + ((num >= 1) ? ((num <= 5) ? num : 5) : 1));
        }
    }

    public void showOverKillMessage(BattleActionData actionData)
    {
        if (this.data.isPlayerID(actionData.actorId))
        {
            this.overKillMessagePrefab.SetActive(true);
            Animation component = this.overKillMessagePrefab.GetComponent<Animation>();
            component.enabled = false;
            component.enabled = true;
            if (this.isAlreadyOverKill)
            {
                if ((Time.time - this.timeStartOverKill) > 0.2)
                {
                    component.Play("overkill_flash");
                }
            }
            else
            {
                component.Play("overkill_in");
                this.isAlreadyOverKill = true;
                this.timeStartOverKill = Time.time;
            }
        }
    }

    public GameObject showSkillName(bool isPlayer, string message)
    {
        GameObject obj2 = null;
        if ((message == null) || (message.Length <= 0))
        {
            return null;
        }
        if (isPlayer)
        {
            obj2 = base.createObject(this.skillInfoPrefab, this.skillInfoRoot, null);
        }
        else
        {
            obj2 = base.createObject(this.skillInfoEnemyPrefab, this.skillInfoRoot, null);
        }
        if (obj2 != null)
        {
            obj2.GetComponent<BattleInfoMessageComponent>().setText(message);
            obj2.SetActive(true);
        }
        return obj2;
    }

    public void showSpecialName(BattleActionData actionData)
    {
        GameObject obj2 = null;
        if (actionData != null)
        {
            TreasureDvcEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<TreasureDvcMaster>(DataNameKind.Kind.TREASUREDEVICE).getEntityFromId<TreasureDvcEntity>(actionData.treasureDvcId);
            if ((entity.getName() != null) && (entity.getName().Length > 0))
            {
                if (this.data.isPlayerID(actionData.actorId))
                {
                    obj2 = base.createObject(this.skillInfoPrefab, this.skillInfoRoot, null);
                }
                else
                {
                    obj2 = base.createObject(this.skillInfoEnemyPrefab, this.skillInfoRoot, null);
                }
                obj2.GetComponent<BattleInfoMessageComponent>().setText(entity.getName());
                obj2.SetActive(true);
            }
        }
    }

    public void showTotalDamage(BattleActionData actionData)
    {
        int num = actionData.getTotalDamage();
        if (num > 0)
        {
            UILabel[] componentsInChildren = base.createObject(this.totalLabelPrefab, this.totalMessageRoot, null).GetComponentsInChildren<UILabel>();
            if (componentsInChildren != null)
            {
                foreach (UILabel label in componentsInChildren)
                {
                    if (label.gameObject.name.Equals("line1"))
                    {
                        label.text = $"Total {num:#,0}";
                    }
                    else if (label.gameObject.name.Equals("line2"))
                    {
                    }
                }
            }
        }
    }
}

