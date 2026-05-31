using UnityEngine;

/*
 * Cuando el enemigo muere, instancia un item recogible en su posición
 * */
public class LootDropper : MonoBehaviour
{

    [Header("Conf")]
    [SerializeField] private GameObject[] lootPrefabs;
    [SerializeField] [Range(0f, 1f)] public float dropChance = 1f;

    private HealthBase healthBase;

    private void OnEnable()
    {
        healthBase = GetComponent<HealthBase>();
        if (healthBase != null )
        {
            healthBase.OnDeath += SpawnLoot;
        }

    }

    private void OnDisable()
    {
        if (healthBase != null)
        {
            healthBase.OnDeath -= SpawnLoot;
        }

    }

    private void SpawnLoot()
    {
        if (Random.value > dropChance) return;

        if (lootPrefabs.Length == 0) return;

        // Elige un drop aleatorio del array
        GameObject lootPrefab = lootPrefabs[Random.Range(0, lootPrefabs.Length)];
        Instantiate(lootPrefab, transform.position, Quaternion.identity);

        Debug.Log($"LOOT DROPEADO EN {transform.position}");
    }
   
}
