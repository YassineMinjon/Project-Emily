using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKCenter : MonoBehaviour
{

    public IKTarget leftHand;
    public IKTarget rightHand;

    void OnTriggerStay(Collider other)
    {
        Debug.Log("e");
        if (other.GetComponent<IKTrigger>())
        {
            if (other.GetComponent<IKTrigger>().hand == Hand.Both)
            {
                leftHand.hasTarget = true;
                leftHand.targetPos = other.GetComponent<IKTrigger>().targetPosition;
                rightHand.hasTarget = true;
                rightHand.targetPos = other.GetComponent<IKTrigger>().targetPosition;
            }
            else if (other.GetComponent<IKTrigger>().hand == Hand.Left)
            {
                leftHand.hasTarget = true;
                leftHand.targetPos = other.GetComponent<IKTrigger>().targetPosition;
            }
            else
            {
                rightHand.hasTarget = true;
                rightHand.targetPos = other.GetComponent<IKTrigger>().targetPosition;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<IKTrigger>())
        {     
                if (other.GetComponent<IKTrigger>().hand == Hand.Both)
                {
                    leftHand.hasTarget = false;
                    rightHand.hasTarget = false;
                }
                else if (other.GetComponent<IKTrigger>().hand == Hand.Left)
                {
                    leftHand.hasTarget = false;
                }
                else
                {
                    rightHand.hasTarget = false;
                }
        }
    }
}
