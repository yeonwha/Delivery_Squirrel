using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpossumMovement : PathFollower
{
    override public void Start()
    {
        speed = 4.0f;
        base.Start();
    }
}
