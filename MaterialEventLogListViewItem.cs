using System;
using System.Runtime.CompilerServices;

public class MaterialEventLogListViewItem : ListViewItem
{
    private MaterialEventLogListViewItem()
    {
    }

    public MaterialEventLogListViewItem(int index, Kind _kind, Info _info) : base(index)
    {
        this.kind = _kind;
        this.info = _info;
    }

    public Info info { get; set; }

    public Kind kind { get; private set; }

    public enum Flag
    {
        NONE,
        NOPLAY_DECIDE_SE,
        SVT_FACE
    }

    public class Info
    {
        public int event_id;
        public MaterialEventLogListViewItem.Flag flag;
        public int font_size;
        public int limit_count;
        public Action<MaterialEventLogListViewItem> on_click_act;
        public int phase_max;
        public int quest_id;
        public string ruby;
        public string str;
        public int svt_id;
        public int war_id;
    }

    public enum Kind
    {
        TOP,
        MAP,
        STORY,
        EVENT,
        FREE,
        QUEST,
        SIZEOF
    }
}

