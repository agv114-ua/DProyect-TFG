using UnityEngine;
using System;
public class PlayerHealth : HealthBase
{
    
    public bool CanBeHealed => Health < maxHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        NotifyHealthChanged();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(10);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Heal(10);
        }
    }   

    
    // Restaurar Salud 
    public void Heal(float amount)
    {
        if( CanBeHealed) // el personaje en este momento puede ser curado si la Salud no es inferior a la salud máxima 
        {
            Health += amount;
            
            if ( Health > maxHealth )
            {
                Health = maxHealth;
            }

            NotifyHealthChanged();
        }
    }
    protected override void Deleted()
    {
        Debug.Log("El jugador ha muerto.");
    }

    
}
