using System;
using UnityEngine;

public class QuestStartEffectComponent : CommonEffectComponent
{
    [SerializeField]
    protected UIPanel basePanel;
    protected CommonEffectParam commonParam;
    [SerializeField]
    protected UILabel messageLabel;
    [SerializeField]
    protected UILabel titleLabel;
    [SerializeField]
    protected UISprite typeSprite;

    public override void SetParam(object param)
    {
        this.commonParam = param as CommonEffectParam;
        this.titleLabel.text = this.commonParam.title;
        this.messageLabel.text = this.commonParam.message;
        string str = null;
        switch (this.commonParam.type)
        {
            case 1:
                str = "quest_main";
                break;

            case 2:
                str = "quest_free";
                break;

            case 3:
                str = "quest_Interlude";
                break;

            case 5:
                str = "quest_event";
                break;

            case 6:
                str = "quest_heroic";
                break;
        }
        this.typeSprite.spriteName = str;
        this.typeSprite.MakePixelPerfect();
    }
}

