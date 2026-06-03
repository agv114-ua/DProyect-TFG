using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Factory ")]
    [SerializeField] private MonoBehaviour enemyFactory;

    [Header("Puntos de spawn")]
    [SerializeField] private Transform[] spawnPoints;

    [Header("Config")]
    [SerializeField] private Transform playerTransform;

    private IEnemyFactory factory;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Verificamos que el MonoBehaviour implementa IEnemyFactory
        factory = enemyFactory as IEnemyFactory;
        if ( factory == null)
        {
            Debug.LogError("EnemySpawner: el objeto asignado no implementa IEnemyFactory.");
        }

        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        foreach ( Transform point in spawnPoints )
        {
            // La factoría decide 
            GameObject enemy = factory.CreateEnemy( point.position );

            // Asignamos la referencia al player
            EnemyController controller = enemy.GetComponent<EnemyController>();
            if ( controller != null )
            {
                controller.player = playerTransform;
            }
        }
    }

    
    // Update is called once per frame
    void Update()
    {
        
    }
}
