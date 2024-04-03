using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigMovement : PathFollower
{
    override public void Start()
    {
        speed = 6.0f;
        base.Start();        
    }
}
