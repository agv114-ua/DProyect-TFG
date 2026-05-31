using UnityEngine;

public class ItemToAdd : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private InventoryItem reference;
    [SerializeField] private int amountToAdd;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            Inventory.Instance.AddItem(reference, amountToAdd);
            Destroy(gameObject);
        }
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
