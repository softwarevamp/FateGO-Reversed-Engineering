using System;

internal class StepFunc
{
    public static double Acc2(double v) => 
        (v * v);

    public static double Acc3(double v) => 
        ((v * v) * v);

    public static double Acc4(double v) => 
        (((v * v) * v) * v);

    public static double Acc5(double v) => 
        ((((v * v) * v) * v) * v);

    public static double AccDec2(double v) => 
        ((v > 0.5) ? (1.0 - (Math.Pow((1.0 - v) * 2.0, 2.0) / 2.0)) : (Math.Pow(v * 2.0, 2.0) / 2.0));

    public static double AccDec3(double v) => 
        ((v > 0.5) ? (1.0 - (Math.Pow((1.0 - v) * 2.0, 3.0) / 2.0)) : (Math.Pow(v * 2.0, 3.0) / 2.0));

    public static double AccDec4(double v) => 
        ((v > 0.5) ? (1.0 - (Math.Pow((1.0 - v) * 2.0, 4.0) / 2.0)) : (Math.Pow(v * 2.0, 4.0) / 2.0));

    public static double AccDec5(double v) => 
        ((v > 0.5) ? (1.0 - (Math.Pow((1.0 - v) * 2.0, 5.0) / 2.0)) : (Math.Pow(v * 2.0, 5.0) / 2.0));

    public static double AccSig(double v) => 
        (0.5 - (Math.Cos(v * 3.1415926535897931) / 2.0));

    public static double AccSin(double v) => 
        (1.0 - Math.Cos((v * 3.1415926535897931) / 2.0));

    public static double Dec2(double v) => 
        (1.0 - Math.Pow(1.0 - v, 2.0));

    public static double Dec3(double v) => 
        (1.0 - Math.Pow(1.0 - v, 3.0));

    public static double Dec4(double v) => 
        (1.0 - Math.Pow(1.0 - v, 4.0));

    public static double Dec5(double v) => 
        (1.0 - Math.Pow(1.0 - v, 5.0));

    public static double DecAcc2(double v) => 
        ((v > 0.5) ? (0.5 + (Math.Pow((v - 0.5) * 2.0, 2.0) / 2.0)) : (0.5 - (Math.Pow((0.5 - v) * 2.0, 2.0) / 2.0)));

    public static double DecAcc3(double v) => 
        ((v > 0.5) ? (0.5 + (Math.Pow((v - 0.5) * 2.0, 3.0) / 2.0)) : (0.5 - (Math.Pow((0.5 - v) * 2.0, 3.0) / 2.0)));

    public static double DecAcc4(double v) => 
        ((v > 0.5) ? (0.5 + (Math.Pow((v - 0.5) * 2.0, 4.0) / 2.0)) : (0.5 - (Math.Pow((0.5 - v) * 2.0, 4.0) / 2.0)));

    public static double DecAcc5(double v) => 
        ((v > 0.5) ? (0.5 + (Math.Pow((v - 0.5) * 2.0, 5.0) / 2.0)) : (0.5 - (Math.Pow((0.5 - v) * 2.0, 5.0) / 2.0)));

    public static double DecSig(double v) => 
        ((v > 0.5) ? (1.0 - (Math.Sin(v * 3.1415926535897931) / 2.0)) : (Math.Sin(v * 3.1415926535897931) / 2.0));

    public static double DecSin(double v) => 
        Math.Sin((v * 3.1415926535897931) / 2.0);

    public static double Linear(double v) => 
        v;
}

