using System;
using System.Runtime.InteropServices;

public class BattleInfoData
{
    public string appVer;
    public int blankCardCnt;
    public int dataVer;
    public DeckData[] enemyDeck;
    public int friendId;
    public int groundNo;
    public int id;
    public bool isCompress;
    public DeckData myDeck;
    public string resultInfo;
    public string resultInfoBlob;
    public int targetId;
    public int targetUserSvtId;
    public DeckData transformDeck;
    public int useOrderCnt;
    public int useOrderMax;
    public int userEquipId;
    public UserFormationData userFormation;
    public int userId;
    public BattleUserServantData[] userSvt;

    public BattleDeckServantData getDeckServantData(int uniqueId)
    {
        foreach (BattleDeckServantData data in this.myDeck.svts)
        {
            if (data.uniqueId == uniqueId)
            {
                return data;
            }
        }
        return null;
    }

    public DeckData getEnemyDeck(int battlecount) => 
        this.enemyDeck[battlecount];

    public BattleUserServantData getEquipFromID(long usersvtid, long useid)
    {
        if (this.userSvt != null)
        {
            for (int i = 0; i < this.userSvt.Length; i++)
            {
                if ((this.userSvt[i].id == usersvtid) && (this.userSvt[i].userId == useid))
                {
                    return this.userSvt[i];
                }
            }
        }
        else
        {
            Debug.Log("err -- getUserServantFromID{" + usersvtid + "} is Null");
        }
        return null;
    }

    public int getGroundNo() => 
        this.groundNo;

    public int getLastWave() => 
        (this.enemyDeck.Length - 1);

    public BattleDeckServantData getTransformDeckServantData(int uniqueId, int trIndex)
    {
        foreach (BattleDeckServantData data in this.transformDeck.svts)
        {
            if ((data.uniqueId == uniqueId) && (data.index == trIndex))
            {
                return data;
            }
        }
        return null;
    }

    public BattleUserServantData getUserServantFromID(long usersvtid, long userid = 0)
    {
        if (this.userSvt != null)
        {
            for (int i = 0; i < this.userSvt.Length; i++)
            {
                if ((this.userSvt[i].id == usersvtid) && (this.userSvt[i].userId == userid))
                {
                    return this.userSvt[i];
                }
            }
        }
        else
        {
            Debug.Log("err -- getUserServantFromID{" + usersvtid + "} is Null");
        }
        return null;
    }

    public bool isLastStage(int wavecount) => 
        ((wavecount + 1) == this.enemyDeck.Length);

    public bool isNextBattle(int battlecount) => 
        ((battlecount + 1) < this.enemyDeck.Length);
}

