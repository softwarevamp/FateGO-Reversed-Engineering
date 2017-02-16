using System;
using UnityEngine;

[RequireComponent(typeof(UIInput)), AddComponentMenu("NGUI/Examples/Chat Input")]
public class ChatInput : MonoBehaviour
{
    public bool fillWithDummyData;
    private UIInput mInput;
    public UITextList textList;

    public void OnSubmit()
    {
        if (this.textList != null)
        {
            string str = NGUIText.StripSymbols(this.mInput.value);
            if (!string.IsNullOrEmpty(str))
            {
                this.textList.Add(str);
                this.mInput.value = string.Empty;
                this.mInput.isSelected = false;
            }
        }
    }

    private void Start()
    {
        this.mInput = base.GetComponent<UIInput>();
        this.mInput.label.maxLineCount = 1;
        if (this.fillWithDummyData && (this.textList != null))
        {
            for (int i = 0; i < 30; i++)
            {
                this.textList.Add(string.Concat(new object[] { ((i % 2) != 0) ? "[AAAAAA]" : "[FFFFFF]", "This is an example paragraph for the text list, testing line ", i, "[-]" }));
            }
        }
    }
}

