using UnityEngine;


public class IdleState : IState
{
    private PlayerController player;

    public IdleState(PlayerController playerController)
    {
        player = playerController;
    }

    public void Enter()
    {
        player.rb.linearVelocity = Vector2.zero;
        Debug.Log("Entrando en Idle: animación de parado.");

    }

    public void Execute()
    {
        Debug.Log("Esperando input...");

        // Primero verifica si está corriendo (Shift + movimiento)
        if (player.IsRunning())
        {
           
            player.stateMachine.TransitionTo(player.stateMachine.runState);
            return;
        }
        // Luego verifica si está caminando (solo movimiento)
        else if (player.IsWalking())
        {
            player.stateMachine.TransitionTo(player.stateMachine.walkState);
            return;
        }

        // Saltar se puede hacer en cualquier momento
        if (player.TryStartJumping())
        {
            player.stateMachine.TransitionTo(player.stateMachine.jumpState);
            return;
        }

        if (player.IsAttacking() )
        {
            player.stateMachine.TransitionTo(player.stateMachine.attackState);
            return;
        }
    }

    public void Exit()
    {
       Debug.Log("Saliendo de Idle.");
    }
}
