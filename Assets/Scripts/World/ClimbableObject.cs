using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ClimbableObject : MonoBehaviour
{
    private BoxCollider2D col;

    void OnDrawGizmos()
    {
        col = GetComponent<BoxCollider2D>();
        Gizmos.color = Color.red;
        Gizmos.DrawCube(new Vector3(col.bounds.min.x, col.bounds.max.y, transform.position.z), new Vector3(0.1F, 0.1F, gameObject.transform.localScale.z));
        Gizmos.DrawCube(new Vector3(col.bounds.max.x, col.bounds.max.y, transform.position.z), new Vector3(0.1F, 0.1F, gameObject.transform.localScale.z));
    }
}
