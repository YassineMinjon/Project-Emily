using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class IKTrigger : MonoBehaviour
{
    public Hand hand;
    public Vector3 offset;
    public Vector3 targetPosition;

    void Update()
    {
        if (Application.isEditor)
        {
            targetPosition = transform.position + offset;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(targetPosition, 0.1f);
    }
}
