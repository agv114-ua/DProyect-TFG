using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform player;
    public float reboteForce = 6f;
    public float speed = 2.0f;
    public float detectionRadius = 5.0f;
    //public int vida = 10;
    private bool gettingDamage = false;
    
    private Rigidbody2D rb;
    private EnemyHealth enemyHealth;
    private Vector2 movement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyHealth = GetComponent<EnemyHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        
        if ( distanceToPlayer < detectionRadius)
        {
            Vector2 direction = (player.position - transform.position).normalized;

            movement = new Vector2(direction.x, 0);
            Debug.Log("yendo a por ti ....");
        }
        else
        {
            movement = Vector2.zero;
        }

        if ( !gettingDamage )
        {
            rb.MovePosition(rb.position + movement * speed * Time.deltaTime);

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ( collision.gameObject.CompareTag("Player") )
        {
            Vector2 directionDamage = new Vector2(transform.position.x, 0);

            collision.gameObject.GetComponent<PlayerController>().GettingDamage(directionDamage, 1);
        }
    }

    // cuando colisiona la espada con el enemigo ( esta instancia de enemigo )
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Espada"))
        {
            Debug.Log("RECIBIENDO DAÑO DE LA ESPADA DEL PLAYER");
            Vector2 directionDamage = new Vector2(collision.gameObject.transform.position.x, 0);

            PlayerController player = collision.GetComponentInParent<PlayerController>();
            int damage = 1;

            if ( player != null && player.CurrentAttackStrategy != null )
            {
                damage = player.CurrentAttackStrategy.GetDamage();
            }

            GetDamage(directionDamage, damage);
        }
    }

    public void GetDamage(Vector2 direction, int amountDamage)
    {
        if (!gettingDamage)
        {
            gettingDamage = true;       // calculamos el sentido de la fuerza de rebote
                                        // para que vaya el sentido del movimiento del player
            Vector2 rebote = new Vector2(transform.position.x - direction.x, 0.2f).normalized;
            rb.AddForce(rebote * reboteForce, ForceMode2D.Impulse);
            // vida = vida - amountDamage;
            enemyHealth.TakeDamage(amountDamage); // dispara el evento OnHealthChanged
            StartCoroutine(DesactivateDamage());
        }
    }

    IEnumerator DesactivateDamage()
    {
        yield return new WaitForSeconds(0.4f);
        gettingDamage = false;
        rb.linearVelocity = Vector2.zero;
        Debug.Log("DESACTIVANDO DAÑO...");

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
