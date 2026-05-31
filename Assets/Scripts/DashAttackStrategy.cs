using UnityEngine;

public class DashAttackStrategy : IAttackStrategy
{

    private int damage;
    private float range;
    private float cooldown;

    public DashAttackStrategy(int damage, float range, float cooldown)
    {
        this.damage = damage;
        this.range = range;
        this.cooldown = cooldown;
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
        return cooldown;
    }
    
    public void Execute(PlayerController controller)
    {
        Debug.Log($"DashAttackStrategy: Embestida ejecutada. Daño={damage}, Cooldown={cooldown}s");
    }
}
