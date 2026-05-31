using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    public PlayerHealth PlayerHealth { get; private set; }

    // Estrategia activa
    private IAttackStrategy currentAttackStrategy;

    //
    public IAttackStrategy CurrentAttackStrategy => currentAttackStrategy;
    [Header("Configuración General")]
    public float moveSpeed = 5f;          // Velocidad del jugador
    public float runSpeed = 8f;           // Velocidad al correr

    [Header("Configuraci�n de Salto (Alundra)")]
    public float jumpForce = 8.5f;        // Fuerza del impulso hacia arriba
    public float gravity = 25f;           // Gravedad falsa (cuanto mayor, máss rápido cae)
    public int vida = 10;
    [Header("Referencias (Arrastrar desde la Jerarquía)")]
    public Transform bodyTransform;       // Arrastra el hijo 'Body' aquí
    public Transform shadowTransform;     // Arrastra el hijo 'Shadow' aquí

    [SerializeField] private UIManager uIManager;
    private int health = 3;
    
    // Referencias internas
    public Rigidbody2D rb;
    public Animator animator;

    // Referencia a la m�quina de estados de gestiona cada uno
    public StateMachine stateMachine { get; private set; }

    private Vector2 movementInput;
    public Vector2 MovementInput => movementInput;

    private bool gettingDamage = false;
    public float reboteForce = 6f;
 
    // Variables para la simulaci�n de altura (Eje Z)
    private float currentHeight = 0f;    // Altura actual del suelo
    public float CurrentHeight => currentHeight; // Propiedad de solo lectura

    private float verticalVelocity = 0f; // Velocidad de subida/bajada
    private bool isGrounded = true;

    public float timeToNextAttack = 0f;
    public float TimeToNextAttack => timeToNextAttack;

    public float timeBetweenAttacks = 0f;
    public float TimeBetweenAttacks => timeBetweenAttacks;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        stateMachine = new StateMachine(this);
        rb = GetComponent<Rigidbody2D>();

        // CAMBIO IMPORTANTE: Buscamos el animator en los hijos (Body), 
        // porque el padre ya no tiene el sprite.
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        // Seguridad para Unity 6
        rb.gravityScale = 0;

        //--------------------------
        // --- C�DIGO MEJORADO EN CONDICIONES
        // Empezamos en el estado de parado
        stateMachine.Initialize(stateMachine.idleState);

        currentAttackStrategy = new MeleeAttackStrategy(3, 1.5f);
        timeBetweenAttacks = currentAttackStrategy.GetCooldown();
    }

    public void SetAttackStrategy(IAttackStrategy newStrategy)
    {
        currentAttackStrategy = newStrategy;
        timeBetweenAttacks = newStrategy.GetCooldown();
        Debug.Log($"Estrategia cambiada. Nuevo daño: {newStrategy.GetDamage()}");
    }
    //
    private void Awake()
    {
        PlayerHealth = GetComponent<PlayerHealth>();
    }
    public void Move()
    {
        // 1. Capturamos el input de movimiento
        movementInput.x = Input.GetAxisRaw("Horizontal");
        movementInput.y = Input.GetAxisRaw("Vertical");

    }

    // ---------------------- L�gica de f�sicas de walk ----------------------- //
    public void ApplyMovement(float speed)
    {
        Vector2 movement = movementInput.normalized * speed;
        rb.linearVelocity = movement;
    }

    
    public bool IsWalking()
    {
        return movementInput.magnitude > 0.1f;
        
    }

    // ---------------------- L�gica de f�sicas de jump ----------------------- //
    public bool TryStartJumping()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            verticalVelocity = jumpForce;
            isGrounded = false;
            return true;
        }

        return false;

    }

    private void ApplyFakeGravity()
    {
        if (!isGrounded)
        {
            verticalVelocity -= gravity * Time.deltaTime;
            currentHeight += verticalVelocity * Time.deltaTime;
            if (currentHeight <= 0)
            {
                currentHeight = 0;
                verticalVelocity = 0;
                isGrounded = true;
            }
        }
    }

    public bool HasLanded()
    {
        return isGrounded && currentHeight == 0;
    }

    // ---------------------- Lógica de físicas de attack ----------------------- //
    public bool IsAttacking()
    {
        bool atacando = Input.GetMouseButtonDown(0) && timeToNextAttack <= 0;
        if (atacando)
        {
            timeToNextAttack = timeBetweenAttacks;
        }
        return atacando;
    }

    // ---------------------- L�gica de f�sicas de run ----------------------- //
    public bool IsRunning()
    {
        return Input.GetKey(KeyCode.LeftShift) && IsWalking();
    }

    // ---------------------- L�gica de actualizaci�n ----------------------- //

    // Update is called once per frame
    void Update()
    {   
        // Probando sistema de corazones de vida
        if (Input.GetKeyDown(KeyCode.K)) {
            TakeDamage();
        }
        //
        if (timeToNextAttack > 0)
        {
            timeToNextAttack -= Time.deltaTime;
            Debug.Log("reduciendo tiempo en atacar ...");
        }

        if (!gettingDamage)
        {
            Move();
        }
        
        ApplyFakeGravity();          // Aplica la gravedad cada frame, si est� en el aire
        stateMachine.Execute();      // Ejecuta el estado actual

        // --- TEST: Cambiar estrategia con teclas ---
        // Pulsa 1 → Melee (espada normal, 3 de daño)
        // Pulsa 2 → Dash (embestida, 8 de daño)
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetAttackStrategy(new MeleeAttackStrategy(3, 1.5f));
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetAttackStrategy(new DashAttackStrategy(8, 3.5f, 4f));
        }
    }

    // ====================00 VERSIÓN CON CORAZONES = ===
    public void TakeDamage()
    {
        if (health > 0)
        {
            health--;
            uIManager.RemoveHeart(health);

            if (health == 0) {
                Debug.Log("Hemos muerto");
                // Invoke(nameof(Morir), 1f) ; // llama a la función morir pasado 1 segundo // se puede llamar desde cualquier estado
                // en la que destruimos al player Destroy(this.gameObject);
            }
        }
    }

    // VERSIÓN DE PRUEBA INDEPENDIENTE PARA PROBAR QUE FUNCIONABA EL ATACKE Y EL CAMPO DE ATACKE
    // ======================================================00
    public void GettingDamage(Vector2 direction, int amountDamage)
    {
        if (!gettingDamage)
        {
            gettingDamage = true;
            //vida -= amountDamage;// bug resuelto. no restaba -1 sino que reasignaba el valor por -amountDamage
            PlayerHealth.TakeDamage(amountDamage);
            Debug.Log("Dañanando ....");
            Vector2 rebote = new Vector2(bodyTransform.position.x - direction.x, 0.2f).normalized;
            rb.AddForce(rebote * reboteForce, ForceMode2D.Impulse);
            StartCoroutine(DesactivateDamage());

        }
    }

    IEnumerator DesactivateDamage()
    {
        yield return new WaitForSeconds(0.4f);
        gettingDamage = false;
        rb.linearVelocity = Vector2.zero;
        Debug.Log("DESACTIVANDO DAÑO EN PLAYER...");

    }
}
