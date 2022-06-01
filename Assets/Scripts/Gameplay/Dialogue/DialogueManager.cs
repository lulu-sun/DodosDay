using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] GameObject dialogueBox;
    [SerializeField] Text dialogueText;
    [SerializeField] Text dialogueName;
    [SerializeField] int lettersPerSecond;

    public event Action OnShowDialogue;
    public event Action OnCloseDialogue;

    public static DialogueManager Instance { get; private set; }
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        dialogueBox.gameObject.SetActive(false);
    }

    Dialogue dialogue;
    Action onDialogueFinished;
    int currentLine = 0;
    bool isTyping;

    public bool IsShowing { get; private set; }

    public IEnumerator ShowDialogue(Dialogue dialogue, Action onFinished=null)
    {
        yield return new WaitForEndOfFrame();

        OnShowDialogue?.Invoke();

        IsShowing = true;
        this.dialogue = dialogue;
        onDialogueFinished = onFinished;

        dialogueBox.SetActive(true);
        StartCoroutine(TypeDialogue(dialogue.Dialogues[0]));
    }

    public void HandleUpdate()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)) && !isTyping)
        {
            ++currentLine;
            if (currentLine < dialogue.Dialogues.Count)
            {
                StartCoroutine(TypeDialogue(dialogue.Dialogues[currentLine]));
            }
            else
            {
                currentLine = 0;
                IsShowing = false;
                dialogueBox.SetActive(false);
                OnCloseDialogue?.Invoke();
                onDialogueFinished?.Invoke();
            }
        }
    }

    public IEnumerator TypeDialogue(SingleDialogue singleDialogue)
    {
        isTyping = true;
        dialogueName.text = singleDialogue.Name;
        AudioManager.Instance.PlayTypingSfx();

        dialogueText.text = "";
        foreach (var letter in singleDialogue.Line.ToCharArray())
        {
            dialogueText.text += letter;
            var waitTime = 1.0f / lettersPerSecond;
            if (Controls.GetSelectKey())
            {
                waitTime /= 3.0f;
            }
            yield return new WaitForSeconds(waitTime);

        }
        AudioManager.Instance.StopTypingSfx();
        isTyping = false;
    }
}
