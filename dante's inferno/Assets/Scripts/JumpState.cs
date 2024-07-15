using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : State
{
    public override void Enter()
    {
        playerController.PlayerAnim.Play("Jump");
    }

    public override void Do()
    {
        if (playerController.IsGrounded)
        {
            isComplete = true;
        }
    }

    public override void Exit()
    {

    }
}
