using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class FCManager : MonoBehaviour
{
    private Vector2 startTouchPosition, currentTouchPosition, endTouchPosition;
    private float touchStartTime;
    private bool isDragging = false;

    public GameObject frontSide;
    public GameObject backSide;
    public GameObject SignOfKnown;
    public GameObject SignOfUnknown;
    private bool isShowingBack = false;

    // Swipe Detection Parameters
    private float maxSwipeTime = 0.5f; // Maximum time for a swipe
    private float minSwipeDistance = 200f; // Minimum distance for a swipe

    // Vocab set parameters
    public VocabularySet vocabularySet;
    private VocabularySet.Category selectedCategory;
    private VocabularySet.Category ReviewCategory;
    private int WordsTotal;
    private List<int> LearningLevel;
    private int WordsIndex = 0;
    private int Round = 0;

    private void Start()
    {

        

    }

    public void InitializeFC(string categoryName)
    {
        Debug.Log(categoryName);
        selectedCategory = vocabularySet.GetCategoryByName(categoryName);
        ReviewCategory = vocabularySet.GetCategoryByName("Review");
        if (ReviewCategory != null && ReviewCategory.Words != null)
        {

            ReviewCategory.Words.Clear();
        }
        frontSide.SetActive(true);
        backSide.SetActive(true);
        
        WordsTotal = selectedCategory != null ? selectedCategory.Words.Count : 0;
        LearningLevel = new List<int>(new int[WordsTotal]);
        WordsIndex = 0;
        Round = 0;
        //Debug.Log(selectedCategory.Words[0].Original);
        //Debug.Log(selectedCategory.Words[0].Translation);
        UpdateCardUI(selectedCategory.Words[0]);


    }

    public void ResetLearningLevels()
    {
        for (int i = 0; i < LearningLevel.Count; i++)
        {
            LearningLevel[i] = 0;
        }
    }


    private void UpdateCardUI(VocabularySet.Word word)
    {
        if (word != null)
        {
            
            TextMeshProUGUI wordText = frontSide.transform.Find("word").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI translationText = backSide.transform.Find("translation").GetComponent<TextMeshProUGUI>();
            wordText.text = word.Original;
            translationText.text = word.Translation;
        }
    }

    private void UpdateReviewCategory(VocabularySet.Word word, bool Shouldadd)
    {
        if (Shouldadd)
        {

            ReviewCategory.AddWord(word.Original, word.Translation);

        }
    }

    public void UpdateLearningLevel(int wordIndex, bool known)
    {
        
            if (known)
            {
                // Increase the learning level if the word is known
                LearningLevel[wordIndex] = Mathf.Min(LearningLevel[wordIndex] + 1, 2); // Max level is 2
            }
            else
            {
                // Reset to 0 if the word is not known
                LearningLevel[wordIndex] = 0;
            }
       
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    touchStartTime = Time.time;
                    startTouchPosition = touch.position;
                    currentTouchPosition = touch.position;
                    isDragging = true;
                    break;

                case TouchPhase.Moved:
                    if (isDragging)
                    {
                        currentTouchPosition = touch.position;
                        MoveCardWithFinger();
                    }
                    break;

                case TouchPhase.Ended:
                    if (isDragging)
                    {
                        endTouchPosition = touch.position;
                        ResetFCPosition();
                        DetectTouchAction();
                        isDragging = false;
                    }
                    break;
            }
        }
    }

    private void MoveCardWithFinger()
    {
        // Assuming currentTouchPosition is in screen coordinates
        Vector3 screenPoint = new Vector3(currentTouchPosition.x, currentTouchPosition.y, 10.0f); // Adjust the z value as needed
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(screenPoint);

        // Set new positions
        Vector3 newPositionFront = new Vector3(worldPoint.x, frontSide.transform.position.y, frontSide.transform.position.z);
        Vector3 newPositionBack = new Vector3(worldPoint.x, backSide.transform.position.y, backSide.transform.position.z);

        frontSide.transform.position = newPositionFront;
        backSide.transform.position = newPositionBack;
    }

    private void ResetFCPosition()
    {
        frontSide.transform.position = new Vector3(0, 0, 0);
        backSide.transform.position = new Vector3(0, 0, 0);
    }

    private void DetectTouchAction()
    {
        float touchDuration = Time.time - touchStartTime;
        float swipeDistance = Vector2.Distance(startTouchPosition, currentTouchPosition);

        if (touchDuration < maxSwipeTime && swipeDistance > minSwipeDistance)
        {
            ProcessSwipe();
        }
        else if (swipeDistance < minSwipeDistance)
        {
            FlipCard();
        }
    }

    private void ProcessSwipe()
    {
        bool isSwipeRight = endTouchPosition.x > startTouchPosition.x;
        if (WordsIndex < WordsTotal)
        {
            if (Round == 0)
            { 

                UpdateLearningLevel(WordsIndex, !isSwipeRight);
                UpdateCardUI(selectedCategory.Words[WordsIndex]);
                WordsIndex++;

            }

            if (Round == 1)
            {
                UpdateLearningLevel(WordsIndex, !isSwipeRight);
                UpdateCardUI(selectedCategory.Words[WordsIndex]);
                UpdateReviewCategory(selectedCategory.Words[WordsIndex], LearningLevel[WordsIndex] != 2);

                WordsIndex++;
            }

        }else 
        {
            if (Round == 0)
            {

                Round++;

            }

            if (Round == 1)
            {

            }
        }


    }

    private void FlipCard()
    {
        StartCoroutine(FlipCardRoutine(0.4f)); 
    }

    private IEnumerator FlipCardRoutine(float duration)
    {
        float time = 0;
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(0, 180, 0);

        while (time < duration)
        {
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.rotation = endRotation; // Ensure it's exactly at the end rotation

        // Toggle the visibility of the front and back sides
        if (isShowingBack)
        {
            frontSide.SetActive(true);
            backSide.SetActive(false);
        }
        else
        {
            frontSide.SetActive(false);
            backSide.SetActive(true);
        }
        isShowingBack = !isShowingBack;
    }

}
