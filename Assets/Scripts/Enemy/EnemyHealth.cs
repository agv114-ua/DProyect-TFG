using UnityEngine;

/**
 * Vida del enemigo. Hereda de HealthBase
 * Cuando la vida llega a 0, se inicia animación de muerte para posteriormente destruirse
 */
public class EnemyHealth : HealthBase
{
    protected override void Deleted()
    {
        Debug.Log($"Enemigo {gameObject.name} ha muerto.");
        Destroy(gameObject);
    }
}
