using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandCategory : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Expand(GameObject gameObject) {
        if (gameObject.active) {
            gameObject.SetActive(false);
        } else {
            if (gameObject.transform.childCount > 1)
            {
                gameObject.SetActive(true);
            }
            // else
            // {
            //     this.transform.Find("Lock").gameObject.SetActive(true);
            // }
        }
    }
}
