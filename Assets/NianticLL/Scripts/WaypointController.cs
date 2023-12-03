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
    public GameObject star;

    public int npcChoice;
    private bool inPlayerRadius = false;
    //private float waypointX;
    //private float waypointZ;

    private Transform waypointObject;
    private List<string> npcAtWaypoint = new List<string> { "UFO", "Zero", "Maki", "OB" };

    public List<GameObject> prefabs = new List<GameObject>();
    
    // Start is called before the first frame update
    void Start()
    {
        //npcChoice = Random.Range(0, npcMesh.Count);
        //waypointX = transform.position.x;
        //waypointZ = transform.position.z;
        transform.Rotate(0, Random.Range(0, 180), 0);

        if (npcChoice == 0)
        {
            transform.Find("Waypoint").gameObject.SetActive(false);
        }
        else
        {
            waypointObject = transform.Find("Waypoint");
            transform.Find("UFO").gameObject.SetActive(false);

            if (VocabularySet.Instance.ongoingCategory.ContainsKey(npcAtWaypoint[npcChoice]))
            {
                waypointObject.GetComponent<MeshFilter>().mesh = npcMesh[npcChoice];
            }
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

        //Debug.Log("Loc for choice num " + npcChoice + ": " + Distance());
        if (Distance() < collisionDistance)
        {
            if (npcChoice > 0) waypointObject.GetComponent<MeshFilter>().mesh = npcMesh[npcChoice];
            star.SetActive(true);
            inPlayerRadius = true;
        }
        else
        {
            //waypointObject.GetComponent<MeshFilter>().mesh = npcMesh[0];
            star.SetActive(false);
            inPlayerRadius = false;
        }

        

        // Check if the user clicked on the waypoints
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

    private float Distance()
    {
        float xDiff = transform.position.x - CoverageManager.Inst.NPCPos.x;
        float zDiff = transform.position.z - CoverageManager.Inst.NPCPos.z;

        //Debug.Log("playerLoc: " + CoverageManager.Inst.NPCPos.x + ", " + CoverageManager.Inst.NPCPos.z);
        //Debug.Log("sqrtDist: " + Mathf.Sqrt(xDiff * xDiff + zDiff * zDiff));


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
            if (npcChoice == 0)
            {
                this.gameObject.SetActive(false);
                SceneManager.LoadScene("argame");
            }

            else if (npcChoice == 1)
            {
                this.gameObject.SetActive(false);
                SceneManager.LoadScene("intro");
                VocabularySet.Instance.npcPrefab = prefabs[0];
                if (VocabularySet.Instance.ongoingCategory.ContainsKey("Zero"))
                {
                    VocabularySet.Instance.dialogueFile = "zero_quiz";
                }
            }

            else if (npcChoice == 2)
            {
                this.gameObject.SetActive(false);
                SceneManager.LoadScene("intro");
                VocabularySet.Instance.npcPrefab = prefabs[1];
                if (VocabularySet.Instance.ongoingCategory.ContainsKey("Maki"))
                {
                    VocabularySet.Instance.dialogueFile = "maki_quiz";
                }
                else
                {
                    VocabularySet.Instance.dialogueFile = "maki_intro";
                }
            }

            else if (npcChoice == 3)
            {
                this.gameObject.SetActive(false);
                SceneManager.LoadScene("intro");
                VocabularySet.Instance.npcPrefab = prefabs[2];
                if (VocabularySet.Instance.ongoingCategory.ContainsKey("OB"))
                {
                    VocabularySet.Instance.dialogueFile = "ob_quiz";
                }
                else
                {
                    VocabularySet.Instance.dialogueFile = "ob_intro";
                }
            }
            else
            {
                Debug.Log("choice doesnt exist");
            }
        }
        
    }
    
}
