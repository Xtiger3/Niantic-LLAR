using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroController : MonoBehaviour
{
    public GameObject nameInput;
    public DialogueManager dm;

    public void ActivateNameInput()
    {
        if (dm.dialogueFileName == "zero" && dm.index == 1)
        {
            nameInput.SetActive(true);
            Debug.Log("hello");
        }
    }

    public void SetName()
    {
        nameInput.SetActive(false);
        PlayerPrefs.SetString("name", "");
        dm.DialogueNext();
    }
}
