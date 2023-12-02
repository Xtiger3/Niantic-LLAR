using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TC : MonoBehaviour
{
    private float spinDuration = 2f;

    public float startScale;
    public float endScale;

    public Vector3 startPos;
    public Vector3 endPos;

    public float[] posY;
    private Vector3[] initialPos;
    public Transform[] tc_parts;

    public GameObject menu;

    private bool open = false;

    private bool coroutine_running = false;

    private void Start()
    {
        initialPos = new Vector3[tc_parts.Length];
    }

    private void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (!coroutine_running && hit.transform == transform)
                {
                    if(!open)
                        StartCoroutine(SpinAndScale());
                }
            }
        }
    }

    public void CloseTCMenu()
    {
        StartCoroutine(SpinAndScaleDown());
    }

    IEnumerator SpinAndScale()
    {
        coroutine_running = true;
        float t = 0.0f;

        float start = startScale;
        float end = endScale;

        float startRotation = transform.localEulerAngles.y;
        float endRotation = startRotation + (360.0f);

        while (t < spinDuration)
        {
            t += Time.deltaTime;
            float scale = Mathf.Lerp(start, end, t / spinDuration);
            transform.localScale = Vector3.one * scale;

            float yRotation = Mathf.Lerp(startRotation, endRotation, t / spinDuration) % 360.0f;
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, yRotation, transform.localEulerAngles.z);

            transform.localPosition = Vector3.Lerp(startPos, endPos, t / spinDuration);

            yield return null;
        }


        for (int i = 0; i < tc_parts.Length; ++i)
        {
            initialPos[i] = tc_parts[i].localPosition;
        }

        yield return Elongate();

        menu.SetActive(true);
        open = true;
        coroutine_running = false;
    }

    IEnumerator Elongate()
    {
        float t = 0.0f;
        while (t < spinDuration)
        {
            for (int i = 0; i < tc_parts.Length; ++i)
            {
                t += Time.deltaTime;
                Transform part = tc_parts[i];
                part.localPosition = Vector3.Lerp(initialPos[i], new Vector3(part.localPosition.x, posY[i], part.localPosition.z), t / spinDuration);
            }

            yield return null;
        }
    }

    IEnumerator SpinAndScaleDown()
    {
        coroutine_running = true;
        menu.SetActive(false);

        yield return Delongate();

        float t = 0.0f;

        float start = startScale;
        float end = endScale;

        float startRotation = transform.localEulerAngles.y;
        float endRotation = startRotation + (360.0f);

        while (t < spinDuration)
        {
            t += Time.deltaTime;
            float scale = Mathf.Lerp(end, start, t / spinDuration);
            transform.localScale = Vector3.one * scale;

            float yRotation = Mathf.Lerp(startRotation, endRotation, t / spinDuration) % 360.0f;
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, yRotation, transform.localEulerAngles.z);

            transform.localPosition = Vector3.Lerp(endPos, startPos, t / spinDuration);

            yield return null;
        }


        for (int i = 0; i < tc_parts.Length; ++i)
        {
            initialPos[i] = tc_parts[i].localPosition;
        }


        open = false;
        coroutine_running = false;
    }

    IEnumerator Delongate()
    {
        float t = 0.0f;
        while (t < spinDuration)
        {
            for (int i = 0; i < tc_parts.Length; ++i)
            {
                t += Time.deltaTime;
                Transform part = tc_parts[i];
                part.localPosition = Vector3.Lerp(new Vector3(part.localPosition.x, posY[i], part.localPosition.z), initialPos[i], t / spinDuration);
            }

            yield return null;
        }
    }
}
