using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Facial Expression", menuName = "Facial Expression")]
public class FacialExpression : ScriptableObject
{
    public new string name;
    public string description;

    public Sprite Eye_Upper;
    #region Eye Upper Vars
    [HideInInspector]
    public Vector3 Eye_Upper_Position;
    [HideInInspector]
    public Quaternion Eye_Upper_Rotation;
    [HideInInspector]
    public Vector3 Eye_Upper_Scale = Vector3.one;
    #endregion 
    public Sprite Eye_Under;
    #region Eye Under Vars
    [HideInInspector]
    public Vector3 Eye_Under_Position;
    [HideInInspector]
    public Quaternion Eye_Under_Rotation;
    [HideInInspector]
    public Vector3 Eye_Under_Scale = Vector3.one;
    #endregion
    public Sprite Pupil_Upper;
    #region Pupil Upper Vars
    [HideInInspector]
    public Vector3 Pupil_Upper_Position;
    [HideInInspector]
    public Quaternion Pupil_Upper_Rotation;
    [HideInInspector]
    public Vector3 Pupil_Upper_Scale = Vector3.one;
    #endregion
    public Sprite Pupil_Under;
    #region Pupil Under Vars
    [HideInInspector]
    public Vector3 Pupil_Under_Position;
    [HideInInspector]
    public Quaternion Pupil_Under_Rotation;
    [HideInInspector]
    public Vector3 Pupil_Under_Scale = Vector3.one;
    #endregion
    public Sprite Mouth;
    #region Mouth Vars
    [HideInInspector]
    public Vector3 Mouth_Position;
    [HideInInspector]
    public Quaternion Mouth_Rotation;
    [HideInInspector]
    public Vector3 Mouth_Scale = Vector3.one;
    #endregion
}
