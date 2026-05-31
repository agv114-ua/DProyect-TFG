using UnityEngine;

public class MeleeAttackStrategy : IAttackStrategy
{
    private int damage;
    private float range;    

    public MeleeAttackStrategy(int damage, float range)
    {
        this.damage = damage;
        this.range = range;
    }

    public int GetDamage()
    {
        return damage;
    }

    public float GetRange()
    {
        return range;
    }

    public float GetCooldown()
    {
        return 0f;
    }

    public void Execute(PlayerController player)
    {
        Debug.Log($"MeleeAttackStrategy: Ataque ejecutado. Daño={damage} range={range}");
    }
   
}
