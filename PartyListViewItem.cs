using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class PartyListViewItem : ListViewItem
{
    protected int cost;
    protected string deckName;
    protected FollowerInfo followerInfo;
    protected long id;
    protected bool isDeckNameDefault;
    protected MenuKind kind;
    protected long mainDeckId;
    protected int maxCost;
    protected PartyOrganizationListViewItem[] memberList;

    protected PartyListViewItem()
    {
        this.memberList = new PartyOrganizationListViewItem[BalanceConfig.DeckMemberMax];
    }

    public PartyListViewItem(MenuKind kind, int index, long mainDeckId, int maxCost, UserDeckEntity deck, FollowerInfo follower = null, int followerClassId = 0, EventUpValSetupInfo setupInfo = null) : base(index)
    {
        this.kind = kind;
        this.mainDeckId = mainDeckId;
        this.maxCost = maxCost;
        this.followerInfo = follower;
        if (string.IsNullOrEmpty(deck.name))
        {
            this.isDeckNameDefault = true;
            this.deckName = string.Format(LocalizationManager.Get("PARTY_ORGANIZATION_NAME_BASE"), index + 1);
        }
        else
        {
            this.isDeckNameDefault = false;
            this.deckName = deck.name;
        }
        this.id = deck.id;
        UserServantEntity[] userServantList = deck.GetUserServantList();
        int followerIndex = deck.GetFollowerIndex();
        if (followerIndex <= 0)
        {
            followerIndex = BalanceConfig.DeckMainMemberMax;
        }
        this.memberList = new PartyOrganizationListViewItem[BalanceConfig.DeckMemberMax];
        this.cost = 0;
        for (int i = 0; i < BalanceConfig.DeckMemberMax; i++)
        {
            if ((i + 1) == followerIndex)
            {
                if (follower != null)
                {
                    this.memberList[i] = new PartyOrganizationListViewItem(i, follower, followerClassId, setupInfo);
                }
                else
                {
                    this.memberList[i] = new PartyOrganizationListViewItem(i, true, setupInfo);
                }
            }
            else if (userServantList[i] != null)
            {
                this.memberList[i] = new PartyOrganizationListViewItem(i, userServantList[i], deck.GetEquipList(i), setupInfo);
            }
            else
            {
                this.memberList[i] = new PartyOrganizationListViewItem(i, false, setupInfo);
            }
            this.cost += this.memberList[i].MargeCost;
        }
    }

    public void ClearFollower()
    {
        for (int i = 0; i < BalanceConfig.DeckMemberMax; i++)
        {
            this.memberList[i].ClearFollower();
        }
        this.followerInfo = null;
    }

    public void ClearMember()
    {
        for (int i = 0; i < BalanceConfig.DeckMemberMax; i++)
        {
            if (!this.memberList[i].IsFollower)
            {
                this.memberList[i].Empty();
            }
        }
        this.cost = 0;
    }

    public void ClearMember(int num)
    {
        this.cost -= this.memberList[num].MargeCost;
        this.memberList[num].Empty();
    }

    public PartyListViewItem Clone()
    {
        PartyListViewItem item = new PartyListViewItem();
        item.Set(this);
        return item;
    }

    public bool CompMember(PartyListViewItem item)
    {
        if (this.DeckName != item.DeckName)
        {
            return false;
        }
        for (int i = 0; i < BalanceConfig.DeckMemberMax; i++)
        {
            PartyOrganizationListViewItem item2 = this.memberList[i];
            PartyOrganizationListViewItem item3 = item.memberList[i];
            if (!item2.CompMember(item3))
            {
                return false;
            }
        }
        return true;
    }

    ~PartyListViewItem()
    {
    }

    public string[] GetAssetNameList()
    {
        List<string> list = new List<string>();
        for (int i = 0; i < BalanceConfig.DeckMemberMax; i++)
        {
            PartyOrganizationListViewItem item = this.memberList[i];
            if (item != null)
            {
                string assetName = item.GetAssetName();
                if (assetName != null)
                {
                    list.Add(assetName);
                }
            }
        }
        if (list.Count > 0)
        {
            return list.ToArray();
        }
        return null;
    }

    public int[] GetCommandCardList(int typeMax)
    {
        int[] numArray = new int[typeMax];
        for (int i = 0; i < BalanceConfig.DeckMainMemberMax; i++)
        {
            if (this.memberList[i] != null)
            {
                int[] commandCardList = this.memberList[i].GetCommandCardList();
                if (commandCardList != null)
                {
                    for (int j = 0; j < typeMax; j++)
                    {
                        if (commandCardList.Length > j)
                        {
                            numArray[j] += commandCardList[j];
                        }
                    }
                }
            }
        }
        return numArray;
    }

    public DeckCondition GetDeckCondition()
    {
        if (this.cost > this.maxCost)
        {
            return DeckCondition.COST_OVER;
        }
        int num = 0;
        bool flag = false;
        for (int i = 0; i < BalanceConfig.DeckMainMemberMax; i++)
        {
            PartyOrganizationListViewItem item = this.memberList[i];
            if (item.IsFollower)
            {
                flag = true;
            }
            else if (item.UserServant != null)
            {
                num++;
            }
        }
        int num3 = num;
        for (int j = BalanceConfig.DeckMainMemberMax; j < BalanceConfig.DeckMemberMax; j++)
        {
            PartyOrganizationListViewItem item2 = this.memberList[j];
            if (item2.UserServant != null)
            {
                num3++;
            }
        }
        if (num3 == 0)
        {
            return DeckCondition.EMPTY_DECK_MEMBER;
        }
        if ((num == 1) && (this.memberList[0].UserServant != null))
        {
            return DeckCondition.LEADER_ONLY_DECK_MEMBER;
        }
        if ((num + (!flag ? 0 : 1)) < BalanceConfig.DeckMainMemberMax)
        {
            return DeckCondition.SHORTAGE_DECK_MEMBER;
        }
        for (int k = 0; k < BalanceConfig.DeckMemberMax; k++)
        {
            PartyOrganizationListViewItem item3 = this.memberList[k];
            if (item3.UserServant != null)
            {
                int baseSvtId = item3.Servant.baseSvtId;
                for (int m = 0; m < BalanceConfig.DeckMemberMax; m++)
                {
                    if (k != m)
                    {
                        PartyOrganizationListViewItem item4 = this.memberList[m];
                        if ((item4.UserServant != null) && (baseSvtId == item4.Servant.baseSvtId))
                        {
                            return DeckCondition.SAME_SAERVANT;
                        }
                    }
                }
            }
        }
        return DeckCondition.OK;
    }

    public long[] GetEquipList()
    {
        List<long> list = new List<long>();
        for (int i = 0; i < this.memberList.Length; i++)
        {
            PartyOrganizationListViewItem item = this.memberList[i];
            if (item.EquipUserSvtId > 0L)
            {
                list.Add(item.EquipUserSvtId);
            }
        }
        return list.ToArray();
    }

    public bool GetEventUpVal(out EventUpValInfo[] eventUpValList)
    {
        eventUpValList = new EventUpValInfo[BalanceConfig.DeckMemberMax];
        for (int i = 0; i < BalanceConfig.DeckMemberMax; i++)
        {
            if (this.memberList[i] != null)
            {
                this.memberList[i].GetEventUpVal(out eventUpValList[i]);
            }
        }
        return true;
    }

    public int GetFriendPointUpVal()
    {
        int num = 0;
        for (int i = 0; i < BalanceConfig.DeckMemberMax; i++)
        {
            if (this.memberList[i] != null)
            {
                int friendPointUpVal = this.memberList[i].GetFriendPointUpVal();
                if (friendPointUpVal > num)
                {
                    num = friendPointUpVal;
                }
            }
        }
        return num;
    }

    public bool[] GetIsFollowerList()
    {
        bool[] flagArray = new bool[BalanceConfig.DeckMemberMax];
        for (int i = 0; i < BalanceConfig.DeckMemberMax; i++)
        {
            if (this.memberList[i] != null)
            {
                flagArray[i] = this.memberList[i].IsFollower;
            }
        }
        return flagArray;
    }

    public PartyOrganizationListViewItem GetMember(int num) => 
        this.memberList[num];

    public int[] GetSvtIdList()
    {
        int[] numArray = new int[BalanceConfig.DeckMemberMax];
        for (int i = 0; i < BalanceConfig.DeckMemberMax; i++)
        {
            if ((this.memberList[i] != null) && (this.memberList[i].Servant != null))
            {
                numArray[i] = this.memberList[i].Servant.id;
            }
        }
        return numArray;
    }

    public string[] GetSvtNameList()
    {
        string[] strArray = new string[BalanceConfig.DeckMemberMax];
        for (int i = 0; i < BalanceConfig.DeckMemberMax; i++)
        {
            if (this.memberList[i] != null)
            {
                strArray[i] = this.memberList[i].SvtNameText;
            }
        }
        return strArray;
    }

    public UserDeckEntity GetUserDeck()
    {
        UserDeckEntity entity = new UserDeckEntity {
            id = this.id,
            name = !this.isDeckNameDefault ? this.deckName : string.Empty,
            deckInfo = new DeckServant(this.memberList.Length)
        };
        for (int i = 0; i < this.memberList.Length; i++)
        {
            PartyOrganizationListViewItem item = this.memberList[i];
            entity.deckInfo.svts[i] = new DeckServantData { 
                id = i + 1,
                userSvtId = (item.UserServant == null) ? 0L : item.UserServant.id,
                isFollowerSvt = item.IsFollower,
                userSvtEquipIds = item.GetEquipList()
            };
        }
        return entity;
    }

    public bool IsDeckEmpty()
    {
        for (int i = 0; i < BalanceConfig.DeckMemberMax; i++)
        {
            PartyOrganizationListViewItem item = this.memberList[i];
            if (item.UserServant != null)
            {
                return false;
            }
        }
        return true;
    }

    public void LeaderOnly()
    {
        for (int i = 1; i < BalanceConfig.DeckMemberMax; i++)
        {
            if (!this.memberList[i].IsFollower)
            {
                this.memberList[i].Empty();
            }
        }
        this.cost = this.memberList[0].MargeCost;
    }

    public void Modify()
    {
        for (int i = 0; i < BalanceConfig.DeckMemberMax; i++)
        {
            this.memberList[i].Modify();
        }
    }

    public void Set(PartyListViewItem item)
    {
        base.Set(item);
        this.kind = item.kind;
        this.mainDeckId = item.mainDeckId;
        this.maxCost = item.maxCost;
        this.id = item.id;
        this.followerInfo = item.followerInfo;
        for (int i = 0; i < BalanceConfig.DeckMemberMax; i++)
        {
            this.memberList[i] = item.memberList[i].Clone();
        }
        this.cost = item.cost;
        this.isDeckNameDefault = item.isDeckNameDefault;
        this.deckName = item.deckName;
    }

    public void SetDeckName(string name)
    {
        this.isDeckNameDefault = false;
        this.deckName = name;
    }

    public void SetEquip(int num, long userSvtId)
    {
        this.cost -= this.memberList[num].MargeCost;
        this.memberList[num].SetEquipUserServantId(userSvtId);
        this.cost += this.memberList[num].MargeCost;
    }

    public void SetMember(int num, PartyServantListViewItem item)
    {
        this.cost -= this.memberList[num].MargeCost;
        this.memberList[num].Modify(item);
        this.cost += this.memberList[num].MargeCost;
    }

    public void SwapMember(int num1, int num2)
    {
        this.memberList[num1].Swap(this.memberList[num2]);
    }

    public int Cost =>
        this.cost;

    public long DeckId =>
        this.id;

    public string DeckName =>
        this.deckName;

    public bool IsMainDeck =>
        (this.id == this.mainDeckId);

    public MenuKind Kind =>
        this.kind;

    public long MainDeckId =>
        this.mainDeckId;

    public int MaxCost =>
        this.maxCost;

    public enum DeckCondition
    {
        OK,
        EMPTY_DECK_MEMBER,
        LEADER_ONLY_DECK_MEMBER,
        SHORTAGE_DECK_MEMBER,
        SAME_SAERVANT,
        COST_OVER
    }

    public enum MenuKind
    {
        QUEST_START,
        SELECT_PARTY
    }

    public enum SetupKind
    {
        PARTY_ORGANIZATION,
        BATTLE_SETUP
    }
}

