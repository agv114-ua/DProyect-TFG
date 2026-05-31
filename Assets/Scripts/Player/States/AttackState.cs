using UnityEngine;

public class AttackState : IState
{
    private PlayerController player;
  

    public AttackState(PlayerController playerController)
    {
        player = playerController;
    }

    public void Enter()
    {
        Debug.Log("Entrando en animación de ataque");
        player.animator.SetTrigger("atacando");

        // Ejecutamos la estrategia activa del jugador
        if(player.CurrentAttackStrategy != null)
        {
            player.CurrentAttackStrategy.Execute(player);
        }
    }

    public void Execute()
    {

        if (player.IsRunning())
        {
            player.stateMachine.TransitionTo(player.stateMachine.runState);
        }
        else if (player.IsWalking())
        {
            player.stateMachine.TransitionTo(player.stateMachine.walkState);
        }
        else
        {
            player.stateMachine.TransitionTo(player.stateMachine.idleState);
        }
    }

    public void Exit()
    {
        Debug.Log("Saliendo....");
    }
}