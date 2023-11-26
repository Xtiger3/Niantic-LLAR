using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class VocabGameController : MonoBehaviour
{
    public static VocabGameController Instance { get; private set; }

    public List<VocabularySet.Word> gameWords = new List<VocabularySet.Word>(); // 5 words
    public VocabularySet.Word currentWord;

    public GameObject ufoPrefab;

    private float timer = 60f;
    public float timeLimit = 60f;
    public TextMeshProUGUI timerText;

    private int countCorrect = 0;
    private int countIncorrect = 0;

    public Material dissolveMaterial;

    public GameObject scorePanel;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI correctCountText;
    public TextMeshProUGUI incorrectCountText;

    public GameObject gameUI;
    public TextMeshProUGUI currWordText;

    //public TextMeshProUGUI categoryText;
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI gameScoreText;
    public TextMeshProUGUI instructionsText;

    public GameObject[] stars;

    public float spinDuration = 0.8f;

    private bool timerRunning = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Update()
    {
        if (timer > 0 && timerRunning)
        {
            timerText.text = Mathf.FloorToInt(timer).ToString();
            timer -= Time.deltaTime; 
        }
        if (timer <= 0)
        {
            ShowScore();
        }
    }

    private void Start()
    {
        StartGame();

    }
    public void StartGame()
    {
        StartCoroutine(Init());
    }

    IEnumerator Init()
    {
        //yield return new WaitForSeconds(1f);
        currWordText.transform.parent.gameObject.SetActive(false);
        countdownText.gameObject.SetActive(true);
        countdownText.text = "3";
        yield return new WaitForSeconds(1f);
        countdownText.text = "2";
        yield return new WaitForSeconds(1f);
        countdownText.text = "1";
        yield return new WaitForSeconds(1f);
        countdownText.text = "START";
        yield return new WaitForSeconds(1f);
        countdownText.gameObject.SetActive(false);
        instructionsText.gameObject.SetActive(false);

        timer = timeLimit;
        timerRunning = true;
        currWordText.transform.parent.gameObject.SetActive(true);

        gameWords = VocabularySet.Instance.categories[Random.Range(0, VocabularySet.Instance.categories.Count)].Words;
        //categoryText.text = VocabularySet.Instance.categories[Random.Range(0, VocabularySet.Instance.categories.Count)].Name;
        while (gameWords.Count == 0)
        {
            gameWords = VocabularySet.Instance.categories[Random.Range(0, VocabularySet.Instance.categories.Count)].Words;
        }

        // only one category per game i think 

        InstantiateUFOs();
    }

    public void InstantiateUFOs()
    {
        List<int> allIndices = new List<int>();
        int totalIndices = gameWords.Count;

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

        currentWord = gameWords[allIndices[Random.Range(0, 5)]];
        currWordText.text = currentWord.Original;

        for (int i = 0; i < 5; ++i)
        {
            Vector3 cameraPosition = Camera.main.transform.position;
            float randomAngle = Random.Range(0f, 2f * Mathf.PI);

            float randomRadius = Random.Range(5f, 10f);
            float x = cameraPosition.x + randomRadius * Mathf.Cos(randomAngle);
            float z = cameraPosition.z + randomRadius * Mathf.Sin(randomAngle);
            float y = cameraPosition.y;
            Vector3 position = new Vector3(x, y, z); // random position in 360

            GameObject ufo = Instantiate(ufoPrefab, position, Quaternion.identity);
            ufo.transform.LookAt(Camera.main.transform);
            ufo.GetComponent<UFOController>().word = gameWords[allIndices[i]];
        }
    }

    public void AddCountCorrect()
    {
        countCorrect++;
        int score = (countCorrect * 10 - countIncorrect * 5);
        gameScoreText.text = score.ToString();
    }

    public void AddCountIncorrect()
    {
        countIncorrect++;
        int score = (countCorrect * 10 - countIncorrect * 5);
        gameScoreText.text = score.ToString();
    }

    public void DestroyUFOs()
    {
        foreach(GameObject ufo in GameObject.FindGameObjectsWithTag("ufo"))
        {
            foreach(Renderer r in ufo.GetComponentsInChildren<Renderer>())
            {
                r.material = dissolveMaterial;
                StartCoroutine(Dissolve(r));
            }
            Destroy(ufo, 3f);
        }
    }

    IEnumerator Dissolve(Renderer r)
    {
        float t = 0;
        r.material.SetFloat("Value", 0);
        while (r.material.GetFloat("Value") < 1)
        {
            r.material.SetFloat("Value", Mathf.Lerp(0, 1, t));
            t += Time.deltaTime;
            yield return null;
        }
    }

    public void NextRound()
    {
        DestroyUFOs();
        Invoke("InstantiateUFOs", 3f);
    }

    public void ReplayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ShowScore()
    {
        DestroyUFOs();
        gameUI.gameObject.SetActive(false);
        scorePanel.SetActive(true);
        int score = (countCorrect * 10 - countIncorrect * 5);
        scoreText.text = score.ToString();
        correctCountText.text = "CORRECT: " + countCorrect.ToString();
        incorrectCountText.text = "INCORRECT: " + countIncorrect.ToString();

        //float percent = 0;
        //if(score != 0) percent = score / ((countCorrect + countIncorrect) * 10);

        if (score == 0)
        {
            //no star
        }
        else if (score < 20)
        {
            // one star
            StartCoroutine(Spin360(0));
        }
        else if (score < 50)
        {
            // two star
            StartCoroutine(TwoStar());
        }
        else
        {
            // three star
            StartCoroutine(ThreeStar());
        }
    }

    IEnumerator TwoStar()
    {
        yield return Spin360(0);
        yield return Spin360(1);
    }

    IEnumerator ThreeStar()
    {
        yield return Spin360(0);
        yield return Spin360(1);
        yield return Spin360(2);
    }

    IEnumerator Spin360(int index)
    {
        stars[index].SetActive(true);
        float startRotation = stars[index].transform.eulerAngles.y;
        float endRotation = startRotation + (360.0f);
        float t = 0.0f;
        while (t < spinDuration)
        {
            t += Time.deltaTime;
            float yRotation = Mathf.Lerp(startRotation, endRotation, t / spinDuration) % 360.0f;
            stars[index].transform.eulerAngles = new Vector3(stars[index].transform.eulerAngles.x, yRotation, stars[index].transform.eulerAngles.z);
            yield return null;
        }
    }
}
