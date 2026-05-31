using TMPro;

using UnityEngine;
using UnityEngine.UI;
using System;


public enum InteractionType
{
    Click,
    Use,
    Equip,
    Drop

}
public class InventorySlot : MonoBehaviour
{
    public static Action<InteractionType, int> interactionSlotEvent;
    public int index { get; set; }

    private Button _button;

    [SerializeField] private Image icon;
    [SerializeField] private GameObject amountBack;
    [SerializeField] private TextMeshProUGUI amountTMP;
    
    private void Awake()
    {
        _button = GetComponent<Button>();
    }
    
    public void UpdateSlot(InventoryItem item, int amount)
    {
        icon.sprite = item.icon;
        amountTMP.text = amount.ToString();

    }
    public void SelectSlot()
    {
        _button.Select();
    }

    public void ActivateSlotUI(bool state)
    {
        icon.gameObject.SetActive(state);
        amountBack.SetActive(state);
    }

    public void ClickSlot()
    {
        interactionSlotEvent?.Invoke(InteractionType.Click, index);

        if( InventoryUI.Instance.InitialIndexForMoving != -1 )
        {
            if ( InventoryUI.Instance.InitialIndexForMoving != index )
            {
                Inventory.Instance.MoveItem(InventoryUI.Instance.InitialIndexForMoving, index);
            }
        }
    }

    public void UseSlotItem()
    {
        // Comprobamos si hay un item en ese slot para usar
        if (Inventory.Instance.InventoryItems[index] != null )
        {
            interactionSlotEvent?.Invoke(InteractionType.Use, index);
        }
    }
}
