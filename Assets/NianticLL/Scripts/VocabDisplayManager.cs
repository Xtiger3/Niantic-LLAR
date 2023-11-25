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

    public VocabularySet vocabularySet;

    // UI references
    // public GameObject scrollView;
    // public GameObject vocabButton;
    // public GameObject MakiCat;
    // public GameObject OBCat;
    public GameObject CloseCat;
    public GameObject BackToCat;

    private Dictionary<string, List<string>> NPCs =  
              new Dictionary<string, List<string>>(){
                                {"Maki", new List<string> {"NUMBERS 1-10", "DAYS", "TIME"}},
                                {"Zero", new List<string> {"GREETINGS", "RELATIONSHIPS", "TRANSPORTATIONS"}},
                                {"OB", new List<string> {"ANIMALS", "???", "???"}}};

    public void DisplayCategoriesForNPC(string NPCName)
    {
        // Clear existing content
        foreach (Transform child in scrollViewContent.transform)
        {
            Destroy(child.gameObject);
        }
        Camera.main.backgroundColor = GameData.Inst.NPCColor[NPCName][2];

        foreach (string category in NPCs[NPCName]) {
            GameObject newCategoryCard = Instantiate(categoryCardPrefab);
            Transform header = newCategoryCard.transform.Find("Header");

            header.Find("Background").GetComponent<Image>().color = GameData.Inst.NPCColor[NPCName][0];
            header.Find("Foreground").GetComponent<Image>().color = GameData.Inst.NPCColor[NPCName][1];
            header.Find("Star").GetComponent<Image>().color = GameData.Inst.NPCColor[NPCName][2];
            header.Find("Title").GetComponent<TextMeshProUGUI>().text = category;

            if (vocabularySet.GetCategoryByName(category).Completed) {
                header.Find("Star").gameObject.SetActive(true);
                header.Find("Title").gameObject.SetActive(true);
            }
            
            if (vocabularySet.GetCategoryByName(category).Locked || vocabularySet.GetCategoryByName(category).Words.Count == 0) {
                header.Find("Lock").gameObject.SetActive(true);
            } else {
                newCategoryCard.transform.Find("Vocabs").GetComponent<Image>().color = GameData.Inst.NPCColor[NPCName][0];
                DisplayWordsForCategory(NPCName, category, newCategoryCard.transform.Find("Vocabs"));
            }
            
            newCategoryCard.transform.SetParent(scrollViewContent.transform, false);

        }
    }

    public void DisplayWordsForCategory(string NPCName, string categoryName, Transform container)
    {
        VocabularySet.Category selectedCategory = vocabularySet.GetCategoryByName(categoryName);

        if (selectedCategory == null) return;
        
        Transform button = container.GetChild(0);
        foreach (VocabularySet.Word word in selectedCategory.Words)
        {
            // Instantiate the wordcard prefab
            GameObject newWordCard = Instantiate(wordCardPrefab);
            
            // Find and set the word and translation texts
            newWordCard.transform.Find("LeftSub").GetComponent<Image>().color = GameData.Inst.NPCColor[NPCName][1];
            newWordCard.transform.Find("RightSub").GetComponent<Image>().color = GameData.Inst.NPCColor[NPCName][1];
            newWordCard.transform.Find("Bar").GetComponent<Image>().color = GameData.Inst.NPCColor[NPCName][1];
            newWordCard.transform.Find("English").GetComponent<TextMeshProUGUI>().text = word.Original;
            newWordCard.transform.Find("Japanese").GetComponent<TextMeshProUGUI>().text = word.Translation;

            newWordCard.transform.SetParent(container, false);
        }

        button.Find("Image").GetComponent<Image>().color = GameData.Inst.NPCColor[NPCName][3];
        button.SetAsLastSibling();
    }

    public void ShowCategories()
    {

        // vocabButton.SetActive(false);
        // MakiCat.SetActive(true);
        // OBCat.SetActive(true);
        setScrollView.SetActive(true);
        CloseCat.SetActive(true);
        Camera.main.backgroundColor = new Color(255, 240, 230, 100);
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
        Camera.main.backgroundColor = new Color(255, 240, 230, 100);
    }
}
