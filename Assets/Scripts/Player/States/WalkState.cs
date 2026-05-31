using UnityEngine;

public class WalkState : IState
{
    private PlayerController player;

    public WalkState(PlayerController playerController)
    {
        player = playerController;
    }

    public void Enter()
    {
        Debug.Log("Entrando en Walk: animación de caminar.");
        player.animator.SetBool("isMoving", true);
        // Actualiza continuamente los parámetros de dirección del BlendTree
        player.animator.SetFloat("moveX", player.MovementInput.x);
        player.animator.SetFloat("moveY", player.MovementInput.y);

    }

    public void Execute()
    {
        Debug.Log("Caminando...");
        player.ApplyMovement(player.moveSpeed);

       

        if (!player.IsWalking())
        {
            player.stateMachine.TransitionTo(player.stateMachine.idleState);
        }
        if (player.IsRunning())
        {
            Debug.Log("Caminando que no entrooooooooooooooooooooooo...");
            player.stateMachine.TransitionTo(player.stateMachine.runState);
        }

        if (player.TryStartJumping())
        {
            player.stateMachine.TransitionTo(player.stateMachine.jumpState);
        }

        if (player.IsAttacking())
        {
            player.stateMachine.TransitionTo(player.stateMachine.attackState);
            return;
        }
    }

    public void Exit()
    {
        Debug.Log("Saliendo de Walk.");
        player.animator.SetBool("isMoving", false);
    }
}
