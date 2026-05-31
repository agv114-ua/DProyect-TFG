using UnityEngine;

/**
 * Interfaz que unifica todo lo que puede recibir daño
 * Jugador, enemigos, 
 * */
public interface IDamageable
{
    void TakeDamage(float amount);
}
