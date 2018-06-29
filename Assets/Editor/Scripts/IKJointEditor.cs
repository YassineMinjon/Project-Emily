using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(IKJoint))]
public class IKJointEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        IKJoint myScript = (IKJoint)target;
        if (GUILayout.Button("Update Bones"))
        {
            //myScript.UpdateBones();
        }
    }
}
