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

    private float originalZ;
    // Start is called before the first frame update
    void Start()
    {
        originalZ = GetComponent<RectTransform>().anchoredPosition3D.z;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(GetComponent<RectTransform>().anchoredPosition.x);


        GetComponent<RectTransform>().anchoredPosition += new Vector2(GetComponent<RectTransform>().rect.width, -GetComponent<RectTransform>().rect.height + offset).normalized * scrollSpeed;
        if (canSpawn && GetComponent<RectTransform>().anchoredPosition.x > 0 && !repeat)
        {
            GameObject bg = Instantiate(backgroundPrefab, Vector3.zero, Quaternion.identity, transform.parent);
            bg.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(GetComponent<RectTransform>().anchoredPosition.x - GetComponent<RectTransform>().rect.width, GetComponent<RectTransform>().anchoredPosition.y, originalZ);
            bg.transform.SetSiblingIndex(0);
            bg.GetComponent<BackgroundScroller>().canSpawn = false;
            //Debug.Log(bg.GetComponent<RectTransform>().anchoredPosition);

            GameObject bgy = Instantiate(backgroundPrefab, Vector3.zero, Quaternion.identity, transform.parent);
            bgy.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(GetComponent<RectTransform>().anchoredPosition.x, GetComponent<RectTransform>().rect.height + GetComponent<RectTransform>().anchoredPosition.y + offset, originalZ);
            bgy.transform.SetSiblingIndex(0);
            bgy.GetComponent<BackgroundScroller>().canSpawn = false;

            GameObject bgr = Instantiate(backgroundPrefab, Vector3.zero, Quaternion.identity, transform.parent);
            bgr.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(GetComponent<RectTransform>().anchoredPosition.x - GetComponent<RectTransform>().rect.width, GetComponent<RectTransform>().rect.height + GetComponent<RectTransform>().anchoredPosition.y + offset, originalZ);
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
