using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public float scrollSpeed = .5f;
    public GameObject backgroundPrefab;
    private bool repeat = false;
    public float offset = 10f;

    public bool canSpawn = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(GetComponent<RectTransform>().anchoredPosition.x);


        GetComponent<RectTransform>().anchoredPosition += new Vector2(GetComponent<RectTransform>().rect.width, -GetComponent<RectTransform>().rect.height + offset).normalized * scrollSpeed;
        if (canSpawn && GetComponent<RectTransform>().anchoredPosition.x > 0 && !repeat)
        {
            GameObject bg = Instantiate(backgroundPrefab, Vector3.zero, Quaternion.identity, transform.parent);
            bg.GetComponent<RectTransform>().anchoredPosition = new Vector2(GetComponent<RectTransform>().anchoredPosition.x - GetComponent<RectTransform>().rect.width, GetComponent<RectTransform>().anchoredPosition.y);
            bg.transform.SetSiblingIndex(0);
            bg.GetComponent<BackgroundScroller>().canSpawn = false;

            GameObject bgy = Instantiate(backgroundPrefab, Vector3.zero, Quaternion.identity, transform.parent);
            bgy.GetComponent<RectTransform>().anchoredPosition = new Vector2(GetComponent<RectTransform>().anchoredPosition.x, GetComponent<RectTransform>().rect.height + GetComponent<RectTransform>().anchoredPosition.y + offset);
            bgy.transform.SetSiblingIndex(0);
            bgy.GetComponent<BackgroundScroller>().canSpawn = false;

            GameObject bgr = Instantiate(backgroundPrefab, Vector3.zero, Quaternion.identity, transform.parent);
            bgr.GetComponent<RectTransform>().anchoredPosition = new Vector2(GetComponent<RectTransform>().anchoredPosition.x - GetComponent<RectTransform>().rect.width, GetComponent<RectTransform>().rect.height + GetComponent<RectTransform>().anchoredPosition.y + offset);
            bgr.transform.SetSiblingIndex(0);
            bgr.GetComponent<BackgroundScroller>().canSpawn = true;

            repeat = true;
        }

        if (GetComponent<RectTransform>().anchoredPosition.x > GetComponent<RectTransform>().rect.width ||
            GetComponent<RectTransform>().anchoredPosition.y < -GetComponent<RectTransform>().rect.height)
        {
            Destroy(gameObject);
        }
    }

}
