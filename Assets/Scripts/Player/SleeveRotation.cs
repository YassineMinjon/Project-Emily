using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SleeveRotation : MonoBehaviour
{
    void LateUpdate()
    {
        Vector3 dir = transform.GetChild(0).transform.position - transform.position;
        dir = transform.GetChild(0).transform.InverseTransformDirection(dir);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Debug.Log(angle);
        //transform.rotation = transform.GetChild(0).transform.rotation;
    }
}
