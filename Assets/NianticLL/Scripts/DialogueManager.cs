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
    public int trigger;
    public string[] choices;
    public int anim;
    public string[] dialoguePaths;
}

public class DialogueManager : MonoBehaviour
{
    public string dialogueFileName;
    public Animator anim;

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

    public int index = 0;

    bool completeText = false;

    public GameObject choice1;
    public GameObject choice2;

    public GameObject nameInputPanel;
    public GameObject pickLanguagePanel;
    public GameObject newContactAddedPanel;

    public GameObject tc;

    public int ready = 0;

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
        anim.SetInteger("anim_id", dialogues[index].anim);
        //anim.SetInteger("anim_id", 0);
        if (!completeText)
        {
            StopAllCoroutines();
            string dialogueText_ = dialogues[index].dialogueText.Replace("_", PlayerPrefs.GetString("name"));
            dialogueText.text = dialogueText_;
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

    public void TextInit(string dialogueFileName_)
    {
        choice1.SetActive(false);
        choice2.SetActive(false);
        GetComponent<Button>().enabled = true;

        dialogueFileName = dialogueFileName_;
        StopAllCoroutines();
        gameObject.SetActive(true);

        dialogues = ParseDialogueFile(dialogueFileName);
        index = 0;

        Dialogue firstDialogue = dialogues[0];
        nameText.text = firstDialogue.speaker;
        dialogueText.text = "";

        ToggleChoices(index);

        string dialogueText_ = firstDialogue.dialogueText.Replace("_", PlayerPrefs.GetString("name"));
        StartCoroutine(TextFlow(dialogueText_));
    }

    public void ReplaceUI(Sprite dialogueSprite)
    {
        dialogueImage.sprite = dialogueSprite;
    }

    IEnumerator TextFlow(string dialogue)
    {
        if (dialogues[index].trigger == 1 || dialogues[index].trigger == 2)
        {
            GetComponent<Button>().enabled = false;
        }

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

        if (dialogues[index].trigger == 1)
        {
            nameInputPanel.SetActive(true);
            GetComponent<Button>().enabled = false;
        }
        if (dialogues[index].trigger == 2)
        {
            pickLanguagePanel.SetActive(true);
            GetComponent<Button>().enabled = false;
        }
        if (dialogues[index].trigger == 3)
        {
            StartCoroutine(Flash());
        }
        if (dialogues[index].trigger == 4)
        {
            StartCoroutine(SpinTC());
        }
    }

    IEnumerator SpinTC()
    {
        tc.SetActive(true);
        float startRotation = tc.transform.eulerAngles.y;
        float endRotation = startRotation + (360.0f);
        float t = 0.0f;
        while (t < 1.5f)
        {
            t += Time.deltaTime;
            float yRotation = Mathf.Lerp(startRotation, endRotation, t / 1.5f) % 360.0f;
            tc.transform.eulerAngles = new Vector3(tc.transform.eulerAngles.x, yRotation, tc.transform.eulerAngles.z);
            yield return null;
        }
        yield return new WaitForSeconds(1.5f);
        tc.SetActive(false);
    }

    IEnumerator Flash()
    {
        newContactAddedPanel.SetActive(true);
        yield return Scale(newContactAddedPanel, 0f, 6f);
        yield return new WaitForSeconds(.5f);
        yield return Scale(newContactAddedPanel, 6f, 0f);
        newContactAddedPanel.SetActive(false);
    }
    IEnumerator Scale(GameObject obj, float scale_begin, float scale_end)
    {
        float duration = .5f;
        Vector3 originalScale = Vector3.one * scale_begin;
        Vector3 targetScale = Vector3.one * scale_end;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            obj.transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        obj.transform.localScale = targetScale;
    }

    private void NextPage()
    {
        choice1.SetActive(false);
        choice2.SetActive(false);
        GetComponent<Button>().enabled = true;

        if (dialogues.Length <= 1)
        {
            return;
        }
        index++;
        StopAllCoroutines();

        completeText = false;

        if (index >= dialogues.Length)
        {
            if (dialogues[index-1].dialoguePaths.Length != 0)
            {
                //if(dialogues[index].dialoguePaths[0])
                string dialoguePathText_ = dialogues[index].dialoguePaths[ready].Replace("_", PlayerPrefs.GetString("name"));
                StartCoroutine(TextFlow(dialoguePathText_));
            }
            else
            {
                gameObject.SetActive(false);
            }
            return;
        }

        nameText.text = dialogues[index].speaker;

        //choices
        ToggleChoices(index);

        string dialogueText_ = dialogues[index].dialogueText.Replace("_", PlayerPrefs.GetString("name"));
        StartCoroutine(TextFlow(dialogueText_));
    }

    private void ToggleChoices(int index)
    {
        //choices
        if (dialogues[index].dialoguePaths.Length != 0)
        {
            choice2.GetComponent<Button>().onClick.AddListener(() => SelectReady()); 
        }
        if (dialogues[index].choices.Length != 0)
        {
            choice1.SetActive(true);
            choice1.GetComponentInChildren<TextMeshProUGUI>().text = dialogues[index].choices[0];
            if (dialogues[index].choices.Length == 2)
            {
                choice2.SetActive(true);
                choice1.GetComponentInChildren<TextMeshProUGUI>().text = dialogues[index].choices[1];
                Debug.Log("true");
            }
            GetComponent<Button>().enabled = false;
        }
    }

    public void ActivateDialogue()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void SelectReady()
    {
        ready = 1;
        choice2.GetComponent<Button>().onClick.RemoveListener(() => SelectReady());
    }
}