using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : Singleton<InventoryUI>
{
    [Header("Description Panel")]
    [SerializeField] private GameObject inventoryPanelDescription;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI name;
    [SerializeField] private TextMeshProUGUI description;


    [SerializeField] private InventorySlot slotPrefab;
    [SerializeField] private Transform container;

    public InventorySlot selectedSlot { get; private set; }
    public int InitialIndexForMoving { get; private set; }
    private List<InventorySlot> availableSlots = new List<InventorySlot>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitializeInventory();
        InitialIndexForMoving = -1;
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateSelectedSlot();
        if ( Input.GetKeyDown(KeyCode.M) )
        {
            if (selectedSlot != null)
            {
                InitialIndexForMoving = selectedSlot.index;
            }
        }
        
    }

    private void InitializeInventory()
    {
        // Eliminar hijos preexistentes del container para evitar offset
        for (int i = container.childCount - 1; i >= 0; i--)
        {
            Destroy(container.GetChild(i).gameObject);
        }

        for(int i = 0; i < Inventory.Instance.SlotCount; i++)
        {
            InventorySlot nuevo = Instantiate(slotPrefab, container);
            nuevo.index = i;
            availableSlots.Add(nuevo);
            Debug.Log($"Slot creado: {i}");
        }    
    }

    private void UpdateSelectedSlot()
    {
        GameObject gOSelected = EventSystem.current.currentSelectedGameObject;
        if (gOSelected != null)  
        {
            // Comprobamos que el objecto seleccionado es un InventorySlot
            InventorySlot slot = gOSelected.GetComponent<InventorySlot>();
            if ( slot != null )
            {
                selectedSlot = slot;
            }
        }
    }

    public void DrawItemInInventory(InventoryItem itemToAdd, int amount, int index)
    {
        InventorySlot slot = availableSlots[index];
        Debug.Log($"DrawItemInInventory → item: {(itemToAdd != null ? itemToAdd.name : "null")} | amount: {amount} | index: {index}");

        if ( itemToAdd != null)
        {
            slot.ActivateSlotUI(true);
            slot.UpdateSlot(itemToAdd, amount);

        }
        else
        {
            slot.ActivateSlotUI(false);
        }
    }

    private void UpdateInventoryDescription(int index)
    {
        if (Inventory.Instance.InventoryItems[index] != null)
        {
            icon.sprite = Inventory.Instance.InventoryItems[index].icon;
            name.text = Inventory.Instance.InventoryItems[index].name;
            description.text = Inventory.Instance.InventoryItems[index].description;
            inventoryPanelDescription.SetActive(true);
        }
        else
        {
            inventoryPanelDescription.SetActive(false);
        }
    }

    private void SlotInteractionRespond(InteractionType type, int index)
    {
        if( type == InteractionType.Click)
        {
            UpdateInventoryDescription(index);
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

    public void UseItem()
    {
        if(selectedSlot != null)
        {
            selectedSlot.UseSlotItem();
            selectedSlot.SelectSlot();

        }
    }
}
