using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Configuración General")]
    public float moveSpeed = 5f;          // Velocidad del jugador
    public float runSpeed = 8f;           // Velocidad al correr

    [Header("Configuración de Salto (Alundra)")]
    public float jumpForce = 8.5f;        // Fuerza del impulso hacia arriba
    public float gravity = 25f;           // Gravedad falsa (cuanto mayor, más rápido cae)

    [Header("Referencias (Arrastrar desde la Jerarquía)")]
    public Transform bodyTransform;       // Arrastra el hijo 'Body' aquí
    public Transform shadowTransform;     // Arrastra el hijo 'Shadow' aquí

    // Referencias internas
    public Rigidbody2D rb;
    public Animator animator;

    private Vector2 movementInput;

    // Variables para la simulación de altura (Eje Z)
    private float _currentHeight = 0f;    // Altura actual del suelo
    private float _verticalVelocity = 0f; // Velocidad de subida/bajada
    private bool _isGrounded = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // CAMBIO IMPORTANTE: Buscamos el animator en los hijos (Body), 
        // porque el padre ya no tiene el sprite.
        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        // Seguridad para Unity 6
        rb.gravityScale = 0;
    }

    void Update()
    {
        // 1. Capturamos el input de movimiento (TU CÓDIGO ORIGINAL)
        movementInput.x = Input.GetAxisRaw("Horizontal");
        movementInput.y = Input.GetAxisRaw("Vertical");

        // 2. Lógica de salto (NUEVO)
        HandleJump();

        // 3. Control de Animaciones (TU CÓDIGO ORIGINAL + ADAPTACIÓN)
        if (movementInput.magnitude > 0.1f)
        {
            animator.SetBool("isMoving", true);
            animator.SetFloat("moveX", movementInput.x);
            animator.SetFloat("moveY", movementInput.y);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }

        // 4. Actualizar la posición visual del salto (NUEVO)
        UpdateVisuals();
    }

    void FixedUpdate()
    {
        // --- Movimiento físico (TU CÓDIGO ORIGINAL) ---
        // Usamos MovePosition como tenías, es perfecto para RPGs
        Vector2 movement = movementInput.normalized;
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    // --- Lógica del Salto Estilo Alundra ---
    void HandleJump()
    {
        

        // Si estamos en el aire, aplicar gravedad simulada
        if (!_isGrounded)
        {
            _verticalVelocity -= gravity * Time.deltaTime;
            _currentHeight += _verticalVelocity * Time.deltaTime;

            // Si tocamos el suelo
            if (_currentHeight <= 0)
            {
                _currentHeight = 0;
                _verticalVelocity = 0;
                _isGrounded = true;
                // Opcional: Sonido de aterrizaje aquí
            }
        }
    }

    void UpdateVisuals()
    {
        // Movemos el SPRITE (Body) hacia arriba, pero el Rigidbody se queda en el suelo
        if (bodyTransform != null)
        {
            bodyTransform.localPosition = new Vector3(0, _currentHeight, 0);
        }

        // Efecto visual: La sombra se hace pequeña al saltar
        if (shadowTransform != null)
        {
            // Cuanto más alto, más pequeña la sombra (mínimo 50% de tamaño)
            float scale = Mathf.Clamp(1f - (_currentHeight * 0.15f), 0.5f, 1f);
            shadowTransform.localScale = new Vector3(scale, scale, 1f);
        }
    }
}