using System;

public class NewsEntity : DataEntityBase
{
    public string detail;
    public long finishedAt;
    public int id;
    public long noticeAt;
    public int priority;
    public string title;
    public int type;

    public string getDetail() => 
        this.detail;

    public long getFinishedAt() => 
        this.finishedAt;

    public int getId() => 
        this.id;

    public long getNoticeAt() => 
        this.noticeAt;

    public override string getPrimarykey() => 
        (string.Empty + this.id);

    public int getPriority() => 
        this.priority;

    public string getTitle() => 
        this.title;

    public int getType() => 
        this.type;

    public enum enType
    {
        DATA_ANY = 4,
        HTML_BODY = 1,
        HTML_URL = 2,
        TEMPLATE = 3
    }
}

