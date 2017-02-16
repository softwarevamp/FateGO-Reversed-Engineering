using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

internal static class TransformHelper
{
    public static int ChangeChildsLayer(this Transform self, int layer)
    {
        self.gameObject.layer = layer;
        IEnumerator enumerator = self.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                ((Transform) enumerator.Current).ChangeChildsLayer(layer);
            }
        }
        finally
        {
            IDisposable disposable = enumerator as IDisposable;
            if (disposable == null)
            {
            }
            disposable.Dispose();
        }
        return layer;
    }

    public static void DestroyChildren(this Transform self)
    {
        IEnumerator enumerator = self.transform.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                Transform current = (Transform) enumerator.Current;
                UnityEngine.Object.Destroy(current.gameObject);
            }
        }
        finally
        {
            IDisposable disposable = enumerator as IDisposable;
            if (disposable == null)
            {
            }
            disposable.Dispose();
        }
    }

    public static Transform getNodeFromLvName(this Transform self, string nodename, int level = -1)
    {
        <getNodeFromLvName>c__AnonStorey6A storeya = new <getNodeFromLvName>c__AnonStorey6A {
            nodename = nodename
        };
        if ((storeya.nodename == null) || storeya.nodename.Equals(string.Empty))
        {
            return self;
        }
        BattleActorControl component = self.GetComponent<BattleActorControl>();
        if ((component != null) && (level == -1))
        {
            level = component.LimitImageIndex + 1;
        }
        IEnumerable<Transform> enumerable = self.GetComponentsInChildren<Transform>().Where<Transform>(new Func<Transform, bool>(storeya.<>m__75));
        List<Transform> list = new List<Transform>();
        List<Transform> list2 = new List<Transform>();
        Transform item = null;
        IEnumerator<Transform> enumerator = enumerable.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                Transform current = enumerator.Current;
                bool flag = false;
                item = current;
                if (!current.name.Contains("_level"))
                {
                    list2.Add(item);
                    continue;
                }
                int startIndex = current.name.IndexOf("_level") + 7;
                string str = current.name.Substring(startIndex);
                if (str.IndexOf(" ") >= 0)
                {
                    str = str.Substring(0, str.IndexOf(" "));
                }
                char[] separator = new char[] { '_' };
                foreach (string str2 in str.Split(separator))
                {
                    int result = 0x63;
                    if (int.TryParse(str2, out result) && (int.Parse(str2) == level))
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag)
                {
                    list.Add(current);
                }
            }
        }
        finally
        {
            if (enumerator == null)
            {
            }
            enumerator.Dispose();
        }
        List<Transform> list3 = list2;
        if (list.Count > 0)
        {
            list3 = list;
        }
        float z = -100f;
        Transform transform3 = null;
        foreach (Transform transform4 in list3)
        {
            if (transform4.lossyScale.z > z)
            {
                transform3 = transform4;
                z = transform4.lossyScale.z;
            }
            else if ((transform4.lossyScale.z == z) && !transform4.name.Contains(" "))
            {
                transform3 = transform4;
            }
        }
        return transform3;
    }

    public static Transform getNodeFromName(this Transform self, string nodename, bool includeInactive = false)
    {
        <getNodeFromName>c__AnonStorey6B storeyb = new <getNodeFromName>c__AnonStorey6B {
            nodename = nodename
        };
        IEnumerable<Transform> enumerable = self.GetComponentsInChildren<Transform>(includeInactive).Where<Transform>(new Func<Transform, bool>(storeyb.<>m__76));
        IEnumerator<Transform> enumerator = enumerable.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                return enumerator.Current;
            }
        }
        finally
        {
            if (enumerator == null)
            {
            }
            enumerator.Dispose();
        }
        return null;
    }

    [CompilerGenerated]
    private sealed class <getNodeFromLvName>c__AnonStorey6A
    {
        internal string nodename;

        internal bool <>m__75(Transform p) => 
            p.gameObject.name.Contains(this.nodename);
    }

    [CompilerGenerated]
    private sealed class <getNodeFromName>c__AnonStorey6B
    {
        internal string nodename;

        internal bool <>m__76(Transform p) => 
            p.gameObject.name.Equals(this.nodename);
    }
}

