using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour
{
    public AnimationController animController;
    public List<Movement> movementList = new List<Movement>();
    private Movement currentMovement;
    private Controller2D controller;

    void Awake()
    {
        controller = GetComponent<Controller2D>();
        if(currentMovement==null)
        {
            SetMovementMode(movementList[0]);
        }
    }

    void Update()
    {
        int highestPriority = 0;
        foreach(Movement movement in movementList)
        {
            int priority = movement.Condition();
            if (priority >= highestPriority)
            {
                SetMovementMode(movement);
                highestPriority = priority;
            }
        }

        if(highestPriority == 0)
        {
            currentMovement = null;
        }

        if (currentMovement != null)
        {
            currentMovement.Execute();
        }
    }

    public void SetMovementMode(Movement newMovement)
    {
        currentMovement = newMovement;
        currentMovement.Init(controller);
        currentMovement.animController = animController;
    }
}
