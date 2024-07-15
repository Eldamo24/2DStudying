using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : State
{
    public override void Enter()
    {
        playerController.PlayerAnim.Play("Run");
    }

    public override void Do()
    {
        if(!playerController.IsGrounded || playerController.MoveVector == Vector2.zero)
        {
            isComplete = true;
        }
    }

    public override void Exit()
    {

    }
}
