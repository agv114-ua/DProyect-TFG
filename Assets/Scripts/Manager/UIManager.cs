using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class UIManager : Singleton<UIManager>
{
    //public static UIManager Instance;

    [SerializeField] private List<GameObject> hearts;
    [SerializeField] private Sprite disabledHeart;

    // VERSIÓN 2 SISTEMA DE VIDA CON BARRA DE VIDA
    [SerializeField] private Image playerHealth;
    [SerializeField] private TextMeshProUGUI healthTMP;

    [SerializeField] private GameObject inventoryPanel;

    [Header("Ref al jugador")]
    [SerializeField] private PlayerHealth playerHealthComponent;

    private float currentHealth;
    private float maxHealth;
       
    

    /*private void Awake()
    {
        Instance = this;
    }*/
    
   

    // Update is called once per frame
    void Update()
    {
       
        UpdatePlayerUI();

    }

    public void RemoveHeart (int index)
    {
        Image heart = hearts[index].GetComponent<Image>();
        heart.sprite = disabledHeart;
    }

    // VERSIÓN 2

    private void UpdatePlayerUI()
    {
        if (maxHealth <= 0f) return; // No se puede dividir por 0

        playerHealth.fillAmount = Mathf.Lerp(playerHealth.fillAmount, currentHealth / maxHealth, 10f * Time.deltaTime);

        healthTMP.text = $"{currentHealth}/{maxHealth}";
    }

    // ================================ INVENTORY ================================ //

    public void OpenCloseInventoryPanel()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
    }

    // ================================ OBSERVER: SUSCRIPCIÓN ================================ //
    private void OnEnable()
    {
        if( playerHealthComponent != null)
        {
            playerHealthComponent.OnHealthChanged += HandleHealthChanged;
            playerHealthComponent.OnDeath += HandlePlayerDeath;
        }
    }

    private void OnDisable()
    {
        if (playerHealthComponent != null)
        {
            playerHealthComponent.OnHealthChanged -= HandleHealthChanged;
            playerHealthComponent.OnDeath -= HandlePlayerDeath;
        }
    }

    // ================================ OBSERVER: CALLBACKS ================================ //

    private void HandleHealthChanged(float current, float max)
    {
        currentHealth = current;
        maxHealth = max;
    }

    private void HandlePlayerDeath()
    {
        Debug.Log("UIManager: El jugador ha muerto - gameOver");
    }
}
