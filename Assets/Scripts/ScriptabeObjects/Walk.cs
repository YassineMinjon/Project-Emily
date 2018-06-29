using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWalk", menuName = "Movement / Walk", order = 50)]
public class Walk : Movement
{
    public float jumpHeight = 4f;
    public float timeToJumpApex = 0.4f;
    float accelerationTimeAirborne = 0.2f;
    float accelerationTimeGrounded = 0.1f;
    public float moveSpeed = 2f;
    float gravity = -20f;
    float jumpVelocity = 8f;
    Vector3 velocity;
    float velocityXSmoothing;

    public override void Init(Controller2D controller)
    {
        this.controller = controller;
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
    }

    public override void Execute()
    {
        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        animController.playerInput = input;

        if (Input.GetKeyDown(KeyCode.Space) && controller.collisions.below)
        {
            velocity.y = jumpVelocity;
            animController.isJumping = true;
        }

        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public override int Condition()
    {
        //if(controller.collisions.below)
        //{
            return priority;
        //}
        //else
        //{
            //return -1;
       // }
    }
}
