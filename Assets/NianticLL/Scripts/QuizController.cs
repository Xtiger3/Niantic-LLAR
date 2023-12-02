using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuizController : MonoBehaviour
{
    public DialogueManager dm;

    public List<VocabularySet.Word> quizWords = new List<VocabularySet.Word>();

    public TextMeshProUGUI currWordText;
    private VocabularySet.Word currWord;

    public Transform choiceButtons;

    private List<int> randomJapaneseIndices = new List<int>();
    private int index = 0;

    private int countCorrect = 0;

    public Color correctColor;
    public Color wrongColor;

    private void Start()
    {
        quizWords = VocabularySet.Instance.GetCategoryByName("DAYS").Words;
        randomJapaneseIndices = ShuffleIndices();
        PopulateQuestion();
    }

    public void ChooseAnswer(GameObject button)
    {
        if (button.GetComponentInChildren<TextMeshProUGUI>().text == currWord.Original)
        {
            //correct word, turn green
            countCorrect++;
            button.GetComponent<Image>().color = correctColor;
        }
        else
        {
            //incorrect word, turn red
            button.GetComponent<Image>().color = wrongColor;
        }

        index++;

        StartCoroutine(AnswerFeedback(button));
    }

    private IEnumerator AnswerFeedback(GameObject button)
    {

        yield return new WaitForSeconds(1f);
        button.GetComponent<Image>().color = Color.white;

        if (index < quizWords.Count)
        {
            PopulateQuestion();
        }
        else
        {
            dm.gameObject.SetActive(true);
            //end quiz, start dialogue 
            if(countCorrect == quizWords.Count)
            {
                string dialogue = "Splendid! You got all of them correct! Come back for more vocab sets.";
                yield return StartCoroutine(dm.TextFlow(dialogue, 0));
            }
            else
            {
                dm.cooldown = true;
                string dialogue = "All you need is practice! Try again next time...";
                yield return StartCoroutine(dm.TextFlow(dialogue, 0));
            }
            gameObject.SetActive(false);
        }
    }

    private void PopulateQuestion()
    {
        List<int> randomEnglishIndices = ShuffleIndices();

        currWord = quizWords[randomJapaneseIndices[index]];
        currWordText.text = currWord.Translation;

        bool b = false;
        for (int i = 0; i < 4; ++i)
        { 
            choiceButtons.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = quizWords[randomEnglishIndices[i]].Original;
            if (quizWords[randomEnglishIndices[i]].Original == currWord.Original) b = true;
        }
        if (!b) choiceButtons.GetChild(Random.Range(0,4)).GetComponentInChildren<TextMeshProUGUI>().text = currWord.Original;
    }

    private List<int> ShuffleIndices()
    {
        List<int> allIndices = new List<int>();
        int totalIndices = quizWords.Count;

        for (int i = 0; i < totalIndices; i++)
        {
            allIndices.Add(i);
        }
        for (int i = allIndices.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            int temp = allIndices[i];
            allIndices[i] = allIndices[randomIndex];
            allIndices[randomIndex] = temp;
        }

        return allIndices;
    }

}
