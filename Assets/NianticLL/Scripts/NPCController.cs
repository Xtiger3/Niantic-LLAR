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
    private bool selected = false;

    //public int progression = -1;

    private void Start()
    {
        dialogueUI = GameObject.Find("DialogueCanvas").transform.GetChild(0).gameObject;
        dialogueFileName = VocabularySet.Instance.dialogueFile;
    }

    private void Update()
    {

        if (this.gameObject.transform.position.y < -50f)
        {
            this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x,
                                                                -50f,
                                                                this.gameObject.transform.position.z);

        }

        if (dialogueUI.GetComponent<DialogueManager>().cooldown)
        {
            VocabularySet.Instance.AddToOngoingCategory(npcName);
            //if(progression != -1)
            //{
            //    MessageData.Inst.progression = progression;
            //    MessageData.Inst.displayed = false;
            //    MessageData.Inst.notif = true;
            //}

            if (VocabularySet.Instance.intro)
            {
                VocabularySet.Instance.intro = false;
                VocabularySet.Instance.LoadScene("CustomMap");
            }
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if(!selected && !EventSystem.current.IsPointerOverGameObject() && hit.transform == transform)
                {
                    selected = true;
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
