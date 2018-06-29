using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class WorldEditorCanvas : MonoBehaviour
{
    public delegate void CanvasEvent();
    public static event CanvasEvent Deleted;
    public static event CanvasEvent Created;

    public Vector2Int size;

    private void OnDestroy()
    {
        if (Deleted != null)
        {
            Deleted();
        }
    }

    private void OnEnable()
    {
        if(Created!=null)
        {
            Created();
        }
    }

    public void SetColliderType(Type col)
    {
        if(col == typeof(BoxCollider))
        {
            if (this.GetComponent<PolygonCollider2D>() != null)
            {
                DestroyImmediate(this.GetComponent<PolygonCollider2D>());
            }
            gameObject.AddComponent<BoxCollider>();
        }
        else if(col == typeof(PolygonCollider2D))
        {
            if (this.GetComponent<BoxCollider>() != null)
            {
                DestroyImmediate(this.GetComponent<BoxCollider>());
            }
            gameObject.AddComponent<PolygonCollider2D>();
        }
    }
}
