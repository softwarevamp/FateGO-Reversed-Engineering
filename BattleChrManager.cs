using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BattleChrManager : SingletonMonoBehaviour<BattleChrManager>
{
    [CompilerGenerated]
    private static Func<Transform, bool> <>f__am$cache4;
    public static readonly float animFps = 30f;
    private static readonly string levelMarker = "_level";
    public GameObject mayaPrefab;
    public Transform rootTransform;

    public void AttachAnimationEvents(GameObject gameObject, TextAsset data, string servantName, int level)
    {
        if (gameObject.GetComponent<BattleChrControl>() == null)
        {
            gameObject.AddComponent<BattleChrControl>();
        }
        char[] separator = new char[] { '\r', '\n' };
        string[] strArray = data.text.Split(separator);
        for (int i = 0; i < strArray.Length; i++)
        {
            char[] chArray2 = new char[] { ","[0] };
            string[] strArray2 = strArray[i].Split(chArray2);
            if (((strArray2.Length <= 0) || !strArray2[0].StartsWith("#")) && (strArray2.Length > 5))
            {
                int num2 = int.Parse(strArray2[1]);
                string name = strArray2[2];
                if ((num2 == level) && (gameObject.GetComponent<Animation>().GetClip(name) != null))
                {
                    for (int j = 3; j < strArray2.Length; j += 3)
                    {
                        if (strArray2[j].Length == 0)
                        {
                            break;
                        }
                        float num4 = float.Parse(strArray2[j]) / animFps;
                        string str2 = strArray2[j + 1];
                        string str3 = strArray2[j + 2];
                        AnimationEvent event2 = new AnimationEvent {
                            time = num4,
                            stringParameter = str2 + ":" + str3,
                            functionName = "OnAnimEvent"
                        };
                    }
                }
            }
        }
    }

    protected void Awake()
    {
        if (this != SingletonMonoBehaviour<BattleChrManager>.Instance)
        {
            UnityEngine.Object.Destroy(this);
        }
    }

    public void SetEvolutionLevel(GameObject gameObject, int level)
    {
        if (<>f__am$cache4 == null)
        {
            <>f__am$cache4 = p => p.gameObject.name.Contains(levelMarker);
        }
        IEnumerator<Transform> enumerator = gameObject.GetComponentsInChildren<Transform>().Where<Transform>(<>f__am$cache4).GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                Transform current = enumerator.Current;
                bool flag = false;
                int startIndex = current.name.IndexOf(levelMarker) + 6;
                char[] separator = new char[] { '_' };
                foreach (string str2 in current.name.Substring(startIndex).Split(separator))
                {
                    if (int.Parse(str2) == level)
                    {
                        flag = true;
                        break;
                    }
                }
                current.GetComponent<MeshRenderer>().enabled = flag;
            }
        }
        finally
        {
            if (enumerator == null)
            {
            }
            enumerator.Dispose();
        }
    }

    private void Start()
    {
        GameObject gameObject = UnityEngine.Object.Instantiate(this.mayaPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        gameObject.layer = 11;
        gameObject.transform.parent = this.rootTransform;
        gameObject.transform.localPosition = new Vector3(-30.67678f, 0.3551643f, -1703.885f);
        gameObject.transform.localRotation = Quaternion.Euler(0f, 270f, 0f);
        gameObject.transform.localScale = new Vector3(20f, 20f, 20f);
        this.SetEvolutionLevel(gameObject, 2);
        TextAsset data = Resources.Load("Maya/fbxevent_arthur", typeof(TextAsset)) as TextAsset;
        this.AttachAnimationEvents(gameObject, data, "アルトリア", 1);
    }
}

