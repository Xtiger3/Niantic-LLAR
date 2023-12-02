using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class VocabDisplayManager : MonoBehaviour
{
    public GameObject wordCardPrefab; // Reference to the wordcard prefab
    public GameObject categoryCardPrefab;
    public GameObject scrollViewContent;

    public GameObject setScrollView;
    public GameObject vocabScrollView;

    //public VocabularySet vocabularySet;

    // UI references
    // public GameObject scrollView;
    // public GameObject vocabButton;
    // public GameObject MakiCat;
    // public GameObject OBCat;
    public GameObject CloseCat;
    public GameObject BackToCat;

    private Color mainBGColor;

    private Dictionary<string, List<string>> NPCs;
    private void Start()
    {
        mainBGColor = Camera.main.backgroundColor;
        NPCs = VocabularySet.Instance.NPCs;
    }

    public void DisplayCategoriesForNPC(string NPCName)
    {
        // Clear existing content
        foreach (Transform child in scrollViewContent.transform)
        {
            Destroy(child.gameObject);
        }
        Camera.main.backgroundColor = VocabularySet.Instance.NPCColor[NPCName][2];

        // Create NPC specific category cards
        foreach (string category in NPCs[NPCName]) {
            GameObject newCategoryCard = Instantiate(categoryCardPrefab);
            newCategoryCard.name = NPCName + "," + category;
            Transform header = newCategoryCard.transform.Find("Header");

            header.Find("Background").GetComponent<Image>().color = VocabularySet.Instance.NPCColor[NPCName][0];
            header.Find("Foreground").GetComponent<Image>().color = VocabularySet.Instance.NPCColor[NPCName][1];
            header.Find("Star").GetComponent<Image>().color = VocabularySet.Instance.NPCColor[NPCName][2];
            header.Find("Title").GetComponent<TextMeshProUGUI>().text = category;

            if (VocabularySet.Instance.GetCategoryByName(category).Completed) {
                header.Find("Star").gameObject.SetActive(true);
                header.Find("Title").gameObject.SetActive(true);
            }
            
            if (VocabularySet.Instance.GetCategoryByName(category).Locked || VocabularySet.Instance.GetCategoryByName(category).Words.Count == 0) {
                header.Find("Lock").gameObject.SetActive(true);
            } else {
                newCategoryCard.transform.Find("Vocabs").GetComponent<Image>().color = VocabularySet.Instance.NPCColor[NPCName][0];

                // Add expandable word cards
                DisplayWordsForCategory(NPCName, category, newCategoryCard.transform.Find("Vocabs"));
            }
            
            newCategoryCard.transform.SetParent(scrollViewContent.transform, false);

        }
    }

    public void DisplayWordsForCategory(string NPCName, string categoryName, Transform container)
    {
        bool dontneedButton = false;
        Transform button = container;
        // Debug.Log(container.name);
        if (container.name == "Container")
        {
            dontneedButton = true;
        }
        VocabularySet.Category selectedCategory = VocabularySet.Instance.GetCategoryByName(categoryName);

        if (selectedCategory == null) return;
        
        if (!dontneedButton)
        {

            button = container.GetChild(0);

        }

        // Create category specific word cards
        foreach (VocabularySet.Word word in selectedCategory.Words)
        {
            // Instantiate the wordcard prefab
            GameObject newWordCard = Instantiate(wordCardPrefab);
            
            // Find and set the word and translation texts
            newWordCard.transform.Find("LeftSub").GetComponent<Image>().color = VocabularySet.Instance.NPCColor[NPCName][1];
            newWordCard.transform.Find("RightSub").GetComponent<Image>().color = VocabularySet.Instance.NPCColor[NPCName][1];
            newWordCard.transform.Find("Bar").GetComponent<Image>().color = VocabularySet.Instance.NPCColor[NPCName][1];
            newWordCard.transform.Find("English").GetComponent<TextMeshProUGUI>().text = word.Original;
            newWordCard.transform.Find("Japanese").GetComponent<TextMeshProUGUI>().text = word.Translation;

            newWordCard.transform.SetParent(container, false);
        }

        if (!dontneedButton)
        {
            button.Find("Image").GetComponent<Image>().color = VocabularySet.Instance.NPCColor[NPCName][3];
            button.SetAsLastSibling();
        }
    }

    public void ShowCategories()
    {

        // vocabButton.SetActive(false);
        // MakiCat.SetActive(true);
        // OBCat.SetActive(true);
        setScrollView.SetActive(true);
        CloseCat.SetActive(true);
        Camera.main.backgroundColor = mainBGColor;
    }

    public void CloseCategories()
    {

        // vocabButton.SetActive(true);
        // MakiCat.SetActive(false);
        // OBCat.SetActive(false);
        // CloseCat.SetActive(false);
        SceneManager.LoadScene("HomeScreen");
    }

    public void ShowScrollView()
    {
        BackToCat.SetActive(true);
        vocabScrollView.SetActive(true);
        // MakiCat.SetActive(false);
        // OBCat.SetActive(false);
        setScrollView.SetActive(false);
        CloseCat.SetActive(false);
    }

    public void BackToCategories()
    {
        BackToCat.SetActive(false);
        vocabScrollView.SetActive(false);
        // MakiCat.SetActive(true);
        // OBCat.SetActive(true);
        setScrollView.SetActive(true);
        CloseCat.SetActive(true);
        Camera.main.backgroundColor = mainBGColor;
    }

    public void ClearEverything()
    {
        BackToCat.SetActive(false);
        vocabScrollView.SetActive(false);
        // MakiCat.SetActive(true);
        // OBCat.SetActive(true);
        setScrollView.SetActive(false);
        CloseCat.SetActive(false);
    }

    public void ShowVocabScrollview()
    {
        BackToCat.SetActive(true);
        vocabScrollView.SetActive(true);
        // MakiCat.SetActive(true);
        // OBCat.SetActive(true);
        //setScrollView.SetActive(true);
        
    }
}
