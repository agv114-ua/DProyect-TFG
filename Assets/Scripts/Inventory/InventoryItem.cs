using UnityEngine;

public enum ItemTypes
{
    Weapons, 
    Potions,
    Ingredients,
    QuestItems
}
public class InventoryItem : ScriptableObject
{
    [Header("Params")]
    public string ID;
    public string name;
    public Sprite icon;
    [TextArea] public string description;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    [Header("Infor")] 
    public ItemTypes type;
    public bool isConsumable;
    public bool isStackable;
    public int maxStack;

    [HideInInspector] public int amount;

    public InventoryItem CopyItem()
    {
        InventoryItem nueva = Instantiate(this);
        return nueva;
    }

    public virtual bool UseItem()
    {
        return true;
    }

    public virtual bool EquipItem()
    {
        return true;
    }

    public virtual bool DropItem()
    {
        return true;
      
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
