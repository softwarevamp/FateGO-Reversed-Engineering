using System;
using UnityEngine;

public class ScriptPosition
{
    protected static Vector2[] charaOffsetList = new Vector2[] { new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(0f, 0f) };
    protected static Vector2[] positionList = new Vector2[] { new Vector2(-256f, 0f), new Vector2(0f, 0f), new Vector2(256f, 0f), new Vector2(-438f, 0f), new Vector2(-512f, 0f), new Vector2(438f, 0f), new Vector2(512f, 0f) };

    public static Vector3 GetCharaOffset(int index)
    {
        if ((index < 0) || (index >= charaOffsetList.Length))
        {
            Debug.LogError("chara offset index error " + index);
            index = 0;
        }
        Vector2 vector = charaOffsetList[index];
        return new Vector3(vector.x, vector.y, 0f);
    }

    public static Vector3 GetCharaOffset(float x, float y) => 
        new Vector3(x, y, 0f);

    public static Vector3 GetPosition(int index)
    {
        if ((index < 0) || (index >= positionList.Length))
        {
            Debug.LogError("position index error " + index);
            index = 0;
        }
        Vector2 vector = positionList[index];
        return new Vector3(vector.x, vector.y, 0f);
    }

    public static Vector3 GetPosition(float x, float y) => 
        new Vector3(x, y, 0f);
}

