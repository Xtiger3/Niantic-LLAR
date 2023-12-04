using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProgressBar : MonoBehaviour
{
    public TextMeshProUGUI progressText;
    public Slider progressBar;
    public string npcName;

    private void Start()
    {
        progressBar.maxValue = VocabularySet.Instance.NPCs[npcName].Count;

        int countUnlocked = 0;
        foreach(string catName in VocabularySet.Instance.NPCs[npcName])
        {
            VocabularySet.Category cat = VocabularySet.Instance.GetCategoryByName(catName);
            if (!cat.Locked) { countUnlocked++; }
        }

        progressBar.value = countUnlocked;
        progressText.text = progressBar.value + "/" + progressBar.maxValue;

        Debug.Log(progressText.text);
    }
}
