using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class WaypointController : MonoBehaviour
{
    public float collisionDistance = 35;
    public float spinSpeed = 10;
    public List<Mesh> npcMesh;

    public int npcChoice;
    private bool inPlayerRadius = false;
    private float waypointX;
    private float waypointZ;

    private Transform waypointObject;
    
    // Start is called before the first frame update
    void Start()
    {
        //npcChoice = Random.Range(0, npcMesh.Count);
        waypointX = transform.position.x;
        waypointZ = transform.position.z;
        transform.Rotate(0, Random.Range(0, 180), 0);

        if (npcChoice == 0)
        {
            transform.Find("Waypoint").gameObject.SetActive(false);
        }
        else
        {
            waypointObject = transform.Find("Waypoint");
            transform.Find("UFO").gameObject.SetActive(false);
        }
    }

    //public void Init(float x, float z, int choice)
    //{
    //    waypointX = x;
    //    waypointZ = z;
    //    npcChoice = choice;
    //    if (choice == 0)
    //    {
    //        transform.Find("Waypoint").gameObject.SetActive(false);
    //    }
    //    else
    //    {
    //        waypointObject = transform.Find("Waypoint");
    //        transform.Find("UFO").gameObject.SetActive(false);
    //    }
    //}

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, spinSpeed * Time.deltaTime, 0);

        if (distance() < collisionDistance)
        {
            if (npcChoice > 0) waypointObject.GetComponent<MeshFilter>().mesh = npcMesh[npcChoice];
            transform.Find("Star").gameObject.SetActive(true);
            inPlayerRadius = true;
        }
        else
        {
            //waypointObject.GetComponent<MeshFilter>().mesh = npcMesh[0];
            transform.Find("Star").gameObject.SetActive(false);
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
        float xDiff = waypointX - CoverageManager.Inst.NPCPos.x;
        float zDiff = waypointZ - CoverageManager.Inst.NPCPos.z;

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
            if (npcChoice == 0) SceneManager.LoadScene("argame");
        }
        
    }
    
}
