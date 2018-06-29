using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BoneType
{
    UpperArm,
    LowerArm,
    Hand
}


[ExecuteInEditMode]
public class IKJoint : MonoBehaviour
{
    public bool On;
    private bool temp;
    public BoneType bone;
    public Transform body;
    public Transform body_Upper;
    public Transform upperArm;
    public Transform lowerArm;
    public Transform hand;
    public Transform wrist;
    public Transform handTarget;
    public string handTextureName;
    private Transform parent;

    public delegate void IKEvent(Transform handtarget);
    public static event IKEvent Enable;
    public static event IKEvent Disable;

    void OnEnable()
    {
            IKJoint.Enable += EnableAll;
            IKJoint.Disable += DisableAll;
    }

    void OnDisable()
    {
        IKJoint.Enable -= EnableAll;
        IKJoint.Disable -= DisableAll;
    }

    void Start()
    {
        temp = On; 
        IKJoint.Enable += EnableAll;
        IKJoint.Disable += DisableAll;
    }

    void LateUpdate()
    {  
        if(Application.isEditor)
        { 
            if (temp != On)
            {
                if (On)
                {
                    temp = On;
                    Enable(handTarget);
                }
                else
                {
                    temp = On;
                    Disable(handTarget);
                }
            }
        }

        if (On)
        {
            Recalculate();
            if (bone == BoneType.Hand && transform.parent != body)
            {
                transform.parent = body_Upper;
            }
            if (bone == BoneType.UpperArm)
            {
                Recalculate();
                GenerateIK();
                IKJoint lower = lowerArm.GetComponent<IKJoint>();
                lower.GenerateIK();
            }
        }
        else
        {
            if(bone == BoneType.Hand && transform.parent != wrist)
            {
                transform.parent = wrist;
                transform.localPosition = Vector3.zero;
            }
        }
    }

    public void Recalculate()
    {
        if(bone == BoneType.Hand)
        {
            transform.position = handTarget.position;
            if(Vector3.Distance(transform.position, upperArm.position) > (Vector3.Distance(upperArm.position, lowerArm.position) + Vector3.Distance(lowerArm.position, wrist.position))-0.05f* (Vector3.Distance(upperArm.position, lowerArm.position) + Vector3.Distance(lowerArm.position, wrist.position)))
            {
                transform.position = upperArm.position + (transform.position - upperArm.position).normalized * ((Vector3.Distance(upperArm.position, lowerArm.position) + Vector3.Distance(lowerArm.position, wrist.position))-0.05f * (Vector3.Distance(upperArm.position, lowerArm.position) + Vector3.Distance(lowerArm.position, wrist.position)));
            }
        }
    }

    void OnDrawGizmos()
    {
        if (bone == BoneType.LowerArm)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(upperArm.position, hand.position);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(hand.position, hand.position + ((upperArm.position - hand.position).normalized * (Vector3.Distance(upperArm.position, hand.position) * 0.5f)));
            Gizmos.color = Color.green;
            Gizmos.DrawLine(hand.position + ((upperArm.position - hand.position).normalized * (Vector3.Distance(upperArm.position, hand.position) * 0.5f)), lowerArm.position);
            Gizmos.DrawIcon(handTarget.position, handTextureName + ".png", true);
        }
    }

    public void GenerateIK()
    {
        if(bone == BoneType.UpperArm)
        {
            float hypotenuse = Vector3.Distance(upperArm.position, lowerArm.position);
            float adjacent = Vector3.Distance(upperArm.position, hand.position)/2;
            float opposite = Mathf.Sqrt(Mathf.Pow(hypotenuse, 2) - Mathf.Pow(adjacent, 2));
            float angle = Mathf.Rad2Deg*(opposite / hypotenuse);

            
            Vector3 diference = hand.position - upperArm.position; 
            float sign = (upperArm.position.x < hand.position.x) ? -1.0f : 1.0f;
            float orientation = (body.localScale.x < 0) ? -1.0f : 1.0f;

            Quaternion temp = Quaternion.LookRotation(diference, Vector3.forward);
            transform.rotation = Quaternion.Euler(0, 0, (sign * temp.eulerAngles.x + (90 * -sign) - (angle*1.3f)*orientation));
        }
        else if(bone == BoneType.LowerArm)
        {
            transform.localScale = new Vector3(1, 1, 1);
            float distance = Vector3.Distance(transform.position, wrist.position);
            Vector3 diference = hand.position - lowerArm.position;
            Quaternion temp = Quaternion.LookRotation(diference, Vector3.forward);
            float sign = (lowerArm.position.x < hand.position.x) ? -1.0f : 1.0f;
            transform.rotation = Quaternion.Euler(0, 0, (sign * (temp.eulerAngles.x) + (90 * -sign)));
            //transform.localScale = new Vector3(1, (Vector3.Distance(transform.position, hand.position)/distance), 1);
        }
    }

    void EnableAll(Transform handtarget)
    {
        if (handtarget == handTarget)
        {
            On = true;
            temp = true;
        }
    }

    void DisableAll(Transform handtarget)
    {
        if(handtarget == handTarget)
        {
            On = false;
            temp = false;
        }
    }
}
