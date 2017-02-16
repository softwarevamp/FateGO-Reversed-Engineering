using System;
using System.Runtime.InteropServices;
using UnityEngine;

public static class Easing
{
    public static float Func(float from, float to, float t, TYPE type = 0)
    {
        float num = to - from;
        float num2 = from;
        switch (type)
        {
            case TYPE.LINER:
                return ((num * t) + from);

            case TYPE.QUADRATIC_IN:
                return (((num * t) * t) + from);

            case TYPE.QUADRATIC_OUT:
                return ((((num * -1f) * t) * (t - 2f)) + from);

            case TYPE.QUADRATIC_IN_OUT:
                t *= 2f;
                if (t >= 1f)
                {
                    t--;
                    return ((((num * -1f) * 0.5f) * ((t * (t - 2f)) - 1f)) + from);
                }
                return ((((num * 0.5f) * t) * t) + from);

            case TYPE.CUBIC_IN:
                return ((((num * t) * t) * t) + from);

            case TYPE.CUBIC_OUT:
                t--;
                return ((num * (((t * t) * t) + 1f)) + from);

            case TYPE.CUBIC_IN_OUT:
                t *= 2f;
                if (t >= 1f)
                {
                    t -= 2f;
                    return (((num * 0.5f) * (((t * t) * t) + 2f)) + from);
                }
                return (((((num * 0.5f) * t) * t) * t) + from);

            case TYPE.QUARTIC_IN:
                return (((((num * t) * t) * t) * t) + from);

            case TYPE.QUARTIC_OUT:
                t--;
                return ((num * ((((t * t) * t) * t) - 1f)) + from);

            case TYPE.QUARTIC_IN_OUT:
                t *= 2f;
                if (t >= 1f)
                {
                    t -= 2f;
                    return (((num * 0.5f) * ((((t * t) * t) * t) - 2f)) + from);
                }
                return ((((((num * 0.5f) * t) * t) * t) * t) + from);

            case TYPE.QUINTIC_IN:
                return ((((((num * t) * t) * t) * t) * t) + from);

            case TYPE.QUINTIC_OUT:
                t--;
                return ((num * (((((t * t) * t) * t) * t) + 1f)) + from);

            case TYPE.QUINTIC_IN_OUT:
                t *= 2f;
                if (t >= 1f)
                {
                    t -= 2f;
                    return (((num * 0.5f) * (((((t * t) * t) * t) * t) + 2f)) + from);
                }
                return (((((((num * 0.5f) * t) * t) * t) * t) * t) + from);

            case TYPE.SINUSOIDAL_IN:
                return ((((num * -1f) * Mathf.Cos(t * 1.570796f)) + num) + from);

            case TYPE.SINUSOIDAL_OUT:
                return ((num * Mathf.Sin(t * 1.570796f)) + from);

            case TYPE.SINUSOIDAL_IN_OUT:
                return ((((num * -1f) / 2f) * (Mathf.Cos(3.141593f * t) - 1f)) + from);

            case TYPE.EXPONENTIAL_IN:
                return ((num * Mathf.Pow(2f, 10f * (t - 1f))) + from);

            case TYPE.EXPONENTIAL_OUT:
                return ((num * (-Mathf.Pow(2f, -10f * t) + 1f)) + from);

            case TYPE.EXPONENTIAL_IN_OUT:
                t *= 2f;
                if (t >= 1f)
                {
                    t--;
                    return (((num * 0.5f) * (-Mathf.Pow(2f, -10f * t) + 2f)) + from);
                }
                return (((num * 0.5f) * Mathf.Pow(2f, 10f * (t - 1f))) + from);

            case TYPE.CIRCULAR_IN:
                return (((num * -1f) * (Mathf.Sqrt(1f - (t * t)) - 1f)) + from);

            case TYPE.CIRCULAR_OUT:
                t--;
                return ((num * Mathf.Sqrt(1f - (t * t))) + from);

            case TYPE.CIRCULAR_IN_OUT:
                t *= 2f;
                if (t >= 1f)
                {
                    t -= 2f;
                    return (((num * 0.5f) * (Mathf.Sqrt(1f - (t * t)) + 1f)) + from);
                }
                return ((((num * -1f) * 0.5f) * (Mathf.Sqrt(1f - (t * t)) - 1f)) + from);
        }
        return num2;
    }

