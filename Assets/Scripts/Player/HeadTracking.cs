using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class HeadTracking : MonoBehaviour
{
    public bool isLooking;
    public Vector3 focusPoint;
    public Transform body;
    private Vector3 velocity;
    Quaternion rotation;
    
	void LateUpdate()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            isLooking = true;
        }
        else
        {
            isLooking = false;
        }
        if(isLooking)
        {
            transform.localScale = new Vector3((focusPoint.x > transform.position.x) ? 1f * body.localScale.x : -1f * body.localScale.x, 1, 1);
            Vector3 diference = focusPoint - transform.position;
            Quaternion temp = Quaternion.LookRotation(diference, Vector3.forward);
            rotation.eulerAngles = new Vector3(0, 0,temp.eulerAngles.x * ((focusPoint.x > transform.position.x) ? -1f : 1f));
            transform.eulerAngles = rotation.eulerAngles;
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, focusPoint);
    }
}
