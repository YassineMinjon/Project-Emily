using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class FacialAnimator : MonoBehaviour
{
    private FacialExpression current;
    public SpriteRenderer Eye_Upper;
    public SpriteRenderer Eye_Under;
    public SpriteRenderer Pupil_Upper;
    public SpriteRenderer Pupil_Under;
    public SpriteRenderer Mouth;
    Vector3 upperScale;
    Vector3 underScale;
    bool blinking = false;
    private float timer;
    private float intervall = 1f;

    public List<FacialExpression> Faces = new List<FacialExpression>();

    private void Start()
    {
        if (Pupil_Upper != null)
        {
            upperScale = Pupil_Upper.transform.localScale;
        }
        if (Pupil_Under != null)
        {
            underScale = Pupil_Under.transform.localScale;
        }
    }

    private void Update()
    {
        if (Application.isPlaying) {
            if (!blinking)
            {
                timer += Time.deltaTime;
                if (timer >= intervall)
                {
                    StopAllCoroutines();
                    StartCoroutine(Blink());
                    timer = 0;
                    intervall = Random.Range(1f, 4f);
                }
            }
        }
    }

    public IEnumerator Blink()
    {
        blinking = true;
        float duration = 0.0001f;
        for(int i = 10; i >= 0; i--)
        {
            Pupil_Upper.transform.localScale = new Vector3(upperScale.x, upperScale.y * (float)i / 10, upperScale.z);
            Pupil_Under.transform.localScale = new Vector3(underScale.x, underScale.y * (float)i / 10, underScale.z);
            yield return new WaitForSecondsRealtime(duration);
        }
        for (int i = 0; i < 10; i++)
        {
            Pupil_Upper.transform.localScale = new Vector3(upperScale.x, upperScale.y * (float)i / 10, upperScale.z);
            Pupil_Under.transform.localScale = new Vector3(underScale.x, underScale.y * (float)i / 10, underScale.z);
            yield return new WaitForSecondsRealtime(duration);
        }
        Pupil_Upper.transform.localScale = new Vector3(upperScale.x, upperScale.y * 1f, upperScale.z);
        Pupil_Under.transform.localScale = new Vector3(underScale.x, underScale.y * 1f, underScale.z);
        blinking = false;
    }

    public void PreviewFacialExpression(FacialExpression expression)
    {
        Debug.Log(expression.name);
        current = expression;

        Eye_Upper.sprite = expression.Eye_Upper;
        Eye_Upper.transform.localPosition = current.Eye_Upper_Position;
        Eye_Upper.transform.localRotation = current.Eye_Upper_Rotation;
        Eye_Upper.transform.localScale = current.Eye_Upper_Scale;

        Eye_Under.sprite = expression.Eye_Under;
        Eye_Under.transform.localPosition = current.Eye_Under_Position;
        Eye_Under.transform.localRotation = current.Eye_Under_Rotation;
        Eye_Under.transform.localScale = current.Eye_Under_Scale;

        Pupil_Upper.sprite = expression.Pupil_Upper;
        Pupil_Upper.transform.localPosition = current.Pupil_Upper_Position;
        Pupil_Upper.transform.localRotation = current.Pupil_Upper_Rotation;
        Pupil_Upper.transform.localScale = current.Pupil_Upper_Scale;

        Pupil_Under.sprite = expression.Pupil_Under;
        Pupil_Under.transform.localPosition = current.Pupil_Under_Position;
        Pupil_Under.transform.localRotation = current.Pupil_Under_Rotation;
        Pupil_Under.transform.localScale = current.Pupil_Under_Scale;

        Mouth.sprite = expression.Mouth;
        Mouth.transform.localPosition = current.Mouth_Position;
        Mouth.transform.localRotation = current.Mouth_Rotation;
        Mouth.transform.localScale = current.Mouth_Scale;
    }

    public void SaveEditedFace()
    {
        current.Eye_Upper_Position = Eye_Upper.transform.localPosition;
        current.Eye_Upper_Rotation = Eye_Upper.transform.localRotation;
        current.Eye_Upper_Scale = Eye_Upper.transform.localScale;

        current.Eye_Under_Position = Eye_Under.transform.localPosition;
        current.Eye_Under_Rotation = Eye_Under.transform.localRotation;
        current.Eye_Under_Scale = Eye_Under.transform.localScale;

        current.Pupil_Upper_Position = Pupil_Upper.transform.localPosition;
        current.Pupil_Upper_Rotation = Pupil_Upper.transform.localRotation;
        current.Pupil_Upper_Scale = Pupil_Upper.transform.localScale;

        current.Pupil_Under_Position = Pupil_Under.transform.localPosition;
        current.Pupil_Under_Rotation = Pupil_Under.transform.localRotation;
        current.Pupil_Under_Scale = Pupil_Under.transform.localScale;

        current.Mouth_Position = Mouth.transform.localPosition;
        current.Mouth_Rotation = Mouth.transform.localRotation;
        current.Mouth_Scale = Mouth.transform.localScale;

        string path = AssetDatabase.GetAssetPath(current);
        ScriptableObject location = AssetDatabase.LoadMainAssetAtPath(path) as ScriptableObject;
        EditorUtility.CopySerialized(current, location);
        AssetDatabase.SaveAssets();
    }
}

