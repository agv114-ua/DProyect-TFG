using UnityEngine;

/**
 * Sección GDD 1.12.4: "cada zona tendrá una Factory propia"
 */
public class CoastalEnemyFactory : MonoBehaviour, IEnemyFactory
{
    [Header("Prefab del enemigo costero")]
    [SerializeField] private GameObject crabPrefab;

    [Header("Conf de la zona")]
    [SerializeField] private float enemySpeed = 2f;
    [SerializeField] private float detectionRadius = 4f;
    [SerializeField] private float initialHealth = 8f;

    public GameObject CreateEnemy(Vector3 position)
    {
        // 
        GameObject enemy = Instantiate(crabPrefab, position, Quaternion.identity);

        // Configuramos según la zona
        EnemyController controller = enemy.GetComponent<EnemyController>();
        if ( controller != null )
        {
            controller.speed = enemySpeed;
            controller.detectionRadius = detectionRadius;
        }

        Debug.Log($"CoastalFactory: Cangrejo creado en {position}");
        return enemy;
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
