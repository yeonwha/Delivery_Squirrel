using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VultureMovement : PathFollower
{
    override public void Start()
    {
        speed = 5.0f;
        base.Start();        
    }
}
