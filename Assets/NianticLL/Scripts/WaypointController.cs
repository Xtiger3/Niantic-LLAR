using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class WaypointController : MonoBehaviour
{
    public float collisionDistance = 35;
    public float spinSpeed = 10;
    public List<Sprite> npcSprites;

    private int npcChoice;
    private bool inPlayerRadius = false;
    private float wayPointX;
    private float wayPointZ;
    
    // Start is called before the first frame update
    void Start()
    {
        npcChoice = Random.Range(1, npcSprites.Count);
        wayPointX = transform.position.x;
        wayPointZ = transform.position.z;

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, spinSpeed * Time.deltaTime, 0);


        //Debug.Log(Mathf.Sqrt(x * x + z * z));
        if (distance() < collisionDistance)
        {
            GetComponent<SpriteRenderer>().sprite = npcSprites[npcChoice];
            transform.Find("Wifi").gameObject.SetActive(true);
            inPlayerRadius = true;
            //Debug.Log("Collided w a waypoint");
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = npcSprites[0];
            transform.Find("Wifi").gameObject.SetActive(false);
            inPlayerRadius = false;
        }



        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (!EventSystem.current.IsPointerOverGameObject() && hit.transform == transform)
                {
                    TransitionToAR();
                }
            }
        }

    }

    private float distance()
    {
        float xDiff = wayPointX - CoverageManager.Inst.NPCPos.y;
        float zDiff = wayPointZ - CoverageManager.Inst.NPCPos.y;

        return Mathf.Sqrt(xDiff * xDiff + zDiff * zDiff);

    }

    private void OnMouseDown()
    {
        Debug.Log("Clicked");
        TransitionToAR();
    }

    void TransitionToAR()
    {
        if (inPlayerRadius)
        {
            //MapManager.Inst.NPCName = "CHANGE LATER";
            //MapManager.Inst.NPCPos = transform.position;
            SceneManager.LoadScene("argame");
        }
        
    }
    
}