    public static Color Func(Color from, Color to, float t, TYPE type = 0)
    {
        Color color = to - from;
        Color color2 = from;
        switch (type)
        {
            case TYPE.LINER:
                return (((Color) (color * t)) + from);

            case TYPE.QUADRATIC_IN:
                return (((Color) ((color * t) * t)) + from);

            case TYPE.QUADRATIC_OUT:
                return (((Color) (((color * -1f) * t) * (t - 2f))) + from);

            case TYPE.QUADRATIC_IN_OUT:
                t *= 2f;
                if (t >= 1f)
                {
                    t--;
                    return (((Color) (((color * -1f) * 0.5f) * ((t * (t - 2f)) - 1f))) + from);
                }
                return (((Color) (((color * 0.5f) * t) * t)) + from);

            case TYPE.CUBIC_IN:
                return (((Color) (((color * t) * t) * t)) + from);

            case TYPE.CUBIC_OUT:
                t--;
                return (((Color) (color * (((t * t) * t) + 1f))) + from);

            case TYPE.CUBIC_IN_OUT:
                t *= 2f;
                if (t >= 1f)
                {
                    t -= 2f;
                    return (((Color) ((color * 0.5f) * (((t * t) * t) + 2f))) + from);
                }
                return (((Color) ((((color * 0.5f) * t) * t) * t)) + from);

            case TYPE.QUARTIC_IN:
                return (((Color) ((((color * t) * t) * t) * t)) + from);

            case TYPE.QUARTIC_OUT:
                t--;
                return (((Color) (color * ((((t * t) * t) * t) - 1f))) + from);

            case TYPE.QUARTIC_IN_OUT:
                t *= 2f;
                if (t >= 1f)
                {
                    t -= 2f;
                    return (((Color) ((color * 0.5f) * ((((t * t) * t) * t) - 2f))) + from);
                }
                return (((Color) (((((color * 0.5f) * t) * t) * t) * t)) + from);

            case TYPE.QUINTIC_IN:
                return (((Color) (((((color * t) * t) * t) * t) * t)) + from);

            case TYPE.QUINTIC_OUT:
                t--;
                return (((Color) (color * (((((t * t) * t) * t) * t) + 1f))) + from);

            case TYPE.QUINTIC_IN_OUT:
                t *= 2f;
                if (t >= 1f)
                {
                    t -= 2f;
                    return (((Color) ((color * 0.5f) * (((((t * t) * t) * t) * t) + 2f))) + from);
                }
                return (((Color) ((((((color * 0.5f) * t) * t) * t) * t) * t)) + from);

            case TYPE.SINUSOIDAL_IN:
                return ((((Color) ((color * -1f) * Mathf.Cos(t * 1.570796f))) + color) + from);

            case TYPE.SINUSOIDAL_OUT:
                return (((Color) (color * Mathf.Sin(t * 1.570796f))) + from);

            case TYPE.SINUSOIDAL_IN_OUT:
                return (((Color) (((color * -1f) / 2f) * (Mathf.Cos(3.141593f * t) - 1f))) + from);

            case TYPE.EXPONENTIAL_IN:
                return (((Color) (color * Mathf.Pow(2f, 10f * (t - 1f)))) + from);

            case TYPE.EXPONENTIAL_OUT:
                return (((Color) (color * (-Mathf.Pow(2f, -10f * t) + 1f))) + from);

            case TYPE.EXPONENTIAL_IN_OUT:
                t *= 2f;
                if (t >= 1f)
                {
                    t--;
                    return (((Color) ((color * 0.5f) * (-Mathf.Pow(2f, -10f * t) + 2f))) + from);
                }
                return (((Color) ((color * 0.5f) * Mathf.Pow(2f, 10f * (t - 1f)))) + from);

            case TYPE.CIRCULAR_IN:
                return (((Color) ((color * -1f) * (Mathf.Sqrt(1f - (t * t)) - 1f))) + from);

            case TYPE.CIRCULAR_OUT:
                t--;
                return (((Color) (color * Mathf.Sqrt(1f - (t * t)))) + from);

            case TYPE.CIRCULAR_IN_OUT:
                t *= 2f;
                if (t >= 1f)
                {
                    t -= 2f;
                    return (((Color) ((color * 0.5f) * (Mathf.Sqrt(1f - (t * t)) + 1f))) + from);
                }
                return (((Color) (((color * -1f) * 0.5f) * (Mathf.Sqrt(1f - (t * t)) - 1f))) + from);
        }
        return color2;
    }

