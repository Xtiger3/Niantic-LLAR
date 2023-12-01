using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    public GameObject prefab;
    public float throwSpeed;

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
#else
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
#endif
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            GameObject bullet = Instantiate(prefab, mouseRay.origin, Quaternion.identity);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.velocity = mouseRay.direction * throwSpeed;
                //bullet.transform.rotation = Quaternion.LookRotation(rb.velocity);
                //bullet.transform.Rotate(0f, 50 * Time.deltaTime, 0f, Space.Self);
            }
        }
    }
}
