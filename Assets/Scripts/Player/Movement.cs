using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public abstract class Movement : ScriptableObject
{
    public AnimationController animController;
    protected Controller2D controller;
    public int priority;
    abstract public void Execute();
    abstract public void Init(Controller2D controller);
    abstract public int Condition();
}
