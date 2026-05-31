using UnityEngine;

public class JumpState : IState
{
    private PlayerController player;

    public JumpState(PlayerController playerController)
    {
        player = playerController;
    }

    public void Enter()
    {
        Debug.Log("Entrando en Jump: animación de salto.");
        player.animator.SetBool("isGrounded", false);
        player.animator.SetBool("tryStartJump", true);
        player.animator.SetTrigger("jump");
    }

    public void Execute()
    {
        Debug.Log("Aplicando física de salto.");

        // Movemos el SPRITE (Body) hacia arriba, pero el Rigidbody se queda en el suelo
        if (player.bodyTransform != null)
        {
            player.bodyTransform.localPosition = new Vector3(0, player.CurrentHeight, 0);
        }

        // Efecto visual: La sombra se hace pequeña al saltar
        if (player.shadowTransform != null)
        {
            // Cuanto más alto, más pequeña la sombra (mínimo 50% de tamaño)
            float scale = Mathf.Clamp(1f - (player.CurrentHeight * 0.15f), 0.5f, 1f);
            player.shadowTransform.localScale = new Vector3(scale, scale, 1f);
        }

        // Al aterrizar, vuelve al estado apropiado según el input
        if (player.HasLanded())
        {
            player.animator.SetBool("isGrounded", true);
            player.animator.SetBool("tryStartJump", false);
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

            if (player.IsAttacking())
            {
                player.stateMachine.TransitionTo(player.stateMachine.attackState);
                return;
            }
        }
    }

    public void Exit()
    {
        Debug.Log("Saliendo de Jump.");
        player.animator.SetBool("isGrounded", true);
        player.rb.linearVelocity = Vector2.zero;
    }
}
