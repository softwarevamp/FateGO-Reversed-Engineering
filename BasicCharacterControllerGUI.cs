using System;
using UnityEngine;

public class BasicCharacterControllerGUI : MonoBehaviour
{
    private void OnGUI()
    {
        GUILayout.TextArea("This is a small Sample Scene, showing how you might seamlessly transition from a Dynamic Gameplay camera into a cutscene.", new GUILayoutOption[0]);
        GUILayout.Space(10f);
        GUILayout.TextArea("W, A, S and D : Move Character.", new GUILayoutOption[0]);
        GUILayout.TextArea("Move into the RED trigger volume to trigger the small sequence.", new GUILayoutOption[0]);
    }
}

