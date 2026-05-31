using UnityEngine;

/**
 * Sección GDD 1.12.4: "cada zona tendrá una Factory propia".
 * */
public class ForestEnemyFactory : MonoBehaviour, IEnemyFactory
{

    [Header("Prefab del enemigo de bosque")]
    [SerializeField] private GameObject forestCreaturePrefab;

    [Header("Config de zona")]
    [SerializeField] private float enemySpeed = 3f;
    [SerializeField] private float detectionRadius = 6f;
    [SerializeField] private float initialHealth = 15f;

    public GameObject CreateEnemy(Vector3 position)
    {
        GameObject enemy = Instantiate(forestCreaturePrefab, position, Quaternion.identity);
        EnemyController controller = enemy.GetComponent<EnemyController>();
        if (controller != null )
        {
            controller.speed = enemySpeed;
            controller.detectionRadius = detectionRadius;
        }

        Debug.Log($"ForestFactory: Criatura creada en {position}");
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
