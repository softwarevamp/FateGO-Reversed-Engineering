using System;
using UnityEngine;

public class ServantStatusParameterGauge : BaseMonoBehaviour
{
    [SerializeField]
    protected UISprite evaluationAdd1Sprite;
    [SerializeField]
    protected UISprite evaluationAdd2Sprite;
    [SerializeField]
    protected UISprite evaluationSprite;
    [SerializeField]
    protected UISprite[] gaugeSpriteList = new UISprite[5];

    public void Set(Kind kind, StatusRank.Kind rank)
    {
        string str = "img_parametergage_bar";
        string str2 = "img_parametergage_edge";
        string str3 = null;
        string str4 = null;
        string str5 = null;
        int num = 0;
        StatusRank.Kind kind2 = rank;
        switch (kind2)
        {
            case StatusRank.Kind.A:
            case StatusRank.Kind.A_PLUS:
            case StatusRank.Kind.A_PLUS2:
            case StatusRank.Kind.A_MINUS:
                str3 = "img_parameter_A";
                num = 5;
                break;

            case StatusRank.Kind.B:
            case StatusRank.Kind.B_PLUS:
            case StatusRank.Kind.B_PLUS2:
            case StatusRank.Kind.B_MINUS:
                str3 = "img_parameter_B";
                num = 4;
                break;

            case StatusRank.Kind.C:
            case StatusRank.Kind.C_PLUS:
            case StatusRank.Kind.C_PLUS2:
            case StatusRank.Kind.C_MINUS:
                str3 = "img_parameter_C";
                num = 3;
                break;

            case StatusRank.Kind.D:
            case StatusRank.Kind.D_PLUS:
            case StatusRank.Kind.D_PLUS2:
            case StatusRank.Kind.D_MINUS:
                str3 = "img_parameter_D";
                num = 2;
                break;

            case StatusRank.Kind.E:
            case StatusRank.Kind.E_PLUS:
            case StatusRank.Kind.E_PLUS2:
            case StatusRank.Kind.E_MINUS:
                str3 = "img_parameter_E";
                num = 1;
                break;

            default:
                if (kind2 == StatusRank.Kind.EX)
                {
                    str = "img_parametergage_bar_ex";
                    str2 = "img_parametergage_edge_ex";
                    str3 = "img_parameter_EX";
                    num = 5;
                }
                break;
        }
        switch (rank)
        {
            case StatusRank.Kind.A_PLUS:
            case StatusRank.Kind.B_PLUS:
            case StatusRank.Kind.C_PLUS:
            case StatusRank.Kind.D_PLUS:
            case StatusRank.Kind.E_PLUS:
                str4 = "img_parameter_plus";
                break;

            case StatusRank.Kind.A_PLUS2:
            case StatusRank.Kind.B_PLUS2:
            case StatusRank.Kind.C_PLUS2:
            case StatusRank.Kind.D_PLUS2:
            case StatusRank.Kind.E_PLUS2:
                str4 = "img_parameter_plus";
                str5 = "img_parameter_plus";
                break;

            case StatusRank.Kind.A_MINUS:
            case StatusRank.Kind.B_MINUS:
            case StatusRank.Kind.C_MINUS:
            case StatusRank.Kind.D_MINUS:
            case StatusRank.Kind.E_MINUS:
                str4 = "img_parameter_minus";
                break;
        }
        this.evaluationSprite.spriteName = str3;
        if (str3 != null)
        {
            this.evaluationSprite.MakePixelPerfect();
        }
        this.evaluationAdd1Sprite.spriteName = str4;
        if (str4 != null)
        {
            this.evaluationAdd1Sprite.MakePixelPerfect();
        }
        this.evaluationAdd2Sprite.spriteName = str5;
        if (str5 != null)
        {
            this.evaluationAdd2Sprite.MakePixelPerfect();
        }
        for (int i = 0; i < 5; i++)
        {
            if (i < num)
            {
                this.gaugeSpriteList[i].spriteName = ((i <= 0) || (i >= 4)) ? str2 : str;
            }
            else
            {
                this.gaugeSpriteList[i].spriteName = null;
            }
        }
    }

    public enum Kind
    {
        POWER,
        DEFENSE,
        AGILITY,
        MAGIC,
        LUCK,
        NP
    }
}

