using UnityEngine;

/**
 * Interfaz Strategy (GoF, Gamma et al. 1994, p.315)
 * 
 * */
public interface IAttackStrategy
{
    int GetDamage();    // Cada estrategia define su propio daño
    float GetRange();   // Alcance del ataque
    float GetCooldown();// Tiempo entre ataques
    void Execute(PlayerController player);

    
}
