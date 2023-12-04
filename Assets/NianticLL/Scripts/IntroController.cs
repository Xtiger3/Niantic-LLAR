using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class IntroController : MonoBehaviour
{
    public GameObject nameInputPanel;
    public TMP_InputField nameInput;
    public DialogueManager dm;

    public GameObject prefab;
    public GameObject scanningUI;
    private void Start()
    {
        StartCoroutine(SpawnPrefab());
    }

    //private void Update()
    //{
    //    if (dm.cooldown)
    //    {
    //        VocabularySet.Instance.AddToOngoingCategory("Zero");
    //        MessageData.Inst.progression = 0;
    //        MessageData.Inst.displayed = false;
    //        MessageData.Inst.notif = true;
    //        VocabularySet.Instance.LoadScene("CustomMap");
    //    }
    //}

    public void SetName()
    {
        nameInputPanel.SetActive(false);
        PlayerPrefs.SetString("name", nameInput.text);
        dm.DialogueNext();
    }
    public void ClosePanel(GameObject panel)
    {
        panel.SetActive(false);
    }

    IEnumerator SpawnPrefab()
    {
        yield return new WaitForSeconds(10f);
        scanningUI.SetActive(false);
        Instantiate(VocabularySet.Instance.npcPrefab);
    }
}
