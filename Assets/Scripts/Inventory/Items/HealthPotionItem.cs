using UnityEngine;

[CreateAssetMenu(menuName = "Items/Health Potion")]
public class HealthPotionItem : InventoryItem
{
    [Header("Potion info")]
    public float healthRestore;

    public override bool UseItem()
    {
        if (Inventory.Instance.Player.PlayerHealth.CanBeHealed)
        {
            Inventory.Instance.Player.PlayerHealth.Heal(healthRestore);
            return true;
        }
        return false;   
    }
    
}
