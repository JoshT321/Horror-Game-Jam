using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueHandler : MonoBehaviour
{
    private GameObject dialogueBox;
    private TMP_Text dialogueText;
    public bool isWritingDialogue;
    private IEnumerator DisplayDialogueText_Holder;

    private void Awake()
    {
        dialogueBox = transform.Find("Dialogue").gameObject;
        dialogueText = dialogueBox.transform.Find("Text").GetComponent<TMP_Text>(); 
    }

    public void ShowDialogueBox()
    {
        dialogueBox.SetActive(true);
        dialogueText.text = ""; 
    }

    public void SkipDialogueWrite()
    {
        isWritingDialogue = false;
    }

    public void HideDialogueBox()
    {
        dialogueBox.SetActive(false);
    }

    public void DisplayDialogueText(string textToDisplay)
    {
        if (DisplayDialogueText_Holder == null)
        {
            DisplayDialogueText_Holder = DisplayDialogueText_Co(textToDisplay);
            StartCoroutine(DisplayDialogueText_Holder);
        }
        else
        {
            StopCoroutine(DisplayDialogueText_Holder);
            DisplayDialogueText_Holder = DisplayDialogueText_Co(textToDisplay);
            StartCoroutine(DisplayDialogueText_Holder);
        }
        
    }

    public IEnumerator DisplayDialogueText_Co(string textToDisplay)
    {
        dialogueText.text = "";
        isWritingDialogue = true;
        int stringIndex = 0;
        while (isWritingDialogue)
        {
            foreach (char character in textToDisplay)
            {
                if (!isWritingDialogue)
                    break;
                yield return new WaitForSeconds(0.1f);
                stringIndex++;
                dialogueText.text += character;
                if (stringIndex >= textToDisplay.Length) isWritingDialogue = false;

            }
        }

        dialogueText.text = textToDisplay;

        yield return null;
    }
}
