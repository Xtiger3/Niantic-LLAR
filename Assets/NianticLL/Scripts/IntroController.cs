using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IntroController : MonoBehaviour
{
    public GameObject nameInputPanel;
    public TMP_InputField nameInput;
    public DialogueManager dm;

    public void SetName()
    {
        nameInputPanel.SetActive(true);
        PlayerPrefs.SetString("name", nameInput.text);
        dm.DialogueNext();
    }
    public void ClosePanel(GameObject panel)
    {
        panel.SetActive(false);
    }
}
