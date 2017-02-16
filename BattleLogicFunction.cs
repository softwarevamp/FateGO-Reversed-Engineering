using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class BattleLogicFunction
{
    public BattleData data;
    public Dictionary<int, bool> lastFuncResult = new Dictionary<int, bool>();
    public BattleLogic logic;
    public BattleLogicTarget logictarget;
    public FunctionMaster master;

    public bool checkFuncAction(int actorId, int targetId, BuffEntity buffEnt, FunctionEntity funcEnt, DataVals baseVals)
    {
        BattleServantData data = this.data.getServantData(actorId);
        BattleServantData targetSvt = this.data.getServantData(targetId);
        int num = 0x3e8;
        int num2 = 0x3e8;
        int param = baseVals.GetParam(DataVals.TYPE.Rate);
        if (param < 0)
        {
            if (!this.lastFuncResult.ContainsKey(targetId))
            {
                this.DebugLog("checkFuncAction firstResult:false");
                this.lastFuncResult[targetId] = false;
                return false;
            }
            if (!this.lastFuncResult[targetId])
            {
                this.DebugLog("checkFuncAction lastFuncResult:false");
                this.lastFuncResult[targetId] = false;
                return false;
            }
            param = Mathf.Abs(param);
        }
        if (FuncList.TYPE.ADD_STATE.Check(funcEnt.funcType))
        {
            if (buffEnt != null)
            {
                num = Mathf.FloorToInt(BattleRandom.getNext(0x3e8) + (targetSvt.getBuffTOLERANCEMagnification(buffEnt.getIndividuality()) * 1000f));
                if (data != null)
                {
                    num2 = Mathf.FloorToInt(param + (data.getBuffGRANTSTATEMagnification(buffEnt.getIndividuality()) * 1000f));
                }
                else
                {
                    num2 = param;
                }
            }
        }
        else if (FuncList.TYPE.INSTANT_DEATH.Check(funcEnt.funcType))
        {
            if (actorId == targetId)
            {
                num = 0;
                num2 = 0x3e8;
            }
            else
            {
                num = BattleRandom.getNext(0x3e8);
                if (targetSvt.checkAvoidInstantDeath(targetSvt))
                {
                    num = 0x3e8;
                    num2 = 0;
                }
                else
                {
                    num2 = Mathf.FloorToInt((param * targetSvt.getDeathRate()) * (1f - (targetSvt.getBuffResistInstantDeath(targetSvt) - targetSvt.getBuffNonResistInstantDeath(targetSvt))));
                }
            }
        }
        else
        {
            num = BattleRandom.getNext(0x3e8);
            num2 = param;
        }
        this.DebugLog(string.Concat(new object[] { "checkFuncAction(", num, " < ", num2 }));
        bool flag = num < num2;
        this.lastFuncResult[targetId] = flag;
        return flag;
    }

    protected void DebugLog(string str)
    {
    }

    public BattleActionData functionAddState(int actorId, int targetId, BuffEntity buffEnt, FunctionEntity funcEnt, DataVals baseVals, int funcIndex, bool passive, bool shortbuff)
    {
        BattleActionData data = new BattleActionData();
        this.DebugLog("functionAddState=>");
        BattleServantData data2 = this.data.getServantData(actorId);
        BattleServantData data3 = this.data.getServantData(targetId);
        BattleActionData.BuffData data4 = new BattleActionData.BuffData {
            targetId = data3.getUniqueID(),
            functionIndex = funcIndex
        };
        int param = baseVals.GetParam(DataVals.TYPE.Rate);
        if (param < 0)
        {
            if (this.lastFuncResult.ContainsKey(targetId))
            {
                if (this.lastFuncResult[targetId])
                {
                    param = Mathf.Abs(param);
                }
            }
            else
            {
                this.lastFuncResult[targetId] = false;
            }
        }
        int num2 = Mathf.FloorToInt(BattleRandom.getNext(0x3e8) + (data3.getBuffTOLERANCEMagnification(buffEnt.getIndividuality()) * 1000f));
        int num3 = 0x3e8;
        if (data2 != null)
        {
            num3 = Mathf.FloorToInt(param + (data2.getBuffGRANTSTATEMagnification(buffEnt.getIndividuality()) * 1000f));
        }
        else
        {
            num3 = Mathf.FloorToInt((float) param);
        }
        this.DebugLog("defRate" + ((float) baseVals.GetParam(DataVals.TYPE.Rate)));
        this.DebugLog(string.Concat(new object[] { "check--Rate( ", num2, " < ", num3 }));
        this.DebugLog("check--Group( " + buffEnt.buffGroup);
        if (buffEnt.isCheckGroup() && data3.checkBuffGroup(buffEnt.buffGroup))
        {
            this.DebugLog("is same Group ");
            return this.getMissObject(data3.getUniqueID(), funcIndex, baseVals);
        }
        if ((param < 0) && (!this.lastFuncResult.ContainsKey(targetId) || !this.lastFuncResult[targetId]))
        {
            num3 = -1000;
        }
        if (num3 < num2)
        {
            this.lastFuncResult[targetId] = false;
            this.DebugLog("is rate over " + num3);
            return this.getMissObject(data3.getUniqueID(), funcIndex, baseVals);
        }
        this.lastFuncResult[targetId] = true;
        BattleBuffData.BuffData buff = new BattleBuffData.BuffData {
            buffId = buffEnt.id,
            turn = baseVals.GetParam(DataVals.TYPE.Turn) * 2
        };
        if (shortbuff)
        {
            buff.turn--;
        }
        buff.count = baseVals.GetParam(DataVals.TYPE.Count);
        buff.param = baseVals.GetParam(DataVals.TYPE.Value);
        buff.paramAdd = baseVals.GetParam(DataVals.TYPE.ParamAdd);
        buff.paramMax = baseVals.GetParam(DataVals.TYPE.ParamMax);
        buff.vals = new int[] { baseVals.GetParam(DataVals.TYPE.Value), baseVals.GetParam(DataVals.TYPE.Value2) };
        if (buffEnt.checkBuffType(BuffList.TYPE.REGAIN_HP) && (data2 != null))
        {
            float num4 = data2.getUpDownGiveHeal();
            buff.param = Mathf.FloorToInt(buff.param * num4);
            buff.paramAdd = Mathf.FloorToInt(buff.paramAdd * num4);
            buff.paramMax = Mathf.FloorToInt(buff.paramMax * num4);
        }
        if ((buffEnt.isDeadAct() || buffEnt.isEndAct()) && (SingletonMonoBehaviour<DataManager>.Instance.getMasterData<SkillLvMaster>(DataNameKind.Kind.SKILL_LEVEL).getEntityFromId<SkillLvEntity>(buff.vals[0], buff.vals[1]) == null))
        {
            throw new Exception($"指定されたスキルID,Levelが存在しません ID,LV = {buff.vals[0]},{buff.vals[1]}");
        }
        if (buffEnt.isEndSelfTurn())
        {
            this.DebugLog("isEndSelfTurn()***************************************************************");
        }
        buff.isUse = false;
        buff.passive = passive;
        buff.buffRate = baseVals.GetParam(DataVals.TYPE.UseRate);
        if (buff.buffRate == 0)
        {
            buff.buffRate = 0x3e8;
        }
        if (BuffList.CheckUpdateHp(buffEnt.type))
        {
            data4.procType = BattleActionData.BuffData.BuffProcType.UPDATE_HP;
            data3.addBuff(buff, true);
        }
        else
        {
            data3.addBuff(buff, false);
        }
        data4.buffId = buff.buffId;
        data4.popLabel = funcEnt.popupText;
        data4.popIcon = funcEnt.popupIconId;
        data4.popColor = funcEnt.popupTextColor;
        data4.effectList = funcEnt.getEffectList();
        data.setBuffData(data4);
        return data;
    }

    public BattleActionData functionDamage(int playerId, int targetId, DataVals baseVals, int funcIndex)
    {
        BattleActionData data = new BattleActionData();
        BattleServantData actor = this.data.getServantData(playerId);
        BattleServantData target = this.data.getServantData(targetId);
        BattleActionData.DamageData data4 = this.logic.getFunctionDamagelist(actor, target, baseVals.GetValue(), funcIndex);
        data.setDamageData(data4);
        return data;
    }

    public BattleActionData functionDelayNpTurn(int targetId, FunctionEntity funcEnt, DataVals baseVals, int funcIndex)
    {
        BattleActionData data = new BattleActionData();
        BattleServantData data2 = this.data.getServantData(targetId);
        if (!data2.isEnemy)
        {
            return null;
        }
        int val = baseVals.GetValue();
        data2.updownNextTDTurn(val);
        BattleActionData.BuffData data3 = this.getFunctionObject(funcEnt, data2.getUniqueID(), funcIndex);
        data3.procType = BattleActionData.BuffData.BuffProcType.UPDATE_NPTURN;
        data.setBuffData(data3);
        return data;
    }

    public BattleActionData functionGainHp(int playerId, int targetId, FunctionEntity funcEnt, DataVals baseVals, int funcIndex)
    {
        BattleActionData data = new BattleActionData();
        BattleServantData data2 = this.data.getServantData(playerId);
        BattleServantData data3 = this.data.getServantData(targetId);
        int healPoint = baseVals.GetValue();
        if (data2 != null)
        {
            Debug.LogError("actionSvtData:" + data2.getUpDownGiveHeal());
            healPoint = Mathf.FloorToInt(healPoint * data2.getUpDownGiveHeal());
        }
        int digit = 1;
        healPoint = (healPoint * data3.getUpDownHeal(out digit)) / digit;
        if (data3.buffData.checkNoHeal())
        {
            healPoint = 0;
        }
        data.setHealData(targetId, healPoint, funcIndex, 0);
        this.DebugLog("functionGainHp:" + healPoint);
        if (0 < healPoint)
        {
            BattleActionData.BuffData data4 = this.getFunctionObject(funcEnt, data3.getUniqueID(), funcIndex);
            data.setBuffData(data4);
            return data;
        }
        data.addAction(this.getMissObject(data3.getUniqueID(), funcIndex, baseVals));
        return data;
    }

    public BattleActionData functionGainHpPer(int playerId, int targetId, FunctionEntity funcEnt, DataVals baseVals, int funcIndex)
    {
        BattleActionData data = new BattleActionData();
        BattleServantData data2 = this.data.getServantData(playerId);
        BattleServantData data3 = this.data.getServantData(targetId);
        int num = baseVals.GetValue();
        if (data2 != null)
        {
            num = Mathf.FloorToInt(num * data2.getUpDownGiveHeal());
        }
        int digit = 1;
        num = (num * data3.getUpDownHeal(out digit)) / digit;
        int healPoint = (data3.getMaxHp() * num) / 0x3e8;
        if (data3.buffData.checkNoHeal())
        {
            healPoint = 0;
        }
        data.setHealData(targetId, healPoint, funcIndex, 0);
        if (0 < healPoint)
        {
            BattleActionData.BuffData data4 = this.getFunctionObject(funcEnt, data3.getUniqueID(), funcIndex);
            data.setBuffData(data4);
            return data;
        }
        data.addAction(this.getMissObject(data3.getUniqueID(), funcIndex, baseVals));
        return data;
    }

    public BattleActionData functionHastenNpTurn(int targetId, FunctionEntity funcEnt, DataVals baseVals, int funcIndex)
    {
        BattleActionData data = new BattleActionData();
        BattleServantData data2 = this.data.getServantData(targetId);
        if (!data2.isEnemy)
        {
            return null;
        }
        int num = baseVals.GetValue();
        data2.updownNextTDTurn(-num);
        BattleActionData.BuffData data3 = this.getFunctionObject(funcEnt, data2.getUniqueID(), funcIndex);
        data3.procType = BattleActionData.BuffData.BuffProcType.UPDATE_NPTURN;
        data.setBuffData(data3);
        return data;
    }

    public BattleActionData functionInstantDeath(int playerId, int targetId, FunctionEntity funcEnt, DataVals baseVals, int funcIndex)
    {
        this.DebugLog("functionInstantDeath>>:" + targetId);
        BattleActionData data = new BattleActionData();
        BattleServantData data2 = this.data.getServantData(targetId);
        data2.reducedhp = data2.hp;
        data2.setActionHistory(playerId, BattleServantActionHistory.TYPE.INSTANT_DEATH, this.logic.getWave());
        BattleActionData.BuffData data3 = this.getFunctionObject(funcEnt, data2.getUniqueID(), funcIndex);
        data3.procType = BattleActionData.BuffData.BuffProcType.INSTANT_DEATH;
        data.setBuffData(data3);
        return data;
    }

    public BattleActionData functionlossHp(int playerId, int targetId, FunctionEntity funcEnt, DataVals baseVals, int funcIndex, bool safe)
    {
        BattleActionData data = new BattleActionData();
        BattleServantData data2 = this.data.getServantData(targetId);
        int num = baseVals.GetValue();
        int hp = data2.hp;
        data2.hp -= num;
        if (safe)
        {
            if (data2.hp <= 1)
            {
                data2.hp = 1;
            }
        }
        else if (data2.hp <= 0)
        {
            data2.hp = 0;
        }
        data2.reducedhp += hp - data2.hp;
        data2.setActionHistory(playerId, BattleServantActionHistory.TYPE.HPLOSS, this.logic.getWave());
        BattleActionData.BuffData data3 = this.getFunctionObject(funcEnt, data2.getUniqueID(), funcIndex);
        data3.procType = BattleActionData.BuffData.BuffProcType.UPDATE_HP;
        data.setBuffData(data3);
        return data;
    }

    public BattleActionData functionMissState(int targetId) => 
        new BattleActionData();

    public BattleActionData functionNPDamage(int playerId, int targetId, DataVals baseVals, int funcIndex, BattleLogic.DamageType type)
    {
        BattleActionData data = new BattleActionData();
        BattleServantData actor = this.data.getServantData(playerId);
        BattleServantData target = this.data.getServantData(targetId);
        List<int> list = new List<int> {
            baseVals.GetParam(DataVals.TYPE.Value)
        };
        if ((type == BattleLogic.DamageType.NOBLE_INDIVIDUAL) || (type == BattleLogic.DamageType.NOBLE_STATE_INDIVIDUAL))
        {
            list.Add(baseVals.GetParam(DataVals.TYPE.Correction));
        }
        else if ((type == BattleLogic.DamageType.NOBLE_HPRATIO_HIGH) || (type == BattleLogic.DamageType.NOBLE_HPRATIO_LOW))
        {
            list.Add(baseVals.GetParam(DataVals.TYPE.Target));
        }
        int[] svtIndv = null;
        if (type == BattleLogic.DamageType.NOBLE_INDIVIDUAL)
        {
            svtIndv = new int[] { baseVals.GetParam(DataVals.TYPE.Target) };
        }
        int[] buffIndv = null;
        if (type == BattleLogic.DamageType.NOBLE_STATE_INDIVIDUAL)
        {
            svtIndv = new int[] { baseVals.GetParam(DataVals.TYPE.Target) };
        }
        BattleActionData.DamageData data4 = this.logic.getFunctionNpDamagelist(actor, target, list.ToArray(), funcIndex, type, svtIndv, buffIndv);
        data.setDamageData(data4);
        return data;
    }

    public BattleActionData functionReplaceMember(int targetId, int subTargetId, FunctionEntity funcEnt, DataVals baeVals, int funcIndex)
    {
        BattleActionData data = new BattleActionData();
        BattleServantData data2 = this.data.getServantData(targetId);
        BattleServantData data3 = this.data.getServantData(subTargetId);
        int index = data2.getDeckIndex();
        int num2 = data3.getDeckIndex();
        if ((this.data.p_entryid.Length <= index) || (this.data.p_entryid[index] != data2.getUniqueID()))
        {
            for (int j = 0; j < this.data.p_entryid.Length; j++)
            {
                if (this.data.p_entryid[j] == data2.getUniqueID())
                {
                    data2.setDeckIndex(j);
                }
            }
            index = data2.getDeckIndex();
        }
        data2.setDeckIndex(num2);
        data3.setDeckIndex(index);
        for (int i = 0; i < this.data.p_entryid.Length; i++)
        {
            if (this.data.p_entryid[i] == data2.getUniqueID())
            {
                this.data.p_entryid[i] = data3.getUniqueID();
            }
        }
        data.setReplaceMember(index, data3.getUniqueID(), data2.getUniqueID(), funcIndex);
        data.redrawCommandCard = true;
        return data;
    }

    public BattleActionData functionResetCommandCard(int targetId, FunctionEntity funcEnt, DataVals baseVals, int funcIndex)
    {
        BattleActionData data = new BattleActionData();
        this.data.shuffleCommand();
        this.logic.drawCommand();
        this.logic.setDrawCard();
        data.redrawCommandCard = true;
        return data;
    }

    public BattleActionData functionSubState(int targetId, FunctionEntity funcEnt, DataVals baseVals, int index)
    {
        BattleActionData data = new BattleActionData();
        BattleServantData data2 = this.data.getServantData(targetId);
        if (data2.subBuffFromIndividualites(funcEnt.vals))
        {
            data.setBuffData(this.getFunctionObject(funcEnt, data2.getUniqueID(), index));
            return data;
        }
        return this.getMissObject(targetId, index, baseVals);
    }

    public BattleActionData functionTransformServant(int targetId, FunctionEntity funcEnt, DataVals baeVals, int funcIndex)
    {
        BattleActionData data = new BattleActionData();
        BattleServantData data2 = this.data.getServantData(targetId);
        int param = baeVals.GetParam(DataVals.TYPE.Target);
        data2.setTransformServant(this.data.getBattleInfo(), param);
        data.setTransformServant(param, targetId, funcIndex);
        return data;
    }

    public FunctionEntity getFunctionEntity(int id) => 
        this.master?.getEntityFromId<FunctionEntity>(id);

    public BattleActionData.BuffData getFunctionObject(FunctionEntity funcEnt, int uniqueId, int funcIndex) => 
        new BattleActionData.BuffData { 
            targetId = uniqueId,
            functionIndex = funcIndex,
            popLabel = funcEnt.popupText,
            popIcon = funcEnt.popupIconId,
            popColor = funcEnt.popupTextColor,
            effectList = funcEnt.getEffectList(),
            procType = BattleActionData.BuffData.BuffProcType.NONE
        };

    public BattleActionData getMissObject(int targetId, int funcIndex, DataVals dataVals)
    {
        BattleActionData data = new BattleActionData();
        BattleActionData.BuffData data2 = new BattleActionData.BuffData {
            targetId = targetId,
            functionIndex = funcIndex,
            isMiss = true,
            buffId = 0
        };
        if (dataVals.isHideMiss())
        {
            data2.popLabel = string.Empty;
        }
        else
        {
            data2.popLabel = "MISS";
        }
        data2.effectList = new int[0];
        data.setBuffData(data2);
        return data;
    }

    public bool isSelectTarget(int uniqueId, int[] funclist)
    {
        Debug.Log("isSelectTarget(" + uniqueId);
        foreach (int num in funclist)
        {
            Debug.Log("Id:" + num);
            if (Target.isChoose(this.getFunctionEntity(num).targetType))
            {
                return true;
            }
        }
        return false;
    }

    public BattleActionData procList(BattleActionData action, int[] functionlist, DataVals[] baseValslist, bool passive = false)
    {
        this.DebugLog("=========");
        this.DebugLog("====< start BattleLogicFunction::procList >=====");
        BattleServantData data = null;
        this.DebugLog(" actorId: " + action.actorId);
        int targetId = action.targetId;
        this.DebugLog(" enemytargetId: " + targetId);
        int pttargetId = action.getPTTargetId();
        int subTargetId = action.getPTSubTargetId();
        this.DebugLog(" pttargetId: " + pttargetId);
        DataVals dataVals = baseValslist[0];
        BuffMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<BuffMaster>(DataNameKind.Kind.BUFF);
        this.logic.resetReducedHpAll();
        this.lastFuncResult.Clear();
        List<FuncInfo> list = new List<FuncInfo>();
        this.DebugLog("funclist.length : " + functionlist.Length);
        for (int i = 0; i < functionlist.Length; i++)
        {
            dataVals = baseValslist[i];
            this.DebugLog("==========>");
            int id = functionlist[i];
            this.DebugLog("functionId : " + id);
            if (dataVals != null)
            {
                for (int j = 0; j < dataVals.Length(); j++)
                {
                    this.DebugLog("baseVals[]" + dataVals.GetValue(j));
                }
            }
            int[] numArray = null;
            FunctionEntity funcEnt = this.getFunctionEntity(id);
            dataVals.SetType((FuncList.TYPE) funcEnt.funcType);
            this.DebugLog("targetType : " + ((Target.TYPE) funcEnt.targetType).ToString());
            int[] target = funcEnt.getQuestTargetValues();
            if ((0 >= target.Length) || Individuality.CheckIndividualities(this.data.getQuestIndividualitie(), target))
            {
                numArray = Target.getTargetIds(this.data, action.actorId, targetId, pttargetId, funcEnt.targetType);
                FuncInfo item = null;
                if (action.isBattleLiveSkill(this.data))
                {
                    item = new FuncInfo {
                        fid = id
                    };
                    list.Add(item);
                    item.targetInfoList = new TargetInfo[numArray.Length];
                    for (int k = 0; k < item.targetInfoList.Length; k++)
                    {
                        List<int> list2 = new List<int>();
                        item.targetInfoList[k] = new TargetInfo { 
                            tid = numArray[k],
                            buffids = list2.ToArray()
                        };
                    }
                }
                this.DebugLog("==targetlist.Length : " + numArray.Length);
                foreach (int num8 in numArray)
                {
                    List<int> list3 = new List<int>();
                    this.DebugLog(" funcEnt.funcType : " + ((FuncList.TYPE) funcEnt.funcType).ToString());
                    this.DebugLog("  => targetId : " + num8);
                    data = this.data.getServantData(num8);
                    TargetInfo info2 = null;
                    if (action.isBattleLiveSkill(this.data) && (item != null))
                    {
                        for (int m = 0; m < item.targetInfoList.Length; m++)
                        {
                            if ((item.targetInfoList[m] != null) && (num8 == item.targetInfoList[m].tid))
                            {
                                info2 = item.targetInfoList[m];
                                info2.isMiss = 1;
                            }
                        }
                    }
                    if (data == null)
                    {
                        this.DebugLog("  <= no Target");
                    }
                    else if ((!data.checkEnemy() || !funcEnt.isTargetEnemy()) && (!data.checkPlayer() || !funcEnt.isTargetPlayer()))
                    {
                        this.DebugLog("  <= is No applyTarget ");
                    }
                    else if (!data.provisionalDamage(0) && !data.isGuts())
                    {
                        this.DebugLog("  <= is Dead");
                    }
                    else if (!Individuality.CheckIndividualities(data.getIndividualities(), funcEnt.tvals))
                    {
                        this.DebugLog("  <= no Individuality");
                        action.addAction(this.getMissObject(data.getUniqueID(), i, dataVals));
                    }
                    else
                    {
                        if (action.isBattleLiveSkill(this.data) && (info2 != null))
                        {
                            info2.isMiss = 0;
                        }
                        if (FuncList.TYPE.ADD_STATE.Check(funcEnt.funcType) || FuncList.TYPE.ADD_STATE_SHORT.Check(funcEnt.funcType))
                        {
                            BuffEntity buffEnt = master.getEntityFromId<BuffEntity>(funcEnt.vals[0]);
                            this.DebugLog(" buff : " + ((BuffList.TYPE) buffEnt.id).ToString());
                            if (data.checkBuffAvoid(buffEnt.getIndividuality()))
                            {
                                this.DebugLog(" avoid buff ");
                                action.addAction(this.getMissObject(data.getUniqueID(), i, dataVals));
                                goto Label_0B6C;
                            }
                            BattleActionData adddata = this.functionAddState(action.actorId, num8, buffEnt, funcEnt, dataVals, i, passive, FuncList.TYPE.ADD_STATE_SHORT.Check(funcEnt.funcType));
                            if (adddata != null)
                            {
                                action.addAction(adddata);
                                if (action.isBattleLiveSkill(this.data))
                                {
                                    foreach (BattleActionData.BuffData data3 in adddata.buffdatalist)
                                    {
                                        if (!list3.Contains(data3.buffId) && !data3.isMiss)
                                        {
                                            list3.Add(data3.buffId);
                                        }
                                    }
                                }
                            }
                        }
                        else if (!this.checkFuncAction(action.actorId, num8, null, funcEnt, dataVals))
                        {
                            if (FuncList.TYPE.GAIN_NP.Check(funcEnt.funcType) || FuncList.TYPE.LOSS_NP.Check(funcEnt.funcType))
                            {
                                if (data.checkPlayer())
                                {
                                    action.addAction(this.getMissObject(data.getUniqueID(), i, dataVals));
                                }
                            }
                            else if (FuncList.TYPE.HASTEN_NPTURN.Check(funcEnt.funcType) || FuncList.TYPE.DELAY_NPTURN.Check(funcEnt.funcType))
                            {
                                if (data.checkEnemy())
                                {
                                    action.addAction(this.getMissObject(data.getUniqueID(), i, dataVals));
                                }
                            }
                            else
                            {
                                action.addAction(this.getMissObject(data.getUniqueID(), i, dataVals));
                            }
                            if (action.isBattleLiveSkill(this.data) && (info2 != null))
                            {
                                info2.isMiss = 1;
                            }
                        }
                        else if (FuncList.TYPE.SUB_STATE.Check(funcEnt.funcType))
                        {
                            action.addAction(this.functionSubState(num8, funcEnt, dataVals, i));
                        }
                        else if (FuncList.TYPE.DAMAGE.Check(funcEnt.funcType))
                        {
                            action.addAction(this.functionDamage(action.actorId, data.getUniqueID(), dataVals, i));
                        }
                        else if (FuncList.TYPE.DAMAGE_NP.Check(funcEnt.funcType))
                        {
                            action.addAction(this.functionNPDamage(action.actorId, data.getUniqueID(), dataVals, i, BattleLogic.DamageType.NOBLE_NOMAL));
                        }
                        else if (FuncList.TYPE.DAMAGE_NP_PIERCE.Check(funcEnt.funcType))
                        {
                            action.addAction(this.functionNPDamage(action.actorId, data.getUniqueID(), dataVals, i, BattleLogic.DamageType.NOBLE_PIERCE));
                        }
                        else if (FuncList.TYPE.DAMAGE_NP_INDIVIDUAL.Check(funcEnt.funcType))
                        {
                            action.addAction(this.functionNPDamage(action.actorId, data.getUniqueID(), dataVals, i, BattleLogic.DamageType.NOBLE_INDIVIDUAL));
                        }
                        else if (FuncList.TYPE.DAMAGE_NP_STATE_INDIVIDUAL.Check(funcEnt.funcType))
                        {
                            action.addAction(this.functionNPDamage(action.actorId, data.getUniqueID(), dataVals, i, BattleLogic.DamageType.NOBLE_STATE_INDIVIDUAL));
                        }
                        else if (FuncList.TYPE.DAMAGE_NP_HPRATIO_HIGH.Check(funcEnt.funcType))
                        {
                            action.addAction(this.functionNPDamage(action.actorId, data.getUniqueID(), dataVals, i, BattleLogic.DamageType.NOBLE_HPRATIO_HIGH));
                        }
                        else if (FuncList.TYPE.DAMAGE_NP_HPRATIO_LOW.Check(funcEnt.funcType))
                        {
                            action.addAction(this.functionNPDamage(action.actorId, data.getUniqueID(), dataVals, i, BattleLogic.DamageType.NOBLE_HPRATIO_LOW));
                        }
                        else if (FuncList.TYPE.GAIN_STAR.Check(funcEnt.funcType))
                        {
                            this.data.addCriticalPoint(dataVals.GetValue());
                            BattleActionData.BuffData data4 = this.getFunctionObject(funcEnt, data.getUniqueID(), i);
                            data4.procType = BattleActionData.BuffData.BuffProcType.UPDATE_CRITICAL;
                            action.setBuffData(data4);
                        }
                        else if (FuncList.TYPE.GAIN_HP.Check(funcEnt.funcType))
                        {
                            action.addAction(this.functionGainHp(action.actorId, num8, funcEnt, dataVals, i));
                        }
                        else if (FuncList.TYPE.GAIN_HP_PER.Check(funcEnt.funcType))
                        {
                            action.addAction(this.functionGainHpPer(action.actorId, num8, funcEnt, dataVals, i));
                        }
                        else if (FuncList.TYPE.GAIN_NP.Check(funcEnt.funcType))
                        {
                            if (data.checkPlayer())
                            {
                                data.addNp(dataVals.GetValue(), false);
                                BattleActionData.BuffData data5 = this.getFunctionObject(funcEnt, data.getUniqueID(), i);
                                data5.procType = BattleActionData.BuffData.BuffProcType.UPDATE_NP;
                                action.setBuffData(data5);
                            }
                        }
                        else if (FuncList.TYPE.LOSS_NP.Check(funcEnt.funcType))
                        {
                            if (data.checkPlayer())
                            {
                                data.addNp(-dataVals.GetValue(), false);
                                BattleActionData.BuffData data6 = this.getFunctionObject(funcEnt, data.getUniqueID(), i);
                                data6.procType = BattleActionData.BuffData.BuffProcType.UPDATE_NP;
                                action.setBuffData(data6);
                            }
                        }
                        else if (FuncList.TYPE.SHORTEN_SKILL.Check(funcEnt.funcType))
                        {
                            data.skillChageShorten(dataVals.GetValue(), 0);
                            action.setBuffData(this.getFunctionObject(funcEnt, data.getUniqueID(), i));
                        }
                        else if (FuncList.TYPE.EXTEND_SKILL.Check(funcEnt.funcType))
                        {
                            data.skillChageExtend(dataVals.GetValue(), 0x3e7);
                            action.setBuffData(this.getFunctionObject(funcEnt, data.getUniqueID(), i));
                        }
                        else if (FuncList.TYPE.LOSS_HP.Check(funcEnt.funcType))
                        {
                            action.addAction(this.functionlossHp(action.actorId, num8, funcEnt, dataVals, i, false));
                        }
                        else if (FuncList.TYPE.LOSS_HP_SAFE.Check(funcEnt.funcType))
                        {
                            action.addAction(this.functionlossHp(action.actorId, num8, funcEnt, dataVals, i, true));
                        }
                        else if (FuncList.TYPE.INSTANT_DEATH.Check(funcEnt.funcType))
                        {
                            action.addAction(this.functionInstantDeath(action.actorId, num8, funcEnt, dataVals, i));
                        }
                        else if (FuncList.TYPE.HASTEN_NPTURN.Check(funcEnt.funcType))
                        {
                            action.addAction(this.functionHastenNpTurn(num8, funcEnt, dataVals, i));
                        }
                        else if (FuncList.TYPE.DELAY_NPTURN.Check(funcEnt.funcType))
                        {
                            action.addAction(this.functionDelayNpTurn(num8, funcEnt, dataVals, i));
                        }
                        else if (FuncList.TYPE.CARD_RESET.Check(funcEnt.funcType))
                        {
                            action.addAction(this.functionResetCommandCard(num8, funcEnt, dataVals, i));
                        }
                        else if (FuncList.TYPE.REPLACE_MEMBER.Check(funcEnt.funcType))
                        {
                            action.addAction(this.functionReplaceMember(num8, subTargetId, funcEnt, dataVals, i));
                        }
                        else if (FuncList.TYPE.TRANSFORM_SERVANT.Check(funcEnt.funcType))
                        {
                            action.addAction(this.functionTransformServant(num8, funcEnt, dataVals, i));
                        }
                        if (action.isBattleLiveSkill(this.data) && (info2 != null))
                        {
                            info2.buffids = new int[list3.Count];
                            Array.Copy(list3.ToArray(), 0, info2.buffids, 0, list3.Count);
                        }
                    Label_0B6C:;
                    }
                }
            }
        }
        if (action.isBattleLiveSkill(this.data))
        {
            foreach (BattleUsedSkills skills in this.data.usedSkilllist)
            {
                if ((skills.uniqueId == action.actorId) && (skills.funcList == null))
                {
                    skills.funcList = new FuncInfo[list.Count];
                    Array.Copy(list.ToArray(), skills.funcList, list.Count);
                }
            }
        }
        this.logic.checkUsedBuff();
        this.DebugLog("====< end BattleLogicFunction::procList >=====");
        return action;
    }
}

