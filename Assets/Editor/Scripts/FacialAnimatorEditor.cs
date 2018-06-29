using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FacialAnimator))]
public class FacialAnimatorEditor : Editor
{
    private string _faceName;
    private int _choiceIndex;
    private string[] _choices;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        FacialAnimator anim = (FacialAnimator)target;
        _choices = new string[anim.Faces.Count];
        for (int i = 0; i < _choices.Length; i++)
        {
            if (anim.Faces[i] != null)
            {
                _choices[i] = anim.Faces[i].name;
            }
        }
        _choiceIndex = EditorGUILayout.Popup(_choiceIndex, _choices);
        if(GUILayout.Button("Preview Facial Expression"))
        {
            anim.PreviewFacialExpression(anim.Faces[_choiceIndex]);
        }
        if (GUILayout.Button("Save Edited Facial Features"))
        {
            anim.SaveEditedFace();
        }
    }
}
