using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NPCController : MonoBehaviour
{
    public GameObject dialogueUI;
    public Sprite dialogueSprite;
    public string dialogueFileName;
    public string npcName;

    private void Start()
    {
        GetComponent<Animator>().SetInteger("anim_id", 0);
    }

    private void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if(!EventSystem.current.IsPointerOverGameObject() && hit.transform == transform)
                {
                    if (dialogueUI.GetComponent<DialogueManager>().cooldown)
                    {
                        dialogueUI.SetActive(true);
                        string dialogue = npcName + " is busy right now...";
                        StartCoroutine(dialogueUI.GetComponent<DialogueManager>().TextFlow(dialogue, 0));
                        return;
                    }
                    dialogueUI.GetComponent<DialogueManager>().TextInit(dialogueFileName);
                    dialogueUI.GetComponent<DialogueManager>().ReplaceUI(dialogueSprite);
                    dialogueUI.GetComponent<DialogueManager>().anim = GetComponent<Animator>();
                }
            }
        }
    }
}
