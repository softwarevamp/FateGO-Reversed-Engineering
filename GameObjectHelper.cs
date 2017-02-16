using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

internal static class GameObjectHelper
{
    public static void addNguiDepth(this GameObject self, int depth, bool flg = false)
    {
        IEnumerator enumerator = self.transform.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                Transform current = (Transform) enumerator.Current;
                UIWidget component = current.GetComponent<UIWidget>();
                if (component != null)
                {
                    component.depth += depth;
                }
                if (flg)
                {
                    current.gameObject.addNguiDepth(depth, flg);
                }
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
}

