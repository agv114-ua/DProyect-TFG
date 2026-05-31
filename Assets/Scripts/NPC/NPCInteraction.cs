using UnityEngine;

/**
 * Detecta al jugador en el rango del NPC triggerZone mostrando un indicador para entrar en conversación
 * */
public class NPCInteraction : MonoBehaviour
{
    [SerializeField] private NPCDialogue dialogue;
    [SerializeField] private GameObject interactionIndicator;

    private bool playerInRange = false; // para detectar si el jugador se encuentra en el rango del NPC

    public NPCDialogue Dialogue => dialogue;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if( other.CompareTag("Player"))
        {
            playerInRange = true;
            if ( interactionIndicator != null )
            {
                interactionIndicator.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if ( other.CompareTag("Player") )
        {
            playerInRange = false;
            if ( interactionIndicator != null )
            {
                interactionIndicator.SetActive(false);
            }
            DialogueManager.Instance.CloseDialogue();
           
        }
    }

    private void Update()
    {
        if ( playerInRange && Input.GetKeyDown(KeyCode.E) )
        {
            DialogueManager.Instance.StartDialogue(dialogue );
        }
    }

    
}
