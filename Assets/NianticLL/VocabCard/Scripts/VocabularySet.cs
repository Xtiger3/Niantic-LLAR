using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VocabularySet : MonoBehaviour
{
    private List<Category> categories = new List<Category>();

    void Start()
    {
        // Create categories
        Category MakiCategory = new Category("Maki");
        Category OBCategory = new Category("OB");

        // Add words to 'Boy' category
        MakiCategory.AddWord("Ȯ (Inu)", "Dog");
        MakiCategory.AddWord("è (Neko)", "Cat");

        // Add words to 'Girl' category
        OBCategory.AddWord("܇ (Kuruma)", "Car");
        OBCategory.AddWord("�Х� (Basu)", "Bus");

        // Add categories to the list
        categories.Add(MakiCategory);
        categories.Add(OBCategory);


    }


    public Category GetCategoryByName(string categoryName)
    {
        return categories.Find(cat => cat.Name == categoryName);
    }

    public class Word
    {
        public string Original; // The original word
        public string Translation; // Translation of the word

        public Word(string original, string translation)
        {
            Original = original;
            Translation = translation;
        }
    }

   
    public class Category
    {
        public string Name; // Name of the category
        public List<Word> Words; // List of words in this category

        public Category(string name)
        {
            Name = name;
            Words = new List<Word>();
        }

        public void AddWord(string original, string translation)
        {
            Words.Add(new Word(original, translation));
        }
    }

}
