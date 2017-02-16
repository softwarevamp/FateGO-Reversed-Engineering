using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[ExecuteInEditMode, RequireComponent(typeof(UIWidget)), AddComponentMenu("NGUI/Examples/Set Color on Selection")]
public class SetColorOnSelection : MonoBehaviour
{
    [CompilerGenerated]
    private static Dictionary<string, int> <>f__switch$map35;
    private UIWidget mWidget;

    public void SetSpriteBySelection()
    {
        if (UIPopupList.current != null)
        {
            if (this.mWidget == null)
            {
                this.mWidget = base.GetComponent<UIWidget>();
            }
            string key = UIPopupList.current.value;
            if (key != null)
            {
                int num;
                if (<>f__switch$map35 == null)
                {
                    Dictionary<string, int> dictionary = new Dictionary<string, int>(7) {
                        { 
                            "White",
                            0
                        },
                        { 
                            "Red",
                            1
                        },
                        { 
                            "Green",
                            2
                        },
                        { 
                            "Blue",
                            3
                        },
                        { 
                            "Yellow",
                            4
                        },
                        { 
                            "Cyan",
                            5
                        },
                        { 
                            "Magenta",
                            6
                        }
                    };
                    <>f__switch$map35 = dictionary;
                }
                if (<>f__switch$map35.TryGetValue(key, out num))
                {
                    switch (num)
                    {
                        case 0:
                            this.mWidget.color = Color.white;
                            break;

                        case 1:
                            this.mWidget.color = Color.red;
                            break;

                        case 2:
                            this.mWidget.color = Color.green;
                            break;

                        case 3:
                            this.mWidget.color = Color.blue;
                            break;

                        case 4:
                            this.mWidget.color = Color.yellow;
                            break;

                        case 5:
                            this.mWidget.color = Color.cyan;
                            break;

                        case 6:
                            this.mWidget.color = Color.magenta;
                            break;
                    }
                }
            }
        }
    }
}

