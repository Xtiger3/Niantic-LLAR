using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Newtonsoft.Json;
using UnityEngine.UI;

[System.Serializable]
public class Dialogue
{
    public string speaker;
    public string dialogueText;
}

public class DialogueManager : MonoBehaviour
{
    //public string dialogueFileName = "quacky";

    public float textSpeed;
    public float pauseLength;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Image dialogueImage;

    public GameObject continueIndicator;
    private float startYPos;
    public float bounceSpeed = 5f;
    public float bounceHeight = 0.01f;

    private Dialogue[] dialogues;

    int index = 0;

    bool completeText = false;

    private void Start()
    {
        //TextInit();
        startYPos = continueIndicator.transform.position.y;
    }

    private void Update()
    {
        if (completeText)
        {
            continueIndicator.SetActive(true);
            continueIndicator.transform.position = new Vector3(
                continueIndicator.transform.position.x, 
                startYPos + (Mathf.Sin(Time.time * bounceSpeed) * bounceHeight),
                continueIndicator.transform.position.z);
        }
        else
        {
            continueIndicator.SetActive(false);
        }
    }

    public void DialogueNext()
    {
        if (!completeText)
        {
            StopAllCoroutines();
            dialogueText.text = dialogues[index].dialogueText;
            completeText = true;
        }
        else
        {
            NextPage();
        }
    }

    public Dialogue[] ParseDialogueFile(string dialogueFileName)
    {
        TextAsset dialogueFile = Resources.Load<TextAsset>("Dialogue/" + dialogueFileName);
        if (dialogueFile == null)
        {
            Debug.Log("json file not found");
            return null;
        }

        string jsonText = dialogueFile.text;
        Dialogue[] dialogues = JsonConvert.DeserializeObject<Dialogue[]>(jsonText);

        return dialogues;
    }

    public void TextInit(string dialogueFileName)
    {
        StopAllCoroutines();
        gameObject.SetActive(true);

        dialogues = ParseDialogueFile(dialogueFileName);
        index = 0;

        Dialogue firstDialogue = dialogues[0];
        nameText.text = firstDialogue.speaker;
        dialogueText.text = "";

        StartCoroutine(TextFlow(firstDialogue.dialogueText));
    }

    public void ReplaceUI(Sprite dialogueSprite)
    {
        dialogueImage.sprite = dialogueSprite;

    }

    IEnumerator TextFlow(string dialogue)
    {
        string str = "" + dialogue[0];

        for (int i = 1; i < dialogue.Length; ++i)
        {
            str += dialogue[i];
            if (dialogue[i - 1] == '.' || dialogue[i - 1] == '?' || dialogue[i - 1] == '!')
            {
                yield return new WaitForSeconds(textSpeed + pauseLength);
            }
            else
            {
                yield return new WaitForSeconds(textSpeed);
            }
            dialogueText.text = str;
        }
        completeText = true;
    }

    private void NextPage()
    {
        if (dialogues.Length <= 1)
        {
            return;
        }
        index++;
        StopAllCoroutines();

        completeText = false;

        if (index >= dialogues.Length)
        {
            gameObject.SetActive(false);
            return;
        }

        nameText.text = dialogues[index].speaker;

        StartCoroutine(TextFlow(dialogues[index].dialogueText));
    }

    public void ActivateDialogue()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }
}