using UnityEngine;

public class RunState : IState
{
    private PlayerController player;

    public RunState(PlayerController playerController)
    {
        player = playerController;
    }

    public void Enter()
    {
        Debug.Log("Entrando en Run: animación de correr.");
        player.animator.SetBool("isRunning", true);
        // Actualiza continuamente los parámetros de dirección del BlendTree
        player.animator.SetFloat("moveX", player.MovementInput.x);
        player.animator.SetFloat("moveY", player.MovementInput.y);
       
    }

    public void Execute()
    {
        Debug.Log("Corriendo...");
        player.ApplyMovement(player.runSpeed);

        // Si sueltas Shift, comprueba si hay movimiento para decidir el siguiente estado
        if (!player.IsRunning())
        {
            if (player.IsWalking())
            {
                player.stateMachine.TransitionTo(player.stateMachine.walkState);
            }
            else
            {
                player.stateMachine.TransitionTo(player.stateMachine.idleState);
            }
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
        Debug.Log("Saliendo de Run.");
        player.animator.SetBool("isRunning", false);
    }
}