    public static Vector3 Func(Vector3 from, Vector3 to, float t, TYPE type = 0)
    {
        Vector3 vector = to - from;
        Vector3 vector2 = from;
        switch (type)
        {
            case TYPE.LINER:
                return (((Vector3) (vector * t)) + from);

            case TYPE.QUADRATIC_IN:
                return (((Vector3) ((vector * t) * t)) + from);

            case TYPE.QUADRATIC_OUT:
                return (((Vector3) (((vector * -1f) * t) * (t - 2f))) + from);

            case TYPE.QUADRATIC_IN_OUT:
                t *= 2f;
                if (t >= 1f)
                {
                    t--;
                    return (((Vector3) (((vector * -1f) * 0.5f) * ((t * (t - 2f)) - 1f))) + from);
                }
                return (((Vector3) (((vector * 0.5f) * t) * t)) + from);

            case TYPE.CUBIC_IN:
                return (((Vector3) (((vector * t) * t) * t)) + from);

            case TYPE.CUBIC_OUT:
                t--;
                return (((Vector3) (vector * (((t * t) * t) + 1f))) + from);

            case TYPE.CUBIC_IN_OUT:
                t *= 2f;
                if (t >= 1f)
                {
                    t -= 2f;
                    return (((Vector3) ((vector * 0.5f) * (((t * t) * t) + 2f))) + from);
                }
                return (((Vector3) ((((vector * 0.5f) * t) * t) * t)) + from);

            case TYPE.QUARTIC_IN:
                return (((Vector3) ((((vector * t) * t) * t) * t)) + from);

            case TYPE.QUARTIC_OUT:
                t--;
                return (((Vector3) (vector * ((((t * t) * t) * t) - 1f))) + from);

            case TYPE.QUARTIC_IN_OUT:
                t *= 2f;
                if (t >= 1f)
                {
                    t -= 2f;
                    return (((Vector3) ((vector * 0.5f) * ((((t * t) * t) * t) - 2f))) + from);
                }
                return (((Vector3) (((((vector * 0.5f) * t) * t) * t) * t)) + from);

            case TYPE.QUINTIC_IN:
                return (((Vector3) (((((vector * t) * t) * t) * t) * t)) + from);

            case TYPE.QUINTIC_OUT:
                t--;
                return (((Vector3) (vector * (((((t * t) * t) * t) * t) + 1f))) + from);

            case TYPE.QUINTIC_IN_OUT:
                t *= 2f;
                if (t >= 1f)
                {
                    t -= 2f;
                    return (((Vector3) ((vector * 0.5f) * (((((t * t) * t) * t) * t) + 2f))) + from);
                }
                return (((Vector3) ((((((vector * 0.5f) * t) * t) * t) * t) * t)) + from);

            case TYPE.SINUSOIDAL_IN:
                return ((((Vector3) ((vector * -1f) * Mathf.Cos(t * 1.570796f))) + vector) + from);

            case TYPE.SINUSOIDAL_OUT:
                return (((Vector3) (vector * Mathf.Sin(t * 1.570796f))) + from);

            case TYPE.SINUSOIDAL_IN_OUT:
                return (((Vector3) (((vector * -1f) / 2f) * (Mathf.Cos(3.141593f * t) - 1f))) + from);

            case TYPE.EXPONENTIAL_IN:
                return (((Vector3) (vector * Mathf.Pow(2f, 10f * (t - 1f)))) + from);

            case TYPE.EXPONENTIAL_OUT:
                return (((Vector3) (vector * (-Mathf.Pow(2f, -10f * t) + 1f))) + from);

            case TYPE.EXPONENTIAL_IN_OUT:
                t *= 2f;
                if (t >= 1f)
                {
                    t--;
                    return (((Vector3) ((vector * 0.5f) * (-Mathf.Pow(2f, -10f * t) + 2f))) + from);
                }
                return (((Vector3) ((vector * 0.5f) * Mathf.Pow(2f, 10f * (t - 1f)))) + from);

            case TYPE.CIRCULAR_IN:
                return (((Vector3) ((vector * -1f) * (Mathf.Sqrt(1f - (t * t)) - 1f))) + from);

            case TYPE.CIRCULAR_OUT:
                t--;
                return (((Vector3) (vector * Mathf.Sqrt(1f - (t * t)))) + from);

            case TYPE.CIRCULAR_IN_OUT:
                t *= 2f;
                if (t >= 1f)
                {
                    t -= 2f;
                    return (((Vector3) ((vector * 0.5f) * (Mathf.Sqrt(1f - (t * t)) + 1f))) + from);
                }
                return (((Vector3) (((vector * -1f) * 0.5f) * (Mathf.Sqrt(1f - (t * t)) - 1f))) + from);
        }
        return vector2;
    }

    public enum TYPE
    {
        LINER,
        QUADRATIC_IN,
        QUADRATIC_OUT,
        QUADRATIC_IN_OUT,
        CUBIC_IN,
        CUBIC_OUT,
        CUBIC_IN_OUT,
        QUARTIC_IN,
        QUARTIC_OUT,
        QUARTIC_IN_OUT,
        QUINTIC_IN,
        QUINTIC_OUT,
        QUINTIC_IN_OUT,
        SINUSOIDAL_IN,
        SINUSOIDAL_OUT,
        SINUSOIDAL_IN_OUT,
        EXPONENTIAL_IN,
        EXPONENTIAL_OUT,
        EXPONENTIAL_IN_OUT,
        CIRCULAR_IN,
        CIRCULAR_OUT,
        CIRCULAR_IN_OUT
    }
}

