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

    private void Update()
    {
        if (dm.cooldown)
        {
            SceneManager.LoadScene("CustomMap");
        }
    }

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
       
        yield return new WaitForSeconds(5f);
        scanningUI.SetActive(false);
        Instantiate(prefab);
    }
}
