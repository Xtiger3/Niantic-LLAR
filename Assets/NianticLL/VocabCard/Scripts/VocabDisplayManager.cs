using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VocabDisplayManager : MonoBehaviour
{
    public GameObject wordCardPrefab; // Reference to the wordcard prefab
    public GameObject scrollViewContent;

    public VocabularySet vocabularySet;

    // UI references
    public GameObject scrollView;
    public GameObject vocabButton;
    public GameObject MakiCat;
    public GameObject OBCat;
    public GameObject CloseCat;
    public GameObject BackToCat;


    public void DisplayWordsForCategory(string categoryName)
    {
        // Clear existing content
        foreach (Transform child in scrollViewContent.transform)
        {
            Destroy(child.gameObject);
        }

        VocabularySet.Category selectedCategory = vocabularySet.GetCategoryByName(categoryName);


        foreach (VocabularySet.Word word in selectedCategory.Words)
        {
            // Instantiate the wordcard prefab
            GameObject newWordCard = Instantiate(wordCardPrefab);

            // Find and set the word and translation texts
            TextMeshProUGUI wordText = newWordCard.transform.Find("word").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI translationText = newWordCard.transform.Find("translation").GetComponent<TextMeshProUGUI>();

            wordText.text = word.Original;
            translationText.text = word.Translation;

            newWordCard.transform.SetParent(scrollViewContent.transform, false);

        }
    }

    

    public void ShowCategories()
    {

        vocabButton.SetActive(false);
        MakiCat.SetActive(true);
        OBCat.SetActive(true);
        CloseCat.SetActive(true);
    }

    public void CloseCategories()
    {

        vocabButton.SetActive(true);
        MakiCat.SetActive(false);
        OBCat.SetActive(false);
        CloseCat.SetActive(false);
    }

    public void ShowScrollView()
    {
        BackToCat.SetActive(true);
        scrollView.SetActive(true);
        MakiCat.SetActive(false);
        OBCat.SetActive(false);
        CloseCat.SetActive(false);
    }

    public void BackToCategories()
    {
        BackToCat.SetActive(false);
        scrollView.SetActive(false);
        MakiCat.SetActive(true);
        OBCat.SetActive(true);
        CloseCat.SetActive(true);
    }
}
