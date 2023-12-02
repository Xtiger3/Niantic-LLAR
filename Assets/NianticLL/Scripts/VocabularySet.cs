using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VocabularySet : MonoBehaviour
{
    public List<Category> categories = new List<Category>();
    public static VocabularySet Instance { get; private set; }

    private List<Color> MakiPalette = new List<Color> { new Color(0.953f, 0.918f, 1f, 1),
                                                        new Color(0.882f, 0.784f, 1f, 1),
                                                        new Color(0.729f, 0.631f, 0.933f, 1),
                                                        new Color(0.686f, 0.537f, 1f, 1) };

    private List<Color> OBPalette = new List<Color> { new Color(0.875f, 0.933f, 1f, 1),
                                                        new Color(0.725f, 0.82f, 1f, 1),
                                                        new Color(0.584f, 0.8f, 1f, 1),
                                                        new Color(0.271f, 0.588f, 0.965f, 1) };

    private List<Color> ZeroPalette = new List<Color> { new Color(0.82f, 0.82f, 0.82f, 1),
                                                        new Color(0.569f, 0.569f, 0.569f, 1),
                                                        new Color(0.329f, 0.329f, 0.329f, 1),
                                                        new Color(0.227f, 0.227f, 0.227f, 1) };

    public Dictionary<string, List<Color>> NPCColor = new Dictionary<string, List<Color>>();

    public List<GameObject> NPCToLoadPrefab; // Should be in the order of

    public Dictionary<string, Category> ongoingCategory = new Dictionary<string, Category>();

    private void Awake()
    {
        NPCColor.Add("Maki", MakiPalette);
        NPCColor.Add("Zero", ZeroPalette);
        NPCColor.Add("OB", OBPalette);

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    void Start()
    {
        Category reviewCategory = new Category("Review");
        Category numsCategory = new Category("NUMBERS 1-10");
        numsCategory.Words = new List<Word> {new Word("1", "一 (Ichi)"),
                                            new Word("2", "二 (Ni)"),
                                            new Word("3", "三 (San)"),
                                            new Word("4", "四 (Shi/ Yon)"),
                                            new Word("5", "五 (Go)"),
                                            new Word("6", "六 (Roku)"),
                                            new Word("7", "七 (Shichi/ Nana)"),
                                            new Word("8", "八 (Hachi)"),
                                            new Word("9", "九 (Kyuu/ Ku)"),
                                            new Word("10", "十 (Juu)"),
                                            new Word("20", "二十 (Nijuu)"),
                                            new Word("30", "三十 (Sanjuu)"),
                                            new Word("40", "四十 (Yonjuu)"),
                                            new Word("50", "五十 (Gojuu)")};

        Category daysCategory = new Category("DAYS");
        daysCategory.Words = new List<Word> {new Word("Monday", "月曜日 (Getsuyoubi)"),
                                                new Word("Tuesday", "火曜日 (Kayoubi)"),
                                                new Word("Wednesday", "水曜日 (Suiyoubi)"),
                                                new Word("Thursday", "木曜日 (Mokuyoubi)"),
                                                new Word("Friday", "金曜日 (Kinyoubi)"),
                                                new Word("Saturday", "土曜日 (Doyoubi)"),
                                                new Word("Sunday", "日曜日 (Nichiyoubi)")};

        Category timeCategory = new Category("TIME");
        timeCategory.Words = new List<Word> {new Word("Evening", "夕方 (Yuugata)"),
                                                new Word("Morning", "朝 (Asa)"),
                                                new Word("Lunch", "昼食 (Chuushoku)"),
                                                new Word("Tomorrow", "明日 (Ashita)"),
                                                new Word("Today", "今日 (Kyou)"),
                                                new Word("Yesterday", "昨日 (Kinou)"),
                                                new Word("Tonight", "今夜 (Konya)")};

        Category greetingsCategory = new Category("GREETINGS");
        greetingsCategory.Words = new List<Word> {new Word("Greetings", "挨拶 (Aisatsu)"),
                                                    new Word("Hello", "こんにちは (Konnichiwa)"),
                                                    new Word("Thank you", "ありがとう (Arigatou)"),
                                                    new Word("Goodbye", "さようなら (Sayonara)"),
                                                    new Word("Please", "お願いします (Onegaishimasu)"),
                                                    new Word("Yes", "はい (Hai)"),
                                                    new Word("No", "いいえ (Iie)"),
                                                    new Word("Maybe", "たぶん (Tabun)")};

        Category relationsCategory = new Category("RELATIONSHIPS");
        relationsCategory.Words = new List<Word> {};

        Category transportsCategory = new Category("TRANSPORTATIONS");
        transportsCategory.Words = new List<Word> {};

        Category animalsCategory = new Category("ANIMALS");
        animalsCategory.Words = new List<Word> {};

        Category idkkk = new Category("???");
        idkkk.Words = new List<Word> {};
        
        // Add categories to the List
        categories.Add(greetingsCategory);
        categories.Add(daysCategory);
        categories.Add(timeCategory);
        categories.Add(numsCategory);
        categories.Add(relationsCategory);
        categories.Add(transportsCategory);
        categories.Add(animalsCategory);
        categories.Add(idkkk);
        categories.Add(reviewCategory);
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
        public bool Locked;
        public bool Completed;

        public Category(string name)
        {
            Name = name;
            Locked = false;
            Completed = false;
            Words = new List<Word>();
        }

        public void AddWord(string original, string translation)
        {
            Words.Add(new Word(original, translation));
        }

        public void Unlock() {
            Locked = false;
        }

        public void MarkAsComplete() {
            Completed = true;
        }
    }
}
