using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class MessagingController : MonoBehaviour
{

    // To use this, increment MessageData.Inst.progression and set MessageData.Inst.displyed to false
    // To add a message, follow the else if statement format

    public GameObject fromMessagePrefab;
    public GameObject toMessagePrefab;
    public GameObject scrollViewContent;

    public GameObject notifPrefab;
    public GameObject playerButton;

    public float duration = 0.5f;

    private List<string> playerTexts = new List<string> { "Thank you!", "Yes boss.", "Nice to meet you too!", "Thanks!" };

    private bool disableTouch = false;

    private void Start()
    {
        if (MessageData.Inst != null)
        {
            foreach (MessageData.Message msg in MessageData.Inst.textToDisplayOnStart)
            {
                if (msg.isPlayer)
                {
                    DisplayToMessage(msg.textMsg);
                }
                else
                {
                    DisplayFromMessage(msg.userName, msg.textMsg);
                }
            }
        }
        
        //SendFromMessage("DR. ZERO", "i love u son");
    }

    private void Update()
    {
        //Debug.Log(MessageData.Inst.replied);
        if (MessageData.Inst.progression == 0 && !MessageData.Inst.displayed)
        {
            MessageData.Inst.replied = false;
            disableTouch = true;
            StartCoroutine(WelcomeMessages());
            MessageData.Inst.displayed = true;
        }
        else if (MessageData.Inst.progression == 0 && MessageData.Inst.replied)
        {
            MessageData.Inst.displayed = false;
            Debug.Log(MessageData.Inst.displayed);
            MessageData.Inst.progression++;
            //MessageData.Inst.replied = true;
        }
        else if (MessageData.Inst.progression == 1 && !MessageData.Inst.displayed)
        {
            disableTouch = true;
            StartCoroutine(WelcomeMessagesPart2());
            MessageData.Inst.displayed = true;
        }
        else if (MessageData.Inst.progression == 1 && !MessageData.Inst.replied)
        {
            updatePlayerOption();
        }
        else if (MessageData.Inst.progression == 2 && !MessageData.Inst.displayed)
        {
            disableTouch = true;
            StartCoroutine(OBEncounter());
            MessageData.Inst.displayed = true;
        }
        else if (MessageData.Inst.progression == 2 && !MessageData.Inst.replied)
        {
            updatePlayerOption();
        }
        else if (MessageData.Inst.progression == 3 && !MessageData.Inst.displayed)
        {
            disableTouch = true;
            StartCoroutine(FirstCompletion());
            MessageData.Inst.displayed = true;
        }
        else if (MessageData.Inst.progression == 3 && !MessageData.Inst.replied)
        {
            updatePlayerOption();
        }

        // Disable the player reply option
        if (MessageData.Inst.replied)
        {
            //playerButtonText.text = "...";
            playerButton.SetActive(false);
        }
    }

    public IEnumerator SendFromMessage(string sender, string messageText)
    {
        GameObject message = Instantiate(fromMessagePrefab, Vector3.zero, Quaternion.identity, scrollViewContent.transform);
        message.GetComponentsInChildren<TextMeshProUGUI>()[0].text = sender;
        message.GetComponentsInChildren<TextMeshProUGUI>()[1].text = messageText;

        yield return ScaleObject(message);
        MessageData.Inst.textToDisplayOnStart.Add(new MessageData.Message(false, sender, messageText));
    }

    public IEnumerator SendToMessage(string messageText)
    {
        GameObject message = Instantiate(toMessagePrefab, Vector3.zero, Quaternion.identity, scrollViewContent.transform);
        //message.GetComponentsInChildren<TextMeshProUGUI>()[0].text = messageText;
        message.GetComponentsInChildren<TextMeshProUGUI>()[1].text = messageText;

        yield return ScaleObject(message);
        MessageData.Inst.textToDisplayOnStart.Add(new MessageData.Message(true, "", messageText));
        MessageData.Inst.replied = true;
    }

    public void DisplayFromMessage(string sender, string messageText)
    {
        GameObject message = Instantiate(fromMessagePrefab, Vector3.zero, Quaternion.identity, scrollViewContent.transform);
        message.GetComponentsInChildren<TextMeshProUGUI>()[0].text = sender;
        message.GetComponentsInChildren<TextMeshProUGUI>()[1].text = messageText;
    }

    public void DisplayToMessage(string messageText)
    {
        GameObject message = Instantiate(toMessagePrefab, Vector3.zero, Quaternion.identity, scrollViewContent.transform);
        message.GetComponentsInChildren<TextMeshProUGUI>()[1].text = messageText;
    }

    public void GenerateToMessages()
    {

    }

    public void SendNotification()
    {
        GameObject notif = Instantiate(notifPrefab, Vector3.zero, Quaternion.identity, scrollViewContent.transform);
    }

    public IEnumerator ScaleObject(GameObject obj)
    {
        Vector3 originalScale = Vector3.zero;
        Vector3 targetScale = Vector3.one;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            obj.transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        obj.transform.localScale = targetScale;
    }

    IEnumerator WelcomeMessages()
    {
        yield return SendFromMessage("Boss", "Let’s welcome our new employee #115!");
        yield return SendFromMessage("#114", "Welcome >D<");
        yield return SendFromMessage("#043", "Welcome to ICRC.");
        yield return SendFromMessage("#006", "Welcome to the team.");
        yield return SendFromMessage("#072", "Weclome, #115.");
        yield return SendFromMessage("#106", "Welcome to ICRC!");
        yield return SendFromMessage("#012", "Welcome to ICRC!");
        yield return SendFromMessage("#027", "Welcome!");
        yield return SendFromMessage("#094", "Welcome to ICRC!");
        playerButton.SetActive(true);
        playerButton.transform.Find("UserText").transform.GetComponent<TextMeshProUGUI>().text = playerTexts[0];
        MessageData.Inst.replied = false;
        disableTouch = false;
    }

    IEnumerator WelcomeMessagesPart2()
    {
        yield return SendFromMessage("Boss", "Go walk around to meet #114 and #008! They have vocabulary sets ready for you.");
        yield return SendFromMessage("Boss", "You might also find some Bins (my assistants) scattered around the map. They are hungry for knowledge so make sure to feed them if you bump into them!");
        MessageData.Inst.replied = false;
        disableTouch = false;
    }

    IEnumerator OBEncounter()
    {
        yield return SendFromMessage("#114", "It was nice to meet you in person >D<");
        yield return SendFromMessage("#114", "Come find me whenever you are ready to be quizzed ^ - ^");
        MessageData.Inst.replied = false;
        disableTouch = false;
    }

    IEnumerator FirstCompletion()
    {
        yield return SendFromMessage("Boss", "Congratulations on completing your first vocab set!");
        yield return SendFromMessage("#114", "Wow you are fast ºOº");
        MessageData.Inst.replied = false;
        disableTouch = false;
    }


    private void updatePlayerOption()
    {
        playerButton.SetActive(true);
        playerButton.transform.Find("UserText").transform.GetComponent<TextMeshProUGUI>().text = playerTexts[MessageData.Inst.progression];
    }

    public void reply()
    {
        disableTouch = true;
        if (!MessageData.Inst.replied)
        {
            StartCoroutine(SendToMessage(playerTexts[MessageData.Inst.progression]));
            //if (MessageData.Inst.progression == 0) MessageData.Inst.progression++;
        }
        disableTouch = false;
    }

    public void prevScene(string sceneName)
    {
        if (!disableTouch)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
