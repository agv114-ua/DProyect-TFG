using System;       
using UnityEngine;

public class HealthBase : MonoBehaviour, IDamageable
{

    [SerializeField] protected float initialHealth;
    [SerializeField] protected float maxHealth;

    public float Health { get; protected set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public event Action<float, float> OnHealthChanged;
    public event Action OnDeath;

    protected virtual void Start()
    {
        Health = initialHealth;
    }

    public void TakeDamage(float amount)
    {
        if (amount <= 0) { return;}

        if (Health > 0f)
        {
            Health -= amount;
            OnHealthChanged?.Invoke(Health, maxHealth); // Dispara el evento solo si hay al menos un subscriptor 

            // Si el personaje se ha quedado sin vida, es decir, ha muerto ...
            if ( Health <= 0f)  // Cambiar al estado de Muerte 
            {
                Health = 0f;
                NotifyHealthChanged();
                OnDeath?.Invoke(); // Notifica muerte
                Deleted();
            }
        }
    }

    protected void NotifyHealthChanged()
    {
        OnHealthChanged?.Invoke(Health, maxHealth);
    }

    protected virtual void Deleted() { }

    //protected virtual void UpdateHealthBar(float currentHealth, float maxHealth) { }



}
