    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine.UI;
    using UnityEngine;

    public class Inventory : Singleton<Inventory>
    {

        [SerializeField] private PlayerController player;
        [SerializeField] private int numSlots;

        [SerializeField] private InventoryItem[] inventoryItems;

        public int SlotCount { get { return numSlots; } }
        public InventoryItem [] InventoryItems => inventoryItems;
        public PlayerController Player => player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventoryItems = new InventoryItem[numSlots];
    }

    public void AddItem(InventoryItem item, int amount)
    {
        if(item)
        {
            List<int> indexes = CheckStock(item.ID);

            if(item.isStackable )
            {
                // CASO verificar que ya exista un ítem

                if (indexes.Count > 0)
                {
                    for(int i = 0;  i < indexes.Count; i++)
                    {
                        if (inventoryItems[indexes[i]].amount < item.maxStack) // Si no supera la cantidad máxima 
                        {
                            // Rellenamos el slot hasta el máximo que puede contener
                            inventoryItems[indexes[i]].amount += amount;

                            if( inventoryItems[indexes[i]].amount > item.maxStack )
                            {   
                                int difference = inventoryItems[indexes[i]].amount - item.maxStack;
                                // Si sobra cantidad se guarda lo que sobra en el siguiente slot donde se encuentre ese ítem
                                inventoryItems[indexes[i]].amount = item.maxStack;
                                AddItem(item, difference);
                            }

                            InventoryUI.Instance.DrawItemInInventory(item, inventoryItems[indexes[i]].amount, indexes[i]);
                            return; 
                        }
                    }
                }
            }

            if (amount <= 0) { return; }

            if (amount > item.maxStack)
            {
                AddItemToAvailableSlot(item, item.maxStack);
                amount -= item.maxStack;
                AddItem(item, amount);
            }
            else
            {
                AddItemToAvailableSlot(item, amount);
            }
        }

        
    }

    private void AddItemToAvailableSlot(InventoryItem item, int amount)
    {
        for(int i = 0; i < inventoryItems.Length; i++)
        {
            if(inventoryItems[i] == null)
            {
                    Debug.Log($"AddItemToAvailableSlot → slot disponible encontrado en index: {i}"); 
                    inventoryItems[i] = item.CopyItem();
                inventoryItems[i].amount = amount;  
                InventoryUI.Instance.DrawItemInInventory(item, amount, i);
                break;
            }
        }
    }

    private List<int> CheckStock(string itemID)
    {
        List<int> indexItem = new List<int>();
        for(int i = 0; i < inventoryItems.Length; i++)
        {
            if (inventoryItems[i] != null)
            {
                if (inventoryItems[i].ID == itemID)
                {
                    indexItem.Add(i);
                }
            }
            
        }

        return indexItem;
    }

    private void DeleteItem(int index)
    {
        inventoryItems[index].amount--;
        if (inventoryItems[index].amount <= 0 )
        {
            inventoryItems[index].amount = 0;
            inventoryItems[index] = null;
            InventoryUI.Instance.DrawItemInInventory(null, 0, index);

        }
        else
        {
            InventoryUI.Instance.DrawItemInInventory(inventoryItems[index], inventoryItems[index].amount, index);
        }
    }

    public void MoveItem(int initialIndex, int finalIndex)
    {
        if(inventoryItems[initialIndex] == null || inventoryItems[finalIndex] != null)
        {
            return;
        }

        // Copiamos el item para guardarlo en el slot final -> finalIndex
        InventoryItem itemForMoving = inventoryItems[initialIndex].CopyItem();
        inventoryItems[finalIndex] = itemForMoving;
        InventoryUI.Instance.DrawItemInInventory(itemForMoving, itemForMoving.amount, finalIndex);

        // Eliminamos del slot en el que se encontraba -> initialIndex
        inventoryItems[initialIndex] = null;
        InventoryUI.Instance.DrawItemInInventory(null, 0, initialIndex);

    }

    private void UseItem(int index)
    {
        if( inventoryItems[index] != null ) 
        {
            if (inventoryItems[index].UseItem() )
            {
                DeleteItem(index);
            }
        }
    }
    private void SlotInteractionRespond(InteractionType type, int index)
    {
        if (type == InteractionType.Use)
        {
            UseItem(index);
        }
        else if (type == InteractionType.Equip)
        {

        }
        else if (type == InteractionType.Drop)
        {

        }
    }

    private void OnEnable()
    {
        InventorySlot.interactionSlotEvent += SlotInteractionRespond;
    }

    private void OnDisable()
    {
        InventorySlot.interactionSlotEvent -= SlotInteractionRespond;

    }

}
