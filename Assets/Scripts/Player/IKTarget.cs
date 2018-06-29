using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Hand
{
    Left,
    Right,
    Both
}

public class IKTarget : MonoBehaviour
{
    public Hand hand;
    public Transform body;
    public bool hasTarget;
    public Vector3 origPos;
    public Vector3 targetPos;
    private Vector3 vel;
    private Vector3 startPos;
    private float timer;

    void Start()
    {
        origPos = transform.localPosition;
    }

    void LateUpdate()
    {
        if(hasTarget)
        {
            if (Vector3.Distance(transform.position, targetPos) >= 0.05f)
            {
                transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref vel, 0.1f);
            }
            else
            {
                transform.position = targetPos;
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, origPos) >= 0.05f)
            {
                transform.localPosition = Vector3.SmoothDamp(transform.localPosition, origPos, ref vel, 0.1f);
            }
            else
            {
                transform.localPosition = origPos;
            }
        }
    }
}
