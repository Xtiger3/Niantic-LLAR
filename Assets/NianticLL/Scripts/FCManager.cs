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
    public GameObject ReviewContainer;
    public GameObject ResultsPage;
    public GameObject Total;
    public GameObject Learned;
    public GameObject BackButton;
    public GameObject Review;
    private bool isShowingBack = false;

    // FC Instructions
    public TextMeshProUGUI FCInstruction;
    public Color startColor = Color.black;
    public Color shineColor = Color.green;
    public float Instruction_duration = 2f;

    // Swipe Detection Parameters
    private float maxSwipeTime = 0.5f; // Maximum time for a swipe
    private float minSwipeDistance = 200f; // Minimum distance for a swipe

    // Vocab set parameters
    public VocabularySet vocabularySet;
    private VocabDisplayManager vacabDisplayManager;
    private VocabularySet.Category selectedCategory;
    private VocabularySet.Category ReviewCategory;
    private TextMeshProUGUI totalNum;
    private TextMeshProUGUI reviewNum;
    private TextMeshProUGUI learnedNum;
    private int WordsTotal;
    private int WordsLearned;
    private int WordsReview;
    private List<int> LearningLevel;
    private int WordsIndex = 0;
    private int Round = 0;
    private bool Enable_TouchDetection = false;

    // NPC and Category Name
    private string NPCName;
    private string categoryName;
    private string combineName;

    private void Start()
    {

        vacabDisplayManager = FindObjectOfType<VocabDisplayManager>();
        totalNum = Total.GetComponent<TextMeshProUGUI>();
        reviewNum = Review.GetComponent<TextMeshProUGUI>();
        learnedNum = Learned.GetComponent<TextMeshProUGUI>();
        


    }

    public void InitializeFC(string combinationName)
    {
        //Debug.Log(categoryName);
        ResultsPage.SetActive(false);
        combineName = combinationName;
        string[] splitStrings = combinationName.Split(',');

        NPCName = splitStrings[0];
        categoryName = splitStrings.Length > 1 ? splitStrings[1] : "";

        //Debug.Log("NPCName: " + NPCName);
        //Debug.Log("categoryName: " + categoryName);
        selectedCategory = vocabularySet.GetCategoryByName(categoryName);
        ReviewCategory = vocabularySet.GetCategoryByName("Review");
        if (ReviewCategory == null)
        {
            Debug.Log("reviewcategory not detected");
        }
        

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
        Enable_TouchDetection = true;
        BackButton.SetActive(true);
        ShowFCinstruction();


    }

    public void BackToVocab()
    {
        Enable_TouchDetection = false;
        frontSide.SetActive(false);
        backSide.SetActive(false);
        SignOfKnown.SetActive(false);
        SignOfUnknown.SetActive(false);
        ResultsPage.SetActive(false);
        vacabDisplayManager.ShowVocabScrollview();
        BackButton.SetActive(false);
    }


    public void ResetLearningLevels()
    {
        for (int i = 0; i < LearningLevel.Count; i++)
        {
            LearningLevel[i] = 0;
        }
    }

    private void ResetFCrotation()
    {
        if (isShowingBack)
        {

            frontSide.transform.localRotation = frontSide.transform.localRotation * Quaternion.Euler(0, 180, 0);
            backSide.transform.localRotation = backSide.transform.localRotation * Quaternion.Euler(0, 180, 0);
        }
    }


    private void UpdateCardUI(VocabularySet.Word word)
    {
        ResetFCrotation();
        frontSide.SetActive(true);
        backSide.SetActive(false);
        isShowingBack = false;
        if (word != null)
        {
            
            TextMeshProUGUI translationText = frontSide.transform.Find("translation").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI wordText =  backSide.transform.Find("word").GetComponent<TextMeshProUGUI>();
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
                SignOfKnown.SetActive(known);
                SignOfUnknown.SetActive(!known);
                LearningLevel[wordIndex] = Mathf.Min(LearningLevel[wordIndex] + 1, 2); // Max level is 2
            }
            else
            {
            // Reset to 0 if the word is not known
                SignOfKnown.SetActive(known);
                SignOfUnknown.SetActive(!known);
                LearningLevel[wordIndex] = 0;
            }
       
    }

    private void ShowFCinstruction()
    {
        StartCoroutine(ShineTextRoutine());
    }

    private IEnumerator ShineTextRoutine()
    {
        FCInstruction.gameObject.SetActive(true);
        float elapsed = 0;
        while (elapsed < Instruction_duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Sin(Mathf.PI * elapsed / Instruction_duration); // Use Sin to create a smooth in and out effect
            FCInstruction.color = Color.Lerp(startColor, shineColor, t);
            yield return null;
        }

        // Optionally reset to start color after shining
        FCInstruction.color = startColor;
        FCInstruction.gameObject.SetActive(false);
    }
    void Update()
    {
        if (Enable_TouchDetection)
        {

        
         #if UNITY_EDITOR
            // Simulate touch with mouse input in the Unity Editor
            if (Input.GetMouseButtonDown(0)) // Equivalent to TouchPhase.Began
            {
                touchStartTime = Time.time;
                startTouchPosition = Input.mousePosition;
                currentTouchPosition = Input.mousePosition;
                isDragging = true;
            }
            else if (Input.GetMouseButton(0)) // Equivalent to TouchPhase.Moved
            {
                if (isDragging)
                {
                    currentTouchPosition = Input.mousePosition;
                    MoveCardWithFinger();
                }
            }
            else if (Input.GetMouseButtonUp(0)) // Equivalent to TouchPhase.Ended
            {
                if (isDragging)
                {
                    endTouchPosition = Input.mousePosition;
                    ResetFCPosition();
                    DetectTouchAction();
                    isDragging = false;
                }
            }
        #else

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
            #endif
        }
    }

    private void MoveCardWithFinger()
    {
        //// Assuming currentTouchPosition is in screen coordinates
        //Vector3 screenPoint = new Vector3(currentTouchPosition.x, currentTouchPosition.y, 10.0f); // Adjust the z value as needed
        //Vector3 worldPoint = Camera.main.ScreenToWorldPoint(screenPoint);

        // Set new positions
        Vector3 newPositionFront = new Vector3(currentTouchPosition.x, frontSide.transform.position.y, 0);
        Vector3 newPositionBack = new Vector3(currentTouchPosition.x, backSide.transform.position.y, 0);

        frontSide.transform.localPosition = newPositionFront;
        backSide.transform.localPosition = newPositionBack;
    }

    private void ResetFCPosition()
    {
        frontSide.transform.localPosition = new Vector3(0, 0, 0);
        backSide.transform.localPosition = new Vector3(0, 0, 0);
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

    public void ReplayTheGame()
    {
        InitializeFC(combineName);
        
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
            if (Round == 1)
            {
                Debug.Log(ReviewCategory.Words.Count);
                WordsReview = ReviewCategory.Words.Count;
                WordsLearned = WordsTotal - WordsReview;

                ResultsPage.SetActive(true);
                frontSide.SetActive(false);
                backSide.SetActive(false);
                SignOfKnown.SetActive(false);
                SignOfUnknown.SetActive(false);
                Enable_TouchDetection = false;
                

                totalNum.text = WordsTotal.ToString();
                reviewNum.text = WordsReview.ToString();
                learnedNum.text = WordsLearned.ToString();
                vacabDisplayManager.DisplayWordsForCategory(NPCName, "Review", ReviewContainer.transform);



            }

            if (Round == 0)
            {

                Round++;
                WordsIndex = 0;

            }

            
        }


    }

    private void FlipCard()
    {
        StartCoroutine(FlipCardRoutine(0.4f)); 
    }

    private IEnumerator FlipCardRoutine(float duration)
    {
        
        // touch will not be detected during the rotation
        Enable_TouchDetection = false;
        float time = 0;
        // Determine which side to flip
        
        Quaternion startRotationFront = frontSide.transform.localRotation; 
        Quaternion endRotationFront = startRotationFront*Quaternion.Euler(0, 180, 0); // Rotate 180 degrees around the y-axis
        Quaternion startRotationBack = backSide.transform.localRotation; 
        Quaternion endRotationBack = startRotationBack*Quaternion.Euler(0, 180, 0); // Rotate 180 degrees around the y-axis




        while (time < duration)
        {
            frontSide.transform.localRotation = Quaternion.Lerp(startRotationFront, endRotationFront, time / duration);
            backSide.transform.localRotation = Quaternion.Lerp(startRotationBack, endRotationBack, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        frontSide.transform.localRotation = endRotationFront;
        backSide.transform.localRotation = endRotationBack;

        // Toggle which side is showing
        isShowingBack = !isShowingBack;
        frontSide.SetActive(!isShowingBack);
        backSide.SetActive(isShowingBack);
        Enable_TouchDetection = true;
    }

}
