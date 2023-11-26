using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UFOController : MonoBehaviour
{
    public float explosionForce = 300f;
    public float radius = 2f;

    public float bounceSpeed = 5f;
    public float bounceHeight = 0.01f;

    private float startYPos;

    public VocabularySet.Word word;

    public float spinDuration = 3f;

    public GameObject deathEffect;

    private bool bounce = true;

    public TextMeshProUGUI wordText;

    private void Start()
    {
        wordText.text = word.Translation.Split(' ')[0];
        startYPos = transform.position.y;
        Debug.Log("uf0: " + word.Original);
    }

    private void Update()
    {
        if(bounce) transform.position = new Vector3(transform.position.x, startYPos + (Mathf.Sin(Time.time * bounceSpeed) * bounceHeight), transform.position.z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "bullet")
        {
            StartCoroutine(Spin360());

            for(int i = 0; i < 5; ++i)
            {
                for(int j = 0; j < 4; ++j)
                {
                    Destroy(collision.gameObject);
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.transform.localScale = transform.localScale / 5;
                    Vector3 firstCube = transform.position - transform.localScale / 2 + cube.transform.localScale / 2;
                    cube.transform.position = firstCube + Vector3.Scale(new Vector3(i,j,0), cube.transform.localScale);
                    Rigidbody rb = cube.AddComponent<Rigidbody>();
                    rb.AddExplosionForce(explosionForce, transform.position, radius);
                    cube.GetComponent<Renderer>().material = collision.gameObject.GetComponentInChildren<Renderer>().material;
                    Destroy(cube, 3f);
                }
            }

            if (word.Original == VocabGameController.Instance.currentWord.Original)
            {
                VocabGameController.Instance.AddCountCorrect();
                VocabGameController.Instance.NextRound();
                Debug.Log("correct");
            }
            else
            {
                VocabGameController.Instance.AddCountIncorrect();
                Instantiate(deathEffect, transform.position, Quaternion.identity);
                GetComponent<Rigidbody>().useGravity = true;
                GetComponent<Rigidbody>().constraints = ~RigidbodyConstraints.FreezePositionY;
                transform.localEulerAngles += new Vector3(0, 0, 180);
                bounce = false;
                Destroy(gameObject, 3f);
            }
        }   
    }

    IEnumerator Spin360()
    {
        float startRotation = transform.eulerAngles.y;
        float endRotation = startRotation + 360.0f;
        float t = 0.0f;
        while (t < spinDuration)
        {
            t += Time.deltaTime;
            float yRotation = Mathf.Lerp(startRotation, endRotation, t / spinDuration) % 360.0f;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, yRotation, transform.eulerAngles.z);
            yield return null;
        }
    }
}
