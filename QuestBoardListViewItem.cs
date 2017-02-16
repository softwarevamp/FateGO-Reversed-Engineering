using System;
using System.Runtime.CompilerServices;

public class QuestBoardListViewItem : ListViewItem
{
    private QuestBoardListViewItem()
    {
    }

    public QuestBoardListViewItem(int index, InfoKind ikind, clsMapCtrl_QuestInfo qinf, QuestBoardListViewManager qbvm) : base(index)
    {
        this.info_kind = ikind;
        this.quest_info = qinf;
        this.qbvm_info = qbvm;
    }

    public InfoKind info_kind { get; private set; }

    public QuestBoardListViewManager qbvm_info { get; private set; }

    public clsMapCtrl_QuestInfo quest_info { get; private set; }

    public enum InfoKind
    {
        AREA,
        MAP,
        STORY,
        HERO,
        CALDEA,
        SIZEOF
    }
}

