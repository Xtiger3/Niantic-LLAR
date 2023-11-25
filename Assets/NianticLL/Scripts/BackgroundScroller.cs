using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public float scrollSpeed = .5f;
    public GameObject backgroundPrefab;
    private bool repeat = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<RectTransform>().anchoredPosition += Vector2.right * scrollSpeed;
        Debug.Log(GetComponent<RectTransform>().anchoredPosition);
        if(GetComponent<RectTransform>().anchoredPosition.x > 0 && !repeat){
            GameObject bg = Instantiate(backgroundPrefab, Vector3.zero, Quaternion.identity, transform.parent);
            bg.GetComponent<RectTransform>().anchoredPosition = new Vector2(-GetComponent<RectTransform>().rect.width, 0);
            bg.transform.SetSiblingIndex(0);
            repeat = true;
        }
        if(GetComponent<RectTransform>().anchoredPosition.x > GetComponent<RectTransform>().rect.width){
            Destroy(gameObject);
        }
    }
}
