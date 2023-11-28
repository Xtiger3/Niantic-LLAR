using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessagingController : MonoBehaviour
{
    public GameObject fromMessagePrefab;
    public GameObject toMessagePrefab;
    public GameObject scrollViewContent;

    public GameObject notifPrefab;

    public float duration = 0.5f;

    private void Start()
    {
        SendFromMessage("DR. ZERO", "i love u son");
    }
    public void SendFromMessage(string sender, string messageText)
    {
        GameObject message = Instantiate(fromMessagePrefab, Vector3.zero, Quaternion.identity, scrollViewContent.transform);
        message.GetComponentsInChildren<TextMeshProUGUI>()[0].text = sender;
        message.GetComponentsInChildren<TextMeshProUGUI>()[1].text = messageText;

        StartCoroutine(ScaleObject(message));
    }

    public void SendToMessage(string messageText)
    {
        GameObject message = Instantiate(fromMessagePrefab, Vector3.zero, Quaternion.identity, scrollViewContent.transform);
        message.GetComponentsInChildren<TextMeshProUGUI>()[0].text = messageText;
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
}
