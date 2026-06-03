using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : Singleton<DialogueManager>
{
    [Header("UI ref")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private Image npcIcon;
    [SerializeField] private TextMeshProUGUI NPCName;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Conf")]
    [SerializeField] private float textSpeed = 0.03f;

    private Queue<string> dialogueQueue;
    private bool textAnimating = false;
    private bool farewellShown = false;
    private NPCDialogue currentDialogue;

    private void Start()
    {
        dialogueQueue = new Queue<string>();
        dialoguePanel.SetActive(false);
    }

    public void StartDialogue(NPCDialogue dialogue)
    {
        if (dialoguePanel.activeSelf) return;

        currentDialogue = dialogue;
        farewellShown = false;
        dialogueQueue.Clear();

        if ( dialogue.conversation != null)
        {
            for( int i = 0; i < dialogue.conversation.Length; i++)
            {
                dialogueQueue.Enqueue(dialogue.conversation[i].text);
            }
        }

        NPCName.text = dialogue.NPCName;
        if ( dialogue.icon != null )
        {
            npcIcon.sprite = dialogue.icon;
        }
        dialoguePanel.SetActive(true);
        ShowAnimatedText(dialogue.greeting);
    }

    private void Update()
    {
        if (!dialoguePanel.activeSelf) return;

        if ( Input.GetKeyDown(KeyCode.Space))
        {
            if ( farewellShown )
            {
                CloseDialogue();
                return;
            }

            if (textAnimating) return;

            ContinueDialogue();
        }
    }

    private void ContinueDialogue()
    {
        if ( dialogueQueue.Count == 0)
        {
            ShowAnimatedText(currentDialogue.farewell);
            farewellShown = true;
            return;
        }

        string nextLine = dialogueQueue.Dequeue();
        ShowAnimatedText(nextLine);
    }

    private void ShowAnimatedText(string text)
    {
        StopAllCoroutines();
        StartCoroutine(AnimateText(text));
    }

    private IEnumerator AnimateText(string text)
    {
        textAnimating = true;
        dialogueText.text = "";

        for (int i = 0; i < text.Length; i++)
        {
            dialogueText.text += text[i];
            yield return new WaitForSeconds(textSpeed);
        }

        textAnimating = false;
    }

    public void CloseDialogue()
    {
        StopAllCoroutines();
        dialoguePanel.SetActive(false);
        farewellShown = false;
        dialogueQueue.Clear();
    }
}
