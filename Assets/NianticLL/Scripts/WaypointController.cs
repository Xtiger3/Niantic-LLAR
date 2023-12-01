using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaypointController : MonoBehaviour
{
    public float collisionDistance = 35;
    public List<Sprite> npcSprites;

    private int npcChoice;
    private bool inPlayerRadius = false;
    
    // Start is called before the first frame update
    void Start()
    {
        npcChoice = Random.Range(1, npcSprites.Count);
    }

    // Update is called once per frame
    void Update()
    {
        float x = transform.position.x;
        float z = transform.position.z;

        //Debug.Log(Mathf.Sqrt(x * x + z * z));
        if (Mathf.Sqrt(x*x+z*z) < collisionDistance)
        {
            GetComponent<SpriteRenderer>().sprite = npcSprites[npcChoice];
            inPlayerRadius = true;
            //Debug.Log("Collided w a waypoint");
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = npcSprites[0];
            inPlayerRadius = false;
        }

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
            MapManager.Inst.NPCName = "CHANGE LATER";
            MapManager.Inst.NPCPos = transform.position;
            SceneManager.LoadScene("xy");
        }
        
    }
    
}
