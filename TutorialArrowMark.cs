using System;
using UnityEngine;

public class TutorialArrowMark : BaseMonoBehaviour
{
    [SerializeField]
    protected UILabel messageLabel;
    [SerializeField]
    protected Transform rotation;

    public void Init(Vector2 pos, float way)
    {
        this.messageLabel.text = LocalizationManager.Get("TUTORIAL_ARROW_MARK_MESSAGE");
        this.rotation.localRotation = Quaternion.Euler(0f, 0f, way);
        base.transform.localPosition = (Vector3) pos;
    }
}

